using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.User.service;

public class UserRepository :Repository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    public User GetByLogin(string login)
    {
        return _matchingSystemContext.Users.Single(user => user.Login.Equals(login));
    }
    
    public IEnumerable<StudentInitDto> SaveStudents(List<StudentInitDto> students)
    {
        foreach (var student in students)
        {
            User user = new User()
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

    public IEnumerable<UserDto> GetStudentUsersByMatching(int matchingId)
    {
        var userRoles = _matchingSystemContext.UsersRoles.Where(
            role => role.MatchingId == matchingId && role.StudentId!=null
        ).ToList();
        return userRoles.Select(role => UserDto.fromEntity(role.User));
    }

    public IEnumerable<TutorInitDto> SetUserIdForTutors(List<TutorInitDto> tutors)
    {
      
       List<TutorViewDto> tutorsRoles =   _matchingSystemContext.Tutors1.Where(role => role.TutorId != null).ToList();
       tutors.ForEach(dto =>
       {
          var userRole =  tutorsRoles.FirstOrDefault(tutor => tutor.NameAbbreviation == dto.nameAbbreviation);
          dto.UserId = userRole.UserId;
          dto.TutorId = (int)userRole.TutorId;
       } );
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
        var userRole = _matchingSystemContext.UsersRoles.FirstOrDefault(
            role => role.UserId == userId &&
                    role.Role.RoleName == RoleName
        );
        userRole.LastVisitDate = DateTime.Now;
        _matchingSystemContext.UsersRoles.Update(userRole);
    }
}