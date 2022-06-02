using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public interface IDocumentsProcessingService
{
    public ExcelPackage formStudentDataReport(List<StudentDto> students);
    public List<StudentDto> parseStudentData(ExcelPackage package);
    public List<GroupTutorDto> parseGroupData(ExcelPackage package);
} 