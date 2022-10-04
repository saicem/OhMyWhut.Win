namespace OhMyWhut.Engine.Services.CourseParser
{
    using OhMyWhut.Engine.Data;
    using System.Collections.Generic;

    public interface ICourseParser
    {
        IEnumerable<Course> Parse();

        bool IsContentValid();
    }
}
