using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class TicketStatusHistoryConfiguration
    : IEntityTypeConfiguration<TicketStatusHistory>
{
    public void Configure(
        EntityTypeBuilder<TicketStatusHistory> builder)
    {
        builder.ToTable("TicketStatusHistories", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_TicketStatusHistories_OldStatus",
                "\"OldStatus\" IS NULL OR \"OldStatus\" IN " +
                "('Open', 'In Progress', 'Waiting for Customer', " +
                "'Resolved', 'Closed')");

            tableBuilder.HasCheckConstraint(
                "CK_TicketStatusHistories_NewStatus",
                "\"NewStatus\" IN " +
                "('Open', 'In Progress', 'Waiting for Customer', " +
                "'Resolved', 'Closed')");

            tableBuilder.HasCheckConstraint(
                "CK_TicketStatusHistories_StatusChanged",
                "\"OldStatus\" IS NULL OR " +
                "\"OldStatus\" <> \"NewStatus\"");
        });

        builder.HasKey(history => history.Id)
            .HasName("PK_TicketStatusHistories");

        builder.Property(history => history.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(history => history.OldStatus)
            .HasMaxLength(30);

        builder.Property(history => history.NewStatus)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(history => history.ChangedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(history => history.Ticket)
            .WithMany(ticket => ticket.StatusHistories)
            .HasForeignKey(history => history.TicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_TicketStatusHistories_Tickets_TicketId");

        builder.HasOne(history => history.ChangedByUser)
            .WithMany(user => user.StatusChanges)
            .HasForeignKey(history => history.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_TicketStatusHistories_Users_ChangedByUserId");

        builder.HasIndex(history => history.TicketId)
            .HasDatabaseName(
                "IX_TicketStatusHistories_TicketId");

        builder.HasIndex(history => history.ChangedByUserId)
            .HasDatabaseName(
                "IX_TicketStatusHistories_ChangedByUserId");

        builder.HasIndex(history => new
            {
                history.TicketId,
                history.ChangedAt
            })
            .HasDatabaseName(
                "IX_TicketStatusHistories_TicketId_ChangedAt");
    }
}