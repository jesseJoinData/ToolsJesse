using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;


namespace CheckEabMessageFlow;

public partial class Form1 : Form
{
    private string filePath = "C://Users//jesse//Documents//Tools//ToolsJesse//CheckEabMessageFlow//companies//companies-2023-11-28-10-34-13.csv";
    private string clientID = "source-registry";
    private string clientSecret = "4029e2b9-d74b-4147-9086-df606c817fbf";
    private string authUrl = "https://production.join-data.net/auth/realms/datahub/protocol/openid-connect/token";
    private string getDataSetsUrl = "https://production.join-data.net/purpose-registry/api/v1.0/admin/datasets";

    public List<DataSets> dataSetsList = new List<DataSets>();

    public class DataSets
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? ParentID { get; set; }
    }
    public Form1()
    {
        InitializeComponent();
    }

    private async Task OpenMappingAndSources()
    {
        MappingAndSources mappingAndSources = new MappingAndSources();

        await mappingAndSources.CheckInfo(customerCompanyCode, providingCompanyCode, authorizedCompanyCode);

        mappingAndSources.Show();
    }

    private async Task OpenPurposesAndParticipations()
    {
        PurposesAndParticipations purposesAndParticipations = new PurposesAndParticipations();
        await purposesAndParticipations.CheckInfo(customerCompanyCode, providingCompanyCode, authorizedCompanyCode, dataSetsList, providingCompanyName);
        purposesAndParticipations.Show();
    }
    public List<Companies> LoadCompaniesFromFile()
    {
        List<Companies> companiesList = new List<Companies>();

        if (File.Exists(filePath))
        {

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                // Skip header if your CSV file has one
                if (!parser.EndOfData)
                {
                    parser.ReadFields(); // Skip header
                }
                int i = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length >= 2)
                    {
                        Companies company = new Companies
                        {
                            ID = i++,
                            CompanyName = fields[0],
                            KvkNumber = fields[1],
                            ClientID = fields[2],
                            ClientSecret = fields[3]
                        };

                        companiesList.Add(company);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show($"File for companies not found {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return companiesList;
    }

    public List<DataSets> LoadDataSetsWithApiCall()
    {
        List<DataSets> dataSetsList = new List<DataSets>();
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage? dataSets = null;
            dataSets = GetDataSets(client);
            FillDataSetsList(dataSets, dataSetsList);
        }

        return dataSetsList;
    }
    private async Task FillDataSetsList(HttpResponseMessage httpResponseMsg, List<DataSets> dataSetsList)
    {
        using (var stream = await httpResponseMsg.Content.ReadAsStreamAsync())
        using (var jsonDocument = await JsonDocument.ParseAsync(stream))
        {
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var jsonBlock in jsonDocument.RootElement.EnumerateArray())
                {
                    DataSets dataSets = new DataSets();
                    if (jsonBlock.TryGetProperty("id", out var id))
                    {
                        dataSets.ID = id.ToString();
                    }
                    if (jsonBlock.TryGetProperty("name", out var name))
                    {
                        dataSets.Name = name.ToString();
                    }
                    if (jsonBlock.TryGetProperty("parentId", out var parentId))
                    {
                        dataSets.ParentID = parentId.ToString();
                    }
                    dataSetsList.Add(dataSets);
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

            var response = await client.PostAsync(authUrl, content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var jsonDocument = await JsonDocument.ParseAsync(stream))
            {
                return jsonDocument.RootElement.GetProperty("access_token").GetString();
            }
        }
    }

    private HttpResponseMessage GetDataSets(HttpClient client)
    {
        string accessToken = GetAccessToken(clientID, clientSecret, authUrl).Result;
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        return client.GetAsync(getDataSetsUrl).Result;
    }


}
