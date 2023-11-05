using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetapp;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;

namespace aspnetapp.Controllers
{
    [Route("api/wx")]
    [ApiController]
    public class WeChatAPIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeChatAPIController(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        /// <summary>
        /// https://localhost:58521/api/wx?url=/wxa/business/gamematch/creatematchrule&jsonValue=name:11,name2:2m2,
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        public async Task<object> OnGet(string url, string jsonValue)
        {


            var todoItemJson = new StringContent(
                  ToJsonStr(jsonValue),
                  Encoding.UTF8,
                  Application.Json);
            var httpResponseMessage = await _httpClientFactory.CreateClient().PostAsync("https://api.weixin.qq.com" + url, todoItemJson);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var s = contentStream.ToString();
                var s2 = contentStream.Seek(0, SeekOrigin.Begin);

                //string s2 = System.Text.UTF8Encoding.UTF8.GetString(contentStream.ReadTimeout);

                return await JsonSerializer.DeserializeAsync<object>(contentStream);
            }

            return string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<object>> Go(string url, string jsonValue)
        {
            return await OnGet(url, jsonValue);
        }

        private string ToJsonStr(string jsonArrayStr)
        {
            if (string.IsNullOrWhiteSpace(jsonArrayStr))
                return string.Empty;

            var s = new JObject(); 
            foreach (string item in jsonArrayStr.Split(","))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var s1 = item.Split(":");
                s.Add(s1[0], s1[1]);
            }


            Console.WriteLine(s);

            return s.ToString();
        }
    }
}
