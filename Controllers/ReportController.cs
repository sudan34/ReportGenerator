using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

namespace ReportGenerator.Controllers
{

    public class ReportController : ControllerBase
    {
        private readonly ReportDataService _reportDataService;

        public ReportController(ReportDataService reportDataService)
        {
            _reportDataService = reportDataService;
        }
        public IActionResult GenerateReport()
        {
            var reportData = _reportDataService.GetReportData();
            // Create a new PDF document
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            
            // Create a new PDF document with custom page size and margins
            // var document = new Document(new Rectangle(PageSize.LETTER.Width, PageSize.LETTER.Height));
            // document.LeftMargin = 50;
            // document.RightMargin = 50;
            // document.TopMargin = 50;
            // document.BottomMargin = 50;
            // Create a MemoryStream to hold the generated PDF data
            using var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);

            // Open the document to begin adding content
            document.Open();

            // Add content to the PDF
            var paragraph = new Paragraph("Sample Report Generated Using iTextSharp");
            paragraph.SpacingAfter = 20;
            document.Add(paragraph);
            // Create a table with 3 columns
            var table = new PdfPTable(3);
            // Set custom padding for table cells
            table.DefaultCell.Padding = 10;
            // Add table headers
            table.AddCell("ID");
            table.AddCell("Name");
            table.AddCell("Address");
            // Add other headers as needed

            // Add data rows to the table
            foreach (var data in reportData)
            {
                table.AddCell(data.Id.ToString());
                table.AddCell(data.Name);
                table.AddCell(data.Address);
                // Add other data cells as needed
            }

            // Add the table to the document
            document.Add(table);
            // Close the document to finish adding content
            document.Close();

            // Set the response headers
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=SampleReport.pdf");

            // Get the byte array from the MemoryStream
            var pdfBytes = stream.ToArray();

            // Return the PDF as a FileContentResult
            return new FileContentResult(pdfBytes, "application/pdf");
        }
    }
}
