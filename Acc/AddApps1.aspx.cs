﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

public partial class Acc_AddApps : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Conn"]);
    SqlCommand cmd;
    DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
    private DataRow dr2;
    Student st = new Student();
    SessionDuration sd;
    string strCompSession, strimid = "";
    ClsExamForm p2 = new ClsExamForm();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Server.HtmlEncode(Request.Cookies["MyLogin"]["PWD"]) == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                lblUnderAge.Text = "NO";
                datastructure(); //lbtnCheckDues.Visible = false;
                maikal dev = new maikal();
                int se = dev.chksession();
                if (se == 0) ddlsession.SelectedValue = "Sum";
                else ddlsession.SelectedValue = "Win";
                txtSession.Text = DateTime.Now.Year.ToString();
                lblSessionHiddend.Text = ddlsession.SelectedValue.ToString() + "" + txtSession.Text.ToString();
                allvisisblefalse();
                lblFeeLevel.Text = getcurrentfees();
                lblHiddendStream.Text = "Tech"; ddlPart.SelectedValue = "PartI"; ddlCourse.SelectedValue = "Civil";
                lblStreamDDL.Text = "Technician Examination.";
                if (txtDiaryRcvDate.Text == "")
                    ddlsession.Focus();
                else ddlAppType.Focus();
                con.Close(); con.Dispose();
            }
        }
        catch (NullReferenceException ex)
        {
            Response.Redirect("../Login.aspx");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        con.Dispose();
    }
    protected void txtdevYearSeason_TextChanged(object sender, EventArgs e)
    {
        con.Close(); con.Open();
        txtIMID.Text = ""; txtDiaryNo.Text = "";
        lblSessionHiddend.Text = ddlsession.SelectedValue.ToString() + "" + txtSession.Text.ToString();
        esngn();
        con.Close(); con.Dispose();
        txtIMID.Focus();
    }
    protected void ddldevExamSeason_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Close(); con.Open();
        txtIMID.Text = ""; txtDiaryNo.Text = "";
        lblSessionHiddend.Text = ddlsession.SelectedValue.ToString() + "" + txtSession.Text.ToString();
        esngn();
        con.Close(); con.Dispose();
        txtSession.Focus();
    }
    protected void txtIMID_TextChanged(object sender, EventArgs e)
    {
        okk(txtIMID.Text.ToString());
        ddlAppType.SelectedIndex = 0;
        txtDiaryNo.Text = ""; txtDiaryRcvDate.Text = "";
        txtDiaryNo.Focus();
    }
    protected void txtDiaryNo_TextChaged(object sender, EventArgs e)
    {
        con.Close(); con.Open();
        dtinfo.DateSeparator = "/";
        dtinfo.ShortDatePattern = "dd/MM/yyyy";
        cmd = new SqlCommand("select IMID from DiaryEntry where DiaryNo='" + txtDiaryNo.Text.ToString() + "' and ExamSession='" + lblSessionHiddend.Text.ToString() + "' and Status='Open'", con);
        string damount = Convert.ToString(cmd.ExecuteScalar());
        if (damount.ToString() == "")
        {
            lblExceptionOK.Text = "Invalid Diary No, Please Insert Valid Diary No.";
            lblExceptionOK.ForeColor = System.Drawing.Color.Red;
            lblExceptionOK.Font.Bold = true;
            txtDiaryNo.Focus();
        }
        else
        {
            if (txtIMID.Text == damount.ToString())
            {
                lblExceptionOK.Text = damount.ToString();
                cmd = new SqlCommand("select Date from DiaryEntry where DiaryNo='" + txtDiaryNo.Text.ToString() + "' and ExamSession='" + lblSessionHiddend.Text.ToString() + "'", con);
                string dat = Convert.ToString(cmd.ExecuteScalar());
                txtDiaryRcvDate.Text = Convert.ToDateTime(dat.ToString()).ToString("dd/MM/yyyy");
                showcount(lblSessionHiddend.Text.ToString(), txtDiaryNo.Text.ToString());
                ddlAppType.Focus();
            }
            else
            {
                lblExceptionOK.Text = "Invalid Diary No. for " + lblIMName.Text.ToString();
                lblExceptionOK.ForeColor = System.Drawing.Color.Red;
                lblExceptionOK.Font.Bold = true;
                txtDiaryNo.Focus();
            }
        }
        con.Close();
        con.Dispose();
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAddApproveVisible.Enabled = true;
            con.Close(); con.Open();
            cleanref();
            allvisisblefalse();
            getAdmissionFee();
            examinationfee();
            if (txtIMID.Text == "" | txtDiaryNo.Text == "" | txtDiaryRcvDate.Text == "")
            {
                lblExceptionOK.Text = "Please Insert IM and Diary Details.";
            }
            else
            {
                panelRightBox.Visible = true;
                if (ddlAppType.SelectedValue.ToString() == "ITI")
                {
                    if (Convert.ToInt32(lblITIRcv.Text) > Convert.ToInt32(lblITISub.Text))
                    {
                        lblExceptionCount.Text = "";
                        PanelProfile.Visible = true;
                        lblExamFee.Text = "0"; lblITISerialNo.Text = serialno("ITI".ToString());
                        PanelITI.Visible = true;
                        lblITISerialNo.Visible = true;
                        chkAddwithAdmisiosn.Visible = true;
                        lblFeesType.Text = "ITI Examination";
                        lblFormType.Text = lblITISerialNo.Text + "ITI,";
                        lbllavel.Text = "2";
                    }
                    else lblExceptionCount.Text = "All ITI Form Submitted";
                }
                else if (ddlAppType.SelectedValue.ToString() == "Composite")
                {
                    PanelComposite.Visible = true;
                    lbllavel.Text = "3";
                    lblFeesStore.Text = lblCompositeFeesFromExam.Text;
                    lblFeesType.Text = "Composite Fees";
                }
                else if (ddlAppType.SelectedValue.ToString() == "Subscription")
                {
                    PanelSubscriptin.Visible = true;
                    lblFeesType.Text = "Annual Subscription"; panelAnnualFromExam.Visible = true;
                    lbllavel.Text = "3"; lblFeesStore.Text = lblAnnualSubscriptin.Text;
                }
                else if (ddlAppType.SelectedValue.ToString() == "Exmp")
                {
                    if (Convert.ToInt32(lblOthersFormRcv.Text) > Convert.ToInt32(lblOthersFormSub.Text))
                    {
                        chkAddwithAdmisiosn.Visible = true;
                        PanelExamFee.Visible = true; panelRightBox.Visible = true;
                        lbllavel.Text = "1";
                    }
                    else lblExceptionCount.Text = "All Form Submitted.";
                }
                lblserialNo.Text = esngn();
                lbtnCheckDues.Text = "Check Duplicate"; lblExceptionDateOfBirth.Text = "";
            }
        }
        catch (FormatException ex)
        {
            lblExceptionCheck.Text = ex.ToString();
        }
        catch (SqlException ex)
        {
            lblExceptionCheck.Text = ex.ToString();
        }
        catch (Exception ex)
        {
            lblExceptionCheck.Text = ex.ToString();
        }
        finally
        {
            con.Close(); con.Dispose();
            ddlAppType.Focus();
        }
    }
    protected void txtEnrolID_TextChanged(object sender, EventArgs e)
    {
        dtinfo.DateSeparator = "/";
        dtinfo.ShortDatePattern = "dd/MM/yyyy";
        lblenrolStatus.Text = "";
        lblExceptionEnrolID.Text = "";
        if (chkAddwithAdmisiosn.Checked == true)
        {
            addwithadmission();
        }
        else if (chkAddwithAdmisiosn.Checked == false)
        {
            lblFullCourse.Visible = true; lblFulldName.Visible = true; lblExceptionCheck.Visible = true;
            lblExceptionEnrolID.Visible = true;
            lblExceptionCheck.Text = ""; lblExceptionAppTable.Text = "";
            getstr = st.status(txtEnrolID.Text.ToString());
            try
            {
                lblEnrolment.Text = txtEnrolID.Text.ToString();
                if (getstr[0].ToString() == "Exception")
                {
                    lblExceptionEnrolID.Text = "Membership No Not Found" + getstr[0].ToString() + "" + getstr[1].ToString();
                    lblFullCourse.Text = ""; lblFulldName.Text = "";
                }
                else if (getstr[0].ToString() == "No")
                {
                    lblExceptionEnrolID.Text = "Membership No Not Found" + getstr[0].ToString() + "" + getstr[1].ToString();
                    lblFullCourse.Text = ""; lblFulldName.Text = "";
                }
                else if (getstr[0].ToString() != "No" & getstr[0].ToString() != "Exception")
                {
                    if (txtEnrolID.Text == "")
                    {
                        lblExceptionEnrolID.Text = "Please Enter Membership No.";
                        lblExceptionEnrolID.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        con.Close(); con.Open();
                        lblserialNo.Text = esngn();
                        cmd = new SqlCommand("select Student.Name,Student.FName,Student.IMID,Student.AnnualSubscription, ExamCurrent.CompositeStatus,ExamCurrent.Part,ExamCurrent.Course,ExamCurrent.Stream,ExamCurrent.ExamStatus,ExamCurrent.CompositeStatus,ExamCurrent.Session,ExamCurrent.CourseID,ExamCurrent.SessionDuration,ExamCurrent.EnrollStatus,ExamCurrent.CourseStatus,ExamCurrent.CourseRemarks from ExamCurrent inner join Student on ExamCurrent.SID=Student.SID where ExamCurrent.SId='" + txtEnrolID.Text.ToString() + "'", con);
                        SqlDataReader reader;
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtName.Text = reader["Name"].ToString();
                            txtFName.Text = reader["FName"].ToString();
                            strimid = reader["IMID"].ToString();
                            lblASF.Text = reader["AnnualSubscription"].ToString();
                            // lblcomStatus.Text = reader["CompositeStatus"].ToString();
                            lblpart.Text = reader["Part"].ToString();
                            lblCourse.Text = reader["Course"].ToString();
                            lblHiddendStream.Text = reader["Stream"].ToString();
                            ddlPart.SelectedValue = lblpart.Text.ToString();
                            ddlCourse.SelectedValue = lblCourse.Text.ToString();
                            strCompSession = reader["Session"].ToString();
                            lblExamStatus.Text = reader["ExamStatus"].ToString();
                            // lblenrolStatus.Text = reader["EnrollStatus"].ToString();
                            lblPartIISID.Text = reader["CourseStatus"].ToString();
                            lblFulldName.Text = txtName.Text.ToString() + " s/o " + txtFName.Text.ToString();
                            lblFullCourse.Text = lblHiddendStream.Text.ToString() + ", " + ddlCourse.SelectedValue.ToString() + "- " + ddlPart.SelectedValue.ToString();
                            txtBirth.Text = getstr[4].ToString();
                        }
                        else
                        {
                            lblExceptionEnrolID.Text = "Please Enter Correct Membership No.";
                            lblExceptionEnrolID.ForeColor = System.Drawing.Color.Red;
                            lblException.Attributes.Add("class", "error");
                        }
                        reader.Close();
                        if (st.isSessionExp(txtEnrolID.Text.ToString(), ddlCourse.SelectedValue.ToString(), ddlPart.SelectedValue.ToString(), lblSessionHiddend.Text.ToString()))
                        {
                            lblExceptionEnrolID.Text = "Course Duration Expired.";
                            lblExceptionEnrolID.ForeColor = System.Drawing.Color.Red;
                            // To Disable Add Appliction, uncomment below line
                            btnAddApproveVisible.Enabled = false;
                        }
                        else if (strimid.ToString() == txtIMID.Text.ToString())
                        { //s1

                            lblFeeLevel.Text = getstr[2].ToString();
                            ddlCourse.Enabled = false; ddlPart.Enabled = false;
                            getAdmissionFee();
                            examinationfee();
                            if (ddlAppType.SelectedValue.ToString() == "Composite" )
                            { //s2
                                lblCompositeFeesFromExam.Text = "0";
                                if (ddlPart.SelectedValue == "PartII" || ddlPart.SelectedValue == "SectionB")
                                { //s3
                                    FeeMaster fm = new FeeMaster();
                                    con.Close(); con.Open();
                                    string[] compstatus = checkCompositeFees(txtEnrolID.Text.ToString());
                                    if (compstatus[1] == "NotSubmitted")
                                    {
                                        string lvlssn = fm.sessionid(strCompSession.ToString());
                                        string lvlcnt = fm.sessionid(lblSessionHiddend.Text.ToString());
                                        if (compstatus[2].ToString() == "Regular")
                                        {
                                            lblCompositeFeesFromExam.Text = compstatus[0].ToString();
                                            lblFeesType.Text = lblFeesType.Text.ToString() + " CompositeFees";
                                            lblExceptionEnrolID.Text = "Composite Fees Dues,";
                                            PanelComposite.Visible = true;
                                        }
                                        else if (compstatus[2].ToString() == "Direct")
                                        {
                                            if (lblCompositeDuration.Text.ToString() == "6")
                                            {
                                                if (Convert.ToInt32(lvlcnt) > Convert.ToInt32(lvlssn))
                                                {
                                                    lblCompositeFeesFromExam.Text = compstatus[0].ToString();
                                                    lblFeesType.Text = lblFeesType.Text.ToString() + " CompositeFees";
                                                    PanelComposite.Visible = true;
                                                    lblExceptionEnrolID.Text = lblExceptionEnrolID.Text = "Composite Fees Dues,";
                                                }
                                                else if (Convert.ToInt32(lvlssn) == Convert.ToInt32(lvlcnt))
                                                {
                                                    lblCompositeFeesFromExam.Text = "0";
                                                }
                                            }
                                            else if (lblCompositeDuration.Text.ToString() == "12")
                                            {
                                                string subsw = lvlssn.Substring(2, 1);
                                                string subyr = lvlssn.Substring(0, 2);
                                                string newssn = "";
                                                int intnewyr = Convert.ToInt32(subyr) + 1;
                                                newssn = intnewyr.ToString() + "" + subsw.ToString();
                                                if (Convert.ToInt32(lvlcnt) >= Convert.ToInt32(newssn))
                                                {
                                                    PanelComposite.Visible = true;
                                                    lblCompositeFeesFromExam.Text = compstatus[0].ToString();
                                                    lblExceptionEnrolID.Text = lblExceptionEnrolID.Text = "Composite Fees Dues,";
                                                }
                                                else if (Convert.ToInt32(lvlcnt) < Convert.ToInt32(newssn))
                                                {
                                                    lblCompositeFeesFromExam.Text = "0";
                                                }
                                            }
                                        }
                                    } //s3  // composite check if end
                                    else
                                    {
                                        PanelComposite.Visible = false;
                                    }
                                } //s2
                                else
                                {
                                    lblLateFeeChargedApptable.Text = "0";
                                    lblCompositeFeesFromExam.Text = "0";
                                    lblAnnualFeesFromExam.Text = "0";
                                }
                            } //s1
                            else
                            {
                                lblLateFeeChargedApptable.Text = "0";
                                lblCompositeFeesFromExam.Text = "0";
                                lblAnnualFeesFromExam.Text = "0";
                            }
                            if (ddlAppType.SelectedValue == "Subscription")
                            {
                                string sen = lblASF.Text.ToString();
                                string sin = sen.Substring(0, 1);
                                string syr = sen.Substring(5, 2);
                                string senp = lblSessionHiddend.Text.ToString();
                                string sinp = senp.Substring(0, 1);
                                string syrp = senp.Substring(5, 2);
                                //lblChangeFeeLavel for partII Passed.
                                if (Convert.ToInt32(syr) >= Convert.ToInt32(syrp) || (lblenrolStatus.Text == "NotSubmitted"))
                                {
                                    panelAnnualFromExam.Visible = false;
                                    lblAnnualFeesFromExam.Text = "0";
                                    lblAnnualYear.Text = "0";
                                }
                                else if ((Convert.ToInt32(syrp) - Convert.ToInt32(syr) == 1) && (sinp == "S") && (sin == "W"))
                                {
                                    panelAnnualFromExam.Visible = false;
                                    lblAnnualFeesFromExam.Text = "0";
                                    lblAnnualYear.Text = "0";
                                }
                                else if (syr != syrp)
                                {
                                    int yrdef = Convert.ToInt32(syrp) - Convert.ToInt32(syr);
                                    int anfee = Convert.ToInt32(lblAnnualSubscriptin.Text);
                                    lblAnnualYear.Text = yrdef.ToString(); panelAnnualFromExam.Visible = true;
                                    lblAnnualFeesFromExam.Text = (yrdef * anfee).ToString();
                                    lblExceptionEnrolID.Text = lblExceptionEnrolID.Text.ToString() + " Annual Subscription Dues,";
                                }
                                btnAddApproveVisible.Enabled = true;
                            }
                            else
                            {
                                lblAnnualFeesFromExam.Text = "0";
                            }
                            chkduplicateform();
                        }
                        else
                        {
                            lblExceptionEnrolID.Text = "This Membership is Not Associated to IMID: " + txtIMID.Text.ToString();
                            btnAddApproveVisible.Enabled = false;
                        }
                        con.Close();
                    }
                }
                txtEnrolID.Focus();
            }
            catch (NullReferenceException ex)
            {
                lblExceptionEnrolID.Text = "Invalid Membership No., Please Refresh page and Reinsert Membership No.";
                txtEnrolID.Focus();
            }
            finally
            {
                con.Dispose();
            }
        }
    }
    private string[] checkCompositeFees(string sid)
    {
        string[] comp = new string[3];
        cmd = new SqlCommand("select Amount,Status,Type from CompositeFees where SID='" + sid + "' and SessionID in(select MAX(SessionID) from CompositeFees where SID='" + sid + "')", con);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            comp[0] = reader["Amount"].ToString().TrimEnd('0').TrimEnd('.');
            comp[1] = reader["Status"].ToString();
            comp[2] = reader["Type"].ToString();
        }
        else
        {
            comp[0] = "NotFound";
            comp[1] = "NotFound";
            comp[2] = "NotFound";
        }
        return comp;
    }
    protected void ddlPart_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Conn"].ToString());
        con.Close(); con.Open();
        if (ddlPart.SelectedValue.ToString() == "PartI")
        {
            lblHiddendStream.Text = "Tech";
            getAdmissionFee();
            lblStreamDDL.Text = "Technician Examination.";
        }
        else if (ddlPart.SelectedValue.ToString() == "PartII")
        {
            lblHiddendStream.Text = "Tech";
            getAdmissionFee();
            lblStreamDDL.Text = "Technician Examination.";
        }
        else if (ddlPart.SelectedValue.ToString() == "SectionA")
        {
            lblHiddendStream.Text = "Asso";
            getAdmissionFee();
            lblStreamDDL.Text = "Associate Examination";
        }
        else if (ddlPart.SelectedValue.ToString() == "SectionB")
        {
            lblHiddendStream.Text = "Asso";
            getAdmissionFee();
            lblStreamDDL.Text = "Associate Examination";
        }
        examinationfee();
        con.Close(); con.Dispose();
        ddlPart.Focus();
    }
    protected void ddlCourse_SeelctedIndexchanged(object sender, EventArgs e)
    {
        con.Close(); con.Open();
        examinationfee();
        getAdmissionFee();
        ddlCourse.Focus();
        con.Close(); con.Dispose();
    }
    protected void lblExampNo_TextChanged(object sender, EventArgs e)
    {
        lblExmpNo.Focus();
    }
    protected void txtDate_TechChanged(object sender, EventArgs e)
    {
        try
        {
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            DateTime dt = Convert.ToDateTime(txtBirth.Text, dtinfo);
            lblExceptionDateOfBirth.Text = "";

            int year = DateTime.Now.Year - dt.Year;
            if (DateTime.Now.Month < dt.Month || DateTime.Now.Month == dt.Month && DateTime.Now.Day < dt.Day)
                --year;
            if (ddlPart.SelectedValue.ToString() == "PartI" | ddlPart.SelectedValue.ToString() == "PartII")
            {
                if (year < 16)
                {
                    lblExceptionDateOfBirth.Text = "Can't under 16 year old  for Technician Examination";
                    lblUnderAge.Text = "YES";
                }
                else
                {
                    lblExceptionDateOfBirth.Text = year.ToString() + "th Year Old.";
                    lblUnderAge.Text = "NO";
                }
            }
            else if (ddlPart.SelectedValue.ToString() == "SectionA" | ddlPart.SelectedValue.ToString() == "SectionB")
            {
                if (year < 18)
                {
                    lblExceptionDateOfBirth.Text = "Can't under 18 year old  for Associate Examination";
                    lblUnderAge.Text = "YES";
                }
                else
                {
                    lblExceptionDateOfBirth.Text = year.ToString() + "th Year Old.";
                    lblUnderAge.Text = "NO";
                }
            }
            lbtnCheckDues.Focus();
        }
        catch (FormatException ex)
        {
            lblExceptionDateOfBirth.Text = "Invalid Date of Birth. " + txtBirth.Text.ToString();
            txtBirth.Text = "";
        }
        txtBirth.Focus();
    }
    protected void chkAddWithAdmission_OnCheckChanged(object sender, EventArgs e)
    {
        if (chkAddwithAdmisiosn.Checked == true)
        {
            lblEnrolName.Text = "Admission SN.";
        }
        else
        {
            lblEnrolName.Text = "Membership.";
            txtEnrolID.Text = "";
            txtName.Text = ""; txtFName.Text = ""; txtBirth.Text = "";
            lblFullCourse.Text = ""; lblFulldName.Text = ""; lblExceptionEnrolID.Text = "";
        }
    }
    protected void txtName_TextChanged(object sender, EventArgs e)
    {
        if (ddlPart.SelectedValue.ToString() == "")
        {
            lblExceptionCheck.Text = "Select Part";
        }
        ddlPart.Focus();
    }
    decimal grdtotal = 0, latetotal = 0; int adno = 0, tono = 0;
    protected void GridAppTable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        dtinfo.DateSeparator = "/";
        dtinfo.ShortDatePattern = "dd/MM/yyyy";
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = "Father's Name";
            e.Row.Cells[3].Text = "Serial No.";
            e.Row.Cells[6].Text = "Late Fee";
            e.Row.Cells[7].Text = "ExmpFee";
            e.Row.Cells[8].Text = "Membership";
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text != "&nbsp;" || e.Row.Cells[2].Text != "")
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString("dd/MM/yyyy");
            e.Row.Cells[5].Text = e.Row.Cells[5].Text.TrimEnd('0').TrimEnd('.');
            e.Row.Cells[6].Text = e.Row.Cells[6].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[7].Text = e.Row.Cells[7].Text.TrimEnd('0').TrimEnd('.');
            e.Row.Cells[9].Text = e.Row.Cells[9].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[10].Text = e.Row.Cells[10].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[11].Text = e.Row.Cells[11].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[12].Text = e.Row.Cells[12].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[13].Text = e.Row.Cells[13].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[14].Text = e.Row.Cells[14].Text.TrimEnd('0').Trim('.');
            e.Row.Cells[15].Text = e.Row.Cells[15].Text.TrimEnd('0').Trim('.');
            decimal rowtatal = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
            decimal latefee = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "LateFee"));
            string strexamtotal = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "FormType"));
            if (strexamtotal == "Admission")
                adno = adno + 1;
            tono = tono + 1;
            grdtotal = grdtotal + rowtatal;
            latetotal = latetotal + latefee;
        }
        lblToAmtFo.Text = grdtotal.ToString();
        lblAdmiSnFO.Text = adno.ToString();
        lblExamSnFo.Text = (tono - adno).ToString();
        lblToLateFo.Text = latetotal.ToString();
    }
    protected void GridDuplicacy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (GridDuplicacy.Rows.Count > 0)
            GridDuplicacy.Rows[0].Cells[1].Focus();
    }
    private void okk(string strid)   // Check IMID.
    {
        con.Close(); con.Open();
        lblExceptionOK.Text = "";
            cmd = new SqlCommand("select Active,Name from Member where ID='" + strid + "'", con);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                lblIMStatus.Text = reader[0].ToString();
                lblIMName.Text = reader[1].ToString();
            }
            else
            {
                txtIMID.Text = "Invalid ID";
                lblExceptionOK.Text = "Please Insert Valid IM ID.";
            }
            if (lblIMStatus.Text == "yes")
            {
                lblExceptionOK.Text = "";
            }
            else if (lblIMStatus.Text == "no")
            {
                lblExamFee.Text = "0"; txtITIFees.Text = "0";
                lblExceptionOK.Text = "IM Status is disactive, Do not add new Membership form";
                lblExceptionOK.ForeColor = System.Drawing.Color.Red;
            }
        con.Close();
        con.Dispose();
    }
    private void datastructure()
    {
        DataTable dtDatas = new DataTable();
        dtDatas.Columns.Add("IMID");
        dtDatas.Columns.Add("AppNo");
        dtDatas.Columns.Add("Stream");
        dtDatas.Columns.Add("Course");
        dtDatas.Columns.Add("Part");
        dtDatas.Columns.Add("Name");
        dtDatas.Columns.Add("FatherName");
        dtDatas.Columns.Add("DOB");
        dtDatas.Columns.Add("DiaryNo");
        dtDatas.Columns.Add("Session");
        dtDatas.Columns.Add("Date");
        dtDatas.Columns.Add("FormType");
        dtDatas.Columns.Add("Amount");
        dtDatas.Columns.Add("LateFee");
        dtDatas.Columns.Add("Enrolment");
        ViewState["dtDatas"] = dtDatas;
    }
    protected void lbtnCheckDuplicate_Onclick(object sender, EventArgs e)
    {
        chkDuplicate();
    }
    private void addwithadmission()
    {
        try
        {
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            con.Close(); con.Open();
            lblExceptionEnrolID.Text = ""; lblFullCourse.Text = ""; lblFulldName.Text = "";
            int i = 0; string apno = "";
            if (txtEnrolID.Text.ToString() == "")
            {
                lblExceptionEnrolID.Text = "Please Insert Serial No.";
            }
            else
            {
                string sn = txtEnrolID.Text.ToString() + "NewAdmission,";
                cmd = new SqlCommand("select AppNo from AppRecord where FormType like '%" + sn.ToString() + "%'  and Session='" + lblSessionHiddend.Text.ToString() + "'", con);
                apno = Convert.ToString(cmd.ExecuteScalar());
                if (apno != "")
                {
                    i += 1;
                }
                else
                {
                    lblExceptionEnrolID.Text = "Record Not Found !";
                    lblExceptionEnrolID.ForeColor = System.Drawing.Color.Red;
                }
            }
            if (i == 1)
            {
                cmd = new SqlCommand("select * from AppRecord where AppNo='" + Convert.ToInt32(apno.ToString()) + "' and Session='" + lblSessionHiddend.Text.ToString() + "'", con);
                SqlDataReader sdr;
                string strimid = "";
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    txtName.Text = sdr["Name"].ToString();
                    txtFName.Text = sdr["FName"].ToString();
                    txtBirth.Text = Convert.ToDateTime(sdr["DOB"].ToString()).ToString("dd/MM/yyyy");
                    lblHiddendStream.Text = sdr["Stream"].ToString();
                    ddlCourse.SelectedValue = sdr["Course"].ToString();
                    ddlPart.SelectedValue = sdr["Part"].ToString();
                    strimid = sdr["IMID"].ToString();
                    lblEnrolment.Text = sdr["Enrolment"].ToString();
                    lblASF.Text = sdr["Session"].ToString();
                    lblCompositeFeesFromExam.Text = "0"; 
                    lblFulldName.Text = txtName.Text.ToString() + " F/H " + txtFName.Text.ToString();
                    lblFullCourse.Text = lblHiddendStream.Text.ToString() + ", " + ddlCourse.SelectedValue.ToString() + "- " + ddlPart.SelectedValue.ToString();
                }
                sdr.Close();
                if (strimid.ToString() != txtIMID.Text.ToString())
                {
                    lblExceptionEnrolID.Text = "This Membership is Not Associated to IMID: " + txtIMID.Text.ToString();
                }
                cmd = new SqlCommand("select * from ExamCurrent where SId='" + lblEnrolment.Text.ToString() + "'", con);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    strCompSession = reader["Session"].ToString();
                    lblenrolStatus.Text = reader["EnrollStatus"].ToString();
                    lblPartIISID.Text = reader["CourseStatus"].ToString();
                    lblcomStatus.Text = reader["CompositeStatus"].ToString();
                    strimid = reader["IMID"].ToString();
                    ddlPart.SelectedValue = reader["Part"].ToString();
                    ddlCourse.SelectedValue = reader["Course"].ToString();
                    lblHiddendStream.Text = reader["Stream"].ToString();
                    txtName.Text = reader["SName"].ToString();
                    lblExamStatus.Text = reader["ExamStatus"].ToString();
                    if (lblcomStatus.Text == "Submitted")
                    {
                        lblCompositeFeesFromExam.Text = "0";
                        lblExceptionEnrolID.Text = lblExceptionEnrolID.Text = "Composite Fees Submitted Already.";
                    }
                    else if (lblcomStatus.Text == "Process")
                    {
                        lblCompositeFeesFromExam.Text = "0";
                        lblExceptionEnrolID.Text = lblExceptionEnrolID.Text = "Composite Fees in Process.";
                    }
                }
                reader.Close();
                if (strimid.ToString() == txtIMID.Text.ToString())
                {
                    getAdmissionFee();
                    examinationfee();
                    if (ddlPart.SelectedValue.ToString() == "PartI" | ddlPart.SelectedValue.ToString() == "SectionA")
                    {
                        lblCompositeFeesFromExam.Text = "0";
                    }
                    if (ddlPart.SelectedValue.ToString() == "PartII" | ddlPart.SelectedValue.ToString() == "SectionB")
                    {
                        lblCompositeFeesFromExam.Text = "0";
                    }
                    if (lblExamStatus.Text == "Submitted" || lblExamStatus.Text == "Approved")
                    {
                        lblExceptionEnrolID.Text = "Exam form " + lblExamStatus.Text.ToString() + ", " + lblExceptionEnrolID.Text.ToString();
                    }
                    // lblHiddendStream.Text = getstr[3].ToString();
                    // lblFeeLevel.Text = getstr[2].ToString();
                    string sen = lblASF.Text.ToString();
                    string sin = sen.Substring(0, 1);
                    string syr = sen.Substring(5, 2);
                    string senp = lblSessionHiddend.Text.ToString();
                    string sinp = senp.Substring(0, 1);
                    string syrp = senp.Substring(5, 2);
                    if (syr.ToString() == syrp.ToString())
                    {
                        panelAnnualFromExam.Visible = false;
                        lblAnnualFeesFromExam.Text = "0";
                        lblAnnualYear.Text = "0";
                    }
                    else if ((Convert.ToInt32(syrp) - Convert.ToInt32(syr) == 1) && (sinp == "S") && (sin == "W"))
                    {
                        panelAnnualFromExam.Visible = false;
                        lblAnnualFeesFromExam.Text = "0";
                        lblAnnualYear.Text = "0";
                    }
                    else if (syr != syrp)
                    {
                        int yrdef = Convert.ToInt32(syrp) - Convert.ToInt32(syr);
                        int anfee = Convert.ToInt32(lblAnnualSubscriptin.Text);
                        lblAnnualYear.Text = yrdef.ToString();
                        lblAnnualFeesFromExam.Text = (yrdef * anfee).ToString();
                        lblExceptionEnrolID.Text = lblExceptionEnrolID.Text.ToString() + " Annual Subscription Dues,";
                    }
                }
                else
                {
                    lblExceptionEnrolID.Text = "This Membership is Not Associated to IMID: " + txtIMID.Text.ToString();
                }
            }
        }
        catch (SqlException ex)
        {
            lblExceptionEnrolID.Text = ex.ToString();
        }
        finally
        {
            con.Close();
        }
    }
    protected void btnAddToApproveTable_Click(object sender, EventArgs e)
    {
        con.Close(); con.Open();
        if (ddlAppType.SelectedValue.ToString() == "Exmp")
        {
            lblFormType.Text = "ExmpFees, "; lblFeesType.Text = "ExmpFees, ";
            lblExamFee.Text = "0"; txtITIFees.Text = "0"; 
            lblFeesStore.Text = (Convert.ToInt32(lblExmpFee.Text) * Convert.ToInt32(lblExmpNo.Text)).ToString();
            
            lblThisFormAmtAppTable.Text = lblFeesStore.Text;
        }
        else if (ddlAppType.SelectedValue.ToString() == "ITI")
        {
            lblFormType.Text = lblITISerialNo.Text.ToString() + "ITI, "; lblFeesType.Text = "ITIFees, ";
            lblFeesStore.Text = txtITIFees.Text; lblExamFee.Text = "0";
            lblLateFeeChargedApptable.Text = "0"; lblCompositeFeesFromExam.Text = "0";
            lblThisFormAmtAppTable.Text = lblFeesStore.Text;
        }
        else if (ddlAppType.SelectedValue.ToString() == "Composite")
        {
            lblFormType.Text = "CompositeFees";
            lblExamFee.Text = "0"; 
            lblFeesStore.Text = "0";
            if (lblCompositeFeesFromExam.Text == "") lblCompositeFeesFromExam.Text = "0";
            int fee = Convert.ToInt32(lblFeesStore.Text.ToString());
            if (lblCompositeFeesFromExam.Text != "0")
            {
                fee = fee + Convert.ToInt32(lblCompositeFeesFromExam.Text.ToString());
                lblFeesType.Text = lblFeesType.Text.ToString() + " Composite Fees.";
            }
            lblThisFormAmtAppTable.Text = fee.ToString();
            lblFeesType.Text = "Composite Fees";
        }
        else if (ddlAppType.SelectedValue.ToString() == "Subscription")
        {
            lblExamFee.Text = "0";  txtITIFees.Text = "0";
            lblFormType.Text = "SubscriptionFees";
            lblFeesStore.Text = "0";
            if (lblAnnualFeesFromExam.Text == "") lblAnnualFeesFromExam.Text = "0";
            int fee = Convert.ToInt32(lblFeesStore.Text.ToString());
            if (lblAnnualFeesFromExam.Text != "0")
            {
                fee = fee + Convert.ToInt32(lblAnnualFeesFromExam.Text.ToString());
                lblFeesType.Text = lblFeesType.Text.ToString() + " Annual Subscription Fees";
            }
            lblThisFormAmtAppTable.Text = fee.ToString();
            lblFeesType.Text = "Annual Subscription fees";
        }
        try
        {
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            bool bl = chkEnrolment(ddlAppType.SelectedValue.ToString());
            if (bl == true && chkduplicateform() == true)
            {
                updateCount(lblSessionHiddend.Text.ToString(), txtDiaryNo.Text.ToString());
                SqlCommand cmdnnagar = new SqlCommand("insert into AppRecord(IMID,AppNo,Stream,Course,Part,Name,FName,DOB,DNo,Session,SubDate,Status,FormType,FeeType,Amount,LateFee,Exempted,Enrolment,AdmissionFees,Lavel,CompositeFees,AnnualSubFees,ITIFees,ExamFees,CADFees,underage,DupForm,SID,Exam,ITI,CAD,Project) values(@IMID,@AppNo,@Stream,@Course,@Part,@Name,@FName,@DOB,@DNo,@Session,@SubDate,@Status,@FormType,@FeeType,@Amount,@LateFee,@Exempted,@Enrolment,@AdmissionFees,@Lavel,@CompositeFees,@AnnualSubFees,@ITIFees,@ExamFees,@CADFees,@UnderAge,@DupForm,@SID,@Exam,@ITI,@CAD,@Project)", con);
                cmdnnagar.Parameters.AddWithValue("@IMID", txtIMID.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@AppNo", Convert.ToInt32(lblserialNo.Text.ToString()));
                cmdnnagar.Parameters.AddWithValue("@Stream", lblHiddendStream.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@Course", ddlCourse.SelectedValue.ToString());
                cmdnnagar.Parameters.AddWithValue("@Part", ddlPart.SelectedValue.ToString());
                cmdnnagar.Parameters.AddWithValue("@Name", txtName.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@FName", txtFName.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txtBirth.Text, dtinfo));
                cmdnnagar.Parameters.AddWithValue("@DNo", txtDiaryNo.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@Session", lblSessionHiddend.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@SubDate", Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo));
                cmdnnagar.Parameters.AddWithValue("@Status", "NotApproved");
                cmdnnagar.Parameters.AddWithValue("@FormType", lblFormType.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@FeeType", lblFeesType.Text.ToString());
                if (lblThisFormAmtAppTable.Text == "") lblThisFormAmtAppTable.Text = "0"; if (lblLateFeeChargedApptable.Text == "") lblLateFeeChargedApptable.Text = "0";
                cmdnnagar.Parameters.AddWithValue("@Amount", lblThisFormAmtAppTable.Text);
                cmdnnagar.Parameters.AddWithValue("@LateFee", lblLateFeeChargedApptable.Text);
                lblExmpFee.Text = (Convert.ToInt32(lblExmpFee.Text) * Convert.ToInt32(lblExmpNo.Text)).ToString();
                cmdnnagar.Parameters.AddWithValue("@Exempted", lblExmpFee.Text);
                cmdnnagar.Parameters.AddWithValue("@Enrolment", lblEnrolment.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@AdmissionFees", "0");
                cmdnnagar.Parameters.AddWithValue("@Lavel", lbllavel.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@CompositeFees", lblCompositeFeesFromExam.Text);
                cmdnnagar.Parameters.AddWithValue("@AnnualSubFees", lblAnnualFeesFromExam.Text);
                cmdnnagar.Parameters.AddWithValue("@ITIFees", txtITIFees.Text);
                cmdnnagar.Parameters.AddWithValue("@ExamFees", lblExamFee.Text);
                cmdnnagar.Parameters.AddWithValue("@CADFees", "0");
                cmdnnagar.Parameters.AddWithValue("@UnderAge", lblUnderAge.Text.ToString());
                 cmdnnagar.Parameters.AddWithValue("@DupForm", "0");
                 cmdnnagar.Parameters.AddWithValue("@Exam", "");
                 cmdnnagar.Parameters.AddWithValue("@ITI", lblITISerialNo.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@CAD", "");
                cmdnnagar.Parameters.AddWithValue("@SID", txtEnrolID.Text.ToString());
                cmdnnagar.Parameters.AddWithValue("@Project", "");
                cmdnnagar.ExecuteNonQuery();
                lblExceptionAppTable.ForeColor = System.Drawing.Color.Green;
                if ( ddlAppType.SelectedValue.ToString() == "ITI")
                {
                    cmd = new SqlCommand("Insert into ITIForm(Name,FName,SID,IMID,AppNo,Stream,Course,Part,DOB,Session,SubDate,Amount,Status,SNO) values(@Name,@FName,@SID,@IMID,@AppNo,@Stream,@Course,@Part,@DOB,@Session,@SubDate,@Amount,@Status,@SNO)", con);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.ToString());
                    cmd.Parameters.AddWithValue("@FName", txtFName.Text.ToString());
                        cmd.Parameters.AddWithValue("@SID", txtEnrolID.Text.ToString());
                    cmd.Parameters.AddWithValue("@IMID", txtIMID.Text.ToString());
                    cmd.Parameters.AddWithValue("@AppNo", lblserialNo.Text.ToString());
                    cmd.Parameters.AddWithValue("@Stream", lblHiddendStream.Text.ToString());
                    cmd.Parameters.AddWithValue("@Course", ddlCourse.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Part", ddlPart.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txtBirth.Text, dtinfo));
                    cmd.Parameters.AddWithValue("@Session", lblSessionHiddend.Text.ToString());
                    cmd.Parameters.AddWithValue("@SubDate", Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo));
                    cmd.Parameters.AddWithValue("@Amount", txtITIFees.Text.ToString());
                    cmd.Parameters.AddWithValue("@Status", "Submitted");
                    cmd.Parameters.AddWithValue("@SNO", lblITISerialNo.Text.ToString());
                    cmd.ExecuteNonQuery();
                }
                    cmd = new SqlCommand("Update ExamCurrent set Stream=@Stream,Course=@Course,Part=@Part, ExamStatus=@ExamStatus,CompositeStatus=@CompositeStatus,EnrollStatus=@EnrollStatus,CourseStatus=@CourseStatus,Session=@Session,SessionDuration=@SessionDuration where SId='" + lblEnrolment.Text.ToString() + "'", con);
                    if (lblExamStatus.Text == "") lblExamStatus.Text = "NotSubmitted";
                    if (lblcomStatus.Text == "") lblcomStatus.Text = "NotSubmitted";  // in case of iti from submission lblcomstatus.tex="";
                    if (lblPartIISID.Text == "") lblPartIISID.Text = "NotPassed";
                    if (strCompSession == null || strCompSession == "") strCompSession = lblSessionHiddend.Text;
                    if (lblenrolStatus.Text == "") lblenrolStatus.Text = "Submitted";
                  
                        cmd.Parameters.AddWithValue("@ExamStatus", lblExamStatus.Text);
                        cmd.Parameters.AddWithValue("@CourseStatus", lblPartIISID.Text.ToString());
                        if (lblCompositeFeesFromExam.Text != "0")
                            cmd.Parameters.AddWithValue("@CompositeStatus", "Process");
                        else
                            cmd.Parameters.AddWithValue("@CompositeStatus", lblcomStatus.Text.ToString());
                        cmd.Parameters.AddWithValue("@sessionDuration", 6);
                    cmd.Parameters.AddWithValue("@Stream", lblHiddendStream.Text.ToString());
                    cmd.Parameters.AddWithValue("@Course", ddlCourse.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Part", ddlPart.SelectedValue.ToString());
                    if (ddlAppType.SelectedValue.ToString() == "ReAdmission" || ddlAppType.SelectedValue.ToString() == "Admission" || ddlAppType.SelectedValue.ToString() == "Exam")
                    {
                        cmd.Parameters.AddWithValue("@EnrollStatus", "Submitted");
                        cmd.Parameters.AddWithValue("@Session", lblSessionHiddend.Text.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EnrollStatus", lblenrolStatus.Text);
                        cmd.Parameters.AddWithValue("@Session", strCompSession);
                    }
                    cmd.ExecuteNonQuery();
                if (lblAnnualYear.Text != "" && lblAnnualYear.Text != "0" && lblAnnualFeesFromExam.Text!="0")
                {
                    cmd = new SqlCommand("select AnnualSubscription from Student where SID='" + lblEnrolment.Text.ToString() + "'", con);
                    string annses = Convert.ToString(cmd.ExecuteScalar());
                    string syr = annses.Substring(3, 4);
                    string sess = annses.Substring(0, 3);
                    syr = (Convert.ToInt32(lblAnnualYear.Text) + Convert.ToInt32(syr)).ToString();
                    sess = sess + syr;
                    cmd = new SqlCommand("update Student set AnnualSubscription='" + sess + "' where SID='" + lblEnrolment.Text.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                }
                // insert composite Fees Table
                SqlDataAdapter ad = new SqlDataAdapter("select Name,FName,DOB,FormType,FeeType,Amount,LateFee,Exempted,Enrolment,AdmissionFees,CompositeFees,AnnualSubFees,ITIFees,ExamFees,CADFees,DupForm from AppRecord where DNo='" + txtDiaryNo.Text.ToString() + "' and Session='" + lblSessionHiddend.Text.ToString() + "' and Status='NotApproved' ORDER BY SN DESC", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                GridAppTable.DataSource = dt;
                GridAppTable.DataBind();
                lblExceptionAppTable.Text = "[" + lblserialNo.Text.ToString() + "]: Serial No: ";
                if (ddlAppType.SelectedValue.ToString() == "ITI")
                {
                    lblExceptionAppTable.Text = lblExceptionAppTable.Text.ToString() + " " + lblITISerialNo.Text.ToString() + ", ";
                    updateserialno("ITI", lblSessionHiddend.Text.ToString());
                }
                lblITISerialNo.Text = serialno("ITI".ToString());
               
                lblExceptionAppTable.Text = lblExceptionAppTable.Text.ToString() + " Added.";
                cleanref(); ddlAppType.SelectedIndex = 0;
                ddlAppType.Focus();
            }
            else if (bl == false)
            {
                lblExceptionAppTable.Text = "Membership No. not found.";
                lblExceptionAppTable.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (lblExceptionCheck.Text == "")
                lblExceptionAppTable.Text = lblExceptionAppTable.Text+ " Invalid Application Form.";
                lblExceptionAppTable.ForeColor = System.Drawing.Color.Red;
                lblExceptionAppTable.Font.Bold = true;
                btnAddApproveVisible.Focus();
            }
            lblserialNo.Text = esngn();
            Log.WriteLog(Request.QueryString["maikal"], "B004", txtIMID.Text.ToString() + ":" + txtDiaryNo.Text.ToString(), lblserialNo.Text.ToString(), "Application Add");
            Log.WriteLog("B004", Request.QueryString["maikal"], txtIMID.Text.ToString() + ":" + txtDiaryNo.Text.ToString(), lblserialNo.Text.ToString(), "Application Add");
            btnAddApproveVisible.Enabled = false;
            con.Close();
        }
        catch (SqlException ex)
        {
            lblExceptionCheck.Text = ex.ToString();
        }
        catch (FormatException ex)
        {
            lblExceptionCheck.Text = "Invalid Data.";
        }
        catch (NullReferenceException ex)
        {
            lblExceptionCheck.Text = "Fees Amount is Null";
        }
        catch (Exception ex)
        {
            lblExceptionCheck.Text = ex.ToString();
        }
        finally
        {
            con.Dispose();
        }
    }
    protected void btnSearch_Onclick(object sender, EventArgs e)
    {
        if (txtNameSearch.Text == "")
        {
            GridDuplicacy.DataSource = null;
            GridDuplicacy.DataBind();
        }
        else
        {
            // SqlDataAdapter ad=new SqlDataAdapter("select * from AppRecord where Name Like '%" + txtName.Text.ToString() + "%' and FName Like '%" + txtFName.Text.ToString() + "%' and DOB Like '%" + Convert.ToDateTime(txtBirth.Text) + "%'", con);
            // SqlDataAdapter ad = new SqlDataAdapter("select * from AppRecord", con);
            string query = "";
            if (ddlSearchIn.SelectedValue.ToString() == "All")
            {
                if (ddlSearchFor.SelectedValue.ToString() == "Name")
                {
                    query = "select * from Student where Name like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "FName")
                {
                    query = "select * from Student where FName like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "DOB")
                {
                    query = "select * from Student where DOB like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "Membership")
                {
                    query = "select * from Student where SID like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "SNO")
                {
                    query = "select * from AppRecord where AppNo like '%" + txtNameSearch.Text.ToString() + "%'";
                }
            }
            else if (ddlSearchIn.SelectedValue.ToString() == "IM")
            {

                if (ddlSearchFor.SelectedValue.ToString() == "Name")
                {
                    query = "select * from Student where IMID='" + txtIMID.Text.ToString() + "' and  Name like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "FName")
                {
                    query = "select * from Student where IMID='" + txtIMID.Text.ToString() + "' and  FName like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "DOB")
                {
                    query = "select * from Student where IMID='" + txtIMID.Text.ToString() + "' and  DOB like '%" + txtNameSearch.Text.ToString() + "%'";
                }

                else if (ddlSearchFor.SelectedValue.ToString() == "Membership")
                {
                    query = "select * from Student  where IMID='" + txtIMID.Text.ToString() + "' and SID like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "SNO")
                {
                    query = "select * from AppRecord where IMID='" + txtIMID.Text.ToString() + "' and  AppNo like '%" + txtNameSearch.Text.ToString() + "%'";
                }
            }
            else if (ddlSearchIn.SelectedValue.ToString() == "App")
            {
                // query = "select * from AppRecord where Name Like '%" + txtName.Text.ToString() + "%' and FName Like '%" + txtFName.Text.ToString() + "%'";
                if (ddlSearchFor.SelectedValue.ToString() == "Name")
                {
                    query = "select * from AppRecord where Name like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "FName")
                {
                    query = "select * from AppRecord where  FName like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "DOB")
                {
                    query = "select * from AppRecord where  DOB like '%" + txtNameSearch.Text.ToString() + "%'";
                }

                else if (ddlSearchFor.SelectedValue.ToString() == "Membership")
                {
                    query = "select * from AppRecord  where  SID like '%" + txtNameSearch.Text.ToString() + "%'";
                }
                else if (ddlSearchFor.SelectedValue.ToString() == "SNO")
                {
                    query = "select * from AppRecord where  AppNo like '%" + txtNameSearch.Text.ToString() + "%'";
                }
            }
            lblSearchlabel.Text = "Search in " + ddlSearchIn.SelectedValue.ToString() + " Record(s) of Student's " + ddlSearchFor.SelectedItem.Text;
            SqlDataAdapter ad = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                GridDuplicacy.DataSource = ds;
                GridDuplicacy.DataBind();
            }
            else
            {
                lblExceptionOK.Text = "New Entry, No duplicate found.";
            }
        }
        GridDuplicacy.Focus();
    }

    #region functions
    private void allvisisblefalse()
    {
        lblExamFee.Text = "0"; lblExmpFee.Text = "0"; lblLFee.Text = "0"; lblExmpNo.Text = "0";
        lblToAmtFo.Text = "0"; lblToLateFo.Text = "0"; 

        chkAddwithAdmisiosn.Checked = false; 

      panelAnnualFromExam.Visible = false; PanelProfile.Visible = false;
        PanelExamFee.Visible = false; PanelComposite.Visible = false;
        panelRightBox.Visible = false;
        PanelITI.Visible = false; PanelProfile.Visible = false;
        PanelSubscriptin.Visible = false;

        lblExceptionEnrolID.Text = ""; lblExceptionAppTable.Text = ""; lblEnrolment.Text = "";
        lblExceptionCount.Text = ""; txtName.Text = ""; txtFName.Text = ""; txtBirth.Text = "";
        lblFullCourse.Text = ""; lblFulldName.Text = ""; lblExceptionCheck.Text = ""; txtEnrolID.Text = ""; lblExceptionEnrolID.Text = "";
        lblITISerialNo.Visible = false;
        lblEnrolName.Text = "Membership No."; chkAddwithAdmisiosn.Visible = false;
        txtEnrolID.Visible = true; lblEnrolName.Visible = true;
    }
    public void cleanref()
    {
        lblcomStatus.Text = ""; lblExamStatus.Text = ""; lblenrolStatus.Text = "";
        chkAddwithAdmisiosn.Checked = false; lblEnrolName.Text = "Membership.";
        ddlPart.SelectedIndex = 0; ddlCourse.SelectedIndex = 0;
        GridDuplicacy.DataSource = null; GridDuplicacy.DataBind();
        txtEnrolID.Text = ""; txtName.Text = ""; txtFName.Text = ""; txtBirth.Text = "";
        lblFullCourse.Text = ""; lblFulldName.Text = ""; lblExceptionEnrolID.Text = ""; lblExceptionCheck.Text = "";
        lblITISerialNo.Visible = false; lblITISerialNo.Visible = false;
       lblExceptionDateOfBirth.Text = "";
        PanelExamFee.Visible = false; PanelComposite.Visible = false;
        PanelITI.Visible = false; PanelSubscriptin.Visible = false;
        panelRightBox.Visible = false; 
        lblserialNo.Text = esngn();
        PanelProfile.Visible = false;
        ddlAppType.Focus();
    }// Admission Amount Total.
    private void chkDuplicate()
    {
        dtinfo.DateSeparator = "/";
        dtinfo.ShortDatePattern = "dd/MM/yyyy";
        if (txtName.Text == "" | txtFName.Text == "")
        {
            GridDuplicacy.DataSource = null; GridDuplicacy.DataBind();
        }
        else
        {
            SqlDataAdapter ad = new SqlDataAdapter("select * from Student where FName like '%" + txtFName.Text.ToString() + "%' and  Name like '%" + txtName.Text.ToString() + "%' and DOB = '" + Convert.ToDateTime(txtBirth.Text.ToString(), dtinfo) + "'", con);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridDuplicacy.DataSource = ds;
                GridDuplicacy.DataBind();
                lbtnCheckDues.Text = "Duplicate Record Found !!!";
            }
            else
            {
                SqlDataAdapter ad1 = new SqlDataAdapter("select * from AppRecord where FName like '%" + txtFName.Text.ToString() + "%' and  Name like '%" + txtName.Text.ToString() + "%' and DOB = '" + Convert.ToDateTime(txtBirth.Text.ToString(), dtinfo) + "' and Session='" + lblSessionHiddend.Text.ToString() + "'", con);
                // DataSet ds = new DataSet();
                ad1.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GridDuplicacy.DataSource = ds;
                    GridDuplicacy.DataBind();
                    lbtnCheckDues.Text = "Duplicate Record Found !!!";
                }
                else
                {
                    lbtnCheckDues.Text = "Duplicate Record Not Found !!!";
                    // lbtnCheckDues.Focus();
                }
            }
        }
    }
    private bool duplicateadmission()
    {
        bool bl = false;
        cmd = new SqlCommand("select sid from Student where FName like '" + txtFName.Text.ToString() + "%' and  Name like '" + txtName.Text.ToString() + "%' and DOB = '" + Convert.ToDateTime(txtBirth.Text.ToString(), dtinfo) + "' ", con);
        string sid = Convert.ToString(cmd.ExecuteScalar());
        if (sid != "")
            bl = true;
        return bl;
    }
    private void showcount(string session, string dairy)
    {
        con.Close(); con.Open();
        cmd = new SqlCommand("select * from DairyCount where Session='" + session.ToString() + "' and DairyNo='" + dairy.ToString() + "'", con);
        SqlDataReader reader;
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lblADDRcv.Text = reader["ADDRcv"].ToString();
            lblADDSub.Text = reader["ADDSub"].ToString();
            lblODDRcv.Text = reader["ODDRcv"].ToString();
            lblODDSub.Text = reader["ODDSub"].ToString();
            lblAdmissionRcv.Text = reader["EnrollFormRcv"].ToString();
            lblAdmissionSub.Text = reader["EnrollFormSub"].ToString();
            lblExamRcv.Text = reader["ExamFormRcv"].ToString();
            lblExamSub.Text = reader["ExamFormSub"].ToString();
            lblITIRcv.Text = reader["ITIRcv"].ToString(); lblITISub.Text = reader["ITISub"].ToString();
            lblCADRcv.Text = reader["CADRcv"].ToString(); lblCADSub.Text = reader["CADSub"].ToString();
            lblOthersFormRcv.Text = reader["OtherFormRcv"].ToString(); lblOthersFormSub.Text = reader["OtherFormSub"].ToString();
            lblProvisionalRcv.Text = reader["ProvisionalRcv"].ToString(); lblProvisionalSub.Text = reader["ProvisionalSub"].ToString();
            lblFinalPassRcv.Text = reader["FinalPassRcv"].ToString(); lblFinalPassSub.Text = reader["FinalPassSub"].ToString();
            lblReCheckingRcv.Text = reader["ReCheckingRcv"].ToString(); lblReCheckingSub.Text = reader["ReCheckingSub"].ToString();
            lblDuplicateRcv.Text = reader["DuplicateDocsRcv"].ToString(); lblDuplicateSub.Text = reader["DuplicateDocsSub"].ToString();
            lblMembershipRcv.Text = reader["MemberRcv"].ToString(); lblMembershipSub.Text = reader["MemberSub"].ToString();
            lblBooksRcv.Text = reader["BooksRcv"].ToString(); lblBooksSub.Text = reader["BooksSub"].ToString();
            lblProsRcv.Text = reader["ProspectusRcv"].ToString(); lblProsSub.Text = reader["ProspectusSub"].ToString();
        }
        reader.Close();
        cmd = new SqlCommand("select * from ProjectCount where Session='" + session.ToString() + "' and DairyNo='" + dairy.ToString() + "'", con);
        SqlDataReader read;
        read = cmd.ExecuteReader();
        while (read.Read())
        {
            lblProjectRcv.Text = read["DDRcv"].ToString(); lblProjectSub.Text = read["DDSub"].ToString();
            lblProformaCRcv.Text = read["ProformaARcv"].ToString(); lblProformaCSub.Text = read["ProformaASub"].ToString();
            lblProformaBRcv.Text = read["ProformaBRcv"].ToString(); lblProformaBSub.Text = read["ProformaBSub"].ToString();
        }
        read.Close();
        con.Close();
        con.Dispose();
    }
    private void updateCount(string session, string dairy)
    {
       
        if (ddlAppType.SelectedValue.ToString() == "ITI")
        {
            lblITISub.Text = (Convert.ToInt32(lblITISub.Text) + 1).ToString();
        }
       
      
        if (ddlAppType.SelectedValue.ToString() == "Composite")
        {
            lblOthersFormSub.Text = (Convert.ToInt32(lblOthersFormSub.Text) + 1).ToString();
        }
        if (ddlAppType.SelectedValue.ToString() == "Subscription")
        {
            lblOthersFormSub.Text = (Convert.ToInt32(lblOthersFormSub.Text) + 1).ToString();
        }
        if (ddlAppType.SelectedValue.ToString() == "Exmp")
        {
            lblOthersFormSub.Text = (Convert.ToInt32(lblOthersFormSub.Text) + 1).ToString();
        }
        cmd = new SqlCommand("update DairyCount set EnrollFormSub=@EnrollFormSub,CADSub=@CADSub,ExamFormSub=@ExamFormSub,ITISub=@ITISub,OtherFormSub=@OtherFormSub,ProvisionalSub=@ProvisionalSub,FinalPassSub=@FinalPassSub,ReCheckingSub=@ReCheckingSub,DuplicateDocsSub=@DuplicateDocsSub where Session='" + session.ToString() + "' and DairyNo='" + dairy.ToString() + "'", con);
        cmd.Parameters.AddWithValue("@EnrollFormSub", Convert.ToInt32(lblAdmissionSub.Text));
        cmd.Parameters.AddWithValue("@ExamFormSub", Convert.ToInt32(lblExamSub.Text));
        cmd.Parameters.AddWithValue("@CADSub", Convert.ToInt32(lblCADSub.Text));
        cmd.Parameters.AddWithValue("@ITISub", Convert.ToInt32(lblITISub.Text));
        cmd.Parameters.AddWithValue("@OtherFormSub", Convert.ToInt32(lblOthersFormSub.Text));
        cmd.Parameters.AddWithValue("@ProvisionalSub", Convert.ToInt32(lblProvisionalSub.Text));
        cmd.Parameters.AddWithValue("@FinalPassSub", Convert.ToInt32(lblFinalPassSub.Text));
        cmd.Parameters.AddWithValue("@ReCheckingSub", Convert.ToInt32(lblReCheckingSub.Text));
        cmd.Parameters.AddWithValue("@DuplicateDocsSub", Convert.ToInt32(lblDuplicateSub.Text));
        cmd.ExecuteNonQuery();
        cmd = new SqlCommand("update ProjectCount set ProformaASub='" + lblProformaCSub.Text + "',ProformaBSub='" + lblProformaBSub.Text + "' where Session='" + session.ToString() + "' and DairyNo='" + dairy.ToString() + "'", con);
        cmd.ExecuteNonQuery();
    }
    private string getcurrentfees()
    {
        con.Close(); con.Open();
        SqlCommand cmd = new SqlCommand("select Max(FeeLevel) from FeeMaster where Status='Active' and Type='" + ddlFeeMaster.SelectedValue.ToString() + "'", con);
        string level = Convert.ToString(cmd.ExecuteScalar()); con.Close();
        return level;
    }
    private void getAdmissionFee()  // Fees from FeeMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Conn"].ToString());
        con.Close(); con.Open();
        cmd = new SqlCommand("select * from FeeMaster where FeeType='" + lblHiddendStream.Text.ToString() + "' and FeeLevel='" + lblFeeLevel.Text.ToString() + "' and Type='" + ddlFeeMaster.SelectedValue.ToString() + "'", con);
        SqlDataReader reader;
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lblCompoisteA.Text = reader["ComFee1"].ToString().TrimEnd('0').TrimEnd('.');
            lblCompositeB.Text = reader["ComFee2"].ToString().TrimEnd('0').TrimEnd('.');
            lblAnnualSubscriptin.Text = reader["ASubscription"].ToString().TrimEnd('0').TrimEnd('.');
            lblCompositeDuration.Text = reader["CompositeDuration"].ToString();
            txtITIFees.Text = reader["ITIFees"].ToString().TrimEnd('0').TrimEnd('.');
        }
        reader.Close();
    }
    private void examinationfee()   // Uses Stream and FeeLvl
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Conn"]);
        try
        {
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            con.Close(); con.Open();
            lblExamFee.Text = "0"; lblExmpFee.Text = "0"; lblLFee.Text = "0";
            cmd = new SqlCommand("select * from FeeMaster where FeeType='" + lblHiddendStream.Text.ToString() + "' and FeeLevel='" + lblFeeLevel.Text.ToString() + "' and Type='" + ddlFeeMaster.SelectedValue.ToString() + "'", con);
            SqlDataReader rd;
            rd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            dr2 = dt.Rows[0];
            if (txtDiaryRcvDate.Text == "")
            { }
            else
            {
                if (ddlsession.SelectedValue.ToString() == "Sum")
                {
                    int i = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["SumExamWOL"], dtinfo));
                    if (i <= 0)
                    {
                        lblLFee.Text = "0";
                    }
                    else
                    {
                        int j = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["SumExamWL1"], dtinfo));
                        if (j <= 0)
                        {
                            lblLFee.Text = dr2["ExamLF1"].ToString().TrimEnd('0').TrimEnd('.');
                        }
                        else
                        {
                            int k = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["SumExamWL2"], dtinfo));
                            if (k <= 0)
                            {
                                lblLFee.Text = dr2["ExamLF2"].ToString().TrimEnd('0').TrimEnd('.');
                            }
                            else
                            {
                                int l = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["SumExamWL3"], dtinfo));
                                if (l <= 0)
                                {
                                    lblLFee.Text = dr2["ExamLF3"].ToString().TrimEnd('0').TrimEnd('.');
                                }
                                else
                                {
                                    lblLFee.Text = "0";
                                    lblException.Text = "";
                                    lblException.ForeColor = System.Drawing.Color.Red;
                                    lblException.Font.Bold = true;
                                }
                            }
                        }
                    }
                }
                else if (ddlsession.SelectedValue.ToString() == "Win")
                {
                    int i = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["WinExamWOL"], dtinfo));
                    if (i <= 0)
                    {
                        lblLFee.Text = "0";
                    }
                    else
                    {
                        int j = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["WinExamWL1"], dtinfo));
                        if (j <= 0)
                        {
                            lblLFee.Text = dr2["ExamLF1"].ToString().TrimEnd('0').TrimEnd('.');
                        }
                        else
                        {
                            int k = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["WinExamWL2"], dtinfo));
                            if (k <= 0)
                            {
                                lblLFee.Text = dr2["ExamLF2"].ToString().TrimEnd('0').TrimEnd('.');
                            }
                            else
                            {
                                int l = DateTime.Compare(Convert.ToDateTime(txtDiaryRcvDate.Text, dtinfo), Convert.ToDateTime(dr2["WinExamWL3"], dtinfo));
                                if (l <= 0)
                                {
                                    lblLFee.Text = dr2["ExamLF3"].ToString().TrimEnd('0').TrimEnd('.');
                                }
                                else
                                {
                                    lblLFee.Text = "0";
                                    lblException.Text = "";
                                    lblException.ForeColor = System.Drawing.Color.Red;
                                    lblException.Font.Bold = true;
                                }
                            }
                        }
                    }
                }
            }
            lblExmpFee.Text = dr2["ExemptionFee"].ToString().TrimEnd('0');
            lblExmpFee.Text = lblExmpFee.Text.TrimEnd('.');
            if (ddlPart.SelectedValue.ToString() == "PartI")
            {
                lblExamFee.Text = dr2["ESecA"].ToString();
                lblExamFee.Text = lblExamFee.Text.TrimEnd('0').TrimEnd('.');
            }
            else if (ddlPart.SelectedValue.ToString() == "PartII")
            {
                lblExamFee.Text = dr2["ESecB"].ToString();
                lblExamFee.Text = lblExamFee.Text.TrimEnd('0').TrimEnd('.');
            }
            else if (ddlPart.SelectedValue.ToString() == "SectionA")
            {
                lblExamFee.Text = dr2["ESecA"].ToString();
                lblExamFee.Text = lblExamFee.Text.TrimEnd('0').TrimEnd('.');
            }
            else if (ddlPart.SelectedValue.ToString() == "SectionB")
            {
                lblExamFee.Text = dr2["ESecB"].ToString();
                lblExamFee.Text = lblExamFee.Text.TrimEnd('0').TrimEnd('.');
            }
            lblExceptionOK.Text = "";
        }
        catch (FormatException ex)
        {
            lblExceptionOK.Text = "Invalid Date Format Or Insert Valid Diary No.";
            lblException.ForeColor = System.Drawing.Color.Red;
            lblException.Font.Bold = true;
        }
        catch (SqlException ex)
        {
            lblExceptionOK.Text = "Invalid Date Format  Or Insert Valid Diary No.";
            lblException.ForeColor = System.Drawing.Color.Red;
            lblException.Font.Bold = true;
        }
        finally
        {
        }
    }
    private void chkFeeValue()
    {
        if (txtITIFees.Text == "") txtITIFees.Text = "0";
        if (lblAnnualSubscriptin.Text == "") lblAnnualSubscriptin.Text = "0";
        lblAnnualYear.Text = "0"; lblAnnualFeesFromExam.Text = "0";
        if (lblCompositeFeesFromExam.Text == "") lblCompositeFeesFromExam.Text = "0";
    }
    string[] getstr;
    private string chkExamForm()
    {
        string examStatus;
        string status = "";
        string dt = txtBirth.Text.ToString();
        cmd = new SqlCommand("select ExamStatus from ExamCurrent where  SName='" + txtName.Text.ToString() + "'  and IMID='" + txtIMID.Text.ToString() + "' and Course='" + ddlCourse.SelectedValue.ToString() + "' and Part='" + ddlPart.SelectedValue.ToString() + "' and Sid='" + lblEnrolment.Text.ToString() + "'", con);
        examStatus = Convert.ToString(cmd.ExecuteScalar());
        if (examStatus == "Submitted")
        {
            lblExceptionCheck.Text = "Exam Form Already Submitted. " + lblExceptionCheck.Text.ToString();
            status = "YES";
        }
        else if (examStatus == "Approved")
        {
            lblExceptionCheck.Text = "Exam Form Already Approved. " + lblExceptionCheck.Text.ToString();
            status = "YES";
        }
        else if (examStatus == "Filled")
        {
            lblExceptionCheck.Text = "Exam Form Already Filled. " + lblExceptionCheck.Text.ToString();
            status = "YES";
        }
        return status;
    }   // Checking the Examination Form Status of Student
    private string chkITI()  // ITI from Status of Student
    {
        string itiname = "";
        try
        {
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            string dt = txtBirth.Text.ToString();
            cmd = new SqlCommand("select Name from ITIForm where Name='" + txtName.Text.ToString() + "' and FName like '%" + txtFName.Text.ToString() + "%' and IMID='" + txtIMID.Text.ToString() + "' and DOB='" +Convert.ToDateTime( dt.ToString(),dtinfo)+ "' ", con);
            itiname = Convert.ToString(cmd.ExecuteScalar());
        }
        catch (FormatException ex)
        {
            itiname = "NO";
        }
        if (itiname == "" | itiname == "NO")
        {
            itiname = "NO";
        }
        else
        {
            itiname = "YES";
        }
        return itiname;
    }
    private string chkCADForm()
    {
        string CADStatus = "";
        try
        {
            if (ddlPart.SelectedValue == "PartI" | ddlPart.SelectedValue == "SectionA")
            {
                CADStatus = "YES";
                lblExceptionCheck.Text += "CAD Form Can't be Submitted for PartI/SectionA";
            }
            else
            {
                DateTime dt = Convert.ToDateTime(Convert.ToDateTime(txtBirth.Text).ToString("dd/MM/yyyy"));
                cmd = new SqlCommand("select AppNo from AppRecord where Name='" + txtName.Text.ToString() + "' and FName='" + txtFName.Text.ToString() + "' and Session='"+lblSessionHiddend.Text.ToString()+"'  and FormType like '%CAD%' ", con);
                CADStatus = Convert.ToString(cmd.ExecuteScalar());
            }
        }
        catch (FormatException ex)
        {
            CADStatus = "NO";
        }
        if (CADStatus == "" | CADStatus == "NO")
            CADStatus = "NO";
        else
            CADStatus = "YES";
        return CADStatus;

    }//CAD Form Status of Student
    private bool chkduplicateform()
    {
        bool bl = true;
        try
        {
            if (lblExceptionCount.Text != "")
            {
                bl = false;
            }
            else
            {
                dtinfo.DateSeparator = "/";
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
               
                if (ddlAppType.SelectedValue.ToString() == "ITI")
                {
                    if (chkITI() == "YES")
                    {
                        lblExceptionAppTable.Text = "Duplicate ITI Form Found.";
                        bl = false;
                    }
                    if (ddlPart.SelectedValue.ToString() == "PartI")
                    {
                        bl = false;
                        lblExceptionAppTable.Text = "PartI ITI Form Can't Accepted.";
                    }
                }
                if (ddlAppType.SelectedValue.ToString() == "Composite")
                {
                    if (lblCompositeFeesFromExam.Text == "0")
                        bl = false;
                }
                if (ddlAppType.SelectedValue.ToString() == "Subscription")
                {
                    if (lblAnnualYear.Text == "0")
                        bl = false;
                }
                if (ddlAppType.SelectedValue.ToString() == "Exmp")
                {
                }
            }
        }
        catch (FormatException ex)
        {
            lblExceptionCheck.Text += ex.ToString();
        }
        catch (SqlException ex)
        {
            lblExceptionCheck.Text += ex.ToString();
        }
        finally
        {
        }
        return bl;
    }
    string asn;
    int sn;
    private string serialno(string type)
    {
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("select SerialNo from FeeList where FeeName='" + type.ToString() + "' and Status='NO' and Session='" + lblSessionHiddend.Text.ToString() + "'", con);
        string rtnsn = Convert.ToString(cmd.ExecuteScalar());
        if (rtnsn == "")
        {
            cmd = new SqlCommand("insert into FeeList (FeeName,Status,Session) values(@FeeName,@Status,@Session)", con);
            cmd.Parameters.AddWithValue("@FeeName", type.ToString());
            cmd.Parameters.AddWithValue("@Status", "NO");
            cmd.Parameters.AddWithValue("@Session", lblSessionHiddend.Text.ToString());
            cmd.ExecuteNonQuery();
            rtnsn = "0";
        }
        string sessionid = "";
        if (type == "Admission")
        {
            // Add Extra SessionID ex Sum2012=121
            sd = new SessionDuration();
            sessionid= sd.sessionid(lblSessionHiddend.Text).ToString();
        }
        string fw = type.Substring(0, 1);
        int intsn = Convert.ToInt32(rtnsn) + 1;
        rtnsn = intsn.ToString();
        return fw.ToString() + sessionid+"" + rtnsn.ToString();
    }  //Generete Serial No of froms.
    private void updateserialno(string type, string session)
    {
        try
        {
            cmd = new SqlCommand("update Feelist set SerialNo=SerialNo+1 where FeeName='" + type.ToString() + "' and Session='" + session.ToString() + "'", con);
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            lblExceptionCheck.Text += type.ToString() + " SerialNo. Not Updated.";
        }
    }
    private string esngn()
    {
        cmd = new SqlCommand("select Max(AppNo) from AppRecord", con);
        asn = cmd.ExecuteScalar().ToString();
        if (asn == "")
            sn = 1;
        else
        {
            sn = Convert.ToInt32(asn);
            sn = sn + 1;
        }
        lblserialNo.Text = sn.ToString();
        return sn.ToString();
    }
    private void anncomp()
    {
        if (lblCompositeFeesFromExam.Text == "") lblCompositeFeesFromExam.Text = "0";
        if (lblAnnualFeesFromExam.Text == "") lblAnnualFeesFromExam.Text = "0";
        int fee = Convert.ToInt32(lblFeesStore.Text.ToString());
        if (lblAnnualFeesFromExam.Text != "0")
        {
            fee = fee + Convert.ToInt32(lblAnnualFeesFromExam.Text.ToString());
            lblFeesType.Text = lblFeesType.Text.ToString() + " Annual Subscription Fees";
        }
        if (lblCompositeFeesFromExam.Text != "0")
        {
            fee = fee + Convert.ToInt32(lblCompositeFeesFromExam.Text.ToString());
            lblFeesType.Text = lblFeesType.Text.ToString() + " Composite Fees.";
        }
        lblThisFormAmtAppTable.Text = fee.ToString();
    }
    private bool chkEnrolment(string enrol)
    {
        bool bl = true;
        if (enrol == "Admission" | enrol == "Exam" | enrol == "Membership" | enrol == "ITI" | enrol == "OldSet" | enrol == "Other")
        {
            bl = true;
        }
        else
        {
            if (txtEnrolID.Text == "")
                bl = false;
            else
            {
                bl = true;
                if (txtIMID.Text == "" || txtDiaryNo.Text == "" || txtDiaryRcvDate.Text == "")
                {
                    bl = false;
                }
                else bl = true;
            }
        }
        return bl;
    }
    private void ChangeFeeLevel(string sid)
    {
        cmd = new SqlCommand("update Student set FeeLevel=@FeeLevel,AnnualSubscription=@AnnualSubscription where SID='" + sid + "'", con);
        cmd.Parameters.AddWithValue("@FeeLevel",lblFeeLevel.Text);
        cmd.Parameters.AddWithValue("@AnnualSubscription", lblSessionHiddend.Text.ToString());
        cmd.ExecuteNonQuery();
    }
    #endregion
}