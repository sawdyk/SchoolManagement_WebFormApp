using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PASSIS.LIB.Utility
{
    public class ConfigHelper
    {
        public static string getInitialResponseCode
        {
            get
            {
                return "99";
            }
        }
        public static string getMACKEY
        {
            get
            {
                return "65A9EA3DBE0D49921C154D928781577D08326997AB2CA0C69C6563D481E4B4AE88C73F41F26FCD5CC3E75527B704AD957888641BF934E498A994D668F1429471";
            }
        }
        public static string getGTPayUrl
        {
            get
            {
                return "https://ibank.gtbank.com/GTPay/Tranx.aspx";
            }
        }
        public static string getGTPayMerchantId
        {
            get
            {
                return "676";
            }
        }
        public static string getInitialResponseDescription
        {
            get
            {
                return "Transaction sent to switch";
            }
        }

        public static string getBankInitialResponseDescription
        {
            get
            {
                return "Transaction awaiting approval";
            }
        }

        public static string getBankFinalResponseDescriptionSuccess
        {
            get
            {
                return "Transaction Approved";
            }
        }

        public static string getBankFinalResponseDescriptionRejected
        {
            get
            {
                return "Transaction Rejected";
            }
        }



        public static string getMerchantProductId
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MerchantProductId"];
                }
                catch { return string.Empty; }
            }
        }
        public static string getMerchantId
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MerchantId"];
                }
                catch { return string.Empty; }
            }
        }
        public static Int64 getGreenspringsSchoolId
        {
            get
            {
                try
                {
                    return Convert.ToInt64(ConfigurationManager.AppSettings["GreenspringsSchoolId"]);
                }
                catch { return 3; }
            }
        }
        public static string getNumberOfDaysBeforePaymentRefExpires
        {
            get
            {
                try
                {
                    Double n = Convert.ToDouble(ConfigurationManager.AppSettings["NumberOfDaysBeforePaymentRefExpires"]);
                    return DateTime.Now.AddDays(n).ToString("yyyy-MM-dd");
                }
                catch { return string.Empty; }
            }
        }
        public static Boolean IsLive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["IsLive"]);
                }
                catch { return false; }
            }
        }
        public static Int32 getDoesReferenceExpire
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Convert.ToBoolean(ConfigurationManager.AppSettings["DoesReferenceExpire"]));
                }
                catch { return 1; }
            }
        }
        public static string getServiceCredentialUsername
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ServiceCredentialUsername"];
                }
                catch { return string.Empty; }
            }
        }
        public static string getServiceCredentialPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ServiceCredentialPassword"];
                }
                catch { return string.Empty; }
            }
        }
        public static decimal getInterswitchPaymentCommissionInPercentage
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(ConfigurationManager.AppSettings["InterswitchPaymentCommissionInPercentage"]);
                }
                catch { return 0m; }
            }
        }
        public static decimal getTelnetCommission
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(ConfigurationManager.AppSettings["getTelnetCommission"]);
                }
                catch { return 0m; }
            }
        }
        public static string getMiddlewareIpAddress
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MiddlewareIpAddress"];
                }
                catch { return string.Empty; }
            }
        }
        public static Int32 getMaximumNumberOfMailRetry
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfMailRetry"]);
                }
                catch { return 10; }
            }
        }
        public static string getMailFrom
        {
            get
            {
                try
                {
                    return  ConfigurationManager.AppSettings["MailFrom"];
                }
                catch { return ""; }
            }
        }
        public static string getMailSmtpHost
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailSmtpHost"];
                }
                catch { return ""; }
            }
        }
        public static string getMailNetworkCredentialUsername
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailNetworkCredentialUsername"];
                }
                catch { return ""; }
            }
        }
        public static string getMailNetworkCredentialPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailNetworkCredentialPassword"];
                }
                catch { return ""; }
            }
        }
        public static string getSmsApiUsername
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["smsApiUsername"];
                }
                catch { return ""; }
            }
        }
        public static string getSmsApiPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["smsApiPassword"];
                }
                catch { return ""; }
            }
        }
        public static string getSmsApiUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["smsApiUrl"];
                }
                catch { return ""; }
            }
        }
        public static string getSmsApiSenderName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["smsApiSenderName"];
                }
                catch { return ""; }
            }
        }
        public static string getMailerHost
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailerHost"];
                }
                catch { return ""; }
            }
        }
        public static Int32 getMailerPort
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["MailerPort"]);
                }
                catch { return 0; }
            }
        }
        public static string getMailerUsername
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailerUsername"];
                }
                catch { return ""; }
            }
        }
        public static string getMailerPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailerPassword"];
                }
                catch { return ""; }
            }
        }
    }
}
