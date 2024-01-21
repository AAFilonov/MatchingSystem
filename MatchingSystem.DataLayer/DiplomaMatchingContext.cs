using MatchingSystem.DataLayer._Technical;
using MatchingSystem.DataLayer.DbSchema.Tables;
using MatchingSystem.DataLayer.DbSchema.View;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.Feature.User;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer;

public class DiplomaMatchingContext: DbContext
{      

    public DiplomaMatchingContext(DbContextOptions<DiplomaMatchingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActiveCommonQuotaView> ActiveCommonQuotas { get; set; } = null!;
    public virtual DbSet<AllocationView> Allocations { get; set; } = null!;
    public virtual DbSet<AllocationFullInfoView> AllocationFullInfos { get; set; } = null!;
    public virtual DbSet<AvailableStudentsPreferenceView> AvailableStudentsPreferences { get; set; } = null!;
    public virtual DbSet<ChoosingTypeEntity> ChoosingTypes { get; set; } = null!;
    public virtual DbSet<CommonQuotaEntity> CommonQuotas { get; set; } = null!;
    public virtual DbSet<CommonQuotaView> commonQuotasViews { get; set; } = null!;
    public virtual DbSet<GroupEntity> Groups { get; set; } = null!;
    public virtual DbSet<MatchingEntity> Matchings { get; set; } = null!;
    public virtual DbSet<MatchingType> MatchingTypes { get; set; } = null!;
    public virtual DbSet<ProjectEntity> Projects { get; set; } = null!;
    public virtual DbSet<ProjectView> Projects1 { get; set; } = null!;
    public virtual DbSet<ProjectsGroupEntity> ProjectsGroups { get; set; } = null!;
    public virtual DbSet<ProjectsTechnology> ProjectsTechnologies { get; set; } = null!;
    public virtual DbSet<ProjectsWorkDirection> ProjectsWorkDirections { get; set; } = null!;
    public virtual DbSet<QuotasStateEntity> QuotasStates { get; set; } = null!;
    public virtual DbSet<RoleEntity> Roles { get; set; } = null!;
    public virtual DbSet<StageEntity> Stages { get; set; } = null!;
    public virtual DbSet<StagesType> StagesTypes { get; set; } = null!;
    public virtual DbSet<StudentEntity> Students { get; set; } = null!;
    public virtual DbSet<StudentView> StudentsViews { get; set; } = null!;
    public virtual DbSet<StudentsPreference> StudentsPreferences { get; set; } = null!;
    public virtual DbSet<StudentsTechnology> StudentsTechnologies { get; set; } = null!;
    public virtual DbSet<StudentsWorkDirection> StudentsWorkDirections { get; set; } = null!;
    public virtual DbSet<TechnologyEntity> Technologies { get; set; } = null!;
    public virtual DbSet<TutorEntity> Tutors { get; set; } = null!;
    public virtual DbSet<TutorView> TutorsViews { get; set; } = null!;
    public virtual DbSet<TutorsChoiceEntity> TutorsChoices { get; set; } = null!;
    public virtual DbSet<TutorsGroup> TutorsGroups { get; set; } = null!;
    public virtual DbSet<UserEntity> Users { get; set; } = null!;
    public virtual DbSet<UsersFullInfoView> UsersFullInfos { get; set; } = null!;
    public virtual DbSet<UsersRoleEntity> UsersRoles { get; set; } = null!;
    public virtual DbSet<VersionInfo> VersionInfos { get; set; } = null!;
    public virtual DbSet<WorkDirectionEntity> WorkDirections { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Таблицы данных
        modelBuilder.ApplyConfiguration(new MatchingDboTable());
        modelBuilder.ApplyConfiguration(new UserRoleDboTable());
        modelBuilder.ApplyConfiguration(new CommonQuotaDboTable());
        modelBuilder.ApplyConfiguration(new UserDboTable());
        modelBuilder.ApplyConfiguration(new GroupDboTable());
        modelBuilder.ApplyConfiguration(new RoleDboTable());
        modelBuilder.ApplyConfiguration(new StageDboTable());
        modelBuilder.ApplyConfiguration(new TutorDboTable());
        modelBuilder.ApplyConfiguration(new ProjectDboTable());
        modelBuilder.ApplyConfiguration(new StudentDboTable());
        modelBuilder.ApplyConfiguration(new TutorChoiseDboTable());
        modelBuilder.ApplyConfiguration(new StudentsPreferenceDboTable());
        
        //вьюшки
        modelBuilder.ApplyConfiguration(new StudentsDboView());
        modelBuilder.ApplyConfiguration(new ActiveCommonQuotaDboView());
        modelBuilder.ApplyConfiguration(new CommonQuotaDboView());
        modelBuilder.ApplyConfiguration(new ProjectDboView());
        modelBuilder.ApplyConfiguration(new AvailableStudentsPreferenceDboView());
        modelBuilder.ApplyConfiguration(new AllocationDboView());
        modelBuilder.ApplyConfiguration(new AllocationFullInfoDboView());
        modelBuilder.ApplyConfiguration(new UserFullInfoDboView());
        modelBuilder.ApplyConfiguration(new TutorDboView());
        
        
        //справочники
        modelBuilder.ApplyConfiguration(new MatchingTypeDboTable());
        
        modelBuilder.Entity<ChoosingTypeEntity>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK_ChoosingTypes");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
            entity.Property(e => e.TypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("TypeName_ru");
        });
        
        modelBuilder.Entity<StagesType>(entity =>
        {
            entity.HasKey(e => e.StageTypeId).HasName("PK_StagesTypes");
            entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

            entity.Property(e => e.StageTypeName).HasMaxLength(50);

            entity.Property(e => e.StageTypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("StageTypeName_ru");
        });
        
        modelBuilder.Entity<TechnologyEntity>(entity =>
        {
            entity.HasKey(e => e.TechnologyId).HasName("PK_Technologies");
            entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

            entity.Property(e => e.TechnologyNameRu)
                .HasMaxLength(200)
                .HasColumnName("TechnologyName_ru");
        });
        
        modelBuilder.Entity<WorkDirectionEntity>(entity =>
        {
            entity.HasKey(e => e.DirectionId).HasName("PK_Directions");

            entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

            entity.Property(e => e.DirectionNameRu)
                .HasMaxLength(200)
                .HasColumnName("DirectionName_ru");
        });
        
        //технические таблицы и таблицы связи

        
        modelBuilder.Entity<ProjectsGroupEntity>(entity =>
        {
            entity.HasKey(e => e.ProjectGroupId)
                .HasName("PK__Projects__17125E7ED62BC05D");

            entity.ToTable("Projects_Groups");

            entity.Property(e => e.ProjectGroupId).HasColumnName("ProjectGroupID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.groupEntity)
                .WithMany(p => p.ProjectsGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Groups_Groups");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.ProjectsGroups)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Groups_Projects");
        });

        modelBuilder.Entity<ProjectsTechnology>(entity =>
        {
            entity.HasKey(e => e.ProjectTechnologyId)
                .HasName("PK_ProjectsTechnologies");

            entity.ToTable("Projects_Technologies");

            entity.Property(e => e.ProjectTechnologyId).HasColumnName("ProjectTechnologyID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.ProjectsTechnologies)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Technologies_Projects");

            entity.HasOne(d => d.Technology)
                .WithMany(p => p.ProjectsTechnologies)
                .HasForeignKey(d => d.TechnologyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Technologies_Technologies");
        });

        modelBuilder.Entity<ProjectsWorkDirection>(entity =>
        {
            entity.HasKey(e => e.ProjectDirectionId)
                .HasName("PK_Projects_Directions");

            entity.ToTable("Projects_WorkDirections");

            entity.Property(e => e.ProjectDirectionId).HasColumnName("ProjectDirectionID");

            entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.directionEntity)
                .WithMany(p => p.ProjectsWorkDirections)
                .HasForeignKey(d => d.DirectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_WorkDirections_WorkDirections");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.ProjectsWorkDirections)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_WorkDirections_Projects");
        });

        modelBuilder.Entity<QuotasStateEntity>(entity =>
        {
            entity.HasKey(e => e.QuotaStateId);

            entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

            entity.Property(e => e.QuotaStateName).HasMaxLength(50);

            entity.Property(e => e.QuotaStateNameRu)
                .HasMaxLength(50)
                .HasColumnName("QuotaStateName_ru");
        });
        
        modelBuilder.Entity<StudentsTechnology>(entity =>
        {
            entity.HasKey(e => e.StudentTechnologyId)
                .HasName("PK_StudentsTechnologies");

            entity.ToTable("Students_Technologies");

            entity.Property(e => e.StudentTechnologyId).HasColumnName("StudentTechnologyID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentsTechnologies)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Technologies_Students");

            entity.HasOne(d => d.Technology)
                .WithMany(p => p.StudentsTechnologies)
                .HasForeignKey(d => d.TechnologyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Technologies_Technologies");
        });

        modelBuilder.Entity<StudentsWorkDirection>(entity =>
        {
            entity.HasKey(e => e.StudentDirectionId)
                .HasName("PK_StudentsDirections");

            entity.ToTable("Students_WorkDirections");

            entity.Property(e => e.StudentDirectionId).HasColumnName("StudentDirectionID");

            entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.directionEntity)
                .WithMany(p => p.StudentsWorkDirections)
                .HasForeignKey(d => d.DirectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_WorkDirections_WorkDirections");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentsWorkDirections)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_WorkDirections_Students");
        });
        
        modelBuilder.Entity<TutorsGroup>(entity =>
        {
            entity.HasKey(e => e.TutorGroupId);

            entity.ToTable("Tutors_Groups");

            entity.Property(e => e.TutorGroupId).HasColumnName("TutorGroupID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.HasOne(d => d.groupEntity)
                .WithMany(p => p.TutorsGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tutors_Groups_Groups");

            entity.HasOne(d => d.Tutor)
                .WithMany(p => p.TutorsGroups)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tutors_Groups_Tutors");
        });
        
        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable("VersionInfo");
            entity.HasIndex(e => e.Version, "UC_Version")
                .IsUnique()
                .IsClustered();
            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });
        
        //modelBuilder.Entity<Test>(entity =>
        //{
        //    entity.HasNoKey();
        //    entity.ToTable("Test");
        //    entity.Property(e => e.Description).HasMaxLength(1024);
        //});
    }
}