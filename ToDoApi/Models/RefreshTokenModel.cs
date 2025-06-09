using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApi.Models;

public class RefreshTokenModel
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    
    [Column("expires_on_utc")]
    public DateTime ExpiresOnUtc { get; set; }
    public Guid UserId { get; set; }
    
    public UserModel? User { get; set; }
}