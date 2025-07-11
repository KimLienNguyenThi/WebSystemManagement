﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Models;
using WebApi.Service.Admin;
using WebApi.Service.Client;
using Microsoft.Extensions.Caching.Memory;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Service.Introduce;
namespace WebApi.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly ManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly PdfService _pdfService;
        private readonly IWebHostEnvironment _env;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        
        public AccountController(IMapper mapper, ManagementDbContext context, AccountService accountService, PdfService pdfService, IWebHostEnvironment env, EmailService emailService, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
            _pdfService = pdfService;
            _env = env;
            _emailService = emailService;
            _config = config;
          
        }

        [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<ActionResult<CompanyAccountDTO>> GetAllCompany([FromBody] GetListCompanyPaging req)
        {
            var company = await _accountService.GetAllCompany(req);
            return Ok(company);
        }

       [Authorize(Roles = "Admin,Director")]
        [HttpPut]
        public IActionResult UpdateStatus([FromBody] updateID updateID)
        {
            try
            {
                Console.WriteLine($"Received UpdateStatus request: CustomerId = {updateID.CustomerId}, Status = {updateID.status}");

                if (_accountService.UpdateStatus(updateID.status, updateID.CustomerId))
                {
                    return Ok(new APIResponse<object>()
                    {
                        Success = true,
                        Message = "Cập nhật thành công",
                        Data = null
                    });
                }
                else
                {
                    return Ok(new APIResponse<object>()
                    {
                        Success = false,
                        Message = "Cập nhật không thành công! Đã xảy ra lỗi",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cập nhật: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public IActionResult Update([FromBody] CompanyAccountDTO companyAccountDTO, [FromQuery] string id)
        {
            if (companyAccountDTO == null || string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Dữ liệu đầu vào không hợp lệ.");
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var company = _accountService.Update(companyAccountDTO, id);
            if (company.StartsWith("IT030300"))
            {
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật tài khoản thành công",
                    companyID = company
                });
            }
            return BadRequest(new { success = false, message = company });
        }

        //[Authorize(Roles = "Admin,Director")]

        [HttpPost]
        public async Task<IActionResult> ExportToCsv([FromBody] ExportRequestDTO request)
        {
            try
            {
                byte[] fileBytes = await _accountService.ExportToCsv(request);
                return File(fileBytes, "text/csv", "DanhSachKhachHang.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Director")]
        [HttpGet]
        public async Task<ActionResult<ServiceTypeDTO2>> GetListServiceID()
        {
            var regu = await _accountService.GetListServiceID();
            return Ok(regu);
        }

        //TẠO FILE CHỜ SẾP KÝ
        [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<IActionResult> GenerateContract([FromBody] CompanyAccountDTO dto, [FromQuery] string id)
        {
            try
            {
                string contractId = Guid.NewGuid().ToString();

                // 1. Tạo file PDF gốc chưa ký
                byte[] pdfBytes = _pdfService.GenerateContractPdf(dto);

                // 3. Lưu file PDF chờ boss ký
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp-pdfs");
                
                string fileName = $"{contractId}.pdf";
                string fullPath = Path.Combine(folderPath, fileName);
                //await System.IO.File.WriteAllBytesAsync(fullPath, signedPdfBytes);

                string relativePath = Path.Combine("/temp-pdfs", fileName).Replace("\\", "/");

                // 5. Lưu thông tin hợp đồng, trạng thái file đã ký vào DB
                var result = await _accountService.SaveContractStatusAsync(new CompanyContractDTOs
                {
                    CustomerId = dto.CustomerId,
                    CompanyName = dto.CompanyName,
                    TaxCode = dto.TaxCode,
                    CompanyAccount = dto.CompanyAccount,
                    AccountIssuedDate = dto.AccountIssuedDate,
                    CPhoneNumber = dto.CPhoneNumber,
                    CAddress = dto.CAddress,
                    RootAccount = dto.RootAccount,
                    RootName = dto.RootName,
                    RPhoneNumber = dto.RPhoneNumber,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    ContractNumber = dto.ContractNumber,
                    Startdate = dto.Startdate,
                    Enddate = dto.Enddate,
                    CustomerType = dto.CustomerType,
                    ServiceType = dto.ServiceType,
                    ConfileName = fileName,
                    FilePath = relativePath,
                    ChangedBy = id,
                    Amount = dto.Amount,
                    Original = dto.Original,
                });

                if (result == null || result.StartsWith("Lỗi") || result.Contains("không tồn tại") || result.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = result });
                }
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                await System.IO.File.WriteAllBytesAsync(fullPath, pdfBytes);


                return Ok(new { success = true, fullPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi gửi email hoặc tạo hợp đồng: {ex.Message}");
            }
        }

        //lấy list hiển thị
       [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<ActionResult<CompanyContractDTOs>> GetListPending([FromBody] GetListCompanyPaging req)
        {
            var regu = await _accountService.GetListPending(req);
            return Ok(regu);
        }


        //Gửi client
        [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<IActionResult> SendEmailtoclient([FromBody] SignAdminRequest dto)
        {
            try
            {
                // Gửi mail hợp đồng tới khách hàng ký
                await _emailService.SendEmailtoclient(dto);
                await _accountService.UploadSendclient(dto);
                return Ok(new { success = true, dto.FilePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi gửi email hoặc tạo hợp đồng: {ex.Message}");
            }
        }

        //Duyệt 
        [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<IActionResult> BrowseSignofClient([FromBody] SignAdminRequest dto)
        {
            try
            {
                // Gửi mail hợp đồng tới khách hàng ký
                await _emailService.BrowseSignofClient(dto);
                await _accountService.BrowseSendclient(dto);
                return Ok(new { success = true, dto.FilePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi gửi email hoặc tạo hợp đồng: {ex.Message}");
            }
        }

        //admin Xác nhận hoàn tất
        [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] SignAdminRequest request)
        {
            if (request == null )
            {
                Console.WriteLine("Dữ liệu đầu vào không hợp lệ.");
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var result = await _accountService.Insert(request);

            if (result?.StartsWith("IT030300") == true)  // Kiểm tra null trước
            {
                return Ok(new
                {
                    success = true,
                    message = "Đăng ký tài khoản thành công",
                    companyID = result
                });
            }
            else
            {
                return BadRequest(new { success = false, message = result ?? "Lỗi không xác định." });
            }

        }

        //boss ký
        [Authorize(Policy = "DirectorPolicy")]
        [HttpPost]
        public async Task<IActionResult> SignPdfWithPfx(IFormFile pfxFile, [FromForm] string password, [FromForm] string fileName, [FromForm] string staffid)
        {
            Console.WriteLine("---- Log đầu vào ----");
            Console.WriteLine("fileName: " + fileName);
            Console.WriteLine("staffid: " + staffid);
            Console.WriteLine("password: " + password);
            Console.WriteLine("pfxFile: " + (pfxFile != null ? pfxFile.FileName : "null"));
            if (pfxFile == null || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Thiếu thông tin bắt buộc");
            }
            try
            {
                // Xác định đường dẫn đến thư mục chứa file PDF cần ký
                var pdfFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp-pdfs");
                var filePath = Path.Combine(pdfFolder, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Không tìm thấy file PDF gốc để ký.");
                }

                var originalPdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                using var pfxStream = pfxFile.OpenReadStream();

                var signedPdfBytes = _pdfService.SignPdfWithAdminCertificate(originalPdfBytes, pfxStream, password, staffid);

                // Trả về file PDF đã ký
                return File(signedPdfBytes, "application/pdf", "signed-contracts" + fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"{ex.Message}"   
                });
            }
        }

        //boss lưu lại file đã ký
        [Authorize(Policy = "DirectorPolicy")]
        [HttpPost]
        public async Task<IActionResult> SaveSignedPdf(IFormFile signedPdf, [FromForm] string fileName, [FromForm] string contractNumber,[FromForm] string staffid)
        {
            if (signedPdf == null || string.IsNullOrEmpty(fileName) ||  string.IsNullOrEmpty(contractNumber))
                return BadRequest("Thiếu thông tin đầu vào");

            var result = await _accountService.UploadDirectorSigned(signedPdf, fileName, contractNumber, staffid);

            if (result != "Cập nhật trạng thái và lưu file đã ký thành công")
                return StatusCode(500, result);

            return Ok(result);
        }
        //admin Xác nhận cancel
        //[Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public async Task<IActionResult> CancelContract([FromBody] CancelContractRequest request)
        {
            if (request == null)
            {
                Console.WriteLine("Dữ liệu đầu vào không hợp lệ.");
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var (result, mes) = await _accountService.CancelContract(request);

            if (result)
            {
                return Ok(new
                {
                    success = true,
                    message = mes,
                });
            }
            else
            {
                return BadRequest(new { success = false, message = mes });
            }
        }
    }
}
