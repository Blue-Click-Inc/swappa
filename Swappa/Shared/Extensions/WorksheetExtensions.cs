using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Swappa.Shared.Extensions
{
    public static class WorksheetExtensions
    {
        public static void SetHeaderValues(this ExcelWorksheet worksheet, 
            Dictionary<string, string> titles, int startRowNum, bool setColumnAutoFit = false)
        {
            foreach (var title in titles)
            {
                worksheet.Cells[title.Key].Value = title.Value;

                if (setColumnAutoFit)
                {
                    worksheet.Column(startRowNum).AutoFit();
                }

                startRowNum++;
            }

        }

        public static void SetRowStyles(this ExcelRange range, 
            int? fontSize,Color? color, Color? bgColor, ExcelVerticalAlignment? vAlign, ExcelHorizontalAlignment? hAlign,
            bool merge = false, bool bold = false, bool italic = false, bool underline = false,
            ExcelFillStyle patterType = ExcelFillStyle.Solid)
        {
            range.Style.Fill.PatternType = patterType;
            if (fontSize.HasValue)
            {
                range.Style.Font.Size = fontSize.Value;
            }
            if (color.HasValue)
            {
                range.Style.Font.Color.SetColor(color.Value);
            }
            if (bgColor.HasValue)
            {
                range.Style.Fill.BackgroundColor.SetColor(bgColor.Value);
            }
            if (vAlign.HasValue)
            {
                range.Style.VerticalAlignment = vAlign.Value;
            }
            if (hAlign.HasValue)
            {
                range.Style.HorizontalAlignment = hAlign.Value;
            }
            if (merge)
            {
                range.Merge = merge;
            }
            if (bold)
            {
                range.Style.Font.Bold = bold;
            }
            if (italic)
            {
                range.Style.Font.Italic = italic;
            }
            if (underline)
            {
                range.Style.Font.UnderLine = underline;
            }
        }

        public static void SetCellValue(this ExcelWorksheet worksheet, int rowNum, 
            int columnNum, object value, bool setAutoFit = false, double minWidth = 0, double maxWidth = 0, string numberFormat = null!)
        {
            var cell = worksheet.Cells[rowNum, columnNum];
            cell.Value = value;
            if (!string.IsNullOrWhiteSpace(numberFormat))
            {
                cell.Style.Numberformat.Format = numberFormat;
            }
            if (setAutoFit)
            {
                if(minWidth != 0 && maxWidth != 0 && maxWidth >= minWidth)
                {
                    worksheet.Column(columnNum).AutoFit(minWidth, maxWidth);
                }
                else
                {
                    worksheet.Column(columnNum).AutoFit();
                }
            }
        }
    }
}
