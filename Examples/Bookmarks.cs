
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace Examples
{


    // http://www.pdfsharp.net/wiki/Bookmarks-sample.ashx
    class Bookmarks
    {


        public static void Test()
        {
            string fileName = "bookmarks_test.pdf";

            // Create a new PDF document
            using (PdfDocument document = new PdfDocument())
            {

                // Create a font
                XFont font = new XFont("Verdana", 16);

                // Create first page
                PdfPage page = document.AddPage();

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                { 
                    gfx.DrawString("Page 1", font, XBrushes.Black, 20, 50, XStringFormats.Default);
                }
                // Create the root bookmark. You can set the style and the color.
                PdfOutline outline = document.Outlines.Add("Root", page, true, PdfOutlineStyle.Bold, XColors.Red);

                // Create some more pages
                for (int idx = 2; idx <= 5; idx++)
                {
                    page = document.AddPage();

                    string text = "Page " + idx;

                    using (XGraphics gfx = XGraphics.FromPdfPage(page))
                    {
                        gfx.DrawString(text, font, XBrushes.Black, 20, 50, XStringFormats.Default);
                    }
                    
                    // Create a sub bookmark
                    outline.Outlines.Add(text, page, true);
                }

                // Save the document...
                const string filename = "Bookmarks_tempfile.pdf";
                document.Save(filename);
            }
            // ...and start a viewer.
            System.Diagnostics.Process.Start(fileName);
        }


    }


}
