using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
     public class SchoolConfigLIB
    {
        public void SaveSubjectsInSchool(SubjectsInSchool subInSchl)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.SubjectsInSchools.InsertOnSubmit(subInSchl);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
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
        //public IList<SubjectsInSchool> RetrieveSubjectsInSchool(Int64 schlId, string code, string name)
        //{
        //    PASSISLIBDataContext context = new PASSISLIBDataContext();
        //    var allSubjectsInSchl = from sub in context.SubjectsInSchools where sub.SchoolId == schlId && (sub.SubjectCode.ToUpper().Equals(code.ToUpper()) || sub.SubjectName.ToUpper().Equals(name.ToUpper())) select sub;
        //    return allSubjectsInSchl.ToList<SubjectsInSchool>();

        //}
        public IList<SubjectsInSchool> RetrieveSubjectsInSchool(IList<Int64> ids)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSubjectsInSchl = from sub in context.SubjectsInSchools where ids.ToArray().Contains(sub.Id) select sub;
            return allSubjectsInSchl.ToList<SubjectsInSchool>();

        }
        public SubjectsInSchool RetrieveSubjectsInSchoolById(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            SubjectsInSchool subj = context.SubjectsInSchools.SingleOrDefault(s => s.Id == Id);
            return subj;

        }
        public IList<SubjectsInSchool> RetrieveYearSubjects(Int64 yearId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Class_Grade yearObj = context.Class_Grades.SingleOrDefault(s => s.Id == yearId);
            string subjectIds = yearObj.ClassSubjectIds;
            IList<SubjectsInSchool> SubjectsInSchoolList = new List<SubjectsInSchool>();
            if (!string.IsNullOrEmpty(subjectIds))
            {
                //search for subect
                string[] ids = subjectIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                IList<Int64> sbIds = new List<Int64>();
                foreach (string str in ids)
                {
                    sbIds.Add(Convert.ToInt64(str));
                }
                SubjectsInSchoolList = RetrieveSubjectsInSchool(sbIds);
            }


            return SubjectsInSchoolList;

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
        //public Subject RetrieveSubject(Int64 Id)
        //{
        //    PASSISLIBDataContext context = new PASSISLIBDataContext();
        //    Subject subj = context.Subjects.SingleOrDefault(s => s.Id == Id.ToString());
        //    return subj;

        //}
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
        public IList<School> getAllSchools()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allSchool = context.Schools;
            return allSchool.ToList<School>();
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
    }
}
