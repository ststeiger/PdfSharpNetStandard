﻿
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;


namespace Examples
{


    // http://www.pdfsharp.net/wiki/TwoPagesOnOne-sample.ashx
    class TwoPagesOnOne
    {


        public static void Test()
        {
            // Get a fresh copy of the sample PDF file
            string filename = "Portable Document Format.pdf";
            System.IO.File.Copy(System.IO.Path.Combine("../../../../../PDFs/", filename),
              System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), filename), true);

            // Create the output document
            PdfDocument outputDocument = new PdfDocument();

            // Show single pages
            // (Note: one page contains two pages from the source document)
            outputDocument.PageLayout = PdfPageLayout.SinglePage;

            XFont font = new XFont("Verdana", 8, XFontStyle.Bold);
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Center;
            format.LineAlignment = XLineAlignment.Far;
            XGraphics gfx;
            XRect box;

            // Open the external document as XPdfForm object
            XPdfForm form = XPdfForm.FromFile(filename);

            for (int idx = 0; idx < form.PageCount; idx += 2)
            {
                // Add a new page to the output document
                PdfPage page = outputDocument.AddPage();
                page.Orientation = PageOrientation.Landscape;
                double width = page.Width;
                double height = page.Height;

                int rotate = page.Elements.GetInteger("/Rotate");

                gfx = XGraphics.FromPdfPage(page);

                // Set page number (which is one-based)
                form.PageNumber = idx + 1;

                box = new XRect(0, 0, width / 2, height);
                // Draw the page identified by the page number like an image
                gfx.DrawImage(form, box);

                // Write document file name and page number on each page
                box.Inflate(0, -10);
                gfx.DrawString(string.Format("- {1} -", filename, idx + 1),
                  font, XBrushes.Red, box, format);

                if (idx + 1 < form.PageCount)
                {
                    // Set page number (which is one-based)
                    form.PageNumber = idx + 2;

                    box = new XRect(width / 2, 0, width / 2, height);
                    // Draw the page identified by the page number like an image
                    gfx.DrawImage(form, box);

                    // Write document file name and page number on each page
                    box.Inflate(0, -10);
                    gfx.DrawString(string.Format("- {1} -", filename, idx + 2),
                      font, XBrushes.Red, box, format);
                }
            }

            // Save the document...
            filename = "TwoPagesOnOne_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }

    }
}
