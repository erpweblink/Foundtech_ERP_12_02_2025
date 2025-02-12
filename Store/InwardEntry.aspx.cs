﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class Store_InwardEntry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    CommonCls objcls = new CommonCls();
    DataTable Dt_Component = new DataTable();
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
                FillGrid();
                ViewState["RowNo"] = 0;
                Dt_Component.Columns.AddRange(new DataColumn[3] { new DataColumn("id"), new DataColumn("Defect"), new DataColumn("DefectQty") });
                ViewState["Details"] = Dt_Component;
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
            cmd.Parameters.AddWithValue("@Mode", "GetInwardlist");
            if (txtcustomersearch.Text == null || txtcustomersearch.Text == "")
            {
                cmd.Parameters.AddWithValue("@CustomerName", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CustomerName", txtcustomersearch.Text);
            }
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

    protected void GVPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RowEdit")
        {
            divinwardform.Visible = true;
            divtabl.Visible = false;                  
            txtrowmetarial.Enabled = false;
            txtcompanyname.Enabled = false;
            dropdownEntry.Visible = false;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPurchase.Rows[rowIndex];
            double wei;
            string PerWeight = ((Label)row.FindControl("PerWeights")).Text;
            if ( PerWeight != "")
            {
                wei = Convert.ToDouble(PerWeight);
            }
            else{
                wei = 0;
            }
            if(wei <= 0)
            {
                txtcompanyname.Text = ((Label)row.FindControl("CustomerName")).Text;
                txtrowmetarial.Text = ((Label)row.FindControl("MaterialName")).Text;
                txtThickness.Text = ((Label)row.FindControl("Thickness")).Text;
                txtwidth.Text = ((Label)row.FindControl("Width")).Text;
                txtlength.Text = ((Label)row.FindControl("Length")).Text;

                txtTotalQty.Text = ((Label)row.FindControl("InwardQty")).Text;
                txtinwardqantity.Text = ((Label)row.FindControl("InwardQty")).Text;
                txtWeight.Text = ((Label)row.FindControl("Weight")).Text;
                txtDescription.Enabled = true;
                txtSize.Text = ((Label)row.FindControl("Size")).Text;
                hdnid.Value = ((Label)row.FindControl("Inwardno")).Text;
                btnsavedata.Text = "Update";
            }
            else
            {
               
                txtWeights.Text = ((Label)row.FindControl("PerWeights")).Text;
                txtcompanyname.Text = ((Label)row.FindControl("CustomerName")).Text;
                txtrowmetarial.Text = ((Label)row.FindControl("MaterialName")).Text;
                txtThickness.Text = ((Label)row.FindControl("Thickness")).Text;
                txtThickness.Enabled = false;
                txtwidth.Text = ((Label)row.FindControl("Width")).Text;
                txtwidth.Enabled = false;
                txtlength.Text = ((Label)row.FindControl("Length")).Text;
                txtlength.Enabled = false;
                totalqty.Visible = false;
                txtinwardqantity.Text = ((Label)row.FindControl("InwardQty")).Text;
                txtWeight.Text = ((Label)row.FindControl("Weight")).Text;
                Weight.Visible = true;
                txtSize.Text = ((Label)row.FindControl("Size")).Text;
                txtSize.Enabled = false;
                txtDescription.Enabled = true;
                hdnid.Value = ((Label)row.FindControl("Inwardno")).Text;
                btnsavedata.Text = "Update";
            }

            
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
            //Authorization
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;

                string empcode = Session["UserCode"].ToString();
                DataTable Dt = new DataTable();
                SqlDataAdapter Sd = new SqlDataAdapter("Select ID from tbl_UserMaster where UserCode='" + empcode + "'", con);
                Sd.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    string id = Dt.Rows[0]["ID"].ToString();
                    DataTable Dtt = new DataTable();
                    SqlDataAdapter Sdd = new SqlDataAdapter("Select * FROM tblUserRoleAuthorization where UserID = '" + id + "' AND PageName = 'InwardEntry.aspx' AND PagesView = '1'", con);
                    Sdd.Fill(Dtt);
                    if (Dtt.Rows.Count > 0)
                    {
                        btnCreate.Visible = false;
                        GVPurchase.Columns[11].Visible = false;
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
            }
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
                com.CommandText = "select DISTINCT RowMaterial from tbl_InwardData where IsDeleted=0 and " + "RowMaterial like '%'+ @Search + '%' AND Status=1";

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
        txtcompanyname.Enabled = false;
        txtrowmetarial.Enabled = false;
        txtSize.Enabled = false;
        txtThickness.Enabled = false;
        txtwidth.Enabled = false;
        txtlength.Enabled = false;
        txtinwardqantity.Enabled = false;
        txtDescription.Enabled = false;
        btnsavedata.Visible = false;

        txtrowmetarial.Text = "";
        txtTotalQty.Text = "";
        txtSize.Text = "";
        hdnid.Value = "";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        divinwardform.Visible = false;
        divtabl.Visible = true;
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
            cmd.Parameters.AddWithValue("@Mode", "UpdateInwarddata");
           // Double Total = Convert.ToDouble(txtTotalQty.Text) + Convert.ToDouble(txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@InwardNo", hdnid.Value);
            cmd.Parameters.AddWithValue("@InwardQty", txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@Size", txtSize.Text);
            cmd.Parameters.AddWithValue("@Thickness", txtThickness.Text);
            cmd.Parameters.AddWithValue("@Width", txtwidth.Text);
            cmd.Parameters.AddWithValue("@Length", txtlength.Text);
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text);
            cmd.Parameters.AddWithValue("@PerWeight", txtWeights.Text);
        }
        else
        {
            cmd.Parameters.AddWithValue("@Mode", "InseartInwarddata");
            cmd.Parameters.AddWithValue("@InwardQty", txtinwardqantity.Text);
            cmd.Parameters.AddWithValue("@Size", txtSize.Text);
            cmd.Parameters.AddWithValue("@Thickness", txtThickness.Text);
            cmd.Parameters.AddWithValue("@Width", txtwidth.Text);
            cmd.Parameters.AddWithValue("@Length", txtlength.Text);
            cmd.Parameters.AddWithValue("@RowMaterial", txtrowmetarial.Text);
            cmd.Parameters.AddWithValue("@CustomerName", txtcompanyname.Text);
            cmd.Parameters.AddWithValue("@InwardNo", DBNull.Value);
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text);
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
        Response.Redirect("InwardEntry.aspx");
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
                com.CommandText = "select DISTINCT InwardNo from tbl_InwardData where  " + "InwardNo like '%' + @Search + '%' and IsDeleted=0 AND IsReturn=0";

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
        Response.Redirect("InwardEntry.aspx");
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
            if(txtThickness.Text!="" && txtwidth.Text!="" && txtlength.Text!="" )
            {
                double thickness = Convert.ToDouble(txtThickness.Text);
                double width = Convert.ToDouble(txtwidth.Text);
                double length = Convert.ToDouble(txtlength.Text);
                double Quantity = string.IsNullOrEmpty(txtinwardqantity.Text)? 0 : Convert.ToDouble(txtinwardqantity.Text);

                // Ensure inputs are non-negative
                if (thickness <= 0 || width <= 0 || length <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DeleteResult('Please enter positive values for thickness, width, and length...!!');", true);
                    
                }

                // Calculate weight in kilograms
                double weight = length/1000*width/1000*thickness * 7.85;
                double totalweight = weight * Quantity;
                // Display the calculated weight
                txtWeight.Text = totalweight.ToString();
            }

        }
        catch { }
    }


    protected void txtrowmetarial_TextChanged(object sender, EventArgs e)
    {
        //Getdata();

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
            if ( txtWeights.Text != "" && txtinwardqantity.Text != "")
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


    [ScriptMethod()]
    [WebMethod]
    public static List<string> GetCompanyList(string prefixText, int count)
    {
        return AutoFillCompanyName(prefixText);
    }

    public static List<string> AutoFillCompanyName(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "Select DISTINCT [Companyname] from [tbl_CompanyMaster] where " + "Companyname like @Search + '%' and IsDeleted=0";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> countryNames = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        countryNames.Add(sdr["Companyname"].ToString());
                    }
                }
                con.Close();
                return countryNames;
            }
        }
    }

    protected void txtcustomersearch_TextChanged(object sender, EventArgs e)
    {
        FillGrid();

    }

    protected void txtdropEntry_TextChanged(object sender, EventArgs e)
    {
        string val = txtdropEntry.SelectedValue;
        if(val == "1")
        {
            txtcompanyname.Enabled = true;
            txtrowmetarial.Enabled = true;
            txtSize.Enabled = false;
            txtSize.Text = "0";
            txtThickness.Enabled = false;
            txtThickness.Text = "0";
            txtwidth.Enabled = false;
            txtwidth.Text = "0";
            txtlength.Enabled = false;
            txtlength.Text = "0";
            txtinwardqantity.Enabled = true;
            txtDescription.Enabled = true;
            btnsavedata.Visible = true;

            Weight.Visible = true;
            totalqty.Visible = false;
            
            txtTotalQty.Text = "";
            txtWeights.Text = "";
            txtinwardqantity.Text = "";
            txtWeight.Text = "";
            txtrowmetarial.Text = "";
            hdnid.Value = "";

        }
        else if(val == "2")
        {
            txtcompanyname.Enabled = true;
            txtrowmetarial.Enabled = true;
            txtSize.Enabled = true;
            txtSize.Text = "";
            txtThickness.Enabled = true;
            txtThickness.Text = "";
            txtwidth.Enabled = true;
            txtwidth.Text = "";
            txtlength.Enabled = true;
            txtlength.Text = "";
            txtinwardqantity.Enabled = true;
            txtDescription.Enabled = true;
            btnsavedata.Visible = true;

            Weight.Visible = false;
            totalqty.Visible = true;

            txtTotalQty.Text = "";
            txtinwardqantity.Text = "";
            txtWeight.Text = "";
            txtrowmetarial.Text = "";
            hdnid.Value = "";
        }
        else
        {
            divinwardform.Visible = true;
            divtabl.Visible = false;
            txtcompanyname.Enabled = false;
            txtrowmetarial.Enabled = false;
            txtSize.Enabled = false;
            txtThickness.Enabled = false;
            txtwidth.Enabled = false;
            txtlength.Enabled = false;
            txtinwardqantity.Enabled = false;
            txtDescription.Enabled = false;
            btnsavedata.Visible = false;
            Weight.Visible = false;
            totalqty.Visible = true;

            txtrowmetarial.Text = "";
            txtTotalQty.Text = "";
            txtWeights.Text = "";
            txtSize.Text = "";
            txtThickness.Text = "";
            txtwidth.Text = "";
            txtinwardqantity.Text = "";
            txtWeight.Text = "";
            txtlength.Text = "";
            hdnid.Value = "";
        }

    }
}


