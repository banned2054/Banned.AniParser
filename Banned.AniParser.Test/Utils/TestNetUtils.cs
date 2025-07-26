using System.Net;
using RestSharp;

namespace Banned.AniParser.Test.Utils;

internal class TestNetUtils
{
    public static async Task<string> Fetch(string url)
    {
        var client   = new RestClient(options => { options.Proxy = new WebProxy("http://127.0.0.1:7890"); });
        var response = await client.ExecuteAsync(new RestRequest(url));
        if (response.StatusCode == HttpStatusCode.OK) return response.Content ?? string.Empty;
        return string.Empty;
    }
}
