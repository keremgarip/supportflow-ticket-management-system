using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_Users_Role",
                "\"Role\" IN ('Customer', 'SupportAgent', 'Admin')");

            tableBuilder.HasCheckConstraint(
                "CK_Users_FullName_NotEmpty",
                "LENGTH(TRIM(\"FullName\")) >= 2");

            tableBuilder.HasCheckConstraint(
                "CK_Users_Email_NotEmpty",
                "LENGTH(TRIM(\"Email\")) > 0");
        });

        builder.HasKey(user => user.Id)
            .HasName("PK_Users");

        builder.Property(user => user.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(user => user.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.Role)
            .HasMaxLength(30)
            .HasDefaultValue("Customer")
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(user => user.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique()
            .HasDatabaseName("UQ_Users_Email");
    }
}