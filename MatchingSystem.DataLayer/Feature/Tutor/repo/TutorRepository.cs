using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Tutor.repo;

public class TutorRepository : GenericRepository<TutorEntity>, ITutorRepository
{
    public TutorRepository(DbContext context) : base(context)
    {
    }

    public void AssignTutorRole(List<TutorInitDto> tuts, int matchingID)
    {
        var tutors = tuts.Select(info =>
        {
            UsersRoleEntity role = new UsersRoleEntity()
            {
                UserId = info.UserId,
                TutorId = info.UserId,
                MatchingId = matchingID,
                RoleId = 1,
            };
            return role;
        });
        using (var context = _matchingSystemContext)
        {
            context.UsersRoles.AddRange(tutors);
            context.SaveChanges();
        }
    }

    public IEnumerable<TutorInitDto> CreateTutors(List<TutorInitDto> tuts, int matchingId)
    {
        var tutors = tuts.Select(info =>
            {
                TutorEntity tutor = new TutorEntity()
                {
                    MatchingId = matchingId,
                    IsReadyToStart = false,
                };

                return tutor;
            }
        ).ToList();

        saveRange(tutors);

        for (var i = 0; i < tuts.Count; i++) {
            tuts[i].TutorId = tutors[i].TutorId;
        }

        return tuts;
    }

    public IEnumerable<TutorChoiceDto> GetChoiceByTutor(int tutorId)
    {
        return _matchingSystemContext.TutorsChoices
            .Where(choice => choice.Project.TutorId == tutorId)
            .Select(entity => new TutorChoiceDto(entity))
            .ToList();
    }

    public IEnumerable<TutorChoiceDto> getChoicesByMatchingCurrentStage(int MatchingId)
    {
        return _matchingSystemContext.TutorsChoices
            .Where(choice => choice.Stage.MatchingId == MatchingId)
            .Select(entity => new TutorChoiceDto(entity))
            .ToList();
    }

    public int GetNotificationsCountByTutor(int tutorId)
    {
        return 0;
    }

    public void SetPreferences(IEnumerable<TutorChoice_1> newChoices, int tutorId)
    {
        using (var context = _matchingSystemContext)
        {
            var tutorChoicesToUpdate = context.TutorsChoices
                .Where(entity => newChoices.Any(val => val.ChoiceID == entity.ChoiceId))
                .ToList();
            foreach (var tutorsChoiceEntity in tutorChoicesToUpdate)
            {
                var newChoise = newChoices.First(choice1 => choice1.ChoiceID == tutorsChoiceEntity.ChoiceId);
                tutorsChoiceEntity.SortOrderNumber = newChoise.SortOrderNumber;
                tutorsChoiceEntity.IsInQuota = newChoise.IsInQuota;
            }

            context.UpdateRange(tutorChoicesToUpdate);
            context.SaveChanges();
        }
    }
    
    public IEnumerable<TutorView> GetTutorsViews()
    {
        return _matchingSystemContext.TutorsViews.ToList();
    }
}