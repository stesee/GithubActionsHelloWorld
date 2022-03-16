using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GithubActionsHelloWorld
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateExcelDocument();
        }

        private static void CreateExcelDocument()
        {
            Microsoft.Office.Interop.Excel.Application excelApplication = null;

            try
            {
                excelApplication = new
                    Microsoft.Office.Interop.Excel.Application();
                Workbook excelWorkBook = excelApplication.Workbooks.Add();
                Worksheet worksheet = (Worksheet)excelWorkBook.Worksheets[1];
                worksheet.Cells[1, 1] = "Product Id";
                worksheet.Cells[1, 2] = "Product Name";
                worksheet.Cells[2, 1] = "1";
                worksheet.Cells[2, 2] = "Lenovo Laptop";
                worksheet.Cells[3, 1] = "2";
                worksheet.Cells[3, 2] = "DELL Laptop";
                var path = Path.GetTempFileName() + ".xlsx";
                Console.WriteLine(path);
                excelWorkBook.SaveAs(Path.Combine(path));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (excelApplication != null)
                {
                    excelApplication.Quit();
                    Marshal.FinalReleaseComObject(excelApplication);
                }
            }
        }
    }
}