namespace PASSIS.LIB.CustomClasses
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class GradeMovement
    {
        public long NewGradeId { get; set; }
        public long StudentId { get; set; }
        public long OldGradeId { get; set; }
    }

    [Serializable]
    public class MoveStudent
    {
        public long OldClassId { get; set; }
        public long OldGradeId { get; set; }
        public long NewClassId { get; set; }
        public long NewGradeId { get; set; }
        public long StudentId { get; set; }
        
    }
    
}

