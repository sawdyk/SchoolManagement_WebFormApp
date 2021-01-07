using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class FeeLIB
    {
        public void SaveFee(Fee fee)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Fees.InsertOnSubmit(fee);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void SaveFeeConfig(FeesConfig feesConfig)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.FeesConfigs.InsertOnSubmit(feesConfig);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IList<Fee> getAllFees(Int64 schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allFees = from cls in context.Fees where (long)cls.SchoolId == schoolId select cls;
                return allFees.ToList<Fee>();
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// Add year to method call
        /// </summary>
        /// <param name="campusId"></param>
        /// <param name="batchName"></param>
        /// <param name="billingId"></param>
        /// <returns></returns>
        public IList<BatchStudent> getBatches(Int64 campusId, string batchName, string billingId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allFees = from bs in context.BatchStudents select bs;

                if (campusId > 0)
                    allFees = allFees.Where(p => p.User.SchoolCampusId == campusId);
                if (!string.IsNullOrEmpty(batchName))
                    allFees = allFees.Where(n => n.Batch.BatchDescription.ToUpper().Contains(batchName.ToUpper()));
                if (!string.IsNullOrEmpty(billingId))
                    allFees = allFees.Where(c => c.BillingId.ToUpper().Contains(billingId.ToUpper()));
                return allFees.ToList<BatchStudent>();
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<Fee> getAllFees(Int64 schoolId, string name, string code)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allFees = from cls in context.Fees select cls;

                if (schoolId > 0)
                    allFees = allFees.Where(p => p.SchoolId == schoolId);
                if (!string.IsNullOrEmpty(name))
                    allFees = allFees.Where(n => n.Name.ToUpper().Contains(name.ToUpper()));
                if (!string.IsNullOrEmpty(code))
                    allFees = allFees.Where(c => c.Code.ToUpper().Contains(code.ToUpper()));
                return allFees.ToList<Fee>();
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<FeeStatus> getAllFeeStatus()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getFeeStatus = from feeStatus in context.FeeStatus
                               select feeStatus;
            return getFeeStatus.ToList<FeeStatus>();
        }
        public Fee RetrieveFee(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Fee usr = context.Fees.SingleOrDefault(s => s.Id == Id);
                return usr;
            }
            catch (Exception ex) { throw ex; }
        }
        #region batch system
        public IList<Batch> getAllBatches()
        {
            PASSISLIBDataContext c = new PASSISLIBDataContext();
            var batchList = from b in c.Batches select b;
            return batchList.ToList<Batch>();
        }
        public IList<BatchFee> getAllBatchFees(Int64 batchId)
        {
            PASSISLIBDataContext c = new PASSISLIBDataContext();
            var batchFeeList = from bf in c.BatchFees where bf.BatchId == batchId select bf;
            return batchFeeList.ToList<BatchFee>();
        }
        public Int64 getCountOfPreviousGeneratedBillings()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allBills = from bs in context.BatchStudents select bs;
            return allBills.Count<BatchStudent>();
        }
        public void SaveBatchFee(BatchFee batchFee)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.BatchFees.InsertOnSubmit(batchFee);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatchFees(IList<BatchFee> batchFees)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                foreach (BatchFee b in batchFees)
                {
                    context.BatchFees.InsertOnSubmit(b);
                    context.SubmitChanges();
                }

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatchStudent(BatchStudent batchStudent)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.BatchStudents.InsertOnSubmit(batchStudent);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatchStudents(IList<BatchStudent> batchStudents)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                foreach (BatchStudent b in batchStudents)
                {
                    context.BatchStudents.InsertOnSubmit(b);
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatchFeesForSage(IList<BatchFeesForSage> batchFeesForSageList)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                foreach (BatchFeesForSage b in batchFeesForSageList)
                {
                    context.BatchFeesForSages.InsertOnSubmit(b);
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatch(Batch batch)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Batches.InsertOnSubmit(batch);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatchFeesForSage(BatchFeesForSage batchFeesForSage)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.BatchFeesForSages.InsertOnSubmit(batchFeesForSage);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveBatches(IList<Batch> batches)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                foreach (Batch b in batches)
                {
                    context.Batches.InsertOnSubmit(b);
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<Fee> RetrieveBillingFees(Int64 studentId,long? TermId, long SessionId, long ClassId, long SchoolCampusId, long? SchoolId)
        {
            try
            {
                string billingId = string.Empty;
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                BatchStudent bs = context.BatchStudents.FirstOrDefault(b => b.StudentId == studentId && b.TermId == TermId && b.SessionId == SessionId && b.ClassId == ClassId && b.SchoolCampusId == SchoolCampusId && b.SchoolId == SchoolId);
                var batchstd = (from b in context.BatchStudents where b.Status == true && b.StudentId == studentId && b.TermId == TermId && b.SessionId == SessionId && b.ClassId == ClassId && b.SchoolCampusId == SchoolCampusId && b.SchoolId == SchoolId select b).OrderByDescending(d => d.Batch.DateCreated.Date).ThenBy(t => t.Batch.DateCreated.TimeOfDay);
                if (batchstd.Count() > 0)
                    return (from f in context.BatchFees where f.BatchId == batchstd.ToList<BatchStudent>()[0].BatchId select f.Fee).ToList<Fee>();
                else
                    return new List<Fee>();
            }
            catch (Exception ex) { throw ex; }

        }
        public BatchStudent RetrieveBillingByBillId(string billingId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                BatchStudent bs = context.BatchStudents.FirstOrDefault(b => b.BillingId.ToLower().Equals(billingId));
                return bs;
            }
            catch (Exception ex) { throw ex; }

        }
        public BatchStudent RetrieveBillingByUserId(Int64 userId)
        {
            try
            {
                string billingId = string.Empty;
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                BatchStudent bs = context.BatchStudents.FirstOrDefault(b => b.StudentId == userId);
                return bs;
            }
            catch (Exception ex) { throw ex; }

        }
         /// <summary>
         /// this method select the latest batch cretaed for each student
         /// </summary>
         /// <param name="studentId"></param>
         /// <returns></returns>
        public string RetrieveBillingByAdmissionNumber(string admissionNumber)
        {
            try
            {
                string billingId = string.Empty;
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                User usr = context.Users.FirstOrDefault(u => u.AdmissionNumber.ToLower().Equals(admissionNumber));
                if (usr != null)
                {
                    BatchStudent bs = context.BatchStudents.FirstOrDefault(b => b.StudentId == usr.Id);
                    billingId = bs.BillingId;
                }
                return billingId;
            }
            catch (Exception ex) { throw ex; }

        }
        #endregion

    }
}
