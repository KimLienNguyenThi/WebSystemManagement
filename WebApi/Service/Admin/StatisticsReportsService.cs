using Microsoft.EntityFrameworkCore;
using WebApi.Controllers.Admin;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Service.Admin
{
    public class StatisticsReportsService
    {
        private readonly ManagementDbContext _context;
        public StatisticsReportsService(ManagementDbContext context)
        {
            _context = context;
        }
        public async Task<CustomerOverviewDTO> GetCustomerOverview()
        {
            var dto = new CustomerOverviewDTO();

            // 1. Tổng lượt liên hệ
            dto.TotalContactRequests = await _context.Contacts.CountAsync();

            // 2. Tổng số công ty đã duyệt (IS_ACTIVE = true)
            dto.TotalApprovedCompanies = await _context.Companies
                .CountAsync(c => c.IsActive == true);

            // 3. Tổng số hợp đồng đang hoạt động (IS_ACTIVE = true)
            dto.ActiveContracts = await _context.Contracts
                .CountAsync(c => c.IsActive == true);

            // 4. Số hợp đồng loại thường đã duyệt chính thức (CUSTOMERTYPE = 0, CONSTATUS = 6)
            dto.NormalApprovedContracts = await _context.Contracts
                .CountAsync(c => c.Customertype == false && c.Constatus  == 6);

            // 5. Số hợp đồng VIP đã duyệt chính thức (CUSTOMERTYPE = 1, CONSTATUS = 6)
            dto.VipApprovedContracts = await _context.Contracts
                .CountAsync(c => c.Customertype == true && c.Constatus == 6);
            var threshold = DateTime.Today.AddDays(7);

            // Gom nhóm các hợp đồng theo ORIGINAL (nếu có), để lấy hợp đồng cuối cùng (ENDDATE mới nhất)
            var expiringGroups = await _context.Contracts
                .Where(c => c.Constatus == 6) // chỉ tính hợp đồng đã duyệt
                .GroupBy(c => string.IsNullOrEmpty(c.Original) ? c.Contractnumber : c.Original)
                .Select(g => g.OrderByDescending(x => x.Enddate).FirstOrDefault())
                .Where(c => c.Enddate <= threshold)
                .CountAsync();

            dto.ExpiringContracts = expiringGroups;
            return dto;
        }
        public async Task<RevenueOverviewDTO> GetRevenueOverview(int year)
        {
            try
            {
                var dto = new RevenueOverviewDTO();

                // 1. Tổng số tiền đã thanh toán trong năm
                dto.TotalPaid = await _context.PaymentTransactions
                    .Where(t => t.PaymentResult == 1
                             && t.PaymentDate.HasValue
                             && t.PaymentDate.Value.Year == year
                             && t.Payment != null)
                    .SumAsync(t => (decimal?)t.Payment.Amount) ?? 0;


                // 2. Số lượng giao dịch thành công trong năm
                dto.SuccessfulTransactions = await _context.PaymentTransactions
                    .Where(t => t.PaymentResult == 1
                             && t.PaymentDate.HasValue
                             && t.PaymentDate.Value.Year == year)
                    .CountAsync();

                // 3. Số lượng giao dịch thất bại trong năm
                dto.FailedTransactions = await _context.PaymentTransactions
                    .Where(t => (t.PaymentResult == 0 || t.PaymentResult == 3)
                             && t.PaymentDate.HasValue
                             && t.PaymentDate.Value.Year == year)
                    .CountAsync();

                // 4. Doanh thu theo từng tháng (có dữ liệu)
                var rawMonthly = await _context.PaymentTransactions
                    .Where(t => t.PaymentResult == 1
                             && t.PaymentDate.HasValue
                             && t.PaymentDate.Value.Year == year)
                    .GroupBy(t => t.PaymentDate!.Value.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        Amount = g.Sum(x => x.Amount ?? 0)
                    })
                    .ToListAsync();

                // 5. Luôn tạo danh sách 12 tháng với Amount = 0
                var monthlyRevenue = Enumerable.Range(1, 12)
                    .Select(m => new MonthlyRevenueDTO
                    {
                        Month = $"{m:D2}/{year}",
                        Amount = 0
                    })
                    .ToList();

                // Gán doanh thu thực tế vào danh sách mặc định
                foreach (var item in rawMonthly)
                {
                    var target = monthlyRevenue.FirstOrDefault(x => x.Month.StartsWith($"{item.Month:D2}/"));
                    if (target != null)
                        target.Amount = item.Amount;
                }

                dto.MonthlyRevenue = monthlyRevenue;

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Revenue Overview Error: " + ex.Message);
                throw;
            }
        }



        public async Task<List<ServiceUsageDTO>> GetServiceUsage()
        {
            var result = await _context.ServiceTypes
                .GroupJoin(
                    _context.Contracts.Where(c => c.Constatus == 6),
                    st => st.Id,
                    c => c.ServiceTypeid,
                    (st, contracts) => new ServiceUsageDTO
                    {
                        ServiceTypeName = st.ServiceTypename,
                        ContractCount = contracts.Count()
                    }
                )
                .OrderByDescending(x => x.ContractCount)
                .ToListAsync();

            return result;
        }

        public async Task<ReviewOverviewDTO> GetReviewStatistics()
        {
            var dto = new ReviewOverviewDTO();

            // 1. Tổng số bài đánh giá
            dto.TotalReviews = await _context.Requirements.CountAsync(r => r.IsReviewed);

            // 2. Trung bình sao theo từng tiêu chí
            dto.AverageStarsPerCriteria = await _context.ReviewDetails
                .GroupBy(d => d.CriteriaId)
                .Select(g => new
                {
                    CriteriaId = g.Key,
                    Average = g.Average(x => x.Star)
                })
                .Join(_context.ReviewCriteria,
                    g => g.CriteriaId,
                    c => c.Id,
                    (g, c) => new ReviewCriteriaAverageDTO
                    {
                        CriteriaName = c.CriteriaName,
                        AverageStar = Math.Round(g.Average, 2)
                    })
                .ToListAsync();

            // 3. Tỉ lệ sao theo tiêu chí
            dto.StarDistributionByCriteria = await _context.ReviewCriteria
                .Select(c => new ReviewCriteriaDistributionDTO
                {
                    CriteriaName = c.CriteriaName,
                    Star1 = _context.ReviewDetails.Count(x => x.CriteriaId == c.Id && x.Star == 1),
                    Star2 = _context.ReviewDetails.Count(x => x.CriteriaId == c.Id && x.Star == 2),
                    Star3 = _context.ReviewDetails.Count(x => x.CriteriaId == c.Id && x.Star == 3),
                    Star4 = _context.ReviewDetails.Count(x => x.CriteriaId == c.Id && x.Star == 4),
                    Star5 = _context.ReviewDetails.Count(x => x.CriteriaId == c.Id && x.Star == 5)
                })
                .ToListAsync();

            // 4. Tỉ lệ mức sao trung bình theo từng bài đánh giá
            var reviewAverages = await _context.ReviewDetails
                .GroupBy(rd => rd.ReviewId)
                .Select(g => new
                {
                    ReviewId = g.Key,
                    Avg = g.Average(x => x.Star)
                })
                .ToListAsync();

            var distributionDict = new Dictionary<string, int>
        {
            { "1 sao", 0 },
            { "2 sao", 0 },
            { "3 sao", 0 },
            { "4 sao", 0 },
            { "5 sao", 0 }
        };

            foreach (var r in reviewAverages)
            {
                var rounded = (int)Math.Round(r.Avg);
                switch (rounded)
                {
                    case 1: distributionDict["1 sao"]++; break;
                    case 2: distributionDict["2 sao"]++; break;
                    case 3: distributionDict["3 sao"]++; break;
                    case 4: distributionDict["4 sao"]++; break;
                    case 5: distributionDict["5 sao"]++; break;
                }
            }

            dto.StarLevelDistribution = distributionDict
                .Select(kv => new StarReviewGroupDTO
                {
                    StarLabel = kv.Key,
                    Count = kv.Value
                }).ToList();

            return dto;
        }

        public async Task<List<LoyalCustomerDTO>> GetLoyalCustomers()
        {
            var topCustomers = await (from contract in _context.Contracts
                                      join company in _context.Companies
                                          on contract.Customerid equals company.Customerid
                                      where contract.IsActive == true && company.IsActive == true
                                      group contract by new { company.Id, company.Customerid, company.Companyname } into g
                                      orderby g.Count() descending
                                      select new LoyalCustomerDTO
                                      {
                                          CompanyId = g.Key.Id,
                                          CustomerId = g.Key.Customerid,
                                          CompanyName = g.Key.Companyname,
                                          ContractCount = g.Count()
                                      })
                              .Take(10)
                              .ToListAsync();

            int missingCount = 10 - topCustomers.Count;
            if (missingCount > 0)
            {
                var placeholders = Enumerable.Range(1, missingCount).Select(i => new LoyalCustomerDTO
                {
                    CompanyId = 0,
                    CustomerId = $"N/A_{i}",
                    CompanyName = $"(Trống {i})",
                    ContractCount = 0
                }).ToList();

                topCustomers.AddRange(placeholders);
            }

            return topCustomers;
        }


    }
}
