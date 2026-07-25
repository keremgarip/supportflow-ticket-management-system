using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_Tickets_Status",
                "\"Status\" IN " +
                "('Open', 'In Progress', 'Waiting for Customer', " +
                "'Resolved', 'Closed')");

            tableBuilder.HasCheckConstraint(
                "CK_Tickets_Priority",
                "\"Priority\" IN ('Low', 'Medium', 'High', 'Critical')");

            tableBuilder.HasCheckConstraint(
                "CK_Tickets_Title_Length",
                "LENGTH(TRIM(\"Title\")) BETWEEN 5 AND 200");

            tableBuilder.HasCheckConstraint(
                "CK_Tickets_Description_Length",
                "LENGTH(TRIM(\"Description\")) >= 10");

            tableBuilder.HasCheckConstraint(
                "CK_Tickets_UpdatedAt",
                "\"UpdatedAt\" >= \"CreatedAt\"");

            tableBuilder.HasCheckConstraint(
                "CK_Tickets_ClosedAt",
                "\"ClosedAt\" IS NULL OR \"ClosedAt\" >= \"CreatedAt\"");
        });

        builder.HasKey(ticket => ticket.Id)
            .HasName("PK_Tickets");

        builder.Property(ticket => ticket.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(ticket => ticket.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ticket => ticket.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(ticket => ticket.Status)
            .HasMaxLength(30)
            .HasDefaultValue("Open")
            .IsRequired();

        builder.Property(ticket => ticket.Priority)
            .HasMaxLength(20)
            .HasDefaultValue("Medium")
            .IsRequired();

        builder.Property(ticket => ticket.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(ticket => ticket.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(ticket => ticket.ClosedAt)
            .HasColumnType("timestamp with time zone");

        builder.HasOne(ticket => ticket.Category)
            .WithMany(category => category.Tickets)
            .HasForeignKey(ticket => ticket.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_Tickets_TicketCategories_CategoryId");

        builder.HasOne(ticket => ticket.Customer)
            .WithMany(user => user.CreatedTickets)
            .HasForeignKey(ticket => ticket.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_Tickets_Users_CustomerId");

        builder.HasOne(ticket => ticket.AssignedAgent)
            .WithMany(user => user.AssignedTickets)
            .HasForeignKey(ticket => ticket.AssignedAgentId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName(
                "FK_Tickets_Users_AssignedAgentId");

        builder.HasIndex(ticket => ticket.CategoryId)
            .HasDatabaseName("IX_Tickets_CategoryId");

        builder.HasIndex(ticket => ticket.CustomerId)
            .HasDatabaseName("IX_Tickets_CustomerId");

        builder.HasIndex(ticket => ticket.AssignedAgentId)
            .HasDatabaseName("IX_Tickets_AssignedAgentId");

        builder.HasIndex(ticket => ticket.Status)
            .HasDatabaseName("IX_Tickets_Status");

        builder.HasIndex(ticket => ticket.Priority)
            .HasDatabaseName("IX_Tickets_Priority");

        builder.HasIndex(ticket => ticket.CreatedAt)
            .HasDatabaseName("IX_Tickets_CreatedAt");
    }
}