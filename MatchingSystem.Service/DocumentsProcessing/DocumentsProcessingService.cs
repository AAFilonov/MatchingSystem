using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.Service.Exception;
using OfficeOpenXml;

namespace MatchingSystem.Service.DocumentsProcessing;

public class DocumentsProcessingService : IDocumentsProcessingService
{
    public ExcelPackage formStudentDataReport(List<StudentInitDto> students)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Студенты");
        worksheet.Cells["A1"].Value = "Группа";
        worksheet.Cells["B1"].Value = "Фамилия";
        worksheet.Cells["C1"].Value = "Имя";
        worksheet.Cells["D1"].Value = "Отчество";
        worksheet.Cells["E1"].Value = "Логин";
        worksheet.Cells["F1"].Value = "Пароль";

        for (var index = 0; index < students.Count; index++)
        {
            var student = students[index];
            var rowIndex = index + 2;
            worksheet.Cells["A" + rowIndex].Value = student.groupName;
            worksheet.Cells["B" + rowIndex].Value = student.firstName;
            worksheet.Cells["C" + rowIndex].Value = student.lastName;
            worksheet.Cells["D" + rowIndex].Value = student.middleName;
            worksheet.Cells["E" + rowIndex].Value = student.login;
            worksheet.Cells["F" + rowIndex].Value = student.password;
        }

        return package;
    }
}