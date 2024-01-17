namespace CheckEabMessageFlow1_0.Services;

public interface IAPIService
{
    Task<string> GetAccessToken(string _clientID, string _clientSecret, string _authUrl);
    HttpResponseMessage PerformAPIAction(HttpClient client);

}