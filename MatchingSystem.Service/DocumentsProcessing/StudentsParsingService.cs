using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.Service.Exception;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public class StudentsParsingService : IStudentsParsingService
{
    public List<StudentInitDto> tryToParseStudentData(ExcelPackage package)
    {
        try
        {
            return parseStudents(package);
        }
        catch (System.Exception e)
        {
            throw new InputDataException("Parsing error: invalid table format", e);
        }
    }

    private List<StudentInitDto> parseStudents(ExcelPackage package)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var dtos = new List<StudentInitDto>();
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            var groupName = worksheet.Name;

            var rowCount = worksheet.Dimension.End.Row;
            for (var row = 1; row <= rowCount; row++)
            {
                StudentInitDto initDto = new StudentInitDto
                {
                    groupName = groupName,
                    lastName = (string)worksheet.Cells["A" + row].Value,
                    firstName = (string)worksheet.Cells["B" + row].Value,
                    middleName = (string)worksheet.Cells["C" + row].Value
                };

                dtos.Add(initDto);
            }
        }

        return dtos;
    }

    public List<GroupInitDto> parseStudentGroupData(ExcelPackage package)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        return package.Workbook.Worksheets
            .Select(worksheet => new GroupInitDto { name = worksheet.Name, value = false }).ToList();
    }
}