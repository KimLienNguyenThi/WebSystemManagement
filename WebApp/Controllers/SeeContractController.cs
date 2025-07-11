﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System.Net.Http;
using System.Text;
using WebApp.DTO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using NuGet.Protocol;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WebApp.Areas.Admin.Controllers.AccountController;
using System.Text.Json;
using WebApp.Configs;
using Microsoft.Extensions.Options;

namespace WebApp.Areas.Controllers
{
    public class SeeContractController : Controller
    {
        private readonly HttpClient _client;
        private readonly ApiConfigs _apiConfigs;

        private readonly IHttpClientFactory _httpClientFactory;

        public SeeContractController(IHttpClientFactory httpClientFactory, IOptions<ApiConfigs> apiConfigs)
        {
            _client = new HttpClient();
            _apiConfigs = apiConfigs.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index(string fileName, string email)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = $"{_apiConfigs.BaseApiUrl}/admin/SeeContract_Sign/CheckStatus?fileName={fileName}&email={email}";

                try
                {
                    var response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.ErrorMessage = "Không tìm thấy hợp đồng hoặc lỗi hệ thống.";
                        return View("Error");
                    }
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var hopDong = JsonConvert.DeserializeObject<StatusSignClient>(jsonString);

                    if (hopDong == null)
                    {
                        ViewBag.ErrorMessage = "Dữ liệu hợp đồng không hợp lệ.";
                        return View("Error");
                    }

                    if (hopDong.status == 2)
                    {
                        ViewBag.ContractNumber = hopDong.contractnumber; 
                        return View(); 
                    }
                    else if (hopDong.status == 3)
                    {
                        //return View("~/Views/SeeContract/Payment.cshtml", hopDong); 
                        ViewBag.ErrorMessage = "Vui lòng chờ Hệ thống xác nhận hợp đồng";
                        return View("Error");
                    }
                    else if (hopDong.status == 4)
                    {
                        ViewBag.ErrorMessage = "Vui lòng vào link thanh toán đã được gửi đến Email của quý khách hàng";
                        return View("Error");
                    }
                    ViewBag.ErrorMessage = "Trạng thái hợp đồng không xác định.";
                    return View("Error");
                }
                catch (Exception ex)
                {
                    return BadRequest("Có lỗi khi gọi API: " + ex.Message);
                }
            }
        }

        //ky so
        [HttpPost]
        public async Task<IActionResult> SignPdfWithPfx(IFormFile pfxFile, string password, string fileName, string email)
        {
            if (pfxFile == null || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Thiếu dữ liệu đầu vào");
            }

            using (var client = new HttpClient())
            {
                var apiUrl = $"{_apiConfigs.BaseApiUrl}/admin/SeeContract_Sign/SignPdfWithPfx";

                var formData = new MultipartFormDataContent();

                // Copy stream thành mảng byte
                using var ms = new MemoryStream();
                await pfxFile.CopyToAsync(ms);
                var pfxBytes = ms.ToArray();

                // Gửi file dưới dạng ByteArrayContent với Content-Type đúng
                var byteContent = new ByteArrayContent(pfxBytes);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-pkcs12"); 

                formData.Add(byteContent, "pfxFile", pfxFile.FileName);
                formData.Add(new StringContent(password), "password");
                formData.Add(new StringContent(fileName), "fileName");
                formData.Add(new StringContent(email), "email");

                var response = await client.PostAsync(apiUrl, formData);

                if (!response.IsSuccessStatusCode)
                {
                    var errContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var errObj = System.Text.Json.JsonSerializer.Deserialize<ApiErrorResponse>(errContent, options);

                        return StatusCode((int)response.StatusCode, new
                        {
                            success = false,
                            message = errObj?.Message ?? "Lỗi không xác định"
                        });
                    }
                    catch
                    {
                        return StatusCode((int)response.StatusCode, new
                        {
                            success = false,
                            message = "Lỗi không xác định hoặc định dạng phản hồi không hợp lệ"
                        });
                    }
                }

                var pdfSignedBytes = await response.Content.ReadAsByteArrayAsync();
                return File(pdfSignedBytes, "application/pdf");
            }

        }
        
        //luu
        [HttpPost]
        public async Task<IActionResult> SaveSignedPdf(IFormFile signedPdf, string fileName, string email, string contractNumber)
        {
            if (signedPdf == null || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Thiếu dữ liệu đầu vào");
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{_apiConfigs.BaseApiUrl}/admin/SeeContract_Sign/SaveSignedPdf";

                    var formData = new MultipartFormDataContent();

                    using var ms = new MemoryStream();
                    await signedPdf.CopyToAsync(ms);
                    var pdfBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(pdfBytes);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    formData.Add(byteContent, "signedPdf", signedPdf.FileName);
                    formData.Add(new StringContent(fileName), "fileName");
                    formData.Add(new StringContent(email), "email");
                    formData.Add(new StringContent(contractNumber), "contractNumber");

                    var response = await client.PostAsync(apiUrl, formData);

                    if (!response.IsSuccessStatusCode)
                    {
                        var err = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, $"Lưu không thành công: {err}");
                    }

                    return Ok("Lưu file đã ký thành công.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi gửi file đã ký đến API: {ex.Message}");
            }
        }

        //ky tay
        [HttpPost]
        public async Task<IActionResult> SignPdfWithHand(string base64Hand, string fileName, string email)
        {
            if (string.IsNullOrEmpty(base64Hand) || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Thiếu dữ liệu đầu vào");
            }

            using (var client = new HttpClient())
            {
                var apiUrl = $"{_apiConfigs.BaseApiUrl}/admin/SeeContract_Sign/SignPdfWithHand";


                // Tách chuỗi base64 ra(loại bỏ tiền tố data: image / png; base64,)
                var base64Data = Regex.Replace(base64Hand, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                var request = new
                {
                    base64Data = base64Hand,
                    fileName = fileName,
                    email = email
                };
                var content = new StringContent(request.ToJson(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Ký không thành công: {err}");
                }

                var pdfSignedBytes = await response.Content.ReadAsByteArrayAsync();
                return File(pdfSignedBytes, "application/pdf");
            }

        }
    }
}


//[HttpPost]
//public async Task<IActionResult> UploadSignedFile(IFormFile signedFile, string email, string originalFileName)
//{
//    if (signedFile == null || signedFile.Length == 0 || originalFileName==null)
//        return Json(new { success = false, message = "Vui lòng chọn file hợp lệ." });

//    // Tạo HttpClient để gọi API Controller
//    using (var client = new HttpClient())
//    {
//        var apiUrl = "https://localhost:7190/api/admin/SeeContract_Sign/UploadSignedFile";

//        var form = new MultipartFormDataContent();
//        form.Add(new StreamContent(signedFile.OpenReadStream()), "signedFile", signedFile.FileName);
//        form.Add(new StringContent(email), "email");
//        form.Add(new StringContent(originalFileName), "originalFileName"); // Thêm dòng này

//        var response = await client.PostAsync(apiUrl, form);

//        if (response.IsSuccessStatusCode)
//        {
//            var result = await response.Content.ReadAsStringAsync();
//            return Json(new { success = true, message = "Gửi đến API thành công", result });
//        }
//        else
//        {
//            var error = await response.Content.ReadAsStringAsync();
//            return Json(new { success = false, message = "Lỗi từ API", error });
//        }
//    }
//}


//[HttpPost]
//public async Task<IActionResult> UploadSignatureImage(IFormFile signatureImage)
//{
//    if (signatureImage == null || signatureImage.Length == 0)
//        return BadRequest("Không có ảnh nào được gửi.");

//    var client = _httpClientFactory.CreateClient();

//    var content = new MultipartFormDataContent();
//    content.Add(new StreamContent(signatureImage.OpenReadStream()), "signatureImage", signatureImage.FileName);

//    var apiUrl = "https://localhost:7190/api/admin/SeeContract_Sign/UploadSignatureImage";
//    var response = await client.PostAsync(apiUrl, content);

//    if (!response.IsSuccessStatusCode)
//        return BadRequest("Không thể ký tài liệu.");

//    var pdfBytes = await response.Content.ReadAsByteArrayAsync();
//    return File(pdfBytes, "application/pdf");
//}