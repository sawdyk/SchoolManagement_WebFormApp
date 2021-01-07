using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class ViewChildDetails : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
          
        }
    }
    protected void BindGrid()
    {
        gdvList.DataSource = new UsersLIB().RetrieveParentsChildren(logonUser.Id);
        gdvList.DataBind();
    }

    public string stdGrade(long studentId)
    {
        PASSIS.LIB.PASSISLIBDataContext context = new PASSISLIBDataContext();
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;
        string gradeNamee = string.Empty;


        //StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(studentId).GradeId;
        //PASSIS.LIB.Grade gradeName = context.Grades.FirstOrDefault(x => x.Id == StudentGradeId);
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent grd = context.GradeStudents.FirstOrDefault(g => g.StudentId == studentId && g.AcademicSessionId == curSessionId);
        if(grd != null)
        {
            PASSIS.LIB.Grade gradeName = context.Grades.FirstOrDefault(x => x.Id == grd.GradeId);
            gradeNamee = string.Format("{0}", gradeName.GradeName.ToString());
            return gradeNamee;
        }
        return gradeNamee;
    }

    public string stdClass(long studentId)
    {
        PASSIS.LIB.PASSISLIBDataContext context = new PASSISLIBDataContext();
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;
        string className = string.Empty;
        
                //StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(studentId).ClassId;
                //PASSIS.LIB.Grade gradeName = context.Grades.FirstOrDefault(x => x.Id == StudentGradeId);
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent grd = context.GradeStudents.FirstOrDefault(g => g.StudentId == studentId && g.AcademicSessionId == curSessionId);
        if (grd != null)
        {
            PASSIS.LIB.Class_Grade clsGrade = context.Class_Grades.FirstOrDefault(x => x.Id == grd.ClassId);
            className = string.Format("{0}", clsGrade.Name.ToString());
            return className;
        }
        return className;
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        gdvList.DataBind();
        BindGrid();
    }
}