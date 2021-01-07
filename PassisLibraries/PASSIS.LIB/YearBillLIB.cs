using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class YearBillLIB
    {
        public void SaveYearBill(YearBill per)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.YearBills.InsertOnSubmit(per);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<YearBill> getAllYearBills()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allYearBills = context.YearBills;
            return allYearBills.ToList<YearBill>();
        }
        public YearBill RetrieveYearBillById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                YearBill per = context.YearBills.SingleOrDefault(s => s.Id == Id);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<YearBill> RetrieveYearBill(long schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from aca in context.YearBills where aca.SchoolId == schoolId select aca;
                return all.ToList<YearBill>();
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<YearBill> RetrieveYearBill(long schoolId, long feeYear)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from aca in context.YearBills where aca.SchoolId == schoolId && aca.FeeYear == feeYear select aca;
                return all.ToList<YearBill>();
            }
            catch (Exception ex) { throw ex; }

        }
    }
}
