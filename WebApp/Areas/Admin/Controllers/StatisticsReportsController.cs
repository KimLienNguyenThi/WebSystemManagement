using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApi.DTO;
using WebApp.Configs;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/StatisticsReports")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class StatisticsReportsController : Controller
    {
        private readonly ApiConfigs _apiConfigs;

        private readonly HttpClient _client;

        public StatisticsReportsController(IOptions<ApiConfigs> apiConfigs)
        {
            _client = new HttpClient();
            _apiConfigs = apiConfigs.Value;

            _client.Timeout = TimeSpan.FromMinutes(5); // Thêm dòng này
        }

        // [AuthorizeToken]
        [AllowAnonymous]

        [Route("")]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (HttpContext.Request.Path.Value.Contains("/admin/LoginAdmin/Login", StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect("/admin/homeadmin/index");
                }
            }
            return View();
        }

        //thống kê của khách hàng 
        [HttpGet]
        [Route("GetCustomerOverview")]
        public async Task<IActionResult> GetCustomerOverview()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { success = false, message = "Thiếu token." });


                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + "/admin/StatisticsReports/GetCustomerOverview");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<CustomerOverviewDTO>(responseData);
                    return Ok(new { success = true, data = responseObject });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Lỗi kết nối đến server." });
            }
        }

        [HttpGet]
        [Route("GetRevenueOverview")]
        public async Task<IActionResult> GetRevenueOverview([FromQuery] int year)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { success = false, message = "Thiếu token." });


                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + $"/admin/StatisticsReports/GetRevenueOverview?year={year}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<RevenueOverviewDTO>(responseData);
                    return Ok(new { success = true, data = responseObject });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Lỗi kết nối đến server." });
            }
        }
        [HttpGet]
        [Route("GetServiceUsage")]
        public async Task<IActionResult> GetServiceUsage()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { success = false, message = "Thiếu token." });


                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + "/admin/StatisticsReports/GetServiceUsage");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<List<ServiceUsageDTO>>(responseData);
                    return Ok(new { success = true, data = responseObject });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Lỗi kết nối đến server." });
            }
        }
        [HttpGet]
        [Route("GetReviewStatistics")]
        public async Task<IActionResult> GetReviewStatistics()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { success = false, message = "Thiếu token." });


                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + "/admin/StatisticsReports/GetReviewStatistics");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<ReviewOverviewDTO>(responseData);
                    return Ok(new { success = true, data = responseObject });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Lỗi kết nối đến server." });
            }
        }
        [HttpGet]
        [Route("GetLoyalCustomers")]
        public async Task<IActionResult> GetLoyalCustomers()
        {
            try
            {
                // Lấy token từ header
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { success = false, message = "Thiếu token." });

                // Đặt token vào header của HttpClient
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Gọi đến API backend lấy danh sách khách hàng thân thiết
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + "/admin/StatisticsReports/GetLoyalCustomers");
               
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<List<LoyalCustomerDTO>>(responseData);

                    return Ok(new
                    {
                        success = true,
                        data = responseObject
                    });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new
                    {
                        success = false,
                        message = errorMessage
                    });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Lỗi kết nối đến server." });
            }
        }

    }
}
