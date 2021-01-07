using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class YearFeeLIB
    {
        public void SaveYearFee(YearFee per)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.YearFees.InsertOnSubmit(per);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<YearFee> getAllYearFees()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allYearFees = context.YearFees;
            return allYearFees.ToList<YearFee>();
        }


        public YearFee RetrieveYearFeeById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                YearFee per = context.YearFees.SingleOrDefault(s => s.Id == Id);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<YearFee> RetrieveYearFee(long schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from aca in context.YearFees where aca.SchoolId == schoolId select aca;
                return all.ToList<YearFee>();
            }
            catch (Exception ex) { throw ex; }

        }

    }
}
