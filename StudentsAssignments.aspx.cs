using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.LIB.Utility;

public partial class StudentsAssignments : PASSIS.LIB.Utility.BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            PASSIS.LIB.AssignmentLIB assDal = new PASSIS.LIB.AssignmentLIB();
            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

            Int64 userId = logonUser.Id;
            Int64 StudentGradeId = 0L;

            if (logonUserRole.Id == (long)PASSIS.LIB.Utility.roles.student)
            {

                try
                {
                    StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;

                }
                catch { }
                //BindGrid(assDal.RetrieveTheAssignment(StudentGradeId));
                BindGrid();
            }
            //else { pnlStudentOnly.Visible = false; }



            if (StudentGradeId != 0)
            {
                //BindGrid(new AssignmentDAL().RetrieveAssignment(0, StudentGradeId, 0, string.Empty));
            }
            else
            {

                //lblErrorMsg.Text = "You have not been assigned to a class. Contact the administrator.";
                //return;
            }
        }

    }
    protected string getClassOrGroupName(object gradeId, object groupId)
    {
        if (gradeId == null)
        {
            return new GroupingDAL().RetrieveGrouping(Convert.ToInt64(groupId)).GroupName;
        }
        else
        {
            return new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(gradeId)).GradeName;
        }
    }
    //protected void BindGrid(IList<PASSIS.LIB.Assignment> assignments)
    //{
    //    gdvList.DataSource = assignments;
    //    gdvList.DataBind();
    //}

    public void BindGrid()
    {
        Int64 userId = logonUser.Id;
        Int64 StudentClassId = 0L;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;

        gdvList.DataSource  = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, StudentClassId);
        gdvList.DataBind();
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;

        BindGrid();
    }
}