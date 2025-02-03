using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<RegisteredCourse> RegisteredCourses { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }
        public DbSet<Course> CreditsStudent { get; set; }


        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Datos semilla para Teacher
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, Name = "Carlos Molina", Email = "carlos@prueba.com", Phone = "5555555" },
                new Teacher { Id = 2, Name = "Laura Suarez", Email = "laura@prueba.com", Phone = "4444444" },
                new Teacher { Id = 3, Name = "Diana Duarte", Email = "diana@prueba.com", Phone = "3333333" },
                new Teacher { Id = 4, Name = "Luis Rodriguez", Email = "luis@prueba.com", Phone = "4562314" },
                new Teacher { Id = 5, Name = "Oscar Londoño", Email = "oscar@prueba.com", Phone = "9563214" }
            );

            // Datos semilla para Course
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Matematicas", Credits = 3 },
                new Course { Id = 2, Name = "Literatura", Credits = 3 },
                new Course { Id = 3, Name = "Programación", Credits = 3 },
                new Course { Id = 4, Name = "Bases de Datos", Credits = 3 },
                new Course { Id = 5, Name = "Seguridad Informatica", Credits = 3 },
                new Course { Id = 6, Name = "Inteligencia Artificial", Credits = 3 },
                new Course { Id = 7, Name = "Arquitectura", Credits = 3 },
                new Course { Id = 8, Name = "Redes", Credits = 3 },
                new Course { Id = 9, Name = "Ingles", Credits = 3 },
                new Course { Id = 10, Name = "Catedra", Credits = 3 }
            );

            // Datos semilla para TeacherCourse
            modelBuilder.Entity<TeacherCourse>().HasData(
                new TeacherCourse { Id = 1, TeacherId = 1, CourseId = 1 },
                new TeacherCourse { Id = 2, TeacherId = 1, CourseId = 2 },
                new TeacherCourse { Id = 3, TeacherId = 2, CourseId = 3 },
                new TeacherCourse { Id = 4, TeacherId = 2, CourseId = 4 },
                new TeacherCourse { Id = 5, TeacherId = 3, CourseId = 5 },
                new TeacherCourse { Id = 6, TeacherId = 3, CourseId = 6 },
                new TeacherCourse { Id = 7, TeacherId = 4, CourseId = 7 },
                new TeacherCourse { Id = 8, TeacherId = 4, CourseId = 8 },
                new TeacherCourse { Id = 9, TeacherId = 5, CourseId = 9 },
                new TeacherCourse { Id = 10, TeacherId = 5, CourseId = 10 }
            );



            // Configuración para Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Phone)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(15);
            });

            // Configuración para Teacher
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Phone)
                      .IsRequired()
                      .HasMaxLength(15);
            });

            // Configuración para Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Credits)
                      .IsRequired();
            });

            // Configuración para RegisteredCourse
            modelBuilder.Entity<RegisteredCourse>(entity =>
            {

                entity.HasKey(e => e.Id);

                entity.HasOne(rc => rc.Student)
                      .WithMany()
                      .HasForeignKey(rc => rc.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rc => rc.Course)
                      .WithMany()
                      .HasForeignKey(rc => rc.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para TeacherCourse
            modelBuilder.Entity<TeacherCourse>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(tc => tc.Teacher)
                      .WithMany()
                      .HasForeignKey(tc => tc.TeacherId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tc => tc.Course)
                      .WithMany()
                      .HasForeignKey(tc => tc.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para CreditsStudent
            modelBuilder.Entity<CreditsStudent>(entity =>
            {

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Total)
                      .IsRequired();

                entity.HasOne(rc => rc.Student)
                      .WithMany()
                      .HasForeignKey(rc => rc.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}
