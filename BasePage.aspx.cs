using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using PASSIS.LIB;
using System.Data.SqlClient;

public partial class BasePage : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    long id;
    protected void Page_Load(object sender, EventArgs e)
    {
        //id = Convert.ToInt16(Request.QueryString["ID"].ToString());
        if (Request.QueryString["id"] == null)
        {
            //LoadIcon();
            Response.Redirect("Default.aspx");
        }
        else
        {
            id = Convert.ToInt16(Request.QueryString["ID"].ToString());
            //LoadIcon();
            LoadMenus();

        }
    }


    public void LoadIcon()
    {
        //var Icons = from s in context.MenuRoles
        //            where s.SubMenuID == id & s.RoleID == 2
        var Icons = from s in context.MenuRoles
                    where s.SubMenuID == id & s.RoleID == logonUserRole.Id
                    select new
                    {
                        s.Menu.ID,
                        s.Menu.MenuName,
                        s.Menu.Url,
                        s.Menu.Icon
                    };

        //iconRepeater.DataSource = Icons;
        //iconRepeater.DataBind();
    }

    public void LoadMenus()
    {
        var Icons = from s in context.MenuRoles
                    where s.SubMenuID == id & s.RoleID == logonUserRole.Id
                    select s;


        string row = @" <div class='row flex-row'>";
        foreach (var icons in Icons)
        {

            row += "<div class='col-xl-4 col-md-6 col-sm-6'>";
            row += "  <a href='" + icons.Menu.Url + @"'  class='offer-img'>";
            row += " <div class='widget widget-12 has-shadow'>";
            row += " <div class='widget-body'>";
            row += "<div class='media'>";
            row += "<div class='align-self-center ml-5 mr-5'>";
            row += "<i class='" + icons.Menu.Icon + @"'></i>";
            row += "</div>";
            row += " <div class='media-body align-self-center'>";
            row += "<div class='title'>" + icons.Menu.MenuName + @"</div>";
            row += "<div class='number'></div>";
            row += "</div>";
            row += "</div>";
            row += "</div>";
            row += "</div>";
            row += "</a>";
            row += "</div>";

        }

        this.containerDivvvs.InnerHtml = row;

    }


}