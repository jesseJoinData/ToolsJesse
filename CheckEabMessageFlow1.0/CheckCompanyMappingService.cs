using CheckEabMessageFlow1_0;
namespace CheckEabMessageFlow1_0.Services;

public class CheckCompanyMappingService
{
    private static string _RVOKvk = "27378529";
    private List<CompanyMapping> ProvidingCompanyMappingList = new List<CompanyMapping>();
    private List<CompanyMapping> AuthorizedCompanyMappingList = new List<CompanyMapping>();
    private List<CompanyMapping> UbnMappingList = new List<CompanyMapping>();
    private string _authorizedCompanyKvk = "";
    private string _providingCompanyKvk = "";
    private string _customerKvk = "";
    public async Task CheckInput(string authorizedCompanyKvk, string providingCompanyKvk, string customerKvk)
    {
        if ((authorizedCompanyKvk.Length == 8) && (providingCompanyKvk.Length == 8) && (customerKvk.Length == 8))
        {
            _authorizedCompanyKvk = authorizedCompanyKvk;
            _providingCompanyKvk = providingCompanyKvk;
            _customerKvk = customerKvk;
            // do the right things
        }
        else
        {
            // provide message input is not okay.
        }
    }

    private async Task GetCompanyMapping()
    {
        using (HttpClient client = new HttpClient())
        {

            List<Task<HttpResponseMessage>> getCompanyMappingTasks = new List<Task<HttpResponseMessage>>();
            getCompanyMappingTasks =
            [
                GetCompanyMappingByOwner(client, _authorizedCompanyKvk, _customerKvk),
                GetCompanyMappingByOwner(client, _providingCompanyKvk, _customerKvk),
                GetCompanyMappingByOwner(client, _RVOKvk,_customerKvk)
            ];
        }
    }

    private async Task<HttpResponseMessage> GetCompanyMappingByOwner(HttpClient httpClient, string OwnerCompany, string TargetCompany)
    {

        return;
    }
}