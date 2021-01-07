namespace PASSIS.LIB.CustomClasses
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SubjectTeachers
    {
        public long GradeId { get; set; }

        public long Id { get; set; }

        public long SchoolId { get; set; }

        public long SubjectId { get; set; }

        public long TeacherId { get; set; }
    }
}

