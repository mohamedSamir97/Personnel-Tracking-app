using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using BLL;
using DAL;

namespace PersonalTracking
{
    public partial class FrmTask : Form
    {
        public FrmTask()
        {
            InitializeComponent();
        }

       

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        TASK task = new TASK();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (task.EmployeeID==0)
            {
                MessageBox.Show("Please select an employee from table");
            }
            else if(txtTitle.Text.Trim()=="")
            {
                MessageBox.Show("Task title is empty");
            }
            else if (txtContent.Text.Trim()=="")
            {
                MessageBox.Show("Content is empty");
            }
            else
            {
                if (!isUpdate)
                {
                    task.TaskTitle = txtTitle.Text;
                    task.TaskContent = txtContent.Text;
                    task.TaskStartDate = DateTime.Today;
                    task.TaskState = 1;
                    TaskBLL.AddTask(task);
                    MessageBox.Show("Task was added");
                    txtTitle.Clear();
                    txtContent.Clear();
                    task = new TASK();
                }
                else if (isUpdate)
                {
                    DialogResult result = MessageBox.Show("Are you sure?", "Warning!!", MessageBoxButtons.YesNo);
                    if (result==DialogResult.Yes)
                    {
                        TASK update = new TASK();
                        update.ID = detail.TaskID;
                        if (Convert.ToInt32(txtUserNo.Text) != detail.UserNo)
                            update.EmployeeID = task.EmployeeID;
                        else
                        {
                            update.EmployeeID = detail.EmployeeID;
                            update.TaskTitle = txtTitle.Text;
                            update.TaskContent = txtContent.Text;
                            update.TaskState = Convert.ToInt32(cmbTaskState.SelectedValue);
                            TaskBLL.UpdateTask(update);
                            MessageBox.Show("Task was updated");
                            this.Close();
                        }
                    }
                }
               
            }
        }
        TaskDTO dto = new TaskDTO();
        bool combofull = false;
        public bool isUpdate = false;
        public TaskDetailDTO detail = new TaskDetailDTO();
        private void FrmTask_Load(object sender, EventArgs e)
        {
            label8.Visible = false;
            cmbTaskState.Visible = false;
            dto = TaskBLL.GetAll();
            dataGridView1.DataSource = dto.Employees;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;

            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;

            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;

            dataGridView1.Columns[1].HeaderText = "User Number";
            dataGridView1.Columns[3].HeaderText = "Name";

            dataGridView1.Columns[4].HeaderText = "Surname";
 

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

            cmbTaskState.DataSource = dto.TaskState;
            cmbTaskState.DisplayMember = "StateName";
            cmbTaskState.ValueMember = "ID";
            cmbTaskState.SelectedIndex = -1;
            if (isUpdate)
            {
                label8.Visible = true;
                cmbTaskState.Visible = true;
                txtName.Text = detail.Name;
                txtUserNo.Text = detail.UserNo.ToString();
                txtSurname.Text = detail.Surname;
                txtTitle.Text = detail.Title;
                txtContent.Text = detail.Content;
                cmbTaskState.DataSource = dto.TaskState;
                cmbTaskState.DisplayMember = "StateName";
                cmbTaskState.ValueMember = "ID";
                cmbTaskState.SelectedValue = detail.TaskStateID;
            }

        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();

                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource = list.Where(x => x.DepartmentID ==
                  Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                
                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource = list.Where(x => x.PositionID ==
                  Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            task.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            
        }

    }
}
