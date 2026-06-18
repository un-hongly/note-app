using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteAppApi.DTOs;
using NoteAppApi.DTOs.Note;
using NoteAppApi.Services;

namespace NoteAppApi.Controllers;

[Route("notes")]
[ApiController]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly NoteService _noteService;

    public NoteController(NoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery] GetAllNoteQueryDTO query)
    {
        query.PageSize = Math.Clamp(query.PageSize, 1, 1000);
        if (query.Page < 1) query.Page = 1;

        var result = await _noteService.GetAllAsync(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _noteService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _noteService.CreateAsync(request);
        
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNoteRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _noteService.UpdateAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _noteService.DeleteAsync(id);
        return Ok();
    }
}
