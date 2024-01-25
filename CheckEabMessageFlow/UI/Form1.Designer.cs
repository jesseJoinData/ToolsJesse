using System.Data;
using System.Dynamic;

namespace CheckEabMessageFlow;
partial class Form1
{
    #region Generated code
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    #endregion

    private CustomButtonCreator buttonCreator;
    private CustomTextBoxCreator textBoxCreator;
    private CustomLabelCreator labelCreator;
    private CustomComboBoxCreator customerComboBox;
    private CustomComboBoxCreator providingComboBox;
    private CustomComboBoxCreator authorizedComboBox;
    private TextBox customer;
    private Label authorizedSelectedValue;
    private Label providingSelectedValue;
    private Label providingSelectedClient;
    private Label authorizedSelectedClient;
    public string customerCompanyCode = "";
    public string providingCompanyCode = "";
    public string authorizedCompanyCode = "";
    public string providingCompanyName = "";
    private string startDateTime;
    private string endDateTime;
    private List<Companies> companyList = new List<Companies> { };
    private void InitializeComponent()
    {
        companyList = LoadCompaniesFromFile();
        dataSetsList = LoadDataSetsWithApiCall();
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Form1";

        buttonCreator = new CustomButtonCreator(this);
        textBoxCreator = new CustomTextBoxCreator(this);
        labelCreator = new CustomLabelCreator(this);
        customerComboBox = new CustomComboBoxCreator(this);
        providingComboBox = new CustomComboBoxCreator(this);
        authorizedComboBox = new CustomComboBoxCreator(this);

        int numberOfTextBoxes = 3;
        for (int i = 0; i < numberOfTextBoxes; i++)
        {
            labelCreator.CreateLabel("customerCode: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            customer = textBoxCreator.CreateTextBox("53386051", new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50));
            i++;
            labelCreator.CreateLabel("authorizedCompanyCode: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            authorizedComboBox.CreateComboBox("authorizedCompany", new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50), companyList, new EventHandler(UpdateLabel));
            authorizedSelectedValue = labelCreator.CreateLabel(authorizedComboBox.SelectedValue, new Point(Left + 10 + 200 + 200, (Top + (30 + 5 + i * 30))), new Size(200, FontHeight));
            authorizedSelectedClient = labelCreator.CreateLabel(authorizedComboBox.SelectedClientID, new Point(Left + 10 + 200 + 200 + 200, (Top + (30 + 5 + i * 30))), new Size(200, FontHeight));
            i++;
            labelCreator.CreateLabel("providingCompanyCode: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            providingComboBox.CreateComboBox("providingCompany", new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50), companyList, new EventHandler(UpdateLabel));
            providingSelectedValue = labelCreator.CreateLabel(providingComboBox.SelectedValue, new Point(Left + 10 + 200 + 200, (Top + (30 + 5 + i * 30))), new Size(200, FontHeight));
            providingSelectedClient = labelCreator.CreateLabel(providingComboBox.SelectedClientID, new Point(Left + 10 + 200 + 200 + 200, (Top + (30 + 5 + i * 30))), new Size(200, FontHeight));
            i++;
            labelCreator.CreateLabel("StartDateTime: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            textBoxCreator.CreateTextBox(startDateTime, new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50));
            i++;
            labelCreator.CreateLabel("EndDateTime: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            textBoxCreator.CreateTextBox(endDateTime, new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50));
            i++;
        }

        buttonCreator.CreateButton("Close Other Forms", new Point((Width / 4) - 100, (Height / 6) * 3 - 40), new Size(200, 40), new EventHandler(CloseOtherFormsClicked));
        buttonCreator.CreateButton("Check", new Point((Width / 2) - 100, (Height / 6) * 5 - 40), new Size(200, 40), new EventHandler(Checkclicked));
    }

    private void CloseOtherFormsClicked(object sender, EventArgs e)
    {
        CloseAllOtherForms();
    }
    private void Checkclicked(object sender, EventArgs e)
    {
        // Go get the info using these parameters function call getinfo(param1, param2, param3)
        if (IsNumeric(customer.Text) && authorizedComboBox.SelectedValue != null && providingComboBox.SelectedValue != null)
        {
            customerCompanyCode = customer.Text;
            authorizedCompanyCode = authorizedComboBox.SelectedValue;
            providingCompanyCode = providingComboBox.SelectedValue;
            providingCompanyName = providingComboBox.SelectedName;

            OpenMappingAndSources().ConfigureAwait(false);
            OpenPurposesAndParticipations().ConfigureAwait(false);
        }
        else if (customer.Text == "" && authorizedComboBox.SelectedValue == null && providingComboBox.SelectedValue == null)
        {
            MessageBox.Show("all are empty");
        }
        else
        {
            MessageBox.Show("One or more inputs are unacceptable");
        }
    }
    private bool IsNumeric(string text)
    {
        return int.TryParse(text, out _);
    }

    private void UpdateLabel(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        if (comboBox.Name == "authorizedCompany")
        {
            if (authorizedComboBox.SelectedValue != null && authorizedComboBox.SelectedValue != "")
            {
                authorizedSelectedValue.Text = authorizedComboBox.SelectedValue;
                authorizedSelectedClient.Text = authorizedComboBox.SelectedClientID;
            }
            else
            {
                authorizedSelectedValue.Text = "No Kvk Found";
            }
        }

        if (comboBox.Name == "providingCompany")
        {
            if (providingComboBox.SelectedValue != null && providingComboBox.SelectedValue != "")
            {
                providingSelectedValue.Text = providingComboBox.SelectedValue;
                providingSelectedClient.Text = providingComboBox.SelectedClientID;
            }
            else
            {
                providingSelectedValue.Text = "No Kvk Found";
            }
        }

    }
    private void CloseAllOtherForms()
    {
        foreach (Form frm in Application.OpenForms.Cast<Form>().ToList())
        {
            if (frm != this) // this ensures the current form stays open
            {
                frm.Close();
            }
        }
    }
}

