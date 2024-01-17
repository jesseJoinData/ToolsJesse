namespace CheckEabMessageFlow;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

public class Customer
{
    public string? KvkNumber { get; set; }
    public string? Value { get; set; }
}
public class Provider
{
    public string? KvkNumber { get; set; }
    public string? Name { get; set; }
}

public class ParticipationsForAuthorizedCompany
{
    public string? Status { get; set; }
    public List<string>? DataSets { get; set; }
    public string? LocationRestriction { get; set; }
}

public class ParticipationsForProvidingCompany
{
    public string? DataSetID { get; set; }
    public string? authorizedCompanyValue { get; set; }
}

public class CheckFlow
{
    #region API configuration
    private string clientID = "source-registry";
    private string clientSecret = "4029e2b9-d74b-4147-9086-df606c817fbf";
    private string purposeClientID = "purpose-registry-client";
    private string purposeClientSecret = "8eefee19-b193-4cc9-bf45-7f6e2b0aa8b6";
    private string joinDataKvk = "64039641";
    private string authUrl = "https://production.join-data.net/auth/realms/datahub/protocol/openid-connect/token"; // sourceRegistry authorization
    private string getSourcesUrl = "https://production.join-data.net/api/source-registry/v2/locations/nl.kvk/{0}/sources?provider-company-value={1}"; // 0 = boer 1 = providingCompany
    private string getFaktuurEabUrl = "https://production.join-data.net/api/broker/icar-ade/v1/locations/nl.kvk/{0}/faktuureab?start-date-time={1}T00:00:00.000Z&end-date-time={2}T00:00:00.000Z"; // 0 = boer 1 = startdatetime 2 = enddatetime
    private string companyMappingUsingOwnerUrl = "https://production.join-data.net/api/company-mapping-service/v1/company-mapping?owner-scheme=nl.kvk&owner-value={0}&target-scheme=nl.kvk&target-value={1}"; // 0 = ownercompany 1 = targetcompany
    private string getParticipationsUrl = "https://production.join-data.net/api/purpose-registry/v1/companies/nl.kvk/{0}/participations/authorized-companies/nl.kvk/{1}"; //0 = boer 1 = authorizedCompany
    private string getProviderRestrictionUrl = "https://production.join-data.net/api/purpose-registry/v1/companies/nl.kvk/{0}/participations/providing-companies/nl.kvk/{1}"; //0 = boer 1 = providingCompany

    #endregion

    #region Private variable initialisation
    private string authorizedCompany = "";

    #endregion

    #region Public variable initialisation
    public List<string> mappedToProvidingCompany { get; private set; } = new List<string>();
    public List<string> mappedToAuthorizedCompany { get; private set; } = new List<string>();
    public List<string> sourcesFound { get; private set; } = new List<string>();
    public List<string> mandatesList { get; private set; } = new List<string>();
    #endregion
    public Customer customer = new Customer();
    public Provider provider = new Provider();
    public List<Form1.DataSets> dataSetsList = new List<Form1.DataSets>();

    [STAThread]
    public async Task CheckCompanyMappingAndSources(string customerCode, string providingCompanyCode, string authorizedCompanyCode)
    {
        customer.KvkNumber = customerCode;
        provider.KvkNumber = providingCompanyCode;
        authorizedCompany = authorizedCompanyCode;
        List<Task> parralelTasks = new List<Task>();
        if ((customer.KvkNumber.Length == 8) && (provider.KvkNumber.Length == 8) && authorizedCompany.Length == 8)
        {
            parralelTasks =
            [
            CheckCompanyMapping(),
            CheckSources()
            ];
            await Task.WhenAll(parralelTasks);
        }
        else
        {
            MessageBox.Show("invalid value entered, must be 8 numbers or empty");
        }
    }

    public async Task CheckPurposesAndParticipations(string customerCode, string providingCompanyCode, string authorizedCompanyCode, List<Form1.DataSets> dataSets, string providerName)
    {
        dataSetsList = dataSets;
        customer.KvkNumber = customerCode;
        provider.KvkNumber = providingCompanyCode;
        provider.Name = providerName;
        authorizedCompany = authorizedCompanyCode;

        List<Task> parralelTasks = new List<Task>();
        if ((customer.KvkNumber.Length == 8) && (provider.KvkNumber.Length == 8) && authorizedCompany.Length == 8)
        {
            parralelTasks =
            [
                CheckParticipationsWithProvidingCompany()
            ];
            await Task.WhenAll(parralelTasks);
        }
    }
    private async Task CheckCompanyMapping()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage? companyMappingProvidingCompany = null;
            HttpResponseMessage? companyMappingAuthorizedCompany = null;

            List<Task<HttpResponseMessage>> getCompanyMappingTasks = new List<Task<HttpResponseMessage>>();
            if (customer.KvkNumber != null)
            {
                getCompanyMappingTasks =
                [
                GetCompanyMappingUsingOwner(client, provider.KvkNumber, customer.KvkNumber),
                GetCompanyMappingUsingOwner(client, authorizedCompany, customer.KvkNumber),
                ];
            }

            await Task.WhenAll(getCompanyMappingTasks);

            if (getCompanyMappingTasks.Count > 0)
            {
                companyMappingProvidingCompany = getCompanyMappingTasks[0].Result;
            }
            if (getCompanyMappingTasks.Count > 1)
            {
                companyMappingAuthorizedCompany = getCompanyMappingTasks[1].Result;
            }

            List<Task> getValueOutOfMappingTasks = new List<Task>();

            if (companyMappingProvidingCompany != null)
            {
                if (companyMappingProvidingCompany.IsSuccessStatusCode)
                {
                    getValueOutOfMappingTasks.Add(GetValueUsingOwner(companyMappingProvidingCompany, mappedToProvidingCompany));
                }
                else
                {
                    MessageBox.Show($"Providing company error: {companyMappingProvidingCompany.StatusCode}");
                }
            }
            else
            {
                MessageBox.Show("providngcompany is bad");
            }
            if (companyMappingAuthorizedCompany != null)
            {
                if (companyMappingAuthorizedCompany.IsSuccessStatusCode)
                {
                    getValueOutOfMappingTasks.Add(GetValueUsingOwner(companyMappingAuthorizedCompany, mappedToAuthorizedCompany));
                }
                else
                {
                    MessageBox.Show($"Authorized company error: {companyMappingAuthorizedCompany.StatusCode}");
                }
            }
            else
            {
                MessageBox.Show("authorized is bad");
            }
            await Task.WhenAll(getValueOutOfMappingTasks);
        }
    }

    private async Task CheckSources()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage? sources = null;
            if (customer.KvkNumber != null)
            {
                sources = await GetDataSource(client, provider.KvkNumber, customer.KvkNumber);
            }
            if (sources != null && sources.IsSuccessStatusCode)
            {
                await ParseSources(sources, sourcesFound);
            }
        }
    }

    private async Task ParseSources(HttpResponseMessage httpResponseMsg, List<string> sourceList)
    {
        using (var stream = await httpResponseMsg.Content.ReadAsStreamAsync())
        using (var jsonDocument = await JsonDocument.ParseAsync(stream))
        {
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object)
            {
                if (jsonDocument.RootElement.TryGetProperty("data", out var dataArray) &&
                dataArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var data in dataArray.EnumerateArray())
                    {
                        if (data.TryGetProperty("sourceType", out var sourceType))
                        {
                            if (sourceType.TryGetProperty("name", out var name))
                            {
                                sourceList.Add(name.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
    private async Task GetValueUsingOwner(HttpResponseMessage httpResponseMsg, List<string> mappedToList)
    {

        using (var stream = await httpResponseMsg.Content.ReadAsStreamAsync())
        using (var jsonDocument = await JsonDocument.ParseAsync(stream))
        {
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var jsonBlock in jsonDocument.RootElement.EnumerateArray())
                {
                    // objecten van maken!!!!
                    jsonBlock.TryGetProperty("value", out var value);

                    var stringValue = value.GetString();

                    if (stringValue is not null)
                    {
                        stringValue = stringValue.Replace(" ", "[]");

                        customer.Value = stringValue;
                        mappedToList.Add(customer.Value);

                    }
                }
            }
        }
    }

    static async Task<string> GetAccessToken(string clientID, string clientSecret, string authUrl)
    {
        var data = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        };

        using (var content = new FormUrlEncodedContent(data))
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientID}:{clientSecret}")));

            var response = await client.PostAsync(authUrl, content);
            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var jsonDocument = await JsonDocument.ParseAsync(stream))
            {
                return jsonDocument.RootElement.GetProperty("access_token").GetString();
            }
        }
    }

    private async Task<HttpResponseMessage> GetCompanyMappingUsingOwner(HttpClient client, string owner, string target)
    {
        string accessToken = await GetAccessToken(clientID, clientSecret, authUrl);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        string getMappingUrl = string.Format(companyMappingUsingOwnerUrl, owner, target);

        return await client.GetAsync(getMappingUrl);
    }

    private async Task<HttpResponseMessage> GetDataSource(HttpClient client, string providing, string target)
    {
        string accessToken = await GetAccessToken(clientID, clientSecret, authUrl);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string sourcesUrl = string.Format(getSourcesUrl, target, providing);

        return await client.GetAsync(sourcesUrl);
    }

    private async Task CheckParticipationsWithProvidingCompany()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage? participationsProvidingCompany = null;
            HttpResponseMessage? participationsNoProviderRestrictions = null;
            if (customer.KvkNumber != null)
            {
                List<Task<HttpResponseMessage>> parralelGet = new List<Task<HttpResponseMessage>>();
                parralelGet =
                [
                GetParticipationsWithProvidingCompany(client, provider.KvkNumber, customer.KvkNumber),
                GetParticipationsWithProvidingCompany(client, joinDataKvk, customer.KvkNumber)
                ];

                await Task.WhenAll(parralelGet);
                if (parralelGet.Count > 0)
                {
                    participationsProvidingCompany = parralelGet[0].Result;
                }
                if (parralelGet.Count > 1)
                {
                    participationsNoProviderRestrictions = parralelGet[1].Result;
                }

                List<Task> parralelParse = new List<Task>();
                parralelParse =
                [
                GetParticipations(participationsProvidingCompany, mandatesList),
                GetParticipations(participationsNoProviderRestrictions, mandatesList)
                ];

                await Task.WhenAll(parralelParse);
            }

        }
    }

    private async Task GetParticipations(HttpResponseMessage httpResponseMsg, List<string> listToFill)
    {

        using (var stream = await httpResponseMsg.Content.ReadAsStreamAsync())
        using (var jsonDocument = await JsonDocument.ParseAsync(stream))
        {
            if (jsonDocument.RootElement.TryGetProperty("data", out var dataArray) &&
                dataArray.ValueKind == JsonValueKind.Array)
            {
                foreach (var dataBlock in dataArray.EnumerateArray())
                {
                    string providerValue = "";
                    if (dataBlock.TryGetProperty("company", out var company) &&
                        company.ValueKind == JsonValueKind.Object)
                    {
                        if (company.TryGetProperty("companyValue", out var companyValue))
                        {
                            providerValue = companyValue.ToString();
                        }
                    }
                    if (dataBlock.TryGetProperty("datasets", out var dataSetsArray) &&
                        dataSetsArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var dataSet in dataSetsArray.EnumerateArray())
                        {
                            if (dataSet.TryGetProperty("clients", out var clientsArray) &&
                                clientsArray.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var client in clientsArray.EnumerateArray())
                                {
                                    if (client.TryGetProperty("companyValue", out var companyValue) &&
                                        companyValue.ToString() == authorizedCompany)
                                    {
                                        if (dataSet.TryGetProperty("datasetId", out var dataSetId))
                                        {
                                            for (int i = 0; i < dataSetsList.Count; i++)
                                            {
                                                if (dataSetsList[i].ID == dataSetId.ToString())
                                                {
                                                    if (providerValue == joinDataKvk)
                                                    {
                                                        listToFill.Add($"{dataSetsList[i].Name} - JoinData");
                                                    }
                                                    else
                                                    {
                                                        listToFill.Add($"{dataSetsList[i].Name} - {provider.Name}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private async Task<HttpResponseMessage> GetParticipationsWithProvidingCompany(HttpClient client, string providingCompany, string authorizingCompany)
    {
        string accessToken = await GetAccessToken(purposeClientID, purposeClientSecret, authUrl);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        client.DefaultRequestHeaders.Accept.Clear();
        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string sourceUrl = string.Format(getProviderRestrictionUrl, authorizingCompany, providingCompany);

        return await client.GetAsync(sourceUrl);
    }


}