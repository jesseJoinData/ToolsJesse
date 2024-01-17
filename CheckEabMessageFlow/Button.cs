public class CustomButtonCreator
{
    private Control form; // Reference to the form where buttons will be added

    public CustomButtonCreator(Control form)
    {
        this.form = form;
    }

    public void CreateButton(string buttonName, Point drawLocation, Size buttonSize, EventHandler clickEventHandler)
    {
        // Create a new button
        var newButton = new Button();

        // Set button properties
        newButton.Name = buttonName;
        newButton.Text = buttonName;
        newButton.Location = drawLocation;
        newButton.Size = buttonSize;

        // Attach the click event handler
        newButton.Click += clickEventHandler;

        // Add the button to the form's controls
        form.Controls.Add(newButton);
    }
}