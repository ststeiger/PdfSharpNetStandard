
namespace TestApplication
{



    public class PdfMargin
    {

        public double Left;
        public double Right;
        public double Top;
        public double Bottom;

        public double All
        {
            set
            {
                this.Left = value;
                this.Right = value;
                this.Top = value;
                this.Bottom = value;
            }
        }

        public PdfMargin()
        { }

        public PdfMargin(double margin)
        {
            this.All = margin;
        }

        public PdfMargin(double left, double right, double top, double bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }
    }


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
                PdfSharp.Pdf.PdfPage sourcePage = sourceDocument.Pages[0];

                // Crop the PDF - DOES IT WRONG...
                // sourcePage.CropBox = new PdfSharp.Pdf.PdfRectangle(cropDim);

                PdfSharp.Drawing.XRect cropRect = new PdfSharp.Drawing.XRect(cropDim.X, sourcePage.Height.Point - cropDim.Height - cropDim.Y, cropDim.Width, cropDim.Height);
                sourcePage.CropBox = new PdfSharp.Pdf.PdfRectangle(cropRect);

                sourceDocument.Save("CropPdf2.pdf");
            } // End Using sourceDocument 

        } // End Sub CropPdf2 



        static void CropPdf3(double page_width, double page_height)
        {
            string fn = @"D:\username\Desktop\0001 Altstetten - GB01 H602 - OG14.pdf";
            // fn = @"D:\username\Desktop\0030 Sentimatt - GB01 Sentimatt - EG00.pdf";
            fn = @"D:\username\Desktop\Altstetten_1_50.pdf";


            // The current implementation of PDFsharp has only one layout of the graphics context.
            // The origin(0, 0) is top left and coordinates grow right and down. 
            // The unit of measure is always point (1 / 72 inch).

            // 1 pt = 0,0352778 cm
            // 1 pt = 0,352778 mm 
            // 1 inch = 2,54 cm 
            // 1/72 inch to cm = 0,035277777777777776 cm


            // A0:
            // w: 2384 pt =  84,10222 cm
            // h: 3370 pt = 118,8861  cm

            // A3:
            // w:  842 pt = 29,7039  cm
            // h: 1191 pt = 42,01583 cm

            // A4:
            // 595 pt to cm = 20,9903 w
            // 842 pt to cm = 29,7039 h



            // A0
            page_width = 2384;
            page_height = 3370;

            // A3
            page_width = 842;
            page_height = 1191;

            // A4
            page_width = 595;
            page_height = 842;

            PdfMargin margin = new PdfMargin(mm2pt(10)); // 1cm in pt 
            

            double crop_width = page_width - margin.Left - margin.Right;
            double crop_height = page_height - margin.Top - margin.Bottom;
            

            using (PdfSharp.Drawing.XPdfForm sourceForm = PdfSharp.Drawing.XPdfForm.FromFile(fn))
            {
                sourceForm.PageNumber = 1;
                int numHori = (int)System.Math.Ceiling(sourceForm.Page.Width.Point / crop_width);
                int numVerti = (int)System.Math.Ceiling(sourceForm.Page.Height.Point / crop_height);

                PdfSharp.Drawing.XRect pageDimenstions = new PdfSharp.Drawing.XRect(0, 0, sourceForm.Page.Width, sourceForm.Page.Height);

                using (PdfSharp.Pdf.PdfDocument destDocument = new PdfSharp.Pdf.PdfDocument())
                {
                    for (int iverti = 0; iverti < numVerti; iverti++)
                    {

                        for (int ihori = 0; ihori < numHori; ihori++)
                        {
                            PdfSharp.Pdf.PdfPage destPage = destDocument.AddPage();
                            destPage.Width = crop_width;
                            destPage.Height = crop_height;

                            PdfSharp.Drawing.XRect cropRect = new PdfSharp.Drawing.XRect(ihori * crop_width, iverti * crop_height, sourceForm.Page.Width, sourceForm.Page.Height);
                            
                            using (PdfSharp.Drawing.XGraphics destGFX = PdfSharp.Drawing.XGraphics.FromPdfPage(destPage))
                            {
                                destGFX.DrawImageCropped(sourceForm, cropRect, pageDimenstions, PdfSharp.Drawing.XGraphicsUnit.Point);
                            } // End Using destGFX 

                        } // ihori

                    } // iverti

                    // destDocument.Save("chopped.pdf");

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        destDocument.Save(ms);
                        ms.Position = 0;

                        using (PdfSharp.Drawing.XPdfForm croppedImages = PdfSharp.Drawing.XPdfForm.FromStream(ms))
                        {

                            using (PdfSharp.Pdf.PdfDocument finalDestination = new PdfSharp.Pdf.PdfDocument())
                            {
                                PdfSharp.Drawing.XFont font = new PdfSharp.Drawing.XFont("Arial", 8);

                                for (int i = 0; i < croppedImages.PageCount; ++i)
                                {
                                    PdfSharp.Pdf.PdfPage targetPage = finalDestination.AddPage();
                                    targetPage.Width = page_width;
                                    targetPage.Height = page_height;

                                    PdfSharp.Drawing.XRect pageSize = new PdfSharp.Drawing.XRect(0, 0, targetPage.Width, targetPage.Height);

                                    try
                                    {
                                        croppedImages.PageIndex = i;

                                        using (PdfSharp.Drawing.XGraphics targetGFX = PdfSharp.Drawing.XGraphics.FromPdfPage(targetPage))
                                        {
#if DEBUG_ME
                                            targetGFX.DrawRectangle(XBrushes.Honeydew, pageSize);
#endif

                                            PdfSharp.Drawing.XRect targetRect = new PdfSharp.Drawing.XRect(margin.Left, margin.Top, crop_width, crop_height);
                                            
                                            targetGFX.DrawImage(croppedImages, targetRect, targetRect, PdfSharp.Drawing.XGraphicsUnit.Point);

                                            DrawBorder(targetGFX, targetPage.Width.Point, targetPage.Height.Point, margin);
                                            DrawCrosshairs(targetGFX, targetPage.Width.Point, targetPage.Height.Point, margin);

                                            // int numHori = (int)System.Math.Ceiling(sourceForm.Page.Width / crop_width);
                                            // int numVerti = (int)System.Math.Ceiling(sourceForm.Page.Height / crop_height);

                                            int col = i % numHori;
                                            int row = i / numHori;

                                            // targetGFX.DrawString($"Column {col + 1}/{numHori} Row {row + 1}/{numVerti}, Page {i + 1}/{croppedImages.PageCount}", font, PdfSharp.Drawing.XBrushes.Black, margin.Left + 5, targetPage.Height.Point - margin.Bottom + font.Size + 5);
                                            targetGFX.DrawString($"Spalte {col + 1}/{numHori} Zeile {row + 1}/{numVerti}, Seite {i + 1}/{croppedImages.PageCount}", font, PdfSharp.Drawing.XBrushes.Black, margin.Left + 5, targetPage.Height.Point - margin.Bottom + font.Size + 5);
                                        } // End using targetGFX

                                    }
                                    catch (System.Exception ex)
                                    {
                                        System.Console.WriteLine(croppedImages.PageIndex);
                                        System.Console.WriteLine(ex.Message);
                                    }

                                } // Next i 

                                finalDestination.Save("CropPdf3.pdf");
                            } // End Using finalDestination 

                        } // End Using croppedImages 

                    } // End Using ms 

                } // End Using destDocument 

            } // End Using sourceForm 

        } // End Sub CropPdf3 


        public static double mm2pt(double mm)
        {
            return mm * 2.83465d;
        }


        public static void DrawBorder(PdfSharp.Drawing.XGraphics gfx, double width, double height, PdfMargin margin)
        {
            PdfSharp.Drawing.XPen[] pens = new PdfSharp.Drawing.XPen[] { PdfSharp.Drawing.XPens.Black, PdfSharp.Drawing.XPens.Black, PdfSharp.Drawing.XPens.Black, PdfSharp.Drawing.XPens.Black };
            // pens = new PdfSharp.Drawing.XPen[] { PdfSharp.Drawing.XPens.Yellow, PdfSharp.Drawing.XPens.HotPink, PdfSharp.Drawing.XPens.Blue, PdfSharp.Drawing.XPens.Green };

            double halfPenWidth = pens[0].Width / 2.0;

            double x1 = 0;
            double y1 = margin.Top - halfPenWidth;
            double x2 = width;
            double y2 = margin.Top - halfPenWidth;
            gfx.DrawLine(pens[0], x1, y1, x2, y2); // Horizontal Top - Yellow
            
            // y1 = height-pageMargin;
            x1 = 0;
            y1 = height - margin.Bottom + halfPenWidth;
            x2 = width; // 1/4 + 1/8 ()*0.5= 3/16
            y2 = height - margin.Bottom + halfPenWidth;
            gfx.DrawLine(pens[1], x1, y1, x2, y2); // Horizontal Bottom  - Hotpink



            x1 = margin.Left - halfPenWidth;
            y1 = 0;
            x2 = margin.Left - halfPenWidth;
            y2 = height;
            gfx.DrawLine(pens[2], x1, y1, x2, y2); // Vertical Left - Blue


            x1 = width- margin.Right + halfPenWidth;
            y1 = 0;
            x2 = width - margin.Right + halfPenWidth;
            y2 = height;
            gfx.DrawLine(pens[3], x1, y1, x2, y2); // Vertical Right - Green 
        }



        public static void DrawCrosshairs(PdfSharp.Drawing.XGraphics gfx, double width, double height, PdfMargin margin)
        {
            PdfSharp.Drawing.XPen[] pens = new PdfSharp.Drawing.XPen[] { PdfSharp.Drawing.XPens.Black, PdfSharp.Drawing.XPens.Black };
            // pens = new PdfSharp.Drawing.XPen[] { PdfSharp.Drawing.XPens.Yellow, PdfSharp.Drawing.XPens.Red };

            double halfPenWidth = pens[0].Width / 2.0;

            DrawCrossTopLeft(gfx, width, height, margin, halfPenWidth, pens);
            DrawCrossTopRight(gfx, width, height, margin, halfPenWidth, pens);
            DrawCrossBottomLeft(gfx, width, height, margin, halfPenWidth, pens);
            DrawCrossBottomRight(gfx, width, height, margin, halfPenWidth, pens);
        }


        public static void DrawCrossTopLeft(PdfSharp.Drawing.XGraphics gfx, double width, double height, PdfMargin margin, double halfPenWidth, PdfSharp.Drawing.XPen[] pens)
        {
            double x1 = 0;
            double y1 = margin.Top - halfPenWidth;
            double x2 = margin.Left;
            double y2 = margin.Top - halfPenWidth;
            gfx.DrawLine(pens[0], x1, y1, x2, y2); // Horizontal - Yellow

            x1 = margin.Left -  halfPenWidth;
            y1 = 0;
            x2 = margin.Left - halfPenWidth; 
            y2 = margin.Top;
            gfx.DrawLine(pens[1], x1, y1, x2, y2); // Vertical - Red
        }

        public static void DrawCrossTopRight(PdfSharp.Drawing.XGraphics gfx, double width, double height, PdfMargin margin, double halfPenWidth, PdfSharp.Drawing.XPen[] pens)
        {
            double x1 = width - margin.Right;
            double y1 = margin.Top - halfPenWidth;
            double x2 = width;
            double y2 = margin.Top - halfPenWidth;
            gfx.DrawLine(pens[0], x1, y1, x2, y2); // Horizontal - Yellow

            x1 = width - margin.Right + halfPenWidth;
            y1 = 0;
            x2 = width - margin.Right + halfPenWidth;
            y2 = margin.Top;
            gfx.DrawLine(pens[1], x1, y1, x2, y2); // Vertical - Red 
        }

        public static void DrawCrossBottomLeft(PdfSharp.Drawing.XGraphics gfx, double width, double height, PdfMargin margin, double halfPenWidth, PdfSharp.Drawing.XPen[] pens)
        {
            double x1 = 0;
            double y1 = height - margin.Bottom + halfPenWidth;
            double x2 = margin.Left;
            double y2 = height - margin.Bottom + halfPenWidth;
            gfx.DrawLine(pens[0], x1, y1, x2, y2); // Horizontal - Yellow 

            x1 = margin.Left - halfPenWidth;
            y1 = height - margin.Bottom;
            x2 = margin.Left - halfPenWidth;
            y2 = height;
            gfx.DrawLine(pens[1], x1, y1, x2, y2); // Vertical - Red 
        }

        public static void DrawCrossBottomRight(PdfSharp.Drawing.XGraphics gfx, double width, double height, PdfMargin margin, double halfPenWidth, PdfSharp.Drawing.XPen[] pens)
        {
            double x1 = width - margin.Right;
            double y1 = height - margin.Bottom + halfPenWidth;
            double x2 = width;
            double y2 = height - margin.Bottom + halfPenWidth;
            gfx.DrawLine(pens[0], x1, y1, x2, y2); // Horizontal - Yellow 

            x1 = width - margin.Right + halfPenWidth;
            y1 = height - margin.Bottom;
            x2 = width - margin.Right + halfPenWidth;
            y2 = height;
            gfx.DrawLine(pens[1], x1, y1, x2, y2); // Vertical - red 
        }


        public static void CropPdf1( double crop_width, double crop_height)
        {
            string fn = @"D:\username\Desktop\0001 Altstetten - GB01 H602 - OG14.pdf";
            // fn = @"D:\username\Desktop\0030 Sentimatt - GB01 Sentimatt - EG00.pdf";


            // The current implementation of PDFsharp has only one layout of the graphics context.
            // The origin(0, 0) is top left and coordinates grow right and down. 
            // The unit of measure is always point (1 / 72 inch).

            // 1 pt = 0,0352778 cm
            // 1 pt = 0,352778 mm 
            // 1 inch = 2,54 cm 
            // 1/72 inch to cm = 0,035277777777777776 cm


            // A0:
            // w: 2384 pt =  84,10222 cm
            // h: 3370 pt = 118,8861  cm

            // A3:
            // w:  842 pt = 29,7039  cm
            // h: 1191 pt = 42,01583 cm

            // A4:
            // 595 pt to cm = 20,9903 w
            // 842 pt to cm = 29,7039 h


            using (PdfSharp.Drawing.XPdfForm sourceForm = PdfSharp.Drawing.XPdfForm.FromFile(fn))
            {
                sourceForm.PageNumber = 1;

                int numHori = (int)System.Math.Ceiling(sourceForm.Page.Width.Point / crop_width);
                int numVerti = (int)System.Math.Ceiling(sourceForm.Page.Height.Point / crop_height);
                PdfSharp.Drawing.XRect pageDimenstions = new PdfSharp.Drawing.XRect(0, 0, sourceForm.Page.Width.Point, sourceForm.Page.Height.Point);


                // Crop the PDF - HAS NO EFFECT...
                // sourceForm.Page.CropBox = new PdfSharp.Pdf.PdfRectangle(cropDim);

                using (PdfSharp.Pdf.PdfDocument destDocument = new PdfSharp.Pdf.PdfDocument())
                {
                    for (int iverti = 0; iverti < numHori; iverti++)
                    {

                        for (int ihori = 0; ihori < numHori; ihori++)
                        {
                            PdfSharp.Pdf.PdfPage destPage = destDocument.AddPage();
                            destPage.Width = sourceForm.Page.Width;
                            destPage.Height = sourceForm.Page.Height;


                            PdfSharp.Drawing.XRect cropDim = new PdfSharp.Drawing.XRect(ihori * crop_width, iverti * crop_height, crop_width, crop_height);

                            PdfSharp.Drawing.XRect cropRect = new PdfSharp.Drawing.XRect(cropDim.X, destPage.Height.Point - cropDim.Height - cropDim.Y, cropDim.Width, cropDim.Height);

                            using (PdfSharp.Drawing.XGraphics destGFX = PdfSharp.Drawing.XGraphics.FromPdfPage(destPage))
                            {
                                destGFX.DrawImage(sourceForm, pageDimenstions);
                                destGFX.DrawRectangle(PdfSharp.Drawing.XPens.DeepPink, cropDim);
                            } // End Using destGFX 

                            // PdfSharp.Drawing.XRect cropRect1 = new PdfSharp.Drawing.XRect(cropRect.X - 30, cropRect.Y - 30, cropRect.Width + 30, cropRect.Height + 30);

                            destPage.CropBox = new PdfSharp.Pdf.PdfRectangle(cropRect);
                            // destPage.MediaBox = new PdfSharp.Pdf.PdfRectangle(cropRect1);

                            // destPage.CropBox = new PdfSharp.Pdf.PdfRectangle(new XPoint(cropDim.X, destPage.Height - cropDim.Height - cropDim.Y),
                            //                      new XSize(cropDim.Width, cropDim.Height));
                        } // Next ihori 

                    } // Next iverti 

                    destDocument.Save("CropPdf1.pdf");
                } // End Using gfx 

            } // End Using document 

        } // End Sub CropPdf1 


        // nuget: System.Text.Encoding.CodePages
        static void Main(string[] args)
        {
            // System.NotSupportedException: No data is available for encoding 1252
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            CropPdf1(400, 800); // Crop page by poster-sector, using cropbox 
            // CropPdf2(); // Crop the source page 
            // CropPdf3(400, 800); // Crop page by poster sector by positioning, not using cropbox, add page border lines

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

                    using (PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromFile(@"D:\username\Documents\Visual Studio 2017\Projects\PdfSharp\TestApplication\Wikipedesketch1.png"))
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
