using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.IO;

public partial class AdminViewMultimedia : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    long id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("AdminExternalResources.aspx");
        }
        else
        {
            id = Convert.ToInt16(Request.QueryString["id"].ToString());
            this.containerDiv.InnerHtml = loadMedia();
            

        }
        PASSIS.LIB.ExternalResourceMultimedia gtMedia = context.ExternalResourceMultimedias.FirstOrDefault(x => x.Id == id);
        lblDescription.Text = gtMedia.Description.ToUpper();
    }


    public string loadMedia()
    {
        string mediatype = string.Empty;
        PASSIS.LIB.ExternalResourceMultimedia gtMedia = context.ExternalResourceMultimedias.FirstOrDefault(x => x.Id == id);
        if (gtMedia.MediaTypeId == 1)// Audio
        {
            string ret = @"<audio controls>
                          <source src='ExternalResourceMultimedia/" + gtMedia.MediaName + @"' type='audio/mp3'>
                        </audio>";
            mediatype = ret;
        }

        if (gtMedia.MediaTypeId == 2)// Video
        {
            string ret = @"<video width='1270' height='1000' controls>
                        <source src='ExternalResourceMultimedia/" + gtMedia.MediaName + @"' type='video/mp4'>
                        </video>";
            mediatype = ret;
        }
        return mediatype;
    
    }



}