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
public partial class CheckAttendance : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindDropDown();
        }
    }

    protected void BindDropDown()
    {
        ddlWard.DataSource = new UsersDAL().RetrieveParentsChildren(logonUser.Id);
        ddlWard.DataBind();
    }

    protected void btnViewAttendance_Click(object sender, EventArgs e)
    {
        lblStudents.Visible = true;
        try
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            long academicSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
            long academicTermId = (long)new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
            PASSIS.LIB.GradeStudent grd = new ClassGradeLIB().RetrieveTeacherGrade(Convert.ToInt64(logonUser.Id));
            var getAttendance = from attendance in context.AttendanceRegisters
                                where attendance.CampusId == logonUser.SchoolCampusId && attendance.AdmissionNo == ddlWard.SelectedValue && attendance.AcademicSessionId == academicSessionId && attendance.AcademicTermId == academicTermId
                                select new
                                {
                                    //attendance.PresentAbsent,
                                    //AttendanceStatus = Convert.ToBoolean(attendance.PresentAbsent) ? "Present" : "Absent",
                                    attendance.AttendanceDate,
                                    attendance.AttendancePeriodIDMorning,
                                    attendance.AttendancePeriodIDAfternoon
                                };
            gdvList.DataSource = getAttendance;
            gdvList.DataBind();

            lblAdmissionNo.Text = "Admission No: " + ddlWard.SelectedValue;
            lblNoOfRecordsSelected.Text = "Total Selected Records: " + getAttendance.Count().ToString();
            //lblNoOfPresent.Text ="No of times present: "+ getAttendance.Where(x => x.PresentAbsent == 1).Count().ToString();
            //lblNoOfAbsent.Text = "No of times absent: " + getAttendance.Where(x => x.PresentAbsent == 0).Count().ToString();

            int presentMorning = getAttendance.Where(x => x.AttendancePeriodIDMorning == 1).Count();
            int absentMorning = getAttendance.Where(x => x.AttendancePeriodIDMorning == 0).Count();
            int presentAfternoon = getAttendance.Where(x => x.AttendancePeriodIDAfternoon == 1).Count();
            int absentAfternoon = getAttendance.Where(x => x.AttendancePeriodIDAfternoon == 0).Count();
            int presentPerDay = presentMorning + presentAfternoon;
            int absentPerDay = absentMorning + absentAfternoon;
            lblNoOfPresent.Text = "No of present: " + presentPerDay.ToString();
            lblNoOfAbsent.Text = "No of absent: " + absentPerDay.ToString();
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