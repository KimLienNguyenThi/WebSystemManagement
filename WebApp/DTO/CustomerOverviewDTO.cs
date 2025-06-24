namespace WebApi.DTO
    {
        public class CustomerOverviewDTO
        {
            public int TotalContactRequests { get; set; }
            public int TotalApprovedCompanies { get; set; }

            public int ActiveContracts { get; set; }
            public int NormalApprovedContracts { get; set; }
            public int VipApprovedContracts { get; set; }
            public int ExpiringContracts { get; set; } // 👈 Thêm dòng này

        }

        public class RevenueOverviewDTO
        {
            public decimal TotalPaid { get; set; } // Tổng tiền đã thanh toán
            public int SuccessfulTransactions { get; set; }
            public int FailedTransactions { get; set; }

            public List<MonthlyRevenueDTO> MonthlyRevenue { get; set; } = new();
        }

        public class MonthlyRevenueDTO
        {
            public string Month { get; set; } = string.Empty; // VD: "06/2025"
            public decimal Amount { get; set; }
        }
        public class ServiceUsageDTO
        {
            public string ServiceTypeName { get; set; } = string.Empty;
            public int ContractCount { get; set; }
        }

        public class ReviewOverviewDTO
        {
            public int TotalReviews { get; set; }
            public List<ReviewCriteriaAverageDTO> AverageStarsPerCriteria { get; set; } = new();
            public List<ReviewCriteriaDistributionDTO> StarDistributionByCriteria { get; set; } = new();
            public List<StarReviewGroupDTO> StarLevelDistribution { get; set; } = new(); // ✅ thêm cái này
        }

        public class StarReviewGroupDTO
        {
            public string StarLabel { get; set; } = string.Empty; // "1 sao", "2 sao", ...
            public int Count { get; set; }
        }
        public class ReviewCriteriaAverageDTO
        {
            public string CriteriaName { get; set; } = string.Empty;  // Ví dụ: "Thái độ"
            public double AverageStar { get; set; }                   // Ví dụ: 4.25
        }

        public class ReviewCriteriaDistributionDTO
        {
            public string CriteriaName { get; set; } = string.Empty;

            public int Star1 { get; set; }  // Số đánh giá 1 sao cho tiêu chí này
            public int Star2 { get; set; }
            public int Star3 { get; set; }
            public int Star4 { get; set; }
            public int Star5 { get; set; }
        }
    }

