
namespace Examples
{


    // https://stackoverflow.com/questions/521146/c-sharp-split-string-but-keep-split-chars-separators
    class SvgViewBoxSplitting
    {

        // Float validator regex: http://regexstorm.net/tester
        // [\+\-]?\s*[0-9]*(?:\.[0-9]*)


        public static string[] SplitAndKeepSeparators(string value, System.StringSplitOptions splitOptions, params char[] separators)
        {
            System.Collections.Generic.List<string> splitValues =
                new System.Collections.Generic.List<string>();

            int itemStart = 0;
            for (int pos = 0; pos < value.Length; pos++)
            {
                for (int sepIndex = 0; sepIndex < separators.Length; sepIndex++)
                {
                    if (separators[sepIndex] == value[pos])
                    {
                        // add the section of string before the separator 
                        // (unless its empty and we are discarding empty sections)
                        if (itemStart != pos || splitOptions == System.StringSplitOptions.None)
                        {
                            splitValues.Add(value.Substring(itemStart, pos - itemStart));
                        }
                        itemStart = pos + 1;

                        // add the separator
                        splitValues.Add(separators[sepIndex].ToString());
                        break;
                    }
                }
            }

            // add anything after the final separator 
            // (unless its empty and we are discarding empty sections)
            if (itemStart != value.Length || splitOptions == System.StringSplitOptions.None)
            {
                splitValues.Add(value.Substring(itemStart, value.Length - itemStart));
            }

            return splitValues.ToArray();
        }

        public static string[] SplitAndKeepSeparators(string value, params char[] separators)
        {
            return SplitAndKeepSeparators(value, System.StringSplitOptions.RemoveEmptyEntries, separators);
        }


        public static System.Collections.Generic.IEnumerable<string> SplitAndKeepEnumerable1(string s, params char[] delims)
        {
            int start = 0, index;

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    yield return s.Substring(start, index - start);

                yield return s.Substring(index, 1);
                start = index + 1;
            }

            if (start < s.Length)
            {
                yield return s.Substring(start);
            }

        }


        public static string[] SplitAndKeep1(string s, params char[] delims)
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
            int start = 0, index;

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    ls.Add(s.Substring(start, index - start));

                ls.Add(s.Substring(index, 1));
                start = index + 1;
            }

            if (start < s.Length)
            {
                ls.Add(s.Substring(start));
            }

            return ls.ToArray();
        }


        public static System.Collections.Generic.IEnumerable<string> SplitAndKeepEnumerable(string s, params char[] delims)
        {
            if (string.IsNullOrEmpty(s))
                yield return null;

            int start = 0, index;

            string separator = "";

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    yield return Trim(separator) + s.Substring(start, index - start);

                separator = s.Substring(index, 1);

                start = index + 1;
            }

            if (start < s.Length)
            {
                yield return separator + s.Substring(start);
            }

        }


        public static string[] SplitAndKeep(string s, params char[] delims)
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            if (string.IsNullOrEmpty(s))
                return new string[0];

            int start = 0, index;

            string separator = "";

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    ls.Add(Trim(separator) + s.Substring(start, index - start));

                separator = s.Substring(index, 1);

                start = index + 1;
            }

            if (start < s.Length)
            {
                ls.Add(separator + s.Substring(start));
            }

            return ls.ToArray();
        }


        public static string Trim(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = input.Trim(' ', '\t', '\v', '\r', '\n', ',', '+');

            return input;
        }


        public static int IndexOfAnyNonExponential(string s, char[] delims, int start)
        {
            int ret = s.IndexOfAny(delims, start);

            if (ret > 0)
            {
                char c = s[ret - 1];
                if (c == 'E' || c == 'e')
                {
                    start = ret + 1;
                    return IndexOfAnyNonExponential(s, delims, start);
                } // End if (c == 'E' || c == 'e')

            } // End if (ret > 0) 

            return ret; // ret [-1, 0] + ret 
        } // End Function IndexOfAnyNonExponential


        public static string[] SplitSvgViewBox(string s, params char[] delims)
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            if (string.IsNullOrEmpty(s))
                return new string[0];

            int start = 0, index;

            string separator = "";

            while ((index = IndexOfAnyNonExponential(s, delims, start)) != -1)
            {
                if (index - start > 0)
                    ls.Add(Trim(separator) + s.Substring(start, index - start));

                separator = s.Substring(index, 1);
                start = index + 1;
            } // Whend 

            if (start < s.Length)
            {
                ls.Add(separator + s.Substring(start));
            }

            for (int i = ls.Count - 1; i > -1; --i)
            {
                if (ls[i].StartsWith("."))
                    ls[i] = "0" + ls[i];
                else if (ls[i] == "-." || ls[i] == "+.")  // OMG
                    ls.RemoveAt(i);
            }

            return ls.ToArray();
        }


        public static string[] SplitSvgViewBox(string input)
        {
            return SplitSvgViewBox(input, ' ', '\t', '\v', '\r', '\n', ',', '+', '-');
        }


        public static decimal[] SplitViewBox(string input)
        {
            System.Collections.Generic.List<string> results = new System.Collections.Generic.List<string>();

            // params char[] delims
            // delims = new char[] { ' ', '\t', '\v', '\r', '\n', ',' , '+', '-' };
            // string delimiters = new string(delims);

            string delimiters = " \t\v\r\n,+-";
            string previous = null;
            System.Collections.Generic.List<char> ls = new System.Collections.Generic.List<char>();

            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];
                int pos = delimiters.IndexOf(c);
                if (pos != -1)
                {
                    char splitChar = delimiters[pos];

                    if (previous == "E" || previous == "e")
                    {
                        ls.Add(c);
                    }
                    else
                    {
                        if (ls.Count > 0)
                        {
                            string s = new string(ls.ToArray());
                            if (s != "-" && s != "+" && s != "." && s != "-." && s != "+.")
                            {
                                if (s.StartsWith("."))
                                    s = "0" + s;
                                results.Add(s);
                            }

                        } // End if (ls.Count > 0)

                        ls.Clear();
                        if (splitChar == '+' || splitChar == '-')
                            ls.Add(splitChar);
                    } // End else of if (previous == "E" || previous == "e") 
                }
                else
                    ls.Add(c);

                previous = c.ToString();
            } // Next i 


            decimal[] viewBoxValues = new decimal[results.Count];

            for (int i = 0; i < results.Count; ++i)
            {
                viewBoxValues[i] = System.Decimal.Parse(results[i], System.Globalization.NumberStyles.Float);
            }

            return viewBoxValues;
        } // End Function SplitViewBox 


        public static void TestViewBoxSplitting()
        {
            decimal d1 = decimal.Parse("0.", System.Globalization.NumberStyles.Float);
            decimal d2 = decimal.Parse(".0", System.Globalization.NumberStyles.Float);
            // decimal d3 = decimal.Parse(".", System.Globalization.NumberStyles.Float);
            // decimal d4 = decimal.Parse("", System.Globalization.NumberStyles.Float);
            System.Console.WriteLine(d1);
            System.Console.WriteLine(d2);


            decimal[] viewBoxValues1 = SplitViewBox("----2E4,,5E-3,12,0,+12-13.13, ,.0   ,14 -15 2E3");
            decimal[] viewBoxValues2 = SplitViewBox("5E-3,12,0,+12-13.13, ,.0   ,14 -15 2E12 5---.");
            System.Console.WriteLine("{0} {1}", viewBoxValues1, viewBoxValues2);


            // string[] points = SplitSvgViewBox("2E4,,5E-3,12,0,+12-13.13, ,.0   ,14 -15 2E3132 5");
            // string[] points = SplitSvgViewBox("5E-3,12,0,+12-13.13, ,.0   ,14 -15 2E3132 5---.");
            string[] points = SplitSvgViewBox("5E-3,12,0,+12-13.13, ,.0   ,14 -15 2E12 5---.");

            foreach (string num in points)
            {
                decimal dec = System.Decimal.Parse(num, System.Globalization.NumberStyles.Float);
            }

            System.Console.WriteLine(points);
        }


    }


}
