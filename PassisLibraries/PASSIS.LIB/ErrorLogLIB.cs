using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class ErrorLogLIB
    {
        public void SaveErrorLog(ErrorLog er)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.ErrorLogs.InsertOnSubmit(er);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void LogException(Exception  ex)
        {
            try
            {

                ErrorLog er = new ErrorLog();
                er.ErrorMessage = ex.InnerException == null ? ex.Message : string.Format("{0} ::::::::::::::::{1}", ex.InnerException.Message, ex.Message);
                er.EventDateTime = DateTime.Now;
                er.RequestUrl = "";
                er.StackTrace = ex.InnerException == null ? ex.StackTrace : string.Format("{0}  :::::::::::::::::::::{1}" , ex.InnerException.StackTrace, ex.StackTrace);

                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.ErrorLogs.InsertOnSubmit(er);
                context.SubmitChanges();

            }
            catch (Exception excp)
            {   }
        }

    }
}
