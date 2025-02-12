﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="Dispatch.aspx.cs" Inherits="Production_Dispatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Content/assets/js/plugin/sweetalert/sweetalert.min.js"></script>

    <script>     
        function SuccessResult(msg) {
            swal("Success", msg, {
                icon: "success",
                buttons: {
                    confirm: {
                        className: "btn btn-success",
                        TimeRanges: "5000",
                    },
                },
            }).then(function () {
                window.location.href = "Dispatch.aspx";
            });
        };
        function DeleteResult(msg) {
            swal("Delete!", msg, {
                icon: "error",
                buttons: {
                    confirm: {
                        className: "btn btn-danger",
                        TimeRanges: "5000",
                    },
                },
            }).then(function () {
                window.location.href = "Dispatch.aspx";
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
            margin-top: 25px;
            /*padding: 17px 5px 18px 22px;*/
            padding: 0px 0px 15px 0px;
            width: 100%;
            top: 40px;
            color: #000;
            border-radius: 5px;
        }

        .lblpopup {
            text-align: left;
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
    <form id="form1" runat="server">
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
                            <h4 class="mt-4 "><b>DISPATCH LIST</b></h4>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label ID="Label19" runat="server" Font-Bold="true" Text="Customer Name :"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txtCustName" CssClass="form-control" placeholder="Search Customer" runat="server" OnTextChanged="txtCustomerName_TextChanged" Width="100%" AutoPostBack="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ErrorMessage="Please Enter Customer"
                                    ControlToValidate="txtCustomerName" ValidationGroup="form1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList"
                                    TargetControlID="txtCustName">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="Label20" runat="server" Font-Bold="true" Text="Project Code :"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txtProjCode" CssClass="form-control" placeholder="Search Project Code" runat="server" OnTextChanged="txtjobno_TextChanged" Width="100%" AutoPostBack="true"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCponoList"
                                    TargetControlID="txtProjCode">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="Label21" runat="server" Font-Bold="true" Text="Project Name:"></asp:Label>
                            <div style="margin-top: 14px;">
                                <asp:TextBox ID="txtGST" CssClass="form-control" placeholder="Search Project Name " runat="server" OnTextChanged="txtGST_TextChanged" Width="100%" AutoPostBack="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ErrorMessage="Please Enter Job Number."
                                    ControlToValidate="txtGST" ValidationGroup="form1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionListCssClass="completionList"
                                    CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                    CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetGSTList"
                                    TargetControlID="txtGST">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>

                        <div class="col-md-1" style="margin-top: 36px">
                            <asp:LinkButton ID="btnrefresh" runat="server" OnClick="btnrefresh_Click" Width="100%" CssClass="form-control btn btn-warning"><i style="color:white" class="fa">&#xf021;</i> </asp:LinkButton>
                        </div>
                        <br />
                        <div class="card">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <div class="table">
                                        <br />
                                        <asp:GridView ID="MainGridLoad" runat="server" CellPadding="4" DataKeyNames="ProjectCode" Width="100%" OnRowCommand="MainGridLoad_RowCommand"
                                            OnRowDataBound="MainGridLoad_RowDataBound" CssClass="display table table-striped table-hover" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="20" HeaderText=" " HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnShowDtls" ToolTip="Send to Details" CommandName="ViewDetails" CommandArgument='<%# Eval("ProjectCode") %>'><i class="fa fa-plus-square" aria-hidden="true" style="font-size: 26px; color: blue;"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Project Code" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProjectCode" runat="server" Text='<%#Eval("ProjectCode")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("CustomerName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Project Name" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProjectName" runat="server" Text='<%#Eval("ProjectName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dispatch Sub Products" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubProdCount" runat="server" Text='<%#Eval("TotalRecords")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TotalQty Sub Products" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("TotalQTY")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inward Qty" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInwardQTY" runat="server" Text='<%#Eval("InwardQTY")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Outward Qty" HeaderStyle-CssClass="gvhead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQtyCount" runat="server" Text='<%#Eval("OutwardQty")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PDF File" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnPdfFile" ToolTip="Show drawings" CausesValidation="false" CommandName="PdfDownload" CommandArgument='<%# Eval("PdfFilePath") %>'><i class="fas fa-folder-open"  style="font-size: 26px;"></i></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <asp:Button ID="btnhist" runat="server" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupHistory" runat="server" TargetControlID="btnhist"
                        PopupControlID="PopupHistoryDetail" OkControlID="Closepophistory" />

                    <asp:Panel ID="PopupHistoryDetail" runat="server" CssClass="modelprofile1">
                        <div class="row container">
                            <div class="col-md-3"></div>
                            <div class="col-md-8">
                                <div class="profilemodel2">
                                    <div class="headingcls">
                                        <h4 class="modal-title">Production Details 
                                    <button type="button" id="Closepophistory" class="btnclose" style="display: inline-block;" data-dismiss="modal">Close</button></h4>
                                    </div>

                                    <br />
                                    <div class="body" style="margin-right: 10px; margin-left: 10px; padding-right: 1px; padding-left: 1px;">
                                        <div class="row">
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label4" runat="server" Font-Bold="true" CssClass="form-label">Job No:</asp:Label>
                                                <asp:TextBox ID="txtjobno" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label5" runat="server" Font-Bold="true" CssClass="form-label">Customer Name:</asp:Label>
                                                <asp:TextBox ID="txtcustomername" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label6" runat="server" Font-Bold="true" CssClass="form-label">Total Quantity:</asp:Label>
                                                <asp:TextBox ID="txttotalqty" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label16" runat="server" Font-Bold="true" CssClass="form-label">Inward QTY:</asp:Label>
                                                <asp:TextBox ID="txtinwardqty" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label12" runat="server" Font-Bold="true" CssClass="form-label">Pending QTY:</asp:Label>
                                                <asp:TextBox ID="txtpending" CssClass="form-control" placeholder="Enter Pending QTY" TextMode="Number" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" CssClass="form-label">Outward QTY:</asp:Label>
                                                <asp:TextBox ID="txtoutwardqty" CssClass="form-control" placeholder="Enter Outward QTY" TextMode="Number" runat="server"></asp:TextBox>
                                            </div>


                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label3" runat="server" Font-Bold="true" CssClass="form-label">Remarks:</asp:Label>
                                                <asp:TextBox ID="txtRemarks" CssClass="form-control" placeholder="Enter Remark" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-md-12" style="margin-top: 18px; text-align: center">

                                                <asp:LinkButton runat="server" ID="btnsendtoback" class="btn btn-warning" OnClick="btnsendtoback_Click" OnClientClick="hideButtons();">
                                                        <span class="btn-label">
                                                            <i class="fa fa-arrow-left"></i>
                                                        </span>
                                                       Save & Back
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="btnSendtopro" class="btn btn-success" OnClick="btnsave_Click" OnClientClick="hideButtons();">
                                                        <span class="btn-label">
                                                            <i class="fa fa-check"></i>
                                                        </span>
                                                       Save & Next
                                                </asp:LinkButton>
                                            </div>
                                            <script type="text/javascript">
                                                function hideButtons() {
                                                    document.getElementById('<%= btnsendtoback.ClientID %>').style.display = 'none';
                                                    document.getElementById('<%= btnSendtopro.ClientID %>').style.display = 'none';
                                                }
                                            </script>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-1"></div>
                        </div>
                    </asp:Panel>

                    <asp:Button ID="Button1" runat="server" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button1"
                        PopupControlID="PopupHistoryDetail1" OkControlID="Closepophistory1" />

                    <asp:Panel ID="PopupHistoryDetail1" runat="server" CssClass="modelprofile1" Style="display: none">
                        <div class="row container">
                            <div class="col-md-4"></div>
                            <div class="col-md-8">
                                <div class="profilemodel2">
                                    <div class="headingcls">
                                        <h4 class="modal-title">Warehouse Material Request Page
                                    <button type="button" id="Closepophistory1" class="btnclose" style="display: inline-block;" data-dismiss="modal">Close</button></h4>
                                    </div>

                                    <br />
                                    <div class="body" style="margin-right: 10px; margin-left: 10px; padding-right: 1px; padding-left: 1px;">
                                        <div class="row">
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="form-label">Stock/RM:</asp:Label>

                                                <asp:TextBox ID="txtRM" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="true" CssClass="form-label">Available Quantity:</asp:Label>

                                                <asp:TextBox ID="txtAvilableqty" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label8" runat="server" Font-Bold="true" CssClass="form-label">Size:</asp:Label>

                                                <asp:TextBox ID="txtsize" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label9" runat="server" Font-Bold="true" CssClass="form-label">Description:</asp:Label>

                                                <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-12 mb-3">
                                                <asp:Label ID="Label10" runat="server" Font-Bold="true" CssClass="form-label">Need QTY:</asp:Label>

                                                <asp:TextBox ID="txtneedqty" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-3" style="margin-top: 18px">
                                                    <asp:Button ID="Button2" OnClick="btnsave_Click" ToolTip="Save" CssClass="form-control btn btn-outline-primary m-2" runat="server" Text="Save" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Button ID="Button3" runat="server" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Button3"
                        PopupControlID="Panel1" OkControlID="Closepophistory2" />

                    <asp:Panel ID="Panel1" runat="server" CssClass="modelprofile1">
                        <div class="row container">
                            <div class="col-md-3"></div>
                            <div class="col-md-6">
                                <div class="profilemodel2">
                                    <div class="headingcls">
                                        <h4 class="modal-title">Drawing Files
                 <button type="button" id="Closepophistory2" class="btnclose" style="display: inline-block;" data-dismiss="modal">Close</button>
                                        </h4>
                                    </div>
                                    <br />
                                    <div class="body" style="margin-right: 10px; margin-left: 10px; padding-right: 1px; padding-left: 1px;">
                                        <div class="row">

                                            <asp:Repeater ID="rptImages" runat="server">
                                                <ItemTemplate>
                                                    <div class="col-md-6 col-12 mb-3">
                                                        <div class="image-item">
                                                            <!-- Display the image -->
                                                            <asp:ImageButton ID="ImageButtonfile2" ImageUrl="../Content1/img/Open-file2.png" runat="server" Width="30px" OnClick="ImageButtonfile2_Click" CommandArgument='<%# Eval("ID") %>' ToolTip="Open File" />
                                                            <asp:Label ID="Label14" runat="server" Font-Bold="true" Text="Drawing Name : " CssClass="form-label"></asp:Label>
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="true" Text='<%# Eval("FileName") %>' CssClass="form-label"></asp:Label>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-3"></div>
                        </div>
                    </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

</asp:Content>

