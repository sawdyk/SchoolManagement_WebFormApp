using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class SubjectOrder : PASSIS.LIB.Utility.BasePage
{
    long curriculumId = 0;
    long schoolTypeId = 0;
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //populate year
            curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblSubjects.Visible = true;
        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select year";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
        gvSubject.DataSource = new TeacherLIB().GetAllSubjectInClass((int)curriculumId, Convert.ToInt64(ddlYear.SelectedValue), (long)logonUser.SchoolId);
        gvSubject.DataBind();
        btnSaveOrder.Visible = true;
    }
    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select year";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        Int64 yearId = 0L, schoolId = 0L;
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        schoolId = (long)logonUser.SchoolId;

        foreach (GridViewRow row in gvSubject.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lblId = (Label)gvSubject.Rows[row.RowIndex].FindControl("lblId");
                TextBox txtPosition = (TextBox)gvSubject.Rows[row.RowIndex].FindControl("txtPosition");
                long subjectId = Convert.ToInt64(lblId.Text.Trim());
                if (txtPosition.Text != "")
                {
                    SubjectsInSchool subInSchool = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subjectId && x.SchoolId == logonUser.SchoolId);
                    subInSchool.ReportCardOrder = Convert.ToInt16(txtPosition.Text.ToString());
                    context.SubmitChanges();
                }
            }
        }
        lblErrorMsg.Text = "Saved Successfully";
        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        lblErrorMsg.Visible = true;
    }
}