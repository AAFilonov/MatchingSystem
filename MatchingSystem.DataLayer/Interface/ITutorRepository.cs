using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Request;
using MatchingSystem.DataLayer.Repository;

namespace MatchingSystem.DataLayer.Interface
{
    public interface ITutorRepository
    {
        Task<bool> GetReadyByTutor(int tutorId);
        Task<int> GetTutorIdAsync(int userId, int matchingId);
        Task<IEnumerable<Tutor>> GetTutorsByMatchingAsync(int matchingId);
        Task<IEnumerable<Group>> GetGroupsByTutorAsync(int tutorId);
        Task<IEnumerable<TutorChoice>> GetChoiceByTutorAsync(int tutorId);
        //prev: GetCommonQuotaAsync
        Task<int> GetCommonQuotaByTutorAsync(int tutorId);
        //prev: GetTutorQuotaHistoryAsync
        Task<IEnumerable<QuotaHistoryTutor>> GetQuotaRequestHistoryByTutorAsync(int tutorId);
        
        /// <summary>
        /// Создает запрос на увеличение квоты к ответственному на втором этапе распределения
        /// </summary>
        /// <param name="tutorId">Идентификатор препода</param>
        /// <param name="quota">Новое значение квоты</param>
        /// <param name="message">Сообщение для ответственного</param>
        /// <returns>Ничего не возвращает</returns>
        Task CreateCommonQuotaRequestForSecondStageAsync(int tutorId, int quota, string message);
        /// <summary>
        /// Создает запрос на увеличение квоты к ответственному на втором этапе распределения
        /// </summary>
        /// <param name="tutorId">Идентификатор препода</param>
        /// <param name="quota">Новое значение квоты</param>
        /// <param name="message">Сообщение для ответственного</param>
        /// <returns>Ничего не возвращает</returns>
        Task CreateCommonQuotaRequestForThirdStageAsync();
        //prev: CreateCommonQuotaRequestForIterationsAsync
        Task CreateCommonQuotaRequestForLastStageAsync(CreateCommonQuotaRequest request);
        Task<int> GetNotificationsCountByTutor(int tutorId);
    }
}
