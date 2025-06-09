using Microsoft.AspNetCore.Mvc;
using ToDoApi.Contracts;
using ToDoApi.Dto;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteRepository _noteRepository;

    public NoteController(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        var notes = await _noteRepository.GetAllNotes();
        return Ok(notes.Select(n => n.ToDto()).ToList());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNote(int id)
    {
        var note = await _noteRepository.GetNote(id);
        /*if (note.UserId != Guid.Parse(HttpContext.User.Identity.Name))
        {
            return Unauthorized();
        }*/
        return Ok(note); 
    }

    [HttpPost]
    public async Task<IActionResult> AddNote(CreateUpdateNoteDto dto)
    {
        var newNote = new NoteModel
        {
            CreatedDate = DateTime.Now,
            Name = dto.name,
            Content = dto.content,
            UserId = null,
            User = null
        };
        await _noteRepository.AddNote(newNote);
        return CreatedAtAction(nameof(GetNote), new { id = newNote.Id }, newNote.ToDto());
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id,NoteDto noteDto)
    {
        var note = await _noteRepository.GetNote(id);
        if (note == null)
        {
            return NotFound();
        }
        /*if (note.UserId != Guid.Parse(HttpContext.User.Identity.Name))
        {
            return Unauthorized();
        }*/
        note.Content = noteDto.content;
        note.Name = noteDto.name;
        await _noteRepository.UpdateNote(note);
        return Ok("Note updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var note = await _noteRepository.GetNote(id);
        if(note is null)
            return NotFound();
        await _noteRepository.DeleteNote(note);
        return Ok("Note deleted");

    }

    [HttpGet]
    [Route("test")]
    public IActionResult Test()
    {
        throw new Exception("Ошибка сервера");
    }
}