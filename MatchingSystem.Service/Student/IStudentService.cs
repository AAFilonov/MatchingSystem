using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace Service.Student
{
    internal interface IStudentService
    {
        public IActionResult GetSelectedParams(int studentId);

        public IActionResult EditProfile();

        public IActionResult GetProjects([FromQuery] int studentId);

        public IActionResult SetPreferences();

        public IActionResult GetStudentInfo([FromQuery] int? studentId);
        public IActionResult GetAllocatedProject([FromQuery] int? studentId);

    }
}
