using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using PASSIS.LIB.Utility;

namespace PASSIS.LIB
{
    public class TransactionLogLIB
    {
        public IList<TransactionFee> RetrieveAllFeesPaid(Int64 studentId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allTransactionFees = from fee in context.TransactionFees where fee.StudentId == studentId select fee;
            return allTransactionFees.ToList<TransactionFee>();
        }
        public IList<Int64> RetrieveAllFeesPaidByFeesId(Int64 studentId, long? TermId, long SessionId, long ClassId, long SchoolCampusId, long? SchoolId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allTransactionFees = from fee in context.TransactionFees where fee.StudentId == studentId && fee.TermId == TermId && fee.SessionId == SessionId && fee.ClassId == ClassId && fee.SchoolCampusId == SchoolCampusId && fee.SchoolId == SchoolId select fee.FeeId;
            return allTransactionFees.ToList<Int64>();
        }
        public IList<TransactionFee> RetrieveTransactionFeesPaid(Int64 trLogId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allTransactionFees = from fee in context.TransactionFees where fee.TransactionLogId == trLogId select fee;

            return allTransactionFees.ToList<TransactionFee>();
        }   //public IList<TransactionLog> RetrieveTransactionLogsByName(string name)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    var allTransactionLogs = from rol in context.TransactionLogs where rol.TransactionLogName.Contains(name) select rol;
        //    return allTransactionLogs.ToList<TransactionLog>();
        //}
        public void SaveTransactionFee(TransactionFee fee)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.TransactionFees.InsertOnSubmit(fee);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveTransactionLog(TransactionLog rol)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.TransactionLogs.InsertOnSubmit(rol);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<TransactionLog> getAllTransactionLogs()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allTransactionLogs = context.TransactionLogs;
            return allTransactionLogs.ToList<TransactionLog>();
        }
        public IList<TransactionLog> getAllTransactionLogs(string trRef, string dtFrom, string dtTo, string stdBillId, long schoolId, long campusId)
        {
            return getAllTransactionLogs(trRef, dtFrom, dtTo, stdBillId, string.Empty, schoolId, campusId);
        }
        public IList<TransactionLog> getAllTransactionLogs(string trRef, string dtFrom, string dtTo, string stdBillId, string admissionNumber, long schoolId, long campusId)
        {
            return getAllTransactionLogs(trRef, dtFrom, dtTo, stdBillId, admissionNumber, "0", schoolId, campusId);
        }
        public IList<TransactionLog> getAllTransactionLogs(string trRef, string dtFrom, string dtTo, string stdBillId, string admissionNumber, string transStatus, long schoolId, long campusId)
        {
            CultureInfo culInfo = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime dateFrom = DateTime.ParseExact(dtFrom, "MM-dd-yyyy", culInfo);
            DateTime dateTo = DateTime.ParseExact(dtTo, "MM-dd-yyyy", culInfo);
            dateTo = dateTo.AddDays(1).Subtract(new TimeSpan(0, 0, 1));
            int transactionStatus = 0;
            try { transactionStatus = Convert.ToInt32(transStatus); }
            catch { }
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allTransactionLogs = from cls in context.TransactionLogs select cls;
                allTransactionLogs = allTransactionLogs.Where(c => c.TransactionDate >= dateFrom && c.TransactionDate <= dateTo && c.SchoolId == schoolId && c.CampusId == campusId);

                if (!string.IsNullOrEmpty(admissionNumber))
                {
                    allTransactionLogs = allTransactionLogs.Where(n => n.User.AdmissionNumber.ToUpper().Equals(admissionNumber.ToUpper()));
                }
                if (!string.IsNullOrEmpty(trRef))
                {
                    allTransactionLogs = allTransactionLogs.Where(n => n.TransactionRef == Convert.ToInt64(trRef));
                }
                if (!string.IsNullOrEmpty(stdBillId))
                {
                    allTransactionLogs = allTransactionLogs.Where(c => c.StudentBillId == stdBillId.ToString());
                }
                if (transactionStatus > 0)
                {
                    allTransactionLogs = allTransactionLogs.Where(c => c.FinalTransactionStatus == transactionStatus);
                }
                return allTransactionLogs.ToList<TransactionLog>();
            }
            catch (Exception ex) { throw ex; }
        }
        public IList<TransactionHistory> getAllSuccessfulTransactionLogs(string trRef, string dtFrom, string dtTo, string admissionNumber, string transStatus, long schoolId, long campusId)
        {
            CultureInfo culInfo = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime dateFrom = DateTime.ParseExact(dtFrom, "MM-dd-yyyy", culInfo);
            DateTime dateTo = DateTime.ParseExact(dtTo, "MM-dd-yyyy", culInfo);
            dateTo = dateTo.AddDays(1).Subtract(new TimeSpan(0, 0, 1));
            int transactionStatus = 0;
            try { transactionStatus = Convert.ToInt32(transStatus); }
            catch { }
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allTransactionLogs = from cls in context.TransactionHistories select cls;
                allTransactionLogs = allTransactionLogs.Where(c => c.TransactionDate >= dateFrom && c.TransactionDate <= dateTo && c.SchoolId == schoolId && c.CampusId == campusId && c.FinalResponseCode.Contains("00"));

                if (!string.IsNullOrEmpty(admissionNumber))
                {
                    allTransactionLogs = allTransactionLogs.Where(n => n.User.AdmissionNumber.ToUpper().Equals(admissionNumber.ToUpper()));
                }
                if (!string.IsNullOrEmpty(trRef))
                {
                    allTransactionLogs = allTransactionLogs.Where(n => n.TransactionRef == Convert.ToInt64(trRef));
                }
                //if (!string.IsNullOrEmpty(stdBillId))
                //{
                //    allTransactionLogs = allTransactionLogs.Where(c => c.StudentBillId == stdBillId.ToString());
                //}
                if (transactionStatus > 0)
                {
                    allTransactionLogs = allTransactionLogs.Where(c => c.FinalTransactionStatus == transactionStatus);
                }
                return allTransactionLogs.ToList<TransactionHistory>();
            }
            catch (Exception ex) { throw ex; }
        }        //public IList<TransactionLog> getAllTransactionLogs(Int32 parentId)
        public IList<TransactionHistory> getAllPendingTransactionLogs(string trRef, string dtFrom, string dtTo, string stdBillId, string admissionNumber, string transStatus, long schoolId, long campusId)
        {
            CultureInfo culInfo = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime dateFrom = DateTime.ParseExact(dtFrom, "MM-dd-yyyy", culInfo);
            DateTime dateTo = DateTime.ParseExact(dtTo, "MM-dd-yyyy", culInfo);
            dateTo = dateTo.AddDays(1).Subtract(new TimeSpan(0, 0, 1));
            int transactionStatus = 0;
            try { transactionStatus = Convert.ToInt32(transStatus); }
            catch { }
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allTransactionLogs = from cls in context.TransactionHistories select cls;
                allTransactionLogs = allTransactionLogs.Where(c => c.TransactionDate >= dateFrom && c.TransactionDate <= dateTo && c.SchoolId == schoolId && c.CampusId == campusId && c.PaymentChannel != Convert.ToInt32(Utility.PaymentChannel.Interswitch) && c.FinalResponseCode != "00");

                return allTransactionLogs.ToList<TransactionHistory>();
            }
            catch (Exception ex) { throw ex; }
        }        //public IList<TransactionLog> getAllTransactionLogs(Int32 parentId)
        public IList<TransactionLog> RetrieveAllFeesPaid(long studentId, long schoolId, long campusId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allTransactionLogs = from tl in context.TransactionLogs
                                         where  tl.SchoolId == schoolId && tl.CampusId == campusId && tl.StudentId == studentId && tl.FinalResponseCode.Contains("00")
                                         select tl;
                return allTransactionLogs.ToList<TransactionLog>();
            }
            catch (Exception ex) { throw ex; }
        } 
        public TransactionLog RetrieveTransactionLog(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                TransactionLog rol = context.TransactionLogs.SingleOrDefault(s => s.Id == Id);
                return rol;
            }
            catch (Exception ex) { throw ex; }

        }
        public TransactionLog RetrieveTransactionLog(string paymentRef)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                TransactionLog rol = context.TransactionLogs.FirstOrDefault(s => s.TransactionRef == Convert.ToInt64(paymentRef));
                return rol;
            }
            catch (Exception ex) { throw ex; }

        }
        public TransactionLog RetrieveTransactionLogByGUID(string globalUniqueId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                TransactionLog rol = context.TransactionLogs.FirstOrDefault(s => s.GlobalUniqueId.Equals(globalUniqueId));
                return rol;
            }
            catch (Exception ex) { throw ex; }

        }
        public void UpdateTransactionLog(TransactionLog tr)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                TransactionLog t = context.TransactionLogs.FirstOrDefault(s => s.Id == tr.Id);
                t.CurrencyCode = tr.CurrencyCode;
                t.DepositorName = tr.DepositorName;
                t.Id = tr.Id;
                //t.payment_route = tr.payment_route;
                t.PaymentChannel = tr.PaymentChannel;
                t.StudentBillId = tr.StudentBillId;
                t.StudentId = tr.StudentId;
                t.TransactionAmount = tr.TransactionAmount;
                t.TransactionDate = tr.TransactionDate;
                t.TransactionRef = tr.TransactionRef;
                t.TransactionStatus = tr.TransactionStatus;
                t.UserId = tr.UserId;
                t.FinalResponseCode = tr.FinalResponseCode;
                t.FinalResponseDescription = tr.FinalResponseDescription;
                t.FinalTransactionDate = tr.FinalTransactionDate;
                t.FinalTransactionStatus = tr.FinalTransactionStatus;
                t.GlobalUniqueId = tr.GlobalUniqueId;
                t.InitialResponseCode = tr.InitialResponseCode;
                t.InitialResponseDescription = tr.InitialResponseDescription;
                t.InitialTransactionDate = tr.InitialTransactionDate;
                t.InitialTransactionStatus = tr.InitialTransactionStatus;
                
                
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }

        }
    }
}
