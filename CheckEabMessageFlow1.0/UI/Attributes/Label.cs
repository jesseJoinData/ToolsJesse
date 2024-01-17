public class CustomLabelCreator
{
    private Control form; // Reference to the form where Labels will be added
    private Label? newLabel;

    public CustomLabelCreator(Control form)
    {
        this.form = form;
    }

    public Label CreateLabel(string labelName, Point drawLocation, Size labelSize)
    {
        var newLabel = new Label();
        ToolTip tooltip = new ToolTip();

        newLabel.Name = labelName;
        newLabel.Text = labelName;
        newLabel.Location = drawLocation;
        newLabel.Size = labelSize;
        tooltip.SetToolTip(newLabel, "Double click to copy");

        form.Controls.Add(newLabel);

        return newLabel;
    }
    public void RemoveLabel()
    {
        if (newLabel != null)
        {
            form.Controls.Remove(newLabel);
            newLabel.Dispose();
            newLabel = null;
        }
    }
}