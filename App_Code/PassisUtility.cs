using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PASSIS.DAO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PASSIS.LIB;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for PassisUtility
/// </summary>
public class PassisUtility
{
    public static string GetConnectionString()
    {
        string _connString = ConfigurationManager.ConnectionStrings["PASSISConnectionString"].ConnectionString;
        return _connString;
    }

    public static void LogErrorMessage(string url, string message, string stackTrace)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.ErrorLog logNewError = new PASSIS.LIB.ErrorLog();
        logNewError.RequestUrl = url;
        logNewError.EventDateTime = DateTime.Now;
        logNewError.ErrorMessage = message;
        logNewError.StackTrace = stackTrace;
        context.ErrorLogs.InsertOnSubmit(logNewError);
        context.SubmitChanges();
    }

}