
namespace System.Drawing 
{
    public static class Color2
    {

        public static Color FromKnownColor(KnownColor color)
        {
            //return color <= 0 || color > KnownColor.MenuHighlight ? FromName(color.ToString()) : new Color(color);
            return FromName(color.ToString());
        }

        private const long NotDefinedValue = 0;
        private const short StateNameValid = 0x0008;

        public static Color FromName(string name)
        {
            // try to get a known color first
            if (ColorTable.TryGetNamedColor(name, out Color color))
                return color;

            // otherwise treat it as a named color
            return ToColor(NotDefinedValue, StateNameValid, name, (KnownColor)0);
        }


        private static System.Drawing.Color ToColor(long value, short state, string name, KnownColor knownColor)
        {
            System.Drawing.Color r = System.Drawing.Color.FromArgb((int)value);

            // this.value = value;
            // this.state = state;
            // this.name = name;
            // this.knownColor = unchecked((short)knownColor);

            return r;
        }

    }
}
