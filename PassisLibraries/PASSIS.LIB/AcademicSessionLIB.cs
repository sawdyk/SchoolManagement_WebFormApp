using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class AcademicSessionLIB
    {

        public void SaveAcademicSession(AcademicSession per)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.AcademicSessions.InsertOnSubmit(per);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public string GetCurrentSession(long? schoolId)
        {
            string ReturnCurrentSession = string.Empty;

            using (PASSISLIBDataContext db = new PASSISLIBDataContext())
            {

                var getCurrentSession = from currentSession in db.AcademicSessions
                                        where currentSession.SchoolId == schoolId
                                        where currentSession.IsCurrent == true
                                        select currentSession.AcademicTerm1.AcademicTermName.ToUpper() + " " + currentSession.AcademicSessionName.SessionName.ToUpper() + " SESSION";

                if (getCurrentSession.Count() > 0)
                {
                    ReturnCurrentSession = getCurrentSession.FirstOrDefault().ToString();
                }
                else
                {
                    ReturnCurrentSession = "";
                }
            }
            return ReturnCurrentSession;
        }
        public long GetCurrentSessionId(long? schoolId)
        {
             long ReturnCurrentSessionId;

            using (PASSISLIBDataContext db = new PASSISLIBDataContext())
            {

                var getCurrentSessionId = from currentSession in db.AcademicSessions
                                        where currentSession.SchoolId == schoolId && currentSession.IsCurrent == true
                                        select currentSession.AcademicSessionId;

                if (getCurrentSessionId.Count() > 0)
                {
                    ReturnCurrentSessionId = getCurrentSessionId.FirstOrDefault();
                }
                else
                {
                    ReturnCurrentSessionId = 0;
                }
            }
            return ReturnCurrentSessionId;
        }
        public long? GetCurrentTermId(long? schoolId)
        {
            long? ReturnCurrentTermId;

            using (PASSISLIBDataContext db = new PASSISLIBDataContext())
            {

                var getCurrentTermId = from currentSession in db.AcademicSessions
                                          where currentSession.SchoolId == schoolId && currentSession.IsCurrent == true
                                          select currentSession.AcademicTermId;

                if (getCurrentTermId.Count() > 0)
                {
                    ReturnCurrentTermId = getCurrentTermId.FirstOrDefault();
                }
                else
                {
                    ReturnCurrentTermId = 0;
                }
            }
            return ReturnCurrentTermId;
        }
        public long? GetGradeTeacherId(long GradeId)
        {
            long? ReturnGradeTeacherId;

            using (PASSISLIBDataContext db = new PASSISLIBDataContext())
            {

                var getGradeTeacherId = from grade in db.Grades
                                        where grade.Id == GradeId
                                        select grade.GradeTeacherId;

                if (getGradeTeacherId.Count() > 0)
                {
                    ReturnGradeTeacherId = getGradeTeacherId.FirstOrDefault();
                }
                else
                {
                    ReturnGradeTeacherId = 0;
                }
            }
            return ReturnGradeTeacherId;
        }
        public IList<SchoolCampus> RetrieveCampusInSchool(Int64 schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from s in context.SchoolCampus where s.SchoolId == schoolId select s;
                //SchoolCampus camp = context.SchoolCampus.SingleOrDefault(s => s.SchoolId == schoolId);
                return all.ToList<SchoolCampus>();
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<SchoolCampus> RetrieveUserCampus(Int64 campusId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from s in context.SchoolCampus where s.Id == campusId select s;
                //SchoolCampus camp = context.SchoolCampus.SingleOrDefault(s => s.SchoolId == schoolId);
                return all.ToList<SchoolCampus>();
            }
            catch (Exception ex) { throw ex; }

        }


        public SchoolCampus RetrieveSchoolCampus(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                SchoolCampus camp = context.SchoolCampus.SingleOrDefault(s => s.Id == Id);
                return camp;
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<AcademicSession> getAllAcademicSessions()
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var allAcademicSessions = context.AcademicSessions;
            return allAcademicSessions.ToList<AcademicSession>();
        }

        
        public AcademicSession RetrieveAcademicSessionById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                AcademicSession per = context.AcademicSessions.SingleOrDefault(s => s.Id == Id);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
        public AcademicSession RetrieveAcademicSession(DateTime currentDateTime)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                AcademicSession per = context.AcademicSessions.SingleOrDefault(s => s.DateStart < currentDateTime && s.DateEnd > currentDateTime);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<AcademicSession> RetrieveAcademicSession(long schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from aca in context.AcademicSessions where aca.SchoolId == schoolId select aca;
                return all.ToList<AcademicSession>();
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<SchoolCampus> GetAllSchoolCampus(long schoolId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from camp in context.SchoolCampus where camp.SchoolId == schoolId select camp;
                return all.ToList<SchoolCampus>();
            }
            catch (Exception ex) { throw ex; }

        }
        public IList<AcademicSessionName> RetrieveAcademicSession()
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from s in context.AcademicSessionNames select s;
                return all.ToList<AcademicSessionName>();
            }
            catch (Exception ex) { throw ex; }

        }
        public string GetCurrentSessionName(long? schoolId)
        {
            string ReturnCurrentSession = string.Empty;

            using (PASSISLIBDataContext db = new PASSISLIBDataContext())
            {

                var getCurrentSession = from currentSession in db.AcademicSessions
                                        where currentSession.SchoolId == schoolId
                                        where currentSession.IsCurrent == true
                                        select currentSession.AcademicSessionName.SessionName.ToUpper();

                if (getCurrentSession.Count() > 0)
                {
                    ReturnCurrentSession = getCurrentSession.FirstOrDefault().ToString();
                }
                else
                {
                    ReturnCurrentSession = "";
                }
            }
            return ReturnCurrentSession;
        }

        public string GetSessionById(long sessionId) 
        {
            string sessionName = "";
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            AcademicSessionName session = context.AcademicSessionNames.FirstOrDefault(x => x.ID == sessionId);
            if (session != null) 
            {
                sessionName = session.SessionName;
            }
            return sessionName;
        }
    }
}
