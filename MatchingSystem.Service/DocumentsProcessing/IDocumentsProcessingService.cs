using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface IDocumentsProcessingService
{
    public ExcelPackage formStudentDataReport(List<StudentInitDto> students);
    public List<StudentInitDto> parseStudentData(ExcelPackage package);
    public List<GroupInitDto> parseGroupData(ExcelPackage package);
} 