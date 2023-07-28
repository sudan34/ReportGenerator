using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ReportGenerator.Models;

namespace ReportGenerator.Controllers
{
    public class InvoiceController : ControllerBase
    {
        public IActionResult GenerateInvoice()
        {
            // Sample invoice data (you can replace this with your actual data)
            var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem { ItemName = "Item 1", Quantity = 5, UnitPrice = 10.50m },
                new InvoiceItem { ItemName = "Item 2", Quantity = 3, UnitPrice = 8.25m },
                new InvoiceItem { ItemName = "Item 3", Quantity = 2, UnitPrice = 12.75m }
            };

            // Create a new PDF document
            var document = new Document(PageSize.A4);

            // Create a MemoryStream to hold the generated PDF data
            using var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);

            // Add header and footer using PdfPageEventHelper
            var eventHelper = new CustomPageEvent();
            writer.PageEvent = eventHelper;

            // Open the document to begin adding content
            document.Open();

            // Add invoice content (table with data)
            var table = new PdfPTable(4); // 4 columns for ItemName, Quantity, UnitPrice, TotalPrice
            table.WidthPercentage = 100; // Make the table fill the page width

            // Add table headers with background color
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10f, BaseColor.WHITE); // Font with white text color
            var headerBackgroundColor = new BaseColor(41, 128, 185); // Change the RGB values to the desired color
            var headerCell = new PdfPCell(new Phrase("Item Name", headerFont));
            headerCell.BackgroundColor = headerBackgroundColor;
            table.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Quantity", headerFont));
            headerCell.BackgroundColor = headerBackgroundColor;
            table.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Unit Price", headerFont));
            headerCell.BackgroundColor = headerBackgroundColor;
            table.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Total Price", headerFont));
            headerCell.BackgroundColor = headerBackgroundColor;
            table.AddCell(headerCell);

            // Add data rows to the table
            foreach (var item in invoiceItems)
            {
                table.AddCell(item.ItemName);
                table.AddCell(item.Quantity.ToString());
                table.AddCell(item.UnitPrice.ToString("C")); // Format as currency
                table.AddCell(item.TotalPrice.ToString("C")); // Format as currency
            }

            // Add the table to the document
            document.Add(table);

            // Close the document to finish adding content
            document.Close();

            // Set the response headers
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=Invoice.pdf");

            // Get the byte array from the MemoryStream
            var pdfBytes = stream.ToArray();

            // Return the PDF as a FileContentResult
            return new FileContentResult(pdfBytes, "application/pdf");
        }
    }
    public class CustomPageEvent : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            // Header content
            var headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

            var headerCell = new PdfPCell(new Phrase("Invoice Header"));
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.Border = Rectangle.NO_BORDER;

            // Set extra spacing after the header cell content
            headerCell.ExtraParagraphSpace = 10f;
            headerTable.AddCell(headerCell);
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + headerTable.TotalHeight, writer.DirectContent);
            // Add spacing after the header
            document.Add(new Paragraph("")); // Empty paragraph for spacing
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Footer content
            var footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            footerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.DefaultCell.Border = Rectangle.NO_BORDER;

            var footerCell = new PdfPCell(new Phrase($"Page {writer.PageNumber}"));
            footerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            footerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCell.Border = Rectangle.NO_BORDER;

            footerTable.AddCell(footerCell);
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
        }
    }
}

