using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Models;
using WebApi.Service.Admin;
using WebApi.Service.Client;

namespace WebApi.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class Request2Controller : ControllerBase
    {
        private readonly ManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly Request2Service _Re2Service;
        public Request2Controller(IMapper mapper, ManagementDbContext context, Request2Service Re2Service)
        {
            _mapper = mapper;
            _Re2Service = Re2Service;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Requirement_Company>> GetAllRequest([FromBody] GetListReqad aa, [FromQuery] string depart)
        {
            var sup = await _Re2Service.GetAllRequest(aa, depart);
            return Ok(sup);
        }
        [HttpGet]
        public async Task<ActionResult<CompanyAccountDTO>> GetAllInfor([FromQuery] string req)
        {
            var company = await _Re2Service.GetAllInfor(req);
            return Ok(company);
        }
        [HttpPost]
        public IActionResult Insert([FromBody] Requirement_C Req , [FromQuery] string id)
        {
            if (Req == null || string.IsNullOrEmpty(Req.ContractNumber))
            {
                Console.WriteLine("Dữ liệu đầu vào không hợp lệ.");
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }
            var result = _Re2Service.Insert(Req,id);

            if (result.StartsWith("RS00"))
            {
                return Ok(new
                {
                    success = true,
                    message = "Tạo yêu cầu thành công",
                    requestID = result
                });
            }
            else
            {
                return BadRequest(new { success = false, message = result });
            }
        }
        [HttpGet]
        public async Task<ActionResult<Requirement_Company>> GetRequestByID([FromQuery] string req)
        {
            var company = await _Re2Service.GetRequestByID(req);
            return Ok(company);
        }

        [HttpPut]
        public IActionResult UpdateStatus([FromBody] historyRequest historyReq)
        {
            try
            {
                Console.WriteLine($"Received UpdateStatus request: RequirementsId = {historyReq.Requirementsid}, Status = {historyReq.Apterstatus}");

                var result = _Re2Service.UpdateStatus(historyReq);

                if (result == null)
                {
                    return Ok(new APIResponse<object>
                    {
                        Success = true,
                        Message = "Cập nhật thành công",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new APIResponse<object>
                    {
                        Success = false,
                        Message = result, // Trả về lỗi cụ thể từ service
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cập nhật: " + ex.Message);
                return BadRequest(new APIResponse<object>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    Data = null
                });
            }
        }


        [HttpGet]
        public async Task<ActionResult<HistoryRequests>> getHIS([FromQuery] string req)
        {
            var company = await _Re2Service.getHIS(req);
            return Ok(company);
        }


        [HttpGet]
        public async Task<ActionResult<ReviewDTO>> GetViewReview([FromQuery] string query)
        {

            var sup = await _Re2Service.GetViewReview(query);
            if (sup == null)
                return NotFound(new { success = false, message = "Không tìm thấy đánh giá." });
            return Ok(sup);
        }
    }
}
