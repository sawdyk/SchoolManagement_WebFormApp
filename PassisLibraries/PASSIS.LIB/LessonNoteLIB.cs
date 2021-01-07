using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASSIS.LIB
{
    public class LessonNoteLIB
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        public void SaveLessonNote(LessonNote ln)
        {
            try
            {

                context.LessonNotes.InsertOnSubmit(ln);
                context.SubmitChanges();


            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DeleteLessonNote(LessonNote ln)
        {
            try
            {

                context.LessonNotes.DeleteOnSubmit(ln);
                context.SubmitChanges();


            }
            catch (Exception ex)
            { throw ex; }
        }

        public LessonNote RetrieveLessonNoteById(Int64 Id)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                LessonNote per = context.LessonNotes.SingleOrDefault(s => s.Id == Id);
                return per;
            }
            catch (Exception ex) { throw ex; }

        }

        public IList<LessonNote> RetrieveLessonNoteByTeacherId(Int64 teacherId)
        {
            var lessonNote = from lNote in context.LessonNotes where lNote.TeacherId == teacherId select lNote;
            return lessonNote.ToList<LessonNote>();
        }

        public IList<LessonNote> RetrieveLessonNote(long schoolId, long campusId, long yearId, long sessionId, long termId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                var all = from lsn in context.LessonNotes where lsn.SchoolId == schoolId select lsn;

                if (campusId > 0)
                    all = all.Where(s => s.CampusId == campusId);
                if (yearId > 0)
                    all = all.Where(s => s.ClassId == yearId);
                if (sessionId > 0)
                    all = all.Where(s => s.SessionId == sessionId);
                if (termId > 0)
                {
                    all = all.Where(s => s.TermId == termId);
                }
                return all.ToList<LessonNote>();
            }
            catch (Exception ex) { throw ex; }

        }

        public IList<LessonNote> RetrieveLessonNoteSupervisor(long schoolId, long campusId, long gradeId, long sessionId, long termId)
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                var all = from lsn in context.LessonNotes where lsn.SchoolId == schoolId select lsn;

                if (campusId > 0)
                    all = all.Where(s => s.CampusId == campusId);
                if (gradeId > 0)
                    all = all.Where(s => s.GradeId == gradeId);
                if (sessionId > 0)
                    all = all.Where(s => s.SessionId == sessionId);
                if (termId > 0)
                {
                    all = all.Where(s => s.TermId == termId);
                }
                return all.ToList<LessonNote>();
            }
            catch (Exception ex) { throw ex; }

        }

    }
}
