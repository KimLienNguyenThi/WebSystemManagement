﻿namespace WebApi.DTO
{
    public class Requirement_Company
    {
        public string RequirementsId { get; set; } = null!;

        public string Support { get; set; } = null!;

        public string RequirementsStatus { get; set; } = null!;

        public DateTime? DateOfRequest { get; set; }
        public string? DescriptionOfRequest { get; set; } = null!;

        public string CustomerId { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string TaxCode { get; set; } = null!;

        public string CompanyAccount { get; set; } = null!;

        public string CPhoneNumber { get; set; } = null!;

        public string CAddress { get; set; } = null!;

        public bool CustomerType { get; set; }

        public string ServiceType { get; set; } = null!;

        public string ContractNumber { get; set; } = null!;

        public string RootAccount { get; set; } = null!;

        public string RootName { get; set; } = null!;

        public string RPhoneNumber { get; set; } = null!;

    }

    public class GetListReq
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Cutomer { get; set; }
    }

    public class Requirement_C
    {
        // public string RequirementsId { get; set; } = null!;

        public string Support { get; set; } = null!;

        public string RequirementsStatus { get; set; } = null!;

        public DateTime? DateOfRequest { get; set; }
        public string? DescriptionOfRequest { get; set; } = null!;

        public string CustomerId { get; set; } = null!;

    }

    public class updateStatus
    {
        public string status { get; set; }
        public string RequirementsId { get; set; }
    }
    public class historyRequest
    {
        public string Requirementsid { get; set; }
        public string Descriptionofrequest { get; set; }
        public string Apterstatus { get; set; }

        public string Staffid { get; set; }
    }

    public class Request_GroupCompany_DTO
    {
        public Request_Group Request_Group { get; set; }
        public List<HistoryRequests> HistoryRequests { get; set; }
    }

    public class Request_Group
    {
        public string RequirementsId { get; set; } = null!;
        public string ContractNumber { get; set; } = null!;

        public string Support { get; set; } = null!;
        public string RequirementsStatus { get; set; } = null!;
        public DateTime? DateOfRequest { get; set; }
        public string? DescriptionOfRequest { get; set; } = null!;
    }
    public class HistoryRequests
    {
        public string Requirementsid { get; set; }
        public string Staffid { get; set; }
        public string Descriptionofrequest { get; set; }
        public string BeforStatus { get; set; }
        public string Apterstatus { get; set; }
        public DateTime? DateOfRequest { get; set; }
    }
    public class Requirement_Company1
    {
        public string RequirementsId { get; set; } = null!;

        public string Support { get; set; } = null!;

        public string RequirementsStatus { get; set; } = null!;

        public DateTime? DateOfRequest { get; set; }
        public string? DescriptionOfRequest1 { get; set; } = null!;

        public string ContractNumber { get; set; } = null!;
        public string Staffid { get; set; }
        public string Descriptionofrequest2 { get; set; }
        public string BeforStatus { get; set; }
        public string Apterstatus { get; set; }

    }
}
