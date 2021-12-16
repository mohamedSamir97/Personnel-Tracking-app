﻿using DAL;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalTracking
{
    public partial class FrmDepartment : Form
    {
        public FrmDepartment()
        {
            InitializeComponent();
        }

        private void txtDepartment_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (txtDepartment.Text.Trim()=="")
            {
                MessageBox.Show("Please fill the name field");
            }
            else
            {
                DEPARTMENT department = new DEPARTMENT();
                if (!isUpdate)
                {
                    department.DepartmentName = txtDepartment.Text;
                    BLL.DepartmentBLL.AddDepartment(department);
                    MessageBox.Show("Department was added successfully");
                    txtDepartment.Clear();
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);
                    if (DialogResult.Yes==result)
                    {
                        department.ID = detail.ID;
                        department.DepartmentName = txtDepartment.Text;
                        DepartmentBLL.UpdateDepartment(department);
                        MessageBox.Show("Department was updated");
                        this.Close();
                    }
                }
                
            }
           
        }
        public bool isUpdate = false;
        public DEPARTMENT detail = new DEPARTMENT();

        private void FrmDepartment_Load(object sender, EventArgs e)
        {
            if (isUpdate)
                txtDepartment.Text = detail.DepartmentName;
        }
    }
}
