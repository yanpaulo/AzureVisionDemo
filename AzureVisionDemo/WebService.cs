using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureVisionDemo
{
    [Flags]
    public enum VisualFeatures
    {
        None =          0,
        Categories =    1,
        Tags =          1 << 1,
        Description =   1 << 2,
        Faces =         1 << 3,
        ImageType =     1 << 4,
        Color =         1 << 5,
        Adult =         1 << 6,
    }

    public enum VisualDetails
    {
        None =          0,
        Celebrities =   1,
        Landmarks =     2
    }

    public class WebService
    {
        private HttpClient _client;

        public WebService(string location, string apiKey)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"https://{location}.api.cognitive.microsoft.com/vision/v1.0/")
            };
            _client.DefaultRequestHeaders.Add("ocp-apim-subscription-key", apiKey);
        }

        public async Task<ImageAnalisys> AnalyzeImageAsync(string url, VisualFeatures features = VisualFeatures.None, VisualDetails details = VisualDetails.None)
        {
            var content = new StringContent($"{{\"url\":\"{url}\"}}", Encoding.UTF8, "application/json"); 
            return await AnalyzeImageAsync(content, features, details);
        }

        public async Task<ImageAnalisys> AnalyzeImageAsync(Stream stream, VisualFeatures features = VisualFeatures.None, VisualDetails details = VisualDetails.None)
        {
            var content = new StreamContent(stream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return await AnalyzeImageAsync(content, features, details);
        }

        private async Task<ImageAnalisys> AnalyzeImageAsync(HttpContent content, VisualFeatures features = VisualFeatures.None, VisualDetails details = VisualDetails.None)
        {
            var result = await _client.PostAsync($"analyze?visualFeatures={UrlEncode(features)}&details={UrlEncode(details)}", content);
            if (!result.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(await result.Content.ReadAsStringAsync());
            }
            var stringResult = await result.Content.ReadAsStringAsync();
            var analisys = JsonConvert.DeserializeObject<ImageAnalisys>(stringResult);
            return analisys;
        }

        private string UrlEncode(object target)
        {
            return string.Join(",", ParseEnum(target));
        }

        private IEnumerable<string> ParseEnum(object target)
        {
            var values = Enum.GetValues(target.GetType());
            var names = new List<string>();
            foreach (object value in values)
            {
                if(((int)target & (int)value) != 0)
                {
                    names.Add(value.ToString());
                }
            }

            return names;
        }
    }
}
