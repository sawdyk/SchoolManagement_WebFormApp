using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    
    public class AttendanceLIB
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();

        public void SaveAttendance(AttendanceRegister Attendance)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.AttendanceRegisters.InsertOnSubmit(Attendance);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IList<AttendancePeriod> RetrieveAttendancePeriod()
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from s in context.AttendancePeriods select s;
                return all.ToList<AttendancePeriod>();
            }
            catch (Exception ex) { throw ex; }

        }


    }
}
