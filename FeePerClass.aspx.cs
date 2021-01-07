using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class FeePerClass : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext db = new PASSISLIBDataContext();
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SchlAdminPayment.master";
    //}

    protected string PageMode
    {
        get
        {
            //if(ViewState[":::PageMode:::"] == null)
            return ViewState[":::PageMode:::"].ToString();
        }
        set
        {
            ViewState[":::PageMode:::"] = value;
        }
    }
    protected Int64 SavedFeeId
    {
        get
        {
            if (ViewState[":::SavedFeeId:::"] == null)
                ViewState[":::SavedFeeId:::"] = "0";
            return Convert.ToInt64(ViewState[":::SavedFeeId:::"].ToString());
        }
        set
        {
            ViewState[":::SavedFeeId:::"] = value;
        }
    }
    protected PASSIS.LIB.Fee VS_Fee
    {
        get
        {
            if (ViewState[":::VS_Fee:::"] == null)
                ViewState[":::VS_Fee:::"] = "0";
            return ViewState[":::VS_Fee:::"] as PASSIS.LIB.Fee;
        }
        set
        {
            ViewState[":::VS_Fee:::"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //LoadFeeTemplate();

            //var feeType = from s in db.PaymentFeeTypes where s.SchoolId == logonUser.SchoolId select s;
            //ddlFeeType.DataSource = feeType.ToList();
            //ddlFeeType.DataTextField = "FeeName";
            //ddlFeeType.DataValueField = "Id";
            //ddlFeeType.DataBind();
            //ddlFeeType.Items.Insert(0, new ListItem("--Select--", "0", true));
            //ddlMandatory.Items.Insert(0, new ListItem("--Select--", "0", true));

            ddlSession.DataSource = schSession().Distinct();
            ddlSession.DataTextField = "SessionName";
            ddlSession.DataValueField = "ID";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            var term = from c in db.AcademicTerm1s
                       select c;
            ddlTerm.DataSource = term;
            ddlTerm.DataTextField = "AcademicTermName";
            ddlTerm.DataValueField = "Id";
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            var schlTypeId = db.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
            ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            var templateList = from s in db.PaymentFeeTemplates where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId select s;
            ddlTemplate.DataSource = templateList;
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));
            //ddlTemplate.Items.Insert(1, new System.Web.UI.WebControls.ListItem("No Template", "-1", true));

            //string mode = Request.QueryString["mode"];
            //if (!string.IsNullOrEmpty(mode))
            //{
            //    switch (mode)
            //    {
            //        case "create":
            //            PageMode = mode;
            //            trFeeCreateA.Visible = trFeeCreateB.Visible = true;
            //            var Session = (from c in db.AcademicSessions

            //                           where c.SchoolId == logonUser.SchoolId

            //                           select new
            //                           {

            //                               c.AcademicSessionName.SessionName


            //                           }).Distinct();


            //            //ddlAcademicSession.DataSource = Session;
            //            //ddlAcademicSession.DataTextField = "SessionName";
            //            //ddlAcademicSession.DataBind();

            //            //ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            //            //ddlTerm.DataBind();
            //            // ddlCampus.DataSource = ddlCampus.DataSource = new AcademicSessionLIB().GetAllSchoolCampus((long)logonUser.SchoolId);
            //            // ddlCampus.DataBind();
            //            // ddlCampus.SelectedValue = logonUser.SchoolCampusId.ToString();
            //            // ddlCampus.Enabled = true;

            //            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            //            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            //            //ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            //            //ddlYear.DataBind();
            //            //ddlFeeStatus.DataSource = new FeeLIB().getAllFeeStatus();
            //            //ddlFeeStatus.DataBind();
            //            break;
            //        case "detail":
            //            PageMode = mode;
            //            trFeeCreateA.Visible = trFeeCreateB.Visible = false;
            //            trFeeDetailA.Visible = trFeeDetailB.Visible = true;
            //            ddlForm.DataSource = new ClassGradeDAL().getAllClassGrade((long)logonUser.SchoolId);
            //            ddlForm.DataBind();
            //            //Fee fee = new FeeDAL().RetrieveFee(Convert.ToInt64(Session["FeeId"]));
            //            PASSIS.LIB.Fee fee = new FeeLIB().RetrieveFee(Convert.ToInt64(Request.QueryString["FeeId"]));
            //            lblFeeCode.Text = fee.Code;
            //            lblFeeName.Text = fee.Name;
            //            lblFeeValue.Text = fee.FlatValue.ToString("N");

            //            break;
            //    }
            //}



        }
    }

    protected void BindGrid()
    {
        //gdvList.DataSource = new FeeDAL().RetrieveAllFeeClassGrade(VS_Fee.SchoolId, VS_Fee.Id);
        //gdvList.DataBind();

    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlClass.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select a class";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Session is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Term is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlTemplate.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select your template";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            if (gvFeeTemplate.Rows.Count == 0)
            {
                lblErrorMsg.Text = "Trying to save empty data";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            foreach (GridViewRow row in gvFeeTemplate.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblFeeType = (Label)gvFeeTemplate.Rows[row.RowIndex].FindControl("lblFeeType");
                    Label lblFeeTypeId = (Label)gvFeeTemplate.Rows[row.RowIndex].FindControl("lblFeeTypeId");
                    Label lblTemplateId = (Label)gvFeeTemplate.Rows[row.RowIndex].FindControl("lblTemplateId");
                    DropDownList ddlMandatory = (DropDownList)gvFeeTemplate.Rows[row.RowIndex].FindControl("ddlMand");
                    TextBox txtAmount = (TextBox)gvFeeTemplate.Rows[row.RowIndex].FindControl("gvtxtAmount");

                    PASSIS.LIB.PaymentFee paymentFee = db.PaymentFees.FirstOrDefault(x =>
                        x.ClassId == Convert.ToInt64(ddlClass.SelectedValue) &&
                        x.SchoolId == logonUser.SchoolId &&
                        x.CampusId == logonUser.SchoolCampusId &&
                        x.FeeTypeId == Convert.ToInt64(lblFeeTypeId.Text) &&
                        x.TermId == Convert.ToInt64(ddlTerm.SelectedValue) &&
                        x.SessionId == Convert.ToInt64(ddlSession.SelectedValue));
                    if (paymentFee == null)
                    {
                        PASSIS.LIB.PaymentFee paymentFeee = new PASSIS.LIB.PaymentFee();
                        paymentFeee.Amount = Convert.ToDecimal(txtAmount.Text);
                        paymentFeee.CampusId = logonUser.SchoolCampusId;
                        paymentFeee.ClassId = Convert.ToInt64(ddlClass.SelectedValue);
                        paymentFeee.FeeTypeId = Convert.ToInt64(lblFeeTypeId.Text);
                        paymentFeee.Mandatory = Convert.ToInt32(ddlMandatory.SelectedValue);
                        paymentFeee.SchoolId = logonUser.SchoolId;
                        paymentFeee.TemplateId = Convert.ToInt64(lblTemplateId.Text);
                        paymentFeee.TermId = Convert.ToInt64(ddlTerm.SelectedValue);
                        paymentFeee.SessionId = Convert.ToInt64(ddlSession.SelectedValue);
                        db.PaymentFees.InsertOnSubmit(paymentFeee);
                    }
                    else
                    {
                        paymentFee.Amount = Convert.ToDecimal(txtAmount.Text);
                        paymentFee.CampusId = logonUser.SchoolCampusId;
                        paymentFee.ClassId = Convert.ToInt64(ddlClass.SelectedValue);
                        paymentFee.FeeTypeId = Convert.ToInt64(lblFeeTypeId.Text);
                        paymentFee.Mandatory = Convert.ToInt32(ddlMandatory.SelectedValue);
                        paymentFee.SchoolId = logonUser.SchoolId;
                        paymentFee.TemplateId = Convert.ToInt64(lblTemplateId.Text);
                        paymentFee.TermId = Convert.ToInt64(ddlTerm.SelectedValue);
                        paymentFee.SessionId = Convert.ToInt64(ddlSession.SelectedValue);
                    }
                }
            }
            db.SubmitChanges();
            lblErrorMsg.Text = "Fee Created Successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            //trErrorMsg.Visible = true;
            lblErrorMsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, try again";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
    protected void btnAddForm_OnClick(object sender, EventArgs e)
    {
        try
        {
            //check if the fee exist before 

            //repopulate the Class content


            //save the Fee 



            //txtPeriodName.Text = txtPeriodCode.Text = string.Empty;
            //BindGrid();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LoadFeeTemplate()
    {
        //var getFeeTemplate = from s in db.PaymentFeeTemplates where s.SchoolId == logonUser.SchoolId
        //                 select new {
        //                     s.Id,
        //                     s.TemplateName
        //                 };
        //gvFeeType.DataSource = getFeeTemplate;
        //gvFeeType.DataBind();
    }

    protected void gvCampus_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvFeeTemplate.EditIndex = e.NewEditIndex;
        LoadFeeTemplate();
        //txtTemplateName.Text = "";
        //txtExamScore.Text = "0";

    }
    protected void gvCampus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvFeeTemplate.EditIndex = -1;
        LoadFeeTemplate();
    }
    protected void gvCampus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Label lblId = (Label)gvFeeTemplate.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtGvTemplateName = (TextBox)gvFeeTemplate.Rows[e.RowIndex].FindControl("gvtxtTemplateName");
            TextBox txtGvAmount = (TextBox)gvFeeTemplate.Rows[e.RowIndex].FindControl("gvtxtAmount");
            DropDownList txtGvFeeCategory = (DropDownList)gvFeeTemplate.Rows[e.RowIndex].FindControl("gvddllFeeCategory");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.PaymentFeeTemplate objTemplateFee = context.PaymentFeeTemplates.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            string templateName = txtGvTemplateName.Text;
            string amount = txtGvAmount.Text;


            long feeCategoryId = Convert.ToInt64(txtGvFeeCategory.SelectedValue);

            objTemplateFee.TemplateName = templateName;
            //objPaymentFee.CategoryId = Convert.ToInt64(feeCategoryId);
            //objTemplateFee.Amount = Convert.ToDecimal(amount);
            context.SubmitChanges();
            gvFeeTemplate.EditIndex = -1;
            LoadFeeTemplate();
            lblErrorMsg.Text = "Updated Successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            txtGvTemplateName.Text = "";
            txtGvAmount.Text = "";
            //}
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, try again";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void gvScore_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFeeTemplate.PageIndex = e.NewPageIndex;
        LoadFeeTemplate();
    }
    protected void gvFeeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            //{
            DropDownList ddlFeeType = (DropDownList)e.Row.FindControl("gvddlFeeType");
            DropDownList ddlMandatory = (DropDownList)e.Row.FindControl("ddlMand");
            Label lblFeeType = (Label)e.Row.FindControl("lblFeeTypeId");
            Label lblMand = (Label)e.Row.FindControl("lblMand");

            ddlMandatory.Text = lblMand.Text;
            //}
        }
    }

    public string Mandatory(int mand)
    {
        string mandatory = "";
        if (mand == 0)
        {
            mandatory = "No";
        }
        else if (mand == 1)
        {
            mandatory = "Yes";
        }
        return mandatory;
    }

    public string FeeType(long id)
    {
        string feeType = "";
        PASSIS.LIB.PaymentFeeType objFeeType = db.PaymentFeeTypes.FirstOrDefault(x => x.Id == id);
        if (objFeeType != null)
        {
            feeType = objFeeType.FeeName;
        }
        return feeType;
    }

    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblfeetemplateEdit.Visible = true;
           var templateList = from s in db.PaymentFeeTemplateLists
                           where s.TemplateId == Convert.ToInt64(ddlTemplate.SelectedValue) && s.SchoolId == logonUser.SchoolId
                           select new
                           {
                               s.Id,
                               s.TemplateId,
                               s.PaymentFeeTemplate.TemplateName,
                               s.FeeTypeId,
                               s.Mandatory,
                               feeType = FeeType((long)s.FeeTypeId),
                               s.Amount,
                               mandatory = Mandatory((int)s.Mandatory)
                           };
        gvFeeTemplate.DataSource = templateList;
        gvFeeTemplate.DataBind();
    }

    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }
}