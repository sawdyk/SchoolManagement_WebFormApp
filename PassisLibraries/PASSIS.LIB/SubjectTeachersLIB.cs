using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class SubjectTeachersLIB
    {
        public void SaveSubjectTeacher(PASSIS.LIB.SubjectTeacher st)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.SubjectTeachers.InsertOnSubmit(st);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public SubjectTeacher RetrieveSubjectTeacher(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            SubjectTeacher usr = context.SubjectTeachers.FirstOrDefault(s => s.Id == Id);
            return usr;
        }
        public IList<SubjectTeacher> getAllSubjectTeachers()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = context.SubjectTeachers;
            return allSubjectTeachers.ToList<SubjectTeacher>();
        }
        public IList<SubjectTeacher> getAllSubjectTeachers(Int64 subjectId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = from st in context.SubjectTeachers where st.SubjectId == subjectId select st;
            return allSubjectTeachers.ToList<SubjectTeacher>();
        }
        public IList<Int64> getAllSubjectTeachersId(Int64 subjectId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = from st in context.SubjectTeachers where st.SubjectId == subjectId select (long)st.TeacherId;
            return allSubjectTeachers.ToList<Int64>();
        }
        public IList<Int64> getAllSubjectTeachersId(Int64 campusId, Int64 yearId, Int64 subjectId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = from st in context.SubjectTeachers where st.SubjectId == subjectId && st.Grade.SchoolCampusId == campusId && st.Grade.ClassId == yearId select (long)st.TeacherId;
            return allSubjectTeachers.ToList<Int64>();
        }
        public IList<User> getAllSubjectTeachersUserObj(Int64 subjectId)
        {
            var allUser = (System.Linq.IQueryable<User>)null;
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            var allSubjectTeachers = from st in context.SubjectTeachers where st.SubjectId == subjectId select (long)st.TeacherId;
            if (allSubjectTeachers.ToList<Int64>().Count != 0)
                allUser = from usr in context.Users where allSubjectTeachers.ToList<Int64>().Contains(usr.Id) select usr;

            return allUser.ToList<User>();
        }
        public IList<SubjectTeacher> getAllSubjectTeacherBySchool(Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = from st in context.SubjectTeachers where st.SchoolId == schoolId select st;

            return allSubjectTeachers.ToList<SubjectTeacher>();
        }
        public IList<SubjectTeacher> getAllSubjectTeacherBySchool(Int64 schoolId, Int64 campusId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = from st in context.SubjectTeachers where st.SchoolId == schoolId && st.Grade.SchoolCampusId == campusId select st;

            return allSubjectTeachers.ToList<SubjectTeacher>();
        }
        /// <summary>
        /// userId is equivalent to teacherID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<SubjectTeacher> getAllTeacherSubject(Int64 userId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectTeachers = from st in context.SubjectTeachers where st.TeacherId == userId select st;

            return allSubjectTeachers.ToList<SubjectTeacher>();
        }
        public IList<Subject> getAllSubjectsInSchool(Int64 classId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allSubjects = from sb in context.Subjects where sb.ClassId == classId select sb;
                return allSubjects.ToList<Subject>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IList<SubjectsInSchool> getAllSubjectsForClass(Int64 schoolId, Int64 classId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allSubjects = from sb in context.SubjectsInSchools where sb.SchoolId == schoolId && sb.Subject.ClassId == classId select sb;
                return allSubjects.ToList<SubjectsInSchool>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IList<SubjectsInSchool> getAllSubjectsForClassAndTeacher(Int64 schoolId, Int64 classId)
        {
            IList<SubjectsInSchool> listOfSub = new List<SubjectsInSchool>();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allSubjects = from sb in context.SubjectsInSchools where sb.SchoolId == schoolId && sb.Subject.ClassId == classId select sb;
                foreach (SubjectsInSchool sub in allSubjects) 
                {
                    SubjectTeacher subTecher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == sub.SubjectId && x.SchoolId == schoolId);
                    if (subTecher != null) 
                    {
                        listOfSub.Add(sub);
                    }
                }
                return listOfSub.ToList<SubjectsInSchool>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IList<SubjectsInSchool> getAllSubjects(Int64 schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allSubjects = from sb in context.SubjectsInSchools where sb.SchoolId == schoolId select sb;
                return allSubjects.ToList<SubjectsInSchool>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public Subject RetrieveSubject(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Subject usr = context.Subjects.FirstOrDefault(s => s.Id == Id);
            return usr;
        }
        public SubjectsInSchool RetrieveSubjectByCode(Int64 schoolId, string code, Int64 idExempted)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            SubjectsInSchool usr = context.SubjectsInSchools.FirstOrDefault(s => s.SchoolId == schoolId && s.Id != idExempted);
            return usr;
        }
        public SubjectsInSchool RetrieveSubjectByName(Int64 schoolId, string name, Int64 idExempted)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            SubjectsInSchool usr = context.SubjectsInSchools.FirstOrDefault(s => s.SchoolId == schoolId && s.Id != idExempted );
            return usr;
        }
        public void UpdateSubjectInSchool(SubjectsInSchool sb)
        {
            try
            {
                //PASSISLIBDataContext context = new PASSISLIBDataContext();
                //SubjectsInSchool subj = context.SubjectsInSchools.FirstOrDefault(s => s.Id == sb.Id);            
                //subj.SubjectName = sb.SubjectName;
                //subj.SubjectCode = sb.SubjectCode;
                //context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        /// <summary>
        /// Deleting record should be reconsidered during code optimization etc ...preferably its better to disable the item
        /// </summary>
        /// <param name="sb"></param>
        public void DeleteSubjectInSchool(SubjectsInSchool sb)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.SubjectsInSchools.DeleteOnSubmit(sb);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DeleteSubjectInSchool(long subjectId, long classId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                IList<SubjectsInSchool> sb = (from s in context.SubjectsInSchools where s.SubjectId == subjectId select s).ToList();
                foreach (SubjectsInSchool s in sb)
                {
                    context.SubjectsInSchools.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }

                IList<SubjectTeacher> sbt = (from s in context.SubjectTeachers where s.SubjectId == subjectId select s).ToList();
                foreach (SubjectTeacher s in sbt)
                {
                    context.SubjectTeachers.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }
                IList<LessonNote> lsn = (from s in context.LessonNotes where s.SubjectId == subjectId select s).ToList();
                foreach (LessonNote s in lsn)
                {
                    context.LessonNotes.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }
                IList<ReportCardData> rpcd = (from s in context.ReportCardDatas where s.SubjectId == subjectId select s).ToList();
                foreach (ReportCardData s in rpcd)
                {
                    context.ReportCardDatas.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }
                IList<ScoreSheetRepo> scsr = (from s in context.ScoreSheetRepos where s.SubjectId == subjectId select s).ToList();
                foreach (ScoreSheetRepo s in scsr)
                {
                    context.ScoreSheetRepos.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }
                IList<StudentScore> stdsc = (from s in context.StudentScores where s.SubjectId == subjectId select s).ToList();
                foreach (StudentScore s in stdsc)
                {
                    context.StudentScores.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }
                IList<StudentScoreRepository> stdRepo = (from s in context.StudentScoreRepositories where s.SubjectId == subjectId select s).ToList();
                foreach (StudentScoreRepository s in stdRepo)
                {
                    context.StudentScoreRepositories.DeleteOnSubmit(s);
                    context.SubmitChanges();
                }
                Subject sub = context.Subjects.FirstOrDefault(s => s.Id == subjectId && s.ClassId == classId);
                context.Subjects.DeleteOnSubmit(sub);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        /// <summary>
        /// Deleting record should be reconsidered during code optimization etc ...preferably its better to disable the item
        /// </summary>
        /// <param name="sb"></param>
        public void DeleteSubjectTeacher(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                SubjectTeacher st = context.SubjectTeachers.FirstOrDefault(s => s.Id == Id);
                context.SubjectTeachers.DeleteOnSubmit(st);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }                      
        }
        public void UpdateSubjectTeacher(SubjectTeacher st)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.SubjectTeachers.Attach(st);
                //context.SubjectTeachers.DeleteOnSubmit(st);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
