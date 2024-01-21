using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.User.repo;

public class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
    public UserEntity GetByLogin(string login)
    {
        return _matchingSystemContext.Users.Single(user => user.Login.Equals(login));
    }

    public IEnumerable<StudentInitDto> SaveStudents(List<StudentInitDto> students)
    {
        
        foreach (var student in students)
        {
            UserEntity user = new UserEntity()
            {
                Login = student.login,
                PasswordHash = student.passwordHash,
                Name = student.firstName,
                Surname = student.lastName,
                Patronimic = student.middleName,
            };
            user = save(user);
            student.UserId = user.UserId;
        }

        return students;
    }

    public List<UsersRoleEntity> GetStudentUsersByMatching(int matchingId)
    {
        return _matchingSystemContext.UsersRoles.Where(
            role => role.MatchingId == matchingId && role.StudentId != null
        ).ToList();
    }

    public IEnumerable<TutorInitDto> SetUserIdForTutors(List<TutorInitDto> tutors)
    {
        List<TutorView> tutorsRoles = _matchingSystemContext.TutorsViews.Where(role => role.TutorId != null).ToList();
        tutors.ForEach(dto =>
        {
            var userRole = tutorsRoles.FirstOrDefault(tutor => tutor.NameAbbreviation == dto.nameAbbreviation);
            dto.UserId = userRole.UserId;
        });
        return tutors;
    }

    public void UpdatePasswordHash(int userId, string newHash)
    {
        var user = findById(userId);
        user.PasswordHash = newHash;
        save(user);
    }

    public void SetLastVisitDate(int userId, string RoleName, int projectId)
    {
        using (var context = _matchingSystemContext)
        {
            var userRole = context.UsersRoles.FirstOrDefault(
                role => role.UserId == userId &&
                        role.Role.RoleName == RoleName
            );
            userRole.LastVisitDate = DateTime.Now;

            context.UsersRoles.Update(userRole);
            context.SaveChanges();
        }
    }
}