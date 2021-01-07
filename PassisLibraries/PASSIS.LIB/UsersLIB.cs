using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PASSIS.LIB.Utility;

namespace PASSIS.LIB
{
    public class UsersLIB: BasePage
    {
        public void SaveParentDetail(ParentDetail pd)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.ParentDetails.InsertOnSubmit(pd);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }

        public void SaveUser(User usr)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Users.InsertOnSubmit(usr);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveUserRole(UserRole usrRol)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.UserRoles.InsertOnSubmit(usrRol);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<User> getAllUsers()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = context.Users;
            return allUsers.ToList<User>();
        }

        public IList<User> getAllUsers(Int64 schlId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = from usr in context.Users where usr.SchoolId == schlId select usr;
            return allUsers.ToList<User>();
        }
        public IList<User> getAllUsersPerCampus(Utility.roles role, Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = from usr in context.UserRoles where usr.Role.Id == (long)role && usr.User.SchoolId == schoolId select usr.User;
            return allUsers.ToList<User>();
        }
        public IList<User> getAllUsers(Utility.roles role)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = from usr in context.UserRoles where usr.Role.Id == (long)role select usr.User;
            return allUsers.ToList<User>();
        }
        public IList<User> RetrieveUsersByName(string username)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = from usr in context.Users where usr.Username.Trim().ToLower().Contains(username.ToLower()) &&
                           usr.SchoolId == logonUser.SchoolId &&
                           usr.SchoolCampusId == logonUser.SchoolCampusId
                           select usr;
            return allUsers.ToList<User>();
        }
        public IList<User> RetrieveUsersByLastname(string firstname)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = from usr in context.Users
                           where usr.FirstName.Trim().ToLower().Contains(firstname.ToLower()) &&
                               usr.SchoolId == logonUser.SchoolId &&
                               usr.SchoolCampusId == logonUser.SchoolCampusId
                           select usr;
            return allUsers.ToList<User>();
        }
        /// <summary>
        /// this method (RetrieveUser) is dangerous
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        //public User RetrieveUser()
        //{
        //    PASSISLIBDataContext context = new PASSISLIBDataContext();
        //    User stf = context.Users.SingleOrDefault(s => s.Username.Trim().ToLower().Equals(logonUser.Username.Trim().ToLower()));
        //    return stf;
        //}
      


     
        public User RetrieveUser(string username)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            User stf = context.Users.SingleOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()));
            return stf;
        }
        public User RetrieveUser(string username, Int64 exemptId)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            User stf = dataContext.Users.FirstOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()) && s.Id != exemptId);
            return stf;
        }
        public bool TeacherCodeExist(string teacherCode)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            User stf = dataContext.Users.FirstOrDefault(s => s.TeacherCode.Trim().ToLower().Equals(teacherCode.Trim().ToLower()));
            if (stf == null)
                result = false;
            return result;
        }
        public bool usernameExist(string username)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            User stf = dataContext.Users.FirstOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()));
            if (stf == null)
                result = false;
            return result;
        }
        public bool usernameExist(string username, Int64 exemptId, long roleId)
        {
            bool result = true;
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            UserRole stf = dataContext.UserRoles.FirstOrDefault(s => s.User.Username.Trim().ToLower().Equals(username.Trim().ToLower()) && s.User.Id != exemptId && s.RoleId == (long)roleId);
            if (stf == null)
                result = false;
            return result;
        }
        public User RetrieveUserByMailAddress(string emailAddress, Int64 exemptId)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            User stf = dataContext.Users.FirstOrDefault(s => s.EmailAddress.Trim().ToLower().Equals(emailAddress.Trim().ToLower()) && s.Id != exemptId);
            return stf;
        }
        public IList<User> RetrieveSingleUserList(long Id)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            var usersFound = from usr in dataContext.Users where usr.Id == Id select usr;
            //User stf = dataConKCtext.Users.SingleOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()));
            return usersFound.ToList<User>();
        }
        public IList<User> RetrieveUser(string username, Utility.roles role)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            var usersFound = from usr in dataContext.UserRoles where usr.User.Username.Trim().ToLower().Equals(username) && usr.RoleId == (long)role select usr.User;
            //User stf = dataConKCtext.Users.SingleOrDefault(s => s.Username.Trim().ToLower().Equals(username.Trim().ToLower()));
            return usersFound.ToList<User>();
        }
        public User RetrieveUser(string username, string password)
        {
            PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
            User stf = dataContext.Users.FirstOrDefault(s => s.Username.Equals(username) && s.Password.Equals(password));
            return stf;
        }

        //public User RetrieveUser(string username, string password, int userCategory)
        //{
        //    PASSISDataContext dataContext = new PASSISDataContext();
        //    User stf = dataContext.Users.SingleOrDefault(s => s.Username.Equals(username) && s.Password.Equals(password) && s.UserCategory == userCategory);
        //    return stf;
        //}
        //public IList<User> RetrieveUsersByCode(string code)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    var allUsers = from usr in context.Users where usr.UserCode.Contains(code) select usr;
        //    return allUsers.ToList<User>();
        //}
        public IList<User> RetrieveUserToList(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var res = from u in context.Users where u.Id == Id select u;
            return res.ToList<User>();
        }
        public User RetrieveUser(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            User usr = context.Users.SingleOrDefault(s => s.Id == Id);
            return usr;
        }
        public AdmissionUser RetrieveAdmissionUser(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            AdmissionUser usr = context.AdmissionUsers.SingleOrDefault(s => s.Id == Id);
            return usr;
        }
        public UserRole RetrieveUserRole(Int64 userId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                UserRole usrRl = context.UserRoles.SingleOrDefault(s => s.UserId == userId);
                return usrRl;
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<User> RetrieveUsersBelowRole(Int64 roleId)
        {
            return RetrieveUsersBelowRole(roleId, 0);
        }
        public IList<User> RetrieveUsersList(IList<Int64> userIds)
        {
            IList<User> results = new List<User>();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUser = (System.Linq.IQueryable<User>)null;
                allUser = from usr in context.Users where userIds.ToList<Int64>().Contains(usr.Id) select usr;
                results = allUser.ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        public IList<User> RetrieveUsersBelowRole(Int64 roleId, Int64 schoolId)
        {
            IList<User> results = new List<User>();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                IList<Int64> IRoleIds = RetrieveRolesBelow(roleId);
                long[] roleIds = IRoleIds.ToArray();
                var allUserIds = from usrRol in context.UserRoles where roleIds.Contains((long)usrRol.RoleId) select (long)usrRol.UserId;
                var allUser = (System.Linq.IQueryable<User>)null;
                if (schoolId == 0)
                    allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) select usr;
                else
                    allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) && usr.SchoolId == schoolId select usr;
                results = allUser.ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        public IList<User> RetrieveUsersBelowRole(Int64 roleId, Int64 schoolId, Int64 excemptRoleId, string filterbyUsername)
        {
            return RetrieveUsersBelowRole(roleId, schoolId, excemptRoleId, filterbyUsername, 0);
        }
        public IList<User> RetrieveUsersBelowRole(Int64 roleId, Int64 schoolId, Int64 excemptRoleId, string filterbyUsername, Int64 campusId)
        {
            IList<User> results = new List<User>();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                IList<Int64> IRoleIds = RetrieveRolesBelowWithException(roleId, excemptRoleId);
                //IList<Int64> IRoleIds = RetrieveRolesBelow(roleId);
                long[] roleIds = IRoleIds.ToArray();
                var allUserIds = (System.Linq.IQueryable<Int64>)null;
                if (campusId > 0)
                    allUserIds = from usrRol in context.UserRoles where roleIds.Contains((long)usrRol.RoleId) && usrRol.User.SchoolCampusId == campusId select (long)usrRol.UserId;
                else
                    allUserIds = from usrRol in context.UserRoles where roleIds.Contains((long)usrRol.RoleId) select (long)usrRol.UserId;
                var allUser = (System.Linq.IQueryable<User>)null;
                if (schoolId == 0)
                {
                    if (string.IsNullOrEmpty(filterbyUsername))
                        allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) select usr;
                    else
                        allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) && usr.Username.Contains(filterbyUsername) select usr;
                }
                else
                {
                    if (string.IsNullOrEmpty(filterbyUsername))
                        allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) && usr.SchoolId == schoolId select usr;
                    else
                        allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) && usr.SchoolId == schoolId && usr.Username.Contains(filterbyUsername) select usr;

                }
                results = allUser.ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        public IList<User> RetrieveUsersBelowRole(Int64 roleId, Int64 schoolId, Int64 excemptRoleId)
        {
            return RetrieveUsersBelowRole(roleId, schoolId, excemptRoleId, 0);
        }
        public IList<User> RetrieveUsersBelowRole(Int64 roleId, Int64 schoolId, Int64 excemptRoleId, Int64 campusId)
        {
            IList<User> results = new List<User>();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                IList<Int64> IRoleIds = RetrieveRolesBelowWithException(roleId, excemptRoleId);
                //IList<Int64> IRoleIds = RetrieveRolesBelow(roleId);
                long[] roleIds = IRoleIds.ToArray();
                var allUserIds = (System.Linq.IQueryable<Int64>)null;
                if (campusId > 0)
                    allUserIds = from usrRol in context.UserRoles where roleIds.Contains((long)usrRol.RoleId) && usrRol.User.SchoolCampusId == campusId select (long)usrRol.UserId;
                else
                    allUserIds = from usrRol in context.UserRoles where roleIds.Contains((long)usrRol.RoleId) select (long)usrRol.UserId;
                var allUser = (System.Linq.IQueryable<User>)null;
                if (schoolId == 0)
                    allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) select usr;
                else
                    allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) && usr.SchoolId == schoolId select usr;
                results = allUser.ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        public IList<User> RetrieveParentsChildren(Int64 parentUserId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var childrenUserIds = from chil in context.ParentStudentMaps where chil.ParentUserId == parentUserId select (long)chil.StudentId;
                var allChildren = from usr in context.Users where childrenUserIds.ToArray().Contains(usr.Id) select usr;
                return allChildren.ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<GradeStudent> RetrieveParentsChildrenFromGradeStudent(Int64 parentUserId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var childrenUserIds = from chil in context.ParentStudentMaps where chil.ParentUserId == parentUserId select (long)chil.StudentId;
                var allChildren = from usr in context.GradeStudents where childrenUserIds.ToArray().Contains(usr.StudentId) select usr;
                return allChildren.ToList<GradeStudent>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<Int64> RetrieveParentsChildrenUserIds(Int64 parentUserId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var childrenUserIds = from chil in context.ParentStudentMaps where chil.ParentUserId == parentUserId select (long)chil.StudentId;
                var allChildren = from usr in context.Users where childrenUserIds.ToArray().Contains(usr.Id) select usr.Id;
                return allChildren.ToList<Int64>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<Int64> RetrieveRolesBelowWithException(Int64 roleId, Int64 excemptRoleId)
        {
            try
            {
                IList<User> results = new List<User>();
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                //var allRoles = from rol in context.Roles where rol.ParentRoleId == roleId select rol;
                //return allRoles.ToList<Role>();
                var allRoles = from rol in context.Roles where rol.ParentRoleId == roleId && rol.Id != excemptRoleId select rol.Id;
                return allRoles.ToList<Int64>();
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<Int64> RetrieveRolesBelow(Int64 roleId)
        {
            try
            {
                IList<User> results = new List<User>();
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                //var allRoles = from rol in context.Roles where rol.ParentRoleId == roleId select rol;
                //return allRoles.ToList<Role>();
                var allRoles = from rol in context.Roles where rol.ParentRoleId == roleId select rol.Id;
                return allRoles.ToList<Int64>();
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<User> RetrieveUsersInRole(Int64 roleId)
        {
            IList<User> results = new List<User>();
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allUsers = from usrRol in context.UserRoles where usrRol.RoleId == roleId select usrRol.User;
            return allUsers.ToList<User>();
        }
        public IList<User> RetrieveUsersInRole(Int64 roleId, Int64 schoolId)
        {
            IList<User> results = new List<User>();
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            var allUsers = from usrRol in context.UserRoles where usrRol.RoleId == roleId && (long)usrRol.User.SchoolId == schoolId select usrRol.User;
            return allUsers.ToList<User>();
        }

        public IList<User> RetrieveUsersInRoleByCampus(Int64 roleId, Int64 schoolId, Int64 CampusId)
        {
            IList<User> results = new List<User>();
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            var allUsers = from usrRol in context.UserRoles where usrRol.RoleId == roleId && (long)usrRol.User.SchoolId == schoolId  && (long)usrRol.User.SchoolCampusId == CampusId select usrRol.User;
            return allUsers.ToList<User>();
        }
        /*
        make saearchable by role, username , Name , Campus ,if student is selected display grade
        */

        public IList<User> RetrieveStudents(Int64 schoolId, Int64 campusId, Int64 yearId, Int64 gradeId, string name, string fathersName, string matricNumber, string admissionNumber, Int64 SelectedLearningSupport, Int64 sessionId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents.Where(x=> x.HasGraduated == null || x.HasGraduated == false) select grd;
                //if (schoolId > 0)
                //{
                //    allUsers = allUsers.Where(c => c.User.SchoolId == schoolId);
                //}
                if (campusId > 0)
                {
                    allUsers = allUsers.Where(c => c.User.SchoolCampusId == campusId);
                }
                if (sessionId > 0)
                {
                    allUsers = allUsers.Where(c => c.AcademicSessionId == sessionId);
                }
                if (yearId > 0)
                {
                    allUsers = allUsers.Where(s => s.ClassId == yearId);
                }
                if (gradeId > 0)
                {
                    allUsers = allUsers.Where(s => s.GradeId == gradeId);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    allUsers = allUsers.Where(c => c.User.StudentFullName.ToLower().Contains(name.ToLower()));
                }
                if (!string.IsNullOrEmpty(fathersName))
                {
                    allUsers = allUsers.Where(c => c.User.FathersName.ToLower().Contains(fathersName.ToLower()));
                }
                if (!string.IsNullOrEmpty(admissionNumber))
                {
                    allUsers = allUsers.Where(c => c.User.AdmissionNumber.ToLower().Contains(admissionNumber.ToLower()));
                }
                if (!string.IsNullOrEmpty(matricNumber))
                {
                    allUsers = allUsers.Where(c => c.User.MatricNumber.ToLower().Contains(matricNumber.ToLower()));
                }
                if (!SelectedLearningSupport.Equals("-1"))
                {
                    Int32 ls = Convert.ToInt32(SelectedLearningSupport);
                    allUsers = allUsers.Where(c => c.User.IsLearningSupport == ls);
                }
                var result = from res in allUsers select res.User;

                return result.ToList<User>();
            }
            catch (Exception e) { throw e; }
        }

       

        public IList<User> RetrieveStudents(Int64 schoolId, Int64 campusId, Int64 yearId, string name, string fathersName, string matricNumber, string admissionNumber)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents.Where(x => x.HasGraduated == null || x.HasGraduated == false) select grd;
                //if (schoolId > 0)
                //{
                //    allUsers = allUsers.Where(c => c.User.SchoolId == schoolId);
                //}
                if (campusId > 0)
                    allUsers = allUsers.Where(c => c.User.SchoolCampusId == campusId);
                if (yearId > 0)
                    allUsers = allUsers.Where(s => s.ClassId == yearId);
                
                if (!string.IsNullOrEmpty(name))
                    allUsers = allUsers.Where(c => c.User.StudentFullName.ToLower().Contains(name.ToLower()));
                if (!string.IsNullOrEmpty(fathersName))
                    allUsers = allUsers.Where(c => c.User.FathersName.ToLower().Contains(name.ToLower()));
                if (!string.IsNullOrEmpty(matricNumber))
                    allUsers = allUsers.Where(c => c.User.MatricNumber.ToLower().Contains(matricNumber.ToLower()));
                if (!string.IsNullOrEmpty(admissionNumber))
                    allUsers = allUsers.Where(c => c.User.AdmissionNumber.ToLower().Contains(admissionNumber.ToLower()));

                var result = from res in allUsers select res.User;

                return result.ToList<User>();
            }
            catch (Exception e) { throw e; }
        }

        public IList<User> RetrieveStudents(Int64 schoolId, Int64 campusId, Int64 yearId, Int64 gradeId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents.Where(x => x.HasGraduated == null || x.HasGraduated == false) select grd;
                //if (schoolId > 0)
                //{
                //    allUsers = allUsers.Where(c => c.User.SchoolId == schoolId);
                //}
                if (campusId > 0)
                    allUsers = allUsers.Where(c => c.User.SchoolCampusId == campusId);
                if (yearId > 0)
                    allUsers = allUsers.Where(s => s.ClassId == yearId);
                
                if (gradeId > 0)
                    allUsers = allUsers.Where(s => s.GradeId == gradeId);
                var result = from res in allUsers select res.User;

                return result.ToList<User>();
            }
            catch (Exception e) { throw e; }
        }


        public IList<User> RetrieveStudents(Int64 schoolId, Int64 campusId, Int64 yearId, Int64 gradeId, Int64 sessionId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allUsers = from grd in context.GradeStudents.Where(x => x.HasGraduated == null || x.HasGraduated == false) select grd;
                //if (schoolId > 0)
                //{
                //    allUsers = allUsers.Where(c => c.User.SchoolId == schoolId);
                //}
                if (campusId > 0)
                    allUsers = allUsers.Where(c => c.User.SchoolCampusId == campusId);
                if (yearId > 0)
                    allUsers = allUsers.Where(s => s.ClassId == yearId);

                if (gradeId > 0)
                    allUsers = allUsers.Where(s => s.GradeId == gradeId);
                if (sessionId > 0)
                    allUsers = allUsers.Where(s => s.AcademicSessionId == sessionId);
                var result = from res in allUsers select res.User;

                return result.ToList<User>();
            }
            catch (Exception e) { throw e; }
        }

        public void UpdateUser(User usr)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                User user = context.Users.SingleOrDefault(s => s.Id == usr.Id);
                //user = usr;
                user.FirstName = usr.FirstName;
                user.LastName = usr.LastName;
                user.MiddleName = usr.MiddleName;
                user.Username = usr.Username;
                user.StudentFullName = usr.StudentFullName;
                user.EmailAddress = usr.EmailAddress;
                user.Gender = usr.Gender;
                user.LastLoginDate = DateTime.Now;
                user.AccomodationType = usr.AccomodationType;
                user.AdmissionNumber = usr.AdmissionNumber;
                user.BusRoute = usr.BusRoute;
                user.Status = usr.Status;
                user.City = usr.City;
                user.Country = usr.Country;
                user.DateOfBirth = usr.DateOfBirth;
                user.EmailAddress = usr.EmailAddress;
                user.FathersEmail = "";
                user.FathersName = user.FathersNationality = user.FathersPhoneNumber = user.FathersTitle = user.FathersWorkAddress = string.Empty;
                user.GuardianDetails = user.GuardianEmail = user.GuardianPhoneNumber = user.GuardianRelationship = string.Empty;
                user.LastSchoolAttended = usr.LastSchoolAttended;
                user.LastSchoolAttendedCityCountry = usr.LastSchoolAttendedCityCountry;
                user.LastSchoolAttendedClass = usr.LastSchoolAttendedClass;
                user.LastSchoolAttendedYear = usr.LastSchoolAttendedYear;
                user.MatricNumber = usr.MatricNumber;
                user.MothersEmail = user.MothersName = user.MothersNationality = user.MothersOtherName = user.MothersPhoneNumber = string.Empty;
                user.MothersWorkAddress = string.Empty;
                user.PassportFileName = usr.PassportFileName;
                user.Password = usr.Password;
                user.PhoneNumber = usr.PhoneNumber;
                user.SchoolCampusId = usr.SchoolCampusId;
                user.SchoolHouse = usr.SchoolHouse;
                user.SchoolHouseParentName = usr.SchoolHouseParentName;
                user.SchoolId = usr.SchoolId;
                user.Siblings = usr.Siblings;
                user.SpecialNoteAlert = usr.SpecialNoteAlert;
                user.State = usr.State;
                user.StreetAddress = usr.StreetAddress;
                user.Username = usr.Username;
                user.IsLearningSupport = usr.IsLearningSupport;
                user.DateCreated = usr.DateCreated;
                user.CreatedBy = usr.CreatedBy;
                user.UserStatus = usr.UserStatus;
                user.TeacherCode = usr.TeacherCode;
                user.StudentStatus = usr.StudentStatus;
                context.SubmitChanges();
                
            }
            catch (Exception ex)
            { throw ex; }

        }

        public void UpdateParentDetail(ParentDetail p)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                ParentDetail pd = context.ParentDetails.FirstOrDefault(s => s.Id == p.Id);
                pd.FathersEmail = p.FathersEmail;
                pd.FathersName = p.FathersName;
                //pd.FathersName_Id = p.FathersName_Id;
                pd.FathersNationality = p.FathersNationality;
                pd.FathersOccupation = p.FathersOccupation;
                pd.FathersPhoneNumber = p.FathersPhoneNumber;
                pd.FathersTitle = p.FathersTitle;
                pd.FathersWorkAddress = p.FathersWorkAddress;
                pd.GuardianDetails = p.GuardianDetails;
                pd.GuardianEmail = p.GuardianEmail;
                pd.GuardianPhoneNumber = p.GuardianPhoneNumber;
                pd.GuardianRelationship = p.GuardianRelationship;
                pd.MothersEmail = p.MothersEmail;
                pd.MothersName = p.MothersName;
                pd.MothersNationality = p.MothersNationality;
                pd.MothersOccupation = p.MothersOccupation;
                pd.MothersOtherName = p.MothersOtherName;
                pd.MothersPhoneNumber = p.MothersPhoneNumber;
                pd.MothersWorkAddress = p.MothersWorkAddress;
                pd.Siblings = p.Siblings;

                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }

        }
        public User RetrieveByAdmissionNumber(string admissionNumber)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                User usr = context.Users.FirstOrDefault(u => u.AdmissionNumber.ToLower().Equals(admissionNumber));
                return usr;
            }
            catch (Exception ex) { throw ex; }

        }
        public bool AdmissionNumberExist(string admissionNumber)
        {
            bool result = false;
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                User usr = context.Users.FirstOrDefault(u => u.AdmissionNumber.ToLower().Equals(admissionNumber));
                if (usr == null) { } else { result = true; }
            }
            catch { }
            return result;
        }
        public string getStudentsHomeRoom(Int64 id)
        {
            PASSISLIBDataContext cn = new PASSISLIBDataContext();
            GradeStudent gd = cn.GradeStudents.FirstOrDefault(g => g.StudentId == id);
            Class_Grade cl = cn.Class_Grades.FirstOrDefault(c => c.Id == gd.ClassId);
            Grade gr = cn.Grades.FirstOrDefault(r => r.Id == gd.GradeId);
            //return string.Format("{0} {1}", cl.Name, gr.GradeName);
            return string.Format("{0}" , gr.GradeName);
        }
        #region parent
        public ParentDetail RetrieveParent(Int64 Id)
        {
            ParentDetail pr = new ParentDetail();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                ParentDetail pd = context.ParentDetails.FirstOrDefault(p => p.Id == Id);
                pr = pd;
            }
            catch { }
            return pr;
        }
        public ParentDetail RetrieveParentDetail(string fathersPhoneNumber)
        {
            ParentDetail pr = new ParentDetail();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                ParentDetail pd = context.ParentDetails.FirstOrDefault(p => p.FathersPhoneNumber.Contains(fathersPhoneNumber.Trim()));
                pr = pd;
            }
            catch { }
            return pr;
        }
        public ParentDetail RetrieveParentDetail(Int64 parentUserId)
        {
            ParentDetail pr = new ParentDetail();
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                ParentStudentMap psm = context.ParentStudentMaps.FirstOrDefault(p => p.ParentUserId == parentUserId);
                pr = context.ParentDetails.SingleOrDefault(s => s.Id == psm.ParentId);
            }
            catch { }
            return pr;
        }

        public List<string> RetrieveParentsNameWithId(string prefix)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            var allUsers = from p in context.ParentDetails where p.FathersName.Contains(prefix) select p.FathersName;
            return allUsers.ToList<string>();
        }
        #endregion

    }
}
