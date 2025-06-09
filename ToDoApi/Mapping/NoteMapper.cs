using ToDoApi.Dto;
using ToDoApi.Models;

namespace ToDoApi;

public static class NoteMapper
{
    public static NoteDto ToDto(this NoteModel noteModel)
    {
        return new NoteDto(noteModel.Id,noteModel.Name,noteModel.Content);
    }
}