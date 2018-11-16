
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace TestApplication
{


    class Program
    {


        static void CropPdf2()
        {
            string fn = @"D:\username\Desktop\0001 Altstetten - GB01 H602 - OG14.pdf";

            int ten = (int)(28.3465 * 10);
            int hundred = (int)(28.3465 * 100);

            int x = ten / 20;
            int y = hundred / 20;

            PdfSharp.Drawing.XRect cropDim = new PdfSharp.Drawing.XRect(0, 0, 200, 200);
            cropDim = new PdfSharp.Drawing.XRect(200, 200, 200, 200);

            using (PdfSharp.Pdf.PdfDocument sourceDocument = PdfSharp.Pdf.IO.PdfReader.Open(fn))
            {
                PdfPage sourcePage = sourceDocument.Pages[0];

                // Crop the PDF - DOES IT WRONG...
                // sourcePage.CropBox = new PdfRectangle(cropDim);

                PdfSharp.Drawing.XRect cropRect = new PdfSharp.Drawing.XRect(cropDim.X, sourcePage.Height - cropDim.Height - cropDim.Y, cropDim.Width, cropDim.Height);
                sourcePage.CropBox = new PdfRectangle(cropRect);

                sourceDocument.Save("cropped2.pdf");
            } // End Using sourceDocument 

        } // End Sub CropPdf2 


        static void CropPdf1(double crop_width, double crop_height)
        {
            string fn = @"D:\username\Desktop\0001 Altstetten - GB01 H602 - OG14.pdf";

            // The current implementation of PDFsharp has only one layout of the graphics context.
            // The origin(0, 0) is top left and coordinates grow right and down. 
            // The unit of measure is always point (1 / 72 inch).

            using (PdfSharp.Drawing.XPdfForm sourceForm = PdfSharp.Drawing.XPdfForm.FromFile(fn))
            {
                sourceForm.PageNumber = 1;

                int numHori = (int) System.Math.Ceiling(sourceForm.Page.Width/ crop_width);
                int numVerti = (int)System.Math.Ceiling(sourceForm.Page.Height/ crop_height);
                PdfSharp.Drawing.XRect pageDimenstions = new PdfSharp.Drawing.XRect(0, 0, sourceForm.Page.Width.Point, sourceForm.Page.Height.Point);


                // Crop the PDF - HAS NO EFFECT...
                // sourceForm.Page.CropBox = new PdfRectangle(cropDim);

                using (PdfDocument destDocument = new PdfDocument())
                {
                    for (int iverti = 0; iverti < numHori; iverti++)
                    {

                        for (int ihori = 0; ihori < numHori; ihori++)
                        {
                            PdfPage destPage = destDocument.AddPage();
                            destPage.Width = sourceForm.Page.Width;
                            destPage.Height = sourceForm.Page.Height;

                            PdfSharp.Drawing.XRect cropDim = new PdfSharp.Drawing.XRect(ihori*crop_width, iverti * crop_height, crop_width, crop_height);

                            PdfSharp.Drawing.XRect cropRect = new PdfSharp.Drawing.XRect(cropDim.X, destPage.Height - cropDim.Height - cropDim.Y, cropDim.Width, cropDim.Height);

                            using (XGraphics destGFX = XGraphics.FromPdfPage(destPage))
                            {
                                destGFX.DrawImage(sourceForm, pageDimenstions);
                                destGFX.DrawRectangle(PdfSharp.Drawing.XPens.DeepPink, cropDim);
                            } // End Using destGFX 

                            destPage.CropBox = new PdfRectangle(cropRect);

                            /* 
                            destPage.CropBox = new PdfRectangle(new XPoint(cropDim.X, destPage.Height - cropDim.Height - cropDim.Y),
                                                 new XSize(cropDim.Width, cropDim.Height));
                            */
                        } // Next pnum 

                    }

                    destDocument.Save("cropped1.pdf");
                } // End Using gfx 

            } // End Using document 

        } // End Sub CropPdf1 


        // nuget: System.Text.Encoding.CodePages
        static void Main(string[] args)
        {
            // System.NotSupportedException: No data is available for encoding 1252
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            CropPdf1(400, 800);
            CropPdf2();
            

            // https://gunnarpeipman.com/net/no-data-is-available-for-encoding/
            // https://stackoverflow.com/questions/49215791/vs-code-c-sharp-system-notsupportedexception-no-data-is-available-for-encodin?noredirect=1&lq=1
            // System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // TestPdfReader();
            // ReadPdf();
            // WriteTest();
            // TestCrop();
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


        static void TestPdfReader()
        {
            string fn = @"C:\Program Files\Microsoft\R Client\R_SERVER\doc\manual\fullrefman.pdf";

            int version = PdfSharp.Pdf.IO.PdfReader.TestPdfFile(fn);
            System.Console.WriteLine(version);

            using (PdfSharp.Pdf.PdfDocument pr = PdfSharp.Pdf.IO.PdfReader.Open(fn))
            {
                System.Console.WriteLine(pr.Pages.Count);
            }

        }


        public static void ReadPdf()
        {
            // Get a fresh copy of the sample PDF file
            string filename = @"C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\R_SERVICES\doc\manual\R-intro.pdf";

            // Create the output document
            PdfSharp.Pdf.PdfDocument outputDocument =
                new PdfSharp.Pdf.PdfDocument();

            // Show single pages
            // (Note: one page contains two pages from the source document)
            outputDocument.PageLayout = PdfSharp.Pdf.PdfPageLayout.SinglePage;

            /*
            PdfSharp.Drawing.XFont font =
                new PdfSharp.Drawing.XFont("Verdana", 8, PdfSharp.Drawing.XFontStyle.Bold);
            PdfSharp.Drawing.XStringFormat format = new PdfSharp.Drawing.XStringFormat();
            format.Alignment = PdfSharp.Drawing.XStringAlignment.Center;
            format.LineAlignment = PdfSharp.Drawing.XLineAlignment.Far;
            */
            PdfSharp.Drawing.XGraphics gfx;
            PdfSharp.Drawing.XRect box;

            // Open the external document as XPdfForm object
            PdfSharp.Drawing.XPdfForm form =
                PdfSharp.Drawing.XPdfForm.FromFile(filename);

            for (int idx = 0; idx < form.PageCount; idx += 2)
            {
                // Add a new page to the output document
                PdfSharp.Pdf.PdfPage page = outputDocument.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                double width = page.Width;
                double height = page.Height;

                int rotate = page.Elements.GetInteger("/Rotate");

                gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);

                // Set page number (which is one-based)
                form.PageNumber = idx + 1;

                box = new PdfSharp.Drawing.XRect(0, 0, width / 2, height);
                // Draw the page identified by the page number like an image
                gfx.DrawImage(form, box);
                

                // Write document file name and page number on each page
                box.Inflate(0, -10);
                /*
                 gfx.DrawString(string.Format("- {1} -", filename, idx + 1),
                     font, PdfSharp.Drawing.XBrushes.Red, box, format);
                 */
                if (idx + 1 < form.PageCount)
                {
                    // Set page number (which is one-based)
                    form.PageNumber = idx + 2;

                    box = new PdfSharp.Drawing.XRect(width / 2, 0, width / 2, height);
                    // Draw the page identified by the page number like an image
                    gfx.DrawImage(form, box);

                    // Write document file name and page number on each page
                    box.Inflate(0, -10);
                    /*
                    gfx.DrawString(string.Format("- {1} -", filename, idx + 2),
                        font, PdfSharp.Drawing.XBrushes.Red, box, format);
                        */
                }
            }

            // Save the document...
            filename = "TwoPagesOnOne_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }



        static void WriteTest()
        {
            using (PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument())
            {
                document.Info.Title = "Family Tree";
                document.Info.Author = "FamilyTree Ltd. - Stefan Steiger";
                document.Info.Subject = "Family Tree";
                document.Info.Keywords = "Family Tree, Genealogical Tree, Genealogy, Bloodline, Pedigree";


                PdfSharp.Pdf.Security.PdfSecuritySettings securitySettings = document.SecuritySettings;

                // Setting one of the passwords automatically sets the security level to
                // PdfDocumentSecurityLevel.Encrypted128Bit.
                securitySettings.UserPassword = "user";
                securitySettings.OwnerPassword = "owner";

                // Don't use 40 bit encryption unless needed for compatibility
                //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

                // Restrict some rights.
                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = true;
                securitySettings.PermitFullQualityPrint = false;
                securitySettings.PermitModifyDocument = true;
                securitySettings.PermitPrint = false;


                

                document.ViewerPreferences.Direction = PdfSharp.Pdf.PdfReadingDirection.LeftToRight;
                PdfSharp.Pdf.PdfPage page = document.AddPage();

                // page.Width = PdfSettings.PaperFormatSettings.Width
                // page.Height = PdfSettings.PaperFormatSettings.Height

                page.Orientation = PdfSharp.PageOrientation.Landscape;
                double dblLineWidth = 1.0;
                string strHtmlColor = "#FF00FF";
                PdfSharp.Drawing.XColor lineColor = XColorHelper.FromHtml(strHtmlColor);
                PdfSharp.Drawing.XPen pen = new PdfSharp.Drawing.XPen(lineColor, dblLineWidth);

                PdfSharp.Drawing.XFont font = new PdfSharp.Drawing.XFont("Arial"
                    , 12.0, PdfSharp.Drawing.XFontStyle.Bold
                );



                using (PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                {
                    gfx.MUH = PdfSharp.Pdf.PdfFontEncoding.Unicode;

                    PdfSharp.Drawing.Layout.XTextFormatter tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
                    tf.Alignment = PdfSharp.Drawing.Layout.XParagraphAlignment.Left;

                    PdfSharp.Drawing.Layout.XTextFormatterEx2 etf = new PdfSharp.Drawing.Layout.XTextFormatterEx2(gfx);


                    gfx.DrawRectangle(pen, new PdfSharp.Drawing.XRect(100, 100, 100, 100));

                    using (PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromFile(@"D:\Stefan.Steiger\Documents\Visual Studio 2017\Projects\PdfSharp\TestApplication\Wikipedesketch1.png"))
                    {
                        gfx.DrawImage(img, 500, 500);
                    }

                    string text = "Lalala";

                    tf.DrawString(text
                                , font
                                , PdfSharp.Drawing.XBrushes.Black
                                , new PdfSharp.Drawing.XRect(300, 300, 100, 100)
                                , PdfSharp.Drawing.XStringFormats.TopLeft
                    );

                } // End Using gfx 

                // Save the document...
                string filename = "TestFilePwProtected.pdf";
                document.Save(filename);
                // ...and start a viewer.
                System.Diagnostics.Process.Start(filename);
            } // End Using document 
            
        } // End Sub WriteTest 


    } // End Class Program 


} // End Namespace TestApplication 
