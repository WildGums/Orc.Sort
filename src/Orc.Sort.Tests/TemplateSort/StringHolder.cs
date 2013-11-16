namespace Orc.Sort.Tests.TemplateSort
{
    using System;

    public class StringHolder
    {
        string _s;

        public StringHolder(String s)
        {
            _s = s;
        }

        public string String
        {
            get { return _s; }
            set { _s = value; }
        }

        public static StringHolder[] FromStringArray(string[] stringArray)
        {
            StringHolder[] holder = new StringHolder[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                holder[i] = new StringHolder(stringArray[i]);
            return holder;
        }
    }
}
