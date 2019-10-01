using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestTaskDirectory.Api.Test
{
    public static class TestExtensions
    {
        public static StringContent SerializeRequest<T>(T obj)
        {
            const string mediaType = "application/json";

            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, mediaType);
        }

        public static async Task<T> DeserializeResult<T>(HttpResponseMessage response)
        {
            var str = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}