﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/FO/FORptMaster.master" AutoEventWireup="true" CodeFile="DiaryLetterRpt.aspx.cs" Inherits="Reports_FO_DiaryLetterRpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="dev" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" Runat="Server">Letter Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <link href="../../style.css" rel="stylesheet" type="text/css" />
    <link href="../../Admin/AdminStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="Scriptmanager1" runat="server" ></asp:ScriptManager>
    <div align="center">
  <h2>  Diary Details by Letters</h2>
  <br />
  <asp:UpdatePanel ID="updatePanel1" runat="server" ><Triggers><asp:PostBackTrigger ControlID="btnView" /></Triggers><ContentTemplate>
<table><tr><td>Member Type
    <asp:DropDownList ID="ddlLetterType" runat="server" CssClass="txtbox"  AutoPostBack="True" 
        onselectedindexchanged="ddlLetterType_SelectedIndexChanged">
        <asp:ListItem>ICE</asp:ListItem>
        <asp:ListItem>IM</asp:ListItem>
        <asp:ListItem>Department</asp:ListItem>
    </asp:DropDownList>
    </td><td>&nbsp;&nbsp; Session
        <asp:DropDownList ID="ddlSession" runat="server" CssClass="txtbox">
            <asp:ListItem Value="Sum">Summer Examination</asp:ListItem>
            <asp:ListItem Value="Win">Winter Examination</asp:ListItem>
              </asp:DropDownList>
        </td>
    <td>&nbsp;&nbsp; Year
        <asp:TextBox ID="txtYear" runat="server" CssClass="txtbox" Width="95px"></asp:TextBox>
       
         </td>
       </tr>
          <tr> <asp:Panel ID="panDepartmentName" runat="server"><td colspan="4" align="center">Department
              <asp:DropDownList ID="ddlDepartmentName" runat="server" CssClass="txtbox" AutoPostBack="True" 
                  onselectedindexchanged="ddlDepartmentName_SelectedIndexChanged">
    </asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
              </asp:Panel>
              </tr>
              <tr><td colspan="4" align="center"><asp:Label ID="lblImid" runat="server" Text="IMID:"> </asp:Label>
                  <asp:TextBox ID="txtIMID" runat="server" CssClass="txtbox" Width="95px"></asp:TextBox>  <asp:Button ID="btnView" runat="server" CssClass="btnsmall" Text="View" onclick="btnView_Click" /></td></tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
   <div style="overflow: scroll; width:100%">
   <cr:crystalreportviewer ID="DiaryType" 
                  runat="server" Width="100%" 
        BestFitPage="True" DisplayPage="true"  DisplayStatusbar="true" 
    ToolPanelView="None" HasCrystalLogo="False" HasToggleGroupTreeButton="false" 
        BorderStyle="None" 
            AutoDataBind="True" Height="1039px" 
        ReportSourceID="CrystalReportSource1" EnableTheming="True" 
                  oninit="CrystalReportViewer1_Init" />
        <cr:crystalreportsource ID="CrystalReportSource1" runat="server">
            <Report FileName="DiaryLetterCrt.rpt">
            </Report>
        </cr:crystalreportsource>



       



   </div>
</asp:Content>

