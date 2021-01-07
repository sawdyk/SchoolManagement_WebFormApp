using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PASSIS.LIB.Utility;
//using PASSIS.DAO.Utility;

namespace PASSIS.LIB
{

    
    public class SchoolLIB : BasePage
    {

        
        public void SaveScoreConfiguration(ScoreConfiguration scoreConfig)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.ScoreConfigurations.InsertOnSubmit(scoreConfig);
            context.SubmitChanges();
        }
        public void SaveScoreGradeConfiguration(ScoreGradeConfiguration scoreConfig)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.ScoreGradeConfigurations.InsertOnSubmit(scoreConfig);
            context.SubmitChanges();
        }
        public void SaveScoreCategoryConfiguration(ScoreCategoryConfiguration scoreConfig)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.ScoreCategoryConfigurations.InsertOnSubmit(scoreConfig);
            context.SubmitChanges();
        }
        public void SaveScoreSubCategoryConfiguration(ScoreSubCategoryConfiguration scoreConfig)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.ScoreSubCategoryConfigurations.InsertOnSubmit(scoreConfig);
            context.SubmitChanges();
        }
        public IList<ScoreConfiguration> GetScoreConfiguration(Int64 schoolId, Int64 campusId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getConfig = from r in context.ScoreConfigurations
                            where r.SchoolId == schoolId && r.CampusId == campusId
                            select r;
            return getConfig.ToList<ScoreConfiguration>();
        }

        public IList<ScoreGradeConfiguration> GetScoreGradeConfiguration(Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getConfig = from r in context.ScoreGradeConfigurations
                            where r.SchoolId == schoolId
                            select r;
            return getConfig.ToList<ScoreGradeConfiguration>();
        }

        public IList<ScoreCategoryConfiguration> GetScoreCategoryConfiguration(long schoolId, long campusId) 
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getCategory = from s in context.ScoreCategoryConfigurations
                              where s.SchoolId == schoolId &&
                              s.CampusId == campusId
                              select s;
            return getCategory.ToList<ScoreCategoryConfiguration>();
        }
        public IList<ScoreCategoryConfiguration> GetScoreCategoryConfiguration(long schoolId, long campusId, long classId, long sessionId, long termId, string categoryName)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getConfig = from r in context.ScoreCategoryConfigurations
                            where r.SchoolId == schoolId && r.CampusId == campusId && r.ClassId == classId && r.Category == categoryName && r.SessionId == sessionId && r.TermId == termId
                            select r;
            return getConfig.ToList<ScoreCategoryConfiguration>();
        }
        public IList<ScoreSubCategoryConfiguration> GetScoreSubCategoryConfiguration(long schoolId, long campusId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getCategory = from s in context.ScoreSubCategoryConfigurations
                              where s.ScoreCategoryConfiguration.SchoolId == schoolId &&
                              s.ScoreCategoryConfiguration.CampusId == campusId
                              select s;
            return getCategory.ToList<ScoreSubCategoryConfiguration>();
        }
        public IList<ScoreSubCategoryConfiguration> GetScoreSubCategoryConfiguration(long schoolId, long campusId, long classId, long sessionId, long termId, long categoryId, string subCategoryName)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getConfig = from r in context.ScoreSubCategoryConfigurations
                            where r.ScoreCategoryConfiguration.SchoolId == schoolId && r.ScoreCategoryConfiguration.CampusId == campusId && r.ScoreCategoryConfiguration.ClassId == classId && r.SessionId == sessionId && r.TermId == termId && r.CategoryId == categoryId && r.SubCategory == subCategoryName
                            select r;
            return getConfig.ToList<ScoreSubCategoryConfiguration>();
        }

        public void SaveNewSchool(School newSchool)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.Schools.InsertOnSubmit(newSchool);
            context.SubmitChanges();
        }
        public bool schoolCodeExist(string schoolCode)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            School code = dataContext.Schools.FirstOrDefault(s => s.Code.Trim().ToLower().Equals(schoolCode.Trim().ToLower()));
            if (code == null)
                result = false;
            return result;
        }
        public bool schoolNameExist(string schoolName)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            School name = dataContext.Schools.FirstOrDefault(s => s.Name.Trim().ToLower().Equals(schoolName.Trim().ToLower()));
            if (name == null)
                result = false;
            return result;
        }
        public long GetSchoolId(string schoolCode)
        {
            long ReturnSchoolId;

            using (PASSISLIBDataContext context = new PASSISLIBDataContext())
            {

                var getSchoolId = from school in context.Schools
                                  where school.Code == schoolCode
                                  select school.Id;

                if (getSchoolId.Count() > 0)
                {
                    ReturnSchoolId = getSchoolId.FirstOrDefault();
                }
                else
                {
                    ReturnSchoolId = 0;
                }
            }
            return ReturnSchoolId;
        }
        public IList<SchoolType> SchoolType()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolTypeObj = from campus in context.SchoolTypes
                            select campus;
            return schoolTypeObj.ToList<SchoolType>();
        }
        public IList<SchoolType> SchoolTypeNusery()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolTypeNuseryObj = from campus in context.SchoolTypes
                                where campus.Id == 3
                                select campus;
            return schoolTypeNuseryObj.ToList<SchoolType>();
        }

        public IList<Curriculum> SchoolCurriculum()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumObj = from curriculum in context.Curriculums
                                      select curriculum;
            return schoolCurriculumObj.ToList<Curriculum>();
        }

        public IList<Curriculum> SchoolCurriculumNusery()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumNuseryObj = from curriculum in context.Curriculums
                                            where curriculum.Id == 2
                                      select curriculum;
            return schoolCurriculumNuseryObj.ToList<Curriculum>();
        }
        public IList<Subject> MasterSubjects(long CurriculumId, long SchoolTypeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumObj = from allSubjects in context.Subjects
                                      where allSubjects.CurriculumId == CurriculumId && allSubjects.SchoolTypeId==SchoolTypeId
                                      select allSubjects;
            return schoolCurriculumObj.ToList<Subject>();
        }
        public IList<Subject> GetClassSubjectsNigeria(long CurriculumId, long SchoolTypeId, long ClassId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumObj = from allSubjects in context.Subjects
                                      where allSubjects.CurriculumId == CurriculumId && allSubjects.SchoolTypeId == SchoolTypeId && allSubjects.ClassId == ClassId
                                      select allSubjects;
            return schoolCurriculumObj.ToList<Subject>();
        }
        public IList<Subject> GetClassSubjectsBritish(long CurriculumId, long ClassId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumObj = from allSubjects in context.Subjects
                                      where allSubjects.CurriculumId == CurriculumId && allSubjects.ClassId == ClassId
                                      select allSubjects;
            return schoolCurriculumObj.ToList<Subject>();
        }
        public IList<Subject> GetMasterSubjects()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumObj = from allSubjects in context.Subjects
                                      select allSubjects;
            return schoolCurriculumObj.ToList<Subject>();
        }
        public long GetSchoolTypeId(long schoolId)
        {
            long ReturnSchoolTypeId;

            using (PASSISLIBDataContext context = new PASSISLIBDataContext())
            {

                var getSchoolTypeId = from school in context.Schools
                                      where school.Id == schoolId
                                      select school.SchoolTypeId;

                if (getSchoolTypeId.Count() > 0)
                {
                    ReturnSchoolTypeId = getSchoolTypeId.FirstOrDefault();
                }
                else
                {
                    ReturnSchoolTypeId = 0;
                }
            }
            return ReturnSchoolTypeId;
        }
        public void SaveSchoolCurriculum(SubjectsInSchool subjectsinSchool)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.SubjectsInSchools.InsertOnSubmit(subjectsinSchool);
            context.SubmitChanges();
        }
        public IList<Subject> OptionalSubjects(long CurriculumId, long SchoolTypeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var schoolCurriculumObj = from allSubjects in context.Subjects
                                      where allSubjects.CurriculumId == CurriculumId && allSubjects.SchoolTypeId==SchoolTypeId                    
                                      select allSubjects;
            return schoolCurriculumObj.ToList<Subject>();
        }
        public IList<PositionInSchool> PositionInSchool()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var positionInSchoolObj = from positionInSchool in context.PositionInSchools
                                   select positionInSchool;
            return positionInSchoolObj.ToList<PositionInSchool>();
        }
        public void SaveNewSetupRequestDetails(SelfSetupDetail selfSetup)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.SelfSetupDetails.InsertOnSubmit(selfSetup);
            context.SubmitChanges();
        }
        public bool selfSetupEmailExist(string email)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            SelfSetupDetail code = dataContext.SelfSetupDetails.FirstOrDefault(e => e.Email.Trim().ToLower().Equals(email.Trim().ToLower()));
            if (code == null)
                result = false;
            return result;
        }
        public IList<School> getAllSchools()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = context.Schools;
            return allSchool.ToList<School>();
        }
        public IList<Class_Grade> getAllClass_Grade(long CurriculmId, long schoolTypeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = from schoolClass in context.Class_Grades
                            where schoolClass.CurriculumId == CurriculmId && schoolClass.SchoolTypeId == schoolTypeId
                            select schoolClass;
            return allSchool.ToList<Class_Grade>();
        }

        public IList<Class_Grade> getTeachersClass_Grade(long CurriculmId, long schoolTypeId , long ClassId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.GradeStudent currentUser = context.GradeStudents.FirstOrDefault(x => x.GradeTeacherId == logonUser.Id);
            var allSchool = from schoolClass in context.Class_Grades
                            where schoolClass.CurriculumId == CurriculmId && schoolClass.SchoolTypeId == schoolTypeId && schoolClass.Id == currentUser.ClassId
                            select schoolClass;
            return allSchool.ToList<Class_Grade>();
        }
        public int? GetSchoolCurriculumId(long? schoolId)
        {
            int? ReturnSchoolCurriculumId;

            using (PASSISLIBDataContext context = new PASSISLIBDataContext())
            {

                var getSchoolCurriculumId = from school in context.Schools
                                            where school.Id == schoolId
                                            select school.CurriculumId;

                if (getSchoolCurriculumId.Count() > 0)
                {
                    ReturnSchoolCurriculumId = getSchoolCurriculumId.FirstOrDefault();
                }
                else
                {
                    ReturnSchoolCurriculumId = 0;
                }
            }
            return ReturnSchoolCurriculumId;
        }

        public int? GetSchoolCurriculumTeacherId(long? schoolId, long? GradeTeacherId)
        {

           
            int? ReturnSchoolCurriculumTeacherId;

            using (PASSISLIBDataContext context = new PASSISLIBDataContext())
            {
                PASSIS.LIB.User currentUser = context.Users.FirstOrDefault(x => x.Id == logonUser.Id );
                var getSchoolCurriculumTeacherId = from school in context.Schools
                                            where school.Id == schoolId && currentUser.Id == GradeTeacherId
                                            select school.CurriculumId;

                if (getSchoolCurriculumTeacherId.Count() > 0)
                {
                    ReturnSchoolCurriculumTeacherId = getSchoolCurriculumTeacherId.FirstOrDefault();
                }
                else
                {
                    ReturnSchoolCurriculumTeacherId = 0;
                }
            }
            return ReturnSchoolCurriculumTeacherId;
        }
        public int? GetSchoolTypeId(long? schoolId)
        {
            int? ReturnSchoolTypeId;

            using (PASSISLIBDataContext context = new PASSISLIBDataContext())
            {

                var getSchoolTypeId = from school in context.Schools
                                            where school.Id == schoolId
                                            select school.SchoolTypeId;

                if (getSchoolTypeId.Count() > 0)
                {
                    ReturnSchoolTypeId = getSchoolTypeId.FirstOrDefault();
                }
                else
                {
                    ReturnSchoolTypeId = 0;
                }
            }
            return ReturnSchoolTypeId;
        }
        public bool subjectInSchoolExist(long subjectId, long schoolId)
        {
            bool result = true;
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            SubjectsInSchool schoolExist = context.SubjectsInSchools.FirstOrDefault(s => s.SubjectId.Equals(subjectId) && s.SchoolId.Equals(schoolId));
            if (schoolExist == null)
                result = false;
            return result;
        }
        //public void SaveSubjectsInSchool(SubjectInSchool subInSchl)
        //{
        //    try
        //    {
        //        PASSISLIBDataContext context = new PASSISLIBDataContext();
        //        context.SubjectInSchools.InsertOnSubmit(subInSchl);
        //        context.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //}
        /// <summary>
        /// deprecated ::: be careful when using it
        /// </summary>
        /// <param name="schlId"></param>
        /// <param name="subjId"></param>
        /// <returns></returns>
        public IList<SubjectsInSchool> RetrieveSubjectsInSchool(Int64 schlId, Int64 subjId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectsInSchl = from sub in context.SubjectsInSchools where sub.SchoolId == schlId && sub.Id == subjId select sub;
            return allSubjectsInSchl.ToList<SubjectsInSchool>();
        }
        public IList<SubjectsInSchool> RetrieveSubjectsInSchool(Int64 schlId, string code, string name)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectsInSchl = from sub in context.SubjectsInSchools where sub.SchoolId == schlId select sub;
            return allSubjectsInSchl.ToList<SubjectsInSchool>();

        }
        public IList<SubjectsInSchool> RetrieveSubjectsInSchool(IList<Int64> ids)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectsInSchl = from sub in context.SubjectsInSchools where ids.ToArray().Contains(sub.Id) select sub;
            return allSubjectsInSchl.ToList<SubjectsInSchool>();

        }
        public Subject RetrieveSubjectInSchoolById(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Subject subj = context.Subjects.SingleOrDefault(s => s.Id == Convert.ToInt32(Id));
            return subj;

        }
        public IList<SubjectsInSchool> RetrieveYearSubjects(Int64 yearId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade yearObj = context.Class_Grades.SingleOrDefault(s => s.Id == yearId);
            string subjectIds = yearObj.ClassSubjectIds;
            IList<SubjectsInSchool> subjectInSchoolList = new List<SubjectsInSchool>();
            if (!string.IsNullOrEmpty(subjectIds))
            {
                //search for subect
                string[] ids = subjectIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                IList<Int64> sbIds = new List<Int64>();
                foreach (string str in ids)
                {
                    sbIds.Add(Convert.ToInt64(str));
                }
                subjectInSchoolList = RetrieveSubjectsInSchool(sbIds);
            }


            return subjectInSchoolList;

        }
        public bool IsSubjectInYear(Int64 yearId, Int64 subjectId)
        {
            bool result = false;
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade yearObj = context.Class_Grades.SingleOrDefault(s => s.Id == yearId);
            string subjectIds = yearObj.ClassSubjectIds;
            if (!string.IsNullOrEmpty(subjectIds))
            {
                //search for subect
                string[] ids = subjectIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Contains(subjectId.ToString()))
                    result = true;
            }
            else { result = false; }

            return result;

        }
        public IList<SubjectsInSchool> RetrieveSubjectsInSchool(Int64 schlId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectsInSchl = from sub in context.SubjectsInSchools where sub.SchoolId == schlId select sub;
            return allSubjectsInSchl.ToList<SubjectsInSchool>();

        }
        public void SaveSubjects(Subject sub)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Subjects.InsertOnSubmit(sub);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public Subject RetrieveSubject(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Subject subj = context.Subjects.SingleOrDefault(s => s.Id == Id);
            return subj;

        }
        public IList<Subject> getAllSubjects()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = context.Subjects;
            return allSchool.ToList<Subject>();
        }
        public void SaveSchools(School region)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Schools.InsertOnSubmit(region);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<School> RetrieveSchoolsByName(string name)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = from reg in context.Schools where reg.Name.Contains(name) select reg;
            return allSchool.ToList<School>();
        }
        public IList<School> RetrieveSchoolsByCode(string code)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = from reg in context.Schools where reg.Code.Contains(code) select reg;
            return allSchool.ToList<School>();
        }
        public School RetrieveSchool(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            School reg = context.Schools.SingleOrDefault(s => s.Id == Id);
            return reg;
        }
        
        public IList<Class_Grade> getAllClass_Grade()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = context.Class_Grades;
            return allSchool.ToList<Class_Grade>();
        }
        public void SaveClass_Grade(Class_Grade region)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Class_Grades.InsertOnSubmit(region);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public User GetUserDetails(string AdmissionNo)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            User getDetails = context.Users.FirstOrDefault(x => x.AdmissionNumber == AdmissionNo);
            return getDetails;
        }

        
    }
}
