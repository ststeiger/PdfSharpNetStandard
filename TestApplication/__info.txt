D:\username\Documents\visual studio 2017\Projects\PdfSharp\PdfSharp\Fonts.OpenType\OpenTypeDescriptor.cs

D:\username\Documents\visual studio 2017\Projects\PdfSharp\PdfSharp\Fonts.OpenType\OpenTypeFontface.cs
D:\username\Documents\visual studio 2017\Projects\PdfSharp\PdfSharp\Drawing\XFontMetrics.cs
D:\username\Documents\visual studio 2017\Projects\PdfSharp\PdfSharp\Drawing\XGlyphTypeface.cs



OpenTypeFontface subSet = subSet = FontDescriptor._descriptor.FontFace.CreateFontSubSet(_cmapInfo.GlyphIndices, true);
byte[] fontData = subSet.FontSource.Bytes;


PdfSharp.Pdf.Advanced.PdfFont

PdfSharp.Drawing.XFont font
GetOrCreateDescriptorFor(XFont font)
OpenTypeDescriptor ttDescriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptorFor(font);


       internal CMapInfo _cmapInfo;

    _cmapInfo = new CMapInfo(ttDescriptor);




        internal void AddChars(string text)
        {
            if (_cmapInfo != null)
                _cmapInfo.AddChars(text);
        }



        internal void AddGlyphIndices(string glyphIndices)
        {
            if (_cmapInfo != null)
                _cmapInfo.AddGlyphIndices(glyphIndices);
        }

