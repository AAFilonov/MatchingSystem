using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface IStudentsParsingService
{
    public List<StudentInitDto> tryToParseStudentData(ExcelPackage package);
    public List<GroupInitDto> parseStudentGroupData(ExcelPackage package);
}