using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class GroupingLIB
    {

        #region Grouping
        public Boolean Exist(string newGroupName)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            Grouping grpn = dataContext.Groupings.FirstOrDefault(g => g.GroupName.Trim().ToLower().Equals(newGroupName.Trim().ToLower()));
            if (grpn == null)
                result = false;
            return result;
        }
        public Boolean GroupCodeExist(string newGroupCode)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            Grouping grpn = dataContext.Groupings.FirstOrDefault(g => g.GroupCode.Trim().ToLower().Equals(newGroupCode.Trim().ToLower()));
            if (grpn == null)
                result = false;
            return result;
        }
        public void SaveGrouping(Grouping gr)
        {
            try
            {
                 PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Groupings.InsertOnSubmit(gr);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<Grouping> RetrieveTeacherGrouping(Int64 teacherUserId)
        {
            return RetrieveTeacherGrouping(teacherUserId, 0);
        }
        public IList<Grouping> RetrieveTeacherGrouping(Int64 teacherUserId, Int64 yearId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudents = from gs in context.Groupings select gs;
                if (teacherUserId > 0)
                {
                    allStudents = allStudents.Where(c => c.TeacherId == teacherUserId);
                }
                if (yearId > 0)
                {
                    allStudents = allStudents.Where(c => c.yearId == yearId);
                }
                return allStudents.ToList<Grouping>();
            }
            catch (Exception e) { throw e; }
        }
        public Grouping RetrieveGrouping(Int64 groupId)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            Grouping grpn = dataContext.Groupings.FirstOrDefault(g => g.Id == groupId);
            return grpn;
        }
        public Class_Grade RetrieveGroupingYear(Int64 groupId)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            Grouping grpn = dataContext.Groupings.FirstOrDefault(g => g.Id == groupId);
            Class_Grade yr = dataContext.Class_Grades.FirstOrDefault(y => y.Id  == grpn.yearId);
            return yr;
        }
        public IList<Grouping> RetrieveGroupingInAYear(Int64 yearId, Int64 campusId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudents = from gs in context.Groupings where gs.yearId == yearId && gs.CampusId == campusId select gs;
                return allStudents.ToList<Grouping>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<Grouping> RetrieveGroupings(Int64 campusId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudents = from gs in context.Groupings select gs;
                if (campusId > 0)
                {
                    allStudents = allStudents.Where(c => c.CampusId == campusId);
                }

                return allStudents.ToList<Grouping>();
            }
            catch (Exception e) { throw e; }
        }
        #endregion

        #region GroupStudent
        public IList<User> RetrieveStudentsNotInYearGroup(Int64 groupId, Int64 yearId, Int64 campusId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            //var allSelectedGroupStudents = from g in context.GroupStudents where g.GroupingId == groupId select g;
            //var allSelectedYearStudents = from s in context.GradeStudents where s.SchoolCampusId == campusId && s.ClassId == yearId select s;

            var yearStudentNotInSelectedGroup = from g in context.GradeStudents where g.User.SchoolCampusId == campusId && g.ClassId == yearId && !(from grpStd in context.GroupStudents where grpStd.GroupingId == groupId select grpStd.User.Id).Contains(g.User.Id) select g.User; ;
            return yearStudentNotInSelectedGroup.ToList<User>();
        }
        public Int64[] RetrieveStudentGroupsById(Int64 studentId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudentGroupIds = from gs in context.GroupStudents where gs.StudentId == studentId select gs.GroupingId;

                return allStudentGroupIds.ToArray<Int64>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<GroupStudent> RetrieveGroupStudents(Int64 campusId, Int64 yearId, Int64 classId, Int64 subjectId, Int64 teacherId, Int64 groupId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudents = from gs in context.GroupStudents select gs;
                if (campusId > 0)
                {
                    allStudents = allStudents.Where(c => c.User.SchoolCampusId == campusId);
                }

                if (yearId > 0)
                    allStudents = allStudents.Where(s => s.Grade.ClassId == yearId);
                if (classId > 0)
                    allStudents = allStudents.Where(s => s.Grade.Id == classId);
                if (subjectId > 0)
                    allStudents = allStudents.Where(s => s.Grouping.SubjectId == subjectId);
                if (teacherId > 0)
                    allStudents = allStudents.Where(s => s.Grouping.TeacherId == teacherId);
                if (groupId > 0)
                    allStudents = allStudents.Where(s => s.GroupingId == groupId);


                //var result = from res in allStudents select res.User;

                return allStudents.ToList<GroupStudent>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<GroupStudent> RetrieveGroupStudents(Int64 campusId, Int64 yearId, Int64 classId, Int64 subjectId, Int64 teacherId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudents = from gs in context.GroupStudents select gs;
                if (campusId > 0)
                {
                    allStudents = allStudents.Where(c => c.User.SchoolCampusId == campusId);
                }

                if (yearId > 0)
                    allStudents = allStudents.Where(s => s.Grade.ClassId == yearId);
                if (classId > 0)
                    allStudents = allStudents.Where(s => s.Grade.Id == classId);
                if (subjectId > 0)
                    allStudents = allStudents.Where(s => s.Grouping.SubjectId == subjectId);
                if (teacherId > 0)
                    allStudents = allStudents.Where(s => s.Grouping.TeacherId == teacherId);


                //var result = from res in allStudents select res.User;

                return allStudents.ToList<GroupStudent>();
            }
            catch (Exception e) { throw e; }
        }
        public void SaveGroupStudent(GroupStudent gs)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.GroupStudents.InsertOnSubmit(gs);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }

        public void SaveGroupStudents(IList<GroupStudent> gsList)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                foreach (GroupStudent gs in gsList)
                {
                    context.GroupStudents.InsertOnSubmit(gs);
                }

                context.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;


            }
        }

        public void DeleteGroupStudent(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                GroupStudent gs = context.GroupStudents.FirstOrDefault(s => s.Id == Id);
                context.GroupStudents.DeleteOnSubmit(gs);
                context.SubmitChanges();

            }
            catch (Exception ex) { }

        }
        public void DeleteGroup(Int64 groupId)
        {
            //delete group student 
            PASSISLIBDataContext ct = new PASSISLIBDataContext();
            var groupstudents = ct.GroupStudents.Where(g => g.GroupingId == groupId);
            foreach (GroupStudent gs in groupstudents)
            {
                ct.GroupStudents.DeleteOnSubmit(gs);
            }
            ct.SubmitChanges();
            //delet group 
            Grouping grp = ct.Groupings.FirstOrDefault(g => g.Id == groupId);
            ct.Groupings.DeleteOnSubmit(grp);

            ct.SubmitChanges();

        }
        #endregion


        #region Option Group
        public OptionGroup RetrieveOptionGroup(Int64 Id)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            OptionGroup og = dataContext.OptionGroups.FirstOrDefault(s => s.Id == Id);
            return og;
        }
        public void SaveOptionGroup(OptionGroup og)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.OptionGroups.InsertOnSubmit(og);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveOptionGroupSubjects(OptionGroupSubject ogs)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.OptionGroupSubjects.InsertOnSubmit(ogs);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveOptionGroupStudents(IList<OptionGroupStudent> gsList)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                foreach (OptionGroupStudent gs in gsList)
                {
                    context.OptionGroupStudents.InsertOnSubmit(gs);
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<OptionGroupSubject> RetrieveOptionGroupSubjects(Int64 optionGroupId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allOptionGrp = from gs in context.OptionGroupSubjects where gs.OptionGroupId == optionGroupId select gs;
                return allOptionGrp.ToList<OptionGroupSubject>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<OptionGroup> RetrieveOptionGroupsById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allOptionGrp = from gs in context.OptionGroups where gs.Id == Id select gs;
                return allOptionGrp.ToList<OptionGroup>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<OptionGroup> RetrieveOptionGroups(Int64 campusId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allOptionGrp = from gs in context.OptionGroups where gs.CampusId == campusId select gs;
                return allOptionGrp.ToList<OptionGroup>();
            }
            catch (Exception e) { throw e; }
        }

        public Boolean OptionNameExist(string newOptionName)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            OptionGroup grpn = dataContext.OptionGroups.FirstOrDefault(g => g.OptionGroupName.Trim().ToLower().Equals(newOptionName.Trim().ToLower()));
            if (grpn == null)
                result = false;
            return result;
        }
        public Boolean OptionCodeExist(string newOptionCode)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            OptionGroup grpn = dataContext.OptionGroups.FirstOrDefault(g => g.OptionGroupCode.Trim().ToLower().Equals(newOptionCode.Trim().ToLower()));
            if (grpn == null)
                result = false;
            return result;
        }
        /// <summary>
        /// Please notet that this method will delete the option students and subject before deleting the option group
        /// </summary>
        /// <param name="optionId"></param>
        /// <returns></returns>
        public Boolean DeleteOptionGroup(Int64 optionId)
        {
            bool result = false;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            try
            {
                //delete option students, subjects and option itself
                foreach (OptionGroupStudent ogs in dataContext.OptionGroupStudents.Where(s => s.OptionGroupSubject.OptionGroupId == optionId))
                {                     
                    dataContext.OptionGroupStudents.DeleteOnSubmit(ogs);                    
                }
                foreach (OptionGroupSubject ogsb in dataContext.OptionGroupSubjects.Where(j => j.OptionGroupId == optionId))
                {
                    dataContext.OptionGroupSubjects.DeleteOnSubmit(ogsb);
                }
                dataContext.OptionGroups.DeleteOnSubmit(dataContext.OptionGroups.FirstOrDefault(o => o.Id == optionId));
                dataContext.SubmitChanges();
                result = true;
            }
            catch (Exception ex) 
            {

            }
            return result;
        }
        #endregion
    }
}
