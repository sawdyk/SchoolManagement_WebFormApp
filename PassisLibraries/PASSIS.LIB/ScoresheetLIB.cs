using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
 
    public class ScoresheetLIB
    {
        public void SaveScoreSheet(ScoreSheetRepo score)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.ScoreSheetRepos.InsertOnSubmit(score);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IList<ScoreSheetRepo> RetrieveTeacherScoreSheets(Int64 teacherId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var results = from s in context.ScoreSheetRepos where s.SubmittedBy == teacherId select s;
            return results.ToList<ScoreSheetRepo>();
        }
        public void SaveStudentScoreDetail(StudentScore score)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.StudentScores.InsertOnSubmit(score);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveStudentScoreDetailCA(StudentScoreCA score)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.StudentScoreCAs.InsertOnSubmit(score);
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveStudentScoreDetailCA(IList<StudentScoreCA> scores)
        {
            try
            {
                foreach (StudentScoreCA sc in scores)
                {
                    SaveStudentScoreDetailCA(sc);
                }

            }
            catch (Exception ex)
            { }
        }
        public void SaveStudentScoreDetail(IList<StudentScore> scores)
        {
            try
            {
                foreach (StudentScore sc in scores)
                {
                    SaveStudentScoreDetail(sc);
                }

            }
            catch (Exception ex)
            { }
        }
        public void SaveStudentTestAssignmentScore(IList<StudentScoreRepository> scores)
        {
            try
            {
                foreach (StudentScoreRepository score in scores)
                {
                    PASSISLIBDataContext context = new PASSISLIBDataContext();
                    context.StudentScoreRepositories.InsertOnSubmit(score);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void SaveStudentScoreCatgory(IList<StudentScoreCategory> scores)
        {
            try
            {
                foreach (StudentScoreCategory score in scores)
                {
                    PASSISLIBDataContext context = new PASSISLIBDataContext();
                    context.StudentScoreCategories.InsertOnSubmit(score);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public StudentScore RetrieveStudentScore(Int64 subjectId, string admNumber)
        {
            //PASSISLIBDataContext context = new PASSISLIBDataContext();
            StudentScore stc = null;
            //stc = context.StudentScores.FirstOrDefault(g => g.ScoreSheetRepo.Grouping.SubjectId == subjectId && g.AdmissionNumber.Equals(admNumber));
            return stc;
        }
        public IList<StudentScore> RetrieveStudentScores(Int64 TermId, Int64 AcademicSessionId, string admNumber, Int64 GradeId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var results = from s in context.StudentScores where s.TermId == TermId && s.AcademicSessionID == AcademicSessionId && s.AdmissionNumber == admNumber select s;
            return results.ToList<StudentScore>();
        }
        /// <summary>
        /// This was used for bulk upload  test and should not be used for LIVE becuse it assumed only one file was downloaded adn did not get the subjectid Id from the scoresheetrepo table
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="admNumber"></param>
        /// <returns></returns>
        /// 
        public IList<StudentScoreRepository> getScorelist(Int64 SessionId, Int64 TermId, Int64 SubjectId, string AdmissionNo, Int64 CampusId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var scoreList = from score in context.StudentScoreRepositories
                            where score.SessionId == SessionId && score.TermId == TermId && score.SubjectId == SubjectId && score.AdmissionNo == AdmissionNo && score.CampusId == CampusId
                            select score;
            return scoreList.ToList<StudentScoreRepository>();
        }

        public StudentScoreRepository getTestScore(Int64 SessionId, Int64 TermId, Int64 SubjectId, string AdmissionNo, Int64 CampusId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var scoreList = context.StudentScoreRepositories.FirstOrDefault(score=>score.SessionId == SessionId && score.TermId == TermId &&
                score.SubjectId == SubjectId && score.AdmissionNo == AdmissionNo && score.CampusId == CampusId && score.TemplateId !=null);
            return scoreList;
        }

        public ScoreConfiguration getScoreConfiguration(Int64 schoolId, Int64 campusId, Int64 classId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            ScoreConfiguration scoreConfig = context.ScoreConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId);
            return scoreConfig;
        }


        public StudentScore RetrieveStudentScoreBulkUploadTest(Int64 subjectId, string admNumber, Int64 academicId, Int64 termId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            StudentScore stc = null;
            stc = context.StudentScores.FirstOrDefault(g => g.SubjectId == subjectId && g.AdmissionNumber.Equals(admNumber) &&
                g.AcademicSessionID == academicId && g.TermId == termId && g.TemplateId != null);
            return stc;
        }
        public IList<ScoreSheetRepo> RetrieveScoreSheets(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var results = from s in context.ScoreSheetRepos where s.Id == Id select s;
            return results.ToList<ScoreSheetRepo>();
        }
        public IList<ScoreSheetRepo> RetrieveScoreSheets(IList<Int64> IdList)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var results = from s in context.ScoreSheetRepos where IdList.ToArray().Contains(s.Id) select s;
            return results.ToList<ScoreSheetRepo>();
        }
        //public IList<ScoreSheetRepo> RetrieveScoreSheetsByGradeId(Int64 gradeId)
        //{
        //    PASSISDataContext context = new PASSISDataContext();
        //    var results = from s in context.ScoreSheetRepos where s.GradeId == gradeId select s;
        //    return results.ToList<ScoreSheetRepo>();
        //}
        public void UpdateScoreSheetRepo(ScoreSheetRepo ssr)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                ScoreSheetRepo s = context.ScoreSheetRepos.FirstOrDefault(f => f.Id == ssr.Id);
                s.SubmissionStatus = ssr.SubmissionStatus;
                s.ProcessedBy = ssr.ProcessedBy;
                context.SubmitChanges();

            }
            catch (Exception ex)
            { throw ex; }

        }
        public IList<ScoreSheetRepo> RetrieveScoreSheets(Int64 campusId, Int64 yearId, Int64 gradeId, Int64 subjectId)
        {
            return RetrieveScoreSheets(campusId, yearId, gradeId, subjectId, 0);
        }
        public IList<ScoreSheetRepo> RetrieveScoreSheets(Int64 campusId, Int64 yearId, Int64 gradeId, Int64 subjectId, Int64 groupId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allSheets = from scr in context.ScoreSheetRepos select scr;
                //if (campusId > 0)
                //{
                //    allSheets = allSheets.Where(c => c.Grouping.CampusId == campusId);
                //}
                if (gradeId > 0)
                    allSheets = allSheets.Where(s =>s.GradeId == gradeId);
                //if (yearId > 0)
                //    allSheets = allSheets.Where(s => s.Grouping.yearId == yearId);
                //if (subjectId > 0)
                //    allSheets = allSheets.Where(c => c.Grouping.SubjectId == subjectId);
                var result = from res in allSheets select res;

                return result.ToList<ScoreSheetRepo>();
            }
            catch (Exception e) { throw e; }
        }
        public IList<Subject> ReportCardSubject(long sessionId, long termId, string admNo){
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var reportSubject = from s in context.StudentScores where s.AcademicSessionID == sessionId &&
                                               s.TermId == termId &&
                                               s.AdmissionNumber == admNo select s.Subject;
                                               return reportSubject.ToList<Subject>();
        }

        public IList<StudentScore> ReportCard_SubjectScore(string admissionNumber)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var results = from s in context.StudentScores where s.AdmissionNumber.Equals(admissionNumber) select s;
            return results.ToList<StudentScore>();
        }
        public TestAssignmentScoreTemplate GetTemplateList(Int64 ClassId, Int64 SubjectId, string templateCode)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getList = context.TestAssignmentScoreTemplates.FirstOrDefault(l => l.ClassId == ClassId && l.SubjectId == SubjectId && l.TemplateCode == templateCode);
            return getList;
        }

        public TestAssigenmentBroadSheetTemplate GetTemplateList(Int64 ClassId,  Int64 DescriptionId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            var getList = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(l => l.ClassId == ClassId && l.DescriptionId == DescriptionId);
            return getList;
        }
        public void SaveTestAssignmentTemplate(TestAssignmentScoreTemplate Temp)
        {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.TestAssignmentScoreTemplates.InsertOnSubmit(Temp);
                context.SubmitChanges();           
        }

        public void SaveTestAssignmentTemplateBroad(TestAssigenmentBroadSheetTemplate Temp)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            context.TestAssigenmentBroadSheetTemplates.InsertOnSubmit(Temp);
            context.SubmitChanges();
        }

    }
}
