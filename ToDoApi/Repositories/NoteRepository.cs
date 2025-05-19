using Microsoft.EntityFrameworkCore;
using ToDoApi.Contracts;
using ToDoApi.Models;

namespace ToDoApi.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly ToDoDbContext _context;

    public NoteRepository(ToDoDbContext context)
    {
        _context = context;
    }
    public async Task<List<NoteModel>> GetAllNotes()
    {
        var notes = await _context.Notes.ToListAsync();
        return notes;
    }

    public async Task<NoteModel?> GetNote(int noteId)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n=>n.Id == noteId);
        return note;
    }

    public async Task AddNote(NoteModel note)
    {
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNote(NoteModel note)
    {
        _context.Notes.Update(note);
         await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(NoteModel note)
    {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
}