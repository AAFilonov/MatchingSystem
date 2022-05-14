using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace Service.Tutor
{
    internal class TutorService
    {
        private readonly ITutorRepository tutorRepository;

        public TutorService(ITutorRepository tutorRepository)
        {
            this.tutorRepository = tutorRepository;
        }

        public IActionResult GetChoice(int tutorId)
        {
            var result = tutorRepository.GetChoiceByTutor(tutorId).ToList();

            var projects = result.Select(x => x.ProjectID).Distinct();

            var viewModel = new IterationData
            {
                CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId)
            };


            foreach (var proj in projects)
            {
                viewModel.ChoiceDatas.Add(new TutorChoiceData
                {
                    ProjectID = proj,
                    ProjectName = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectName,
                    Qty = result.FirstOrDefault(x => x.ProjectID == proj)?.Qty,
                    Choices = result.Where(x => x.ProjectID == proj).OrderBy(t => t.SortOrderNumber).ToList(),
                    ProjectIsClosed = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectIsClosed
                });
            }

            return new JsonResult(viewModel);
        }

        public async void SetReady([FromQuery] int tutorId)
        {
                await tutorRepository.SetReadyAsync(tutorId);
        }

        public void SaveChoice([FromBody] List<TutorChoice_1> data, [FromQuery] int tutorId)
        {
                tutorRepository.SetPreferences(data, tutorId);
        }
    }
}
