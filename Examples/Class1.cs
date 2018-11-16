﻿
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Examples
{


    public class Class1
    {


        public static void Test()
        {
            string fn = @"D:\username\Desktop\BuildingXY-Floor14.pdf";

            using (PdfDocument destDocument = new PdfDocument())
            {
                PdfPage destPage = destDocument.AddPage();

                using (XGraphics gfx = XGraphics.FromPdfPage(destPage))
                {

                    using (PdfSharp.Drawing.XPdfForm sourceForm = PdfSharp.Drawing.XPdfForm.FromFile(fn))
                    {
                        DrawFormWithOptions(gfx, sourceForm);
                    }

                }

            }

        }

        public static void DrawFormWithOptions(XGraphics gfx, XForm form)
        {
            // Draw the form on the page of the document in its original size
            gfx.DrawImage(form, 20, 50);

            // Draw it stretched
            gfx.DrawImage(form, 300, 100, 250, 40);

            // Draw and rotate it
            const int d = 25;
            for (int idx = 0; idx < 360; idx += d)
            {
                gfx.DrawImage(form, 300, 480, 200, 200);
                gfx.RotateAtTransform(d, new XPoint(300, 480));
            }
        }


        public static void DrawBeziers()
        {
            string fn = @"input.pdf";
            using (PdfSharp.Pdf.PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(fn))
            {

                // Create an empty XForm object with the specified width and height
                // A form is bound to its target document when it is created. The reason is that the form can 
                // share fonts and other objects with its target document.
                using (XForm form = new XForm(document, XUnit.FromMillimeter(70), XUnit.FromMillimeter(55)))
                {
                    // Create an XGraphics object for drawing the contents of the form.
                    using (XGraphics formGfx = XGraphics.FromForm(form))
                    {

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

                    } // End Using formGfx 

                } // End Using form 

            } // End Using document 

        }


    }
}
