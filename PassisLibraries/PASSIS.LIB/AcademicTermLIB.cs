using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PASSIS.LIB
{
     public class AcademicTermLIB
    {
        public IList<AcademicTerm1> RetrieveAcademicTerm()
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from s in context.AcademicTerm1s select s;
                return all.ToList<AcademicTerm1>();
            }
            catch (Exception ex) { throw ex; }

        }
        public string RetrieveAcademicTermBySession(string SessionName)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var termbysession = from s in context.AcademicSessions
                                where s.AcademicSessionName.SessionName == SessionName
                                select s.AcademicTerm1.AcademicTermName;
            return termbysession.ToString();

        }
        public string GetTermById(long sessionId)
        {
            string termName = "";
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            AcademicTerm1 term = context.AcademicTerm1s.FirstOrDefault(x => x.Id == sessionId);
            if (term != null)
            {
                termName = term.AcademicTermName;
            }
            return termName;
        }
    }
}
