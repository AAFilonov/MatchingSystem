using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature._Interface
{
    public interface ITutorRepository
    {
        bool GetReadyByTutor(int tutorId);
        int GetTutorId(int userId, int matchingId);
        IEnumerable<TutorDto> GetTutorsByMatching(int matchingId);
        void SetCommonQuotasForTutors(List<TutorInitDto> tuts, int stageId);
        IEnumerable<TutorInitDto> CreateTutors(List<TutorInitDto> tuts, int matchingId);
         //Вынести в репозиторий ролей
        void AssignTutorRole(List<TutorInitDto> tuts, int matchingID);
        IEnumerable<GroupDto> GetGroupsByTutor(int tutorId);
        Task<IEnumerable<TutorChoice>> GetChoiceByTutorAsync(int tutorId);
        IEnumerable<TutorChoice> GetChoiceByTutor(int tutorId);
        IEnumerable<TutorChoice> getChoicesByMatchingCurrentStage(int MatchingId);
        public IEnumerable<TutorChoice> getChoicesByMatching(int MatchingId);
        int GetCommonQuotaByTutor(int tutorId);
        IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId);
        IEnumerable<TutorDto> GetAllTutors();
        
        /// <summary>
        /// Создает запрос на увеличение квоты к ответственному на втором этапе распределения
        /// </summary>
        /// <param name="tutorId">Идентификатор препода</param>
        /// <param name="quota">Новое значение квоты</param>
        /// <param name="message">Сообщение для ответственного</param>
        /// <returns>Ничего не возвращает</returns>
        void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message);
        /// <summary>
        /// Создает запрос на увеличение квоты к ответственному на втором этапе распределения
        /// </summary>
        /// <param name="tutorId">Идентификатор препода</param>
        /// <param name="quota">Новое значение квоты</param>
        /// <param name="message">Сообщение для ответственного</param>
        /// <returns>Ничего не возвращает</returns>
        void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message);
        //prev: CreateCommonQuotaRequestForIterationsAsync
        void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params);
        int GetNotificationsCountByTutor(int tutorId);
        void SetReady(int tutorId);
        void SetPreferences(IEnumerable<TutorChoice_1> choices, int tutorId);
    }
}
