using System;
using System.Collections.Generic;
using System.IO;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public class DocumentsProcessingService : IDocumentsProcessingService
{
    public void parseStudentDataFile()
    {
        var package = new ExcelPackage();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public List<StudentInitDto> parseStudentData(ExcelPackage package)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<StudentInitDto> dtos = new List<StudentInitDto>();
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            var groupName = worksheet.Name;
            
            int rowCount = worksheet.Dimension.End.Row; //get row count
            for (int row = 1; row <= rowCount; row++)
            {
                StudentInitDto initDto = new StudentInitDto();
                initDto.groupName = groupName;
                initDto.lastName = (string)worksheet.Cells["A"+row].Value;
                initDto.firstName = (string)worksheet.Cells["B"+row].Value;
                initDto.middleName = (string)worksheet.Cells["C"+row].Value;
              
                dtos.Add(initDto);
            }
        }

        return dtos;
    }

    public List<GroupInitDto> parseGroupData(ExcelPackage package)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<GroupInitDto> dtos = new List<GroupInitDto>();
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                GroupInitDto group = new GroupInitDto
                {
                    name = worksheet.Name,
                    value = false
                };
                dtos.Add(group);

            }

            return dtos;
        }
    
    
    public ExcelPackage formStudentDataReport(List<StudentInitDto> students)
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