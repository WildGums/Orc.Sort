namespace Orc.Sort.Tests.TemplateSort
{
    public class Foo
    {
        public string Bar { get; set; }
        public int Cho { get; set; }

        public static Foo Create(string bar, int cho)
        {
            return new Foo { Bar = bar, Cho = cho };
        }
    }
}
