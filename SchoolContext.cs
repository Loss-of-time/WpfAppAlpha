using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class Major
{
    public int MajorNo { get; set; }
    public string MajorName { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Course> Courses { get; set; }
}

public class Teacher
{
    public int Tno { get; set; }
    public string Tname { get; set; }
    public string Tsex { get; set; }
    public string Tpassword { get; set; }
    public ICollection<Course> Courses { get; set; }
}

public class Student
{
    public int Sno { get; set; }
    public string Sname { get; set; }
    public string Ssex { get; set; }
    public string Shome { get; set; }
    public int Syear { get; set; }
    public string Spassword { get; set; }
    public int MajorNo { get; set; }
    public Major Major { get; set; }
    public ICollection<CourseSelect> CourseSelects { get; set; }
}

public class Course
{
    public int Cno { get; set; }
    public string Cname { get; set; }
    public int MajorNo { get; set; }
    public Major Major { get; set; }
    public int Tno { get; set; }
    public Teacher Teacher { get; set; }
    public string Ctype { get; set; }
    public float Ccredit { get; set; }
    public string Cstatus { get; set; }
    public string Cterm { get; set; }
    public ICollection<CourseSelect> CourseSelects { get; set; }
}

public class CourseSelect
{
    public int Cno { get; set; }
    public Course Course { get; set; }
    public int Sno { get; set; }
    public Student Student { get; set; }
    public int CSstatus { get; set; }
    public float CSscore { get; set; }
}

public class SchoolContext : DbContext
{
    public DbSet<Major> Major { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Course> Course { get; set; }
    public DbSet<CourseSelect> CourseSelect { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("Server=47.109.205.212;Port=3306;Database=InternShip;User=root;Password=your_strong_password;", new MySqlServerVersion(new Version(8, 0, 21)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Major>()
            .ToTable("Major") // Map Major entity to Major table
            .HasKey(m => m.MajorNo);

        modelBuilder.Entity<Teacher>()
            .ToTable("Teacher") // Map Teacher entity to Teacher table
            .HasKey(t => t.Tno);

        modelBuilder.Entity<Student>()
            .ToTable("Student") // Map Student entity to Student table
            .HasKey(s => s.Sno);

        modelBuilder.Entity<Course>()
            .ToTable("Course") // Map Course entity to Course table
            .HasKey(c => c.Cno);

        modelBuilder.Entity<CourseSelect>()
            .ToTable("CourseSelect") // Map CourseSelect entity to CourseSelect table
            .HasKey(cs => new { cs.Cno, cs.Sno });

        // Configure relationships
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Major)
            .WithMany(m => m.Students)
            .HasForeignKey(s => s.MajorNo);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Major)
            .WithMany(m => m.Courses)
            .HasForeignKey(c => c.MajorNo);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.Tno);

        modelBuilder.Entity<CourseSelect>()
            .HasOne(cs => cs.Course)
            .WithMany(c => c.CourseSelects)
            .HasForeignKey(cs => cs.Cno);

        modelBuilder.Entity<CourseSelect>()
            .HasOne(cs => cs.Student)
            .WithMany(s => s.CourseSelects)
            .HasForeignKey(cs => cs.Sno);

        // Configure other properties if needed
    }
}
