using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.UI.RequestModels;

namespace Service.Quotas
{
    internal class QuotasService
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IExecutiveRepository executiveRepository;

        public QuotasService(
            ITutorRepository tutorRepository,
            IProjectRepository projectRepository,
            IExecutiveRepository executiveRepository
        )
        {
            this.tutorRepository = tutorRepository;
            this.projectRepository = projectRepository;
            this.executiveRepository = executiveRepository;
        }

        public IActionResult Initialize(int tutorId, int stageTypeCode)
        {
            var model = new TutorQuotaData
            {
                History = tutorRepository.GetQuotaRequestHistoryByTutor(tutorId),
                CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId),
            };
            if (stageTypeCode == 4)
            {
                model.Projects = projectRepository.GetProjectsByTutor(tutorId);
            }

            return new JsonResult(model);
        }

        public void CreateRequest([FromBody] ChangeQuotaRequest data)
        {
            tutorRepository.CreateCommonQuotaRequestForSecondStage(data.TutorId, data.NewQuotaQty, data.Message);
        }

        public void CreateRequestForLastStage([FromBody] ChangeQuotaRequest data)
        {
            var request = new CreateCommonQuotaParams
            {
                Message = data.Message,
                NewQuota = data.NewQuotaQty,
                TutorId = data.TutorId
            };
            request.FillProjectQuota(data.Deltas);

            tutorRepository.CreateCommonQuotaRequestForLastStage(request);
        }


        public void AcceptRequest(int quotaid,string action)
        {
            executiveRepository.AcceptQuotaRequest(quotaid, action == "accept");
        }
    }
}
