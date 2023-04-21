using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMS_lab2
{
    public partial class Form1 : Form
    {
        SqlConnection cs = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        SqlDataAdapter daParent = new SqlDataAdapter();
        SqlDataAdapter daChild = new SqlDataAdapter();
        DataSet dsParent = new DataSet();
        DataSet dsChild = new DataSet();
        BindingSource bsParent = new BindingSource();
        BindingSource bsChild = new BindingSource();

        string ParentID = ConfigurationManager.AppSettings["ParentID"];
        string ChildID = ConfigurationManager.AppSettings["ChildID"];
        List<string> ChildColumnsList = new List<string>(ConfigurationManager.AppSettings["ChildColumns"].Split(','));
        string SelectParent = ConfigurationManager.AppSettings["SelectParent"];
        string SelectChild = ConfigurationManager.AppSettings["SelectChild"];
        string AddChild = ConfigurationManager.AppSettings["AddChild"];
        string RemoveChild = ConfigurationManager.AppSettings["RemoveChild"];
        string UpdateChild = ConfigurationManager.AppSettings["UpdateChild"];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int pointY = 0;
            pnlChild.Controls.Clear();

            foreach (string column in ChildColumnsList)
            {
                Label lbl = new Label();
                lbl.Text = column;
                lbl.AutoSize = true;
                lbl.Location = new Point(0, pointY);

                TextBox txt = new TextBox();
                txt.Name = column;
                txt.Location = new Point(90, pointY);

                pnlChild.Controls.Add(lbl);
                pnlChild.Controls.Add(txt);
                pnlChild.Show(); 

                pointY += 20;
            }

            pointY += 10;

            Label l = new Label();
            l.Text = "Change " + ParentID;
            l.AutoSize = true;
            l.Location = new Point(0, pointY);

            TextBox t = new TextBox();
            t.Name = ParentID;
            t.Width = 50;
            t.Location = new Point(120, pointY);

            pnlChild.Controls.Add(l);
            pnlChild.Controls.Add(t);
            pnlChild.Show();
        }

        private void clearTextboxes()
        {
            foreach (string column in ChildColumnsList)
            {
                TextBox textBox = (TextBox)pnlChild.Controls[column];
                textBox.Text = "";
            }
            TextBox txt = (TextBox)pnlChild.Controls[ParentID];
            txt.Text = "";
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            daParent.SelectCommand = new SqlCommand(SelectParent, cs);

            dsParent.Clear(); 
            daParent.Fill(dsParent); 
            dataParent.DataSource = dsParent.Tables[0];
            bsParent.DataSource = dsParent.Tables[0];
        }

        private void dataParent_SelectionChanged(object sender, DataGridViewCellMouseEventArgs e)
        {
            SqlCommand cmd = new SqlCommand(SelectChild, cs);

            cmd.Parameters.AddWithValue("@" + ParentID, dataParent.CurrentRow.Cells[0].Value);

            daChild.SelectCommand = cmd;

            dsChild.Clear();
            daChild.Fill(dsChild);
            dataChild.DataSource = dsChild.Tables[0];
            bsChild.DataSource = dsChild.Tables[0];

            clearTextboxes();
        }
        
        private void dataChild_SelectionChanged(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i = 2;
            foreach (string column in ChildColumnsList)
                pnlChild.Controls[column].Text = dataChild.CurrentRow.Cells[i++].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(AddChild, cs);

                cmd.Parameters.AddWithValue("@" + ParentID, dataParent.CurrentRow.Cells[0].Value);

                foreach (string column in ChildColumnsList)
                {
                    TextBox textBox = (TextBox)pnlChild.Controls[column];
                    cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                }

                cs.Open();
                cmd.ExecuteNonQuery();
                cs.Close();

                MessageBox.Show("Record was added!");
                clearTextboxes();

                dsChild.Clear();
                daChild.Fill(dsChild);
                dataChild.DataSource = dsChild.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cs.Close();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Are you sure?", "Confirm removal", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand(RemoveChild, cs);

                cmd.Parameters.AddWithValue("@" + ChildID, dataChild.CurrentRow.Cells[0].Value);

                cs.Open();
                cmd.ExecuteNonQuery();
                cs.Close();

                MessageBox.Show("Record was removed!");
                clearTextboxes();

                dsChild.Clear();
                daChild.Fill(dsChild);
                dataChild.DataSource = dsChild.Tables[0];
            }
            else
                MessageBox.Show("Record was not removed!");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(UpdateChild, cs);

                cmd.Parameters.AddWithValue("@" + ChildID, dataChild.CurrentRow.Cells[0].Value);

                TextBox txt = (TextBox)pnlChild.Controls[ParentID];
                if (txt.Text == "")
                    cmd.Parameters.AddWithValue("@" + ParentID, dataParent.CurrentRow.Cells[0].Value);
                else
                    cmd.Parameters.AddWithValue("@" + ParentID, txt.Text);

                foreach (string column in ChildColumnsList)
                {
                    TextBox textBox = (TextBox)pnlChild.Controls[column];
                    cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                }

                cs.Open();
                cmd.ExecuteNonQuery();
                cs.Close();

                MessageBox.Show("Record was updated!");
                clearTextboxes();

                dsChild.Clear();
                daChild.Fill(dsChild);
                dataChild.DataSource = dsChild.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cs.Close();
            }
        }
    }
}
