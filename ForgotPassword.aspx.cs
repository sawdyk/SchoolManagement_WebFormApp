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

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PASSISConnectionString"].ConnectionString);
        SqlConnection.ClearPool(con);

        int x = 0;
        bool xstat = int.TryParse(Request.QueryString["sid"], out x);
        if (xstat)
        {
            clsMyDB db = new clsMyDB();
            db.connct();
            string que = "SELECT Logo FROM Schools WHERE Id=" + x;
            SqlDataReader dat = db.fetch(que);
            string logo = "";
            while (dat.Read())
            {
                logo = dat[0].ToString();
            }
            db.closeConnct();
            if (logo != "")
            {
                //this.slogo.Src = logo;
                //slogo.Visible = true;
            }
        }
        if (!IsPostBack)
        {
            // txtPassword.Value = txtUsername.Value = string.Empty;
            txtPhoneNumber.Text = txtUsername.Text = string.Empty;
            lblErrorMsg.Visible = false;
        }


    }
    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string phoneNumber = txtPhoneNumber.Text.Trim();
        if (string.IsNullOrEmpty(username))
        {
            lblErrorMsg.Text = string.Format("The username field can not be null. Try again.");
            lblErrorMsg.Visible = true;
            return;
        }
        if (string.IsNullOrEmpty(phoneNumber))
        {
            lblErrorMsg.Text = string.Format("The phone number field can not be null. Try again.");
            lblErrorMsg.Visible = true;
            return;
        }
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.User logonUser = new PASSIS.LIB.User();
        UsersLIB usrDal = new UsersLIB();
        logonUser = context.Users.FirstOrDefault(x => x.Username == username && x.PhoneNumber == phoneNumber);

        if (logonUser == null)
        {
            lblErrorMsg.Text = string.Format("Incorrect credential(s). Try again.");
            lblErrorMsg.Visible = true;
            return;
        }
        else
        {
            lblErrorMsg.Text = "Your Password: " + logonUser.Password.ToString();
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;
            return;
        }

    }
   
}
