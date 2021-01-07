using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PASSIS.LIB;


public partial class NextTerm : PASSIS.LIB.Utility.BasePage
{
    string _connString = PassisUtility.GetConnectionString();
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    BindGrid();
        //    ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
        //    ddlAcademicTerm.DataTextField = "AcademicTermName";
        //    ddlAcademicTerm.DataValueField = "id";
        //    ddlAcademicTerm.DataBind();

        //    ddlAcademicSession.DataSource = new AcademicSessionLIB().RetrieveAcademicSession();
        //    ddlAcademicSession.DataTextField = "SessionName";
        //    ddlAcademicSession.DataValueField = "ID";
        //    ddlAcademicSession.DataBind();
        //}
    }

    //protected void BindGrid()
    //{
    //    try
    //    {
    //        PASSISLIBDataContext passisLibDataContext = new PASSISLIBDataContext();
    //        gvdAcademicSession.DataSource = from academicSession in passisLibDataContext.AcademicSessions
    //                                        where academicSession.SchoolId == logonUser.SchoolId
    //                                        select new
    //                                        {
    //                                            academicSession.Id,
    //                                            academicSession.AcademicSessionName,
    //                                            AcademicTermName = academicSession.AcademicTerm1.AcademicTermName,
    //                                            academicSession.DateStart,
    //                                            academicSession.DateEnd
    //                                        };
    //        gvdAcademicSession.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

   
    protected void btnSaveAcademicSession_OnClick(object sender, EventArgs e)
    {
        try
        {
            DateTime DateStart = DateTime.ParseExact(this.txtDateStart.Text, "MM-dd-yyyy", null);

            PASSIS.LIB.ReportCardNextTermBegin newTerm = new ReportCardNextTermBegin();
            newTerm.NextTermBegins = DateStart;
            newTerm.SchoolID = logonUser.SchoolId;
            newTerm.CampusID = logonUser.SchoolCampusId;

            lblErrorMsg.Text = "Saved Successfully!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;

            context.ReportCardNextTermBegins.InsertOnSubmit(newTerm);
            context.SubmitChanges();
            
        }
        catch (Exception ex) { throw ex; }
    }
    
}
