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
using iTextSharp.text;
using System.Globalization;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
public partial class Exam_RecheckingUpdate : System.Web.UI.Page
{
    DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Conn"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(Server.HtmlEncode(Request.Cookies["MyLogin"]["PWD"])) == "")
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                con.Open();
                btnSave.Enabled = false;
                txtYearSeason.Text = DateTime.Now.Year.ToString();
                maikal mk = new maikal();
                int sn = mk.chksession();
                if (sn == 0) ddlExamSeason.SelectedValue = "Sum"; else ddlExamSeason.SelectedValue = "Win";
                lblHiddenSeason.Text = ddlExamSeason.SelectedValue.ToString() + "" + txtYearSeason.Text.ToString();
                ddlCourse.SelectedValue = "Civil";
                ddlPart.SelectedValue = "PartI";
                string qyry = "";
                if (ddlCourse.SelectedValue.ToString() == "Civil")
                {
                    qyry = "select distinct CourseID from CivilSubMaster";
                }
                else if (ddlCourse.SelectedValue.ToString() == "Architecture")
                {
                    qyry = "select distinct CourseID from ArchiSubMaster";
                }

                SqlDataAdapter ad = new SqlDataAdapter(qyry, con);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                ddlSyllabus.DataSource = ds;
                ddlSyllabus.DataTextField = "CourseID";
                ddlSyllabus.DataValueField = "CourseID";
                ddlSyllabus.DataBind();
                showcourse();
                details();
                GridMarks.DataSource = grid();
                GridMarks.DataBind();
                if (GridMarks.Rows.Count == 0) btnSave.Enabled = false; else btnSave.Enabled = true;
                ddlExamSeason.Focus();
              

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
    protected void lblHomeRedirect_Click(object sender, EventArgs e)
    {
        try
        {
        maikal mk = new maikal();
        int i = mk.returnlevel(Server.HtmlEncode(Request.Cookies["MyLogin"]["UID"]).ToString(), Server.HtmlEncode(Request.Cookies["MyLogin"]["PWD"]).ToString());
        if (i == 0 | i == 1)
            Response.Redirect("../SuperAdmin.aspx?" + Request.Cookies["redic"].Value.ToString());
        else if (i == 2)
        {
            Response.Redirect("../UserHome.aspx?" + Request.Cookies["redic"].Value.ToString());
        }
        }
        catch (NullReferenceException ex)
        {
            Response.Redirect("../Login.aspx");
        }
    }
    protected void lbtnNext1Redirect_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExamDefault.aspx?dev=" + Request.QueryString["dev"] + "&lnk=null&typ=Ex&id=");
    }
    
    protected void ddlSyllabus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCourse.SelectedValue.ToString() == "Civil")
        {
            cmd = "select * from CivilSubMaster where Section='" + ddlPart.SelectedValue.ToString() + "' and CourseID='" + ddlSyllabus.SelectedValue.ToString() + "'";
        }
        else if (ddlCourse.SelectedValue.ToString() == "Architecture")
        {
            cmd = "select * from ArchiSubMaster where Section='" + ddlPart.SelectedValue.ToString() + "' and CourseID='" + ddlSyllabus.SelectedValue.ToString() + "'";
        }
        SqlDataAdapter ad = new SqlDataAdapter(cmd, con);
        DataTable dt = new DataTable();
        ad.Fill(dt);
        ddlsubcode.DataSource = dt;
        ddlsubcode.DataValueField = "SubID";
        ddlsubcode.DataTextField = "SubID";
        ddlsubcode.DataBind();
        showcourse();
        ddlCourse.Focus();
    }
    string stcmss;
    private void details()
    {
       
        if (ddlCourse.SelectedValue.ToString() == "Civil")
        {
            stcmss = "select * from CivilSubMaster where SubID='" + ddlsubcode.SelectedValue.ToString() + "' and CourseID='" + ddlSyllabus.SelectedValue.ToString() + "'";
        }
        else if (ddlCourse.SelectedValue.ToString() == "Architecture")
        {
            stcmss = "select * from ArchiSubMaster where SubID='" + ddlsubcode.SelectedValue.ToString() + "' and CourseID='" + ddlSyllabus.SelectedValue.ToString() + "'";
        }
        SqlCommand cms = new SqlCommand(stcmss, con);
        SqlDataReader rd;
        rd = cms.ExecuteReader();
        while (rd.Read())
        {
            lblSubNamess.Text = rd["SubName"].ToString();
            lblMinMarsk.Text = rd["MinMark"].ToString();
            lblToMarks.Text = rd["MaxMark"].ToString();
            lblFirstMarks.Text = rd["First"].ToString();
        }
        rd.Close();
        rd.Dispose();
        
    }
    private DataSet grid()
    {
        SqlDataAdapter ad = new SqlDataAdapter("select * from Rechecking where SubID='" + ddlsubcode.SelectedValue + "' and Session='" + lblHiddenSeason.Text + "' and Remarks='No Change'", con);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        return ds;
    }
    protected void ddlSubCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Close(); con.Open();
        details();
        try
        {

            GridMarks.DataSource = grid();
            GridMarks.DataBind();
            if (GridMarks.Rows.Count == 0) btnSave.Enabled = false; else btnSave.Enabled = true;
            GridMarks.Focus();
            con.Close(); con.Dispose();
        }
        catch (SqlException ex)
        {
            //lblExceptionShowSubject.Text = ex.ToString();
        }
        
    }
    protected void ddlPart_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlPart.SelectedValue.ToString() == "PartI" | ddlPart.SelectedValue.ToString() == "PartII")
        {
            lblStreamCode.Text = "Tech";
            lblStreamName.Text = "Technician Engineering";
        }
        else if (ddlPart.SelectedValue.ToString() == "SectionA" | ddlPart.SelectedValue.ToString() == "SectionB")
        {
            lblStreamName.Text = "Associate Engineering";
            lblStreamCode.Text = "Asso";
        }
        showcourse();
        ddlsubcode.Focus();
    }
    protected void ddlCourse_SeelctedIndexchanged(object sender, EventArgs e)
    {
        string qyry = "";
        if (ddlCourse.SelectedValue.ToString() == "Civil")
        {
            qyry = "select distinct CourseID from CivilSubMaster";
        }
        else if (ddlCourse.SelectedValue.ToString() == "Architecture")
        {
            qyry = "select distinct CourseID from ArchiSubMaster";
        }
        SqlDataAdapter ad = new SqlDataAdapter(qyry, con);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        ddlSyllabus.DataSource = ds;
        ddlSyllabus.DataTextField = "CourseID";
        ddlSyllabus.DataValueField = "CourseID";
        ddlSyllabus.DataBind();
        showcourse();
        ddlPart.Focus();
    }
    string cmd;
    private void showcourse()
    {
        try
        {
            if (ddlPart.SelectedValue.ToString() == "PartI" | ddlPart.SelectedValue.ToString() == "PartII")
            {
                lblStreamCode.Text = "Tech";
                lblStreamName.Text = "Technician Engineering";
            }
            else if (ddlPart.SelectedValue.ToString() == "SectionA" | ddlPart.SelectedValue.ToString() == "SectionB")
            {
                lblStreamName.Text = "Associate Engineering";
                lblStreamCode.Text = "Asso";
            }

            if (ddlCourse.SelectedValue.ToString() == "Civil")
            {
                cmd = "select * from CivilSubMaster where Section='" + ddlPart.SelectedValue.ToString() + "' and CourseID='" + ddlSyllabus.SelectedValue.ToString() + "'";
            }
            else if (ddlCourse.SelectedValue.ToString() == "Architecture")
            {
                cmd = "select * from ArchiSubMaster where Section='" + ddlPart.SelectedValue.ToString() + "' and CourseID='" + ddlSyllabus.SelectedValue.ToString() + "'";
            }
            SqlDataAdapter ad = new SqlDataAdapter(cmd, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            ddlsubcode.DataSource = dt;
            ddlsubcode.DataValueField = "SubID";
            ddlsubcode.DataTextField = "SubID";
            ddlsubcode.DataBind();
        }
        catch (SqlException ex)
        {
        }
        finally
        {
        }
    }
   protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            SqlCommand cmd = new SqlCommand();
            GridMarks.AllowPaging = false;
            string strStatus = "";
            con.Close(); con.Open();
            for (int i = 0; i < GridMarks.Rows.Count; i++)
            {
                TextBox box = (TextBox)GridMarks.Rows[i].Cells[3].FindControl("txtObtainMarks");
                if (box.Text != "")
                {
                    try
                    {
                        if (box.Text == "A")
                        {
                            strStatus = "Fail";
                        }
                        else if (box.Text == "EXMP" | box.Text == "exmp" | box.Text == "Exmp")
                        {
                            strStatus = "Fail";
                        }
                        else if (box.Text == "UFM" | box.Text == "ufm" | box.Text == "Ufm")
                        {
                            strStatus = "Fail";
                        }
                        else if (Convert.ToInt32(box.Text.ToString()) < 50)
                        {
                            strStatus = "Fail";
                        }
                        else if (Convert.ToInt32(box.Text.ToString()) >= 50)
                        {
                            strStatus = "Pass";
                        }
                    }
                    catch (FormatException ex)
                    {
                        strStatus = "Fail";
                    }
                        cmd = new SqlCommand("update SExamMarks set GetMarks='" + box.Text + "',Status='"+strStatus+"' where RollNo='" + GridMarks.Rows[i].Cells[2].Text.ToString() + "' and SubID='" + GridMarks.Rows[i].Cells[3].Text.ToString() + "' and ExamSeason='"+lblHiddenSeason.Text+"'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("update Rechecking set NewMarks='" + box.Text + "',Remarks='Updated',Status='" + strStatus + "' where RollNo='" + GridMarks.Rows[i].Cells[2].Text.ToString() + "' and SubID='" + GridMarks.Rows[i].Cells[3].Text.ToString() + "' and Session='" + lblHiddenSeason.Text + "' ", con);
                        cmd.ExecuteNonQuery();
                    
                }
            }
            btnSave.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('Marks Successfully Submitted.')", true);
        }
        catch (SqlException ex)
        {
            //lblExceptionShowSubject.Text = ex.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            
        }
    }
   
         protected void GridMarks_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                GridMarks.PageIndex = e.NewPageIndex;
                GridMarks.DataBind();
            }
         protected void txtYearSeason_TextChanged(object sender, EventArgs e)
         {
             lblHiddenSeason.Text = ddlExamSeason.SelectedValue.ToString() + "" + txtYearSeason.Text.ToString();
             //txtRollNo.Focus();
         }
         protected void ddlExamSeason_SelectedIndexChanged(object sender, EventArgs e)
         {
             lblHiddenSeason.Text = ddlExamSeason.SelectedValue.ToString() + "" + txtYearSeason.Text.ToString();
             txtYearSeason.Focus();
         }
         protected void GridMarks_SelectedIndexChanged(object sender, EventArgs e)
         {

         }
}
