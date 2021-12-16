﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using DAL;
using BLL;

namespace PersonalTracking
{
    public partial class FrmPermissionList : Form
    {
        public FrmPermissionList()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtDayAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDayAmount_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmPermission frm = new FrmPermission();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.PermissionID == 0)
                MessageBox.Show("Please select a permission from table");
            else if (detail.State == PermissionStates.Approved || detail.State == PermissionStates.Disapproved)
                MessageBox.Show("You can not update any approved or disapproved permission");
            else
            {
                FrmPermission frm = new FrmPermission();
                frm.isUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }
            
        }
        PermissionDTO dto = new PermissionDTO();
        private bool combofull=false;
        void FillAllData()
        {
            dto = PermissionBLL.GetAll();
            dataGridView1.DataSource = dto.Permission;
            combofull = false;

            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedValue = -1;
            cmbPosition.SelectedValue = -1;
            combofull = true;

            cmbState.DataSource = dto.States;
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "ID";
            cmbState.SelectedIndex = -1;
        }

        private void FrmPermissionList_Load(object sender, EventArgs e)
        {

            FillAllData();
            if (!UserStatic.IsAdmin)
                dto.Permission = dto.Permission.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "User Number";
            dataGridView1.Columns[2].HeaderText = "Name";
            dataGridView1.Columns[3].HeaderText = " Surname";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].HeaderText = "Start Date";
            dataGridView1.Columns[9].HeaderText = "End Date";
            dataGridView1.Columns[10].HeaderText = "Day Amount";
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].HeaderText = "State";
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            if (!UserStatic.IsAdmin)
            {
                pnlForAdmin.Visible = false;
                btnApprove.Hide();
                btnDisapproved.Hide();
                btnDelete.Hide();
                btnClose.Location = new Point(427,38);

            }



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<PermissionDetailDTO> list = dto.Permission;
            if (txtUserNo.Text.Trim() != "")
            {
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();

            }
            if (txtName.Text.Trim() != "")
            {
                list = list.Where(x => x.Name.ToUpper().Contains(txtName.Text.ToUpper())).ToList();

            }
            if (txtSurname.Text.Trim() != "")
            {
                list = list.Where(x => x.Surname.ToUpper().Contains(txtSurname.Text.ToUpper())).ToList();

            }
            if (cmbDepartment.SelectedIndex != -1)
            {
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();

            }

            if (cmbPosition.SelectedIndex != -1)
            {
                list = list.Where(x => x.PositionID == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();

            }
            if (rbStartDate.Checked)
            {
                list = list.Where(x => x.StartDate < Convert.ToDateTime(dpEnd.Value) &&
                  x.StartDate > Convert.ToDateTime(dpStart.Value)).ToList();
            }
            else if (rbEndDate.Checked)
            {
                list = list.Where(x => x.EndDate < Convert.ToDateTime(dpEnd.Value) &&
                x.EndDate > Convert.ToDateTime(dpStart.Value)).ToList();
            }
            if (cmbState.SelectedIndex != -1)
            {
                list = list.Where(x => x.State == Convert.ToInt32(cmbState.SelectedValue)).ToList();

            }
            if (txtDayAmount.Text.Trim() != "")
            {
                list = list.Where(x => x.PermissionDayAmoun == Convert.ToInt32(txtDayAmount.Text)).ToList();

            }

            dataGridView1.DataSource = list;
        }
        private void CleanFilters()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            rbStartDate.Checked = false;
            rbEndDate.Checked = false;
            cmbState.SelectedIndex = -1;
            txtDayAmount.Clear();
            dataGridView1.DataSource = dto.Permission;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }
        PermissionDetailDTO detail = new PermissionDetailDTO();
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.PermissionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[14].Value);
            detail.StartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
            detail.EndDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            detail.Explanation = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.State = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
            detail.PermissionDayAmoun = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[10].Value);

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionStates.Approved);
            MessageBox.Show("Approved");
            FillAllData();
            CleanFilters();
        }

        private void btnDisapproved_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionStates.Disapproved);
            MessageBox.Show("Dispproved");
            FillAllData();
            CleanFilters();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete this permission",
                "Warning", MessageBoxButtons.YesNo);
            if(result==DialogResult.Yes)
            {
                if (detail.State==PermissionStates.Approved || detail.State==PermissionStates.Disapproved)
                {
                    MessageBox.Show("You cannot delete approved or disapproved permissions");

                }
                else
                {
                    PermissionBLL.DeletePermission(detail.PermissionID);
                    MessageBox.Show("Permission was deleted");
                    FillAllData();
                    CleanFilters();
                }
            }
        }

        private void txtExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }
    }
}
