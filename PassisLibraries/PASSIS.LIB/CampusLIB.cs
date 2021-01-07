using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class CampusLIB
    {
        public void UpdateCampus(string Name, string Code, string CampusAddress, int Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            SchoolCampus campusObj = context.SchoolCampus.FirstOrDefault(x => x.Id == Id);
            campusObj.Name = Name;
            campusObj.Code = Code;
            campusObj.CampusAddress = CampusAddress;
            context.SubmitChanges();
        }

        public IList<SchoolCampus> GetAllCampus(string SchoolId)
        {
            Int64 schoolId = Convert.ToInt64(SchoolId);
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var campusObj = from campus in context.SchoolCampus
                            where campus.SchoolId== schoolId
                            select campus;
            return campusObj.ToList<SchoolCampus>();
        }

        public void SaveCampus(SchoolCampus newCampus)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.SchoolCampus.InsertOnSubmit(newCampus);
            context.SubmitChanges();
        }

        public bool CampusExist(string campusName, string campusCode)
        {
            bool result = false;
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var hm = from campus in context.SchoolCampus
                         where campus.Name == campusName || campus.Code == campusCode
                         select campus;
                if (hm.Count() > 0)
                {
                    result = true;
                }
                else { }
            }
            catch { }
            return result;
        }

        public void UpdateCampus(SchoolCampus h)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                SchoolCampus dbObj = context.SchoolCampus.FirstOrDefault(hr => hr.Id == h.Id);
                dbObj.Id = h.Id;
                dbObj.Name = h.Name;
                dbObj.Code = h.Code;
                dbObj.CampusAddress = h.CampusAddress;
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }

        }

        public long GetCampusId(string campusCode)
        {
            long ReturnCampusId;

            using (PASSISLIBDataContext context = new PASSISLIBDataContext())
            {

                var getCampusId = from campus in context.SchoolCampus
                                  where campus.Code == campusCode
                                  select campus.Id;

                if (getCampusId.Count() > 0)
                {
                    ReturnCampusId = getCampusId.FirstOrDefault();
                }
                else
                {
                    ReturnCampusId = 0;
                }
            }
            return ReturnCampusId;
        }

        public bool CampusExist(string campusName)
        {
            bool result = false;
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var hm = from campus in context.SchoolCampus
                         where campus.Name == campusName
                         select campus;
                if (hm.Count() > 0)
                {
                    result = true;
                }
                else { }
            }
            catch { }
            return result;
        }

    }
}
