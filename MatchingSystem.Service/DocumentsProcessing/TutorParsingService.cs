using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.Service.Exception;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public class TutorParsingService : ITutorsParsingService
{
    private const int TUTOR_GROUP_SINCE_COLUMN_INDEX = 3;
    private const int TUTOR_GROUP_SINCE_ROW_INDEX = 2;
    
    public List<GroupInitDto> parseTutorGroupData(ExcelPackage package)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<GroupInitDto> dtos = new List<GroupInitDto>();

        var worksheet = package.Workbook.Worksheets[0];
        int totalCols = worksheet.Dimension.End.Column;
        var range = worksheet.Cells[1, 1, 1, totalCols];
        for (int i = TUTOR_GROUP_SINCE_COLUMN_INDEX; i < totalCols; i++)
        {
            var cell = (string)range[1, i].Value;
            dtos.Add(new GroupInitDto() { name = cell, value = false });
        }

        return dtos;
    }

    public List<TutorInitDto> tryToParseTutorsData(ExcelPackage package)
    {
        try
        {
            return parseTutors(package);
        }
        catch (System.Exception e)
        {
            throw new InputDataException("Parsing error: invalid table format", e);
        }
    }
    
    public List<TutorInitDto> parseTutors(ExcelPackage package)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<TutorInitDto> dtos = new List<TutorInitDto>();
        
        var worksheet = package.Workbook.Worksheets[0];
        

        int rowCount = worksheet.Dimension.End.Row; 
        int colCount = worksheet.Dimension.End.Column; 
        var range = worksheet.Cells[1, 1, rowCount, colCount];
        
        for (int row = TUTOR_GROUP_SINCE_ROW_INDEX; row < rowCount; row++)
        {
            var tutorNameAbbreviation = (string)range[row, 1].Value;
            if ( tutorNameAbbreviation == null) break;
            
            var tutor = fillTutorDto(range, row, colCount);

            dtos.Add(tutor);
        }

        return dtos;
    }

    private static TutorInitDto fillTutorDto(ExcelRange range, int row, int colCount)
    {
        var tutor = new TutorInitDto();
        
        tutor.nameAbbreviation = (string)range[row, 1].Value;
        tutor.quota = Convert.ToInt32((Double)range[row, 2].Value);
        tutor.groups = new List<GroupInitDto>();

        for (var col = TUTOR_GROUP_SINCE_COLUMN_INDEX; col <= colCount; col++)
        {
            int tutorGroupCell = Convert.ToInt32((Double)range[row, col].Value);
            var groupName = (string)range[1, col].Value;
            var groupDto = new GroupInitDto() { name = groupName, value = tutorGroupCell == 1 };
            tutor.groups.Add(groupDto);
        }

        tutor.isIncluded = tutor.groups.Select(dto => dto.value).Count(value => value) > 0 && tutor.quota > 0;
        return tutor;
    }

    public ExcelPackage formStudentDataReport(List<StudentInitDto> students)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Студенты");
        worksheet.Cells["A1"].Value = "Группа";
        worksheet.Cells["B1"].Value = "Фамилия";
        worksheet.Cells["C1"].Value = "Имя";
        worksheet.Cells["D1"].Value = "Отчество";
        worksheet.Cells["E1"].Value = "Пароль";

        for (var index = 0; index < students.Count; index++)
        {
            var student = students[index];
            var rowIndex = index + 2;
            worksheet.Cells["A" + rowIndex].Value = student.groupName;
            worksheet.Cells["B" + rowIndex].Value = student.firstName;
            worksheet.Cells["C" + rowIndex].Value = student.lastName;
            worksheet.Cells["D" + rowIndex].Value = student.middleName;
            worksheet.Cells["E" + rowIndex].Value = student.password;
        }

        return package;
    }
}