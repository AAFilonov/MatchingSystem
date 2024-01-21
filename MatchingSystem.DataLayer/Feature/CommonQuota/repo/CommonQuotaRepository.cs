using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public class CommonQuotaRepository : GenericRepository<CommonQuotaEntity>, ICommonQuotaRepository
{
    public void CreateCommonQuotas(List<TutorInitDto> tuts, int stageId,
        DiplomaMatchingContext? extContext = null)
    {
        var quotas = tuts.Select(info =>
        {
            var commonQuota = new CommonQuotaEntity
            {
                TutorId = info.TutorId,
                Qty = (short)info.quota,
                StageId = stageId,
                CreateDate = DateTime.Now,
                IsNotification = false,
                QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE
            };

            return commonQuota;
        });
        saveRange(quotas, extContext);
    }

    public CommonQuotaEntity GetActiveTutorQuota(int tutorId, int MatchingId)
    {
        return find(entity => entity.Stage.MatchingId == MatchingId &&
                              entity.QuotaStateId == (int)CommonQuotaStateEnum.ACTIVE &&
                       entity.TutorId == tutorId).SingleOrDefault() ?? throw new Exception("Не найдена квота для преподавателя tutorId="+tutorId);
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

    public List<CommonQuotaEntity> GetNotificationsByTutor(int tutorId)
    {
      return find(entity => entity.Tutor.TutorId == tutorId && entity.IsNotification == true).ToList();
    }
    
    public List<CommonQuotaEntity> GetNotificationsByExecutive( int matchingId)
    {
        return find(entity =>
                entity.Stage.MatchingId == matchingId &&
                entity.IsNotification == true &&
                entity.QuotaStateId == (int)CommonQuotaStateEnum.REQUESTED
            ).ToList();
    }
}