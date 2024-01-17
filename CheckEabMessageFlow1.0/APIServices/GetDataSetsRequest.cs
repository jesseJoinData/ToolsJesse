using System.Text;
using System.Text.Json;

namespace CheckEabMessageFlow1_0.Services.APIcall;

public class GetDataSetsRequest : IAPIService
{
    private readonly string _clientID = "source-registry";
    private readonly string _clientSecret = "4029e2b9-d74b-4147-9086-df606c817fbf";
    private readonly string _authUrl = "https://production.join-data.net/auth/realms/datahub/protocol/openid-connect/token";
    private readonly string _getDataSetsUrl = "https://production.join-data.net/purpose-registry/api/v1.0/admin/datasets";

    public async Task<List<DataSets>> LoadDataSetsWithApiCall()
    {
        List<DataSets> dataSetsList = new List<DataSets>();
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage? dataSets = null;
            dataSets = PerformAPIAction(client);
            await FillDataSetsList(dataSets, dataSetsList);
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
    public async Task<string> GetAccessToken(string clientID, string clientSecret, string authUrl)
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
                return jsonDocument.RootElement.GetProperty("access_token").GetString()!;
            }
        }
    }

    public HttpResponseMessage PerformAPIAction(HttpClient client)
    {
        string accessToken = GetAccessToken(_clientID, _clientSecret, _authUrl).Result;
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        return client.GetAsync(_getDataSetsUrl).Result;
    }
}