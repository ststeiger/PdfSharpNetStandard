
namespace Examples
{


    // http://www.pdfsharp.net/wiki/PrivateFonts-sample.ashx
    class PrivateFonts
    {
        //private Dictionary<string, System.Windows.Media.FontFamily> fontFamilies;
        private System.Collections.Generic.Dictionary<string, string> fontFamilies =
            new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);


        public void Add(System.Uri baseUri, string familyName)
        {
            if (string.IsNullOrEmpty(familyName))
                throw new System.ArgumentNullException("familyName");
            if (familyName.Contains(","))
                throw new System.NotImplementedException("Only one family name is supported.");

            // family name starts right of '#'
            int idxHash = familyName.IndexOf('#');
            if (idxHash < 0)
                throw new System.ArgumentException("Family name must contain a '#'. Example './#MyFontFamilyName'", "familyName");

            string key = familyName.Substring(idxHash + 1);
            if (string.IsNullOrEmpty(key))
                throw new System.ArgumentException("familyName has invalid format.");

            if (this.fontFamilies.ContainsKey(key))
                throw new System.ArgumentException("An entry with the specified family name already exists.");

            // System.Windows.Media.FontFamily fontFamily = new System.Windows.Media.FontFamily(baseUri, familyName);
            // this.fontFamilies.Add(key, fontFamily);
        }


    }
}
