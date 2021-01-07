using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;

public partial class StudentExamPage : System.Web.UI.Page
{
    Wizard wizard1 = new Wizard();

    protected void Page_Load(object sender, EventArgs e)
    {
        int j = 4;

        string minutes = "1"; //gets the CBT time
        min.InnerText = minutes;

        PASSISLIBDataContext context = new PASSISLIBDataContext();
        for (int i = 0; i <= j; i++)
        {
            WizardStepBase newStep = new WizardStep();
            RadioButtonList radioBtn1 = new RadioButtonList();
            Label lbl = new Label();

            //for (int x = 0; x < 4; x++)
            //{
            radioBtn1.ID = "RadioBtn"+ i.ToString();
            radioBtn1.Text = "options";
            radioBtn1.Items.Add("options1");
            radioBtn1.Items.Add("options2");
            radioBtn1.Items.Add("options3");
            radioBtn1.Items.Add("options4");
            newStep.Controls.Add(radioBtn1);

           
            //}
            radioBtn1.SelectedIndexChanged += new EventHandler(radioBtn1_SelectedIndexChanged);

            newStep.ID = "step" + (i + 1).ToString(); ;
            newStep.Title = "Step" + (i + 1).ToString(); ;
            //WizardStepBase newStep = new WizardStep();
            //newStep.ID = "Step" + (i + 1).ToString();
            //wzdTaxPayment.WizardSteps.Add(newStep);
            wizard1.WizardSteps.Add(newStep);


        }


        wizard1.NextButtonClick += new
         WizardNavigationEventHandler(wizard1_NextButtonClick);
        wizard1.FinishButtonClick += new
        WizardNavigationEventHandler(wizard1_FinishButtonClick);


        //  PlaceHolder1.Controls.Add(WizardControl);

        wizard_container.Controls.Add(wizard1);
        //Wizard wizard1 = new Wizard();
        //TextBox text1 = new TextBox();
        //WizardStep step1 = new WizardStep();
        //step1.Controls.Add(text1);
        //step1.ID = "step3";
        //step1.Title = "Step 3";


        //TextBox text2 = new TextBox();
        //WizardStep step2 = new WizardStep();
        //step2.Controls.Add(text2);
        //step2.ID = "step4";
        //step2.Title = "Step 4";


        //wizard1.WizardSteps.Add(step2);

    }


    public void radioBtn1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        int score = 0;
        for (int i = 0; i < wizard1.WizardSteps.Count; i++)
        {
            
            if (e.CurrentStepIndex >= 0)
            {
                

                WizardStepBase wz = wizard1.WizardSteps[i];
                RadioButtonList rdbtn = wz.FindControl("RadioBtn" + i.ToString()) as RadioButtonList;
                // WizardStepBase wzStp = wizard1.WizardSteps[i];
                //RadioButton rdbt = wizard1.WizardSteps[e.CurrentStepIndex].Controls[i] as RadioButton;
                //RadioButton rdbt = wizard1.WizardSteps[i].Controls[i] as RadioButton ;
                //if (!IsPostBack) {
                    if (rdbtn.SelectedValue == "options1")
                    {
                        Response.Write("Checked!");
                    score = score + 1;

                    Response.Write(score);
                        Session["score"] = score.ToString();
                    }
                //}
               
            }
        }
    }
    protected void wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
       
            
                lblErrorMsg.Text = "Your Total score is " + Session["score"].ToString() + " out of " + wizard1.WizardSteps.Count;
            
        
        //int score = 0;

        //if (e.CurrentStepIndex != wizard1.WizardSteps.Count - 1)
        //    return;
        //if (e.CurrentStepIndex > 0)
        //{
          

        //    for (int i = 0; i < wizard1.WizardSteps.Count; i++)
        //    {
                

        //        for (int x = 0; x < 4; x++)
        //        {
        //            WizardStepBase wzStp = wizard1.WizardSteps[i];
        //            RadioButton rd = wzStp.FindControl("") as RadioButton;

        //            if (rd.Checked == true)
        //            {
        //                Response.Write("isChecked");
        //                score = score + 1;
        //            }
        //        }

        //    }

        //    lblErrorMsg.Text = score.ToString();
        //}
    }

    //Wizard WZ = (Wizard)sender;

    //int score = 0;
    //for (int i = 0; i < WZ.WizardSteps.Count; i++)
    //{
    //    RadioButton newRad = new RadioButton();

    //    newRad = wizard_container.FindControl("option") as RadioButton;

    //    if (newRad.Checked == true)
    //    {
    //        score = score + 1;
    //    }
    //}
    //lblErrorMsg.Text = "Your score is " + score.ToString() +
    //                 " out of a total of " +
    //                 (WZ.WizardSteps.Count).ToString();



    
}