
using PdfSharp.Pdf.Security;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;


namespace Examples
{

    // http://www.pdfsharp.net/wiki/ProtectDocument-sample.ashx
    class ProtectDocument
    {

        public static void Test()
        {
            // Get a fresh copy of the sample PDF file
            const string filenameSource = "HelloWorld.pdf";
            const string filenameDest = "HelloWorld_tempfile.pdf";
            System.IO.File.Copy(System.IO.Path.Combine("../../../../../PDFs/", filenameSource),
              System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), filenameDest), true);

            // Open an existing document. Providing an unrequired password is ignored.
            PdfDocument document = PdfReader.Open(filenameDest, "some text");

            PdfSecuritySettings securitySettings = document.SecuritySettings;

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

            // Save the document...
            document.Save(filenameDest);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filenameDest);
        }

    }
}
