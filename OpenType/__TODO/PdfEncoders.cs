
namespace PdfSharp.Pdf.Internal
{


    internal class PdfEncoders
    {
        static System.Text.Encoding _unicodeEncoding;
        static System.Text.Encoding _winAnsiEncoding;


        /// <summary>
        /// Gets the UNICODE little-endian encoding.
        /// </summary>
        public static System.Text.Encoding UnicodeEncoding
        {
            get { return _unicodeEncoding ?? (_unicodeEncoding = System.Text.Encoding.Unicode); }
        } // End Property UnicodeEncoding 


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

        } // End Property WinAnsiEncoding 


    } // End Class PdfEncoders 


} // End Namespace PdfSharp.Pdf.Internal 
