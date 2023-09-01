using MatchingSystem.Data.Model;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Context;

public partial class DiplomaMatchingContext :DbContext
{      

    public DiplomaMatchingContext(DbContextOptions<DiplomaMatchingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActiveCommonQuota> ActiveCommonQuotas { get; set; } = null!;
    public virtual DbSet<Allocation> Allocations { get; set; } = null!;
    public virtual DbSet<AllocationFullInfo> AllocationFullInfos { get; set; } = null!;
    public virtual DbSet<AvailableStudentsPreference> AvailableStudentsPreferences { get; set; } = null!;
    public virtual DbSet<ChoosingType> ChoosingTypes { get; set; } = null!;
    public virtual DbSet<CommonQuota> CommonQuotas { get; set; } = null!;
    public virtual DbSet<CommonQuota1> CommonQuotas1 { get; set; } = null!;
    public virtual DbSet<Document> Documents { get; set; } = null!;
    public virtual DbSet<Group> Groups { get; set; } = null!;
    public virtual DbSet<Log> Logs { get; set; } = null!;
    public virtual DbSet<Matching> Matchings { get; set; } = null!;
    public virtual DbSet<MatchingType> MatchingTypes { get; set; } = null!;
    public virtual DbSet<Project> Projects { get; set; } = null!;
    public virtual DbSet<Project1> Projects1 { get; set; } = null!;
    public virtual DbSet<ProjectsGroup> ProjectsGroups { get; set; } = null!;
    public virtual DbSet<ProjectsTechnology> ProjectsTechnologies { get; set; } = null!;
    public virtual DbSet<ProjectsWorkDirection> ProjectsWorkDirections { get; set; } = null!;
    public virtual DbSet<QuotasState> QuotasStates { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Stage> Stages { get; set; } = null!;
    public virtual DbSet<StagesType> StagesTypes { get; set; } = null!;
    public virtual DbSet<Student> Students { get; set; } = null!;
    public virtual DbSet<Student1> Students1 { get; set; } = null!;
    public virtual DbSet<StudentsPreference> StudentsPreferences { get; set; } = null!;
    public virtual DbSet<StudentsTechnology> StudentsTechnologies { get; set; } = null!;
    public virtual DbSet<StudentsWorkDirection> StudentsWorkDirections { get; set; } = null!;
    public virtual DbSet<Technology> Technologies { get; set; } = null!;
    public virtual DbSet<Tutor> Tutors { get; set; } = null!;
    public virtual DbSet<TutorViewDto> Tutors1 { get; set; } = null!;
    public virtual DbSet<TutorsChoice> TutorsChoices { get; set; } = null!;
    public virtual DbSet<TutorsGroup> TutorsGroups { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<UsersFullInfo> UsersFullInfos { get; set; } = null!;
    public virtual DbSet<UsersRole> UsersRoles { get; set; } = null!;
    public virtual DbSet<VersionInfo> VersionInfos { get; set; } = null!;
    public virtual DbSet<WorkDirection> WorkDirections { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActiveCommonQuota>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("ActiveCommonQuotas", "dbo_v");

            entity.Property(e => e.CommonQuotaId).HasColumnName("CommonQuotaID");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.Message).HasMaxLength(250);

            entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

            entity.Property(e => e.StageId).HasColumnName("StageID");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Allocation>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Allocation", "dbo_v");

            entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

            entity.Property(e => e.StageTypeName).HasMaxLength(50);

            entity.Property(e => e.StageTypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("StageTypeName_ru");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.Property(e => e.TypeName).HasMaxLength(50);

            entity.Property(e => e.TypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("TypeName_ru");
        });

        modelBuilder.Entity<AllocationFullInfo>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Allocation_FullInfo", "dbo_v");

            entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.GroupName).HasMaxLength(100);

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.ProjectName).HasMaxLength(200);

            entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

            entity.Property(e => e.StageTypeName).HasMaxLength(50);

            entity.Property(e => e.StageTypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("StageTypeName_ru");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.StudentName).HasMaxLength(100);

            entity.Property(e => e.StudentNameAbbreviation).HasMaxLength(106);

            entity.Property(e => e.StudentPatronimic).HasMaxLength(100);

            entity.Property(e => e.StudentSurname).HasMaxLength(100);

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.TutorName).HasMaxLength(100);

            entity.Property(e => e.TutorNameAbbreviation).HasMaxLength(106);

            entity.Property(e => e.TutorPatronimic).HasMaxLength(100);

            entity.Property(e => e.TutorSurname).HasMaxLength(100);

            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.Property(e => e.TypeName).HasMaxLength(50);

            entity.Property(e => e.TypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("TypeName_ru");
        });

        modelBuilder.Entity<AvailableStudentsPreference>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("AvailableStudentsPreferences", "dbo_v");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
        });

        modelBuilder.Entity<ChoosingType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.Property(e => e.TypeName).HasMaxLength(50);

            entity.Property(e => e.TypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("TypeName_ru");
        });

        modelBuilder.Entity<CommonQuota>(entity =>
        {
            entity.Property(e => e.CommonQuotaId).HasColumnName("CommonQuotaID");

            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Message).HasMaxLength(250);

            entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

            entity.Property(e => e.StageId).HasColumnName("StageID");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.QuotaState)
                .WithMany(p => p.CommonQuota)
                .HasForeignKey(d => d.QuotaStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommonQuotas_QuotasStates");

            entity.HasOne(d => d.Stage)
                .WithMany(p => p.CommonQuota)
                .HasForeignKey(d => d.StageId)
                .HasConstraintName("FK_CommonQuotas_Stages");

            entity.HasOne(d => d.Tutor)
                .WithMany(p => p.CommonQuota)
                .HasForeignKey(d => d.TutorId)
                .HasConstraintName("FK_CommonQuotas_Tutors");
        });

        modelBuilder.Entity<CommonQuota1>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("CommonQuotas", "dbo_v");

            entity.Property(e => e.CommonQuotaId).HasColumnName("CommonQuotaID");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.Message).HasMaxLength(250);

            entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

            entity.Property(e => e.QuotaStateName).HasMaxLength(50);

            entity.Property(e => e.QuotaStateNameRu)
                .HasMaxLength(50)
                .HasColumnName("QuotaStateName_ru");

            entity.Property(e => e.StageId).HasColumnName("StageID");

            entity.Property(e => e.StageTypeName).HasMaxLength(50);

            entity.Property(e => e.StageTypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("StageTypeName_ru");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

            entity.Property(e => e.DocumentName).HasMaxLength(100);

            entity.Property(e => e.StageId).HasColumnName("StageID");

            entity.HasOne(d => d.Stage)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.StageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documents_Stages");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.GroupBk).HasColumnName("GroupBK");

            entity.Property(e => e.GroupName).HasMaxLength(100);

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.HasOne(d => d.Matching)
                .WithMany(p => p.Groups)
                .HasForeignKey(d => d.MatchingId)
                .HasConstraintName("FK_Groups_Matching");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("Log");

            entity.Property(e => e.Endpoint).HasColumnType("text");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.Property(e => e.Request).HasColumnType("text");
        });

        modelBuilder.ApplyConfiguration(new MatchingTable());
        modelBuilder.ApplyConfiguration(new MatchingTypeTable());
    
        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.CloseDate).HasColumnType("datetime");

            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.ProjectName).HasMaxLength(200);

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Tutor)
                .WithMany(p => p.Projects)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Tutors");
        });

        modelBuilder.Entity<Project1>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Projects", "dbo_v");

            entity.Property(e => e.AvailableGroupsNameList).HasColumnName("AvailableGroupsName_List");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.ProjectName).HasMaxLength(200);

            entity.Property(e => e.QtyDescription).HasMaxLength(50);

            entity.Property(e => e.TechnologiesNameList).HasColumnName("TechnologiesName_List");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.TutorIsClosed).HasColumnName("Tutor_IsClosed");

            entity.Property(e => e.TutorName).HasMaxLength(100);

            entity.Property(e => e.TutorNameAbbreviation).HasMaxLength(106);

            entity.Property(e => e.TutorPatronimic).HasMaxLength(100);

            entity.Property(e => e.TutorSurname).HasMaxLength(100);

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.Property(e => e.WorkDirectionsNameList).HasColumnName("WorkDirectionsName_List");
        });

        modelBuilder.Entity<ProjectsGroup>(entity =>
        {
            entity.HasKey(e => e.ProjectGroupId)
                .HasName("PK__Projects__17125E7ED62BC05D");

            entity.ToTable("Projects_Groups");

            entity.Property(e => e.ProjectGroupId).HasColumnName("ProjectGroupID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Group)
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

            entity.HasOne(d => d.Direction)
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

        modelBuilder.Entity<QuotasState>(entity =>
        {
            entity.HasKey(e => e.QuotaStateId);

            entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

            entity.Property(e => e.QuotaStateName).HasMaxLength(50);

            entity.Property(e => e.QuotaStateNameRu)
                .HasMaxLength(50)
                .HasColumnName("QuotaStateName_ru");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.Property(e => e.RoleName).HasMaxLength(50);

            entity.Property(e => e.RoleNameRu)
                .HasMaxLength(50)
                .HasColumnName("RoleName_ru");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.Property(e => e.StageId).HasColumnName("StageID");

            entity.Property(e => e.EndDate).HasColumnType("datetime");

            entity.Property(e => e.EndPlanDate).HasColumnType("datetime");

            entity.Property(e => e.IsCurrent)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.StageName).HasMaxLength(100);

            entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Matching)
                .WithMany(p => p.Stages)
                .HasForeignKey(d => d.MatchingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stages_Matching");

            entity.HasOne(d => d.StageType)
                .WithMany(p => p.Stages)
                .HasForeignKey(d => d.StageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stages_StagesTypes");
        });

        modelBuilder.Entity<StagesType>(entity =>
        {
            entity.HasKey(e => e.StageTypeId);

            entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

            entity.Property(e => e.StageTypeName).HasMaxLength(50);

            entity.Property(e => e.StageTypeNameRu)
                .HasMaxLength(50)
                .HasColumnName("StageTypeName_ru");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.Info2).HasMaxLength(250);

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.StudentBk).HasColumnName("StudentBK");

            entity.HasOne(d => d.Group)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Groups");
        });

        modelBuilder.Entity<Student1>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Students", "dbo_v");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.GroupName).HasMaxLength(100);

            entity.Property(e => e.Info2).HasMaxLength(250);

            entity.Property(e => e.LastVisitDate).HasColumnType("datetime");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.NameAbbreviation).HasMaxLength(106);

            entity.Property(e => e.Patronimic).HasMaxLength(100);

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.Property(e => e.TechnologiesNameList).HasColumnName("TechnologiesName_List");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.Property(e => e.WorkDirectionsNameList).HasColumnName("WorkDirectionsName_List");
        });

        modelBuilder.Entity<StudentsPreference>(entity =>
        {
            entity.HasKey(e => e.PreferenceId);

            entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IsAvailable)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.IsInUse).HasDefaultValueSql("((0))");

            entity.Property(e => e.IsUsed).HasDefaultValueSql("((0))");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.TypeId)
                .HasColumnName("TypeID")
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.StudentsPreferences)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentsPreferences_Projects");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentsPreferences)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentsPreferences_Students");

            entity.HasOne(d => d.Type)
                .WithMany(p => p.StudentsPreferences)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_StudentsPreferences_ChoosingTypes");
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

            entity.HasOne(d => d.Direction)
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

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

            entity.Property(e => e.TechnologyNameRu)
                .HasMaxLength(200)
                .HasColumnName("TechnologyName_ru");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.TutorBk).HasColumnName("TutorBK");
        });

        modelBuilder.Entity<TutorViewDto>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Tutors", "dbo_v");

            entity.Property(e => e.LastVisitDate).HasColumnType("datetime");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.NameAbbreviation).HasMaxLength(106);

            entity.Property(e => e.Patronimic).HasMaxLength(100);

            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<TutorsChoice>(entity =>
        {
            entity.HasKey(e => e.ChoiceId)
                .HasName("PK_TutorsMatching");

            entity.ToTable("TutorsChoice");

            entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");

            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IsChangeble).HasDefaultValueSql("((1))");

            entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.SortOrderNumber).HasDefaultValueSql("((32767))");

            entity.Property(e => e.StageId).HasColumnName("StageID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.TypeId)
                .HasColumnName("TypeID")
                .HasDefaultValueSql("((2))");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.TutorsChoices)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorsChoice_Projects");

            entity.HasOne(d => d.Stage)
                .WithMany(p => p.TutorsChoices)
                .HasForeignKey(d => d.StageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorsChoice_Stages");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.TutorsChoices)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorsChoice_Students");

            entity.HasOne(d => d.Type)
                .WithMany(p => p.TutorsChoices)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorsChoice_ChoosingTypes");
        });

        modelBuilder.Entity<TutorsGroup>(entity =>
        {
            entity.HasKey(e => e.TutorGroupId);

            entity.ToTable("Tutors_Groups");

            entity.Property(e => e.TutorGroupId).HasColumnName("TutorGroupID");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.HasOne(d => d.Group)
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

        modelBuilder.ApplyConfiguration(new UserTable());

        modelBuilder.Entity<UsersFullInfo>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Users_FullInfo", "dbo_v");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.Property(e => e.GroupName).HasMaxLength(100);

            entity.Property(e => e.LastVisitDate).HasColumnType("datetime");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.NameAbbreviation).HasMaxLength(106);

            entity.Property(e => e.Patronimic).HasMaxLength(100);

            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.Property(e => e.RoleName).HasMaxLength(50);

            entity.Property(e => e.RoleNameRu)
                .HasMaxLength(50)
                .HasColumnName("RoleName_ru");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UsersRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId);

            entity.ToTable("Users_Roles");

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

            entity.Property(e => e.LastVisitDate).HasColumnType("datetime");

            entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.TutorId).HasColumnName("TutorID");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Matching)
                .WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.MatchingId)
                .HasConstraintName("FK_Users_Roles_Matching");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles_Roles");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Users_Roles_Students");

            entity.HasOne(d => d.Tutor)
                .WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.TutorId)
                .HasConstraintName("FK_Users_Roles_Tutors");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UsersRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles_Users");
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

        modelBuilder.Entity<WorkDirection>(entity =>
        {
            entity.HasKey(e => e.DirectionId)
                .HasName("PK_Directions");

            entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

            entity.Property(e => e.DirectionNameRu)
                .HasMaxLength(200)
                .HasColumnName("DirectionName_ru");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}