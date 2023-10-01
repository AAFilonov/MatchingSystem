using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

public class TutorRepository : Repository<Tutor>, ITutorRepository
{
    public TutorRepository(DbContext context) : base(context)
    {
    }

    public void AssignTutorRole(List<TutorInitDto> tuts, int matchingID)
    {
        var tutors = tuts.Select(info =>
        {
            UsersRole role = new UsersRole()
            {
                UserId = info.UserId,
                TutorId = info.UserId,
                MatchingId = matchingID,
                RoleId = 1,
            };
            return role;
        });
        _matchingSystemContext.UsersRoles.AddRange(tutors);
    }

    public void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params)
    {
        throw new System.NotImplementedException();
    }

    public void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message)
    {
        throw new System.NotImplementedException();
    }

    public void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorInitDto> CreateTutors(List<TutorInitDto> tuts, int matchingId)
    {
        var tutors = tuts.Select(info =>
        {
            Tutor tutor = new Tutor()
            {
                MatchingId = matchingId,
                IsReadyToStart = false,
            };

            return tutor;
        }
        ).ToList();

        saveAll(tutors);

        for (var i = 0; i < tuts.Count; i++) {
            tuts[i].TutorId = tutors[i].TutorId;
        }

        return tuts;
    }

    public IEnumerable<TutorDto> GetAllTutors()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorChoice> GetChoiceByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorChoice> getChoicesByMatching(int MatchingId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorChoice> getChoicesByMatchingCurrentStage(int MatchingId)
    {
        throw new System.NotImplementedException();
    }

    public int GetCommonQuotaByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorFullDTO> GetFullInfoTutorByMatching(int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<MatchingSystem.DataLayer.OldEntities.Group> GetGroupsByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public int GetNotificationsCountByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public bool GetReadyByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public int GetTutorId(int userId, int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TutorDto> GetTutorsByMatching(int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public void SetCommonQuotasForTutors(List<TutorInitDto> tuts, int stageId)
    {
        var quotas = tuts.Select(info =>
        {
            var commonQuota = new CommonQuota()
            {
                TutorId = info.TutorId,
                Qty = (short)info.quota,
                StageId = stageId,
            };

            return commonQuota;
        });

        _matchingSystemContext.CommonQuotas.AddRange(quotas);
    }

    public void SetPreferences(IEnumerable<TutorChoice_1> choices, int tutorId)
    {
        throw new System.NotImplementedException();
    }

    public void SetReady(int tutorId)
    {
        throw new System.NotImplementedException();
    }
}