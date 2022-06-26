using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Interface
{
    public interface ITutorRepository
    {
        Task<bool> GetReadyByTutorAsync(int tutorId);
        bool GetReadyByTutor(int tutorId);
        Task<int> GetTutorIdAsync(int userId, int matchingId);
        int GetTutorId(int userId, int matchingId);
        Task<IEnumerable<Tutor>> GetTutorsByMatchingAsync(int matchingId);
        IEnumerable<Tutor> GetTutorsByMatching(int matchingId);

        IEnumerable<TutorFullDTO> GetFullInfoTutorByMatching(int matchingId);
        Task<IEnumerable<Group>> GetGroupsByTutorAsync(int tutorId);
        void SetCommonQuotasForTutors(List<TutorInitDto> tuts, int stageId);
        IEnumerable<TutorInitDto> CreateTutors(List<TutorInitDto> tuts, int matchingId);
         //Вынести в репозиторий ролей
        void AssignTutorRole(List<TutorInitDto> tuts, int matchingID);
        IEnumerable<Group> GetGroupsByTutor(int tutorId);
        Task<IEnumerable<TutorChoice>> GetChoiceByTutorAsync(int tutorId);
        IEnumerable<TutorChoice> GetChoiceByTutor(int tutorId);
        IEnumerable<TutorChoice> getChoicesByMatchingCurrentStage(int MatchingId);
        public IEnumerable<TutorChoice> getChoicesByMatching(int MatchingId);
        Task<int> GetCommonQuotaByTutorAsync(int tutorId);
        int GetCommonQuotaByTutor(int tutorId);
        Task<IEnumerable<QuotaHistoryTutor>> GetQuotaRequestHistoryByTutorAsync(int tutorId);
        IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId);
        IEnumerable<Tutor> GetAllTutors();
        
        /// <summary>
        /// Создает запрос на увеличение квоты к ответственному на втором этапе распределения
        /// </summary>
        /// <param name="tutorId">Идентификатор препода</param>
        /// <param name="quota">Новое значение квоты</param>
        /// <param name="message">Сообщение для ответственного</param>
        /// <returns>Ничего не возвращает</returns>
        Task CreateCommonQuotaRequestForSecondStageAsync(int tutorId, int quota, string message);
        void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message);
        /// <summary>
        /// Создает запрос на увеличение квоты к ответственному на втором этапе распределения
        /// </summary>
        /// <param name="tutorId">Идентификатор препода</param>
        /// <param name="quota">Новое значение квоты</param>
        /// <param name="message">Сообщение для ответственного</param>
        /// <returns>Ничего не возвращает</returns>
        Task CreateCommonQuotaRequestForThirdStageAsync(int tutorId, int quota, string message);
        void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message);
        //prev: CreateCommonQuotaRequestForIterationsAsync
        Task CreateCommonQuotaRequestForLastStageAsync(CreateCommonQuotaParams @params);
        void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params);
        Task<int> GetNotificationsCountByTutorAsync(int tutorId);
        int GetNotificationsCountByTutor(int tutorId);
        Task SetReadyAsync(int tutorId);
        void SetReady(int tutorId);
        void SetPreferences(IEnumerable<TutorChoice_1> choices, int tutorId);
        Task SetPreferencesAsync(IEnumerable<TutorChoice_1> choices, int tutorId);
    }
}
