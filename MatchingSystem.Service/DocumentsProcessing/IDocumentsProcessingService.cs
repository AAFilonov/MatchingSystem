using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface IDocumentsProcessingService
{
    public ExcelPackage formStudentDataReport(List<StudentInitDto> students);
}