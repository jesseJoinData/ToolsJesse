namespace CheckEabMessageFlow;
partial class MappingAndSources
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
        this.Text = "Mapping and Sources";
    }

    private void ShowResults()
    {
        labelCreator = new CustomLabelCreator(this);
        textBoxCreator = new CustomTextBoxCreator(this);

        labelCreator.CreateLabel($"customer:", new Point(Left + 10, (Top)), new Size(150, FontHeight));
        labelCreator.CreateLabel($"ProvidingCompany:", new Point(Left + 10 + 150, (Top)), new Size(150, FontHeight));
        labelCreator.CreateLabel($"authorizedCompany:", new Point(Left + 10 + 300, (Top)), new Size(150, FontHeight));
        labelCreator.CreateLabel($"Source locations:", new Point(Left + 10 + 450, (Top)), new Size(150, FontHeight));

        labelCreator.CreateLabel($"{customer} ", new Point(Left + 10, (Top + (30))), new Size(100, FontHeight));
        for (int i = 0; i < checkFlowInstance.mappedToProvidingCompany.Count; i++)
        {
            labelCreator.CreateLabel($"{checkFlowInstance.mappedToProvidingCompany[i]}", new Point(Left + 10 + 150, (Top + (30 + (i * FontHeight + 5)))), new Size(150, FontHeight));
        }

        for (int i = 0; i < checkFlowInstance.mappedToAuthorizedCompany.Count; i++)
        {
            labelCreator.CreateLabel($"{checkFlowInstance.mappedToAuthorizedCompany[i]}", new Point(Left + 10 + 300, (Top + (30 + (i * FontHeight + 5)))), new Size(150, FontHeight));
        }

        for (int i = 0; i < checkFlowInstance.sourcesFound.Count; i++)
        {
            labelCreator.CreateLabel($"{checkFlowInstance.sourcesFound[i]}", new Point(Left + 10 + 450, (Top + (30 + (i * FontHeight + 5)))), new Size(checkFlowInstance.sourcesFound[i].Length * 10, FontHeight));
        }
    }
}
