using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;

public partial class SubjectAssigned : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<Subject> subjects = new List<Subject>();
            IList<PASSIS.LIB.SubjectTeacher> test = new TeacherLIB().getTeacherSubjects(logonUser.Id);
            var all = from s in test select new { s.Subject.Name, className = ClassName(s.ClassId), s.Grade.GradeName };
            gdvLists.DataSource = all.ToList();
            gdvLists.DataBind();
        }
    }

    private string ClassName(long? classId)
    {
        Class_Grade classGrade = context.Class_Grades.FirstOrDefault(x => x.Id == classId);
        if (classGrade != null)
        {
            return classGrade.Name;
        }
        else { return string.Empty; }
    }
}