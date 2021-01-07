namespace PASSIS.LIB.CustomClasses
{
    using System;
    using System.Runtime.CompilerServices;

    public class ResultSummaryStatistics
    {
        public decimal FemaleAverageScore { get; set; }

        public long Id { get; set; }

        public decimal MaleAverageScore { get; set; }

        public long StudentWithA { get; set; }

        public long StudentWithB { get; set; }

        public long StudentWithC { get; set; }

        public long StudentWithD { get; set; }

        public long StudentWithE { get; set; }

        public decimal SubjectAverageScore { get; set; }

        public long TotalNumberOfStudents { get; set; }
    }
}

