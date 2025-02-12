﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="OAList.aspx.cs" Inherits="SalesMarketing_OAList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.all.min.js"></script>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.min.css' />
    <script>
        function HideLabelerror(msg) {
            Swal.fire({
                icon: 'error',
                text: msg,

            })
        };
        function HideLabel(msg) {

            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 5000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                window.location.href = "OAList.aspx";
            })
        };
    </script>
    <%--<link href="../Content1/css/Griddiv.css" rel="stylesheet" />--%>
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



        /*Loader css*/

        #BodyMain {
            background-color: #030710;
            margin: 0px;
            height: 8vh;
        }

        #DivMain {
            position: relative;
            left: 50%;
            top: 50%;
            transform: translate(-50%, -50%);
            color: #031d51;
            text-align: center;
            padding: 10px;
        }

        #SpanMain {
            font-size: 36px;
            letter-spacing: 6px;
            text-transform: uppercase;
            line-height: 90%;
            position: relative;
            mix-blend-mode: difference;
            color: whitesmoke;
        }

        #DivMain::before {
            content: "";
            position: absolute;
            left: 0;
            top: 2px;
            width: 0;
            height: 92%;
            background-color: #ddc05b;
            animation: move 5s linear infinite;
            z-index: 0;
        }

        @keyframes move {
            0%, 100% {
                width: 0;
            }

            50% {
                width: 100%; 
            }
        }
        /*End*/
    </style>

    <script type="text/javascript">
        function showLoadingSpinner() {
            document.getElementById('loadingSpinner').style.display = 'block';
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
                            <h4 class="mt-4 "><b>ORDER ACCEPTANCE LIST</b></h4>
                        </div>
                        <div class="col-3 col-md-2 mt-4">
                            <asp:Button ID="btnCreate" CssClass="form-control btn btn-warning" OnClick="btnCreate_Click" runat="server" Text="Create" />
                        </div>
                    </div>
                    <hr />
                    <div>
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Customer Name :"></asp:Label>
                                <div style="margin-top: 14px;">
                                    <asp:TextBox ID="txtCustomerName" CssClass="form-control" placeholder="Search Company" runat="server" OnTextChanged="txtCustomerName_TextChanged" Width="100%" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Please Enter Company Name"
                                        ControlToValidate="txtCustomerName" ValidationGroup="form1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCustomerList"
                                        TargetControlID="txtCustomerName">
                                    </asp:AutoCompleteExtender>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Project Code:"></asp:Label>
                                <div style="margin-top: 14px;">
                                    <asp:TextBox ID="txtCpono" CssClass="form-control" placeholder="Search by Project Code" runat="server" OnTextChanged="txtCpono_TextChanged" Width="100%" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetCponoList"
                                        TargetControlID="txtCpono">
                                    </asp:AutoCompleteExtender>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Project Name:"></asp:Label>
                                <div style="margin-top: 14px;">
                                    <asp:TextBox ID="txtGST" CssClass="form-control" placeholder="Search by Project Name " runat="server" OnTextChanged="txtGST_TextChanged" Width="100%" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="Please Enter GST No."
                                        ControlToValidate="txtGST" ValidationGroup="form1" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionListCssClass="completionList"
                                        CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                        CompletionInterval="10" MinimumPrefixLength="1" ServiceMethod="GetGSTList"
                                        TargetControlID="txtGST">
                                    </asp:AutoCompleteExtender>
                                </div>
                            </div>
                            <div class="col-md-3 d-none">
                                <asp:Label ID="lblfromdate" runat="server" Font-Bold="true" Text="From Date :"></asp:Label>
                                <div style="margin-top: 14px;">
                                    <asp:TextBox ID="txtfromdate" Placeholder="Enter From Date" runat="server" TextMode="Date" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 d-none">
                                <asp:Label ID="lbltodate" runat="server" Font-Bold="true" Text="To Date :"></asp:Label>
                                <div style="margin-top: 14px;">
                                    <asp:TextBox ID="txttodate" Placeholder="Enter To Date" runat="server" TextMode="Date" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1 col-2" style="margin-top: 36px">
                                <asp:LinkButton ID="btnSearch" OnClick="btnSearch_Click" runat="server" Width="100%" CssClass="form-control btn btn-info"><i style="color:white" class="fa">&#xf002;</i> </asp:LinkButton>
                            </div>
                            <div class="col-md-1" style="margin-top: 36px">
                                <asp:LinkButton ID="btnrefresh" runat="server" OnClick="btnrefresh_Click" Width="100%" CssClass="form-control btn btn-warning"><i style="color:white" class="fa">&#xf021;</i> </asp:LinkButton>
                            </div>
                            <br />

                            <%--<div class="table-responsive text-center">--%>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-body">

                            <div class="table-responsive text-center">
                                <asp:GridView ID="GVPurchase" runat="server" CellPadding="4" DataKeyNames="id" PageSize="10" AllowPaging="true" Width="100%" OnRowDataBound="GVPurchase_RowDataBound"
                                    OnRowCommand="GVPurchase_RowCommand" OnPageIndexChanging="GVPurchase_PageIndexChanging" CssClass="display table table-striped table-hover" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OA No." HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="Pono" runat="server" Text='<%#Eval("Pono")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Project Code." HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="projectCode" runat="server" Text='<%#Eval("ProjectCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="CustomerName" runat="server" Text='<%#Eval("CustomerName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Project Name" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="projectName" runat="server" Text='<%# Eval("ProjectName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PDF File" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnPdf" ToolTip="Show drawings" CausesValidation="false" CommandName="PdfFiles" CommandArgument='<%# Eval("PdfFilePath") %>'><i class="fas fa-folder-open" style="font-size: 26px;"></i></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="PO File" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButtonfile5" ImageUrl="../Content1/img/Open-file2.png" runat="server" Width="30px" OnClick="ImageButtonfile5_Click" CommandArgument='<%# Eval("id") %>' ToolTip="Open File" />

                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <%--     <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:Label ID="username" runat="server" Text='<%#Eval("Username1")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="ACTION" ItemStyle-Width="120" HeaderStyle-CssClass="gvhead">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" Height="27px" ToolTip="Edit" CausesValidation="false" CommandName="RowEdit" Visible='<%# Eval("Status").ToString() == "0" ? true : false %>' CommandArgument='<%#Eval("ID")%>'><i class='fas fa-edit' style='font-size:24px;color: #212529;' onclick="showLoadingSpinner()"></i></asp:LinkButton>&nbsp;


                                                <asp:LinkButton ID="btnDelete" runat="server" Height="27px" ToolTip="Delete" CausesValidation="false" CommandName="RowDelete" OnClientClick="Javascript:return confirm('Are you sure to Delete?')" CommandArgument='<%#Eval("Pono")%>'><i class='fas fa-trash' style='font-size:24px;color: red;'></i></asp:LinkButton>&nbsp;
                                                <asp:LinkButton runat="server" ID="btnpdfview" ToolTip="View Order Acceptance PDF" CommandName="RowView" CommandArgument='<%# Eval("Pono") %>'><i class="fa fa-table"  style="font-size: 26px; color:green; "></i></i></asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="btnCancel" runat="server" Height="27px" ToolTip="Cancel the order" CausesValidation="false" CommandName="RowClose" OnClientClick="Javascript:return confirm('Are you sure to close this order?')" CommandArgument='<%#Eval("Pono")%>' Visible="false"><i class='fa fa-window-close' style='font-size:24px;color: #ffad46;'></i></asp:LinkButton>&nbsp;
                                                <asp:LinkButton runat="server" ID="btnSendtopro" ToolTip="Send to Production" CommandName="Sendtoproduction" Visible='<%# Eval("Status").ToString() == "0" ? true : false %>' OnClientClick="Javascript:return confirm('Are you sure to send order acceptance to production....?')" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-arrow-circle-right"  style="font-size: 26px; color:dodgerblue; "></i></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div id="loadingSpinner" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); z-index: 9999;">
                                <div id="BodyMain">
                                    <div id="DivMain">
                                        <span id="SpanMain">Loading..</span>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="GVPurchase" />
            </Triggers>

        </asp:UpdatePanel>

    </form>

</asp:Content>

