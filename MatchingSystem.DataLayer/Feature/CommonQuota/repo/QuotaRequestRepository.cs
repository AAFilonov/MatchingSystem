using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public class QuotaRequestRepository : GenericRepository<CommonQuotaEntity>, IQuotaRequestRepository
{
    public CommonQuotaEntity GetActiveTutorQuota(int tutorId)
    {
        return find(entity => entity.QuotaStateId == (int)CommonQuotaStateEnum.ACTIVE &&
                              entity.TutorId == tutorId).SingleOrDefault() ??
               throw new Exception("Не найдена квота для преподавателя tutorId=" + tutorId);
    }

    public IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int matchingId)
    {
        return CallInContext<IEnumerable<QuotaHistoryExecutive>>(context =>
        {
           
            var views = context.CommonQuotas
                .Include(quota => quota.QuotaState)
                .Include(quota => quota.Tutor)
                .ThenInclude(tutor => tutor.UsersRoles)
                .ThenInclude(usersRole => usersRole.User)
                .Include(quota => quota.Stage)
                .Where(entity => entity.Stage.MatchingId == matchingId)
                .OrderByDescending(entity => entity.CommonQuotaId)
                .ToList();
         
            return views.Select(entity => new QuotaHistoryExecutive
            {
                NameAbbreviation = entity.Tutor.UsersRoles.FirstOrDefault().User.NameAbbreviation,
                RequestedQuotaQty = entity.Qty,
                QuotaStateName_ru = entity.QuotaState.QuotaStateNameRu,
                StageTypeName_ru = entity.QuotaState.QuotaStateNameRu,
                Message = entity.Message,
                IterationNumber = entity.Stage.IterationNumber

            });
        });
    }
    
    public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId, int matchingId)
    {
        return CallInContext<IEnumerable<QuotaHistoryTutor>>(context =>
        {
            var views = context.commonQuotasViews.Where(entity =>
                entity.TutorId == tutorId &&
                entity.MatchingId == matchingId
            ).OrderByDescending(entity =>entity.CommonQuotaId )
                .ToList();
            return views.Select(entity => new QuotaHistoryTutor
            {
                CreateDate = entity.CreateDate,
                QuotaStateName_ru = entity.QuotaStateNameRu,
                StageTypeName_ru = entity.StageTypeNameRu,
                QuotaStateCode = entity.QuotaStateCode,
                Qty = entity.Qty
            });
        });
    }

    public IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId)
    {
        return CallInContext<IEnumerable<QuotaRequest>>(context =>
        {
            return find(entity => entity.QuotaStateId == (int)CommonQuotaStateEnum.REQUESTED)
                .Select(entity =>
                {
                    ActiveCommonQuotaView currentQuota = context.ActiveCommonQuotas.Where(view => view.TutorId == entity.TutorId).FirstOrDefault();
                    return new QuotaRequest
                    {
                        QuotaID = entity.CommonQuotaId,
                        TutorID = entity.Tutor.TutorId,
                        NameAbbreviation = entity.Tutor.UsersRoles.FirstOrDefault().User.NameAbbreviation,
                        RequestedQuotaQty = (short)entity.Qty,
                        Message = entity.Message,
                        CurrentQuotaQty = (int)currentQuota.Qty
                    };
                })
                .ToList();
        });
    }
}