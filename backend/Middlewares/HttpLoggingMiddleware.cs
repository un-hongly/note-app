using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NoteAppApi.Middlewares;

public class HttpLoggingMiddleware
{
    private static readonly HashSet<string> DefaultRedactFields =
        ["password", "accessToken", "refreshToken", "token"];

    private readonly RequestDelegate _next;
    private readonly ILogger<HttpLoggingMiddleware> _logger;
    private readonly string[] _redactFields;

    public HttpLoggingMiddleware(
        RequestDelegate next,
        ILogger<HttpLoggingMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;

        var configured = configuration.GetSection("Logging:RedactFields").Get<string[]>();
        _redactFields = configured is { Length: > 0 }
            ? [.. DefaultRedactFields, .. configured]
            : [.. DefaultRedactFields];
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var timestamp = DateTime.UtcNow;

        context.Request.EnableBuffering();

        var requestBody = await ReadBodyAsync(context.Request.Body);
        context.Request.Body.Position = 0;

        var originalResponseBody = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            responseBody.Position = 0;
            var responseBodyText = await ReadBodyAsync(responseBody);
            responseBody.Position = 0;
            await responseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;

            _logger.LogInformation(
                "HTTP {method} {path} {statusCode} {duration}ms {timestamp} | Request: {requestBody} | Response: {responseBody}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                timestamp,
                RedactSensitiveData(requestBody),
                RedactSensitiveData(responseBodyText));
        }
    }

    private string RedactSensitiveData(string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return body;

        try
        {
            var node = JsonNode.Parse(body);
            if (node == null) return body;

            foreach (var field in _redactFields)
            {
                if (field.Contains('.'))
                    RedactByPath(node, field.Split('.'), 0);
                else
                    RedactByKey(node, field);
            }

            return node.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
        }
        catch
        {
            return body;
        }
    }

    private static void RedactByPath(JsonNode node, string[] segments, int index)
    {
        if (node is not JsonObject obj) return;
        if (!obj.TryGetPropertyValue(segments[index], out var child)) return;

        if (index == segments.Length - 1)
            obj[segments[index]] = "***";
        else if (child != null)
            RedactByPath(child, segments, index + 1);
    }

    private static void RedactByKey(JsonNode node, string key)
    {
        switch (node)
        {
            case JsonObject obj:
                var keys = obj.ToList();
                foreach (var (k, v) in keys)
                {
                    if (k == key)
                        obj[k] = "***";
                    else if (v != null)
                        RedactByKey(v, key);
                }
                break;
            case JsonArray arr:
                foreach (var item in arr)
                {
                    if (item != null) RedactByKey(item, key);
                }
                break;
        }
    }

    private static async Task<string> ReadBodyAsync(Stream stream)
    {
        using var reader = new StreamReader(stream, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }
}
