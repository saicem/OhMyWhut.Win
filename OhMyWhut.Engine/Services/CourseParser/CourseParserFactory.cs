namespace OhMyWhut.Engine.Services.CourseParser
{
    using System;

    public class CourseParserFactory
    {
        private CourseParserFactory()
        {
        }

        public static CourseParserFactory Factory
        {
            get => new()
            {
                courseParserTypes = new Type[]
                {
                    typeof(BksCourseParser),
                    typeof(YjsCourseParser),
                }
            };
        }

        private Type[] courseParserTypes = null!;

        public ICourseParser? Create(Stream dataSource)
        {
            var str = new StreamReader(dataSource).ReadToEnd();
            return Create(str);
        }

        public ICourseParser? Create(string dataSource)
        {
            foreach (var parserType in courseParserTypes)
            {
                if (parserType.GetConstructor(new Type[] { typeof(string) })?.Invoke(new object[] { dataSource }) is not ICourseParser parser)
                {
                    throw new ArgumentException("未找到合适的CourseParser构造器");
                }

                if (parser.IsContentValid())
                {
                    return parser;
                }
            }

            return null;
        }
    }
}
