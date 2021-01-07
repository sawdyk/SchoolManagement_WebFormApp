using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PASSIS.LIB.Utility;

namespace PASSIS.LIB
{
    public class HomeRoomLIB
    {
        public void SaveHomeRoom(HomeRoom homeRoom)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.HomeRooms.InsertOnSubmit(homeRoom);
                context.SubmitChanges();

                
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void DeleteHomeRoom(Int64 homeRoomId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                HomeRoom h = context.HomeRooms.FirstOrDefault(hr => hr.Id == homeRoomId);
                if (h != null)
                {
                    context.HomeRooms.DeleteOnSubmit(h);
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveHomeRoomTeacher(HomeRoomTeacher homeRoomTeacher)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.HomeRoomTeachers.InsertOnSubmit(homeRoomTeacher);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<HomeRoom> GetAllHomeRooms(Int64 schoolId, Int64 campusId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var res = from c in context.HomeRooms where c.SchoolId == schoolId && c.SchoolCampusId == campusId select c;
                return res.ToList<HomeRoom>();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void UpdateHomeRoom(HomeRoom h)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                HomeRoom dbObj = context.HomeRooms.FirstOrDefault(hr => hr.Id == h.Id);
                dbObj.Id = h.Id;
                dbObj.Name = h.Name;
                dbObj.RoomNumber = h.RoomNumber;
                //dbObj.SchoolCampusId = h.SchoolCampusId;
                //dbObj.SchoolId = h.SchoolId;                
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }

        }
        public bool HomeRoomExist(string homeroom)
        {
            bool result = false;
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                HomeRoom hm = context.HomeRooms.FirstOrDefault(u => u.Name.ToLower().Equals(homeroom));
                if (hm == null) { } else { result = true; }
            }
            catch { }
            return result;
        }
        public bool HomeRoomExist(string homeroom, Int64 originalObjId)
        {
            bool result = false;
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                HomeRoom hm = context.HomeRooms.FirstOrDefault(u => u.Name.ToLower().Equals(homeroom) && u.Id != originalObjId);
                if (hm == null) { } else { result = true; }
            }
            catch { }
            return result;
        }
        //public HomeRoom RetrieveHomeRoomByName(Int64 schoolId, string name)
        //{
        //    PASSISDataContext context = new PASSISDataContext();

        //    HomeRoom usr = context.HomeRooms.FirstOrDefault(s => s. == schoolId && s.Name.Trim().ToLower().Equals(name.Trim().ToLower()));
        //    return usr;
        //}
        //public HomeRoom RetrieveClass_GradeByCode(Int64 schoolId, string code)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    HomeRoom usr = context.Class_Grades.FirstOrDefault(s => s.School == schoolId && s.Code.Trim().ToLower().Equals(code.Trim().ToLower()));
        //    return usr;
        //} 
    }
}
