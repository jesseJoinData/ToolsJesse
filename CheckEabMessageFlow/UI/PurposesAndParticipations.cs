namespace CheckEabMessageFlow;

public partial class PurposesAndParticipations : Form
{

    private CheckFlow checkFlowInstance = new CheckFlow();
    public PurposesAndParticipations()
    {
        InitializeComponent();
    }

    public async Task CheckInfo(string customerCode, string providingCompanyCode, string authorizedCompanyCode, List<Form1.DataSets> dataSets, string providerName)
    {
        await checkFlowInstance.CheckPurposesAndParticipations(customerCode, providingCompanyCode, authorizedCompanyCode, dataSets, providerName);
        ShowResults();
    }
}
