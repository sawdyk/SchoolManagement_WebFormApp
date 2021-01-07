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


public partial class AcademicSession : PASSIS.LIB.Utility.BasePage
{
    string _connString = PassisUtility.GetConnectionString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            BindGrid2();
            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataTextField = "AcademicTermName";
            ddlAcademicTerm.DataValueField = "id";
            ddlAcademicTerm.DataBind();

            ddlAcademicSession.DataSource = new AcademicSessionLIB().RetrieveAcademicSession();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
        }
    }

    protected void BindGrid()
    {
        try
        {
            PASSISLIBDataContext passisLibDataContext = new PASSISLIBDataContext();
            gvdAcademicSession.DataSource = from academicSession in passisLibDataContext.AcademicSessions
                                            where academicSession.SchoolId == logonUser.SchoolId
                                            select new
                                            {
                                                academicSession.Id,
                                                academicSession.AcademicSessionName,
                                                AcademicTermName = academicSession.AcademicTerm1.AcademicTermName,
                                                academicSession.DateStart,
                                                academicSession.DateEnd
                                            };
            gvdAcademicSession.DataBind();
        }
        catch (Exception ex)
        {
        }
    }

    protected void BindGrid2()
    {
        try
        {
            PASSISLIBDataContext passisLibDataContext = new PASSISLIBDataContext();
            gvdAcademicSession2.DataSource = from academicSession in passisLibDataContext.AcademicSessions
                                            where academicSession.SchoolId == logonUser.SchoolId
                                             select new
                                            {
                                                academicSession.Id,
                                                academicSession.AcademicSessionName.SessionName,
                                                AcademicTermName = academicSession.AcademicTerm1.AcademicTermName,
                                                academicSession.DateStart,
                                                academicSession.DateEnd,
                                                academicSession.IsCurrent,
                                                academicSession.IsOpened,
                                                academicSession.IsLocked,
                                                academicSession.IsClosed
                                            };
            gvdAcademicSession2.DataBind();
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvdAcademicSession_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdAcademicSession.PageIndex = e.NewPageIndex;
        BindGrid();
    }
    protected void gvdAcademicSession2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdAcademicSession2.PageIndex = e.NewPageIndex;
        BindGrid2();
    }


    public string checkTorF (string val)
    {
        string status = string.Empty;
        if (val == "True")
        {
            status = "Yes";
        }
        else { status = "No"; }
        return status;
    }

    protected void btnSaveAcademicSession_OnClick(object sender, EventArgs e)
    {
        try
        {
            DateTime DateStart = DateTime.ParseExact(this.txtDateStart.Text, "MM-dd-yyyy", null);
            DateTime DateEnd = DateTime.ParseExact(this.txtDateEnd.Text, "MM-dd-yyyy", null);

            PASSIS.LIB.AcademicSession aca = new PASSIS.LIB.AcademicSession();
            aca.AcademicSessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
            aca.CreatedBy = logonUser.Id;
            aca.DateEnd = DateEnd;
            aca.DateStart = DateStart;
            aca.AcademicTermId = Convert.ToInt32(ddlAcademicTerm.SelectedValue);
            aca.SchoolId = (long)logonUser.SchoolId;
            aca.IsOpened = false;
            aca.IsClosed = false;
            aca.IsLocked = false;
            new AcademicSessionLIB().SaveAcademicSession(aca);
            Response.Redirect("~/AcademicSession.aspx");
        }
        catch (Exception ex) { throw ex; }
    }
    protected void gvdAcademicSession_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("CURRENTTERM"))
        {

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
            long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);

            int rowItemIdd = Convert.ToInt32(e.CommandArgument);

            PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.Id == rowItemIdd);
            if (academicSession != null && academicSession.IsClosed == true)
            {
                lblResultMsg.Text = "";
                lblErrorMsg.Text = "This term has been closed for this session, Kindly contact Administrator!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connString))
                {
                    string SetCurrentSessionOff = "UPDATE AcademicSession SET IsCurrent = '" + false + "', IsOpened = '" + false + "' where SchoolId='" + logonUser.SchoolId + "' and IsCurrent = '" + true + "'";
                    SqlCommand cmdSetCurrentSessionOff = new SqlCommand(SetCurrentSessionOff, con);
                    con.Open();
                    cmdSetCurrentSessionOff.ExecuteNonQuery();

                    int rowItemId = Convert.ToInt32(e.CommandArgument);
                    string updateAcademicSession = "UPDATE AcademicSession SET IsCurrent = '" + true + "', IsOpened = '" + true + "' where Id='" + rowItemId + "'";
                    SqlCommand cmdUpdateAcademicSession = new SqlCommand(updateAcademicSession, con);
                    cmdUpdateAcademicSession.ExecuteNonQuery();

                    string updateAcademicSessionOpened = "UPDATE AcademicSession SET IsLocked = '" + false + "' where Id='" + rowItemId + "'";
                    SqlCommand cmdUpdateAcademicSessionOpened = new SqlCommand(updateAcademicSessionOpened, con);
                    cmdUpdateAcademicSessionOpened.ExecuteNonQuery();

                    
                }
                lblErrorMsg.Text = "";
                lblResultMsg.Text = "Session Set Successfully!";
               // Response.Redirect("~/AcademicSession.aspx");
                BindGrid();
                BindGrid2();
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = "Application error, kindly contact your administrator";
                using (PASSISLIBDataContext dbContext = new PASSISLIBDataContext())
                {
                    PASSIS.LIB.ErrorLog newlog = new PASSIS.LIB.ErrorLog
                    {
                        RequestUrl = Request.Url.ToString(),
                        ErrorMessage = ex.Message + "<br/> " + ex.StackTrace + "<br/> " + ex.Source,
                        EventDateTime = Convert.ToDateTime(DateTime.Now.ToString()),
                        StackTrace = ex.StackTrace
                    };
                    dbContext.ErrorLogs.InsertOnSubmit(newlog);
                    dbContext.SubmitChanges();
                }
            }
        }


        if (e.CommandName.Equals("OPENTERM"))
        {
            try
            { 
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                int rowItemId = Convert.ToInt32(e.CommandArgument);

                PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x=>x.Id == rowItemId && x.SchoolId == logonUser.SchoolId);
                if(academicSession.IsClosed == true)
                {
                    lblResultMsg.Text = "";
                    lblErrorMsg.Text = "This term has been closed for this session, Kindly contact Administrator!";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                PASSIS.LIB.AcademicSession academicSessions = context.AcademicSessions.FirstOrDefault(x => x.Id == rowItemId && x.SchoolId == logonUser.SchoolId);
                if (academicSessions.IsCurrent == false)
                {
                    lblResultMsg.Text = "";
                    lblErrorMsg.Text = "kindly set term as current before Opening!";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                academicSession.IsOpened = true;
                academicSession.IsLocked = false;
                context.SubmitChanges();

                lblErrorMsg.Text = "";
                lblResultMsg.Text = "";
                lblResultMsg.Text = "Term Opened Successfully!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;

                BindGrid();
                BindGrid2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        if (e.CommandName.Equals("CLOSETERM"))
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                int rowItemId = Convert.ToInt32(e.CommandArgument);
                PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.Id == rowItemId && x.SchoolId == logonUser.SchoolId);


                academicSession.IsClosed = true;
                academicSession.IsCurrent = false;
                academicSession.IsOpened = false;
                academicSession.IsLocked = false;
                context.SubmitChanges();

                lblResultMsg.Text = "";
                lblResultMsg.Text = "Term Closed Successfully!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;

                BindGrid();
                BindGrid2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        if (e.CommandName.Equals("LOCKTERM"))
        {
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();

                int rowItemId = Convert.ToInt32(e.CommandArgument);
                long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.Id == rowItemId && x.SchoolId == logonUser.SchoolId);
                if (academicSession.IsClosed == true)
                {
                    lblResultMsg.Text = "";
                    lblErrorMsg.Text = "This term has been closed for this session, Kindly contact Administrator!";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                academicSession.IsOpened = false;
                academicSession.IsLocked = true;
                context.SubmitChanges();

                lblResultMsg.Text = "";
                lblResultMsg.Text = "Term Locked Successfully!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;

                BindGrid();
                BindGrid2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
    