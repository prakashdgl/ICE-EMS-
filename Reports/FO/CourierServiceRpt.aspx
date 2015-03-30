﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/FO/FORptMaster.master" AutoEventWireup="true" CodeFile="CourierServiceRpt.aspx.cs" Inherits="Reports_FO_CourierServiceRpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="dev" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" Runat="Server">Courier Service Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
<link href="../../style.css" rel="stylesheet" type="text/css" />
    <link href="../../Admin/AdminStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="Scriptmanager1" runat="server" ></asp:ScriptManager>
    <div align="center">

 <h2>  Diary Details by Courier Service</h2> 
 <br />
 <asp:UpdatePanel ID="updatePanel1" runat="server" ><Triggers><asp:PostBackTrigger ControlID="btnView" /></Triggers><ContentTemplate>
<table><tr><td>Courier Service:
    <asp:DropDownList ID="ddlCourierService" CssClass="txtbox" runat="server" AutoPostBack="True">
    </asp:DropDownList>
    
    </td><td>Session:
        <asp:DropDownList ID="ddlSession" runat="server" CssClass="txtbox" AutoPostBack="True">
            <asp:ListItem Value="Win">Winter Examination</asp:ListItem>
            <asp:ListItem Value="Sum">Summer Examination</asp:ListItem>
              </asp:DropDownList>
        </td>
    <td>Year:
        <asp:TextBox ID="txtYear" runat="server" CssClass="txtbox" Width="95px"></asp:TextBox></td>
        <td>
        <asp:Button ID="btnView" runat="server" Text="View" CssClass="btnsmall" onclick="btnView_Click" />
          </td></tr>
           </table>
           </ContentTemplate>
           </asp:UpdatePanel>
           </div>
          <br />
 <div style="overflow: scroll; width:100%">
 <cr:crystalreportviewer ID="CourierServiceReport" 
                  runat="server" Width="100%" 
        BestFitPage="True" DisplayPage="true"  DisplayStatusbar="false" ToolPanelView="None"
       HasCrystalLogo="False" HasToggleGroupTreeButton="false" 
        BorderStyle="None" 
            AutoDataBind="True" Height="1039px" 
        ReportSourceID="CrystalReportSource1" EnableTheming="True" 
        oninit="CrystalReportViewer1_Init" />
        <cr:crystalreportsource ID="CrystalReportSource1" runat="server">
            <Report FileName="CourierServiceCrt.rpt">
            </Report>
        </cr:crystalreportsource>
</div>
   
</asp:Content>

