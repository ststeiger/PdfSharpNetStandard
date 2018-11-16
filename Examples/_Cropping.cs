
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace Examples
{

    // https://www.codeproject.com/Articles/20245/WPF-Interactive-Image-Cropping-Control#HowItWorks
    public class Cropping
    {


        public static void Test()
        {
            using (PdfDocument document = new PdfDocument())
            {

                PdfPage page = document.AddPage();
                page.Width = XUnit.FromMillimeter(80).Point;
                page.Height = XUnit.FromMillimeter(800).Point;

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    XFont fontTicket = new XFont("Courier New", 9, XFontStyle.Regular);

                    int baseX = 5;
                    int baseY = 10;

                    gfx.DrawString("************************************", fontTicket, XBrushes.Black, XUnit.FromMillimeter(baseX), XUnit.FromMillimeter(baseY += 5));
                    gfx.DrawString("***            Ticket            ***", fontTicket, XBrushes.Black, XUnit.FromMillimeter(baseX), XUnit.FromMillimeter(baseY += 5));
                    gfx.DrawString("************************************", fontTicket, XBrushes.Black, XUnit.FromMillimeter(baseX), XUnit.FromMillimeter(baseY += 5));

                    // Crop the PDF dimensions at the current height
                    double height = XUnit.FromMillimeter(baseY + 10).Point;
                    page.CropBox = new PdfRectangle(new XPoint(0, page.Height - height),
                                                    new XSize(page.Width, height));

                    document.Save("ticket.pdf");
                } // End Using gfx 

            }// End Using document 

        } // End Sub 


    }


}
