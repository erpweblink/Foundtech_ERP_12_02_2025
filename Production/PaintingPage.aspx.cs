﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Production_PaintingPage : System.Web.UI.Page
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
                if (Request.QueryString["ID"] != null)
                {
                    string Page = Request.QueryString["ID"].ToString();
                    string Id = objcls.Decrypt(Request.QueryString["EncryptedValue"].ToString());
                    Session["ProjectCode"] = Id;
                    Session["Stage"] = Page;
                    //lblPageName.Text = Page;
                    FillGrid();
                    ViewState["TempDataGrid"] = "";
                }
            }
            if (Request["__EVENTTARGET"] == btnhist.ClientID)
            {
                RefreshPage();
            }
        }
    }

    private void RefreshPage()
    {
        string url = Request.Url.ToString();

        Response.Redirect(Request.Url.ToString());
    }

    private void FillGrid()
    {
        DataTable dt = Cls_Main.Read_Table(@"SELECT  t1.OANumber, t1.Stage, RowMaterial, RawMateReqQTY, RawMateRemainingReqQty,
        (Sum(CAST(InwardQty as int)) / (SUM(CAST(TotalQTY AS INT)) / Cast(RawMateReqQTY as int))) AS ReceivedQty,
        (Cast(RawMateReqQTY as Int) - Cast(RawMateRemainingReqQty as int)) AS SentQTy,
        ((Sum(CAST(InwardQty as int)) / (SUM(CAST(TotalQTY AS INT)) / Cast(RawMateReqQTY as int)) - (Cast(RawMateReqQTY as Int) - Cast(RawMateRemainingReqQty as int))))AS RemainingQTy,
        t1.ProjectCode, t1.ProjectName, Count(t1.ID) AS JobCounts,
        TRY_CAST(SUM(CAST(TotalQTY AS INT)) AS INT) AS TotalQuantity,

           STUFF((SELECT ',' + CAST(JobNo AS NVARCHAR)
           FROM tbl_NewProductionDTLS t2
           WHERE t2.RowMaterial = t1.RowMaterial  And t2.Stage = t1.Stage And t2.OANumber = t1.OANumber
           FOR XML PATH('')), 1, 1, '') AS JobNoList,

           STUFF((SELECT ',' + CAST(TotalQTY AS NVARCHAR)
           FROM tbl_NewProductionDTLS AS t3
           WHERE t3.RowMaterial = t1.RowMaterial And t3.Stage = t1.Stage And t3.OANumber = t1.OANumber
           FOR XML PATH('')), 1, 1, '') AS TotalQTYlist,

           STUFF((SELECT ',' + CAST(InwardQTY AS NVARCHAR)
           FROM tbl_NewProductionDTLS AS t4
           WHERE t4.RowMaterial = t1.RowMaterial And t4.Stage = t1.Stage And t4.OANumber = t1.OANumber
           FOR XML PATH('')), 1, 1, '') AS InwardQTYlist,

          STUFF((SELECT ',' + CAST((cast(TotalQTY AS INT)/CAST(RawMateReqQTY AS INT)) AS NVARCHAR)
	        FROM tbl_NewProductionDTLS t6
			WHERE t6.RowMaterial = t1.RowMaterial AND t6.Stage = t1.Stage AND t6.OANumber = t1.OANumber
			FOR XML PATH('')), 1, 1, '') AS PerQTY,

           STUFF((SELECT ',' + CAST(Discription AS NVARCHAR)
           FROM tbl_NewProductionDTLS t5
           WHERE  t5.RowMaterial = t1.RowMaterial And t5.Stage = t1.Stage And t5.OANumber = t1.OANumber
           FOR XML PATH('')), 1, 1, '') AS Disc,

           CASE 
              WHEN STUFF((SELECT ',' + CAST(TotalQTY AS NVARCHAR)
                            FROM tbl_NewProductionDTLS AS t3
                            WHERE t3.RowMaterial = t1.RowMaterial 
                              AND t3.Stage = t1.Stage 
                              AND t3.OANumber = t1.OANumber
                            FOR XML PATH('')), 1, 1, '') = 
                     STUFF((SELECT ',' + CAST(InwardQTY AS NVARCHAR)
                            FROM tbl_NewProductionDTLS AS t4
                            WHERE t4.RowMaterial = t1.RowMaterial 
                              AND t4.Stage = t1.Stage 
                              AND t4.OANumber = t1.OANumber
                            FOR XML PATH('')), 1, 1, '') 
              THEN 'Complete'
		      WHEN (
					        SELECT SUM(CAST(InwardQTY AS INT))
					        FROM tbl_NewProductionDTLS AS t4
					        WHERE t4.RowMaterial = t1.RowMaterial 
					          AND t4.Stage = t1.Stage 
					          AND t4.OANumber = t1.OANumber
			         ) > 0
		      THEN 'In-Process'
              ELSE 'Pending'
           END AS Status

           FROM tbl_NewProductionDTLS AS t1
           WHERE " + " t1.ProjectCode = '" + Session["ProjectCode"].ToString() + "' and t1.Stage = '" + Session["Stage"].ToString() + "' " +
          " GROUP BY t1.OANumber, t1.Stage, RowMaterial,RawMateReqQTY,RawMateRemainingReqQty, t1.ProjectCode,t1.ProjectName " +
          " order by SentQTy desc, RemainingQTy desc ");


        if (dt.Rows.Count > 0)
        {
            txtProjCode.Text = dt.Rows[0]["ProjectCode"].ToString();
            txtProjName.Text = dt.Rows[0]["ProjectName"].ToString();
            txtOaNumber.Text = dt.Rows[0]["OaNumber"].ToString();
        }

        GroupRecords.DataSource = dt;
        GroupRecords.DataBind();
    }

    protected void GroupRecords_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblReqQty = e.Row.FindControl("lblReqQty") as Label;
                Label lblSentQty = e.Row.FindControl("lblSentQty") as Label;
                Label lblProdStatus = e.Row.FindControl("lblProdStatus") as Label;
                Label lblRemainingSet = e.Row.FindControl("lblRemainingSet") as Label;

                if (lblReqQty.Text == lblSentQty.Text)
                {
                    lblProdStatus.Text = "Completed";
                    lblProdStatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    if (lblProdStatus.Text == "Complete")
                    {
                        lblProdStatus.Text = "Read for Quality";
                        lblProdStatus.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (lblProdStatus.Text == "In-Process")
                    {
                        lblProdStatus.Text = "In-Process";
                        lblProdStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                    else
                    {
                        lblProdStatus.Text = "Pending";
                        lblProdStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

        }

        catch
        {
        }
    }

    protected void GroupRecords_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewDetails")
        {
            string rowIndex = e.CommandArgument.ToString();

            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

            string TotalQty = ((Label)row.FindControl("lblTotalQuantity")).Text;
            string reqQty = ((Label)row.FindControl("lblReqQty")).Text;
            string RemainreqQty = ((Label)row.FindControl("lblRemainReqQty")).Text;
            string JobNoList = ((Label)row.FindControl("lblJobNoList")).Text;
            string QtyList = ((Label)row.FindControl("lblTotalQTYlist")).Text;
            string DiscList = ((Label)row.FindControl("lblDisclist")).Text;
            string InwardQtyList = ((Label)row.FindControl("lblInwardQtylist")).Text;
            string PerQtylist = ((Label)row.FindControl("lblPerQtylist")).Text;

            int divVal = Convert.ToInt32(TotalQty) / Convert.ToInt32(reqQty);


            txtRequestedQty.Text = reqQty;
            txtRemainRequeQty.Text = RemainreqQty;
            txtinwardqty.Text = divVal.ToString();
            txttotalqty.Text = TotalQty;
            txtJobList.Text = JobNoList;

            // txtpending.Text = "0";
            txtoutwardqty.Text = TotalQty;

            string[] jobsNo = JobNoList.Split(',');
            string[] Qty = QtyList.Split(',');
            string[] Disc = DiscList.Split(',');
            string[] InwardQty = InwardQtyList.Split(',');
            string[] PerQty = PerQtylist.Split(',');
            DataTable Dt = new DataTable();
            Dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Checkbox"), new DataColumn("JobNo"), new DataColumn("Qty"), new DataColumn("Discr"), new DataColumn("InwardQTYlist"), new DataColumn("PerSetQty"), new DataColumn("Status") });
            int rowCount = jobsNo.Length > Qty.Length ? jobsNo.Length : Qty.Length;

            for (int i = 0; i < rowCount; i++)
            {
                string jobNo = jobsNo.Length > i ? jobsNo[i] : string.Empty;
                string qty = Qty.Length > i ? Qty[i] : string.Empty;
                string disc = Disc.Length > i ? Disc[i] : string.Empty;
                string inwardQty = InwardQty.Length > i ? InwardQty[i] : string.Empty;
                string perqty = PerQty.Length > i ? PerQty[i] : string.Empty;


                Dt.Rows.Add("", jobNo, qty, disc, inwardQty, perqty, "");
            }

            if (Dt.Rows.Count > 0)
            {
                grdgrid.DataSource = Dt;
                grdgrid.DataBind();

                btnSendtopro.Visible = allCompleted;
                btnsendtoback.Visible = allCompleted;
                ViewState["TempDataGrid"] = Dt;
            }
            if (RemainreqQty == "0")
            {
                btnSendtopro.Visible = false;
                btnsendtoback.Visible = false;
            }

            this.ModalPopupHistory.Show();
        }

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string url = Request.Url.ToString();
            string[] jobNos = txtJobNos.Text.Split(',');

            if (txtEnteredQty.Text != null && txtEnteredQty.Text != "")
            {
                int TotalQtny = Convert.ToInt32(txtRemainRequeQty.Text);
                int EnteredQtny = Convert.ToInt32(txtEnteredQty.Text);
                if (TotalQtny >= EnteredQtny)
                {
                    foreach (var item in jobNos)
                    {
                        Cls_Main.Conn_Open();
                        SqlCommand cmd = new SqlCommand("DB_Foundtech.QualityManageProductionDetails", Cls_Main.Conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "UpdateSendToNext");
                        cmd.Parameters.AddWithValue("@JobNo", item);
                        cmd.Parameters.AddWithValue("@StageNumber", 4);

                        int FinalQty = 0;
                        int totalQty = 0;
                        DataTable dt = Cls_Main.Read_Table("Select * from tbl_NewProductionDtls where JobNo ='" + item + "' and StageNumber = 4");
                        if (dt.Rows.Count > 0)
                        {
                            totalQty = Convert.ToInt32(dt.Rows[0]["TotalQTY"].ToString());
                            int InwardQty = Convert.ToInt32(dt.Rows[0]["InwardQTY"].ToString());
                            int ReqQty = Convert.ToInt32(dt.Rows[0]["RawMateReqQTY"].ToString());

                            int subQty = InwardQty / ReqQty;
                            for (int i = 0; i < EnteredQtny; i++)
                            {
                                FinalQty += subQty;
                            }

                        }

                        cmd.Parameters.AddWithValue("@InwardQty", Convert.ToDouble(totalQty));
                        cmd.Parameters.AddWithValue("@OutwardQty", Convert.ToDouble(FinalQty));
                        cmd.Parameters.AddWithValue("@PendingQty", "0");
                        cmd.Parameters.AddWithValue("@RemainingSentQty", TotalQtny - EnteredQtny);
                        cmd.Parameters.AddWithValue("@Remark", txtRemarks.Text);
                        cmd.Parameters.AddWithValue("@UserCode", Session["UserCode"].ToString());
                        cmd.ExecuteNonQuery();
                        Cls_Main.Conn_Close();
                        Cls_Main.Conn_Dispose();

                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "SuccessResult('Saved Record Successfully And Send to the Next..!!', '" + url + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please check Outward Quantity is Greater then Inward Quantity..!!', '" + url + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please fill data...........!!', '" + url + "');", true);
            }
        }
        catch
        {

        }
    }

    protected void btnsendtoback_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtoutwardqty.Text != null && txtoutwardqty.Text != "" && txtpending.Text != "")
            {
                if (Convert.ToDouble(txtpending.Text) + 1 > Convert.ToDouble(txtoutwardqty.Text))
                {
                    Cls_Main.Conn_Open();
                    SqlCommand cmd = new SqlCommand("DB_Foundtech.ManageProductionDetails", Cls_Main.Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "UpdateSendToBack");
                    //  cmd.Parameters.AddWithValue("@JobNo", txtjobno.Text);
                    cmd.Parameters.AddWithValue("@StageNumber", 5);
                    cmd.Parameters.AddWithValue("@InwardQty", Convert.ToDouble(txtinwardqty.Text));
                    cmd.Parameters.AddWithValue("@OutwardQty", Convert.ToDouble(txtoutwardqty.Text));
                    cmd.Parameters.AddWithValue("@PendingQty", Convert.ToDouble(txtpending.Text));
                    cmd.Parameters.AddWithValue("@Remark", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@UserCode", Session["UserCode"].ToString());
                    cmd.ExecuteNonQuery();
                    Cls_Main.Conn_Close();
                    Cls_Main.Conn_Dispose();


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "SuccessResult('Saved Record Successfully And Send Back..!!');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please fill data...........!!');", true);
            }
        }
        catch
        {

        }
    }

    protected void ImageButtonfile2_Click(object sender, ImageClickEventArgs e)
    {
        string id = ((sender as ImageButton).CommandArgument).ToString();

        Display(id);
    }

    public void Display(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                string CmdText = "select FileName from tbl_DrawingDetails where ID='" + id + "'";

                SqlDataAdapter ad = new SqlDataAdapter(CmdText, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string fileName = dt.Rows[0]["FileName"].ToString();
                    string fileExtension = Path.GetExtension(fileName);

                    if (fileExtension == ".pdf")
                    {
                        //Old Code 
                        Response.Redirect("~/Drawings/" + dt.Rows[0]["FileName"].ToString());
                    }
                    else
                    {
                        //New Code by Nikhil 04-01-2025
                        string filePath = Server.MapPath("~/Drawings/" + fileName);

                        if (File.Exists(filePath))
                        {
                            byte[] fileBytes = File.ReadAllBytes(filePath);
                            string base64File = Convert.ToBase64String(fileBytes);
                            string safeBase64File = base64File.Replace("'", @"\'");
                            string script = "downloadDWGFile('" + safeBase64File + "', '" + fileName + "');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "DownloadDWG", script, true);

                        }
                    }
                }
                else
                {
                    //lblnotfound.Text = "File Not Found or Not Available !!";
                }

            }
        }
    }

    protected void GroupRecords_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void lblCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelect = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkSelect.NamingContainer;

        Label lblQty = (Label)row.FindControl("lblQty");
        Label Desc = (Label)row.FindControl("lblDiscr");

        if (chkSelect.Checked)
        {
            int val = Convert.ToInt32(txtpending.Text) + Convert.ToInt32(lblQty.Text);
            txtpending.Text = val.ToString();
        }
        else
        {
            int val = Convert.ToInt32(txtpending.Text) - Convert.ToInt32(lblQty.Text);
            txtpending.Text = val.ToString();
        }
        this.ModalPopupHistory.Show();
    }

    protected void grdgrid_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void grdgrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                Label lblQty = e.Row.FindControl("lblQty") as Label;
                Label lblInwardQty = e.Row.FindControl("lblInwardQty") as Label;

                Label lblQtyPerSet = e.Row.FindControl("lblQtyPerSet") as Label;

                CheckBox checkBox = e.Row.FindControl("lblCheckBox") as CheckBox;
                checkBox.Enabled = false;
                if (lblInwardQty != null && lblQty != null)
                {
                    int InwardQty = Convert.ToInt32(lblInwardQty.Text);
                    int Qty = Convert.ToInt32(lblQty.Text);
                    int QtyPerSet = Convert.ToInt32(lblQtyPerSet.Text);

                    if (InwardQty == Qty)
                    {
                        lblStatus.Text = "Completed";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        checkBox.Checked = true;
                    }
                    else if (InwardQty >= QtyPerSet)
                    {
                        lblStatus.Text = "In-Process";
                        lblStatus.ForeColor = System.Drawing.Color.Orange;
                        checkBox.Checked = true;
                    }
                    else
                    {
                        lblStatus.Text = "Pending";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }

                if (lblStatus.Text != "Completed")
                {
                    allCompleted = false;
                }

            }
        }
        catch
        {
        }
    }

    public void SendModalPopUP(object sender, EventArgs e)
    {
        this.ModalPopupHistory.Show();
        txtReqQuantity.Text = txtRemainRequeQty.Text;
        if (txtReqQuantity.Text == "1")
        {
            txtEnteredQty.Text = txtReqQuantity.Text;
            txtEnteredQty.ReadOnly = true;
        }
        txtJobNos.Text = txtJobList.Text;
        this.ModalPopupExtender1.Show();

        /* New Function testing code by Nikhil 
         * DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[7]
        { new DataColumn("JobNo"),new DataColumn("Discr"), new DataColumn("Qty"), new DataColumn("InwardQTYlist"),
            new DataColumn("PerSetQty"),new DataColumn("OutwardQTYlist"), new DataColumn("Status")
        });
        int MainValue = 0;
        for (int i = 1; i <= Convert.ToInt32(txtReqQuantity.Text); i++)
        {

            int vl = 0;
            foreach (GridViewRow row in grdgrid.Rows)
            {
                vl = 0;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string jobNo = ((Label)row.FindControl("lblJobNo")).Text;
                    string discr = ((Label)row.FindControl("lblDiscr")).Text;
                    int qty = Convert.ToInt32(((Label)row.FindControl("lblQty")).Text);
                    int inwardQty = Convert.ToInt32(((Label)row.FindControl("lblInwardQty")).Text);
                    int outwardQty = Convert.ToInt32(((Label)row.FindControl("lblOutwardQty")).Text);
                    int perSetQty = Convert.ToInt32(((Label)row.FindControl("lblQtyPerSet")).Text);
                    string status = ((Label)row.FindControl("lblStatus")).Text;

                    if (inwardQty >= perSetQty)
                    {
                        dt.Rows.Add(jobNo, discr, qty, inwardQty - perSetQty, perSetQty,outwardQty, status);
                        vl = 1;
                    }
                }
            }
            if (vl > 0)
            {
                grdgrid.DataSource = null;
                grdgrid.DataSource = dt;
                grdgrid.DataBind();
                ++MainValue;
            }
        }
        if (MainValue != 0)
        {
            if (MainValue == 1)
            {
                txtEnteredQty.Text = MainValue.ToString();
                txtEnteredQty.ReadOnly = true;
            }
          
                txtJobNos.Text = txtJobList.Text;
            this.ModalPopupExtender1.Show();
        }*/
    }
    public void BackModalPopUP(object sender, EventArgs e)
    {
        this.ModalPopupHistory.Show();
        DataTable Dt = ViewState["TempDataGrid"] as DataTable;
        if (Dt.Rows.Count > 0)
        {
            GridSendBack.DataSource = Dt;
            GridSendBack.DataBind();
        }

        txtReqQuantity.Text = txtRemainRequeQty.Text;
        txtJobNos.Text = txtJobList.Text;
        this.ModalPopupExtender2.Show();
    }

    protected void txtEnteredQty_TextChanged(object sender, EventArgs e)
    {
        this.ModalPopupHistory.Show();
        TextBox txt = (TextBox)sender;
        if (txt.Text != "")
        {
            int enteredText = Convert.ToInt32(txt.Text);

            if (enteredText > Convert.ToInt32(txtReqQuantity.Text))
            {
                txt.Text = "";
            }
            if (enteredText <= 0)
            {
                txt.Text = "";
            }
        }

        this.ModalPopupExtender1.Show();
    }

    protected void GridSendBack_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void GridSendBack_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = e.Row.FindControl("lblStatus1") as Label;
                Label lblQty = e.Row.FindControl("lblQty1") as Label;
                Label lblInwardQty = e.Row.FindControl("lblInwardQty1") as Label;
                CheckBox checkBox = e.Row.FindControl("lblCheckBox1") as CheckBox;
                checkBox.Enabled = false;
                if (lblInwardQty != null)
                {
                    int InwardQty = Convert.ToInt32(lblInwardQty.Text);
                    int Qty = Convert.ToInt32(lblQty.Text);

                    // Update the status text based on the status code
                    if (InwardQty == Qty)
                    {
                        lblStatus.Text = "Completed";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        checkBox.Checked = true;
                    }
                    else
                    {
                        lblStatus.Text = "In-Process";
                        lblStatus.ForeColor = System.Drawing.Color.Orange;
                    }
                }

                if (lblStatus.Text != "Completed")
                {
                    allCompleted = false;
                }

            }
        }
        catch
        {
        }

    }

}