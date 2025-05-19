using ToDoApi.Models;

namespace ToDoApi.Contracts;

public interface INoteRepository
{
    public Task<List<NoteModel>> GetAllNotes();
    public Task<NoteModel?> GetNote(int noteId);
    public Task AddNote(NoteModel note);
    public Task UpdateNote(NoteModel note);
    public Task DeleteNote(NoteModel note);    
}