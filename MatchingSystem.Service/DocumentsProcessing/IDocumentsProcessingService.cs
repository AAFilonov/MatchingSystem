using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface IDocumentsProcessingService
{
    public ExcelPackage formStudentDataReport(List<StudentInitDto> students);
} 