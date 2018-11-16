
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;


namespace Examples
{


    // http://www.pdfsharp.net/wiki/PageSizes-sample.ashx
    class PageSizes
    {

        public static void foo()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create a font
            XFont font = new XFont("Times", 25, XFontStyle.Bold);

            PageSize[] pageSizes = (PageSize[])System.Enum.GetValues(typeof(PageSize));
            foreach (PageSize pageSize in pageSizes)
            {
                if (pageSize == PageSize.Undefined)
                    continue;

                // One page in Portrait...
                PdfPage page = document.AddPage();
                page.Size = pageSize;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize.ToString(), font, XBrushes.DarkRed,
                  new XRect(0, 0, page.Width, page.Height),
                  XStringFormats.Center);

                // ... and one in Landscape orientation.
                page = document.AddPage();
                page.Size = pageSize;
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize + " (landscape)", font,
                  XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height),
                  XStringFormats.Center);
            }

            // Save the document...
            const string filename = "PageSizes_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }


    }
}
