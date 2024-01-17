using CheckEabMessageFlow;

public class CustomComboBoxCreator
{
    private Control form; // Reference to the form where comboBoxs will be added
    private AutoCompleteStringCollection autoCompleteOptions;
    private List<Companies> companyList = new List<Companies> { };
    public string SelectedValue { get; private set; } = "";
    public string SelectedName { get; private set; } = "";
    public string SelectedClientID { get; private set; } = "";
    public string SelectedClientSecret { get; private set; } = "";
    public CustomComboBoxCreator(Control form)
    {
        this.form = form;
    }

    public ComboBox CreateComboBox(string comboBoxName, Point drawLocation, Size comboBoxSize, List<Companies> comboBoxList)
    {
        companyList = comboBoxList;
        List<string> companyName = new List<string> { };
        for (int i = 0; i < companyList.Count; i++)
        {
            companyName.Add(companyList[i].CompanyName);
        }

        // Create a new comboBox
        var newComboBox = new ComboBox();

        // Set comboBox properties
        newComboBox.Name = comboBoxName;
        newComboBox.Text = comboBoxName;
        newComboBox.Location = drawLocation;
        newComboBox.Size = comboBoxSize;

        newComboBox.Items.AddRange(companyName.ToArray());
        newComboBox.AutoCompleteMode = AutoCompleteMode.None;
        newComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        autoCompleteOptions = new AutoCompleteStringCollection();
        autoCompleteOptions.AddRange(companyName.ToArray());

        // ! tells compiler I am certain a null reference wont be given
        newComboBox.TextChanged += SearchFieldChanged!;
        newComboBox.DropDown += ManualDropDownAction!;
        newComboBox.SelectedIndexChanged += SelectionChanged!;
        // ! tells compiler I am certain a null reference wont be given

        form.Controls.Add(newComboBox);

        return newComboBox;
    }

    public ComboBox CreateComboBox(string comboBoxName, Point drawLocation, Size comboBoxSize, List<Companies> comboBoxList, EventHandler labelUpdate)
    {
        companyList = comboBoxList;
        List<string> companyName = new List<string> { };
        for (int i = 0; i < companyList.Count; i++)
        {
            companyName.Add(companyList[i].CompanyName);
        }

        // Create a new comboBox
        var newComboBox = new ComboBox();

        // Set comboBox properties
        newComboBox.Name = comboBoxName;
        newComboBox.Text = comboBoxName;
        newComboBox.Location = drawLocation;
        newComboBox.Size = comboBoxSize;

        newComboBox.Items.AddRange(companyName.ToArray());
        newComboBox.AutoCompleteMode = AutoCompleteMode.None;
        newComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        autoCompleteOptions = new AutoCompleteStringCollection();
        autoCompleteOptions.AddRange(companyName.ToArray());

        // ! tells compiler I am certain a null reference wont be given
        newComboBox.TextChanged += SearchFieldChanged!;
        newComboBox.DropDown += ManualDropDownAction!;
        newComboBox.SelectedIndexChanged += SelectionChanged!;
        newComboBox.SelectedIndexChanged += labelUpdate;
        // ! tells compiler I am certain a null reference wont be given

        form.Controls.Add(newComboBox);

        return newComboBox;
    }

    private void ManualDropDownAction(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        comboBox.Items.Clear();
        comboBox.SelectAll();

        foreach (string option in autoCompleteOptions)
        {
            comboBox.Items.Add(option);
        }
    }

    private void SearchFieldChanged(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        comboBox.Items.Clear();
        string currentText = comboBox.Text;
        comboBox.SelectionStart = currentText.Length;
        comboBox.SelectionLength = 0; // Set to 0 to ensure the cursor is at the end

        // Add items that match the search text
        foreach (Companies item in companyList)
        {
            if (item.CompanyName.ToLower().Contains(comboBox.Text.ToLower()))
            {
                comboBox.Items.Add(item.CompanyName);
            }
        }
        if (comboBox.DroppedDown == false)
        {
            comboBox.DroppedDown = true;
            comboBox.Text = currentText;
        }
    }

    private void SelectionChanged(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        for (int i = 0; i < companyList.Count; i++)
        {
            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == companyList[i].CompanyName)
            {
                SelectedValue = companyList[i].KvkNumber;
                SelectedName = companyList[i].CompanyName;
                SelectedClientID = companyList[i].ClientID;
                SelectedClientSecret = companyList[i].ClientSecret;
            }
        }
    }
}