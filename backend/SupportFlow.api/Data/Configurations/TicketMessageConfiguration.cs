using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class TicketMessageConfiguration
    : IEntityTypeConfiguration<TicketMessage>
{
    public void Configure(EntityTypeBuilder<TicketMessage> builder)
    {
        builder.ToTable("TicketMessages", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_TicketMessages_Message_Length",
                "LENGTH(TRIM(\"Message\")) BETWEEN 1 AND 10000");
        });

        builder.HasKey(message => message.Id)
            .HasName("PK_TicketMessages");

        builder.Property(message => message.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(message => message.Message)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(message => message.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(message => message.Ticket)
            .WithMany(ticket => ticket.Messages)
            .HasForeignKey(message => message.TicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_TicketMessages_Tickets_TicketId");

        builder.HasOne(message => message.Sender)
            .WithMany(user => user.SentMessages)
            .HasForeignKey(message => message.SenderId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_TicketMessages_Users_SenderId");

        builder.HasIndex(message => message.TicketId)
            .HasDatabaseName("IX_TicketMessages_TicketId");

        builder.HasIndex(message => message.SenderId)
            .HasDatabaseName("IX_TicketMessages_SenderId");

        builder.HasIndex(message => new
            {
                message.TicketId,
                message.CreatedAt
            })
            .HasDatabaseName(
                "IX_TicketMessages_TicketId_CreatedAt");
    }
}