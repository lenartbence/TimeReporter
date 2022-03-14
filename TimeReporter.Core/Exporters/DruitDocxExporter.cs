using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters
{
    internal class DruitDocxExporter : AbstractExporter
    {
        private string _month = "{month}";
        private string _start = "{start}";
        private string _end = "{end}";
        private string _hours = "{hours}";

        public override string Name => "DruIT docx";

        public override void Export(List<Day> days)
        {
            base.Export(days);

            var dayMap = days.ToDictionary(x => x.Date.Day);

            // TODO: Whole thing could be simpler if the template contained numbered placeholders.
            // Then it can be solved by string replace insead of table parsing.
            try
            {
                string outputDirectory = Path.Join(Path.GetDirectoryName(TemplatePath), Name);
                string outputPath = Path.Join(outputDirectory, $"{days.First().Date:yyyy-MM}.docx");

                Directory.CreateDirectory(outputDirectory);
                File.Copy(TemplatePath, outputPath, true);

                using (WordprocessingDocument doc = WordprocessingDocument.Open(outputPath, true))
                {
                    HandleTables(dayMap, doc);
                }

                Message = "Export done.";
            }
            catch (Exception ex)
            {
                Message = "Error during export";
                // TODO: Log
            }
        }

        private void HandleTables(Dictionary<int, Day> dayMap, WordprocessingDocument doc)
        {
            IEnumerable<Table> tables = doc.MainDocumentPart.Document.Body.Elements<Table>();
            foreach (var table in tables)
            {
                var rows = table.Elements<TableRow>();
                HandleRows(dayMap, rows);
            }
        }

        private void HandleRows(Dictionary<int, Day> dayMap, IEnumerable<TableRow> rows)
        {
            int day = 0;
            foreach (var row in rows)
            {
                var cells = row.Elements<TableCell>();
                if (row.InnerText.Contains(_start))
                {
                    day = int.Parse(cells.First().InnerText);
                }

                HandleCells(dayMap, day, cells);
            }
        }

        private void HandleCells(Dictionary<int, Day> dayMap, int day, IEnumerable<TableCell> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.InnerText.Contains(_month))
                {
                    ChangeCellText(cell, _month, dayMap.Values.First().Date.Month.ToString("00"));
                    continue;
                }

                if (day == 0)
                    continue;

                UpdateDayCellText(cell, day, dayMap[day].Type);
            }
        }

        private void UpdateDayCellText(TableCell cell, int dayNumber, DayType dayType)
        {
            // TODO: Mark Saturday workdays as DayOffs

            string start = string.Empty, end = string.Empty, hours = string.Empty;

            switch (dayType)
            {
                case DayType.Work:
                    {
                        start = "9:00";
                        end = "17:00";
                        hours = "8";
                        break;
                    }
                case DayType.DayOff:
                    {
                        start = "szabi";
                        end = string.Empty;
                        hours = "0";
                        break;
                    }
                case DayType.NationalHoliday:
                    {
                        start = "szün.";
                        end = string.Empty;
                        hours = "0";
                        break;
                    }
                case DayType.Weekend:
                    {
                        start = string.Empty;
                        end = string.Empty;
                        hours = "0";
                        break;
                    }
            }

            var texts = new Dictionary<string, string>()
            {
                [_start] = start,
                [_end] = end,
                [_hours] = hours
            };

            if (texts.TryGetValue(cell.InnerText.Trim(), out string result))
            {
                OverwriteCellText(cell, result);
            }
        }

        private static void OverwriteCellText(TableCell cell, string content)
        {
            cell.Elements<Paragraph>().First()
                .Elements<Run>().First()
                .Elements<Text>().First()
                .Text = content;
        }

        private static void ChangeCellText(TableCell cell, string from, string to)
        {
            foreach (var p in cell.Elements<Paragraph>())
            {
                foreach (var r in p.Elements<Run>())
                {
                    foreach (var t in r.Elements<Text>())
                    {
                        t.Text = t.Text.Replace(from, to);
                    }
                }
            }
        }
    }
}
