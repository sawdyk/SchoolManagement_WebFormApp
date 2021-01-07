using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class FeeTemplate : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext db = new PASSISLIBDataContext();
    
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
            LoadFeeTemplate();
            LoadFeeType();
            lblsubjectResp.Visible = true;
            lblfeetemplateEdit.Visible = true;
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
            if (txtTemplateName.Text == "")
            {
                lblErrorMsg.Text = "Template Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            //PASSISLIBDataContext db = new PASSISLIBDataContext();
            PASSIS.LIB.PaymentFeeTemplate getFeeTemplate = db.PaymentFeeTemplates.FirstOrDefault(x => x.TemplateName == txtTemplateName.Text.Trim() && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);
            if (getFeeTemplate == null)
            {
                PASSIS.LIB.PaymentFeeTemplate FeeTemplate = new PASSIS.LIB.PaymentFeeTemplate();
                FeeTemplate.TemplateName = txtTemplateName.Text;
                FeeTemplate.SchoolId = Convert.ToInt64(logonUser.SchoolId);
                FeeTemplate.CampusId = Convert.ToInt64(logonUser.SchoolCampusId);

                db.PaymentFeeTemplates.InsertOnSubmit(FeeTemplate);
                db.SubmitChanges();

                foreach (GridViewRow row in GvFeeTemplate.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblId = (Label)GvFeeTemplate.Rows[row.RowIndex].FindControl("lblId");
                        DropDownList ddlMandatory = (DropDownList)GvFeeTemplate.Rows[row.RowIndex].FindControl("ddlMand");
                        TextBox txtAmount = (TextBox)GvFeeTemplate.Rows[row.RowIndex].FindControl("gvtxtAmount");
                        long feeTypeId = Convert.ToInt64(lblId.Text);
                        int mandatory = Convert.ToInt16(ddlMandatory.SelectedValue);
                        decimal amount = Convert.ToDecimal(txtAmount.Text);

                        PaymentFeeTemplateList objTempList = db.PaymentFeeTemplateLists.FirstOrDefault(x => x.FeeTypeId == feeTypeId && x.SchoolId == logonUser.SchoolId && x.TemplateId == FeeTemplate.Id);
                        if (objTempList == null)
                        {
                            PaymentFeeTemplateList objList = new PaymentFeeTemplateList();
                            objList.Amount = amount;
                            objList.FeeTypeId = feeTypeId;
                            objList.Mandatory = mandatory;
                            objList.SchoolId = logonUser.SchoolId;
                            objList.CampusId = logonUser.SchoolCampusId;
                            objList.TemplateId = FeeTemplate.Id;

                            db.PaymentFeeTemplateLists.InsertOnSubmit(objList);
                        }
                        else
                        {
                            objTempList.Amount = amount;
                            objTempList.FeeTypeId = feeTypeId;
                            objTempList.Mandatory = mandatory;
                            objTempList.SchoolId = logonUser.SchoolId;
                            objTempList.CampusId = logonUser.SchoolCampusId;
                            objTempList.TemplateId = FeeTemplate.Id;
                        }
                        db.SubmitChanges();
                    }
                }

                lblErrorMsg.Text = "Fee Template created successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                //trErrorMsg.Visible = true;
                txtTemplateName.Text = "";
                LoadFeeTemplate();
            }
            else
            {
                foreach (GridViewRow row in GvFeeTemplate.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblId = (Label)GvFeeTemplate.Rows[row.RowIndex].FindControl("lblId");
                        DropDownList ddlMandatory = (DropDownList)GvFeeTemplate.Rows[row.RowIndex].FindControl("ddlMand");
                        TextBox txtAmount = (TextBox)GvFeeTemplate.Rows[row.RowIndex].FindControl("gvtxtAmount");
                        long feeTypeId = Convert.ToInt64(lblId.Text);
                        int mandatory = Convert.ToInt16(ddlMandatory.SelectedValue);
                        decimal amount = Convert.ToDecimal(txtAmount.Text);

                        PaymentFeeTemplateList objTempList = db.PaymentFeeTemplateLists.FirstOrDefault(x => x.FeeTypeId == feeTypeId && x.SchoolId == logonUser.SchoolId && x.TemplateId == getFeeTemplate.Id);
                        if (objTempList == null)
                        {
                            PaymentFeeTemplateList objList = new PaymentFeeTemplateList();
                            objList.Amount = amount;
                            objList.FeeTypeId = feeTypeId;
                            objList.Mandatory = mandatory;
                            objList.SchoolId = logonUser.SchoolId;
                            objList.CampusId = logonUser.SchoolCampusId;
                            objList.TemplateId = getFeeTemplate.Id;

                            db.PaymentFeeTemplateLists.InsertOnSubmit(objList);
                        }
                        else
                        {
                            objTempList.Amount = amount;
                            objTempList.FeeTypeId = feeTypeId;
                            objTempList.Mandatory = mandatory;
                            objTempList.SchoolId = logonUser.SchoolId;
                            objTempList.CampusId = logonUser.SchoolCampusId;
                            objTempList.TemplateId = getFeeTemplate.Id;
                        }
                        db.SubmitChanges();
                    }
                }

                lblErrorMsg.Text = "Fee Template created successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                //trErrorMsg.Visible = true;
                txtTemplateName.Text = "";
                LoadFeeTemplate();

                //lblErrorMsg.Text = "The Fee Template already exists";
                //lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                //lblErrorMsg.Visible = true;
                //LoadFeeTemplate();
            }


            //PASSIS.LIB.Fee fee = new PASSIS.LIB.Fee();
            ////fee.FlatValue = Convert.ToDecimal(txtFeeValue.Text.Trim());
            //fee.Code = txtFeeCode.Text.Trim();
            //fee.IsDeleted = false;
            ////fee.Currency = Convert.ToInt64(ddlCurrency.SelectedValue);
            //fee.Name = txtFeeName.Text.Trim();
            //fee.SchoolId = (long)logonUser.SchoolId;
            ////fee.FeeStatusId = Convert.ToInt32(ddlFeeStatus.SelectedValue);
            //fee.AccountId = 0;
            //fee.CreatedBy = logonUser.Id;
            //fee.DateCreated = DateTime.Now;

            //new FeeLIB().SaveFee(fee);
            //SavedFeeId = fee.Id;
            //PASSIS.LIB.Fee feesConfigLink = db.Fees.FirstOrDefault(x => x.Id == SavedFeeId);
            ////PASSIS.LIB.Class_Grade getgrade = db.Class_Grades.FirstOrDefault(x => x.Name == ddlYear.SelectedItem.Text.Trim());
            ////PASSIS.LIB.AcademicSessionName _getsessionId = db.AcademicSessionNames.FirstOrDefault(x => x.SessionName == ddlAcademicSession.SelectedItem.Text.Trim());
            ////PASSIS.LIB.AcademicTerm1 _getTermId = db.AcademicTerm1s.FirstOrDefault(x => x.AcademicTermName == ddlTerm.SelectedItem.Text.Trim());
            //PASSIS.LIB.FeesConfig feesConfig = new PASSIS.LIB.FeesConfig();
            //feesConfig.FeeId = feesConfigLink.Id;
            ////feesConfig.FeeStatusId = Convert.ToInt32(ddlFeeStatus.SelectedValue);
            ////feesConfig.ClassId = getgrade.Id;
            ////feesConfig.Amount = Convert.ToDecimal(txtFeeValue.Text.Trim());
            ////feesConfig.SessionId = _getsessionId.ID;
            ////feesConfig.TermId = _getTermId.Id;
            ////feesConfig.Currency = Convert.ToInt64(ddlCurrency.SelectedValue);
            //feesConfig.SchoolId = (long)logonUser.SchoolId;
            //feesConfig.CampusId = (long)logonUser.SchoolCampusId;
            //new FeeLIB().SaveFeeConfig(feesConfig);

            //Session["FeeID"] = fee.Id.ToString();
            ////VS_Fee = fee;

            ////txtFeeCode.Text = txtFeeName.Text = txtFeeValue.Text = string.Empty;
            ////assign the values to the detail page 
            //lblFeeCode.Text = fee.Code;
            //lblFeeName.Text = fee.Name;
            //lblFeeValue.Text = fee.FlatValue.ToString("N");

            //BindGrid();
            //Response.Redirect(string.Format("~/cFee.aspx?mode=detail&FeeId={0}", fee.Id.ToString()));
            //Response.Redirect(string.Format("~/vFee.aspx"));
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
    protected void ddlClass_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //BindGrid();
    }


    public void LoadFeeTemplate()
    {
        var getFeeTemplate = from s in db.PaymentFeeTemplates
                             where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                             select new
                             {
                                 s.Id,
                                 s.TemplateName,
                                 templateList = getTemplateList(s.Id)
                             };
        gvFeeType.DataSource = getFeeTemplate;
        gvFeeType.DataBind();
    }

    public void LoadFeeType()
    {
        var getFeeType = from s in db.PaymentFeeTypes
                         where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                         select new
                         {
                             s.Id,
                             s.FeeName
                         };
        GvFeeTemplate.DataSource = getFeeType;
        GvFeeTemplate.DataBind();
    }

    protected void gvCampus_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvFeeType.EditIndex = e.NewEditIndex;
        LoadFeeTemplate();
        txtTemplateName.Text = "";
        //txtExamScore.Text = "0";

    }
    protected void gvCampus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvFeeType.EditIndex = -1;
        LoadFeeTemplate();
    }
    protected void gvCampus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Label lblId = (Label)gvFeeType.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtGvTemplateName = (TextBox)gvFeeType.Rows[e.RowIndex].FindControl("gvtxtTemplateName");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.PaymentFeeTemplate objTemplateFee = context.PaymentFeeTemplates.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            string templateName = txtGvTemplateName.Text;

            objTemplateFee.TemplateName = templateName;
            //objPaymentFee.CategoryId = Convert.ToInt64(feeCategoryId);
            //objTemplateFee.Amount = Convert.ToDecimal(amount);
            context.SubmitChanges();
            gvFeeType.EditIndex = -1;
            LoadFeeTemplate();
            lblMessage.Text = "Updated Successfully";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            txtGvTemplateName.Text = "";
            //}
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error occurred, try again";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void gvScore_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFeeType.PageIndex = e.NewPageIndex;
        LoadFeeTemplate();
    }
    protected void gvFeeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlList = (DropDownList)e.Row.FindControl("gvddllFeeCategory");

            var category = from s in db.PaymentCategories select s;
            //ddlList.DataSource = category.ToList();
            //ddlList.DataTextField = "CategoryName";
            //ddlList.DataValueField = "Id";
            //ddlList.DataBind();
            //ddlList.Items.Insert(0, new ListItem("--Select--", "0", true));
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

    public List<PaymentFeeTemplateList> getTemplateList(long templateId)
    {
        var list = from s in db.PaymentFeeTemplateLists where s.TemplateId == templateId select s;
        return list.ToList();
    }
    protected void GvFeeTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddlMandatory = (DropDownList)e.Row.FindControl("ddlMand");
    }
}