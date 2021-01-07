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
using System.Data.SqlClient;

public partial class ViewAttendanceSheet : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!isUserClassTeacher)
            {
                lblErrorMsg.Text = "You are not eligible to view attendance because you are not a class teacher";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {
                pnlMarkAttendance.Visible = true;
            }
        }
    }
    protected void btnViewAttendance_Click(object sender, EventArgs e)
    {
        lblStudents.Visible = true;
        try
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.GradeStudent grd = new ClassGradeLIB().RetrieveTeacherGrade(Convert.ToInt64(logonUser.Id));
            DateTime attendanceDate = Convert.ToDateTime(txtAttendanceDate.Text.Trim()).Date;
            var getAttendance = from attendance in context.AttendanceRegisters
                                where attendance.CampusId == logonUser.SchoolCampusId && attendance.GradeId == grd.GradeId && attendance.AttendanceDate == attendanceDate
                                select new
                                {
                                    Fullname = new UsersLIB().RetrieveByAdmissionNumber(attendance.AdmissionNo).StudentFullName,
                                    attendance.AdmissionNo,
                                    //attendance.PresentAbsent,
                                    attendance.AttendancePeriodIDMorning,
                                    attendance.AttendancePeriodIDAfternoon,
                                    attendance.SupervisorApproval
                                    //AttendanceStatus = Convert.ToBoolean(attendance.PresentAbsent) ? "Present" : "Absent"
                                };
            gdvList.DataSource = getAttendance;
            gdvList.DataBind();

            lblDate.Text = txtAttendanceDate.Text.Trim();
            lblNoOfRecordsSelected.Text =  getAttendance.Count().ToString();
            int presentMorning = getAttendance.Where(x => x.AttendancePeriodIDMorning == 1).Count();
            int absentMorning = getAttendance.Where(x => x.AttendancePeriodIDMorning == 0).Count();
            int presentAfternoon = getAttendance.Where(x => x.AttendancePeriodIDAfternoon == 1).Count();
            int absentAfternoon = getAttendance.Where(x => x.AttendancePeriodIDAfternoon == 0).Count();
            int presentPerDay = presentMorning + presentAfternoon;
            int absentPerDay = absentMorning + absentAfternoon;
            lblNoOfPresent.Text =  presentPerDay.ToString();
            lblNoOfAbsent.Text =  absentPerDay.ToString();
        }
        catch (Exception ex)
        {

        }
    }

    public string getAttendanceStatus(int attendanceStatus)
    {
        string status = "";
        if (attendanceStatus == 0)
        {
            status = "Absent";
        }
        else if (attendanceStatus == 1)
        {
            status = "Present";
        }

        return status;
    }
}