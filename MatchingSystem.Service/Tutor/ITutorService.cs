using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Entities;

namespace Service.Tutor
{
    internal interface ITutorService
    {
        public IActionResult GetChoice(int tutorId);

        public void SetReady([FromQuery] int tutorId);

        public void SaveChoice([FromBody] List<TutorChoice_1> data, [FromQuery] int tutorId);
    }
}
