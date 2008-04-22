using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetMX.WebUI.WebControls;

public partial class _Default : System.Web.UI.Page
{
//   protected MBeanServerProxy proxy;
//   protected MBeanUI MBeanUI;
//   protected MultiView view;
//   protected View browse;
//   protected DropDownList beanList;
//   protected Button selectButton;
//   protected View details;
//   protected Button returnButton;
   
	protected void Page_Load(object sender, EventArgs e)
	{		
	}

	protected void ShowDetails(object sender, EventArgs e)
	{
		string selectedName = beanList.SelectedValue;
		MBeanUI.ObjectName = selectedName;
		view.ActiveViewIndex = 1;
	}

	protected void HideDetails(object sender, EventArgs e)
	{
		MBeanUI.ObjectName = null;
		view.ActiveViewIndex = 0;
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		beanList.DataSource = proxy.ServerConnection.QueryNames(null, null);
		beanList.DataBind();
	}
}