
namespace Examples
{


    // http://www.pdfsharp.net/wiki/FontResolver-sample.ashx
    class SegoeWpFontResolver 
        : PdfSharp.Fonts.IFontResolver
    {

        public static void Test()
        {
            // Register font resolver before start using PDFsharp.
            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new SegoeWpFontResolver();
        }


        byte[] PdfSharp.Fonts.IFontResolver.GetFont(string faceName)
        {
            throw new System.NotImplementedException();
        }


        PdfSharp.Fonts.FontResolverInfo PdfSharp.Fonts.IFontResolver.
            ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            throw new System.NotImplementedException();
        }

    } // End Class SegoeWpFontResolver 


} // End Namespace Examples 
