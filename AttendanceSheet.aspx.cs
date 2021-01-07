using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.IO;
using PASSIS.LIB;

public partial class AttendanceSheet : PASSIS.LIB.Utility.BasePage
{
    const int maximumAttendanceForaDay = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            lblErrorMsg.Text = " ";
        }
        if (!IsPostBack)
        {
            if (!isUserClassTeacher)
            {
                lblErrorMsg.Text = "You are not eligible to take attendance because you are not a class teacher";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {
                //BindGrid();
                pnlMarkAttendance.Visible = true;
                ddlAttendancePeriod.DataSource = new AttendanceLIB().RetrieveAttendancePeriod();
                ddlAttendancePeriod.DataTextField = "AttendancePeriodName";
                ddlAttendancePeriod.DataValueField = "ID";
                ddlAttendancePeriod.DataBind();
                ddlAttendancePeriod.Items.Insert(0, new ListItem("--Select--", "0", true));


            }
        }
    }

    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        if (ddlAttendancePeriod.SelectedValue == "0")
        {
            gdvList2.DataSource = null;
            gdvList2.DataBind();
            gdvList.DataSource = null;
            gdvList.DataBind();
            btnSaveAttendance.Visible = false;
        }


        if (ddlAttendancePeriod.SelectedValue == "1")
        {

            gdvList.DataSource = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
            gdvList.DataBind();
            gdvList2.DataSource = null;
            gdvList2.DataBind();
            btnSaveAttendance.Visible = true;
            lblPeriod.Text = "MORNING ATTENDANCE SHEET";
            lblPeriod.ForeColor = System.Drawing.Color.Green;
            lblPeriod.Visible = true;

        }

        if (ddlAttendancePeriod.SelectedValue == "2")
        {

            gdvList.DataSource = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
            gdvList.DataBind();
            gdvList2.DataSource = null;
            gdvList2.DataBind();
            btnSaveAttendance.Visible = true;
            lblPeriod.Text = "AFTERNOON ATTENDANCE SHEET";
            lblPeriod.ForeColor = System.Drawing.Color.Green;
            lblPeriod.Visible = true;
        }

        if (ddlAttendancePeriod.SelectedValue == "3")
        {

            gdvList2.DataSource = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
            gdvList2.DataBind();
            gdvList.DataSource = null;
            gdvList.DataBind();
            btnSaveAttendance.Visible = true;
            lblPeriod.Text = "MORNING AND AFTERNOON ATTENDANCE SHEET";
            lblPeriod.ForeColor = System.Drawing.Color.Green;
            lblPeriod.Visible = true;
        }
    }



    protected void BindGrid()
    {
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        gdvList.DataSource = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
        gdvList.DataBind();
    }

    protected void BindGrid2()
    {
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        gdvList2.DataSource = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
        gdvList2.DataBind();
    }


    protected void btnSaveAttendance_Click(object sender, EventArgs e)
    {
        try
        {
            long currentTermId = (long)new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
            long currentSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

            if (currentTermId == 0 || currentSessionId == 0)
            {
                lblErrorMsg.Text = "Kindly inform the school admin to set current term and current session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {
                if (ddlAttendancePeriod.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select the attendance period";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }


                if (txtdate.Text == "")
                {
                    lblErrorMsg.Text = "Kindly select the date";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                // No more using line 81-94

                int totalStdInClass = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId,currentSessionId).ToList().Count;
                int maxAttendanceCount = totalStdInClass * maximumAttendanceForaDay;

                PASSISLIBDataContext context = new PASSISLIBDataContext();
                PASSIS.LIB.GradeStudent grd = new ClassGradeLIB().RetrieveTeacherGrade(Convert.ToInt64(logonUser.Id));
                IList<AttendanceRegister> attendanceList = (from attendance in context.AttendanceRegisters
                                                            where attendance.CampusId == logonUser.SchoolCampusId && attendance.GradeId == grd.GradeId && attendance.AttendanceDate == Convert.ToDateTime(txtdate.Text)
                                                            select attendance).ToList();
                if (attendanceList.ToList().Count > maxAttendanceCount)
                {
                    lblErrorMsg.Text = "Maximum attendance has been taken for today";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }
                else   //---------------------------------------Morning Attendance--------------------------------------------//
                {
                    if (ddlAttendancePeriod.SelectedValue == "1")
                    {
                        foreach (GridViewRow row in gdvList.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                //var confirm = attendanceList.Where(x => x.AdmissionNo == row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text && x.AttendancePeriodIDMorning == Convert.ToInt16(ddlAttendancePeriod.SelectedValue));
                                PASSIS.LIB.AttendanceRegister attObj = context.AttendanceRegisters.FirstOrDefault(x => x.AdmissionNo == row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text && x.AttendanceDate == Convert.ToDateTime(txtdate.Text));

                                if (attObj == null)
                                {
                                    bool isChecked = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                    DateTime attendanceDate = DateTime.ParseExact(this.txtdate.Text, "yyyy-MM-dd", null);

                                    if (isChecked)
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 1;
                                        newAttendanceObj.AttendancePeriodIDMorning = 1;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    }
                                    else
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 0;
                                        newAttendanceObj.AttendancePeriodIDMorning = 0;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                    }
                                    lblErrorMsg.Text = "Attendance taken successfully";
                                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                    lblErrorMsg.Visible = true;
                                }
                                else
                                {
                                    if (attObj.AttendancePeriodIDMorning != null && attObj.AttendancePeriodIDAfternoon != null)
                                    {
                                        lblErrorMsg.Text = "Maximum attendance has been taken for today";
                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                        lblErrorMsg.Visible = true;
                                    }
                                    else
                                    {
                                        bool isChecked = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                        DateTime attendanceDate = DateTime.ParseExact(this.txtdate.Text, "yyyy-MM-dd", null);

                                        if (isChecked)
                                        {

                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 1;
                                            attObj.AttendancePeriodIDMorning = 1;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            //new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                            context.SubmitChanges();
                                            row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                        }
                                        else
                                        {
                                            //AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 0;
                                            attObj.AttendancePeriodIDMorning = 0;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            context.SubmitChanges();
                                            //new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        }
                                        lblErrorMsg.Text = "Attendance taken successfully";
                                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                        lblErrorMsg.Visible = true;
                                    }
                                }
                            }

                            //lblErrorMsg.Text = "Attendance taken successfully";
                            //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            //lblErrorMsg.Visible = true;

                        }
                    }

                    //-------------------------------------------------Afternoon Attendance--------------------------------------------------------------//

                    else if (ddlAttendancePeriod.SelectedValue == "2")
                    {
                        foreach (GridViewRow row in gdvList.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                //var confirm = attendanceList.Where(x => x.AdmissionNo == row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text && x.AttendancePeriodIDAfternoon == Convert.ToInt16(ddlAttendancePeriod.SelectedValue));
                                PASSIS.LIB.AttendanceRegister attObj = context.AttendanceRegisters.FirstOrDefault(x => x.AdmissionNo == row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text && x.AttendanceDate == Convert.ToDateTime(txtdate.Text));
                                if (attObj == null)
                                {
                                    bool isChecked = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                    DateTime attendanceDate = DateTime.ParseExact(this.txtdate.Text, "yyyy-MM-dd", null);

                                    if (isChecked)
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 1;
                                        newAttendanceObj.AttendancePeriodIDAfternoon = 1;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    }
                                    else
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 0;
                                        newAttendanceObj.AttendancePeriodIDAfternoon = 0;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                    }
                                    lblErrorMsg.Text = "Attendance taken successfully";
                                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                    lblErrorMsg.Visible = true;
                                }
                                else
                                {
                                    if (attObj.AttendancePeriodIDMorning != null && attObj.AttendancePeriodIDAfternoon != null)
                                    {
                                        lblErrorMsg.Text = "Maximum attendance has been taken for today";
                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                        lblErrorMsg.Visible = true;
                                    }
                                    else
                                    {
                                        bool isChecked = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                        DateTime attendanceDate = DateTime.ParseExact(this.txtdate.Text, "yyyy-MM-dd", null);

                                        if (isChecked)
                                        {

                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 1;
                                            attObj.AttendancePeriodIDAfternoon = 1;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            //new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                            context.SubmitChanges();
                                            row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                        }
                                        else
                                        {
                                            //AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 0;
                                            attObj.AttendancePeriodIDAfternoon = 0;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            context.SubmitChanges();
                                            //new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        }
                                        lblErrorMsg.Text = "Attendance taken successfully";
                                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                        lblErrorMsg.Visible = true;
                                    }
                                }
                            }

                            //lblErrorMsg.Text = "Attendance taken successfully";
                            //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            //lblErrorMsg.Visible = true;

                        }
                    }

                    //--------------------------------------------Morning and Afternoon Attendance-----------------------------------------------------------//

                    else if (ddlAttendancePeriod.SelectedValue == "3")
                    {
                        foreach (GridViewRow row in gdvList2.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {


                                //var confirm = attendanceList.Where(x => x.AdmissionNo == row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text && x.AttendancePeriodIDAfternoon == Convert.ToInt16(ddlAttendancePeriod.SelectedValue));
                                PASSIS.LIB.AttendanceRegister attObj = context.AttendanceRegisters.FirstOrDefault(x => x.AdmissionNo == row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text && x.AttendanceDate == Convert.ToDateTime(txtdate.Text));
                                if (attObj == null)
                                {
                                    bool isCheckedMorning = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                    bool isCheckedAfternoon = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                    DateTime attendanceDate = DateTime.ParseExact(this.txtdate.Text, "yyyy-MM-dd", null);

                                    if (isCheckedMorning && isCheckedAfternoon)
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 1;
                                        newAttendanceObj.AttendancePeriodIDMorning = 1;
                                        newAttendanceObj.AttendancePeriodIDAfternoon = 1;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                        row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    }

                                    else if (isCheckedAfternoon && !isCheckedMorning)
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 1;
                                        newAttendanceObj.AttendancePeriodIDMorning = 0;
                                        newAttendanceObj.AttendancePeriodIDAfternoon = 1;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    }

                                    else if (isCheckedMorning && !isCheckedAfternoon)
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 1;
                                        newAttendanceObj.AttendancePeriodIDMorning = 1;
                                        newAttendanceObj.AttendancePeriodIDAfternoon = 0;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    }

                                    //else if (isCheckedMorning)
                                    //{
                                    //    AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                    //    newAttendanceObj.AttendanceDate = Convert.ToDateTime(txtdate.Text);
                                    //    newAttendanceObj.schoolId = logonUser.SchoolId;
                                    //    newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                    //    newAttendanceObj.PresentAbsent = 1;
                                    //    newAttendanceObj.AttendancePeriodIDMorning = 1;
                                    //    newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                    //    newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                    //    newAttendanceObj.GradeId = grd.GradeId;
                                    //    newAttendanceObj.AcademicSessionId = currentSessionId;
                                    //    newAttendanceObj.AcademicTermId = currentTermId;
                                    //    new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                    //    row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    //}

                                    //else if (isCheckedAfternoon)
                                    //{
                                    //    AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                    //    newAttendanceObj.AttendanceDate = Convert.ToDateTime(txtdate.Text);
                                    //    newAttendanceObj.schoolId = logonUser.SchoolId;
                                    //    newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                    //    newAttendanceObj.PresentAbsent = 1;
                                    //    newAttendanceObj.AttendancePeriodIDAfternoon = 1;
                                    //    newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                    //    newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                    //    newAttendanceObj.GradeId = grd.GradeId;
                                    //    newAttendanceObj.AcademicSessionId = currentSessionId;
                                    //    newAttendanceObj.AcademicTermId = currentTermId;
                                    //    new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                    //    row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                    //}

                                    else
                                    {
                                        AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                        newAttendanceObj.AttendanceDate = attendanceDate;
                                        newAttendanceObj.schoolId = logonUser.SchoolId;
                                        newAttendanceObj.CampusId = logonUser.SchoolCampusId;
                                        newAttendanceObj.PresentAbsent = 0;
                                        newAttendanceObj.AttendancePeriodIDMorning = 0;
                                        newAttendanceObj.AttendancePeriodIDAfternoon = 0;
                                        newAttendanceObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                        newAttendanceObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                        newAttendanceObj.GradeId = grd.GradeId;
                                        newAttendanceObj.AcademicSessionId = currentSessionId;
                                        newAttendanceObj.AcademicTermId = currentTermId;
                                        new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                    }

                                    lblErrorMsg.Text = "Attendance taken successfully";
                                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                    lblErrorMsg.Visible = true;

                                }

                                else
                                {
                                    if (attObj.AttendancePeriodIDMorning != null && attObj.AttendancePeriodIDAfternoon != null)
                                    {
                                        lblErrorMsg.Text = "Maximum attendance has been taken for today";
                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                        lblErrorMsg.Visible = true;
                                    }
                                    else
                                    {
                                        bool isCheckedMorning = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                        bool isCheckedAfternoon = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                                        DateTime attendanceDate = DateTime.ParseExact(this.txtdate.Text, "yyyy-MM-dd", null);

                                        if (isCheckedMorning && isCheckedAfternoon)
                                        {

                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 1;
                                            attObj.AttendancePeriodIDMorning = 1;
                                            attObj.AttendancePeriodIDAfternoon = 1;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            context.SubmitChanges();
                                            row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                            row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                        }

                                        else if (isCheckedMorning && !isCheckedAfternoon)
                                        {
                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 1;
                                            attObj.AttendancePeriodIDMorning = 1;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            context.SubmitChanges();
                                            row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                        }

                                        else if (isCheckedAfternoon && !isCheckedMorning)
                                        {

                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 1;
                                            attObj.AttendancePeriodIDAfternoon = 1;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            context.SubmitChanges();
                                            row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                                        }

                                        else
                                        {
                                            //AttendanceRegister newAttendanceObj = new AttendanceRegister();
                                            attObj.AttendanceDate = attendanceDate;
                                            attObj.schoolId = logonUser.SchoolId;
                                            attObj.CampusId = logonUser.SchoolCampusId;
                                            attObj.PresentAbsent = 0;
                                            attObj.AttendancePeriodIDMorning = 0;
                                            attObj.AttendancePeriodIDAfternoon = 0;
                                            attObj.AttendancePeriodID = Convert.ToInt16(ddlAttendancePeriod.SelectedValue);
                                            attObj.AdmissionNo = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                                            attObj.GradeId = grd.GradeId;
                                            attObj.AcademicSessionId = currentSessionId;
                                            attObj.AcademicTermId = currentTermId;
                                            context.SubmitChanges();
                                            //new AttendanceLIB().SaveAttendance(newAttendanceObj);
                                        }
                                        lblErrorMsg.Text = "Attendance taken successfully";
                                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                        lblErrorMsg.Visible = true;
                                    }
                                }
                            }

                            //lblErrorMsg.Text = "Attendance taken successfully";
                            //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            //lblErrorMsg.Visible = true;

                        }
                    }




                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }

        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occured, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }




    protected void chkAll1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll1")
            {
                foreach (GridViewRow row in gdvList2.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }

        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occured, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }


    protected void chkAll2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll2")
            {
                foreach (GridViewRow row in gdvList2.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }

        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occured, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }



}