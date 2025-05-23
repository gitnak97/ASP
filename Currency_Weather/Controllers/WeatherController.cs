using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Currency_Weather.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetWeather()
        {
            try
            {
                string apiKey = "CphOgsk4rTQcVJG7C0QeYx0abOMQDoLM";
                string locationKey = "318251"; // 예: 이스탄불 (터키)
                string url = $"http://dataservice.accuweather.com/currentconditions/v1/{locationKey}?apikey={apiKey}&language=ko&details=true";

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        return Content(response.StatusCode, json);

                    var data = JsonConvert.DeserializeObject<JArray>(json)[0];

                    var result = new
                    {
                        상태 = data["WeatherText"],
                        온도 = data["Temperature"]["Metric"]["Value"] + "°C",
                        체감온도 = data["RealFeelTemperature"]["Metric"]["Value"] + "°C",
                        습도 = data["RelativeHumidity"] + "%",
                        갱신시각 = data["LocalObservationDateTime"]
                    };

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // 서버 오류 시 예외 출력
            }
        }
    }
}
