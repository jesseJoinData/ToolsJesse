public class CustomTextBoxCreator
{
    private Control form; // Reference to the form where textBoxs will be added

    public CustomTextBoxCreator(Control form)
    {
        this.form = form;
    }

    public TextBox CreateTextBox(string textBoxName, Point drawLocation, Size textBoxSize, Boolean readOnly = false)
    {
        // Create a new textBox
        var newTextBox = new TextBox();

        // Set textBox properties
        newTextBox.Name = textBoxName;
        newTextBox.Text = textBoxName;
        newTextBox.Location = drawLocation;
        newTextBox.Size = textBoxSize;
        if (readOnly)
        {
            newTextBox.ReadOnly = true;
        }

        // Add the textBox to the form's controls
        form.Controls.Add(newTextBox);

        return newTextBox;
    }
}