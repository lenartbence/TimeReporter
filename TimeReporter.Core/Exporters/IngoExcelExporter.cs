using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters
{
    internal class IngoExcelExporter : AbstractExporter
    {
        public override string Name => "Ingo Excel";

        public override void Export(List<Day> days)
        {
            base.Export(days);

            try
            {
                DateTime targetDate = days.First().Date;

                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(TemplatePath, true))
                {
                    string monthName = targetDate.ToString("MMMM", CultureInfo.InvariantCulture);
                    Worksheet worksheet = GetWorksheetPartByName(doc, monthName);

                    if (worksheet == null)
                    {
                        Message = $"Could not find sheet for \"{monthName}\"";
                        return;
                    }

                    doc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                    doc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;

                    ProcessSheet(worksheet, days);
                    doc.Save();

                    Message = "Export done.";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private void ProcessSheet(Worksheet worksheet, List<Day> days)
        {
            int firstDateCellIndex = 15;
            var rows = worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(x => x.RowIndex >= firstDateCellIndex).ToList();

            for (int i = 0; i < days.Count(); i++)
            {
                Cell hoursCell = rows[i].Elements<Cell>().FirstOrDefault(x => x.CellReference == $"C{firstDateCellIndex + i}");
                Cell noteCell = rows[i].Elements<Cell>().FirstOrDefault(x => x.CellReference == $"D{firstDateCellIndex + i}");

                int hours;
                string note;
                if (days[i].Type == DayType.Work)
                {
                    hours = 8;
                    note = days[i].Project;
                }
                else
                {
                    hours = 0;
                    note = string.Empty;
                }

                hoursCell.CellValue = new CellValue(hours);
                noteCell.CellValue = new CellValue(note);

                hoursCell.DataType = new EnumValue<CellValues>(CellValues.Number);
                noteCell.DataType = new EnumValue<CellValues>(CellValues.String);
            }

            worksheet.Save();
        }

        private Worksheet GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            Sheet sheet = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().FirstOrDefault(s => s.Name == sheetName);

            if (sheet == null)
            {
                return null;
            }

            string relationshipId = sheet.Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart.Worksheet;
        }
    }
}
