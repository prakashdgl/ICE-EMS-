﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Acc/Account.master" AutoEventWireup="true" CodeFile="AddApp3.aspx.cs" Inherits="Acc_AddApp3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="dev" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" Runat="Server">Add Application Forms-3
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
 <link rel="stylesheet" href="../style.css" type="text/css" charset="utf-8" />	
 <link href="../Admin/AdminStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="rightpanel2">

<div class="fromRegisterlbl"><h1 style="float:right; margin-right:50px;">&nbsp;&nbsp;Fee Master:&nbsp;<asp:DropDownList 
        ID="ddlFeeMaster" runat="server" CssClass="txtbox" Width="85px" 
        AutoPostBack="True" Height="16px" 
        onselectedindexchanged="ddlFeeMaster_SelectedIndexChanged"><asp:ListItem Value="Home" Text="Home" /><asp:ListItem Value="Overseas" Text="Overseas" /></asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblEnrolment" runat="server" ></asp:Label></h1><h1>Application Forms Entry:</h1></div>
<center>Session:&nbsp;&nbsp;<asp:DropDownList ID="ddlsession" runat="server" 
         AutoPostBack="true" CssClass="txtbox" 
        onselectedindexchanged="ddlsession_SelectedIndexChanged"><asp:ListItem Text="Summer Examination" Value="Sum"></asp:ListItem><asp:ListItem Text="Winter Examination" Value="Win"></asp:ListItem></asp:DropDownList>&nbsp;&nbsp;Year:&nbsp; <asp:TextBox ID="txtSession" runat="server" CssClass="txtbox" AutoPostBack="true" Width="80px" OnTextChanged="txtdevYearSeason_TextChanged"></asp:TextBox></center>
<table class="tbl"><tr><td>Diary No:&nbsp;&nbsp;</td><td>
    <asp:TextBox ID="txtDiaryNo" Width="100px" runat="server" CssClass="txtbox" 
        AutoPostBack="true" ontextchanged="txtDiaryNo_TextChanged" ></asp:TextBox><asp:Image ID="ibtnViewDairy" ImageUrl="~/images/dairycount.gif"  runat="server" AlternateText="Dairy" /></td>
<tr><td>IMID:&nbsp;&nbsp;</td><td><asp:Label ID="lblIMID"  runat="server"></asp:Label></td>
<td><asp:Label ID="lblIMName" runat="server" Font-Bold="true" ForeColor="Maroon"></asp:Label>&nbsp;&nbsp;</td></tr>
<tr>
    <td>
        Diary Date:</td><td><asp:Label ID="txtDiaryRcvDate" runat="server" Font-Bold="true"></asp:Label>
        &nbsp;&nbsp;&nbsp;Session:</td><td>
        <asp:Label ID="lblSessionHiddend" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    </tr>
</table><dev:PopupControlExtender ID="popupex" runat="server" Position="Center" OffsetX="-350" OffsetY="0" PopupControlID="pnlDairyCount" TargetControlID="ibtnViewDairy" ></dev:PopupControlExtender>
<asp:Panel ID="pnlDairyCount" runat="server" Width="350px" CssClass="pnlpopup" >
<div class="redsubtitle"><center>Application Form Count</center></div>
<table width="100%"><tr><td>Applications</td><td>Total Received</td><td>Total Submitted</td></tr>
<tr><td>Academic DD</td><td><asp:Label ID="lblADDRcv" ForeColor="White" runat="server" ></asp:Label></td><td><asp:Label ForeColor="White" ID="lblADDSub" runat="server" ></asp:Label></td></tr>
<tr><td>Others DD</td><td><asp:Label ID="lblODDRcv" runat="server"  ForeColor="White"></asp:Label></td><td><asp:Label ID="lblODDSub"  ForeColor="White"  runat="server" ></asp:Label></td></tr>
<tr><td>Admission </td><td><asp:Label ID="lblAdmissionRcv" runat="server"  ForeColor="White"></asp:Label></td><td><asp:Label ID="lblAdmissionSub" ForeColor="White"  runat="server" ></asp:Label></td></tr>
<tr><td>Examination</td><td><asp:Label ID="lblExamRcv" runat="server"  ForeColor="White"></asp:Label></td><td><asp:Label ID="lblExamSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>ITI </td><td><asp:Label ID="lblITIRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblITISub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Auto-CAD</td><td><asp:Label ID="lblCADRcv" runat="server" ForeColor="White"></asp:Label></td><td><asp:Label ID="lblCADSub" runat="server" ForeColor="White"></asp:Label></td></tr>
<tr><td>Others Form</td><td><asp:Label ID="lblOthersFormRcv" runat="server"  ForeColor="White"></asp:Label></td><td><asp:Label ID="lblOthersFormSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Provisional</td><td><asp:Label ID="lblProvisionalRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblProvisionalSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Final Pass</td><td><asp:Label ID="lblFinalPassRcv" runat="server"  ForeColor="White"></asp:Label></td><td><asp:Label ID="lblFinalPassSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Re-Checking</td><td><asp:Label ID="lblReCheckingRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblReCheckingSub" runat="server"  ForeColor="White" ></asp:Label></td></tr>
<tr><td>Duplicate Docs</td><td><asp:Label ID="lblDuplicateRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblDuplicateSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Project DD</td><td><asp:Label ID="lblProjectRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblProjectSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Project ProformaC</td><td><asp:Label ID="lblProformaCRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblProformaCSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Project ProformaB</td><td><asp:Label ID="lblProformaBRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblProformaBSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Membership DD</td><td><asp:Label ID="lblMembershipRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblMembershipSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Books DD</td><td><asp:Label ID="lblBooksRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblBooksSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
<tr><td>Prospectus DD</td><td><asp:Label ID="lblProsRcv" runat="server" ForeColor="White" ></asp:Label></td><td><asp:Label ID="lblProsSub" runat="server" ForeColor="White"  ></asp:Label></td></tr>
</table>
</asp:Panel>
<center><asp:Label ID="lblExceptionOK" runat="server" Font-Bold="true"></asp:Label><asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label></center><hr />


<center>
<table ID="tblAdd2" runat="server" width="90%">
<tr><td align="center">Select Application Form Type:
<asp:DropDownList ID="ddlForms" runat="server" AutoPostBack="True" CssClass="txtbox" 
            onselectedindexchanged="ddlForms_SelectedIndexChanged">
<%--<asp:ListItem Value="ExamCenter" Text="Change of Exam Center" />
<asp:ListItem Value="Rechecking" Text="Re-Checking Subjects" />

<asp:ListItem Value="FinalPass" Text="Final Marksheet" />
<asp:ListItem Value="ProvisionalCertificate" Text="Provisional Certificate" ></asp:ListItem>
   
    <asp:ListItem Value="IDCard" Text="ID Card"></asp:ListItem>
    <asp:ListItem Value="AdmitCard" Text="Admit Card"></asp:ListItem>
    <asp:ListItem Value="MarksStatement" Text="Marks Statement"></asp:ListItem>
    <asp:ListItem Value="MembershipCertificate" Text="Membership Certificate"></asp:ListItem>--%>
    </asp:DropDownList></td><td>Membership No.<asp:TextBox ID="txtMem" runat="server" CssClass="txtbox"></asp:TextBox></td><td>
    <asp:Button ID="btnView" Text="View" runat="server" CssClass="btnsmall" 
        onclick="btnView_Click"/></td></tr>
        <tr><td>&nbsp;&nbsp;&nbsp;&nbsp; Fee:&nbsp;&nbsp;<asp:Label ID="lblFee" runat="server" ForeColor="Maroon"></asp:Label>
           </td><td>Seesion:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlFeeTypeSession" runat="server" 
                    AutoPostBack="True" CssClass="txtbox"  onselectedindexchanged="ddlFeeTypeSession_SelectedIndexChanged" ></asp:DropDownList></td></tr></table>
</center><br /><asp:Panel ID="pnlMain" runat="server">
<asp:Panel ID="PnlMembership" runat="server" CssClass="panelCenter" >
<table class="tbl" runat="server" id="tblDetails" width="100%"><tr>
    <td ><b>Student Name:</b><asp:Label ID="lblName" runat="server" 
            ForeColor="Maroon"></asp:Label>
    </td>
    <td><b>Father's Name:</b><asp:Label ID="lblFName" runat="server"></asp:Label>
    </td></tr>
<tr><td><b>Date Of Birth:</b><asp:Label ID="lblDOB" runat="server"></asp:Label>
    <asp:Label ID="lblLvl" runat="server" Visible="false"></asp:Label>
    </td>
    <td ><b>Course:</b><asp:Label ID="lblCourse" runat="server" 
            ForeColor="Maroon"></asp:Label>
        <asp:Label ID="lblPart" runat="server" ForeColor="Maroon"></asp:Label>
        &nbsp;
        <asp:Label ID="lblStream" runat="server" ForeColor="Maroon"></asp:Label>
    </td></tr><tr><td colspan="2" align="center"> <b>Application No:</b><asp:Label ID="lblappno" runat="server" Font-Bold="True" ForeColor="Green"></asp:Label></td></tr></table>
</asp:Panel><br /><asp:Label ID="lbllavel" runat="server" Visible="False"></asp:Label>
       <br />
     <center><asp:Label ID="lblAmount" runat="server" Visible="false"></asp:Label><br /><asp:Button ID="btnSubmit" runat="server" Text="Add Record" CssClass="btnsmall" 
             onclick="btnSubmit_Click" /></center></asp:Panel>
             <script>
                 function toggleA1w(showHideDiv, switchImgTag) {
                     var ele = document.getElementById(showHideDiv);
                     var imageEle = document.getElementById(switchImgTag);
                     var imageEle = document.getElementById(switchImgTag);
                     if (ele.style.display == "block") {
                         ele.style.display = "none";
                         imageEle.innerHTML = '<img src="../images/plus.png">';
                     }
                     else {
                         ele.style.display = "block";
                         imageEle.innerHTML = '<img src="../images/minus.png">';
                     }
                 }
    </script>
        <div class="togalfees" style="width:100%">
    <div class="headerDivImgfees">
        &nbsp;&nbsp;&nbsp;&nbsp;<a id="A12" href="javascript:toggleA1w('Div12', 'A12');"><img src="../images/minus.png" alt="Show"></a>
</div><br /><br />
<div id="Div12" style="display:block;">
 <input id="scrollPos2" runat="server" type="hidden" value="0" /><div id="divdatagrid2" style="width: 100%; overflow:scroll; height:200px"><asp:GridView ID="GridAppTable" runat="server" BackColor="#DEBA84" AutoGenerateColumns="true" OnRowDataBound="GridAppTable_RowDataBound"
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="5" 
        CellSpacing="5" Width="100%">
        <Columns>
        </Columns>
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
    </asp:GridView></div></div>
    <asp:Panel ID="pnlSpace" runat="server" Height="300px"></asp:Panel>
</div>
    
    </div>
</asp:Content>