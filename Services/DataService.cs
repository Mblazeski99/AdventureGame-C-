using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Entities;

namespace AdventureGame.Services
{
    public static class DataService
    {
        public async static Task<List<ResponseData>> GetData<ResponseData>(string url)
        {
            string data;
            WebResponse res = await WebRequest.CreateHttp(url).GetResponseAsync();
            using(StreamReader resReader = new StreamReader(res.GetResponseStream()))
            {
                data = resReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<List<ResponseData>>(data);
        }
    }
}
