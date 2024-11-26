using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AttorneyNetwork.Services;

namespace AttorneyNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Initialized componets to set the default values
            cbxCasesWonRegistration.SelectedIndex = 0;
            cbxType.SelectedIndex = 0;
            cbxManagening.SelectedIndex = 0;
            cbxSearchTypeWork.SelectedIndex = 0;
            dtpFundationDateRecords.Format = DateTimePickerFormat.Custom;
            dtpFundationDateRecords.CustomFormat = "MM-dd-yyyy";
            dtpFundationDateRegistration.Format = DateTimePickerFormat.Custom;
            dtpFundationDateRegistration.CustomFormat = "MM-dd-yyyy";
            dtpEndDate.Format = DateTimePickerFormat.Custom;
            dtpEndDate.CustomFormat = "MM-dd-yyyy";
            dtpStartDate.Format = DateTimePickerFormat.Custom;
            dtpStartDate.CustomFormat = "MM-dd-yyyy";
            rdAttorneyCriminalRegistration.Checked = true;
        }



        private void cbxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxType.SelectedIndex)
            {
                case 0:
                    pnlAttornneyRegistration.Visible = true;
                    pnlLawFirmRegistration.Visible = false;
                    break;
                case 1:
                    pnlAttornneyRegistration.Visible = false;
                    pnlLawFirmRegistration.Visible = true;
                    break;
            }

            txtAttorneyIdRegistration.Text = "";
            txtAttorneyNameRegistration.Text = "";
            rdAttorneyCriminalRegistration.Checked = false;
            rbAttorneyCivilRegistration.Checked = false;
            cbxCasesWonRegistration.SelectedIndex = 0;
            txtLawFirmNitRegistration.Text = "";
            txtLawFirmNameRegistration.Text = "";
            dtpFundationDateRegistration.Value = DateTime.Now;
        }

        private void txtAttorneyIDSearch_Click(object sender, EventArgs e)
        {
            string identifier = "Enter an ID to search";
            if (identifier == txtAttorneyIDSearch.Text)
                txtAttorneyIDSearch.Text = "";
        }
        private void txtLawfirmNitRecords_Click(object sender, EventArgs e)
        {
            string identifier = "Enter a NIT to search";
            if (identifier == txtLawfirmNitRecords.Text)
                txtLawfirmNitRecords.Text = "";
        }

        private void cbxManagening_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxManagening.SelectedIndex)
            {
                case 0:
                    pnlAttorneyRecord.Visible = true;
                    pnlLawFirmRecords.Visible = false;
                    pnlWorkRecords.Visible = false;
                    break;
                case 1:
                    pnlAttorneyRecord.Visible = false;
                    pnlLawFirmRecords.Visible = true;
                    pnlWorkRecords.Visible = false;
                    break;
                case 2:
                    pnlWorkRecords.Visible = true;
                    pnlAttorneyRecord.Visible = false;
                    pnlLawFirmRecords.Visible = false;
                    break;
            }
        }

        private void btnClearAttorneyRegistration_Click(object sender, EventArgs e)
        {
            switch (cbxType.SelectedIndex)
            {
                case 0:
                    txtAttorneyIdRegistration.Text = "";
                    txtAttorneyNameRegistration.Text = "";
                    rdAttorneyCriminalRegistration.Checked = false;
                    rbAttorneyCivilRegistration.Checked = false;
                    cbxCasesWonRegistration.SelectedIndex = 0;
                    break;
                case 1:
                    txtLawFirmNitRegistration.Text = "";
                    txtLawFirmNameRegistration.Text = "";
                    dtpFundationDateRegistration.Value = DateTime.Now;
                    break;
            }

        }
        private Services.AttorneyService attorneyService = new Services.AttorneyService();
        private Services.LawfirmService lawfirmService = new Services.LawfirmService();
        private void btnSaveAttorneyRegistration_Click(object sender, EventArgs e)
        {
            switch (cbxType.SelectedIndex)
            {
                case 0:
                    try
                    {
                        attorneyService.RegisterAttorney(new Services.AttorneyModel
                        {
                            AttorneyId = long.Parse(txtAttorneyIdRegistration.Text),
                            AttorneyName = txtAttorneyNameRegistration.Text,
                            AttorneyType = rdAttorneyCriminalRegistration.Checked ? "Criminal" : "Civil",
                            CasesWon = cbxCasesWonRegistration.SelectedItem.ToString()
                        });
                        MessageBox.Show("Attorney registered successfully");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 1:
                    try
                    {
                        if (dtpFundationDateRegistration.Value > DateTime.UtcNow.Date)
                            throw new Exception("Fundation date can't be greater than today");

                        lawfirmService.RegisterLawfirm(new Services.LawfirmModel
                        {
                            LawFirmNit = long.Parse(txtLawFirmNitRegistration.Text),
                            LawFirmName = txtLawFirmNameRegistration.Text,
                            FundationDate = dtpFundationDateRegistration.Value
                        });
                        MessageBox.Show("Law firm successfully registered", "Registration");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
            }
        }

        private AttorneyModel tempAttorney;
        private LawfirmModel lawfirmTemp;
        private DataSet workTemp;
        private void btnSearchAttorney_Click(object sender, EventArgs e)
        {
            switch (cbxManagening.SelectedIndex)
            {
                case 0:
                    try
                    {
                        tempAttorney = attorneyService.GetAttorneyById(long.Parse(txtAttorneyIDSearch.Text));
                        if (tempAttorney == null)
                            throw new Exception("Attorney not found");

                        txtAttorneyNameRecords.Text = tempAttorney.AttorneyName;
                        rbCriminalAttorneyRecords.Checked = tempAttorney.AttorneyType == "Criminal";
                        rbCivilAttorneyRecords.Checked = !rbCriminalAttorneyRecords.Checked;
                        cbxAttorneyCasesWon.SelectedItem = tempAttorney.CasesWon;

                        txtAttorneyNameRecords.Enabled = true;
                        rbCriminalAttorneyRecords.Enabled = true;
                        rbCivilAttorneyRecords.Enabled = true;
                        cbxAttorneyCasesWon.Enabled = true;
                        btnUpdateRecords.Enabled = true;
                        btnDeleteRecords.Enabled = true;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 1:
                    try
                    {
                        lawfirmTemp = lawfirmService.GetLawfirmModelById(long.Parse(txtLawfirmNitRecords.Text));
                        if (lawfirmTemp == null)
                            throw new Exception("Law firm not found");

                        txtLawFirmNameRecords.Text = lawfirmTemp.LawFirmName;
                        dtpFundationDateRecords.Value = lawfirmTemp.FundationDate;

                        txtLawFirmNameRecords.Enabled = true;
                        dtpFundationDateRecords.Enabled = true;
                        btnUpdateRecords.Enabled = true;
                        btnDeleteRecords.Enabled = true;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 2:
                    switch (cbxSearchTypeWork.SelectedIndex)
                    {
                        case 0:
                            try
                            {
                                btnUpdateRecords.Enabled = false;
                                btnDeleteRecords.Enabled = false;
                                workTemp = workService.GetworksByDate(dtpWorkSearchHelper.Value);
                                if (workTemp == null) throw new Exception("No data found");
                                dgvWorkRecords.Rows.Clear();
                                if (workTemp.Tables[0].Rows.Count == 0)
                                    throw new Exception("No data found for this date: " + dtpWorkSearchHelper.Value.ToString("MM/dd/yyyy"));

                                for (int i = 0; i < workTemp.Tables[0].Rows.Count; i++)
                                {
                                    dgvWorkRecords.Rows.Add();
                                    for (int j = 0; j < workTemp.Tables[0].Columns.Count; j++)
                                    {
                                        dgvWorkRecords.Rows[i].Cells[j].Value = workTemp.Tables[0].Rows[i][j];
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message);
                            }
                            break;
                        case 1:
                            try
                            {
                                btnUpdateRecords.Enabled = false;
                                btnDeleteRecords.Enabled = false;
                                workTemp = workService.GetworksByAttorneyId(long.Parse(txtSearchingWorkHelper.Text));
                                if (workTemp == null) throw new Exception("No data found");
                                dgvWorkRecords.Rows.Clear();
                                if (workTemp.Tables[0].Rows.Count == 0)
                                    throw new Exception("No data found for this attorney id " + txtSearchingWorkHelper.Text);

                                for (int i = 0; i < workTemp.Tables[0].Rows.Count; i++)
                                {
                                    dgvWorkRecords.Rows.Add();
                                    for (int j = 0; j < workTemp.Tables[0].Columns.Count; j++)
                                    {
                                        dgvWorkRecords.Rows[i].Cells[j].Value = workTemp.Tables[0].Rows[i][j];
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message);
                            }
                            break;
                        case 2:
                            try
                            {
                                btnUpdateRecords.Enabled = false;
                                btnDeleteRecords.Enabled = false;
                                workTemp = workService.GetworksByLawFirmNit(long.Parse(txtSearchingWorkHelper.Text));
                                if (workTemp == null) throw new Exception("No data found");
                                dgvWorkRecords.Rows.Clear();
                                if (workTemp.Tables[0].Rows.Count == 0)
                                    throw new Exception("No data found for this law firm Nit " + txtSearchingWorkHelper.Text);

                                for (int i = 0; i < workTemp.Tables[0].Rows.Count; i++)
                                {
                                    dgvWorkRecords.Rows.Add();
                                    for (int j = 0; j < workTemp.Tables[0].Columns.Count; j++)
                                    {
                                        dgvWorkRecords.Rows[i].Cells[j].Value = workTemp.Tables[0].Rows[i][j];
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message);
                            }
                            break;
                    }
                    
                    break;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            switch (cbxManagening.SelectedIndex)
            {
                case 0:
                    try
                    {
                        if (tempAttorney == null)
                            throw new Exception("User doesn't allow to edit");
                        attorneyService.UpdateAttorney(new Services.AttorneyModel
                        {
                            AttorneyId = tempAttorney.AttorneyId,
                            AttorneyName = txtAttorneyNameRecords.Text,
                            AttorneyType = rbCriminalAttorneyRecords.Checked ? "Criminal" : "Civil",
                            CasesWon = cbxAttorneyCasesWon.SelectedItem.ToString()
                        });
                        MessageBox.Show("Successfully updated attorney", "Succesffully",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 1:
                    try
                    {

                        if (lawfirmTemp == null)
                            throw new Exception("User doesn't allow to edit");
                        lawfirmService.UpdateLawFirm(new LawfirmModel()
                        {
                            LawFirmNit = lawfirmTemp.LawFirmNit,
                            LawFirmName = txtLawFirmNameRecords.Text,
                            FundationDate = dtpFundationDateRecords.Value


                        });
                        MessageBox.Show("Successfully updated lawfirm", "Succesffully");
                    }
                    catch (Exception exception)
                    {
                        System.Windows.Forms.MessageBox.Show(exception.Message);
                    }

                    break;
            }
        }

        private void btnDeleteLawFirm_Click(object sender, EventArgs e)
        {
            switch (cbxManagening.SelectedIndex)
            {
                case 0:
                    try
                    {
                        if (tempAttorney == null)
                            throw new Exception("User doesn't allow to delete");
                        DialogResult result = MessageBox.Show("Are you sure about that?", "Delete Account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                        attorneyService.DeleteAttorneyById(long.Parse(txtAttorneyIDSearch.Text));
                        MessageBox.Show("Successfully deleted attorney", "Succesffully",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 1:
                    try
                    {
                        if (lawfirmTemp == null)
                            throw new Exception("User doesn't allow to delete");
                        DialogResult result = MessageBox.Show("Are you sure about that?", "Delete Account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                        lawfirmService.DeleteLawFirm(long.Parse(txtLawfirmNitRecords.Text));
                        MessageBox.Show("Successfully deleted lawfirm", "Succesffully",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
            }
        }

        private void tcGestor_Click(object sender, EventArgs e)
        {
            flowPanelAttorney.Controls.Clear();
            flowLawFirm.Controls.Clear();
            var attorneys = attorneyService.GetAttorneys();
            var lawfirms = lawfirmService.GetLawFirms();
            if (tcGestor.SelectedIndex == 1)
            {
                foreach (var item in attorneys)
                {
                    Button tempButton = new Button()
                    {
                        Text = item.AttorneyName,
                        Font = new Font("Arial", 12),
                        Height = 40,
                        Width = 250,
                        Tag = item.AttorneyId
                    };
                    tempButton.Click += (s, ev) =>
                    {
                        txtCurrentAttorney.Text = item.AttorneyId.ToString();
                    };
                    flowPanelAttorney.Controls.Add(tempButton);
                }

                foreach (var lawfirmModel in lawfirms)
                {
                    Button tempButton = new Button()
                    {
                        Text = lawfirmModel.LawFirmName,
                        Font = new Font("Arial", 12),
                        Height = 40,
                        Width = 250,
                        Tag = lawfirmModel.LawFirmNit
                    };
                    tempButton.Click += (s, ev) =>
                    {
                        txtCurrentLawfirm.Text = lawfirmModel.LawFirmNit.ToString();
                    };
                    flowLawFirm.Controls.Add(tempButton);
                }
            }

        }

        WorkService workService = new WorkService();
        private void btnAdvanceOpt_Click(object sender, EventArgs e)
        {
            btnAdvanceOpt.Text = txtCurrentAttorney.Enabled == false ? "Disable advance options" : "Enable advance options";
            txtCurrentAttorney.Enabled = !txtCurrentAttorney.Enabled;
            txtCurrentLawfirm.Enabled = !txtCurrentLawfirm.Enabled;
        }

        private void btnNotEndDate_Click(object sender, EventArgs e)
        {
            btnNotEndDate.Text = dtpEndDate.Enabled == true ? "Enable" : "Disable";
            dtpEndDate.Enabled = !dtpEndDate.Enabled;
        }

        private void btnConfirmWork_Click(object sender, EventArgs e)
        {
            try
            {
                WorkModel model = new WorkModel()
                {
                    ATTORNEYID = long.Parse(txtCurrentAttorney.Text),
                    LAWFIRMNIT = long.Parse(txtCurrentLawfirm.Text),
                    STARTDATE = dtpStartDate.Value
                };
                if (dtpStartDate.Value > DateTime.UtcNow)
                    throw new Exception("Start date can't be greater than today");
                if (dtpEndDate.Enabled == true)
                {
                    if (dtpEndDate.Value < dtpStartDate.Value)
                        throw new Exception("End date can't be less than start date");
                    model.ENDDATE = dtpEndDate.Value;
                }
                workService.RegisterWork(model);
                MessageBox.Show("Successfully");
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message);
            }
        }

        private void cbxSearchTypeWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbxSearchTypeWork.SelectedIndex)
            {
                case 0:
                    dtpWorkSearchHelper.Visible = true;
                    txtSearchingWorkHelper.Visible = false;
                    break;
                case 1:
                    dtpWorkSearchHelper.Visible = false;  
                    txtSearchingWorkHelper.Visible = true;
                    break;
                case 2:
                    dtpWorkSearchHelper.Visible = false;
                    txtSearchingWorkHelper.Visible = true;
                    break;
            }
        }
    }
}
