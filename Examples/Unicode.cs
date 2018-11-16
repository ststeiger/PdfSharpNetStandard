using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples
{
    class Unicode
    {

        static readonly string[] texts = new string[]
{
  // International version of the text in English
  "English\n" +
  "PDFsharp is a .NET library for creating and processing PDF documents 'on the fly'. " +
  "The library is completely written in C# and based exclusively on safe, managed code. " +
  "PDFsharp offers two powerful abstraction levels to create and process PDF documents.\n" +
  "For drawing text, graphics, and images there is a set of classes which are modeled similar to the classes " +
  "of the name space System.Drawing of the .NET framework. With these classes it is not only possible to create " +
  "the content of PDF pages in an easy way, but they can also be used to draw in a window or on a printer.\n" +
  "Additionally PDFsharp completely models the structure elements PDF is based on. With them existing PDF documents " +
  "can be modified, merged, or split with ease.\n" +
  "The source code of PDFsharp is Open Source under the MIT license (http://en.wikipedia.org/wiki/MIT_License). " +
  "Therefore it is possible to use PDFsharp without limitations in non open source or commercial projects/products.",
 
  // PDFsharp is 'Made in Germany'
  "German\n" +
  "PDFsharp ist eine .NET-Bibliothek zum Erzeugen und Verarbeiten von PDF-Dokumenten 'On the Fly'. " +
  "Die Bibliothek ist vollständig in C# geschrieben und basiert ausschließlich auf sicherem, verwaltetem Code. " +
  "PDFsharp bietet zwei leistungsstarke Abstraktionsebenen zur Erstellung und Verarbeitung von PDF-Dokumenten.\n" +
  "Zum Zeichnen von Text, Grafik und Bildern gibt es einen Satz von Klassen, die sehr stark an die Klassen " +
  "des Namensraums System.Drawing des .NET Frameworks angelehnt sind. Mit diesen Klassen ist es nicht " +
  "nur auf einfache Weise möglich, den Inhalt von PDF-Seiten zu gestalten, sondern sie können auch zum " +
  "Zeichnen in einem Fenster oder auf einem Drucker verwendet werden.\n" +
  "Zusätzlich modelliert PDFsharp vollständig die Stukturelemente, auf denen PDF basiert. Dadurch können existierende " +
  "PDF-Dokumente mit Leichtigkeit zerlegt, ergänzt oder umgebaut werden.\n" +
  "Der Quellcode von PDFsharp ist Open-Source unter der MIT-Lizenz (http://de.wikipedia.org/wiki/MIT-Lizenz). " +
  "Damit kann PDFsharp auch uneingeschränkt in Nicht-Open-Source- oder kommerziellen Projekten/Produkten eingesetzt werden.",
 
  // Greek version
  // The text was translated by Babel Fish. We here in Germany have no idea what it means.
  // If you are a native speaker please correct it and mail it to mailto: ((see Impressum))
  "Greek (Translated with Babel Fish)\n" +
  "Το PDFsharp είναι βιβλιοθήκη δικτύου α. για τη δημιουργία και την επεξεργασία των εγγράφων PDF 'σχετικά με τη μύγα'. "+
  "Η βιβλιοθήκη γράφεται εντελώς γ # και βασίζεται αποκλειστικά εκτός από, διοικούμενος κώδικας. "+
  "Το PDFsharp προσφέρει δύο ισχυρά επίπεδα αφαίρεσης για να δημιουργήσει και να επεξεργαστεί τα έγγραφα PDF. "+
  "Για το κείμενο, τη γραφική παράσταση, και τις εικόνες σχεδίων υπάρχει ένα σύνολο κατηγοριών που διαμορφώνονται "+
  "παρόμοιος με τις κατηγορίες του διαστημικού σχεδίου συστημάτων ονόματος του. πλαισίου δικτύου. "+
  "Με αυτές τις κατηγορίες που είναι όχι μόνο δυνατό να δημιουργηθεί το περιεχόμενο των σελίδων PDF με έναν εύκολο "+
  "τρόπο, αλλά αυτοί μπορεί επίσης να χρησιμοποιηθεί για να επισύρει την προσοχή σε ένα παράθυρο ή σε έναν εκτυπωτή. "+
  "Επιπλέον PDFsharp διαμορφώνει εντελώς τα στοιχεία PDF δομών είναι βασισμένο. Με τους τα υπάρχοντα έγγραφα PDF "+
  "μπορούν να τροποποιηθούν, συγχωνευμένος, ή να χωρίσουν με την ευκολία. Ο κώδικας πηγής PDFsharp είναι ανοικτή πηγή "+
  "με άδεια MIT (http://en.wikipedia.org/wiki/MIT_License). Επομένως είναι δυνατό να χρησιμοποιηθεί PDFsharp χωρίς "+
  "προβλήματα στη μη ανοικτή πηγή ή τα εμπορικά προγράμματα/τα προϊόντα.",
 
  // Russian version (by courtesy of Alexey Kuznetsov)
  "Russian\n" +
  "PDFsharp это .NET библиотека для создания и обработки PDF документов 'налету'. " +
  "Библиотека полностью написана на языке C# и базируется исключительно на безопасном, управляемом коде. " +
  "PDFsharp использует два мощных абстрактных уровня для создания и обработки PDF документов.\n" +
  "Для рисования текста, графики, и изображений в ней используется набор классов, которые разработаны аналогично с" +
  "пакетом System.Drawing, библиотеки .NET framework. С помощью этих классов возможно не только создавать" +
  "содержимое PDF страниц очень легко, но они так же позволяют рисовать напрямую в окне приложения или на принтере.\n" +
  "Дополнительно PDFsharp имеет полноценные модели структурированных базовых элементов PDF. Они позволяют работать с существующим PDF документами " +
  "для изменения их содержимого, склеивания документов, или разделения на части.\n" +
  "Исходный код PDFsharp библиотеки это Open Source распространяемый под лицензией MIT (http://ru.wikipedia.org/wiki/MIT_License). " +
  "Теоретически она позволяет использовать PDFsharp без ограничений в не open source проектах или коммерческих проектах/продуктах.",
 
  // Your language may come here
  "Invitation\n" +
  "If you use PDFsharp and haven't found your native language in this document, we will be pleased to get your translation of the text above and include it here.\n" +
  "Mail to ((see Impressum))"
};

        public static void Test()
        {
            // Create new document
            PdfDocument document = new PdfDocument();

            // Set font encoding to unicode
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            XFont font = new XFont("Times New Roman", 12, XFontStyle.Regular, options);

            // Draw text in different languages
            for (int idx = 0; idx < texts.Length; idx++)
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatter tf = new XTextFormatter(gfx);
                tf.Alignment = XParagraphAlignment.Left;

                tf.DrawString(texts[idx], font, XBrushes.Black,
                  new XRect(100, 100, page.Width - 200, 600), XStringFormats.TopLeft);
            }

            const string filename = "Unicode_tempfile.pdf";
            // Save the document...
            document.Save(filename);
            // ...and start a viewer.
            System.Diagnostics.Process.Start(filename);
        }

    }
}
