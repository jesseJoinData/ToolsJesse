namespace CheckEabMessageFlow1_0;

partial class Form1
{
    # region Generated Code
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    #endregion

    private CustomButtonCreator _buttonCreator;
    private CustomTextBoxCreator _textBoxCreator;
    private CustomLabelCreator _labelCreator;
    private CustomComboBoxCreator _customerComboBox;
    private CustomComboBoxCreator _providingComboBox;
    private CustomComboBoxCreator _authorizedComboBox;
    private Label _authorizedSelectedValue;
    private Label _providingSelectedValue;
    private TextBox _customer;
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Form1";

        _buttonCreator = new CustomButtonCreator(this);
        _textBoxCreator = new CustomTextBoxCreator(this);
        _labelCreator = new CustomLabelCreator(this);
        _customerComboBox = new CustomComboBoxCreator(this);
        _providingComboBox = new CustomComboBoxCreator(this);
        _authorizedComboBox = new CustomComboBoxCreator(this);

        int numberOfTextBoxes = 3;
        for (int i = 0; i < numberOfTextBoxes; i++)
        {
            _labelCreator.CreateLabel("customerCode: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            _customer = _textBoxCreator.CreateTextBox("53386051", new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50));
            i++;
            _labelCreator.CreateLabel("authorizedCompanyCode: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            _authorizedComboBox.CreateComboBox("authorizedCompany", new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50), companyList, new EventHandler(UpdateLabel));
            _authorizedSelectedValue = _labelCreator.CreateLabel(_authorizedComboBox.SelectedValue, new Point(Left + 10 + 200 + 200, (Top + (30 + 5 + i * 30))), new Size(200, FontHeight));
            i++;
            _labelCreator.CreateLabel("providingCompanyCode: ", new Point(Left + 10, (Top + (30 + i * 30))), new Size(200, FontHeight));
            _providingComboBox.CreateComboBox("providingCompany", new Point((Left + 10 + 200), (Top + (30 + i * 30))), new Size(200, 50), companyList, new EventHandler(UpdateLabel));
            _labelCreator.CreateLabel(_providingComboBox.SelectedValue, new Point(Left + 10 + 200 + 200, (Top + (30 + 5 + i * 30))), new Size(200, FontHeight));
            i++;
        }

        _buttonCreator.CreateButton("Check", new Point((Width / 2) - 100, (Height / 6) * 5 - 40), new Size(200, 40), new EventHandler(Checkclicked));
    }

    private void UpdateLabel(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        if (comboBox.Name == "authorizedCompany")
        {
            if (_authorizedComboBox.SelectedValue != null && _authorizedComboBox.SelectedValue != "")
            {
                _authorizedSelectedValue.Text = _authorizedComboBox.SelectedValue;
            }
            else
            {
                _authorizedSelectedValue.Text = "No Kvk Found";
            }
        }

        if (comboBox.Name == "providingCompany")
        {
            if (_providingComboBox.SelectedValue != null && _providingComboBox.SelectedValue != "")
            {
                _providingSelectedValue.Text = _providingComboBox.SelectedValue;
            }
            else
            {
                _providingSelectedValue.Text = "No Kvk Found";
            }
        }
    }

    private void Checkclicked(object sender, EventArgs e)
    {
        CheckDataFlow(_authorizedComboBox.SelectedValue, _providingComboBox.SelectedValue, _customer.Text);
    }

}
