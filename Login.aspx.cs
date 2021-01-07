using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlConnection.ClearPool(con);

        int x = 0;
        bool xstat = int.TryParse(Request.QueryString["sid"], out x);
        //if (xstat)
        //{
        //    btnRegister.Visible = false;
        //    clsMyDB db = new clsMyDB();
        //    db.connct();
        //    string que = "SELECT Logo FROM Schools WHERE Id=" + x;
        //    SqlDataReader dat = db.fetch(que);
        //    string logo = "";
        //    while (dat.Read())
        //    {
        //        logo = dat[0].ToString();
        //    }
        //    db.closeConnct();
        //    if (logo != "")
        //    {
        //        this.slogo.Src = logo;
        //        slogo.Visible = true;

        //    }
        //}
        if (!IsPostBack)
        {
            txtPassword.Text = txtUsername.Text = string.Empty;
            lblErrorMsg.Visible = false;
        }


    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        if (string.IsNullOrEmpty(username))
        {
            lblErrorMsg.Text = string.Format("The username field can not be null. Try again.");
            lblErrorMsg.Visible = true;
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            lblErrorMsg.Text = string.Format("The password field can not be null. Try again.");
            lblErrorMsg.Visible = true;
            return;
        }

        PASSIS.LIB.User logonUser = new PASSIS.LIB.User();


        UsersLIB usrDal = new UsersLIB();
        logonUser = usrDal.RetrieveUser(username, password);

        if (logonUser == null)
        {
            lblErrorMsg.Text = string.Format("Incorrect credential(s). Try again.");
            lblErrorMsg.Visible = true;
            return;
        }
        else
        {


            if (logonUser.Username == "superadmin")
        {
            logonUser.LastLoginDate = DateTime.Now;
            usrDal.UpdateUser(logonUser);
            PASSIS.LIB.Utility.UserSessionData usrSessionDatasuper = new PASSIS.LIB.Utility.UserSessionData();
            usrSessionDatasuper.UserId = logonUser.Id;
            Session["UserSession"] = usrSessionDatasuper;
            Response.Redirect("~/Default.aspx");
        }

            usrDal.UpdateUser(logonUser);
            PASSIS.LIB.Utility.UserSessionData usrSessionData = new PASSIS.LIB.Utility.UserSessionData();
            usrSessionData.UserId = logonUser.Id;
            Session["UserSession"] = usrSessionData;
            if (logonUser.FirstName != null && logonUser.LastName != null)
            {
                PAConfig.LogAudit(PAConfig.AuditAction.Login, Request.Url.AbsoluteUri, "User Login: Fullname: " + logonUser.FirstName + " " + logonUser.LastName + ", Login Time: " + DateTime.Now, logonUser.Id);
            }

            Response.Redirect("~/Default.aspx");
        }

    }
    //protected void btnRegister_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/Self_Setup/Schools.aspx");
    //}
}