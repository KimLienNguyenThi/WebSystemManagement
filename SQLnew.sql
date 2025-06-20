﻿CREATE DATABASE MANAGEMENT;

USE MANAGEMENT;

-- T?O B?NG COMPANY
CREATE TABLE COMPANY (
    ID INT PRIMARY KEY IDENTITY NOT NULL,
    CUSTOMERID VARCHAR(15) NOT NULL UNIQUE,      -- MÃ KH (S? ???C T?O T? ??NG)
    COMPANYNAME NVARCHAR(150) NOT NULL,			-- TÊN KH
    TAXCODE VARCHAR(30) NOT NULL UNIQUE,		-- MÃ S? THU?
    COMPANYACCOUNT VARCHAR(100) NOT NULL UNIQUE, -- EMAIL
    ACCOUNTISSUEDDATE DATETIME,				-- NGÀY C?P TK
    CPHONENUMBER VARCHAR(10) NOT NULL UNIQUE, -- S?T	
    CADDRESS NVARCHAR(150) NOT NULL,     -- ??A CH?
	IS_ACTIVE BIT, --0 Bản nháp, 1 bản duyệt
	
    --CUSTOMERTYPE BIT NOT NULL,            -- PHÂN LO?I KH (0: BÌNH TH??NG, 1: VIP)
	--SERVICETYPE NVARCHAR(50) NOT NULL,	 -- LO?I D?CH V?
	--CONTRACTNUMBER VARCHAR(10) NOT NULL  -- S? H?P ??NG
	--OPERATINGSTATUS BIT NOT NULL,            -- TR?NG THÁI (0: KHÔNG HO?T ??NG, 1: HO?T ??NG)
);

-- T?O B?NG ACCOUNT
CREATE TABLE ACCOUNT (
    CUSTOMERID VARCHAR(15) PRIMARY KEY NOT NULL, -- MÃ KH (KHÓA CHÍNH & KHÓA NGO?I)
    ROOTACCOUNT VARCHAR(100) NOT NULL UNIQUE, -- EMAIL
    ROOTNAME NVARCHAR(40) NOT NULL,          -- H? TÊN
    RPHONENUMBER VARCHAR(10) NOT NULL UNIQUE, -- S?T
    DATEOFBIRTH DATETIME NOT NULL,           -- NGÀY SINH
    GENDER BIT NOT NULL,                     -- GI?I TÍNH (0: NAM, 1: N?)
	--IS_ACTIVE BIT, --0 Bản nháp, 1 bản duyệt
    CONSTRAINT FK_ACCOUNT_COMPANY FOREIGN KEY (CUSTOMERID) REFERENCES COMPANY(CUSTOMERID) ON DELETE CASCADE
);

--T?O B?NG LOGIN CLIENT
CREATE TABLE LOGINCLIENT(
	CUSTOMERID VARCHAR(15) NOT NULL PRIMARY KEY,
	FOREIGN KEY (CUSTOMERID) REFERENCES COMPANY(CUSTOMERID) ON DELETE CASCADE,
	USERNAME VARCHAR(40) NOT NULL,
	PASSWORDCLIENT VARCHAR(100) NOT NULL
);

--T?O B?NG RESETPASS
CREATE TABLE RESETPASSWORD(
	ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CUSTOMERID VARCHAR(15) NOT NULL,			--MÃ KH
	FOREIGN KEY (CUSTOMERID) REFERENCES COMPANY(CUSTOMERID),
	USERNAME VARCHAR(40) NOT NULL,
	PASSWORDCLIENT VARCHAR(100) NOT NULL
);

--LO?I H? TR? DỰA VÀO ĐÂY CHIA VỀ BỘ PHẬN 
CREATE TABLE SUPPORT_TYPE (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
    SUPPORT_CODE VARCHAR(10) NOT NULL UNIQUE, 
    SUPPORT_NAME NVARCHAR(40) NOT NULL UNIQUE
);

--NHÓM D?CH V? 
CREATE TABLE SERVICE_GROUP (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
    SERVICE_GROUPID VARCHAR(10) NOT NULL UNIQUE, 
    GROUP_NAME NVARCHAR(50) NOT NULL
);

--LO?I D?CH V?
CREATE TABLE SERVICE_TYPE (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,  
    SERVICE_GROUPID VARCHAR(10) NOT NULL,
    SERVICE_TYPENAME NVARCHAR(255) NOT NULL,
	DESCRIPTION_SR NVARCHAR(MAX) NOT NULL,  -- Mô tả chi tiết về loại dịch vụ
    FOREIGN KEY (SERVICE_GROUPID) REFERENCES SERVICE_GROUP(SERVICE_GROUPID)
);

--T?O B?NG H?P ??NG
CREATE TABLE CONTRACTS (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY, -- CHỈ DÙNG ID LÀM KHÓA CHÍNH
    CONTRACTNUMBER VARCHAR(10) NOT NULL UNIQUE, --hợp đồng
    STARTDATE DATETIME NOT NULL,
    ENDDATE DATETIME NOT NULL,
	SERVICE_TYPEID INT NOT NULL,           --loại dịch vụ
	CUSTOMERID VARCHAR(15) NOT NULL,
	ORIGINAL VARCHAR(10) NULL,				-- mã hợp đồng gốc nếu có thay đổi hạn 
	CUSTOMERTYPE BIT NOT NULL,            -- PHÂN LO?I KH (0: BÌNH TH??NG, 1: VIP)
    FOREIGN KEY (SERVICE_TYPEID) REFERENCES SERVICE_TYPE(ID),
    FOREIGN KEY (CUSTOMERID) REFERENCES COMPANY(CUSTOMERID),
	IS_ACTIVE BIT NOT NULL, -- 0 NGỪNG HOẠT ĐỘNG, 1 HOẠT ĐỘNG
	CONSTATUS INT --0: CHƯA KÝ, 1: Đã ký, 2. Chờ Client ký , 3 :Kyhoantat 4.Đã duyệt ký  5 đã thanh toán 6 Duyệt (chính thức),7: Gia hạn, 8: Bị từ chối  
	--cHỈ LẤY CÁI MỚI NHẤT TỪ CONTRACTFILE CHO HỢP ĐỒNG ĐÓ
);


-- Bảng lưu file hợp đồng
CREATE TABLE CONTRACT_FILES (
    ID INT IDENTITY PRIMARY KEY,
    CONTRACTNUMBER VARCHAR(10) NOT NULL, --hợp đồng
    CONFILE_NAME NVARCHAR(255) NOT NULL,
    FILE_PATH NVARCHAR(500) NOT NULL,
    UPLOADED_AT DATETIME DEFAULT GETDATE(),
    FILE_STATUS INT --0: CHƯA KÝ, 1: Đã ký, 2. Chờ Client ký , 3 :Kyhoantat 4.Đã duyệt ký  5 đã thanh toán 6 Duyệt (chính thức),7: Gia hạn, 8: Bị từ chối  
    FOREIGN KEY (CONTRACTNUMBER) REFERENCES CONTRACTS(CONTRACTNUMBER)
);

-- Bảng lưu lịch sử thay đổi trạng thái
CREATE TABLE CONTRACT_STATUS_HISTORY (
    ID INT IDENTITY PRIMARY KEY,
    CONTRACTNUMBER VARCHAR(10) NOT NULL, --hợp đồng
    OLD_STATUS INT,
    NEW_STATUS INT,
    CHANGED_AT DATETIME DEFAULT GETDATE(),
    CHANGED_BY NVARCHAR(100),
    FOREIGN KEY (CONTRACTNUMBER) REFERENCES CONTRACTS(CONTRACTNUMBER),
);


--T?O B?NG NHÂN VIÊN
CREATE TABLE STAFF (
	ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	STAFFID VARCHAR(10) NOT NULL UNIQUE,
	STAFFEMAIL VARCHAR(100) NOT NULL UNIQUE, -- EMAIL
	STAFFNAME NVARCHAR(40) NOT NULL,
	STAFFDATE DATETIME,
	STAFFGENDER BIT, 
	STAFFADDRESS NVARCHAR(100),
	STAFFPHONE VARCHAR(10) NOT NULL UNIQUE,
	DEPARTMENT NVARCHAR(50) NOT NULL --B? PH?N
);

--T?O B?NG LOGIN NHÂN VIÊN
CREATE TABLE LOGINADMIN(
	STAFFID VARCHAR(10) NOT NULL PRIMARY KEY,				
	FOREIGN KEY (STAFFID) REFERENCES STAFF(STAFFID) ON DELETE CASCADE,
	USERNAMEAD VARCHAR(40) NOT NULL,
	PASSWORDAD VARCHAR(100) NOT NULL
);

--T?O B?NG YÊU C?U
CREATE TABLE REQUIREMENTS
(
	ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	REQUIREMENTSID VARCHAR(10) NOT NULL UNIQUE, --MÃ YÊU C?U
	REQUIREMENTSSTATUS NVARCHAR(40) NOT NULL,--TR?NG THÁI YÊU C?U

	--REQUIREMENTSSTATUS INT NOT NULL,--TR?NG THÁI YÊU C?U

	DATEOFREQUEST DATETIME,					--NGÀY T?O
	DESCRIPTIONOFREQUEST NVARCHAR(MAX) NOT NULL   ,    --MÔ  TA YÊU C?U 
	--CUSTOMERID VARCHAR(15) NOT NULL,			--MÃ KH
    CONTRACTNUMBER VARCHAR(10) NOT NULL,
	SUPPORT_CODE VARCHAR(10) NOT NULL, 
	--FOREIGN KEY (CUSTOMERID) REFERENCES COMPANY(CUSTOMERID),
	FOREIGN KEY (SUPPORT_CODE) REFERENCES SUPPORT_TYPE(SUPPORT_CODE),
	FOREIGN KEY (CONTRACTNUMBER) REFERENCES CONTRACTS(CONTRACTNUMBER)
);

--T?O B?NG L?CH S? C?P NH?T YÊU C?U
CREATE TABLE HISTORYREQ
(
	ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
	REQUIREMENTSID VARCHAR(10) NOT NULL, 
	DESCRIPTIONOFREQUEST NVARCHAR(MAX) NOT NULL   ,    --MÔ  TA YÊU C?U 
	DATEOFUPDATE DATETIME,					--NGÀY T?O
	BEFORSTATUS NVARCHAR(40) NOT NULL,--TR?NG THÁI YÊU C?U TR??C 
	APTERSTATUS NVARCHAR(40) NOT NULL,--TR?NG THÁI YÊU C?U SAU
	STAFFID VARCHAR(10) NOT NULL,
	FOREIGN KEY (STAFFID) REFERENCES STAFF(STAFFID),
	FOREIGN KEY (REQUIREMENTSID) REFERENCES REQUIREMENTS(REQUIREMENTSID)
);

--TẠO BẢNG PHÂN CÔNG YÊU CẦU CHO NHÂN VIÊN BỘ PHẬN NÀO 
CREATE TABLE ASSIGN(
	ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
	REQUIREMENTSID VARCHAR(10) NOT NULL, 
	DEPARTMENT NVARCHAR(50) NOT NULL,--B? PH?N LẤY THEO LOẠI HỖ TRỢ
	STAFFID VARCHAR(10),   --LÚC SAU NHÂN VIÊN BỘ PHẬN ĐÓ CÓ THỂ NHẬN
	FOREIGN KEY (STAFFID) REFERENCES STAFF(STAFFID),
	FOREIGN KEY (REQUIREMENTSID) REFERENCES REQUIREMENTS(REQUIREMENTSID)
);

--T?O B?NG ?ÁNH GIÁ
CREATE TABLE REVIEW (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
    REQUIREMENTSID VARCHAR(10) NOT NULL,  -- MÃ YÊU C?U
    --CUSTOMERID VARCHAR(15) NOT NULL,  -- MÃ KHÁCH HÀNG
    COMMENT NVARCHAR(MAX) NOT NULL,  -- MÔ T? ?ÁNH GIÁ T?NG TH?
    DATEOFUPDATE DATETIME DEFAULT GETDATE(),  -- NGÀY ?ÁNH GIÁ
	--STAFFID VARCHAR(10) NOT NULL,
	--FOREIGN KEY (STAFFID) REFERENCES STAFF(STAFFID),
    --FOREIGN KEY (CUSTOMERID) REFERENCES COMPANY(CUSTOMERID),
    FOREIGN KEY (REQUIREMENTSID) REFERENCES REQUIREMENTS(REQUIREMENTSID)
);

CREATE TABLE REVIEW_CRITERIA (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    CRITERIA_NAME NVARCHAR(100) NOT NULL UNIQUE  -- TÊN TIÊU CHÍ (VD: "THÁI ??", "TH?I GIAN X? LÝ")
);

CREATE TABLE REVIEW_DETAIL (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
    REVIEW_ID INT NOT NULL,  -- LIÊN K?T V?I B?NG REVIEW
    CRITERIA_ID INT NOT NULL,  -- LIÊN K?T V?I B?NG REVIEW_CRITERIA
    STAR INT CHECK (STAR BETWEEN 1 AND 5) NOT NULL,  -- ?I?M (1-5 SAO)
    FOREIGN KEY (REVIEW_ID) REFERENCES REVIEW(ID),
    FOREIGN KEY (CRITERIA_ID) REFERENCES REVIEW_CRITERIA(ID)
);

--TẠO BẢNG QUẢN LÝ THANH TOÁN
CREATE TABLE PAYMENT(
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    CONTRACTNUMBER VARCHAR(10) NOT NULL,  -- MÃ HỢP ĐỒNG
	AMOUNT DECIMAL(18,2) NOT NULL,        -- SỐ TIỀN THANH TOÁN
	PAYMENTSTATUS BIT NOT NULL,           -- 0: CHƯA THANH TOÁN, 1: ĐÃ THANH TOÁN
    FOREIGN KEY (CONTRACTNUMBER) REFERENCES CONTRACTS(CONTRACTNUMBER)
);

CREATE TABLE PAYMENT_TRANSACTION (
    ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    TRANSACTION_CODE VARCHAR(50)  UNIQUE,       -- Mã nội bộ duy nhất
    AMOUNT DECIMAL(18,2),                      -- Số tiền giao dịch
    BANK_CODE NVARCHAR(20),                             -- Mã ngân hàng (vnp_BankCode)
    BANK_TRANSACTION_CODE NVARCHAR(50),                 -- Mã giao dịch tại ngân hàng (vnp_BankTranNo)
    CARD_TYPE NVARCHAR(20),                             -- Loại thẻ (ATM, CREDIT...)
    ORDER_INFO NVARCHAR(255),                           -- Nội dung thanh toán (vnp_OrderInfo)
    PAYMENT_DATE DATETIME ,   -- Ngày giao dịch
    RESPONSE_CODE VARCHAR(10),                          -- Mã phản hồi từ VNPAY
    TMN_CODE NVARCHAR(20),                              -- Mã merchant
    PAYMENT_METHOD NVARCHAR(50),                        -- Phương thức thanh toán
    PAYMENT_RESULT INT DEFAULT 0,              -- 0: Thất bại, 1: Thành công, 2: Đang xử lý, 3: Hủy, 4: Nghi ngờ
    EMAIL NVARCHAR(100),                                -- Email người thanh toán (nếu có)
    PAYMENT_ID INT ,                            -- FK đến bảng PAYMENT
    FOREIGN KEY (PAYMENT_ID) REFERENCES PAYMENT(ID)
);


-- tạo bảng giá dịch vụ 
CREATE TABLE REGULATIONS
(
	ID INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
	SERVICE_GROUPID VARCHAR(10) NOT NULL, 
	PRICE DECIMAL(18,2) NOT NULL,        
	FOREIGN KEY(SERVICE_GROUPID) REFERENCES SERVICE_GROUP(SERVICE_GROUPID)
);
-- BẢNG ƯU ĐÃI
CREATE TABLE ENDOW
(
	ID INT PRIMARY KEY IDENTITY(1,1),
	ENDOWID VARCHAR(10) NOT NULL UNIQUE,  -- MÃ ƯU ĐÃI
	SERVICE_GROUPID VARCHAR(10) NOT NULL, -- ÁP DỤNG CHO LOẠI DỊCH VỤ NÀO
	DISCOUNT FLOAT NOT NULL,              -- % GIẢM
	STARTDATE DATETIME NULL,              -- BẮT ĐẦU ƯU ĐÃI
	ENDDATE DATETIME NULL,                -- KẾT THÚC ƯU ĐÃI
	DURATION INT NULL,                    -- SỐ THÁNG ÁP DỤNG (nếu có) CHO DẠNG HỢP ĐỒNG BAO LÂU 
	DESCRIPTIONENDOW NVARCHAR(255) NULL,       -- MÔ TẢ ƯU ĐÃI
	FOREIGN KEY(SERVICE_GROUPID) REFERENCES SERVICE_GROUP(SERVICE_GROUPID)
);

CREATE TABLE PASSWORDRESETTOKEN (
    ID INT PRIMARY KEY IDENTITY(1,1),
    EMAIL NVARCHAR(100) NOT NULL,   
    OTP NVARCHAR(10) NOT NULL,      
    EXPIRYTIME DATETIME NOT NULL,   
    ISUSED BIT NOT NULL DEFAULT 0  
);

CREATE TABLE [dbo].[CONTACT](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Subject] [nvarchar](500) NULL,
	[Message] [nvarchar](2000) NULL,
	[Status] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CompanyName] [nvarchar](500) NULL,

 CONSTRAINT [PK_CONTACT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE NOTIFICATIONS (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Typenoti INT NOT NULL,
    ReferenceId BIGINT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    Data NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);


ALTER TABLE CONTRACTS
ALTER COLUMN SERVICE_TYPEID INT NULL;

SELECT name 
FROM sys.foreign_keys 
WHERE parent_object_id = OBJECT_ID('CONTRACTS');

ALTER TABLE CONTRACTS
DROP CONSTRAINT FK__CONTRACTS__SERVI__52593CB8;

ALTER TABLE CONTRACTS
DROP CONSTRAINT FK__CONTRACTS__CUSTO__534D60F1;

ALTER TABLE CONTRACTS
ADD CONSTRAINT FK_CONTRACTS_SERVICE_TYPEID
FOREIGN KEY (SERVICE_TYPEID) REFERENCES SERVICE_TYPE(ID)
ON DELETE SET NULL;

INSERT INTO SUPPORT_TYPE ( SUPPORT_CODE, SUPPORT_NAME) VALUES 
('SP0001',N'Hỗ trợ Cước phí'),
('SP0002',N'Cập nhật hợp đồng, dịch vụ'),
('SP0003',N'Hỗ trợ Kỹ thuật'),
('SP0004',N'Bảo hành thiết bị');	

-- Thêm nhóm dịch vụ
INSERT INTO SERVICE_GROUP (SERVICE_GROUPID, GROUP_NAME) VALUES
('SER0001', N'Viễn thông & Truyền thông'),
('SER0002', N'Điện toán đám mây & Phần mềm'),
('SER0003', N'Hỗ trợ & An toàn thông tin'),
('SER0004', N'Hợp đồng & Đối tác');

-- Thêm loại dịch vụ
INSERT INTO SERVICE_TYPE (SERVICE_GROUPID, SERVICE_TYPEName,DESCRIPTION_SR) VALUES 
('SER0001', N'Đầu số thoại', N'Cung cấp đầu số điện thoại cố định hoặc di động cho doanh nghiệp. Hỗ trợ đăng ký, chuyển đổi, và duy trì hoạt động đầu số phục vụ tổng đài hoặc hệ thống liên lạc nội bộ.'),
('SER0001', N'Kênh truyền', N'Cung cấp kênh truyền dữ liệu chuyên biệt (leased line, MPLS, VPN...). Đảm bảo kết nối ổn định, bảo mật cao giữa các chi nhánh/doanh nghiệp.'),
('SER0001', N'Tổng đài', N'Triển khai hệ thống tổng đài nội bộ (IPPBX, VoIP) hỗ trợ gọi nội bộ, gọi ra ngoài, ghi âm cuộc gọi, định tuyến thông minh. Hỗ trợ vận hành, bảo trì và xử lý sự cố.'),
('SER0001', N'Hội nghị truyền hình', N'Cung cấp nền tảng và thiết bị hội nghị trực tuyến chất lượng cao, hỗ trợ kết nối đa điểm, chia sẻ màn hình và ghi hình cuộc họp.'),
('SER0001', N'Tin nhắn', N'Dịch vụ gửi tin nhắn SMS/Brandname phục vụ truyền thông, chăm sóc khách hàng, OTP. Có báo cáo thống kê và hỗ trợ API tích hợp.'),
('SER0001', N'Dịch vụ truyền hình', N'Triển khai hệ thống truyền hình IPTV hoặc truyền hình doanh nghiệp, hỗ trợ truyền phát nội dung tùy chỉnh theo nhu cầu khách hàng.'),
('SER0002', N'Cloud partner', N'Tư vấn, triển khai và đồng hành cùng khách hàng trong quá trình sử dụng dịch vụ từ các nhà cung cấp Cloud lớn (AWS, Azure, GCP...).'),
('SER0002', N'Điện toán đám mây', N'Cung cấp dịch vụ hạ tầng ảo hóa (IaaS), lưu trữ, máy chủ ảo trên nền tảng cloud. Đảm bảo tính sẵn sàng cao, bảo mật và linh hoạt trong mở rộng.'),
('SER0002', N'Dịch vụ điện tử', N'Triển khai và hỗ trợ các nền tảng giao dịch điện tử như ký số, hóa đơn điện tử, cổng thanh toán, chứng thực số,...'),
('SER0002', N'Dịch vụ phần mềm (SaaS)', N'Cung cấp các phần mềm dạng dịch vụ (CRM, ERP, quản lý nhân sự, ticket support...) qua nền tảng web, không cần cài đặt, hỗ trợ vận hành và bảo trì.'),
('SER0003', N'An toàn thông tin', N'Cung cấp giải pháp bảo mật mạng, bảo vệ dữ liệu, giám sát tấn công, quản lý rủi ro an ninh. Có đội ngũ SOC hỗ trợ 24/7.'),
('SER0003', N'Giám sát', N'Giải pháp giám sát hệ thống CNTT: hiệu năng hệ thống, ứng dụng, mạng; cảnh báo sự cố sớm để giảm thiểu gián đoạn.'),
('SER0003', N'Trung tâm dữ liệu', N'Dịch vụ đặt máy chủ (Colocation), thuê chỗ (hosting), và các giải pháp lưu trữ tại Data Center đạt chuẩn Tier III trở lên.'),
('SER0003', N'Thiết bị', N'Cung cấp, lắp đặt và cấu hình thiết bị mạng, máy chủ, thiết bị bảo mật (firewall, switch, router...) kèm dịch vụ bảo trì.'),
('SER0003', N'Hỗ trợ CNTT', N'Dịch vụ Helpdesk/IT outsourcing hỗ trợ người dùng cuối, khắc phục sự cố, bảo trì định kỳ, tư vấn tối ưu hệ thống CNTT doanh nghiệp.'),
('SER0004', N'Hợp đồng tích hợp/dự án', N'Cung cấp giải pháp tổng thể theo yêu cầu doanh nghiệp: từ tư vấn, thiết kế, triển khai, đến bảo trì hệ thống CNTT/viễn thông.'),
('SER0004', N'Hợp đồng hợp tác', N'Các hợp đồng hợp tác chiến lược giữa hai bên để cùng khai thác dịch vụ, chia sẻ lợi ích, thúc đẩy chuyển đổi số hoặc phát triển hạ tầng.'),
('SER0004', N'Đối tác', N'Thiết lập và duy trì mối quan hệ với các đối tác chiến lược nhằm mở rộng thị trường, nâng cao năng lực cung cấp dịch vụ và tạo ra giá trị cộng hưởng. Hỗ trợ tư vấn mô hình hợp tác, chia sẻ hạ tầng, công nghệ và khai thác dịch vụ chung.');

insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0001','1000000')
insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0002','1500000')
insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0003','1800000')
insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0004','1200000')

insert into staff( STAFFID, STAFFNAME, STAFFADDRESS, STAFFDATE, STAFFEMAIL, STAFFGENDER, STAFFPHONE,DEPARTMENT)
values('Sta1', N'Nguyễn Thị Kim Liên', N'Quận 9', '2000-08-25', 'lien@gmail.com',1,'0365812848',N'Director'),
	  ('Sta2', N'Nguyễn Thành A', N'Quận 9', '2000-08-25', 'liennbtbt11@gmail.com',1,'0365812847',N'Admin')

INSERT INTO LOGINADMIN(STAFFID, USERNAMEAD, PASSWORDAD) VALUES ('Sta1', '0365812848','$2b$12$N1SXbDnA99tyVDL2ZbRKHeR59ou2F.O88hf9roeTrz.U4yHrswrye');
INSERT INTO LOGINADMIN(STAFFID, USERNAMEAD, PASSWORDAD) VALUES ('Sta2', '0365812847','$2a$11$WhusSTM44fBBxah.J17CIObhHKylM7PfbFW7jddYDWJ.RjFQlzT1G');


INSERT INTO ENDOW(ENDOWID,SERVICE_GROUPID,DISCOUNT,STARTDATE,ENDDATE)
VALUES ('ENDOW00001','SER0001',5,'2024-08-25','2026-08-25')

--nếu hợp đồng >=6 tháng giảm 5%
--nếu hợp đồng >=12 tháng giảm 10%
--nếu TELCO 1.000.000/ tháng 
--nếu CLOUD_SW 1.500.000/ tháng
--nếu SUPPORT 1.800.000/ tháng
--nếu CONTRACT 1.200.000/ tháng
--VIP tăng 30%

--INSERT INTO Company ( customerID, companyName, taxCode, companyAccount, accountIssuedDate, cPhoneNumber, cAddress, customerType,OPERATINGSTATUS) VALUES
--('IT03030001', N'TNHH Nam Á', '012345432', 'rireland0@answers.com', '2024-08-25', '0147258369', N'Đường Lê Lợi, quận 10', 0,0),
--('IT03030002', N'Công ty Cổ phần ABC', '012355432', 'contact@abc.com', '2024-09-10', '0147258370', N'123 Đường Trần Hưng Đạo, quận 1', 1,1),
--('IT03030003', N'Công ty TNHH XYZ', '012365432', 'info@xyz.com', '2024-10-05', '0147258371', N'456 Đường Nguyễn Huệ, quận 3', 0,0),
--('IT03030004', N'Công ty TNHH DEF', '012375432', 'support@def.com', '2024-11-15', '0147258372', N'789 Đường Lý Thường Kiệt, quận 5', 1,1),
--('IT03030005', N'Công ty Cổ phần GHI', '012385432', 'sales@ghi.com', '2024-12-20', '0147258373', N'321 Đường Phạm Ngũ Lão, quận 7', 0,0),
--('IT03030006', N'Công ty TNHH JKL', '012395432', 'contact@jkl.com', '2025-01-10', '0147258374', N'654 Đường Hai Bà Trưng, quận 2', 1,1),
--('IT03030007', N'Công ty Cổ phần MNO', '012405432', 'info@mno.com', '2025-02-05', '0147258375', N'987 Đường Lê Văn Sỹ, quận 4', 0,0),
--('IT03030008', N'Công ty TNHH PQR', '012415432', 'support@pqr.com', '2025-03-15', '0147258376', N'159 Đường Nguyễn Thị Minh Khai, quận 6', 1,1),
--('IT03030009', N'Công ty Cổ phần STU', '012425432', 'sales@stu.com', '2025-04-20', '0147258377', N'753 Đường Điện Biên Phủ, quận 8', 0,0),
--('IT03030010', N'Công ty TNHH VWX', '012435432', 'contact@vwx.com', '2025-05-25', '0147258378', N'852 Đường Phan Đình Phùng, quận 9', 1,1),
--('IT03030011', N'Công ty Cổ phần YZA', '012445432', 'info@yza.com', '2025-06-30', '0147258379', N'951 Đường Hoàng Văn Thụ, quận 11', 0,0),
--('IT03030012', N'Công ty TNHH BCD', '012455432', 'support@bcd.com', '2025-07-05', '0147258380', N'159 Đường Lý Chính Thắng, quận 12', 1,1),
--('IT03030013', N'Công ty Cổ phần EFG', '012465432', 'sales@efg.com', '2025-08-10', '0147258381', N'357 Đường Nam Kỳ Khởi Nghĩa, quận Bình Thạnh', 0,0),
--('IT03030014', N'Công ty TNHH HIJ', '012475432', 'contact@hij.com', '2025-09-15', '0147258382', N'753 Đường Võ Thị Sáu, quận Phú Nhuận', 1,1),
--('IT03030015', N'Công ty Cổ phần KLM', '012485432', 'info@klm.com', '2025-10-20', '0147258383', N'456 Đường Nguyễn Văn Trỗi, quận Tân Bình', 0,0);

--INSERT INTO Account (customerID, rootAccount, rootName, rPhoneNumber, dateOfBirth, gender) VALUES
--('IT03030001', 'nam.a@domain.com', N'Nguyễn Văn Huy', '0912345671',  '1985-05-15', 0),
--('IT03030002', 'abc@domain.com', N'Trần Quốc Khang', '0912345672',  '1990-08-22', 0),
--('IT03030003', 'xyz@domain.com', N'Lê Hoàng Bảo', '0912345673',  '1987-12-05', 0),
--('IT03030004', 'def@domain.com', N'Phạm Minh Anh', '0912345674',  '1992-03-18', 1),
--('IT03030005', 'ghi@domain.com', N'Hoàng Gia Hân', '0912345675',  '1989-07-30', 1),
--('IT03030006', 'jkl@domain.com', N'Vũ Tuấn Khoa', '0912345676',  '1991-11-11', 0),
--('IT03030007', 'mno@domain.com', N'Đặng Thế Phát', '0912345677',  '1986-02-25', 0),
--('IT03030008', 'pqr@domain.com', N'Bùi Đình Đạt', '0912345678',  '1993-09-09', 0),
--('IT03030009', 'stu@domain.com', N'Ngô Khánh Lan', '0912345679',  '1988-06-21', 1),
--('IT03030010', 'vwx@domain.com', N'Đỗ Văn Long', '0912345680',  '1994-04-14', 0),
--('IT03030011', 'yz@domain.com', N'Phan Minh Nam', '0912345681',  '1990-10-10', 0),
--('IT03030012', 'abc2@domain.com', N'Nguyễn Ngọc Bảo Nhi', '0912345682', '1985-01-20', 1),
--('IT03030013', 'def2@domain.com', N'Trần Văn Quân', '0912345683',  '1992-05-25', 0),
--('IT03030014', 'ghi2@domain.com', N'Lê Thành Kim Yến', '0912345684', '1987-08-08', 1),
--('IT03030015', 'jkl2@domain.com', N'Phạm Hữu Thịnh', '0912342685',  '1991-12-12', 0);

--INSERT INTO REQUIREMENTS (requirementsID, requirementsStatus, dateOfRequest, descriptionOfRequest, customerID) VALUES
--('RS0001', N'Yêu cầu hỗ trợ','2025-01-01',N'Gọi điện thoại trước khi đến','IT03030001'),
--('RS0002', N'Yêu cầu hỗ trợ','2025-01-01',N'Gọi điện thoại trước khi đến','IT03030002'),
--('RS0003', N'Yêu cầu hỗ trợ','2025-01-05',N'Gọi điện thoại trước khi đến','IT03030003'),
--('RS0004', N'Yêu cầu hỗ trợ','2025-01-07',N'Gọi điện thoại trước khi đến','IT03030004'),
--('RS0005', N'Yêu cầu hỗ trợ','2025-01-07',N'Gọi điện thoại trước khi đến','IT03030005'),
--('RS0006', N'Yêu cầu hỗ trợ','2025-01-09', N'Gọi điện thoại trước khi đến','IT03030006'),
--('RS0007', N'Yêu cầu hỗ trợ','2025-01-11',N'Gọi điện thoại trước khi đến','IT03030007'),
--('RS0008', N'Yêu cầu hỗ trợ','2025-01-012',N'Gọi điện thoại trước khi đến','IT03030008'),
--('RS0009', N'Yêu cầu hỗ trợ','2025-01-15',N'Gọi điện thoại trước khi đến','IT03030009'),
--('RS0010', N'Yêu cầu hỗ trợ','2025-02-01',N'Gọi điện thoại trước khi đến','IT03030010');

--INSERT INTO CONTRACTS (CONTRACTNUMBER,STARTDATE,ENDDATE,SERVICE_TYPEName,CUSTOMERID) VALUES
--('SV0001','2025-01-01','2025-09-01',N'Đầu số thoại','IT03030001'),
--('SV0002','2025-01-01','2025-09-01',N'An toàn thông tin','IT03030002'),
--('SV0003','2025-01-02','2025-09-02',N'Cloud partner','IT03030003'),
--('SV0004','2025-01-02','2025-10-02',N'Dịch vụ điện tử','IT03030004'),
--('SV0005','2025-01-04','2025-10-04',N'Dịch vụ phần mềm (SaaS)','IT03030005'),
--('SV0006','2025-01-05','2025-11-05',N'Điện toán đám mây','IT03030006'),
--('SV0007','2025-01-07','2025-12-07',N'Giám sát','IT03030007'),
--('SV0008','2025-01-09','2025-12-09',N'Trung tâm dữ liệu','IT03030008'),
--('SV0009','2025-01-15','2026-01-15',N'Thiết bị','IT03030009'),
--('SV0010','2025-01-20','2026-01-20',N'Hội nghị truyền hình','IT03030010'),
--('SV0011','2025-02-01','2026-02-01',N'Kênh truyền','IT03030011'),
--('SV0012','2025-02-01','2026-02-01',N'Tin nhắn','IT03030012'),
--('SV0013','2025-02-05','2026-02-05',N'Tổng đài','IT03030013'),
--('SV0014','2025-02-08','2026-02-08',N'Hỗ trợ CNTT','IT03030014'),
--('SV0015','2025-02-09','2026-02-09',N'Hợp đồng tích hợp/dự án','IT03030015');

--update  contracts set constatus ='Ký hoàn tất' where CONTRACTNUMBER = 'SV0001'


update Payment set PAYMENTSTATUS = 0 where id = 1 = 'SV0004'
DELETE FROM CONTRACT_STATUS_HISTORY WHERE id = 48
DELETE FROM PAYMENT_TRANSACTION WHERE PAYMENT_ID = 8

DELETE FROM CONTRACT_STATUS_HISTORY WHERE CONTRACTNUMBER = 'SV0005'
DELETE FROM CONTRACT_FILES WHERE CONTRACTNUMBER = 'SV0005' 
DELETE FROM Payment WHERE CONTRACTNUMBER = 'SV0005'  
DELETE FROM contracts WHERE CONTRACTNUMBER = 'SV0005' 

DELETE FROM CONTRACT_STATUS_HISTORY WHERE CONTRACTNUMBER = 'SV0006'
DELETE FROM CONTRACT_FILES WHERE CONTRACTNUMBER = 'SV0006' 
DELETE FROM Payment WHERE CONTRACTNUMBER = 'SV0006'  
DELETE FROM contracts WHERE CONTRACTNUMBER = 'SV0006' 

DELETE FROM CONTRACT_STATUS_HISTORY WHERE CONTRACTNUMBER = 'SV0002'
DELETE FROM CONTRACT_FILES WHERE CONTRACTNUMBER = 'SV0002' 
DELETE FROM Payment WHERE CONTRACTNUMBER = 'SV0002'  
DELETE FROM contracts WHERE CONTRACTNUMBER = 'SV0002' 

DELETE FROM CONTRACT_STATUS_HISTORY WHERE CONTRACTNUMBER = 'SV0004'
DELETE FROM CONTRACT_FILES WHERE CONTRACTNUMBER = 'SV0004' 
DELETE FROM Payment WHERE CONTRACTNUMBER = 'SV0004'  
DELETE FROM contracts WHERE CONTRACTNUMBER = 'SV0004' 

DELETE FROM CONTRACT_STATUS_HISTORY WHERE CONTRACTNUMBER = 'SV0003'
DELETE FROM CONTRACT_FILES WHERE CONTRACTNUMBER = 'SV0003' 
DELETE FROM Payment WHERE CONTRACTNUMBER = 'SV0003'  
DELETE FROM contracts WHERE CONTRACTNUMBER = 'SV0003' 

DELETE FROM CONTRACT_STATUS_HISTORY WHERE CONTRACTNUMBER = 'SV0001'
DELETE FROM CONTRACT_FILES WHERE CONTRACTNUMBER = 'SV0001' 
DELETE FROM Payment WHERE CONTRACTNUMBER = 'SV0001'  
DELETE FROM contracts WHERE CONTRACTNUMBER = 'SV0001' 

DELETE FROM Account WHERE CUSTOMERID = 'IT03030002' 
DELETE FROM company WHERE CUSTOMERID = 'IT03030002' 


select * from company 
select * from Account 
select * from contracts
select * from CONTRACT_FILES
select * from CONTRACT_STATUS_HISTORY
select * from Payment
select * from PAYMENT_TRANSACTION
select * from SERVICE_TYPE
select * from LOGINclient
select * from PASSWORDRESETTOKEN
select * from endow
select * from REGULATIONS
select * from CONTACT
select * from NOTIFICATIONS
