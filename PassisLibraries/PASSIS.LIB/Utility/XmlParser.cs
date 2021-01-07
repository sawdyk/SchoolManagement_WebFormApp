using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PASSIS.LIB.Utility
{
    public class XmlParser
    {
        public static ServiceChargeResponse getServiceChargeResponse(string xmlString)
        {
            ServiceChargeResponse scr = new ServiceChargeResponse();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNodeList xmlList = doc.SelectNodes("/ServiceChargeResponse/TransactionStatus");
                TransactionStatus tr = new TransactionStatus();
                tr.ErrorCode = xmlList[0]["ErrorCode"].InnerText;
                tr.ErrorDescription = xmlList[0]["ErrorDescription"].InnerText;
                scr.TranStatus = tr;
                XmlNodeList xmlList2 = doc.SelectNodes("/ServiceChargeResponse");
                scr.ServiceCharge = xmlList2[0]["ServiceCharge"].InnerText;
                scr.IsServiceChargeFixed = xmlList2[0]["IsServiceChargeFixed"].InnerText;
                return scr;
            }
            catch (Exception e) { return null; }
        } 
        public static PaymentReferenceResponse getPaymentReferenceResponse(string xmlString)
        {
            PaymentReferenceResponse prr = new PaymentReferenceResponse();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNodeList xmlList = doc.SelectNodes("/PaymentReferenceResponse/TransactionStatus");
                TransactionStatus tr = new TransactionStatus();
                tr.ErrorCode = xmlList[0]["ErrorCode"].InnerText;
                tr.ErrorDescription = xmlList[0]["ErrorDescription"].InnerText;
                prr.TranStatus = tr;
                XmlNodeList xmlList2 = doc.SelectNodes("/PaymentReferenceResponse");
                prr.PaymentReference = xmlList2[0]["PaymentReference"].InnerText; 
                return prr;
            }
            catch (Exception e) { return null; }
        }
        public static  PaymentStatusResponse getPaymentStatusResponse(string xmlString)
        {
            PaymentStatusResponse psr = new PaymentStatusResponse();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNodeList xmlList = doc.SelectNodes("/PaymentStatus/TransactionStatus");
                TransactionStatus tr = new TransactionStatus();
                tr.ErrorCode = xmlList[0]["ErrorCode"].InnerText;
                tr.ErrorDescription = xmlList[0]["ErrorDescription"].InnerText;
                psr.TranStatus = tr;
                try
                {
                    XmlNodeList xmlList2 = doc.SelectNodes("/PaymentStatus");
                    psr.PaymentReference = xmlList2[0]["PaymentReference"].InnerText;
                    psr.Amount = xmlList2[0]["Amount"].InnerText;
                    psr.CustomerFullName = xmlList2[0]["CustomerFullName"].InnerText;
                    psr.HasCustomerPaid = xmlList2[0]["HasCustomerPaid"].InnerText;
                    psr.PaymentDate = xmlList2[0]["PaymentDate"].InnerText;
                    psr.PaymentBranchName = xmlList2[0]["PaymentBranchName"].InnerText;
                    psr.DepositorName = xmlList2[0]["DepositorName"].InnerText;
                    psr.DepositAmount = xmlList2[0]["DepositAmount"].InnerText;
                    psr.BankPaymentReference = xmlList2[0]["BankPaymentReference"].InnerText;
                    psr.PaymentChannel = xmlList2[0]["PaymentChannel"].InnerText;
                }
                catch { }
                return psr;
            }
            catch (Exception e) { return null; }
        }
    }

    public class TransactionStatus
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class PaymentReferenceResponse
    {
        public string PaymentReference { get; set; }
        public TransactionStatus TranStatus { get; set; }

    }
    public class ServiceChargeResponse
    {
        public string ServiceCharge { get; set; }
        public string IsServiceChargeFixed { get; set; }
        public TransactionStatus TranStatus { get; set; }
    } 
    public class PaymentStatusResponse
    { 
        public string PaymentReference { get; set; }
        public string Amount { get; set; }
        public string CustomerFullName { get; set; }
        public string HasCustomerPaid { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentBranchName { get; set; }
        public string DepositorName { get; set; }
        public string DepositAmount { get; set; }
        public string BankPaymentReference { get; set; }
        public string PaymentChannel { get; set; } 
        public TransactionStatus TranStatus { get; set; }
    }
}
