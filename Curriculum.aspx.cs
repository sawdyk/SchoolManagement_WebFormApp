using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Data.SqlClient;

public partial class Curriculum : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ddlTheme.SelectedValue != "0") tbdCurriculum.InnerHtml = @"
                                            <tr>
                                                <td id='tdSubject' runat='server'></td>
                                                <td id='tdTheme' runat='server'></td>
                                                <td id='tdThemeTopic' runat='server'></td>
                                            </tr>";
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlClass.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("---Select---", "0"));
        }

    }
    //protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlSubject.Items.Clear();
    //    PASSISLIBDataContext context = new PASSISLIBDataContext();
    //    IList<SubjectsInSchool> getId = new SubjectTeachersLIB().getAllSubjectsInSchool((long)logonUser.SchoolId, Convert.ToInt64(ddlClass.SelectedValue));
    //    foreach (SubjectsInSchool subjId in getId)
    //    {
    //        PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
    //        ddlSubject.Items.Add(new System.Web.UI.WebControls.ListItem(reqSubject.Name, reqSubject.Id.ToString()));
    //    }
    //}

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //ddlTheme.Items.Clear();
        //loads
        if (!IsPostBack)
        {
            //this.ddlCurriculum_SelectedIndexChanged(sender, e);

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));

            //dsClass.SelectCommand = "SELECT * FROM Class_Grade WHERE CurriculumId=" + curriculumId+"AND SchoolTypeId=" +schoolTypeId;
            //dsSubjectTheme.SelectCommand = "SELECT * FROM Subject_Theme WHERE SubjectId=" + subjectId + " AND ClassId=" + classId + " ORDER BY Title ASC";
            //ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
            ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
            ddlTheme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.searchTitle.InnerText = "";
        //Subject
        this.tdSubject.ColSpan = 1;
        this.tdSubject.InnerHtml = "";
        this.tdSubject.Style.Value = "";
        //Subject Ends
        this.tdTheme.InnerHtml = "";
        this.tdThemeTopic.InnerHtml = "";
        if (ddlClass.SelectedValue != "" && ddlSubject.SelectedValue != "" && ddlTheme.SelectedValue != "")
        {
            this.searchTitle.InnerText = ddlClass.SelectedItem.Text;
            this.tdSubject.InnerText = ddlSubject.SelectedItem.Text;

            clsMyDB db = new clsMyDB();
            db.connct();
            //fetch Subject Theme
            if (ddlTheme.SelectedValue == "0")
            {
                int x = 0, y = 0;
                //Select Theme Topics
                int.TryParse(ddlClass.SelectedValue.ToString(), out x);
                int.TryParse(ddlSubject.SelectedValue.ToString(), out y);
                string query = "SELECT * FROM Subject_Theme WHERE SubjectId=" + y + " AND ClassId=" + x;// +" ORDER BY Title ASC";
                SqlDataReader subjectThemeReader = db.fetch(query);

                List<string[]> themes = new List<string[]>();
                while (subjectThemeReader.Read())
                {
                    string[] values = { subjectThemeReader[0].ToString(), subjectThemeReader[1].ToString(), subjectThemeReader[2].ToString(), subjectThemeReader[3].ToString(), subjectThemeReader[4].ToString(), subjectThemeReader[5].ToString() }; //redefine dis when necessary
                    //subjectThemeReader.GetValues(values);
                    themes.Add(values);
                }
                subjectThemeReader.Close();

                string rows = "<tr> <td style='font-weight:Bold'>   " + ddlSubject.SelectedItem.Text + "</td><td></td><td></td> </tr>";
                for (int i = 0; i < themes.Count; i++)
                {
                    string row = "<tr> <td></td><td>" + themes[i][2] + "</td>";
                    //fetch Subject Theme Topics
                    query = "SELECT * FROM SubjectThemeTopic WHERE ThemeID=" + themes[i][0];
                    SqlDataReader subjectThemeTopicReader = db.fetch(query);
                    string subjectThemeTopic = "<ul>";
                    while (subjectThemeTopicReader.Read())
                    {
                        subjectThemeTopic += "<li>" + subjectThemeTopicReader[1] + "</li>";
                    }
                    subjectThemeTopicReader.Close();
                    subjectThemeTopic += "</ul>";
                    row += "<td>" + subjectThemeTopic + "</td>";

                    row += " </tr>";
                    rows += row;
                }

                tbdCurriculum.InnerHtml = rows;
            }
            else
            {
                int x = 0, y = 0;
                //Select Theme Topics
                int.TryParse(ddlClass.SelectedValue.ToString(), out x);
                int.TryParse(ddlSubject.SelectedValue.ToString(), out y);
                string query = "SELECT * FROM Subject_Theme WHERE SubjectId=" + y + " AND ClassId=" + x;// +" ORDER BY Title ASC";
                SqlDataReader subjectThemeReader = db.fetch(query);
                string rows = "<tr> <td id='tdSubject' runat='server' style='font-weight:Bold'>   " + ddlSubject.SelectedItem.Text + "</td>";
                string subjectTheme = "<td id='tdTheme' runat='server'><ul>";
                while (subjectThemeReader.Read())
                {
                    if (ddlTheme.SelectedValue == subjectThemeReader[0].ToString())
                    //{
                    //    subjectTheme += "<li style='padding:3px; font-weight: bold; background:#ccc;'>" + subjectThemeReader[2] + "</li>";
                    //}
                    //else
                    {
                        subjectTheme += "<li style='padding:3px; '>" + subjectThemeReader[2] + "</li>";
                    }
                }
                subjectThemeReader.Close();
                subjectTheme += "</ul></td>";
                rows += subjectTheme;
                //fetch Subject Theme Topics
                query = "SELECT * FROM SubjectThemeTopic WHERE ThemeID=" + ddlTheme.SelectedValue;
                SqlDataReader subjectThemeTopicReader = db.fetch(query);
                string subjectThemeTopic = "<td id='tdThemeTopic' runat='server'><ul>";
                while (subjectThemeTopicReader.Read())
                {
                    subjectThemeTopic += "<li>" + subjectThemeTopicReader[1] + "</li>";
                }
                subjectThemeTopicReader.Close();
                subjectThemeTopic += "</ul></td>";
                rows += subjectThemeTopic;
                rows += " </tr>";
                tbdCurriculum.InnerHtml = rows;
            }
            db.closeConnct();
        }
        else
        {
            this.tdSubject.ColSpan = 3;
            this.tdSubject.Style.Value = "color: #004;";
            this.tdSubject.InnerText = "No Record Found For the parameters submitted.";
        }
        //this.tdSubject.Focus();
    }


    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubject.Items.Clear();
        ddlTheme.Items.Clear();
        clsMyDB db = new clsMyDB();
        db.connct();
        long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
        //Select Class
        int classId = 0;
        int schoolType = 0;
        int.TryParse(ddlClass.SelectedValue, out classId);
        string query = "SELECT SchoolTypeID FROM Class_Grade WHERE id=" + classId;
        SqlDataReader reader = db.fetch(query);
        while (reader.Read())
        {
            int.TryParse(reader[0].ToString(), out schoolType);
        }
        reader.Close();

        switch (curriculumId)
        {
            case 1: dsSubjects.SelectCommand = "SELECT * FROM Subject WHERE CurriculumId=" + curriculumId + " AND ClassId=" + ddlClass.SelectedValue + " AND SchoolTypeId=0 ORDER BY Name ASC"; break;
            case 2: dsSubjects.SelectCommand = "SELECT * FROM Subject WHERE CurriculumId=" + curriculumId + " AND ClassId=" + ddlClass.SelectedValue + " AND SchoolTypeId=" + schoolType + " OR SchoolTypeId=3 ORDER BY Name ASC"; break;
            default: break;
        }

        int subjectId = 0;
        query = "SELECT TOP 1 Id FROM Subject WHERE CurriculumId=" + curriculumId + " AND SchoolTypeId=" + schoolType + " ORDER BY Name ASC";
        reader = db.fetch(query);
        while (reader.Read())
        {
            int.TryParse(reader[0].ToString(), out subjectId);
        }
        reader.Close();
        db.closeConnct();
        //int x = 0;
        //int subjectId = 0;
        //int.TryParse(ddlClass.SelectedValue, out x);
        //int.TryParse(ddlSubject.SelectedValue, out subjectId);
        dsSubjectTheme.SelectCommand = "SELECT * FROM Subject_Theme WHERE SubjectId=" + subjectId + " AND ClassId=" + classId + " ORDER BY Title ASC";
        ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
        ddlTheme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlTheme.Items.Clear();
        //try
        //{
        //ddlTheme.Items.RemoveAt(0);

        int classId = 0, subjectId = 0;
        //Select Theme Topics
        int.TryParse(ddlClass.SelectedValue.ToString(), out classId);
        int.TryParse(ddlSubject.SelectedValue.ToString(), out subjectId);
        dsSubjectTheme.SelectCommand = "SELECT * FROM Subject_Theme WHERE SubjectId=" + subjectId + " AND ClassId=" + classId + " ORDER BY Title ASC";

        ddlTheme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
        //}
        //catch
        //{
        //    int classId = 0, subjectId = 0;
        //    //Select Theme Topics
        //    int.TryParse(ddlClass.SelectedValue.ToString(), out classId);
        //    int.TryParse(ddlSubject.SelectedValue.ToString(), out subjectId);
        //    dsSubjectTheme.SelectCommand = "SELECT * FROM Subject_Theme WHERE SubjectId=" + subjectId + " AND ClassId=" + classId + " ORDER BY Title ASC";

        //    //ddlTheme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select All--", "0", true));
        //}
    }

}