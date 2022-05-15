using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.UI.RequestModels;

namespace Service.Projects
{
    internal interface IProjectsService
    {
        public IActionResult AddTutorProject([FromForm] ProjectRequest project);

        public void EditProject([FromForm] ProjectRequest project);

        public void DeleteProject([FromQuery] int projectId);

        public IActionResult GetProjectsData([FromQuery] int tutorId);

        public void EditQuota(int projectId, int quota, int tutorId, int matchingid);

        public void CloseProject(int tutorId, int projectId);

        public IActionResult GetProjectsByTutor([FromQuery] int tutorId);
    }
}
