
using PdfSharp.Drawing;


namespace Examples
{


    // http://www.pdfsharp.net/wiki/XForms-sample.ashx
    class XForms
    {

        public static void Test(PdfSharp.Pdf.PdfDocument document)
        {
            // Create an empty XForm object with the specified width and height
            // A form is bound to its target document when it is created. The reason is that the form can 
            // share fonts and other objects with its target document.
            XForm form = new XForm(document, XUnit.FromMillimeter(70), XUnit.FromMillimeter(55));

            // Create an XGraphics object for drawing the contents of the form.
            XGraphics formGfx = XGraphics.FromForm(form);

            // Draw a large transparent rectangle to visualize the area the form occupies
            XColor back = XColors.Orange;
            back.A = 0.2;
            XSolidBrush brush = new XSolidBrush(back);
            formGfx.DrawRectangle(brush, -10000, -10000, 20000, 20000);

            // On a form you can draw...

            // ... text
            formGfx.DrawString("Text, Graphics, Images, and Forms", new XFont("Verdana", 10, XFontStyle.Regular), XBrushes.Navy, 3, 0, XStringFormats.TopLeft);
            XPen pen = XPens.LightBlue.Clone();
            pen.Width = 2.5;

            // ... graphics like Bézier curves
            formGfx.DrawBeziers(pen, XPoint.ParsePoints("30,120 80,20 100,140 175,33.3"));

            // ... raster images like GIF files
            XGraphicsState state = formGfx.Save();
            formGfx.RotateAtTransform(17, new XPoint(30, 30));
            formGfx.DrawImage(XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Test.gif"), 20, 20);
            formGfx.Restore(state);

            // ... and forms like XPdfForm objects
            state = formGfx.Save();
            formGfx.RotateAtTransform(-8, new XPoint(165, 115));
            formGfx.DrawImage(XPdfForm.FromFile("../../../../../PDFs/SomeLayout.pdf"), new XRect(140, 80, 50, 50 * System.Math.Sqrt(2)));
            formGfx.Restore(state);

            // When you finished drawing on the form, dispose the XGraphic object.
            formGfx.Dispose();
        }


    }
}
