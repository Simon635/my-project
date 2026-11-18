using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<RepairRequest> RepairRequests => Set<RepairRequest>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.ToTable("classrooms");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasData(
                new Classroom { Id = 205, Name = "Аудитория 205" },
                new Classroom { Id = 210, Name = "Аудитория 210" },
                new Classroom { Id = 215, Name = "Аудитория 215" }
            );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(x => x.Login).HasColumnName("login").HasMaxLength(50).IsRequired();
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(x => x.Role).HasColumnName("role").HasMaxLength(30).IsRequired();
            entity.HasIndex(x => x.Login).IsUnique();
            entity.HasData(
                new User { Id = 5, Login = "teacher", Name = "Преподаватель", Role = "Teacher" },
                new User { Id = 6, Login = "dispatcher", Name = "Диспетчер", Role = "Dispatcher" },
                new User { Id = 7, Login = "technician", Name = "Техник", Role = "Technician" }
            );
        });

        modelBuilder.Entity<RepairRequest>(entity =>
        {
            entity.ToTable("repair_requests", table =>
                table.HasCheckConstraint(
                    "ck_repair_requests_status",
                    "\"status\" IN ('New', 'InProgress', 'Closed', 'Cancelled')"
                ));

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(x => x.Number).HasColumnName("number").IsRequired();
            entity.HasIndex(x => x.Number).IsUnique();
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasColumnType("text").IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("New").IsRequired();
            entity.Property(x => x.ClassroomId).HasColumnName("classroom_id");
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.AssigneeUserId).HasColumnName("assignee_user_id");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(x => x.Status).HasDatabaseName("idx_repair_requests_status");
            entity.HasIndex(x => x.ClassroomId).HasDatabaseName("idx_repair_requests_classroom");
            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_repair_requests_created_at");

            entity.HasOne(x => x.Classroom)
                .WithMany(x => x.RepairRequests)
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedRequests)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.AssigneeUser)
                .WithMany(x => x.AssignedRequests)
                .HasForeignKey(x => x.AssigneeUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
