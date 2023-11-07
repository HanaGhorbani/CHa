using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Chart
{
    public class DataReceiver<T>
    {
        string _url;

        public DataReceiver(string url)
        {
            _url = url;
        }

        public async Task<T> GetData()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(jsonString);
         
            return data;
        }
    }
}
