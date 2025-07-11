using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Models;
using WebApi.Service.Admin;

namespace WebApi.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class StatisticsReportsController : ControllerBase
    {
        private readonly StatisticsReportsService _reportsService;
        private readonly ManagementDbContext _context;

        public StatisticsReportsController(StatisticsReportsService reportsService, ManagementDbContext context)
        {
            _reportsService = reportsService;
            _context = context;
        }

        [Authorize(Roles = "Admin,Director")]
        [HttpGet]
        public async Task<ActionResult<CustomerOverviewDTO>> GetCustomerOverview()
        {
            var data = await _reportsService.GetCustomerOverview();
            return Ok(data);
        }

        //doanh thu
        [Authorize(Roles = "Admin,Director")]
        [HttpGet]
        public async Task<ActionResult<RevenueOverviewDTO>> GetRevenueOverview([FromQuery] int year)
        {
            var result = await _reportsService.GetRevenueOverview(year);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Director")]

        [HttpGet]
        public async Task<ActionResult<ServiceUsageDTO>> GetServiceUsage()
        {
            var data = await _reportsService.GetServiceUsage();
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Director")]

        [HttpGet]
        public async Task<ActionResult<ReviewOverviewDTO>> GetReviewStatistics()
        {
            var result = await _reportsService.GetReviewStatistics();
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Director")]

        [HttpGet]
        public async Task<ActionResult<List<ReviewOverviewDTO>>> GetLoyalCustomers()
        {
            var result = await _reportsService.GetLoyalCustomers();
            return Ok(result);
        }
    }
}
