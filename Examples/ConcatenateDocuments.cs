using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;


namespace Examples
{


    // http://www.pdfsharp.net/wiki/ConcatenateDocuments-sample.ashx
    class ConcatenateDocuments
    {


        private static string[] GetFiles()
        {
            return new string[] { "", "", "" };
        }


        private static void DrawNumber(XGraphics gfx, XFont font, int pageNumber)
        {
            string text = "Page " + pageNumber;

            gfx.DrawString(text, font, XBrushes.Black, 20, 50, XStringFormats.Default);
        }


        /// <summary>
        /// Imports all pages from a list of documents.
        /// </summary>
        static void Variant1()
        {
            // Get some file names
            string[] files = GetFiles();

            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // Iterate files
            foreach (string file in files)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add it to the output document.
                    outputDocument.AddPage(page);
                }
            }

            // Save the document...
            const string filename = "ConcatenatedDocument1_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }


        /// <summary>
        /// This sample adds each page twice to the output document. The output document
        /// becomes only a little bit larger because the content of the pages is reused 
        /// and not duplicated.
        /// </summary>
        static void Variant2()
        {
            // Get some file names
            string[] files = GetFiles();

            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // Show consecutive pages facing. Requires Acrobat 5 or higher.
            outputDocument.PageLayout = PdfPageLayout.TwoColumnLeft;

            // Iterate files
            foreach (string file in files)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add them twice to the output document.
                    outputDocument.AddPage(page);
                    outputDocument.AddPage(page);
                }
            }

            // Save the document...
            const string filename = "ConcatenatedDocument2_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }

        /// <summary>
        /// This sample adds a consecutive number in the middle of each page.
        /// It shows how you can add graphics to an imported page.
        /// </summary>
        static void Variant3()
        {
            // Get some file names
            string[] files = GetFiles();

            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // Note that the output document may look significant larger than in Variant1.
            // This is because adding graphics to an imported page causes the 
            // uncompression of its content if it was compressed in the external document.
            // To compare file sizes you should either run the sample as Release build
            // or uncomment the following line.
            //outputDocument.Options.CompressContentStreams = true;

            XFont font = new XFont("Verdana", 40, XFontStyle.Bold);
            int number = 0;

            // Iterate files
            foreach (string file in files)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add it to the output document.
                    // Note that the PdfPage instance returned by AddPage is a
                    // different object.
                    page = outputDocument.AddPage(page);

                    // Create a graphics object for this page. To draw beneath the existing
                    // content set 'Append' to 'Prepend'.
                    XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
                    DrawNumber(gfx, font, ++number);
                }
            }

            // Save the document...
            const string filename = "ConcatenatedDocument3_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }

        /// <summary>
        /// This sample is the combination of Variant2 and Variant3. It shows that you 
        /// can add external pages more than once and still add individual graphics on
        /// each page. The external content is shared among the pages, the new graphics
        /// are unique to each page. You can check this by comparing the file size
        /// of Variant3 and Variant4.
        /// </summary>
        static void Variant4()
        {
            // Get some file names
            string[] files = GetFiles();

            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // For checking the file size uncomment next line.
            //outputDocument.Options.CompressContentStreams = true;

            XFont font = new XFont("Verdana", 40, XFontStyle.Bold);
            int number = 0;

            // Iterate files
            foreach (string file in files)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                // Show consecutive pages facing. Requires Acrobat 5 or higher.
                outputDocument.PageLayout = PdfPageLayout.TwoColumnLeft;

                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add it twice to the output document.
                    PdfPage page1 = outputDocument.AddPage(page);
                    PdfPage page2 = outputDocument.AddPage(page);

                    XGraphics gfx =
                      XGraphics.FromPdfPage(page1, XGraphicsPdfPageOptions.Append);
                    DrawNumber(gfx, font, ++number);

                    gfx = XGraphics.FromPdfPage(page2, XGraphicsPdfPageOptions.Append);
                    DrawNumber(gfx, font, ++number);
                }
            }

            // Save the document...
            const string filename = "ConcatenatedDocument4_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }


    }
}
