
using PdfSharp.Pdf.Annotations;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace Examples
{

    // http://www.pdfsharp.net/wiki/PDFsharpSamples.ashx
    // http://www.pdfsharp.net/wiki/Annotations-sample.ashx
    class Annotations
    {


        public static void First()
        {
            // Create a PDF text annotation
            PdfTextAnnotation textAnnot = new PdfTextAnnotation();
            textAnnot.Title = "This is the title";
            textAnnot.Subject = "This is the subject";
            textAnnot.Contents = "This is the contents of the annotation.\rThis is the 2nd line.";
            textAnnot.Icon = PdfTextAnnotationIcon.Note;



            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                page.Width = XUnit.FromMillimeter(80).Point;
                page.Height = XUnit.FromMillimeter(800).Point;


                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    XFont font = new XFont("Courier New", 9, XFontStyle.Regular);
                    gfx.DrawString("The first text annotation", font, XBrushes.Black, 30, 50, XStringFormats.Default);

                    // Convert rectangle from world space to page space. This is necessary because the annotation is
                    // placed relative to the bottom left corner of the page with units measured in point.
                    XRect rect = gfx.Transformer.WorldToDefaultPage(new XRect(new XPoint(30, 60), new XSize(30, 30)));
                    textAnnot.Rectangle = new PdfRectangle(rect);
                    // Add the annotation to the page
                    page.Annotations.Add(textAnnot);
                }
            }
        }


        public static void Second()
        {
            // Create another PDF text annotation which is open and transparent
            PdfTextAnnotation textAnnot = new PdfTextAnnotation();
            textAnnot.Title = "Annotation 2 (title)";
            textAnnot.Subject = "Annotation 2 (subject)";
            textAnnot.Contents = "This is the contents of the 2nd annotation.";
            textAnnot.Icon = PdfTextAnnotationIcon.Help;
            textAnnot.Color = XColors.LimeGreen;
            textAnnot.Opacity = 0.5;
            textAnnot.Open = true;

            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                page.Width = XUnit.FromMillimeter(80).Point;
                page.Height = XUnit.FromMillimeter(800).Point;


                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    XFont font = new XFont("Courier New", 9, XFontStyle.Regular);
                    gfx.DrawString("The second text annotation (opened)", font, XBrushes.Black, 30, 140, XStringFormats.Default);
                    XRect rect = gfx.Transformer.WorldToDefaultPage(new XRect(new XPoint(30, 150), new XSize(30, 30)));
                    textAnnot.Rectangle = new PdfRectangle(rect);

                    // Add the 2nd annotation to the page
                    page.Annotations.Add(textAnnot);
                }
            }

        }

        public static void Third()
        {
            // Create a so called rubber stamp annotation. I'm not sure if it is useful, but at least
            // it looks impressive...
            PdfRubberStampAnnotation rsAnnot = new PdfRubberStampAnnotation();
            rsAnnot.Icon = PdfRubberStampAnnotationIcon.TopSecret;
            rsAnnot.Flags = PdfAnnotationFlags.ReadOnly;



            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                page.Width = XUnit.FromMillimeter(80).Point;
                page.Height = XUnit.FromMillimeter(800).Point;


                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    XRect rect = gfx.Transformer.WorldToDefaultPage(new XRect(new XPoint(100, 400), new XSize(350, 150)));
                    rsAnnot.Rectangle = new PdfRectangle(rect);


                    // Add the rubber stamp annotation to the page
                    page.Annotations.Add(rsAnnot);
                }
            }

        } // End Sub Third 


    }


}
