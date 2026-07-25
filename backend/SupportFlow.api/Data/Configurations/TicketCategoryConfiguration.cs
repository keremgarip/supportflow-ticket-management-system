using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class TicketCategoryConfiguration
    : IEntityTypeConfiguration<TicketCategory>
{
    public void Configure(EntityTypeBuilder<TicketCategory> builder)
    {
        builder.ToTable("TicketCategories", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_TicketCategories_Name_NotEmpty",
                "LENGTH(TRIM(\"Name\")) >= 2");
        });

        builder.HasKey(category => category.Id)
            .HasName("PK_TicketCategories");

        builder.Property(category => category.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(category => category.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(category => category.Description)
            .HasMaxLength(500);

        builder.Property(category => category.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(category => category.Name)
            .IsUnique()
            .HasDatabaseName("UQ_TicketCategories_Name");
    }
}