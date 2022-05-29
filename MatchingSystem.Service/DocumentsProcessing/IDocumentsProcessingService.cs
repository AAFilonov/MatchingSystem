using System.Collections.Generic;
using System.IO;
using MatchingSystem.DataLayer.Dto;
using OfficeOpenXml;

namespace MatchingSystem.Service;

public interface IDocumentsProcessingService
{
    public ExcelPackage formStudentDataReport(List<StudentDto> students);
    public List<StudentDto> parseStudentData(ExcelPackage package);
} 