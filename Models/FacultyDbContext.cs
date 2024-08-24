using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Faculty_M.Models;

public partial class FacultyDbContext : DbContext
{
    public FacultyDbContext()
    {
    }

    public FacultyDbContext(DbContextOptions<FacultyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }
 

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BO5202G;Database=Faculty_DB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CrsId).HasName("PK__Course__56CAA5D570A225EA");

            entity.ToTable("Course");

            entity.Property(e => e.CrsId)
                .ValueGeneratedNever()
                .HasColumnName("Crs_Id");
            entity.Property(e => e.CrsDuration).HasColumnName("Crs_Duration");
            entity.Property(e => e.CrsName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Crs_Name");
            entity.Property(e => e.DeptId).HasColumnName("Dept_Id");

            entity.HasOne(d => d.Dept).WithMany(p => p.Courses)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK__Course__Dept_Id__4222D4EF");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__Departme__72ABC2CC8B5B53B5");

            entity.ToTable("Department");

            entity.Property(e => e.DeptId)
                .ValueGeneratedNever()
                .HasColumnName("Dept_Id");
            entity.Property(e => e.DeptDesc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Dept_Desc");
            entity.Property(e => e.DeptLocation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Dept_Location");
            entity.Property(e => e.DeptManager)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Dept_Manager");
            entity.Property(e => e.DeptName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Dept_Name");
            entity.Property(e => e.ManagerHireDate).HasColumnName("Manager_HireDate");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InsId).HasName("PK__Instruct__151409EDCDBB3CFF");

            entity.ToTable("Instructor");

            entity.Property(e => e.InsId)
                .ValueGeneratedNever()
                .HasColumnName("Ins_Id");
            entity.Property(e => e.DeptId).HasColumnName("Dept_Id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InsDegree)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ins_Degree");
            entity.Property(e => e.InsName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ins_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Dept).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK__Instructo__Dept___3F466844");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.SchId).HasName("PK__Schedule__85BF5D2E5B129557");

            entity.ToTable("Schedule");

            entity.Property(e => e.SchId)
                .ValueGeneratedNever()
                .HasColumnName("Sch_Id");
            entity.Property(e => e.CrsId).HasColumnName("Crs_Id");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InsId).HasColumnName("Ins_Id");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Crs).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Crs_Id__44FF419A");

            entity.HasOne(d => d.Ins).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.InsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Ins_Id__45F365D3");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StId).HasName("PK__Student__B9034FB3178DFBD2");

            entity.ToTable("Student");

            entity.Property(e => e.StId)
                .ValueGeneratedNever()
                .HasColumnName("St_Id");
            entity.Property(e => e.DeptId).HasColumnName("Dept_Id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.StAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("St_Address");
            entity.Property(e => e.StAge).HasColumnName("St_Age");
            entity.Property(e => e.StName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("St_Name");

            entity.HasOne(d => d.Dept).WithMany(p => p.Students)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK__Student__Dept_Id__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}