﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class Store_ReturnInventory : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    CommonCls objcls = new CommonCls();
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
                    string ID = Request.QueryString["ID"].ToString();
                    divinwardform.Visible = true;
                    divtabl.Visible = false;
                    txtrowmetarial.Enabled = false;
                    BindReturnData(ID);
                    GenerateCode();
                }
                else
                {
                    FillGrid();
                }

            }
        }
    }
    public void BindReturnData(string ID)
    {
        SqlDataAdapter ad = new SqlDataAdapter("select * from tbl_InventoryRequest where IsDeleted=0 AND ID=" + ID + "", Cls_Main.Conn);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            string weights = dt.Rows[0]["APPPerWeight"].ToString();
            if(weights != "")
            {
                lblnumber.Text = dt.Rows[0]["RequestNo"].ToString();
                txtrowmetarial.Text = dt.Rows[0]["RowMaterial"].ToString();
                txtThickness.Text = dt.Rows[0]["Thickness"].ToString();
                txtThickness.Enabled = false;
                txtwidth.Text = dt.Rows[0]["Width"].ToString();
                txtwidth.Enabled = false;
                txtlength.Text = dt.Rows[0]["Length"].ToString();
                txtlength.Enabled = false;
                totalqty.Visible = false;
                Weight.Visible = true;
                txtWeight.Enabled = false;
                txtTotalQty.Text = dt.Rows[0]["APPQuantity"].ToString();
                txtinwardqantity.Text = dt.Rows[0]["APPQuantity"].ToString();
                txtWeights.Text = dt.Rows[0]["APPPerWeight"].ToString();
            }
            else
            {
                lblnumber.Text = dt.Rows[0]["RequestNo"].ToString();
                txtrowmetarial.Text = dt.Rows[0]["RowMaterial"].ToString();
                txtThickness.Text = dt.Rows[0]["Thickness"].ToString();
                txtThickness.Enabled = true;
                txtwidth.Text = dt.Rows[0]["Width"].ToString();
                txtwidth.Enabled = true;
                txtlength.Text = dt.Rows[0]["Length"].ToString();
                txtlength.Enabled = true;
                totalqty.Visible = true;
                Weight.Visible = false;
                txtWeight.Enabled = false;
                txtTotalQty.Text = dt.Rows[0]["APPQuantity"].ToString();
                txtinwardqantity.Text = dt.Rows[0]["APPQuantity"].ToString();
                txtWeights.Text = dt.Rows[0]["APPPerWeight"].ToString();
            }


        }


    }
    //Fill GridView
    private void FillGrid()
    {

        try
        {
            SqlCommand cmd = new SqlCommand("SP_InventoryDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "GetReturnInventorylist");
            if (txtRowMaterial.Text == null || txtRowMaterial.Text == "")
            {
                cmd.Parameters.AddWithValue("@RowMaterial", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RowMaterial", txtRowMaterial.Text);
            }
            if (txtInwardno.Text == null || txtInwardno.Text == "")
            {
                cmd.Parameters.AddWithValue("@InwardNo", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@InwardNo", txtInwardno.Text);
            }

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Columns.Count > 0)
            {
                GVPurchase.DataSource = dt;
                GVPurchase.DataBind();
            }


        }
        catch (Exception ex)
        {

            string errorMsg = "An error occurred : " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('" + errorMsg + "') ", true);

        }

    }
    public string GenerateCode()
    {

        string FinYear = null;
        string FinFullYear = null;
        if (DateTime.Today.Month > 3)
        {
            FinYear = DateTime.Today.AddYears(1).ToString("yy");
            FinFullYear = DateTime.Today.AddYears(1).ToString("yy");
        }
        else
        {
            var finYear = DateTime.Today.AddYears(1).ToString("yy");
            FinYear = (Convert.ToInt32(finYear) - 1).ToString();

            var finfYear = DateTime.Today.AddYears(1).ToString("yy");
            FinFullYear = (Convert.ToInt32(finfYear) - 1).ToString();
        }
        string previousyear = (Convert.ToDecimal(FinFullYear) - 1).ToString();
        string strInvoiceNumber = "";
        string fY = previousyear.ToString() + "-" + FinYear;
        string strSelect = @"select ISNULL(
        MAX(CAST(
            SUBSTRING(
                InwardNo, 
                CHARINDEX('/', InwardNo) + 1, 
                CHARINDEX('-', InwardNo) - CHARINDEX('/', InwardNo) - 1
            ) AS INT)
        ), 0
    ) AS maxid  from tbl_InwardData where InwardNo like '%" + fY + "%' AND IsReturn=1";
        // string strSelect = @"SELECT TOP 1 MAX(ID) FROM tblTaxInvoiceHdr where InvoiceNo like '%" + fY + "%' ";

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strSelect;
        con.Open();
        string result;

        result = cmd.ExecuteScalar().ToString();


        con.Close();
        if (result != "0")
        {
            int numbervalue = Convert.ToInt32(result.Substring(result.LastIndexOf("/") + 1));
            numbervalue = numbervalue + 1;
            strInvoiceNumber = numbervalue + "/" + previousyear.ToString() + "-" + FinYear;
        }
        else
        {
            strInvoiceNumber = "01" + "/" + previousyear.ToString() + "-" + FinYear;
        }
        txtInwardno.Text = strInvoiceNumber;
        return strInvoiceNumber;
    }
    protected void GVPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            divinwardform.Visible = true;
            divtabl.Visible = false;
            txtrowmetarial.Enabled = false;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPurchase.Rows[rowIndex];
            txtrowmetarial.Text = ((Label)row.FindControl("MaterialName")).Text;
            txtThickness.Text = ((Label)row.FindControl("Thickness")).Text;
            txtwidth.Text = ((Label)row.FindControl("Width")).Text;
            txtlength.Text = ((Label)row.FindControl("Length")).Text;

            txtTotalQty.Text = ((Label)row.FindControl("InwardQty")).Text;
            txtinwardqantity.Text = ((Label)row.FindControl("InwardQty")).Text;
            txtWeight.Text = ((Label)row.FindControl("Weight")).Text;
            lblnumber.Text = ((Label)row.FindControl("Inwardno")).Text;
            hdnid.Value = ((Label)row.FindControl("Inwardno")).Text;
            btnsavedata.Text = "Update";
        }
        if (e.CommandName == "RowDelete")
        {
            Cls_Main.Conn_Open();
            SqlCommand Cmd = new SqlCommand("UPDATE [tbl_InwardData] SET IsDeleted=@IsDeleted,DeletedBy=@DeletedBy,DeletedOn=@DeletedOn WHERE ID=@ID", Cls_Main.Conn);
            Cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
            Cmd.Parameters.AddWithValue("@IsDeleted", '1');
            Cmd.Parameters.AddWithValue("@DeletedBy", Session["UserCode"].ToString());
            Cmd.Parameters.AddWithValue("@DeletedOn", DateTime.Now);
            Cmd.ExecuteNonQuery();
            Cls_Main.Conn_Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "SuccessResult('Inward Entry Deleted Successfully..!!')", true);
            FillGrid();
        }
    }

    protected void GVPurchase_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVPurchase.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GVPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label InwardQty = e.Row.FindControl("InwardQty") as Label;
            //    Label OutwardQty = e.Row.FindControl("OutwardQty") as Label;
            //    Label DefectedQty = e.Row.FindControl("DefectedQty") as Label;
            //    LinkButton btnoutward = (LinkButton)e.Row.FindControl("btnoutward");
            //    LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
            //    LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");

            //    Double total = Convert.ToDouble(OutwardQty.Text) + Convert.ToDouble(DefectedQty.Text);
            //    if (Convert.ToDouble(InwardQty.Text) == total)
            //    {
            //        //e.Row.BackColor = System.Drawing.Color.LightPink;
            //        btnoutward.Visible = false;
            //    }

            //    if (OutwardQty.Text != "0" || DefectedQty.Text != "0")
            //    {
            //        btnEdit.Visible = false;
            //        btnDelete.Visible = false;
            //    }
            //    Label Inwardno = e.Row.FindControl("Inwardno") as Label;
            //    GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;
            //    gvDetails.DataSource = GetData(string.Format("select * from tbl_LM_Defects where InwardNo='{0}'", Inwardno.Text));
            //    gvDetails.DataBind();
            //}
        }
        catch
        {

        }
    }


    //Search RMC Search methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetRMCList(string prefixText, int count)
    {
        return AutoFillRowmName(prefixText);
    }

    public static List<string> AutoFillRowmName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT RowMaterial from tbl_InwardData where IsDeleted=0 and " + "RowMaterial like '%'+ @Search + '%' AND IsReturn=1";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["RowMaterial"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        divinwardform.Visible = true;
        divtabl.Visible = false;
        txtrowmetarial.Enabled = true;
        txtrowmetarial.Text = "";
        txtTotalQty.Text = "";
        txtrowmetarial.Enabled = true;
        hdnid.Value = "";
      
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        divinwardform.Visible = false;
        divtabl.Visible = true;
        Response.Redirect("../Store/ReturnInventory.aspx");
    }

    protected void btnsavedata_Click(object sender, EventArgs e)
    {
        Cls_Main.Conn_Open();
        SqlCommand cmd = new SqlCommand("SP_InventoryDetails", Cls_Main.Conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
        cmd.Parameters.AddWithValue("@Createdby", Session["UserCode"].ToString());
        if (btnsavedata.Text == "Update")
        {
            cmd.Parameters.AddWithValue("@Mode", "UpdateReturnInwarddata");
            cmd.Parameters.AddWithValue("@InwardQty", txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@Thickness", txtThickness.Text);
            cmd.Parameters.AddWithValue("@Width", txtwidth.Text);
            cmd.Parameters.AddWithValue("@Length", txtlength.Text);       
            cmd.Parameters.AddWithValue("@InwardNo", txtInwardno.Text);
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text);
            cmd.Parameters.AddWithValue("@Number", lblnumber.Text);
            cmd.Parameters.AddWithValue("@PerWeight", txtWeights.Text);
        }
        else
        {
            cmd.Parameters.AddWithValue("@Mode", "InseartReturnInwarddata");
            cmd.Parameters.AddWithValue("@InwardQty", txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@Thickness", txtThickness.Text);
            cmd.Parameters.AddWithValue("@Width", txtwidth.Text);
            cmd.Parameters.AddWithValue("@Length", txtlength.Text);
            cmd.Parameters.AddWithValue("@RowMaterial", txtrowmetarial.Text);
            cmd.Parameters.AddWithValue("@InwardNo", txtInwardno.Text);
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text);
            cmd.Parameters.AddWithValue("@Number", lblnumber.Text);
            cmd.Parameters.AddWithValue("@PerWeight", txtWeights.Text);
        }


        cmd.ExecuteNonQuery();
        Cls_Main.Conn_Close();
        Cls_Main.Conn_Dispose();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "SuccessResult('Saved Record Successfully..!!');", true);
    }

    protected void GVPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    //protected void txtDefectqty_TextChanged(object sender, EventArgs e)
    //{
    //    Double inwardqty = Convert.ToDouble(txtRemaining.Text);
    //    Double outwardqty = Convert.ToDouble(txtoutwardqty.Text);
    //    Double totaldefect = 0;

    //    totaldefect = Convert.ToDouble(txtDefectqty.Text);
    //    Double total = inwardqty - outwardqty - totaldefect;

    //    this.ModalPopupHistory.Show();
    //}

    //protected void txtoutwardqty_TextChanged(object sender, EventArgs e)
    //{
    //    Double inwardqty = Convert.ToDouble(txtinwardqty.Text);
    //    Double outwardqty = Convert.ToDouble(txtRemaining.Text);
    //    Double totaldefect = 0;
    //    if(inwardqty>=outwardqty)
    //    {
    //        //if (txtDefectqty.Text != "")
    //        //{
    //        //    totaldefect = Convert.ToDouble(txtDefectqty.Text);
    //        //    Double total = inwardqty - outwardqty - totaldefect;
    //        //    txtRemaining.Text = Convert.ToString(total);
    //        //}
    //        //else
    //        //{
    //        //    Double total = inwardqty - outwardqty;
    //        //    txtRemaining.Text = Convert.ToString(total);
    //        //}
    //    }
    //    else
    //    {
    //        txtoutwardqty.Text = "";
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please add quantity less then inward quantity..!!');", true);
    //    }


    //    this.ModalPopupHistory.Show();
    //}

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReturnInventory.aspx");
    }



    private static DataTable GetData(string query)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void txtRowMaterial_TextChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    //Search Inward Number methods
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetInwardnoList(string prefixText, int count)
    {
        return AutoFillInwardNo(prefixText);
    }

    public static List<string> AutoFillInwardNo(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select DISTINCT InwardNo from tbl_InwardData where  " + "InwardNo like '%' + @Search + '%' and IsDeleted=0 AND IsReturn=1";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["InwardNo"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void txtInwardno_TextChanged(object sender, EventArgs e)
    {
        FillGrid();

    }

    protected void btnSearchData_Click(object sender, EventArgs e)
    {
        FillGrid();

    }

    protected void btnresetfilter_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReturnInventory.aspx");
    }



    protected void txtSize_TextChanged(object sender, EventArgs e)
    {

        //DataTable dtpt = Cls_Main.Read_Table("select * from tbl_InwardData WHERE RowMaterial='" + txtrowmetarial.Text.Trim() + "' AND Size='" + txtSize.Text.Trim() + "' AND IsDeleted=0");
        //if (dtpt.Rows.Count > 0)
        //{
        //    txtTotalQty.Text = dtpt.Rows[0]["InwardQty"].ToString();
        //    hdnid.Value = dtpt.Rows[0]["InwardNo"].ToString();
        //    btnsavedata.Text = "Update";
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Already available this size row material please update quantity..!!');", true);
        //}
        //else
        //{
        //    txtTotalQty.Text = "";
        //    hdnid.Value = "";
        //    btnsavedata.Text = "Save";
        //}
    }
    public void Getdata()
    {
        try
        {
            //DataTable dtpt = Cls_Main.Read_Table("select * from tbl_InwardData WHERE RowMaterial='" + txtrowmetarial.Text.Trim() + "' AND Thickness='" + txtThickness.Text.Trim() + "' AND Width='" + txtwidth.Text.Trim() + "' AND Length='" + txtlength.Text.Trim() + "' AND IsDeleted=0");
            //if (dtpt.Rows.Count > 0)
            //{

            //    hdnid.Value = dtpt.Rows[0]["InwardNo"].ToString();
            //    txtWeight.Text = dtpt.Rows[0]["Weight"].ToString();
            //    txtTotalQty.Text = dtpt.Rows[0]["Quantity"] != DBNull.Value ? dtpt.Rows[0]["Quantity"].ToString(): "0";
            //    txtinwardqantity.Text = dtpt.Rows[0]["InwardQty"].ToString();
            //    Double Total = Convert.ToDouble(txtTotalQty.Text) + Convert.ToDouble(txtinwardqantity.Text);
            //    txtTotalQty.Text = Total.ToString();
            //    btnsavedata.Text = "Update";
            //}
            //else
            //{
            //    txtTotalQty.Text = "";
            //    hdnid.Value = "";
            //    btnsavedata.Text = "Save";
            //}
            if (txtThickness.Text != "" && txtwidth.Text != "" && txtlength.Text != "")
            {
                double thickness = Convert.ToDouble(txtThickness.Text);
                double width = Convert.ToDouble(txtwidth.Text);
                double length = Convert.ToDouble(txtlength.Text);
                double Quantity = string.IsNullOrEmpty(txtinwardqantity.Text) ? 0 : Convert.ToDouble(txtinwardqantity.Text);

                // Ensure inputs are non-negative
                if (thickness <= 0 || width <= 0 || length <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please enter positive values for thickness, width, and length...!!');", true);

                }

                // Calculate weight in kilograms
                double weight = length / 1000 * width / 1000 * thickness * 7.85;
                double totalweight = weight * Quantity;
                // Display the calculated weight
                txtWeight.Text = totalweight.ToString();
            }

        }
        catch { }
    }


    protected void txtrowmetarial_TextChanged(object sender, EventArgs e)
    {
        Getdata();

    }

    protected void txtThickness_TextChanged(object sender, EventArgs e)
    {
        Getdata();
    }

    protected void txtwidth_TextChanged(object sender, EventArgs e)
    {
        Getdata();
    }

    protected void txtlength_TextChanged(object sender, EventArgs e)
    {
        Getdata();
    }

    protected void txtinwardqantity_TextChanged(object sender, EventArgs e)
    {
        if(txtWeights.Text == "")
        {
            if (txtThickness.Text != "" && txtwidth.Text != "" && txtlength.Text != "" && txtinwardqantity.Text != "")
            {
                double thickness = Convert.ToDouble(txtThickness.Text);
                double width = Convert.ToDouble(txtwidth.Text);
                double length = Convert.ToDouble(txtlength.Text);
                double Quantity = Convert.ToDouble(txtinwardqantity.Text);

                // Ensure inputs are non-negative
                if (thickness <= 0 || width <= 0 || length <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please enter positive values for thickness, width, and length...!!');", true);

                }

                // Calculate weight in kilograms
                double weight = length / 1000 * width / 1000 * thickness * 7.85;
                double totalweight = weight * Quantity;
                // Display the calculated weight
                txtWeight.Text = totalweight.ToString();
            }
        }
        else
        {
            double PerWeight = Convert.ToDouble(txtWeights.Text);
            double Quantity = Convert.ToDouble(txtinwardqantity.Text);

            // Ensure inputs are non-negative
            if (PerWeight <= 0 )
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please enter positive values for thickness, width, and length...!!');", true);

            }
            double totalweight = PerWeight * Quantity;
            // Display the calculated weight
            txtWeight.Text = totalweight.ToString();

        }
       
    }
}


