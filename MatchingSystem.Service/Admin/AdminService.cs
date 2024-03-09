using System;
using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;

namespace MatchingSystem.Service.Admin;

[AllArgsConstructor]
public partial class AdminService : IAdminService
{
    private readonly IRoleRepository roleRepository;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;
    private readonly ICommonQuotaRepository commonQuotaRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IProjectRepository projectRepository;

    private void CheckUserRoleEntity(int userId, int matchingId, int roleId)
    {
        UsersRoleEntity userRole = roleRepository.find(role =>
            role.UserId == userId
            && role.MatchingId == matchingId
            && role.RoleId == roleId
        ).FirstOrDefault();

        if (userRole != null && userRole.IsDeleted == false)
        {
            throw new System.Exception("Роль с такой конфигурацией уже существует");
        }
    }

    public void AssignDefaultRole(AssignDefaultRoleRequest payload)
    {
        CheckUserRoleEntity(
            payload.User.UserId,
            payload.User.MatchingId,
            payload.User.RoleId
        );

        roleRepository.AssignRoleForUser(
            payload.User.UserId,
            payload.User.MatchingId,
            payload.User.RoleId
        );
    }

    public void AssignStudentRole(AssignStudentRoleRequest payload)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(payload.User.MatchingId);

        if (currentStage.StageTypeId > (int)StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES)
        {
            throw new System.Exception("Этап создания студентов уже прошёл");
        }
        
        CheckUserRoleEntity(
            payload.User.UserId,
            payload.User.MatchingId,
            payload.User.RoleId
        );

        roleRepository.CallInTransaction((context) =>
        {
            var possibleStudentRole = roleRepository.find(role =>
                role.MatchingId == payload.User.MatchingId
                && role.RoleId == (int)RolesEnum.STUDENT
                && role.UserId == payload.User.UserId
            ).FirstOrDefault();

            if (possibleStudentRole != null)
            {
                possibleStudentRole.IsDeleted = false;
                roleRepository.update(possibleStudentRole, context);

                StudentEntity studentEntity = studentRepository.find(student =>
                    student.StudentId == possibleStudentRole.StudentId
                ).Single();
                studentEntity.IsDeleted = false;
                studentRepository.update(studentEntity);
            }
            else
            {
                StudentEntity newStudentEntity = new StudentEntity
                {
                    GroupId = payload.GroupId,
                    MatchingId = payload.User.MatchingId,
                };
                studentRepository.save(newStudentEntity, context);

                var studentRole = new UsersRoleEntity
                {
                    MatchingId = payload.User.MatchingId,
                    RoleId = (int)RolesEnum.STUDENT,
                    UserId = payload.User.UserId,
                    StudentId = newStudentEntity.StudentId,
                };

                roleRepository.save(studentRole, context);
            }
        });
    }

    public void AssignTutorRole(AssignTutorRoleRequest payload)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(payload.User.MatchingId);

        if (currentStage.StageTypeId > (int)StageTypeEnum.MATCHING_PREPARATION)
        {
            throw new System.Exception("Этап создания преподавателей уже прошёл");
        }
        
        CheckUserRoleEntity(
            payload.User.UserId,
            payload.User.MatchingId,
            payload.User.RoleId
        );

        roleRepository.CallInTransaction((context) =>
        {
            var possibleTutorRole = roleRepository.find(role =>
                role.MatchingId == payload.User.MatchingId
                && role.RoleId == (int)RolesEnum.TUTOR
                && role.UserId == payload.User.UserId
            ).FirstOrDefault();

            if (possibleTutorRole != null)
            {
                possibleTutorRole.IsDeleted = false;
                roleRepository.update(possibleTutorRole, context);

                TutorEntity tutorEntity =
                    tutorRepository.find(tutor =>
                        tutor.TutorId == possibleTutorRole.TutorId
                    ).Single();
                tutorEntity.IsDeleted = false;
                tutorRepository.update(tutorEntity);
                
                
                CommonQuotaEntity quotaEntity = new CommonQuotaEntity
                {
                    TutorId = tutorEntity.TutorId,
                    Qty = (short)payload.Quota,
                    QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE,
                    IsNotification = false,
                    CreateDate = DateTime.Now,
                    StageId = currentStage.StageId
                };
                commonQuotaRepository.save(quotaEntity, context);

                ProjectEntity projectEntity = new ProjectEntity
                {
                    ProjectName = "Записаться к преподавателю",
                    IsClosed = false,
                    IsDefault = true,
                    TutorId = tutorEntity.TutorId,
                    MatchingId = payload.User.MatchingId,
                };
                projectRepository.save(projectEntity, context);

                context.TutorsGroups.AddRange(payload.AssignedGroups.Select(groupId => new TutorsGroup
                {
                    GroupId = groupId,
                    TutorId = tutorEntity.TutorId,
                }));

                context.ProjectsGroups.AddRange(payload.AssignedGroups.Select((groupId) => new ProjectsGroupEntity()
                {
                    GroupId = groupId,
                    ProjectId = projectEntity.ProjectId,
                }));
            }
            else
            {
                TutorEntity newTutorEntity = new TutorEntity
                {
                    MatchingId = payload.User.MatchingId,
                    IsReadyToStart = false,
                };
                tutorRepository.save(newTutorEntity, context);

                StageEntity currentStage = matchingRepository.GetCurrentStage(payload.User.MatchingId);
                CommonQuotaEntity quotaEntity = new CommonQuotaEntity
                {
                    TutorId = newTutorEntity.TutorId,
                    Qty = (short)payload.Quota,
                    QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE,
                    IsNotification = false,
                    CreateDate = DateTime.Now,
                    StageId = currentStage.StageId
                };
                commonQuotaRepository.save(quotaEntity, context);

                ProjectEntity projectEntity = new ProjectEntity
                {
                    ProjectName = "Записаться к преподавателю",
                    IsClosed = false,
                    IsDefault = true,
                    TutorId = newTutorEntity.TutorId,
                    MatchingId = payload.User.MatchingId,
                };
                projectRepository.save(projectEntity, context);

                context.TutorsGroups.AddRange(payload.AssignedGroups.Select(groupId => new TutorsGroup
                {
                    GroupId = groupId,
                    TutorId = newTutorEntity.TutorId,
                }));

                context.ProjectsGroups.AddRange(payload.AssignedGroups.Select((groupId) => new ProjectsGroupEntity()
                {
                    GroupId = groupId,
                    ProjectId = projectEntity.ProjectId,
                }));

                var studentRole = new UsersRoleEntity
                {
                    MatchingId = payload.User.MatchingId,
                    RoleId = (int)RolesEnum.TUTOR,
                    UserId = payload.User.UserId,
                    TutorId = newTutorEntity.TutorId,
                };

                roleRepository.save(studentRole, context);
            }
        });
    }

    public void RemoveDefaultRole(RemoveRoleRequest payload)
    {
        UsersRoleEntity userRole = roleRepository.find(role =>
            role.UserId == payload.User.UserId
            && role.MatchingId == payload.User.MatchingId
            && role.RoleId == payload.User.RoleId
        ).Single();
        roleRepository.remove(userRole);
    }

    public void RemoveStudentRole(RemoveRoleRequest payload)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(payload.User.MatchingId);

        if (currentStage.StageTypeId > (int)StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES)
        {
            throw new System.Exception("Этап удаления студентов уже прошёл");
        }
        
        UsersRoleEntity userRole = roleRepository.find(role =>
            role.UserId == payload.User.UserId
            && role.MatchingId == payload.User.MatchingId
            && role.RoleId == payload.User.RoleId
        ).Single();
        userRole.IsDeleted = true;
        roleRepository.update(userRole);


        StudentEntity studentEntity = studentRepository.find(student =>
            student.StudentId == userRole.StudentId
        ).Single();
        studentEntity.IsDeleted = true;
        studentRepository.update(studentEntity);
    }

    public void RemoveTutorRole(RemoveRoleRequest payload)
    {
        StageEntity currentStage = matchingRepository.GetCurrentStage(payload.User.MatchingId);

        if (currentStage.StageTypeId > (int)StageTypeEnum.MATCHING_PREPARATION)
        {
            throw new System.Exception("Этап удаления преподавателей уже прошёл");
        }
        
        roleRepository.CallInTransaction(context =>
        {
            UsersRoleEntity userRole = roleRepository.find(role =>
                role.UserId == payload.User.UserId
                && role.MatchingId == payload.User.MatchingId
                && role.RoleId == payload.User.RoleId
            ).Single();
            userRole.IsDeleted = true;
            roleRepository.update(userRole, context);


            TutorEntity tutorEntity = tutorRepository.find(tutor =>
                tutor.TutorId == userRole.TutorId
            ).Single();
            tutorEntity.IsDeleted = true;
            tutorEntity.IsReadyToStart = false;
            tutorRepository.update(tutorEntity, context);

            // нахождение всех проектов
            IEnumerable<ProjectEntity> projectEntities = projectRepository.find(proj =>
                proj.TutorId == tutorEntity.TutorId
            );
            // нахождение и удаление всех групп, связанных с преподом
            IEnumerable<TutorsGroup> tutorsGroups = context.TutorsGroups.Where(group =>
                group.TutorId == tutorEntity.TutorId
            );
            context.TutorsGroups.RemoveRange(tutorsGroups);

            // нахождение и удаление всех групп, связанных с проектами, найденными ранее
            foreach (var projectEntity in projectEntities)
            {
                IEnumerable<ProjectsGroupEntity> projectsGroups = context.ProjectsGroups.Where(group =>
                    group.ProjectId == projectEntity.ProjectId
                );
                context.ProjectsGroups.RemoveRange(projectsGroups);
            }

            // нахождение и удаление всех квот
            commonQuotaRepository.removeRange(commonQuotaRepository.find(quota =>
                quota.TutorId == tutorEntity.TutorId
            ), context);
            
            // удаление всех проектов
            projectRepository.removeRange(projectEntities, context);
        });
    }
}