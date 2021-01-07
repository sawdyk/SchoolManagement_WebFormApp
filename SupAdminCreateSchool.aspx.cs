using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using PASSIS.LIB;


public partial class SupAdminCreateSchool : PASSIS.LIB.Utility.BasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlSchoolType.DataSource = new SchoolLIB().SchoolType();
            ddlSchoolType.DataBind();
            BindGrid();
        }
    }

    protected void BindGrid()
    {
        gdvList.DataSource = new PASSIS.DAO.SchoolConfigDAL().getAllSchools();
        gdvList.DataBind();
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
       
        string cond = "Name='" + txtName.Text + "'";
        if (new clsMyDB().exist("Schools", cond))
        {
            //report
            lblErrorMsg.Text = "School Name already exist, kindly try a different one.";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            txtName.Focus();
            return;
        }
        if (txtName.Text.Trim() == "" || txtCode.Text.Trim() == "" || txtAddress.Text.Trim() == "")
        {
            //report
            lblErrorMsg.Text = "Some fields are empty, kindly review your input data.";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Focus();
        }
        else
        {
            try
            {
                string name = string.Empty;
                string code = string.Empty;
                string address = string.Empty;
                string url = string.Empty;
                name = txtName.Text.Trim();
                code = txtCode.Text.Trim().ToUpper();
                address = txtAddress.Text.Trim();
                url = "http:\\" + txtUrl.Text.Trim();
                SchoolConfigDAL schDal = new SchoolConfigDAL();
               
                //Upload image file....
                string logo = "~/Images/Schools_Logo/name-generic.png";
                if (slogo.PostedFile != null && slogo.PostedFile.ContentLength > 0)
                {
                    string[] fnSplit = slogo.PostedFile.FileName.Split('.');
                    string logo0 = "~/Images/Schools_Logo/" + code + "-logo." + fnSplit[fnSplit.Length - 1].ToString();
                    if (!System.IO.File.Exists(logo))
                    {
                        string logo1 = Server.MapPath(logo0);
                        slogo.PostedFile.SaveAs(logo1);
                        logo = logo0;
                    }
                }

                PASSIS.DAO.School sch = new PASSIS.DAO.School();
                sch.Code = code;
                sch.Name = name;
                sch.Address = address;
                //sch.Logo = logo;
                schDal.SaveSchools(sch);

                //update school logo
                clsMyDB mdb = new clsMyDB();
                mdb.connct();
                string que = "UPDATE Schools SET Logo='" + logo + "', Url='" + url + "', SchoolTypeId=" + ddlSchoolType.SelectedValue + "  WHERE Code='" + code + "' AND Name='" + name + "'";
                mdb.excQuery(que);
                mdb.closeConnct();

                //school admin

                //get school id
                clsMyDB db = new clsMyDB();
                db.connct();
                que = "SELECT id FROM Schools WHERE Code='" + code + "' AND Name='" + name + "'";
                SqlDataReader dat = db.fetch(que);
                int SchoolID = 0;
                while (dat.Read())
                {
                    int.TryParse(dat[0].ToString(), out SchoolID);
                }
                db.closeConnct();

                ////create school admin
                //Int32 selectedRole = 0;

                //selectedRole = Convert.ToInt32(2);
                //UsersDAL usrDal = new UsersDAL();

                //PASSIS.DAO.User usr = new PASSIS.DAO.User();
                //usr.EmailAddress = txtEmailAddress.Text.Trim();
                //usr.FirstName = txtFirstname.Text.Trim();
                ////string dt = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                ////usr.LastLoginDate = DateTime.Parse(dt, new CultureInfo("fr-FR"));
                //usr.LastLoginDate = (DateTime)SqlDateTime.MinValue;
                //usr.LastName = txtLastname.Text.Trim();
                //usr.DateOfBirth = (DateTime)SqlDateTime.MinValue;
                //usr.Password = "password";
                //usr.PhoneNumber = txtPhoneNumber.Text.Trim();
                ////usr.SchoolId = Convert.ToInt64(ddlSchool.SelectedValue.Trim());
                //usr.SchoolId = Convert.ToInt64(SchoolID);
                //usr.Username = txtUsername.Text.Trim();
                //usrDal.SaveUser(usr);

                ////save user role
                //UserRole usrRl = new UserRole();
                //usrRl.RoleId = selectedRole;
                //usrRl.UserId = usr.Id;
                //usrDal.SaveUserRole(usrRl);

                //report
                lblErrorMsg.Text = "School created successfully!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                BindGrid();

                ////ddlRole.SelectedValue = "0";
                //txtEmailAddress.Text = txtFirstname.Text = txtLastname.Text = txtPhoneNumber.Text = txtUsername.Text = string.Empty;

                //txtAddress.Text = txtCode.Text = txtName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = ex.Message;
                lblErrorMsg.Visible = true;
                throw ex;
            }
        }
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }


    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvList.EditIndex = -1;
        BindGrid();
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvList.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void gdvList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblId = (Label)gdvList.Rows[e.RowIndex].FindControl("lblId");
            TextBox schoolName = (TextBox)gdvList.Rows[e.RowIndex].FindControl("txtGetSchoolName");
            TextBox schoolAddress = (TextBox)gdvList.Rows[e.RowIndex].FindControl("txtGetSchoolAddress");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.School objSchool = context.Schools.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            objSchool.Name = schoolName.Text;
            objSchool.Address = schoolAddress.Text;
            context.SubmitChanges();
            gdvList.EditIndex = -1;
            BindGrid();
            lblResponse.Text = "Saved Successfully";
            lblResponse.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {

        }
    }
}
