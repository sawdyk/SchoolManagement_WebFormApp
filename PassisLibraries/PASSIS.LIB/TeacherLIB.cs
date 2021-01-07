using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class TeacherLIB
    {
        public IList<Class_Grade> getTeacherAllClass_Grade(long? TeacherId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = (from getclassName in context.SubjectTeachers
                             where getclassName.TeacherId == TeacherId
                             where getclassName.GradeId == getclassName.Grade.Id
                             where getclassName.Grade.Class_Grade.Id == getclassName.Grade.ClassId
                             select getclassName.Grade.Class_Grade).Distinct();
            return allSchool.ToList<Class_Grade>();
        }


        public IList<SubjectTeacher> getTeacherSubjects(long? TeacherId, long? GradeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var teacherClassSubjects = from getSubjects in context.SubjectTeachers
                                       where getSubjects.TeacherId == TeacherId
                                       where getSubjects.GradeId == GradeId
                                       select getSubjects;

            return teacherClassSubjects.ToList<SubjectTeacher>();
        }

        public IList<SubjectTeacher> getTeacherSubjects(long? TeacherId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var teacherClassSubjects = from getSubjects in context.SubjectTeachers
                                       where getSubjects.TeacherId == TeacherId
                                       select getSubjects;

            return teacherClassSubjects.ToList<SubjectTeacher>();
        }


        public IList<Subject> GetAllSubject(int curriculumId , Int64 classid)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjects = from getSubjects in context.Subjects
                              where getSubjects.CurriculumId == curriculumId &&
                              getSubjects.ClassId == classid
                                       
                                       select getSubjects;

            return allSubjects.ToList<Subject>();
        }

        public IList<Subject> GetAllSubjectInClass(int curriculumId, Int64 classid, long schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            IList<Subject> AllSubject = new List<Subject>();
            //var getSubjectInClass = from s in context.SubjectsInSchools where s.SchoolId == schoolId select s.SubjectId;
            IList<SubjectsInSchool> getSubjectInClass = (from s in context.SubjectsInSchools where s.SchoolId == schoolId select s).ToList();
            IList<SubjectsInSchool> sortedTScore = getSubjectInClass.OrderBy(c => c.ReportCardOrder).ToList();

            //foreach (long subjectId in getSubjectInClass)
            //{
            //    Subject subject = context.Subjects.FirstOrDefault(x =>
            //                      x.CurriculumId == curriculumId &&
            //                      x.ClassId == classid && x.Id == subjectId);
            //    if (subject != null)
            //    {
            //        AllSubject.Add(subject);
            //    }
            //}
            foreach (SubjectsInSchool subjects in sortedTScore)
            {
                Subject subject = context.Subjects.FirstOrDefault(x =>
                                  x.CurriculumId == curriculumId &&
                                  x.ClassId == classid && x.Id == subjects.SubjectId);
                if (subject != null)
                {
                    AllSubject.Add(subject);
                }
            }
            return AllSubject.ToList<Subject>();
        }
    }
}
