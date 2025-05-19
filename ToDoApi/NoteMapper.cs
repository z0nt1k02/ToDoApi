using ToDoApi.Dto;
using ToDoApi.Models;

namespace ToDoApi;

public static class NoteMapper
{
    public static CreateUpdateNoteDto ToDto(this NoteModel noteModel)
    {
        return new CreateUpdateNoteDto(noteModel.Id,noteModel.Name,noteModel.Content);
    }
}