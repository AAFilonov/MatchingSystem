using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface ITutorsParsingService
{
    public List<GroupInitDto> parseTutorGroupData(ExcelPackage package);
    public  List<TutorInitDto> tryToParseTutorsData(ExcelPackage package);
} 