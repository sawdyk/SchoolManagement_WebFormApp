namespace PASSIS.LIB.CustomClasses
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class gtpayQueryStringProperties
    {
        public string gtpay_cust_id { get; set; }

        public string gtpay_tranx_amt { get; set; }

        public string gtpay_tranx_id { get; set; }

        public string gtpay_tranx_status_code { get; set; }

        public string gtpay_tranx_status_msg { get; set; }
    }
}

