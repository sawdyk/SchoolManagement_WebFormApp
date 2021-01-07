using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class ClassGradeLIB
    {
        public void SaveGradeStudent(GradeStudent gradeStudent)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.GradeStudents.InsertOnSubmit(gradeStudent);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        #region gradestudents
        public IList<User> RetrieveUnallocatedStudents(Int64 schoolId)
        {
            try
            {
                PASSISLIBDataContext p = new PASSISLIBDataContext();
                var query = from u in p.UserRoles where !(from s in p.GradeStudents select s.StudentId).Contains(u.User.Id) && (long)u.RoleId == (long)PASSIS.LIB.Utility.roles.student && u.User.SchoolId == schoolId select u.User;

                return query.OrderBy(r => r.LastName).ToList<User>();
            }
            catch (Exception e) { throw e; }
        }

        public IList<User> RetrieveUnallocatedStudentsByCampusID(Int64 schoolId, Int64 campusId)
        {
            try
            {
                PASSISLIBDataContext p = new PASSISLIBDataContext();
                var query = from u in p.UserRoles where !(from s in p.GradeStudents select s.StudentId).Contains(u.User.Id) && (long)u.RoleId == (long)PASSIS.LIB.Utility.roles.student && u.User.SchoolId == schoolId  && u.User.SchoolCampusId == campusId select u.User;

                return query.OrderBy(r => r.LastName).ToList<User>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<GradeStudent> RetrieveGradeStudents(Int64 schoolId, Int64 campusId, Int64 yearId, Int64 gradeId, Int64 sessionId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents
                               where grd.SchoolId == schoolId && grd.SchoolCampusId == campusId && grd.ClassId==yearId &&
                               grd.GradeId == gradeId && grd.AcademicSessionId == sessionId && (grd.HasGraduated == false || grd.HasGraduated == null)
                               select grd;
                //if (schoolId > 0)
                //    allUsers = allUsers.Where(c => c.User.SchoolId == schoolId);
                //if (yearId > 0)
                //    allUsers = allUsers.Where(s => s.ClassId == yearId);
                //if (campusId > 0)
                //    allUsers = allUsers.Where(c => c.User.SchoolCampusId == campusId);
                //if (gradeId > 0)
                //    allUsers = allUsers.Where(s => s.GradeId == gradeId);

                return allUsers.ToList<GradeStudent>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<GradeStudent> RetrieveSingleGradeStudent(Int64 schoolId, Int64 campusId, Int64 yearId, Int64 gradeId, Int64 studentId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents
                               where grd.SchoolId == schoolId && grd.SchoolCampusId == campusId && grd.ClassId == yearId && grd.GradeId == gradeId && grd.StudentId == studentId
                               select grd;

                return allUsers.ToList<GradeStudent>();
            }
            catch (Exception e) { throw e; }
        }

        public IList<GradeStudent> RetrieveSingleGradeStudent(Int64 schoolId, Int64 campusId,Int64 studentId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents
                               where grd.SchoolId == schoolId && grd.SchoolCampusId == campusId && grd.StudentId == studentId
                               select grd;

                return allUsers.ToList<GradeStudent>();
            }
            catch (Exception e) { throw e; }
        }
        public void DeleteGradeStudents(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                GradeStudent gs = context.GradeStudents.FirstOrDefault(s => s.Id == Id);
                context.GradeStudents.DeleteOnSubmit(gs);
                context.SubmitChanges();

            }
            catch (Exception ex) { }

        }
        public void DeleteGrade(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Grade gs = context.Grades.FirstOrDefault(s => s.Id == Id);
                context.Grades.DeleteOnSubmit(gs);
                context.SubmitChanges();

            }
            catch (Exception ex) { }

        }

        public IList<GradeStudent> getAllGradeStudents(Int64 gradeTeacherId, Int64 schoolId, Int64 campusId, Int64 sessionId)
        {

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            IList<GradeStudent> results = new List<GradeStudent>();
            Grade gs = context.Grades.FirstOrDefault(s => s.GradeTeacherId == gradeTeacherId && s.SchoolId == schoolId && s.SchoolCampusId== campusId);
            if (gs == null)
            {

            }
            else
            {
                var res = from st in context.GradeStudents where (long)st.GradeId == gs.Id && st.GradeTeacherId == gradeTeacherId && st.AcademicSessionId == sessionId && (st.HasGraduated ==false || st.HasGraduated == null) select st;
                results = res.ToList<GradeStudent>();
            }
            return results;
        }

        public IList<User> getAllGradeStudentsUser(Int64 gradeTeacherId, Int64 schoolId, Int64 campusId)
        {

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            IList<GradeStudent> results = new List<GradeStudent>();
            IList<User> studentUser = new List<User>();
            Grade gs = context.Grades.FirstOrDefault(s => s.GradeTeacherId == gradeTeacherId && s.SchoolId == schoolId && s.SchoolCampusId == campusId);
            if (gs == null)
            {

            }
            else
            {
                var res = from st in context.GradeStudents where (long)st.GradeId == gs.Id && (st.HasGraduated == null || st.HasGraduated == false) select st;
                results = res.ToList<GradeStudent>();
                foreach (GradeStudent gdst in results) 
                {
                    var student = context.Users.FirstOrDefault(x => x.Id == gdst.StudentId);
                    studentUser.Add(student);
                }
                
            }
            return studentUser;
        }


        public IList<GradeStudent> getAllGradeStudentsNew(long gradeId)
        {

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            IList<GradeStudent> results = new List<GradeStudent>();
            Grade gs = context.Grades.FirstOrDefault(s => s.Id == gradeId );
            if (gs == null)
            {

            }
            else
            {
                var res = from st in context.GradeStudents where st.GradeId == gs.Id select st;
                results = res.ToList<GradeStudent>();
            }
            return results;
        }



       


        //public IList<GradeStudent> getAllGradeStudents(Int64 gradeTeacherId)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    var results = from res in context.GradeStudents where (long)res.GradeTeacherId == gradeTeacherId select res;
        //    return results.ToList<GradeStudent>();
        //}

        //public void SaveGradeStudent(GradeStudent gradeStudent)
        //{
        //    try
        //    {
        //        PASSISLIBDataContext context = new PASSISLIBDataContext();
        //        context.GradeStudents.InsertOnSubmit(gradeStudent);
        //        context.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //}
        /// <summary>
        /// obsolete in current design dont use this method  again, confirm this from d database.
        /// use RetrieveGradeClassOfTeacher method in this class.
        /// </summary>
        /// <param name="gradeTeacherId"></param>
        /// <returns></returns>
        public GradeStudent RetrieveTeacherGrade(Int64 gradeTeacherId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            GradeStudent grd = null;
            grd = context.GradeStudents.FirstOrDefault(g => g.GradeTeacherId == gradeTeacherId);
            return grd;
        }
        public GradeStudent RetrieveTeacherGrade(Int64 yearId, Int64 gradeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            GradeStudent grd = null;
            grd = context.GradeStudents.FirstOrDefault(g => g.ClassId == yearId && g.GradeId == gradeId);
            return grd;
        }
        public GradeStudent RetrieveStudentGrade(Int64 studentId, Int64 sessionId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext(); 
            GradeStudent grd = null;
            grd = context.GradeStudents.FirstOrDefault(g => g.StudentId == studentId && g.AcademicSessionId == sessionId);
            return grd;
        }

      
        public User RetrieveOneParticularStudent(Int64 studentId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            User CurrentUSer = context.Users.FirstOrDefault(usr => usr.Id == studentId);
            return CurrentUSer;
        }
        public Int64 RetrieveStudentGradeId(PASSISLIBDataContext context, Int64 studentId)
        {
            GradeStudent grd = null;
            grd = context.GradeStudents.FirstOrDefault(g => g.StudentId == studentId);
            return grd != null ? grd.GradeId : 0L;
        }
        /// <summary>
        /// Formroom is synonymous to grade and class
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IsStudentInFormRoom(PASSISLIBDataContext context, Int64 studentId, Int64 formId)
        {
            GradeStudent grd = null;
            try
            {
                grd = context.GradeStudents.FirstOrDefault(g => g.StudentId == studentId && g.GradeId == formId);
            }
            catch { }
            return grd == null ? false : true;
        }

        #endregion
        public void SaveGrade(Grade grade)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Grades.InsertOnSubmit(grade);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<Grade> getAllGrades(Int64 schoolId)
        { return getAllGrades(schoolId, 0); }
        public IList<Grade> getAllGrades(Int64 schoolId, Int64 campusId)
        {
            return getAllGrades(schoolId, campusId, string.Empty);
        }
        public IList<Grade> getAllGrades(Int64 schoolId, Int64 campusId, string gradeCode)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allGrds = (System.Linq.IQueryable<Grade>)null;
            if (campusId > 0)
            {
                allGrds = from grd in context.Grades where (long)grd.SchoolId == schoolId && grd.SchoolCampusId == campusId select grd;
                if (!string.IsNullOrEmpty(gradeCode))
                    allGrds = from grd in context.Grades where (long)grd.SchoolId == schoolId && grd.SchoolCampusId == campusId && grd.GradeCode.Trim().ToLower().Equals(gradeCode.Trim().ToLower()) select grd;
            }
            else
            {
                allGrds = from grd in context.Grades where (long)grd.SchoolId == schoolId select grd;
                if (!string.IsNullOrEmpty(gradeCode))
                    allGrds = from grd in context.Grades where (long)grd.SchoolId == schoolId && grd.GradeCode.Trim().ToLower().Equals(gradeCode.Trim().ToLower()) select grd;
            }
            return allGrds.ToList<Grade>();
        }
        public Grade RetrieveGradeBy(Int64 schoolId)
        {
            return RetrieveGradeBy(schoolId, 0, string.Empty, string.Empty);
        }
        public Grade RetrieveGradeBy(Int64 schoolId, Int64 campusId)
        {
            return RetrieveGradeBy(schoolId, campusId, string.Empty, string.Empty);
        }
        public Grade RetrieveGradeBy(Int64 schoolId, Int64 campusId, string gradeCode)
        {
            return RetrieveGradeBy(schoolId, campusId, gradeCode, string.Empty);
        }
        public Grade RetrieveGradeByYearAndHomeRoom(Int64 schoolId, Int64 campusId, string homeRoom, string year)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Grade grd = null;
            grd = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId && s.SchoolCampusId == campusId && s.GradeCode.Trim().ToLower().Equals(homeRoom.Trim().ToLower()) && s.Class_Grade.Code.ToString().ToLower().Equals(year.Trim().ToLower()));

            return grd;
        }
        /// <summary>
        /// complete the fourth case for the 3 conditions in each of the if else segment
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="campusId"></param>
        /// <param name="gradeCode"></param>
        /// <param name="gradeClass"></param>
        /// <returns></returns>
        public Grade RetrieveGradeBy(Int64 schoolId, Int64 campusId, string gradeCode, string gradeClass)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Grade usr = null;
            if (campusId > 0)
            {
                if (!string.IsNullOrEmpty(gradeCode) && string.IsNullOrEmpty(gradeClass))
                    usr = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId && s.SchoolCampusId == campusId && s.GradeCode.Trim().ToLower().Equals(gradeCode.Trim().ToLower()));
                if (string.IsNullOrEmpty(gradeCode) && string.IsNullOrEmpty(gradeClass))
                    usr = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId && s.SchoolCampusId == campusId);

                if (!string.IsNullOrEmpty(gradeCode) && !string.IsNullOrEmpty(gradeClass))
                    usr = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId && s.SchoolCampusId == campusId && s.GradeCode.Trim().ToLower().Equals(gradeCode.Trim().ToLower()) && s.Class_Grade.Code.ToString().ToLower().Equals(gradeClass.Trim().ToLower()));

            }
            else
            {
                if (!string.IsNullOrEmpty(gradeCode) && string.IsNullOrEmpty(gradeClass))
                    usr = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId && s.GradeCode.Trim().ToLower().Equals(gradeCode.Trim().ToLower()));
                if (string.IsNullOrEmpty(gradeCode) && string.IsNullOrEmpty(gradeClass))
                    usr = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId);
                if (!string.IsNullOrEmpty(gradeCode) && !string.IsNullOrEmpty(gradeClass))
                    usr = context.Grades.FirstOrDefault(s => s.SchoolId == schoolId && s.GradeCode.Trim().ToLower().Equals(gradeCode.Trim().ToLower()) && s.ClassId.ToString().ToLower().Equals(gradeClass.Trim().ToLower()));
            }

            return usr;
        }
        public Class_Grade RetrieveClassGrade(Int64 ClassId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade usr = context.Class_Grades.FirstOrDefault(s => s.Id == ClassId);
            return usr;
        }
        public Grade RetrieveGrade(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Grade usr = context.Grades.FirstOrDefault(s => s.Id == Id);
            return usr;
        }
        public Grade RetrieveGradeClassOfTeacher(Int64 gradeTeacherId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Grade grd = null;
            grd = context.Grades.FirstOrDefault(g => g.GradeTeacherId == gradeTeacherId);
            return grd;
        }
        public void SaveGradeClassTeacher(GradeClassTeacher gradeClassTeacher)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.GradeClassTeachers.InsertOnSubmit(gradeClassTeacher);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<GradeClassTeacher> getAllClassGradeTeachers(Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allGrdTchrs = from cls in context.GradeClassTeachers where (long)cls.User.SchoolId == schoolId select cls;
            return allGrdTchrs.ToList<GradeClassTeacher>();
        }
        public GradeClassTeacher RetrieveGradeClassTeacher(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            GradeClassTeacher usr = context.GradeClassTeachers.FirstOrDefault(s => s.Id == Id);
            return usr;
        }
        public void UpdateClassGrade(Class_Grade classGrade)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                //context.Class_Grades.InsertOnSubmit(classGrade);
                Class_Grade cls = context.Class_Grades.FirstOrDefault(s => s.Id == classGrade.Id);
                //cls = classGrade;
                cls.Name = classGrade.Name;
                cls.Code = classGrade.Code;
                cls.ClassSubjectIds = classGrade.ClassSubjectIds;

                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveClassGrade(Class_Grade classGrade)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Class_Grades.InsertOnSubmit(classGrade);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<Class_Grade> getAllClassGrade(Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allClassGrd = from cls in context.Class_Grades where cls.School == schoolId select cls;
            return allClassGrd.ToList<Class_Grade>();
        }
        public IList<Class_Grade> getClassGrade(long curriculum)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allClassGrd = from cls in context.Class_Grades where cls.CurriculumId == curriculum select cls;
            return allClassGrd.ToList<Class_Grade>();
        }
        public IList<Class_Grade> getNewClassGrade(long curriculum, long schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            School schl = context.Schools.FirstOrDefault(x => x.Id == schoolId);
            var allClassGrd = from cls in context.Class_Grades where cls.CurriculumId == curriculum && schl.Id == schoolId select cls;
            return allClassGrd.ToList<Class_Grade>();
        }

        public IList<Class_Grade> getClassGradeNusery(long curriculum, long schoolTypeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allClassGrd = from cls in context.Class_Grades where cls.CurriculumId == curriculum && cls.SchoolTypeId == schoolTypeId select cls;
            return allClassGrd.ToList<Class_Grade>();
        }
        public IList<Class_Grade> getClassGrade(long curriculum, long schoolTypeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allClassGrd = from cls in context.Class_Grades where cls.CurriculumId == curriculum && cls.SchoolTypeId== schoolTypeId select cls;
            return allClassGrd.ToList<Class_Grade>();
        }
        public IList<Int64> getAllAllocatedClassesId(Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allAllocatedClassIds = from cls in context.GradeClassTeachers where (long)cls.User.SchoolId == schoolId select (long)cls.ClassGrade;
            return allAllocatedClassIds.ToList<Int64>();
        }
        public Class_Grade RetrieveClass_Grade(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade usr = context.Class_Grades.FirstOrDefault(s => s.Id == Id);
            return usr;
        }
        public Class_Grade RetrieveClass_GradeByName(Int64 schoolId, string name)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            Class_Grade usr = context.Class_Grades.FirstOrDefault(s => s.School == schoolId && s.Name.Trim().ToLower().Equals(name.Trim().ToLower()));
            return usr;
        }
        public Class_Grade RetrieveClass_GradeByCode(Int64 schoolId, string code)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade usr = context.Class_Grades.FirstOrDefault(s => s.School == schoolId && s.Code.Trim().ToLower().Equals(code.Trim().ToLower()));
            return usr;
        }
        public IList<Grade> getAllGradesByYear(Int64 schoolId, Int64 campusId, Int64 yearId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allGrds = from g in context.Grades where g.SchoolId == schoolId && g.SchoolCampusId == campusId && g.ClassId == yearId select g;
            return allGrds.ToList<Grade>();
        }

      
        public Class_Grade RetrieveClass_GradeByName(Int64 schoolId, string name, Int64 idExempted)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade usr = context.Class_Grades.FirstOrDefault(s => s.School == schoolId && s.Id != idExempted && s.Name.Trim().ToLower().Equals(name.Trim().ToLower()));
            return usr;
        }
        public Class_Grade RetrieveClass_GradeByCode(Int64 schoolId, string code, Int64 idExempted)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade usr = context.Class_Grades.FirstOrDefault(s => s.School == schoolId && s.Id != idExempted && s.Code.Trim().ToLower().Equals(code.Trim().ToLower()));
            return usr;
        }
        //public IList<PassisWebService.Util.YearSubjectMapping> getYearSubjectMapping(Int64 schoolId)
        //{
        //    try
        //    {
        //        Int64 id = 1;
        //        var yearList = getAllClassGrade(schoolId);
        //        IList<YearSubjectMapping> ysmList = new List<YearSubjectMapping>();
        //        foreach (Class_Grade cg in yearList)
        //        {
        //            if (!string.IsNullOrEmpty(cg.ClassSubjectIds))
        //            {
        //                var subjectIds = Util.GetIdListFromString(cg.ClassSubjectIds);
        //                foreach (Int64 subjectId in subjectIds)
        //                {
        //                    YearSubjectMapping ysm = new YearSubjectMapping();
        //                    ysm.Id = id++;
        //                    ysm.SchoolId = schoolId;
        //                    ysm.SubjectId = subjectId;
        //                    ysm.YearId = cg.Id;
        //                    ysm.YearName = cg.Name;

        //                    ysmList.Add(ysm);

        //                }
        //            }
        //        }
        //        return ysmList;
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

    }
}
