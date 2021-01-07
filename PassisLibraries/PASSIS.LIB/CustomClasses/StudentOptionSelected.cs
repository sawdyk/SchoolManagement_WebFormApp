namespace PASSIS.LIB.CustomClasses
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class StudentOptionSelected
    {
        public long Id { get; set; }

        public long OptionGroupSubjectId { get; set; }

        public long SubjectId { get; set; }

        public long SubjectName { get; set; }

        public long TeacherId { get; set; }

        public long TeacherName { get; set; }

        public long yearId { get; set; }
    }
}

