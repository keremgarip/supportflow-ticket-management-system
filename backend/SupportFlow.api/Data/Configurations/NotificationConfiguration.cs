using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_Notifications_Title_Length",
                "LENGTH(TRIM(\"Title\")) BETWEEN 1 AND 200");

            tableBuilder.HasCheckConstraint(
                "CK_Notifications_Message_Length",
                "LENGTH(TRIM(\"Message\")) BETWEEN 1 AND 1000");
        });

        builder.HasKey(notification => notification.Id)
            .HasName("PK_Notifications");

        builder.Property(notification => notification.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(notification => notification.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(notification => notification.Message)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(notification => notification.IsRead)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(notification => notification.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(notification => notification.User)
            .WithMany(user => user.Notifications)
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_Notifications_Users_UserId");

        builder.HasOne(notification => notification.Ticket)
            .WithMany(ticket => ticket.Notifications)
            .HasForeignKey(notification => notification.TicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_Notifications_Tickets_TicketId");

        builder.HasIndex(notification => notification.UserId)
            .HasDatabaseName("IX_Notifications_UserId");

        builder.HasIndex(notification => notification.TicketId)
            .HasDatabaseName("IX_Notifications_TicketId");

        builder.HasIndex(notification => new
            {
                notification.UserId,
                notification.IsRead
            })
            .HasDatabaseName(
                "IX_Notifications_UserId_IsRead");

        builder.HasIndex(notification => new
            {
                notification.UserId,
                notification.CreatedAt
            })
            .HasDatabaseName(
                "IX_Notifications_UserId_CreatedAt");
    }
}