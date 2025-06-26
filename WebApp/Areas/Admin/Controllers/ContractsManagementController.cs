﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using WebApp.Configs;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/ContractsManagement")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class ContractsManagementController : Controller
    {
        private readonly ApiConfigs _apiConfigs;

        private readonly HttpClient _client;

        public ContractsManagementController(IOptions<ApiConfigs> apiConfigs)
        {
            _client = new HttpClient();
            _apiConfigs = apiConfigs.Value;

        }

        [AuthorizeToken]
        [Route("")]
        public IActionResult Index()
        {
            //if (User.IsInRole("HanhChinh") || User.IsInRole("KyThuat"))
            //{
            //    return RedirectToAction("Index", "phanquyen");
            //}
            //else
            //{
               return View();
            //}
        }

        //Lấy thông tin công ty chính thức
        [HttpPost]
        [Route("GetAllCompany")]
        public async Task<IActionResult> GetAllCompany([FromBody] GetListCompanyPaging req)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { success = false, message = "Thiếu token." });

                List<CompanyAccountDTO> listCompany = new List<CompanyAccountDTO>();
                var reqjson = JsonConvert.SerializeObject(req);
                var httpContent = new StringContent(reqjson, Encoding.UTF8, "application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.PostAsync(_apiConfigs.BaseApiUrl + "/admin/ContractsManagement/GetAllCompany", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<PagingResult<CompanyAccountDTO>>(responseData);
                    return Ok(new { success = true, listCompany = responseObject });
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

        //Lưu và tạo hợp đồng mới
        [HttpPost]
        [Route("GenerateContract")]
        public async Task<IActionResult> GenerateContract([FromBody] CompanyAccountDTO companyAccountDTO, [FromQuery] string id)
        {
            // Lấy token từ header Authorization
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized(new { success = false, message = "Thiếu token." });

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { success = false, message = "Mã nhân viên không hợp lệ!" });

            if (companyAccountDTO == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(companyAccountDTO), Encoding.UTF8, "application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/GenerateContract?id={id}", jsonContent);

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

        //Gia hạn hợp đồng
        [HttpPost]
        [Route("InsertExtend")]
        public async Task<IActionResult> InsertExtend([FromBody] ContractDTO contractDTO, [FromQuery] string id)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized(new { success = false, message = "Thiếu token" });
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { success = false, message = "Token không hợp lệ" });
            }
            if (string.IsNullOrEmpty(contractDTO.CustomerId))
            {
                return BadRequest(new { success = false, message = "Mã khách hàng không hợp lệ" });
            }
            if (contractDTO == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
            }
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(contractDTO), Encoding.UTF8, "application/json");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/InsertExtend?id={id}", jsonContent);
                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(result);
                string errorMessage = apiResponse["message"]?.ToString() ?? "Có lỗi xảy ra từ API";
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
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại sau" });
            }
        }

        //Nâng cấp hợp đồng
        [HttpPost]
        [Route("InsertUpgrade")]
        public async Task<IActionResult> InsertUpgrade([FromBody] ContractDTO contractDTO, [FromQuery] string id)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized(new { success = false, message = "Thiếu token" });
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { success = false, message = "Token không hợp lệ" });
            }
            if (string.IsNullOrEmpty(contractDTO.CustomerId))
            {
                return BadRequest(new { success = false, message = "Mã khách hàng không hợp lệ" });
            }
            if (contractDTO == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
            }
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(contractDTO), Encoding.UTF8, "application/json");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/InsertUpgrade?id={id}", jsonContent);
                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(result);
                string errorMessage = apiResponse["message"]?.ToString() ?? "Có lỗi xảy ra từ API";
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
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại sau" });
            }
        }

        //Lấy ưu đãi đang có hiệu lực theo nhóm dịch vụ của hợp đồng
        [HttpGet]
        [Route("GetListEndow")]
        public async Task<IActionResult> GetListEndow([FromQuery] string id)
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return Unauthorized(new { success = false, message = "Thiếu token" });
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ" });
                }

                List<Endow> endow = new List<Endow>();

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/GetListEndow?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    var responseObject = JsonConvert.DeserializeObject<List<Endow>>(responseData);
                    return Ok(new { success = true, listendow = responseObject });

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

        //Lấy thông tin công ty search để tạo hợp đồng mới
        [HttpGet]
        [Route("GetAllInfor")]
        public async Task<IActionResult> GetAllInfor([FromQuery] string customerID)
        {
            try
            {
                List<CompanyAccountDTO> listRequest = new List<CompanyAccountDTO>();

                HttpResponseMessage response = await _client.GetAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/GetAllInfor?req={customerID}");

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

        //Tạo file hợp đồng gia hạn mới
        [HttpPost]
        [Route("GenerateContractExtend")]
        public async Task<IActionResult> GenerateContractExtend([FromBody] ContractDTO contractDTO, [FromQuery] string id)
        {
            // Lấy token từ header Authorization
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized(new { success = false, message = "Thiếu token." });

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { success = false, message = "Mã nhân viên không hợp lệ!" });

            if (contractDTO == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(contractDTO), Encoding.UTF8, "application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/GenerateContractExtend?id={id}", jsonContent);

                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(result);

                bool success = apiResponse["success"]?.Value<bool>() ?? false;

                if (success)
                {
                    string fullPath = apiResponse["fullPath"]?.ToString();
                    return Ok(new { success = true, message = "Gia hạn thành công", filePath = fullPath });
                }
                else
                {
                    string errorMessage = apiResponse["message"]?.ToString() ?? "Có lỗi xảy ra từ API.";
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại sau." });
            }
        }

        //tạo file nâng cấp hợp đồng
        [HttpPost]
        [Route("GenerateContractUpgrade")]
        public async Task<IActionResult> GenerateContractUpgrade([FromBody] ContractDTO contractDTO, [FromQuery] string id)
        {
            // Lấy token từ header Authorization
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized(new { success = false, message = "Thiếu token." });

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Token không hợp lệ." });

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { success = false, message = "Mã nhân viên không hợp lệ!" });

            if (contractDTO == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(contractDTO), Encoding.UTF8, "application/json");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _client.PostAsync(_apiConfigs.BaseApiUrl + $"/admin/ContractsManagement/GenerateContractUpgrade?id={id}", jsonContent);

                var result = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(result);

                bool success = apiResponse["success"]?.Value<bool>() ?? false;

                if (success)
                {
                    string fullPath = apiResponse["fullPath"]?.ToString();
                    return Ok(new { success = true, message = "Gia hạn thành công", filePath = fullPath });
                }
                else
                {
                    string errorMessage = apiResponse["message"]?.ToString() ?? "Có lỗi xảy ra từ API.";
                    return BadRequest(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại sau." });
            }
        }

    }
}
