using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorLearnWebApp.Models;

namespace SeniorLearnWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<DeliveryPattern> DeliveryPatterns { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberRole> MemberRoles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeliveryPattern>()
                .Property(m => m.DeliveryPatternId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Enrolment>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.LessonId });

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.Enrolments)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.LessonId);

                entity.HasOne(e => e.Instructor)
                    .WithMany()
                    .HasForeignKey(e => e.InstructorId)
                    .HasPrincipalKey(m => m.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DeliveryPattern)
                    .WithMany(dp => dp.Lessons) 
                    .HasForeignKey(e => e.DeliveryPatternId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Member>()
                .Property(m => m.MemberId)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<MemberRole>(entity =>
            {
                entity.HasKey(e => e.MemberRoleId);

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.MemberRoles)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.HasOne(p => p.Member)
                    .WithMany(m => m.Payments)
                    .HasForeignKey(p => p.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Amount)
                    .HasPrecision(10, 2);

            });
            
            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.HasKey(t => t.TimetableId);
                
                entity.HasOne(t => t.Lesson)
                    .WithMany(l => l.Timetables)
                    .HasForeignKey(t => t.LessonId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });
        }
        

    }
}
