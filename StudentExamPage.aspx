<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StudentExamPage.aspx.cs" Inherits="StudentExamPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }


    </style>

                                        <script type="text/javascript">
                                    //        function startTimer(duration, display) {

                                    //    var timer = duration, minutes, seconds;
                                    //    setInterval(function () {
                                    //        minutes = parseInt(timer / 60, 10);
                                    //        seconds = parseInt(timer % 60, 10);

                                    //        minutes = minutes < 10 ? "0" + minutes : minutes;
                                    //        seconds = seconds < 10 ? "0" + seconds : seconds;

                                    //        display.textContent = minutes + ":" + seconds;

                                    //        if (--timer < 0) {
                                    //            timer = duration;
                                    //        }
                                    //    }, 1000);
                                    //}
                                            
                                    //window.onload = function () {
                                    //    var fiveMinutes = 120 * 5,
                                    //        display = document.querySelector('#time');
                                    //    startTimer(fiveMinutes, display);
                                            //};
                                            window.onload = function() {
                                                var minutes = document.getElementById('<%= min.ClientID %>').innerHTML;

                                            var sec = 60;
                                            setInterval(function () {
                                               sec = sec < 10 ? "0" + sec : sec;
                                            document.getElementById("timer").innerHTML = minutes + " : " + sec;
                                            sec--;
                                            if (sec == 00) {
                                              minutes--;
                                              sec = 60;
                                                if (minutes == 0) {
                                                    window.location = "BasePage?ID=5";
                                                //minutes = 2;
                                              }
                                            }
                                          }, 1000);
                                        }
                                        </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
    
    <div class="content-inner">
                    <div class="container-fluid">
                        <!-- Begin Page Header-->
                      <%--  <div class="row">
                            <div class="page-header">
	                            <div class="d-flex align-items-center">
	                                <h2 class="page-header-title">PASSIS</h2>
	                                <div>
	                                <!-- <div class="page-header-tools">
	                                    <a class="btn btn-gradient-01" href="#">Login</a>
	                                </div> -->
	                                </div>
	                            </div>
                            </div>
                        </div>--%>
                        <!-- End Page Header -->
                        <!-- Begin Row -->
                        <div class="row flex-row">
                            <div class="col-12">
                                <!-- Form -->
                              <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CBT</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                             <div>
  
     <%--<asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="2000">
     </asp:Timer>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
               <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
           </ContentTemplate>
               <Triggers>

                   <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />

               </Triggers>
     </asp:UpdatePanel>--%>
 </div>
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
    <div>Examination closes in <span id="timer"><div id="min" runat="server" ></div></span> minutes! </div>
 
       

                 <%--   <asp:Wizard ID="wzdTaxPayment" runat="server"  OnFinishButtonClick="wzdTaxPayment_FinishButtonClick"
                                Width="100%" DisplaySideBar="False" OnNextButtonClick="wzdTaxPayment_NextButtonClick">
                                <StartNextButtonStyle CssClass="button" />
                                <FinishCompleteButtonStyle CssClass="button" />
                                <FinishPreviousButtonStyle CssClass="button" />
                                <FinishNavigationTemplate>
                                    <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                        class="btn btn-secondary" OnClick="FinishPreviousButton_Click" TabIndex="2" Text="Previous" />
                                    <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete"  class="btn btn-secondary"
                                        Text=" Save Student Details" Visible="<%# FinishCompleteButtonVisibility %>" />
                                </FinishNavigationTemplate>
                                <StepPreviousButtonStyle CssClass="button" />
                                <StartNavigationTemplate>
                                    <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" class="btn btn-secondary"
                                        Text="Next" />
                                </StartNavigationTemplate>
                                <StepNavigationTemplate>
                                    <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                        class="btn btn-secondary" meta:resourcekey="StepPreviousButtonResource1" TabIndex="3"
                                        Text="Previous" Visible="<%# StepPreviousButtonVisibility %>" />
                                    <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" TabIndex="2"
                                        Text="Next" class="btn btn-secondary" ValidationGroup="simple" Visible="<%# StepNextButtonVisibility %>" />
                                </StepNavigationTemplate>--%>

  <%--<WizardSteps>
      <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 2">

          </asp:WizardStep>
      </WizardSteps>--%>
<%--      </asp:Wizard>--%>

<div id="wizard_container" runat="server"></div>
<asp:HiddenField runat="server" ID="HiddenField1" />
<%--public partial class StudentExamPage : System.Web.UI.Page
{
    Wizard wizard1 = new Wizard();

    protected void Page_Load(object sender, EventArgs e)
    {
        int j =4;

        PASSISLIBDataContext context = new PASSISLIBDataContext();
        for (int i = 0; i <= j; i++)
        {
            WizardStep newStep = new WizardStep();
            RadioButtonList radioBtn1 = new RadioButtonList();

            for (int x = 0; x < 4; x++)
            {
                //if ()
                //{ }
                radioBtn1.Text = "options"+ x.ToString();
                radioBtn1.Items.Add("options" + x.ToString());
                newStep.Controls.Add(radioBtn1);


            }
            radioBtn1.SelectedIndexChanged += new EventHandler(radioBtn1_SelectedIndexChanged);

            newStep.ID = "step" + (i + 1).ToString(); ;
            newStep.Title = "Step" + (i + 1).ToString(); ;
            //WizardStepBase newStep = new WizardStep();
            //newStep.ID = "Step" + (i + 1).ToString();
            //wzdTaxPayment.WizardSteps.Add(newStep);
            wizard1.WizardSteps.Add(newStep);


        }


        //wizard1.NextButtonClick += new
        // WizardNavigationEventHandler(wizard1_NextButtonClick);
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


    //protected void wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    //{
    //    int score = 0;
    //    for (int i = 0; i < wizard1.WizardSteps.Count; i++)
    //    {
    //        if (e.CurrentStepIndex > 0)
    //        {
    //            RadioButton rdbt = wizard1.WizardSteps[e.CurrentStepIndex].Controls[1] as RadioButton;
    //            if (rdbt.Checked == true)
    //            {
    //                Response.Write(rdbt.Text);
    //                score = score + 1;
    //            }
    //        }
    //    }
    //}
    protected void wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        int score = 0;
        for (int i = 0; i < wizard1.WizardSteps.Count; i++)
        {
            for (int x = 0; x < 4; x++)
            {
                RadioButtonList rdList = (RadioButtonList)
               wizard1.WizardSteps[i].FindControl("RadioButtonList");

                if (rdList.SelectedIndex == 0)
                {
                    Response.Write(rdList.Text);
                    score = score + 1;
                }
            }
            
        }
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


    public bool FinishCompleteButtonVisibility
    {
        get
        {
            if (ViewState[":::Finish_Complete_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Finish_Complete_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Finish_Complete_Button_Visibility:::"] = value;
        }
    }

   

    public bool StepNextButtonVisibility
    {
        get
        {
            if (ViewState[":::Step_Next_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Step_Next_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Step_Next_Button_Visibility:::"] = value;
        }
    }
    protected void FinishPreviousButton_Click(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// Gets/Sets the visibility of the previous button 
    /// </summary>
    public bool StepPreviousButtonVisibility
    {
        get
        {
            if (ViewState[":::Step_Previous_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Step_Previous_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Step_Previous_Button_Visibility:::"] = value;
        }
    }

    protected void wzdTaxPayment_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {

        switch (e.CurrentStepIndex)
        {
            case 0:

                
                break;

            case 1:
              
                break;
        }

    }
    protected void wzdTaxPayment_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        switch (e.CurrentStepIndex)
        {
            case 0:

                break;
            case 1:

                break;
        }
    }
    protected void wzdTaxPayment_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
       

    }

}
    --%>



                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>
