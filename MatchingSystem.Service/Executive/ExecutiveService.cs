using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.UI.RequestModels;

namespace Service.Executive
{
    internal class ExecutiveService
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IExecutiveRepository executiveRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IProjectRepository projectRepository;

        public ExecutiveService(
            ITutorRepository tutorRepository,
            IExecutiveRepository executiveRepository,
            IMatchingRepository matchingRepository,
            IProjectRepository projectRepository
            )
        {
            this.tutorRepository = tutorRepository;
            this.executiveRepository = executiveRepository;
            this.matchingRepository = matchingRepository;
            this.projectRepository = projectRepository;
        }

        public void SetNextStage([FromQuery] int? matchingId, [FromQuery] int? userId)
        {
            var quotaRequest = executiveRepository.GetQuotaRequestsByExecutive(userId.Value, matchingId.Value);

            matchingRepository.SetNextStage(matchingId.Value);
        }

        public IActionResult GetAllocationByExecutive([FromQuery] int? userId, [FromQuery] int? matchingId)
        {
            var model = new AdjustmentData()
            {
                Allocations = executiveRepository.GetAllocationsByExecutive(userId.Value, matchingId.Value),
                Tutors = tutorRepository.GetTutorsByMatching(matchingId.Value),
                Projects = new List<Project>()
            };

            var tutorIds = model.Tutors.Select(x => x.TutorID).ToList();

            foreach (var tutorId in tutorIds)
            {
                var temp = projectRepository.GetProjectsByTutor(tutorId).ToList();
                temp.ForEach(x => x.TutorID = tutorId);
                model.Projects.AddRange(temp);
            }

            return new JsonResult(model);
        }

        public void SetAllocationByExecutive([FromBody] AdjustmentRequest request)
        {
            foreach (var student in request.Allocations)
            {
                var inParams = new AdjustmentParams()
                {
                    MatchingId = request.MatchingID.Value,
                    ProjectId = student.ProjectID.Value,
                    StudentId = student.StudentID.Value,
                    UserId = request.UserID.Value
                };
                executiveRepository.SetAdjustmentByExecutive(inParams);
            }
        }
    }
}
