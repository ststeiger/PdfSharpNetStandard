
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace Examples
{


    class Watermark
    {


        public static void Variant1(PdfPage page, XFont font, string watermark)
        {
            // Variation 1: Draw a watermark as a text string.

            // Get an XGraphics object for drawing beneath the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

            // Get the size (in points) of the text.
            var size = gfx.MeasureString(watermark, font);

            // Define a rotation transformation at the center of the page.
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-System.Math.Atan(page.Height / page.Width) * 180 / System.Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

            // Create a string format.
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Create a dimmed red brush.
            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));

            // Draw the string.
            gfx.DrawString(watermark, font, brush,
                new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
                format);
        }


        public static void Variant2(PdfPage page, XFont font, string watermark)
        {
            // Variation 2: Draw a watermark as an outlined graphical path.
            // NYI: Does not work in Core build.

            // Get an XGraphics object for drawing beneath the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

            // Get the size (in points) of the text.
            var size = gfx.MeasureString(watermark, font);

            // Define a rotation transformation at the center of the page.
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-System.Math.Atan(page.Height / page.Width) * 180 / System.Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

            // Create a graphical path.
            var path = new XGraphicsPath();

            // Create a string format.
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Add the text to the path.
            // AddString is not implemented in PDFsharp Core.
            path.AddString(watermark, font.FontFamily, XFontStyle.BoldItalic, 150,
            new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
                format);

            // Create a dimmed red pen.
            var pen = new XPen(XColor.FromArgb(128, 255, 0, 0), 2);

            // Stroke the outline of the path.
            gfx.DrawPath(pen, path);
        }

        public static void Variant3(PdfPage page, XFont font, string watermark)
        {
            // Variation 3: Draw a watermark as a transparent graphical path above text.
            // NYI: Does not work in Core build.

            // Get an XGraphics object for drawing above the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);

            // Get the size (in points) of the text.
            var size = gfx.MeasureString(watermark, font);

            // Define a rotation transformation at the center of the page.
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-System.Math.Atan(page.Height / page.Width) * 180 / System.Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

            // Create a graphical path.
            var path = new XGraphicsPath();

            // Create a string format.
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Add the text to the path.
            // AddString is not implemented in PDFsharp Core.
            path.AddString(watermark, font.FontFamily, XFontStyle.BoldItalic, 150,
                new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
                format);

            // Create a dimmed red pen and brush.
            var pen = new XPen(XColor.FromArgb(50, 75, 0, 130), 3);
            XBrush brush = new XSolidBrush(XColor.FromArgb(50, 106, 90, 205));

            // Stroke the outline of the path.
            gfx.DrawPath(pen, brush, path);
        }


    }
}
