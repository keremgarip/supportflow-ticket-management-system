using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data.Configurations;

public class TicketAttachmentConfiguration
    : IEntityTypeConfiguration<TicketAttachment>
{
    public void Configure(
        EntityTypeBuilder<TicketAttachment> builder)
    {
        builder.ToTable("TicketAttachments", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_TicketAttachments_FileName_NotEmpty",
                "LENGTH(TRIM(\"FileName\")) BETWEEN 1 AND 255");

            tableBuilder.HasCheckConstraint(
                "CK_TicketAttachments_FilePath_NotEmpty",
                "LENGTH(TRIM(\"FilePath\")) BETWEEN 1 AND 1000");

            tableBuilder.HasCheckConstraint(
                "CK_TicketAttachments_ContentType_NotEmpty",
                "LENGTH(TRIM(\"ContentType\")) BETWEEN 1 AND 150");
        });

        builder.HasKey(attachment => attachment.Id)
            .HasName("PK_TicketAttachments");

        builder.Property(attachment => attachment.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(attachment => attachment.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(attachment => attachment.FilePath)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(attachment => attachment.ContentType)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(attachment => attachment.UploadedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(attachment => attachment.Ticket)
            .WithMany(ticket => ticket.Attachments)
            .HasForeignKey(attachment => attachment.TicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(
                "FK_TicketAttachments_Tickets_TicketId");

        builder.HasOne(attachment => attachment.UploadedByUser)
            .WithMany(user => user.UploadedAttachments)
            .HasForeignKey(attachment => attachment.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(
                "FK_TicketAttachments_Users_UploadedByUserId");

        builder.HasIndex(attachment => attachment.TicketId)
            .HasDatabaseName(
                "IX_TicketAttachments_TicketId");

        builder.HasIndex(attachment => attachment.UploadedByUserId)
            .HasDatabaseName(
                "IX_TicketAttachments_UploadedByUserId");

        builder.HasIndex(attachment => new
            {
                attachment.TicketId,
                attachment.UploadedAt
            })
            .HasDatabaseName(
                "IX_TicketAttachments_TicketId_UploadedAt");
    }
}