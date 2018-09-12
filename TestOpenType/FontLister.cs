
namespace TestOpenType
{


    // https://stackoverflow.com/questions/16769758/get-a-font-filename-based-on-the-font-handle-hfont
    // https://www.codeproject.com/Articles/1235/Finding-a-Font-file-from-a-Font-name
    // https://www.codeproject.com/Articles/4190/XFont-Get-font-name-and-file-information
    public class FontLister
    {


        public static void Test()
        {
            string fonts = ListFontNames();
            fonts = ListFontNamesReverse();
            System.Console.WriteLine(fonts);
            // System.IO.File.WriteAllText(@"AllFonts.json", fonts);
            System.IO.File.WriteAllText(@"AllFontsReverse.json", fonts);

            string fontName = GetFontName(@"Arial");
            System.Console.WriteLine(fontName);
        }


        public static string ListFontNames()
        {
            string json;
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();

            string fonts = @"Software\Microsoft\Windows NT\CurrentVersion\Fonts";
            // fonts = @"Software\Microsoft\Windows\CurrentVersion\Fonts";

            using (Microsoft.Win32.RegistryKey fontsSubKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(fonts))
            {

                foreach (string valuename in fontsSubKey.GetValueNames())
                {
                    dict[valuename] = fontsSubKey.GetValue(valuename).ToString();
                } // Next valuename 

            } // End Using fontsSubKey 

            json = Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            return json;
        } // End Function ListFontNames 


        public static string ListFontNamesReverse()
        {
            string json;
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();

            string fonts = @"Software\Microsoft\Windows NT\CurrentVersion\Fonts";
            // fonts = @"Software\Microsoft\Windows\CurrentVersion\Fonts";

            using (Microsoft.Win32.RegistryKey fontsSubKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(fonts))
            {

                foreach (string valuename in fontsSubKey.GetValueNames())
                {
                    dict[fontsSubKey.GetValue(valuename).ToString()] = valuename;
                } // Next valuename 

            } // End Using fontsSubKey 


            json = Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            return json;
        } // End Function ListFontNames 


        public static string GetFontName(string fontName)
        {
            string fonts = @"Software\Microsoft\Windows NT\CurrentVersion\Fonts";
            // fonts = @"Software\Microsoft\Windows\CurrentVersion\Fonts";

            using (Microsoft.Win32.RegistryKey fontsSubKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(fonts))
            {

                foreach (string valuename in fontsSubKey.GetValueNames())
                {

                    if (valuename.StartsWith(fontName))
                    {
                        return fontsSubKey.GetValue(valuename).ToString();
                    } // End if (valuename.StartsWith(fontname)) 

                } // Next valuename 

            } // End Using fontsSubKey 

            return string.Empty;
        }


    } // End Class FontLister 


} // End Namespace TestOpenType 
