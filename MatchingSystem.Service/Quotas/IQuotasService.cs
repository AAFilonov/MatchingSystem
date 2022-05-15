using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.UI.RequestModels;

namespace Service.Quotas
{
    internal interface IQuotasService
    {
        public IActionResult Initialize(int tutorId, int stageTypeCode);

        public void CreateRequest([FromBody] ChangeQuotaRequest data);

        public void CreateRequestForLastStage([FromBody] ChangeQuotaRequest data);

        public void AcceptRequest(int quotaid, string action);

    }
}
