
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;


namespace Examples
{


    class SplitDocument
    {


        public static void Test()
        {
            // Get a fresh copy of the sample PDF file
            const string filename = "Portable Document Format.pdf";
            System.IO.File.Copy(System.IO.Path.Combine("../../../../../PDFs/", filename),
              System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), filename), true);

            // Open the file
            PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import);

            string name = System.IO.Path.GetFileNameWithoutExtension(filename);
            for (int idx = 0; idx < inputDocument.PageCount; idx++)
            {
                // Create new document
                PdfDocument outputDocument = new PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Title =
                  string.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[idx]);
                outputDocument.Save(string.Format("{0} - Page {1}_tempfile.pdf", name, idx + 1));
            }
        }


    }
}
