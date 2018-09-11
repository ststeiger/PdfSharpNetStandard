using System;

namespace OpenType
{
    public class Class1
    {
    }
}




namespace PdfSharp.Drawing
{
    public class XGraphics
    {
    }

    public class XPdfFontOptions
    { }

}


namespace PdfSharp.Pdf.Internal
{
    internal class PdfEncoders
    {
        static System.Text.Encoding _winAnsiEncoding;

        /// <summary>
        /// Gets the Windows 1252 (ANSI) encoding.
        /// </summary>
        public static System.Text.Encoding WinAnsiEncoding
        {
            get
            {
                if (_winAnsiEncoding == null)
                {
#if !SILVERLIGHT && !NETFX_CORE && !UWP
                    // Use .net encoder if available.
                    _winAnsiEncoding = System.Text.Encoding.GetEncoding(1252);
#else
                    // Use own implementation in Silverlight and WinRT
                    _winAnsiEncoding = new AnsiEncoding();
#endif
                }
                return _winAnsiEncoding;
            }
        }
    }
}