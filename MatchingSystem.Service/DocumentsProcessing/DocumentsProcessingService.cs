using System;
using System.Collections.Generic;
using System.IO;
using MatchingSystem.DataLayer.Dto;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public class DocumentsProcessingService : IDocumentsProcessingService
{
    public void parseStudentDataFile()
    {
        var package = new ExcelPackage();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public List<StudentDto> parseStudentData(ExcelPackage package)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<StudentDto> dtos = new List<StudentDto>();
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            var groupName = worksheet.Name;
            
            int rowCount = worksheet.Dimension.End.Row; //get row count
            for (int row = 1; row <= rowCount; row++)
            {
                StudentDto dto = new StudentDto();
                dto.groupName = groupName;
                dto.lastName = (string)worksheet.Cells["A"+row].Value;
                dto.firstName = (string)worksheet.Cells["B"+row].Value;
                dto.middleName = (string)worksheet.Cells["C"+row].Value;
              
                dtos.Add(dto);
            }
        }

        return dtos;
    }

    public List<GroupTutorDto> parseGroupData(ExcelPackage package)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<GroupTutorDto> dtos = new List<GroupTutorDto>();
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                GroupTutorDto group = new GroupTutorDto
                {
                    name = worksheet.Name,
                    value = false
                };
                dtos.Add(group);

            }

            return dtos;
        }
    
    
    public ExcelPackage formStudentDataReport(List<StudentDto> students)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage package = new ExcelPackage();
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Студенты");
        worksheet.Cells["A1"].Value = "Группа";
        worksheet.Cells["B1"].Value = "Фамилия";
        worksheet.Cells["C1"].Value = "Имя";
        worksheet.Cells["D1"].Value = "Отчество";
        worksheet.Cells["E1"].Value = "Пароль";

        for (var index = 0; index < students.Count; index++)
        {
            var student = students[index];
            int rowIndex = index + 2;
            worksheet.Cells["A" + rowIndex].Value = student.groupName;
            worksheet.Cells["B" + rowIndex].Value = student.firstName;
            worksheet.Cells["C" + rowIndex].Value = student.lastName;
            worksheet.Cells["D" + rowIndex].Value = student.middleName;
            worksheet.Cells["E" + rowIndex].Value = student.password;
        }
        return package;
    }
}