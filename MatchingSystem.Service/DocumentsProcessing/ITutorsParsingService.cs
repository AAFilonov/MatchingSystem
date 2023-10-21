using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface ITutorsParsingService
{
    public List<GroupInitDto> parseTutorGroupData(ExcelPackage package);
    public List<TutorInitDto> tryToParseTutorsData(ExcelPackage package);
}