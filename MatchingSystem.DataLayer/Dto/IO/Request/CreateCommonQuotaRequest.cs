using System.Collections.Generic;
using System.Data;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto.IO.Request
{
    public class CreateCommonQuotaRequest
    {
        public int TutorId { get; set; }
        public int NewQuota { get; set; }
        public string Message { get; set; }
        public DataTable Table { get; private set; }

        public void FillProjectQuota(IEnumerable<ProjectQuota> args)
        {
            var table = new DataTable();
            table.Columns.Add("ProjectID", typeof(int));
            table.Columns.Add("Quota", typeof(short));

            foreach (var projectQuota in args)
            {
                var row = table.NewRow();
                row.SetField("ProjectID", projectQuota.ProjectID);
                row.SetField("Quota", projectQuota.Quota);
                table.Rows.Add(row);
            }
            Table = table;
        }
    }
}
