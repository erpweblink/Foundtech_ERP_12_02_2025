using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

public partial class Production_OutwardList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    CommonCls objcls = new CommonCls();
    bool allCompleted = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserCode"] == null)
        {
            Response.Redirect("../Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["encryptedValue"] != null)
                {
                    string Id = objcls.Decrypt(Request.QueryString["encryptedValue"].ToString());
                    Session["ProjectCode"] = Id;
                    FillGrid();
                }
            }

        }
    }

    private void FillGrid()
    {
        DataTable dt = Cls_Main.Read_Table(" SELECT ProjectCode,OaNumber,ProjectName,ProductName,TotalSet,"+
            " InwardSet,OutwardSet,RemainingSet,Convert(nvarchar(10), SentDate, 121) AS SentDate,CreatedBy, "+
            " Remark FROM tbl_DispatchOutwardData Where ProjectCode ='" + Session["ProjectCode"].ToString() + "' ");

        txtProjCode.Text = dt.Rows[0]["ProjectCode"].ToString();
        txtProjName.Text = dt.Rows[0]["ProjectName"].ToString();
        txtOaNumber.Text = dt.Rows[0]["OaNumber"].ToString();

        GroupRecords.DataSource = dt;
        GroupRecords.DataBind();
    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Report();
    }


    public void Report()
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand(" SELECT ProjectCode,ProjectName,ProductName,TotalSet," +
                " InwardSet,OutwardSet,RemainingSet,Convert(nvarchar(10), SentDate, 121) AS SentDate, "+
                " UM.Username AS CreatedBy,Remark FROM tbl_DispatchOutwardData as DO "+
                " LEFT JOIN tbl_UserMaster as UM ON UM.UserCode = DO.CreatedBy "+
                " Where ProjectCode ='" + Session["ProjectCode"].ToString() + "' ", con))
            {
                DataTable Dt = new DataTable();
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ReportDataSource obj1 = new ReportDataSource("DataSet1", dt);
                    ReportViewer1.LocalReport.DataSources.Add(obj1);
                    ReportViewer1.LocalReport.ReportPath = "RDLC_Reports\\FTOutwardReport.rdlc";
                    ReportViewer1.LocalReport.Refresh();

                    //-------- Print PDF directly without showing ReportViewer ----
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string extension;
                    byte[] bytePdfRep = ReportViewer1.LocalReport.Render("EXCEL", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;

                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment; filename=" + dt.Rows[0]["ProjectCode"].ToString() + "_Reports.xls");


                    Response.BinaryWrite(bytePdfRep);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.Reset();

                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}