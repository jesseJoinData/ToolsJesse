namespace CheckEabMessageFlow;

public partial class MappingAndSources : Form
{

    private CheckFlow checkFlowInstance = new CheckFlow();
    private string? customer = "";
    public MappingAndSources()
    {
        InitializeComponent();
    }

    public async Task CheckInfo(string customerCode, string providingCompanyCode, string authorizedCompanyCode)
    {
        customer = customerCode;
        await checkFlowInstance.CheckCompanyMappingAndSources(customerCode, providingCompanyCode, authorizedCompanyCode);
        ShowResults();
    }
}
