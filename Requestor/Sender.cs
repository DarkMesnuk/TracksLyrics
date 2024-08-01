using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Requestor.Domain;

namespace Requestor;

public class Sender(
    IHttpClientFactory httpClient,
    ILogger<Sender> logger
    ) : ISender
{
    public async Task<string> SendAsync(ApiRequest apiRequest)
    {
        try
        {
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            message.Method = apiRequest.Type;

            if (apiRequest.Data != null)
            {
                if (message.Method == HttpMethod.Get)
                    message.RequestUri = new Uri(apiRequest.Url + GenerateQueryParams(apiRequest.Data));
                else
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
            }

            var client = httpClient.CreateClient("TrackLyrics");
            client.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

            var apiResponse = await client.SendAsync(message);
            return await apiResponse.Content.ReadAsStringAsync();
        }
        catch (Exception)
        {
            logger.LogError($"Error sending API request: {apiRequest.Url}");
            return string.Empty;
        }
    }

    private string GenerateQueryParams(object data)
    {
        var properties = data.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var keyValuePairs = new List<string>();

        foreach (var property in properties)
        {
            var value = property.GetValue(data);

            if (value != null)
                keyValuePairs.Add($"{property.Name}={value}");
        }

        return "?" + string.Join("&", keyValuePairs);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}