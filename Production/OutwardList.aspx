<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="OutwardList.aspx.cs" Inherits="Production_OutwardList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Content/assets/js/plugin/sweetalert/sweetalert.min.js"></script>
    <script>     
        function SuccessResult(msg, url) {
            swal("Success", msg, {
                icon: "success",
                buttons: {
                    confirm: {
                        className: "btn btn-success",
                        TimeRanges: "5000",
                    },
                },
            }).then(function () {
                // window.location.href = "Packing.aspx";
                window.location.href = url;
            });
        };
        function DeleteResult(msg, url) {
            swal("Delete!", msg, {
                icon: "error",
                buttons: {
                    confirm: {
                        className: "btn btn-danger",
                        TimeRanges: "5000",
                    },
                },
            }).then(function () {
                /*window.location.href = "Packing.aspx";*/
                window.location.href = url;
            });
        };
    </script>

    <style>
        .spancls {
            color: #5d5656 !important;
            font-size: 13px !important;
            font-weight: 600;
            text-align: left;
        }

        .starcls {
            color: red;
            font-size: 18px;
            font-weight: 700;
        }

        .card .card-header span {
            color: #060606;
            display: block;
            font-size: 13px;
            margin-top: 5px;
        }

        .errspan {
            float: right;
            margin-right: 6px;
            margin-top: -25px;
            position: relative;
            z-index: 2;
            color: black;
        }

        .currentlbl {
            text-align: center !important;
        }

        .completionList {
            border: solid 1px Gray;
            border-radius: 5px;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
        }

        .reqcls {
            color: red;
            font-weight: 600;
            font-size: 14px;
        }

        .aspNetDisabled {
            cursor: not-allowed !important;
        }

        .rwotoppadding {
            padding-top: 10px;
        }
    </style>
    <style>
        .modelprofile1 {
            background-color: rgba(0, 0, 0, 0.54);
            display: block;
            position: fixed;
            z-index: 1;
            left: 0;
            /*top: 10px;*/
            height: 100%;
            overflow: auto;
            width: 100%;
            margin-bottom: 25px;
        }

        .profilemodel2 {
            background-color: #fefefe;
            margin-top: 15px;
            /*padding: 17px 5px 18px 22px;*/
            padding: 0px 0px 15px 0px;
            width: 1116px;
            top: 40px;
            color: #000;
            border-radius: 5px;
        }

        .lblpopup {
            text-align: center;
        }

        .wp-block-separator:not(.is-style-wide):not(.is-style-dots)::before, hr:not(.is-style-wide):not(.is-style-dots)::before {
            content: '';
            display: block;
            height: 1px;
            width: 100%;
            background: #cccccc;
        }

        .btnclose {
            background-color: #ef1e24;
            float: right;
            font-size: 18px !important;
            /* font-weight: 600; */
            color: #f7f6f6 !important;
            border: 0px groove !important;
            background-color: none !important;
            /*margin-right: 10px !important;*/
            cursor: pointer;
            font-weight: 600;
            border-radius: 4px;
            padding: 4px;
        }

        /*hr {
            margin-top: 5px !important;
            margin-bottom: 15px !important;
            border: 1px solid #eae6e6 !important;
            width: 100%;
        }*/
        hr.new1 {
            border-top: 1px dashed green !important;
            border: 0;
            margin-top: 5px !important;
            margin-bottom: 5px !important;
            width: 100%;
        }

        .errspan {
            float: right;
            margin-right: 6px;
            margin-top: -25px;
            position: relative;
            z-index: 2;
            color: black;
        }

        .currentlbl {
            text-align: center !important;
        }

        .completionList {
            border: solid 1px Gray;
            border-radius: 5px;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
        }

        .headingcls {
            background-color: #01a9ac;
            color: #fff;
            padding: 15px;
            border-radius: 5px 5px 0px 0px;
        }

        @media (min-width: 1200px) {
            .container {
                max-width: 1250px !important;
            }
        }
    </style>
    <style>
        /* Make GridView header sticky */
        .sticky-header {
            position: sticky;
            top: 0;
            background-color: #fff; /* Ensure background stays white */
            z-index: 1; /* Keeps header above the grid content */
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1); /* Optional shadow */
        }

            /* Optional: Style for header text */
            .sticky-header th {
                padding: 10px;
                background-color: #f1f1f1; /* Light gray background */
                font-weight: bold;
            }
    </style>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Content1/img/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Content1/img/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
    <script type="text/javascript">
        function downloadDWGFile(base64File, fileName) {
            if (!base64File || !fileName) {
                console.error("File data or file name is missing.");
                return;
            }

            var byteCharacters = atob(base64File);
            var byteArray = new Uint8Array(byteCharacters.length);


            for (var i = 0; i < byteCharacters.length; i++) {
                byteArray[i] = byteCharacters.charCodeAt(i);
            }

            var blob = new Blob([byteArray], { type: "application/octet-stream" });

            var link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.download = fileName;

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid px-4">
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="row">
                        <div class="col-9 col-md-10">
                            <h4 class="mt-4 "><b>OUTWARD LIST</b></h4>
                        </div>
                        <div class="col-9 col-md-2">
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="btn btn-outline-success" Text="Import Excel" Style="font-size: 14px; margin-top: 15px;" />
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <br />
                        <div id="divtable" runat="server">
                            <div class="card">
                                <div class="card-header">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblProjCode" runat="server" Font-Bold="true" Text="Project Code" CssClass="form-label"></asp:Label>
                                            <asp:TextBox ID="txtProjCode" runat="server" Text="" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblProjName" runat="server" Font-Bold="true" Text="Project Name" CssClass="form-label"></asp:Label>
                                            <asp:TextBox ID="txtProjName" runat="server" Text="" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblOaNumber" runat="server" Font-Bold="true" Text="OA Number" CssClass="form-label"></asp:Label>
                                            <asp:TextBox ID="txtOaNumber" runat="server" Text="" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <div class="table">
                                            <br />
                                            <%-- Testing Gtid --%>

                                            <asp:GridView ID="GroupRecords" runat="server" CellPadding="4" DataKeyNames="ProjectCode" Width="100%"
                                                CssClass="display table table-striped table-hover" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label8" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Project Code" HeaderStyle-CssClass="gvhead" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProjectCode" runat="server" Text='<%#Eval("ProjectCode")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="OA Number" HeaderStyle-CssClass="gvhead" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOaNumber" runat="server" Text='<%#Eval("OaNumber")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Set" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReqQty" runat="server" Text='<%#Eval("TotalSet")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Inward Set" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRecivedQty" runat="server" Text='<%#Eval("InwardSet")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Outward Set" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSentQty" runat="server" Text='<%#Eval("OutwardSet")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remaining Set" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemainReqQty" runat="server" Text='<%#Eval("RemainingSet")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sent Date" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSentDate" runat="server" Text='<%#Eval("SentDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remark" HeaderStyle-CssClass="gvhead">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <%-- End  --%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
            </Triggers>
        </asp:UpdatePanel>

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false"></rsweb:ReportViewer>
    </form>

</asp:Content>

