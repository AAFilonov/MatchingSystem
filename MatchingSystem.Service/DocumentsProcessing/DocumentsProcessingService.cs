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
        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        int colCount = worksheet.Dimension.End.Column;  //get Column Count
        int rowCount = worksheet.Dimension.End.Row;     //get row count
        for (int row = 1; row <= rowCount; row++)
        {
            for (int col = 1; col <= colCount; col++)
            {
                Console.WriteLine(" Row:" + row + " column:" + col + " Value:" + worksheet.Cells[row, col].Value?.ToString().Trim());
            }
        }

        return null;
    }
    
    public ExcelPackage formStudentDataReport(List<StudentDto> students)
    
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage package = new ExcelPackage(); 
        
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Студенты");
        worksheet.Cells["A1"].Value = "Группа";
        worksheet.Cells["B1"].Value = "Имя";
        worksheet.Cells["C1"].Value = "Фамилия";
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