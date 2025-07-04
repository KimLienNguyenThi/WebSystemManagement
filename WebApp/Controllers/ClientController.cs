﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using System.Net.Http.Headers;
using System.Text;
using WebApp.Configs;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Controllers
{
    
    [Authorize(AuthenticationSchemes = "User")]
    public class ClientController : Controller
    {
        private readonly HttpClient _client;
        private readonly ApiConfigs _apiConfigs;


        public ClientController(IOptions<ApiConfigs> apiConfigs)
        {
            _client = new HttpClient();
            _apiConfigs = apiConfigs.Value;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetAllInfor([FromBody] reqSelect reqSelect)
        {
            try
            {
                List<CompanyAccountDTO> listRequest = new List<CompanyAccountDTO>();

                // Chuẩn bị request body
                var jsonContent = new StringContent(JsonConvert.SerializeObject(reqSelect), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + "/client/Requirements/GetAllInfor", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<List<CompanyAccountDTO>>(responseData);
                    return Ok(new { success = true, listRequest = responseObject });
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


        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Requirement_C Req)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(Req.ContractNumber))
                return BadRequest(new { success = false, message = "Mã khách hàng không hợp lệ!" });

            if (Req == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            try
            {
                // Chuẩn bị request body
                var jsonContent = new StringContent(JsonConvert.SerializeObject(Req), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + "/client/Requirements/Insert", jsonContent);

                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(result);

                // Lấy message dưới dạng string, tránh lỗi mảng rỗng
                string errorMessage = apiResponse["message"]?.ToString() ?? "Có lỗi xảy ra từ API.";

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = errorMessage });
                }
                else
                {
                    return BadRequest(new { success = false, message = errorMessage });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại sau." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetdetailRequest([FromQuery] string query)
        {
            try
            {
                List<Requirement_Company> listHis = new List<Requirement_Company>();
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + $"/client/Requirements/GetdetailRequest?query={query}");
                if (response.IsSuccessStatusCode)
                {
                    var reponseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<Requirement_Company>(reponseData);
                    return Ok(new { success = true, listHis = responseObject });
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

        //lấy list yêu cầu hiển thị  
        [HttpPost]
        public async Task<IActionResult> GetAllRequest([FromBody] GetListReq req)
        {
            try
            {
                PagingResult<Request_GroupCompany_DTO> phieuTraList = new PagingResult<Request_GroupCompany_DTO>();

                var reqjson = JsonConvert.SerializeObject(req);
                var httpContent = new StringContent(reqjson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_apiConfigs.BaseApiUrl + "/client/Requirements/GetAllRequest", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<PagingResult<Request_GroupCompany_DTO>>(responseData);
                    return Ok(new { success = true, listRequest = responseObject });
                  
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, new { success = false, message = "Lỗi kết nối đến server." });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Review([FromBody] ReviewDTO request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + "/client/Requirements/Review", jsonContent);

                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(result);

                string errorMessage = apiResponse["message"]?.ToString() ?? "Có lỗi xảy ra từ API.";

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = errorMessage });
                }
                else
                {
                    return BadRequest(new { success = false, message = errorMessage });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại sau." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetViewReview([FromQuery] string query)
        {
            try
            {
                List<ReviewDTO> listHis = new List<ReviewDTO>();
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + $"/client/Requirements/GetViewReview?query={query}");
                if (response.IsSuccessStatusCode)
                {
                    var reponseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<ReviewDTO>(reponseData);
                    return Ok(new { success = true, listHis = responseObject });
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
    }
}
