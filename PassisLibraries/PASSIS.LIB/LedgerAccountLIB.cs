using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB
{
    public class LedgerAccountLIB
    {
        public void SaveLedgerAccount(LedgerAccount ledgerAccount)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.LedgerAccounts.InsertOnSubmit(ledgerAccount);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public IList<LedgerAccount> getAllLedgerAccounts()
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allLedgerAccounts = from cls in context.LedgerAccounts select cls;
                return allLedgerAccounts.OrderBy(l => l.AccountDescription).ToList<LedgerAccount>();
            }
            catch (Exception ex) { throw ex; }
        }

        
        public LedgerAccount RetrieveLedgerAccount(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                LedgerAccount usr = context.LedgerAccounts.SingleOrDefault(s => s.Id == Id);
                return usr;
            }
            catch (Exception ex) { throw ex; }
        }
         
    }
}
