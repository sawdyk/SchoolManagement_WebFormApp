using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class AdmissionLIB
    {
        public AdmissionPayment RetrievePaymentStatus(string ApplicationId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                AdmissionPayment Payment = context.AdmissionPayments.SingleOrDefault(p => p.ApplicantId == ApplicationId);
                return Payment;
            }
            catch (Exception ex) { throw ex; }

        }

    }
}
