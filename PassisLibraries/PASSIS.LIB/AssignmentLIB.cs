using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class AssignmentLIB
    {
        public void SaveAssignments(Assignment ass)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                context.Assignments.InsertOnSubmit(ass);
                context.SubmitChanges();

               
            }
            catch (Exception ex)
            { throw ex; }
        }

        public Assignment RetrieveAssignmentById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                Assignment per = context.Assignments.SingleOrDefault(s => s.Id == Id);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }

        public IList<Assignment> RetrieveAssignment(long teacherId, long gradeId, long subjectId, string title)
        {
            return RetrieveAssignment(teacherId, gradeId, subjectId, title, 0);
        }

        public IList<Assignment> RetrieveAssignment(long teacherId, long gradeId, long subjectId, string title, Int64 studentId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from ass in context.Assignments select ass;
                if (teacherId > 0)
                    all = all.Where(s => s.TeacherId == teacherId);
                if (!string.IsNullOrEmpty(title))
                    all = all.Where(f => f.Description.Contains(title));
                if (gradeId >= 0)
                    all = all.Where(s => s.GradeId == gradeId);
                if (subjectId > 0)
                    all = all.Where(s => s.SubjectId == subjectId);
                if (studentId > 0)//logic  land mine(??) not yet fully  checked Meanwhile , RetrieveAssignment(long gradeId, Int64 studentId) can be used instead
                {
                    Int64[] groupIds = RetrieveStudentGroupsById(studentId);
                    all = all.Where(g => groupIds.Contains((long)g.GroupId));
                }
                return all.ToList<Assignment>();
            }
            catch (Exception ex) { throw ex; }

        }

        public Int64[] RetrieveStudentGroupsById(Int64 studentId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var allStudentGroupIds = from gs in context.GroupStudents where gs.StudentId == studentId select gs.GroupingId;

                return allStudentGroupIds.ToArray<Int64>();
            }
            catch (Exception e) { throw e; }
        }

        public IList<Assignment> RetrieveAssignment(long gradeId, Int64 studentId)
        {
            try
            {
               PASSISLIBDataContext context = new PASSISLIBDataContext();
                Int64[] groupIds = RetrieveStudentGroupsById(studentId);
                var all = (from ass in context.Assignments where groupIds.Contains((long)ass.GroupId) || ass.GradeId == gradeId && ass.SubjectId == ass.SubjectId select ass).Distinct();
                return all.ToList<Assignment>();
            }
            catch (Exception ex) { throw ex; }
        }

        public IList<Assignment> RetrieveTheAssignment(long gradeId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = (from ass in context.Assignments where ass.GradeId == gradeId select ass).Distinct();
                return all.ToList<Assignment>();
            }
            catch (Exception ex) { throw ex; }
        }

        public IList<Assignment> RetrieveAssignmentDetails(Int64 subjectId, long studentGradeId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from ass in context.Assignments
                          where ass.SubjectId == subjectId && ass.GradeId == studentGradeId
                          select ass;
                return all.ToList<Assignment>();
            }
            catch (Exception ex) { throw ex; }
        }

        public IList<AssignmentsGraded> RetrieveGradedAssignment(long studentId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                var all = from ass in context.AssignmentsGradeds where ass.AssignmentSubmitted.StudentId == studentId select ass;

                return all.ToList<AssignmentsGraded>();
            }
            catch (Exception ex) { throw ex; }

        }

        public IList<AssignmentSubmitted> getSubmittedAssignment(long teacherUserId)
        {
            try
            {
                PASSISLIBDataContext ct = new PASSISLIBDataContext();
                
                var result = from s in ct.AssignmentSubmitteds where s.Assignment.TeacherId == teacherUserId select s;
                return result.ToList<AssignmentSubmitted>();
            }
            catch (Exception ex) { throw ex; }
        }
        public GradeStudent RetrieveStudentGrade(Int64 studentId, Int64 sessionId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            GradeStudent grd = null;
            grd = context.GradeStudents.FirstOrDefault(g => g.StudentId == studentId && g.AcademicSessionId == sessionId);
            return grd;
        }

        public Subject RetrieveSubjectInSchoolById(Int64 Id)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Subject subj = context.Subjects.SingleOrDefault(s => s.Id == Id);
            return subj;

        } 

    }
}
