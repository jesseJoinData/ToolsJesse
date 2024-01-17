using CheckEabMessageFlow1_0.Services;
using CheckEabMessageFlow1_0.Services.APIcall;
namespace CheckEabMessageFlow1_0;

public partial class Form1 : Form
{
    public List<Companies> companyList = new List<Companies>();
    public List<DataSets> dataSetsList = new List<DataSets>();
    CSVFileService csvFileService = new CSVFileService();
    GetDataSetsRequest getDataSets = new GetDataSetsRequest();
    public Form1()
    {
        companyList = csvFileService.LoadCompaniesFromFile();
        dataSetsList = getDataSets.LoadDataSetsWithApiCall().Result;
        InitializeComponent();
    }

    public void CheckDataFlow(string authorizedCompanyKvk, string providingCompanyKvk, string customerKvk)
    {
        
    }
}
