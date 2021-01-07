using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PASSIS.LIB.Utility;
using System.Net.Mail;
using System.Net;

namespace PASSIS.LIB
{
    public class PendingEmailLIB
    {
        public void SavePendingEmail(PendingEmail pendingEmail)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.PendingEmails.InsertOnSubmit(pendingEmail);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public PendingEmail CreateMailObject(string subject, string body, string To, string from, string sender, bool isBodyHtml, string bcc, string cc)
        {
            PendingEmail pd = new PendingEmail();
            pd.Subject = subject;
            pd.Bcc = bcc;
            pd.Body = body;
            pd.CC = cc;
            pd.DueDate = DateTime.Now;
            pd.ErrorMessage = string.Empty;
            pd.From = from;
            pd.IsBodyHtml = isBodyHtml;
            pd.Retries = ConfigHelper.getMaximumNumberOfMailRetry;
            pd.RetryCount = 0;
            pd.Sender = sender;
            pd.To = To;
            return pd;
        }
        protected void sendingMailDirectly(string subject, string body, string receiverMailAddress)
        {
            try
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.From = new MailAddress(ConfigHelper.getMailFrom);
                msg.To.Add(new MailAddress(receiverMailAddress));
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(ConfigHelper.getMailNetworkCredentialUsername, ConfigHelper.getMailNetworkCredentialPassword);

                client.Host = ConfigHelper.getMailSmtpHost;
                client.EnableSsl = false;

                client.Send(msg);

            }
            catch (Exception ex) { new ErrorLogLIB().LogException(ex); }

        }
        //public IList<PendingEmail> getAllPendingEmails(Int64 schoolId)
        //{
        //    try
        //    {
        //        PASSISDataContext context = new PASSISDataContext();
        //        var allPendingEmails = from cls in context.PendingEmails where (long)cls.SchoolId == schoolId select cls;
        //        return allPendingEmails.ToList<PendingEmail>();
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        public PendingEmail RetrievePendingEmail(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                PendingEmail usr = context.PendingEmails.SingleOrDefault(s => s.ID == Id);
                return usr;
            }
            catch (Exception ex) { throw ex; }
        }

        public User getEmail(int UserId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            User getUserDetails = context.Users.FirstOrDefault(x => x.Id == UserId);
            return getUserDetails;
        }


        public void SendMail(string email, string message, User objUser)
        {
            using (PASSISLIBDataContext db = new PASSISLIBDataContext())
            {
                MailMessaging objNewMessage = new MailMessaging();
                objNewMessage.SchoolID = objUser.SchoolId;
                objNewMessage.RecipientUserID = objUser.Id;
                objNewMessage.MailRecipient = email;
                objNewMessage.MailSubject = "School Setup Notification";
                objNewMessage.Body = message.ToString();
                objNewMessage.SendingStatus = 0;
                objNewMessage.submitteddate = DateTime.Now;
                db.MailMessagings.InsertOnSubmit(objNewMessage);
                db.SubmitChanges();
            }
        }
        public void SendMail(string email, string message, string subject)
        {
            MailAddress fromuser = new MailAddress("info@passis.com.ng", "Passis");
            MailAddress to = new MailAddress(email);
            MailMessage mail_message = new MailMessage(fromuser, to);
            MailAddress bbccopy = new MailAddress("ope4sure1@gmail.com");
            mail_message.Subject = subject;
            mail_message.IsBodyHtml = true;
            mail_message.Body = message;
            mail_message.Bcc.Add(bbccopy);

            SmtpClient client = new SmtpClient("74.86.97.86",26);
            client.Credentials = new NetworkCredential()
            {
                UserName = "info@passis.com.ng",
                Password = "Passisinfo123"
            };

            //client.EnableSsl = true;
            client.Send(mail_message);
        }




    }
}
