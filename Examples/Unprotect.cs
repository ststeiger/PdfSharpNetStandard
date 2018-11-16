
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf.Security;


namespace Examples
{


    // http://www.pdfsharp.net/wiki/UnprotectDocument-sample.ashx
    class Unprotect
    {

        /// <summary>
        /// The 'get the password' call back function.
        /// </summary>
        static void PasswordProvider(PdfPasswordProviderArgs args)
        {
            // Show a dialog here in a real application
            args.Password = "owner";
        }


        public static void Test()
        {
            // Get a fresh copy of the sample PDF file.
            // The passwords are 'user' and 'owner' in this sample.
            const string filenameSource = "HelloWorld (protected).pdf";
            const string filenameDest = "HelloWorld_tempfile.pdf";
            System.IO.File.Copy(System.IO.Path.Combine("../../../../../PDFs/", filenameSource),
              System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), filenameDest), true);

            PdfDocument document;

            // Opening a document will fail with an invalid password.
            try
            {
                document = PdfSharp.Pdf.IO.PdfReader.Open(filenameDest, "invalid password");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            // You can specifiy a delegate, which is called if the document needs a
            // password. If you want to modify the document, you must provide the
            // owner password.
            document = PdfReader.Open(filenameDest, PdfDocumentOpenMode.Modify, PasswordProvider);

            // Open the document with the user password.
            document = PdfReader.Open(filenameDest, "user", PdfDocumentOpenMode.ReadOnly);

            // Use the property HasOwnerPermissions to decide whether the used password
            // was the user or the owner password. In both cases PDFsharp provides full
            // access to the PDF document. It is up to the programmer who uses PDFsharp
            // to honor the access rights. PDFsharp doesn't try to protect the document
            // because this make little sence for an open source library.
            bool hasOwnerAccess = document.SecuritySettings.HasOwnerPermissions;

            // Open the document with the owner password.
            document = PdfReader.Open(filenameDest, "owner");
            hasOwnerAccess = document.SecuritySettings.HasOwnerPermissions;

            // A document opened with the owner password is completely unprotected
            // and can be modified.
            XGraphics gfx = XGraphics.FromPdfPage(document.Pages[0]);
            gfx.DrawString("Some text...",
              new XFont("Times New Roman", 12), XBrushes.Firebrick, 50, 100);

            // The modified document is saved without any protection applied.
            PdfDocumentSecurityLevel level = document.SecuritySettings.DocumentSecurityLevel;

            // If you want to save it protected, you must set the DocumentSecurityLevel
            // or apply new passwords.
            // In the current implementation the old passwords are not automatically
            // reused. See 'ProtectDocument' sample for further information.

            // Save the document...
            document.Save(filenameDest);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filenameDest);
        }

    }
}
