namespace CheckEabMessageFlow;
partial class PurposesAndParticipations
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

    private CustomLabelCreator labelCreator;
    private CustomTextBoxCreator textBoxCreator;
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Purposes and participations";
    }

    private void ShowResults()
    {
        labelCreator = new CustomLabelCreator(this);
        textBoxCreator = new CustomTextBoxCreator(this);
        for (int i = 0; i < checkFlowInstance.mandatesList.Count; i++)
        {
            labelCreator.CreateLabel($"{checkFlowInstance.mandatesList[i]}", new Point(Left + 10 + 150, (Top + (30 + (i * FontHeight + 5)))), new Size(checkFlowInstance.mandatesList[i].Length * 10, FontHeight));
        }
    }
}
