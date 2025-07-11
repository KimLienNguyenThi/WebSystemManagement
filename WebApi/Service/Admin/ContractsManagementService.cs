﻿using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Models;
using WebApi.Service.Client;

namespace WebApi.Service.Admin
{
    public class ContractsManagementService
    {
        private readonly ManagementDbContext _context;

        public ContractsManagementService(ManagementDbContext context)
        {
            _context = context;
        }

        //Lấy thông tin công ty chính thức và hợp đồng đã duyệt
        public async Task<PagingResult<CompanyAccountDTO>> GetAllCompany(GetListCompanyPaging req)
        {
            var query = from c in _context.Companies
                        join a in _context.Accounts on c.Customerid equals a.Customerid
                        join b in _context.Contracts on c.Customerid equals b.Customerid
                        join h in _context.ServiceTypes on b.ServiceTypeid equals h.Id
                        join q in _context.Payments on b.Contractnumber equals q.Contractnumber
                        where c.IsActive == true && b.Constatus == 6
                        select new CompanyAccountDTO
                        {
                            CustomerId = c.Customerid,
                            CompanyName = c.Companyname,
                            TaxCode = c.Taxcode,
                            CompanyAccount = c.Companyaccount,
                            AccountIssuedDate = c.Accountissueddate,
                            CPhoneNumber = c.Cphonenumber,
                            CAddress = c.Caddress,
                            CustomerType = b.Customertype,
                            ServiceType = h.ServiceTypename,
                            ContractNumber = b.Contractnumber,
                            RootAccount = a.Rootaccount,
                            RootName = a.Rootname,
                            RPhoneNumber = a.Rphonenumber,
                            IsActive = b.IsActive,
                            DateOfBirth = a.Dateofbirth,
                            Gender = a.Gender,
                            Startdate = b.Startdate,
                            Enddate = b.Enddate,
                            Amount = q.Amount,
                            Original = b.Original,
                        };

            if (!string.IsNullOrEmpty(req.Keyword))
            {
                query = query.Where(c =>
                    c.CompanyName.Contains(req.Keyword) ||
                    c.CustomerId.Contains(req.Keyword) ||
                    c.CompanyAccount.Contains(req.Keyword) ||
                    c.TaxCode.Contains(req.Keyword));
            }

            if (req.Category == "Bình thường")
            {
                query = query.Where(c => !c.CustomerType);
            }
            else if (req.Category == "Vip")
            {
                query = query.Where(c => c.CustomerType);
            }

            // Lấy toàn bộ dữ liệu trước khi group
            var rawList = await query.ToListAsync();

            // Gom nhóm theo Original (nếu có) hoặc ContractNumber
            var grouped = rawList
                .GroupBy(x => x.Original ?? x.ContractNumber)
                .Select(g => new CompanyAccountDTO
                {
                    CustomerId = g.First().CustomerId,
                    CompanyName = g.First().CompanyName,
                    TaxCode = g.First().TaxCode,
                    CompanyAccount = g.First().CompanyAccount,
                    AccountIssuedDate = g.First().AccountIssuedDate,
                    CPhoneNumber = g.First().CPhoneNumber,
                    CAddress = g.First().CAddress,
                    CustomerType = g.First().CustomerType,
                    ServiceType = g.First().ServiceType,
                    ContractNumber = g.Key, // dùng ContractNumber gốc
                    RootAccount = g.First().RootAccount,
                    RootName = g.First().RootName,
                    RPhoneNumber = g.First().RPhoneNumber,
                    IsActive = g.First().IsActive,
                    DateOfBirth = g.First().DateOfBirth,
                    Gender = g.First().Gender,
                    Startdate = g.Min(x => x.Startdate), // Lấy nhỏ nhất trong các lần gia hạn
                    Enddate = g.Max(x => x.Enddate),     // Lấy lớn nhất
                    Amount = g.Sum(x => x.Amount),       // Tổng tiền thanh toán của tất cả hợp đồng
                    Original = null // không cần hiển thị Original nữa
                })
                .ToList();

            // Phân trang
            var totalRow = grouped.Count;
            var pagedResult = grouped
                .OrderByDescending(c => c.CustomerId)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();

            var pageCount = (int)Math.Ceiling(totalRow / (double)req.PageSize);

            return new PagingResult<CompanyAccountDTO>
            {
                Results = pagedResult,
                CurrentPage = req.Page,
                RowCount = totalRow,
                PageSize = req.PageSize,
                PageCount = pageCount
            };
        }

        //Insert 1 hợp đồng mới của khách hàng cũ. 
        public async Task<string?> InsertContract(CompanyAccountDTO CompanyAccountDTO, string id)
        {
            if (CompanyAccountDTO == null)
            {
                return "Dữ liệu không hợp lệ.";
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var staff = _context.Staff.FirstOrDefault(s => s.Staffid == id);
                    if (staff == null)
                    {
                        return $"Nhân viên với mã nhân viên = {id} không tồn tại";
                    }

                    var lastContract = _context.Contracts
                .OrderByDescending(c => c.Contractnumber)
                .FirstOrDefault();
                    int nextContractNumber = lastContract != null ? int.Parse(lastContract.Contractnumber.Substring(2)) + 1 : 1;
                    string newContractNumber = $"SV{nextContractNumber:D4}";

                    var serviceType = _context.ServiceTypes
    .FirstOrDefault(st => st.ServiceTypename == CompanyAccountDTO.ServiceType);

                    if (serviceType == null)
                    {
                        return $"Loại dịch vụ '{CompanyAccountDTO.ServiceType}' không tồn tại.";
                    }
                    // ✅ Kiểm tra khách hàng đã có dịch vụ này chưa
                    var existingContract = _context.Contracts
                        .Where(c => c.Customerid == CompanyAccountDTO.CustomerId
                                 && c.ServiceTypeid == serviceType.Id
                                 && c.Enddate >= DateTime.Now)
                        .FirstOrDefault();

                    if (existingContract != null)
                    {
                        return $"Khách hàng đã có hợp đồng với loại dịch vụ '{CompanyAccountDTO.ServiceType}' đang còn hiệu lực.";
                    }
                    var newContract = new Contract
                    {
                        Contractnumber = newContractNumber,
                        Startdate = (DateTime)CompanyAccountDTO.Startdate!,
                        Enddate = (DateTime)CompanyAccountDTO.Enddate!,
                        ServiceTypeid = serviceType.Id,
                        Customerid = CompanyAccountDTO.CustomerId,
                        Constatus = 5
                    };
                    var newPayment = new Payment
                    {
                        //Customerid = newCustomerID,
                        Contractnumber = newContractNumber,
                        Amount = CompanyAccountDTO.Amount,
                        Paymentstatus = false,
                    };
                    _context.Payments.Add(newPayment);
                    _context.Contracts.Add(newContract);
                    _context.SaveChanges();

                    transaction.Commit();

                    return newContractNumber;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                    return "Lỗi hệ thống, vui lòng thử lại sau.";
                }
            }
        }

        //Gia hạn hợp đồng
        public string? InsertExtend(ContractDTO contractDTO, string id)
        {
            if (contractDTO == null || contractDTO.chooseMonth <= 0)
            {
                return "Dữ liệu không hợp lệ.";
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var staff = _context.Staff.FirstOrDefault(s => s.Staffid == id);
                    if (staff == null)
                    {
                        return $"Nhân viên với mã nhân viên = {id} không tồn tại";
                    }

                    var oldContract = _context.Contracts
                        .FirstOrDefault(c => c.Contractnumber == contractDTO.ContractNumber);

                    if (oldContract == null)
                    {
                        return $"Hợp đồng với mã {contractDTO.ContractNumber} không tồn tại.";
                    }

                    // Tạo số hợp đồng mới
                    var lastContract = _context.Contracts
                        .OrderByDescending(c => c.Contractnumber)
                        .FirstOrDefault();

                    int nextContractNumber = lastContract != null
                        ? int.Parse(lastContract.Contractnumber.Substring(2)) + 1
                        : 1;

                    string newContractNumber = $"SV{nextContractNumber:D4}";

                    // Tìm loại dịch vụ
                    var serviceType = _context.ServiceTypes
                        .FirstOrDefault(st => st.ServiceTypename == contractDTO.ServiceType);

                    if (serviceType == null)
                    {
                        return $"Loại dịch vụ '{contractDTO.ServiceType}' không tồn tại.";
                    }

                    DateTime newStartDate;
                    DateTime newEndDate;
                    string? originalContract = null;
                    // Tìm toàn bộ chuỗi hợp đồng gốc + gia hạn
                    var contractChain = _context.Contracts
                        .Where(c => c.Contractnumber == contractDTO.ContractNumber || c.Original == contractDTO.ContractNumber)
                        .ToList();

                    if (!contractChain.Any())
                    {
                        return $"Hợp đồng với mã {contractDTO.ContractNumber} không tồn tại.";
                    }

                    // Tìm hợp đồng có ngày kết thúc lớn nhất
                    var latestContract = contractChain.OrderByDescending(c => c.Enddate).First();

                    if (latestContract.Enddate < DateTime.Today)
                    {
                        // Hợp đồng đã hết hạn → tạo mới
                        newStartDate = DateTime.Today;
                        newEndDate = newStartDate.AddMonths(contractDTO.chooseMonth);
                        originalContract = null;
                    }
                    else
                    {
                        // Hợp đồng còn hạn → gia hạn từ ngày hết hạn cũ + 1 đến ngày người dùng chọn
                        newStartDate = latestContract.Enddate.AddDays(1);
                        newEndDate = contractDTO.Enddate;
                        originalContract = latestContract.Contractnumber;
                    }
                    var newContract = new Contract
                    {
                        Contractnumber = newContractNumber,
                        Startdate = newStartDate,
                        Enddate = newEndDate,
                        ServiceTypeid = serviceType.Id,
                        Customerid = contractDTO.CustomerId,
                        Original = originalContract
                    };

                    var newPayment = new Payment
                    {
                        Contractnumber = newContractNumber,
                        Amount = contractDTO.Amount,
                        Paymentstatus = false
                    };

                    _context.Contracts.Add(newContract);
                    _context.Payments.Add(newPayment);
                    _context.SaveChanges();
                    transaction.Commit();

                    return null;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                    return "Lỗi hệ thống, vui lòng thử lại sau.";
                }
            }
        }
        
        //Nâng cấp hợp đồng
        public string? InsertUpgrade(ContractDTO contractDTO, string id)
        {
            if (contractDTO == null)
            {
                return "Dữ liệu không hợp lệ.";
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var staff = _context.Staff.FirstOrDefault(s => s.Staffid == id);
                    if (staff == null)
                    {
                        return $"Nhân viên với mã nhân viên = {id} không tồn tại.";
                    }

                    var existingContract = _context.Contracts.FirstOrDefault(c => c.Contractnumber == contractDTO.ContractNumber);
                    if (existingContract == null)
                    {
                        return "Hợp đồng không tồn tại.";
                    }

                    // Kiểm tra mã khách hàng có tồn tại trong bảng hợp đồng không
                    if (!_context.Contracts.Any(s => s.Customerid == contractDTO.CustomerId))
                    {
                        return "Mã khách hàng không tồn tại trong hệ thống.";
                    }

                    // Cập nhật loại khách hàng
                    existingContract.Customertype = contractDTO.Customertype;
                    // Cập nhật loại khách hàng cho các hợp đồng phụ có ORIGINAL là contract gốc
                    var relatedContracts = _context.Contracts
                        .Where(c => c.Original == contractDTO.ContractNumber)
                        .ToList();

                    foreach (var contract in relatedContracts)
                    {
                        contract.Customertype = contractDTO.Customertype;
                    }

                    _context.SaveChanges();


                    var newPayment = new Payment
                    {
                        Contractnumber = contractDTO.ContractNumber,
                        Amount = contractDTO.Amount,
                        Paymentstatus = false
                    };

                    _context.Payments.Add(newPayment);
                    _context.SaveChanges();

                    transaction.Commit();
                    return null; // Trả về null khi thành công
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                    return "Lỗi hệ thống, vui lòng thử lại sau.";
                }
            }
        }
        
        //Danh sách giảm giá
        public async Task<List<Endow>> GetListEndow(string id)
        {
            var endowList = await (from endow in _context.Endows
                                   where endow.ServiceGroupid == id
                                   select new Endow
                                   {
                                       Endowid = endow.Endowid,
                                       ServiceGroupid = endow.ServiceGroupid,
                                       Discount = endow.Discount,
                                       Startdate = endow.Startdate,
                                       Enddate = endow.Enddate,
                                       Duration = endow.Duration,
                                   }).ToListAsync();
            return endowList;
        }

        //lấy thông tin cty search để tạo phiếu yêu cầu
                //đã thêm kiểm tra điều kiện hạn hợp đồng còn, vaf  hoat dong.          Bỏ
        //vì ở đây chỉ lấy thông tin cty để insert nên không cần lấy theo nhiều loại dịch vụ hợp đồng. 
        public async Task<List<CompanyAccountDTO>> GetAllInfor(string req)
        {
            req = req?.Trim().ToLower();

            var query = from c in _context.Companies
                        join a in _context.Accounts on c.Customerid equals a.Customerid
                        join h in _context.Contracts
                        on c.Customerid equals h.Customerid
                        join q in _context.ServiceTypes on h.ServiceTypeid equals q.Id
                        where (
                             (string.IsNullOrEmpty(req) ||
                              c.Customerid.ToLower().Contains(req) ||
                              c.Companyname.ToLower().Contains(req) ||
                              c.Taxcode.ToLower().Contains(req))
                             // && h.IsActive == true
                              //&& h.Enddate >= DateTime.Now
                         )
                        group new { c, a } by c.Customerid into g
                        select new CompanyAccountDTO
                        {
                            CustomerId = g.First().c.Customerid,
                            CompanyName = g.First().c.Companyname,
                            TaxCode = g.First().c.Taxcode,
                            CompanyAccount = g.First().c.Companyaccount,
                            CPhoneNumber = g.First().c.Cphonenumber,
                            CAddress = g.First().c.Caddress,
                            RootAccount = g.First().a.Rootaccount,
                            RootName = g.First().a.Rootname,
                            RPhoneNumber = g.First().a.Rphonenumber,
                            Gender = g.First().a.Gender,
                            DateOfBirth = g.First().a.Dateofbirth,
                        };

            return await query.ToListAsync();
        }

        //Lưu thông tin công ty tạm thời. chờ boos ký. 
        public async Task<string?> SaveContractStatusAsync(CompanyContractDTOs dto)
        {
            if (dto == null)
            {
                return "Dữ liệu không hợp lệ.";
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var staff = _context.Staff.FirstOrDefault(s => s.Staffid == dto.ChangedBy);
                    if (staff == null)
                    {
                        return $"Nhân viên với mã nhân viên = {dto.ChangedBy} không tồn tại";
                    }

                    var lastContract = _context.Contracts
                .OrderByDescending(c => c.Contractnumber)
                .FirstOrDefault();
                    if (!_context.ServiceTypes.Any(s => s.ServiceTypename == dto.ServiceType))
                    {
                        return "Loại dịch vụ không tồn tại trong hệ thống. Vui lòng kiểm tra lại.";
                    }

                    int nextContractNumber = lastContract != null ? int.Parse(lastContract.Contractnumber.Substring(2)) + 1 : 1;
                    string newContractNumber = $"SV{nextContractNumber:D4}";

                    var serviceType = _context.ServiceTypes
    .FirstOrDefault(st => st.ServiceTypename == dto.ServiceType);

                    if (serviceType == null)
                    {
                        return $"Loại dịch vụ '{dto.ServiceType}' không tồn tại.";
                    }

                    var newContract = new Contract
                    {
                        Contractnumber = newContractNumber,
                        Startdate = (DateTime)dto.Startdate!,
                        Enddate = (DateTime)dto.Enddate!,
                        ServiceTypeid = serviceType.Id,
                        Customerid = dto.CustomerId,
                        Customertype = dto.CustomerType,
                        IsActive = false,
                        Constatus = 0
                    };
                    var newPayment = new Payment
                    {
                        Contractnumber = newContractNumber,
                        Amount = dto.Amount,
                        Paymentstatus = false,
                    };
                    _context.Payments.Add(newPayment);
                    _context.Contracts.Add(newContract);

                    var newContractfile = new ContractFile
                    {
                        Contractnumber = newContractNumber,
                        ConfileName = dto.ConfileName,
                        FilePath = dto.FilePath,
                        UploadedAt = DateTime.Now,
                        FileStatus = 0,
                    };

                    var newContractStatusHistory = new ContractStatusHistory
                    {
                        Contractnumber = newContractNumber,
                        OldStatus = 0,
                        NewStatus = 0,
                        ChangedAt = DateTime.Now,
                        ChangedBy = dto.ChangedBy,
                    };
                    _context.ContractFiles.Add(newContractfile);
                    _context.ContractStatusHistories.Add(newContractStatusHistory);

                    _context.SaveChanges();
                    transaction.Commit();

                    return newContractNumber;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                    return "Lỗi hệ thống, vui lòng thử lại sau.";
                }
            }
        }

        //Lấy thông tin khách hàng để tạo hợp đồng gia hạn
        public async Task<CompanyAccountDTO?> GetByContractNumberAsync(string contractNumber)
        {
            contractNumber = contractNumber?.Trim();
            var result = await (
                from c in _context.Companies
                join a in _context.Accounts on c.Customerid equals a.Customerid
                join h in _context.Contracts on c.Customerid equals h.Customerid
                join q in _context.ServiceTypes on h.ServiceTypeid equals q.Id
                join f in _context.ContractFiles on h.Contractnumber equals f.Contractnumber into fileJoin
                from file in fileJoin.DefaultIfEmpty()
                join p in _context.Payments on h.Contractnumber equals p.Contractnumber into paymentJoin
                from payment in paymentJoin.DefaultIfEmpty()
                where h.Contractnumber == contractNumber
                select new CompanyAccountDTO
                {
                    CustomerId = c.Customerid,
                    CompanyName = c.Companyname,
                    TaxCode = c.Taxcode,
                    CompanyAccount = c.Companyaccount,
                    AccountIssuedDate = c.Accountissueddate,
                    CPhoneNumber = c.Cphonenumber,
                    CAddress = c.Caddress,
                    RootAccount = a.Rootaccount,
                    RootName = a.Rootname,
                    RPhoneNumber = a.Rphonenumber,
                    DateOfBirth = a.Dateofbirth,
                    Gender = a.Gender,
                    ContractNumber = h.Contractnumber,
                    Startdate = h.Startdate,
                    Enddate = h.Enddate,
                    CustomerType = h.Customertype,
                    ServiceType = q.ServiceTypename,
                    //ConfileName = file.ConfileName,
                    // FilePath = file.FilePath,
                    Amount = payment.Amount,
                    //Constatus = h.Constatus
                }
            ).FirstOrDefaultAsync();

            return result;
        }

        //check hợp đồng gia hạn hoặc nâng cấp còn sót  
        public async Task<string?> CheckExistingIncompleteExtension(string contractNumber)
        {
            var incomplete = await _context.Contracts
                .Where(c => c.Original == contractNumber && c.Constatus != 6)
                .FirstOrDefaultAsync();

            return incomplete?.Contractnumber;
        }

        //Tính toán thời gian gia hạn
        public (DateTime newStartDate, DateTime newEndDate, string? originalContract) CalculateNewContractPeriod(ContractDTO contractDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                DateTime newStartDate;
                DateTime newEndDate;
                string? originalContract = null;
                // Tìm toàn bộ chuỗi hợp đồng gốc + gia hạn
                var contractChain = _context.Contracts
                    .Where(c => c.Contractnumber == contractDTO.ContractNumber || c.Original == contractDTO.ContractNumber)
                    .ToList();

                // Tìm hợp đồng có ngày kết thúc lớn nhất
                var latestContract = contractChain.OrderByDescending(c => c.Enddate).First();

                if (latestContract.Enddate < DateTime.Today)
                {
                    // Hợp đồng đã hết hạn → tạo mới
                    newStartDate = DateTime.Today;
                    newEndDate = newStartDate.AddMonths(contractDTO.chooseMonth);
                    originalContract = null;
                    return (newStartDate, newEndDate, originalContract);
                }

                else
                {
                    // Hợp đồng còn hạn → gia hạn từ ngày hết hạn cũ + 1 đến ngày người dùng chọn
                    newStartDate = latestContract.Enddate.AddDays(1);
                    newEndDate = contractDTO.Enddate;
                    originalContract = latestContract.Contractnumber;
                    return (newStartDate, newEndDate, originalContract);
                }
            }
        }

        public async Task<string?> SaveContractExtend(CompanyContractDTOs dto, ContractDTO contractDTO, string id)
        {
            if (dto == null || contractDTO == null)
            {
                return "Dữ liệu không hợp lệ.";
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var staff = _context.Staff.FirstOrDefault(s => s.Staffid == id);
                    if (staff == null)
                    {
                        return $"Nhân viên với mã nhân viên = {id} không tồn tại";
                    }

                    var oldContract = _context.Contracts
                       .FirstOrDefault(c => c.Contractnumber == contractDTO.ContractNumber);

                    if (oldContract == null)
                    {
                        return $"Hợp đồng với mã {contractDTO.ContractNumber} không tồn tại.";
                    }

                    // Tạo số hợp đồng mới
                    var lastContract = _context.Contracts
                        .OrderByDescending(c => c.Contractnumber)
                        .FirstOrDefault();

                    int nextContractNumber = lastContract != null
                        ? int.Parse(lastContract.Contractnumber.Substring(2)) + 1
                        : 1;

                    string newContractNumber = $"SV{nextContractNumber:D4}";

                    // Tìm loại dịch vụ
                    var serviceType = _context.ServiceTypes
                        .FirstOrDefault(st => st.ServiceTypename == contractDTO.ServiceType);

                    if (serviceType == null)
                    {
                        return $"Loại dịch vụ '{contractDTO.ServiceType}' không tồn tại.";
                    }

                    var newContract = new Contract
                    {
                        Contractnumber = newContractNumber,
                        Startdate = (DateTime)dto.Startdate,
                        Enddate = (DateTime)dto.Enddate,
                        ServiceTypeid = serviceType.Id,
                        Customerid = dto.CustomerId,
                        Customertype = dto.CustomerType,
                        IsActive = false,
                        Constatus = 0, 
                        Original = dto.Original,
                    };
                    var newPayment = new Payment
                    {
                        Contractnumber = newContractNumber,
                        Amount = dto.Amount,
                        Paymentstatus = false,
                    };
                    _context.Payments.Add(newPayment);
                    _context.Contracts.Add(newContract);

                    var newContractfile = new ContractFile
                    {
                        Contractnumber = newContractNumber,
                        ConfileName = dto.ConfileName,
                        FilePath = dto.FilePath,
                        UploadedAt = DateTime.Now,
                        FileStatus = 0,
                    };

                    var newContractStatusHistory = new ContractStatusHistory
                    {
                        Contractnumber = newContractNumber,
                        OldStatus = 0,
                        NewStatus = 0,
                        ChangedAt = DateTime.Now,
                        ChangedBy = dto.ChangedBy,
                    };
                    _context.ContractFiles.Add(newContractfile);
                    _context.ContractStatusHistories.Add(newContractStatusHistory);

                    _context.SaveChanges();
                    transaction.Commit();

                    return newContractNumber;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                    return "Lỗi hệ thống, vui lòng thử lại sau.";
                }
            }
        }

        public async Task<string?> SaveContractUpgrade(CompanyContractDTOs dto, string id)
        {
            if (dto == null)
            {
                return "Dữ liệu không hợp lệ.";
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var staff = _context.Staff.FirstOrDefault(s => s.Staffid == id);
                    if (staff == null)
                    {
                        return $"Nhân viên với mã nhân viên = {id} không tồn tại";
                    }

                    var oldContract = _context.Contracts
                       .FirstOrDefault(c => c.Contractnumber == dto.ContractNumber);

                    if (oldContract == null)
                    {
                        return $"Hợp đồng với mã {dto.ContractNumber} không tồn tại.";
                    }

                    // Tạo số hợp đồng mới
                    var lastContract = _context.Contracts
                        .OrderByDescending(c => c.Contractnumber)
                        .FirstOrDefault();

                    int nextContractNumber = lastContract != null
                        ? int.Parse(lastContract.Contractnumber.Substring(2)) + 1
                        : 1;

                    string newContractNumber = $"SV{nextContractNumber:D4}";

                    // Tìm loại dịch vụ
                    var serviceType = _context.ServiceTypes
                        .FirstOrDefault(st => st.ServiceTypename == dto.ServiceType);

                    if (serviceType == null)
                    {
                        return $"Loại dịch vụ '{dto.ServiceType}' không tồn tại.";
                    }

                    var newContract = new Contract
                    {
                        Contractnumber = newContractNumber,
                        Startdate = (DateTime)dto.Startdate,
                        Enddate = (DateTime)dto.Enddate,
                        ServiceTypeid = serviceType.Id,
                        Customerid = dto.CustomerId,
                        Customertype = dto.CustomerType,
                        IsActive = false,
                        Constatus = 0,
                        Original = dto.Original,
                    };
                    var newPayment = new Payment
                    {
                        Contractnumber = newContractNumber,
                        Amount = dto.Amount,
                        Paymentstatus = false,
                    };
                    _context.Payments.Add(newPayment);
                    _context.Contracts.Add(newContract);

                    var newContractfile = new ContractFile
                    {
                        Contractnumber = newContractNumber,
                        ConfileName = dto.ConfileName,
                        FilePath = dto.FilePath,
                        UploadedAt = DateTime.Now,
                        FileStatus = 0,
                    };

                    var newContractStatusHistory = new ContractStatusHistory
                    {
                        Contractnumber = newContractNumber,
                        OldStatus = 0,
                        NewStatus = 0,
                        ChangedAt = DateTime.Now,
                        ChangedBy = dto.ChangedBy,
                    };
                    _context.ContractFiles.Add(newContractfile);
                    _context.ContractStatusHistories.Add(newContractStatusHistory);

                    _context.SaveChanges();
                    transaction.Commit();

                    return newContractNumber;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                    return "Lỗi hệ thống, vui lòng thử lại sau.";
                }
            }
        }

        public async Task ExpiredContract()
        {
            try
            {
                var contractsExpired = await _context.Contracts.Where(item => item.IsActive == true && item.Enddate < DateTime.Now).ToListAsync();
                if (contractsExpired != null && contractsExpired.Any())
                {
                    foreach (var contract in contractsExpired)
                    {
                        contract.IsActive = false;
                        _context.Contracts.Update(contract);
                        _context.SaveChanges();

                        // Send mail nếu cần
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
            }
        }
        
    }
}
