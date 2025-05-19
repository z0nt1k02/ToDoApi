using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApi.Models;

namespace ToDoApi.Contracts;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(r => r.Token).HasMaxLength(200);
        
        builder.HasIndex(x => x.Token).IsUnique();
        
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
    }
}