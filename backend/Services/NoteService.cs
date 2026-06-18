using System.Net;
using NoteAppApi.Common;
using NoteAppApi.Common.Exceptions;
using NoteAppApi.DTOs.Common;
using NoteAppApi.DTOs.Note;
using NoteAppApi.Entities;
using NoteAppApi.Mappings;
using NoteAppApi.Repositories;

namespace NoteAppApi.Services;

public class NoteService
{
    private readonly NoteRepository _noteRepository;
    private readonly AuthData _authData;

    public NoteService(NoteRepository noteRepository, AuthData authData)
    {
        _noteRepository = noteRepository;
        _authData = authData;
    }
    
    public async Task<PaginatedResponseDTO<NoteResponseDTO>> GetAllAsync(GetAllNoteQueryDTO query)
    {
        var (items, totalCount) = await _noteRepository.GetAllByUserIdAsync(_authData.UserId, query);
        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new PaginatedResponseDTO<NoteResponseDTO>(
            items.Select(NoteMapper.ToResponse).ToList(),
            query.Page,
            query.PageSize,
            totalCount,
            totalPages
        );
    }

    public async Task<NoteResponseDTO> GetByIdAsync(Guid id)
    {
        var note = await _noteRepository.GetByIdAsync(id);

        if (note is null || note.UserId != _authData.UserId)
            throw new AppException(ErrorCodes.NoteNotFound, "Note not found", HttpStatusCode.NotFound);

        return NoteMapper.ToResponse(note);
    }

    public async Task<NoteResponseDTO> CreateAsync(CreateNoteRequestDTO request)
    {
        var note = await _noteRepository.CreateAsync(new Note
        {
            Title = request.Title,
            Content = request.Content,
            UserId = _authData.UserId,
        });

        return NoteMapper.ToResponse(note);
    }

    public async Task<NoteResponseDTO> UpdateAsync(Guid id, UpdateNoteRequestDTO request)
    {
        var existing = await _noteRepository.GetByIdAsync(id);

        if (existing is null || existing.UserId != _authData.UserId)
            throw new AppException(ErrorCodes.NoteNotFound, "Note not found", HttpStatusCode.NotFound);

        await _noteRepository.UpdateAsync(id, request.Title, request.Content);

        var updated = await _noteRepository.GetByIdAsync(id);

        return NoteMapper.ToResponse(updated!);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _noteRepository.GetByIdAsync(id);

        if (existing is null || existing.UserId != _authData.UserId)
            throw new AppException(ErrorCodes.NoteNotFound, "Note not found", HttpStatusCode.NotFound);

        await _noteRepository.SoftDeleteAsync(id);
    }
}
