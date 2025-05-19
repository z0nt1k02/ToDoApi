using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApi.Models;

public class NoteModel
{
    [Key] // Указывает, что это первичный ключ
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }
    public DateTime CreatedDate { get; set; }
}