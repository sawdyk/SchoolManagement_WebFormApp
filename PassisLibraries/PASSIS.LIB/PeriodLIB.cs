using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class PeriodLIB
    {
        public void SavePeriod(Period per)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Periods.InsertOnSubmit(per);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<Period> getAllPeriods()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allPeriods = context.Periods;
            return allPeriods.ToList<Period>();
        }
        public IList<Period> getAllPeriods(Int64 schoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allPeriods = from per in context.Periods where per.SchoolID == schoolId select per;

            return allPeriods.ToList<Period>();
        }
        public IList<Period> getAllPeriods(Int64 schoolId, Int64 classId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allPeriods = from per in context.Periods where per.SchoolID == schoolId select per;

            return allPeriods.ToList<Period>();
        }
        //public IList<Period> RetrievePeriodsByName(string name)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    var allPeriods = from rol in context.Periods where rol.PeriodName.Contains(name) select rol;
        //    return allPeriods.ToList<Period>();
        //}
        //public IList<Period> RetrievePeriods(string code, string name)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    var allPeriods = from rol in context.Periods where rol.PeriodCode.Contains(code) select rol;
        //    return allPeriods.ToList<Period>();
        //}
        public Period RetrievePeriodById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Period per = context.Periods.SingleOrDefault(s => s.ID == Id);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
        public Period RetrievePeriod(long schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Period per = context.Periods.SingleOrDefault(s => s.SchoolID == schoolId);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
        public Period RetrievePeriod(long schoolId, Int64 classId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Period per = context.Periods.SingleOrDefault(s => s.SchoolID == schoolId);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
    }
}
