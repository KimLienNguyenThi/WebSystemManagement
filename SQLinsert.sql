USE MANAGEMENT;

INSERT INTO COMPANY (CUSTOMERID, COMPANYNAME, TAXCODE, COMPANYACCOUNT, ACCOUNTISSUEDDATE, CPHONENUMBER, CADDRESS, IS_ACTIVE)
VALUES
('IT03030001', N'CÔNG TY TNHH GIẢI PHÁP SỐ ABC', '0101234567', 'abc.solution@example.com', '2025-01-15', '0901234567', N'123 Đường Nguyễn Huệ, Quận 1, TP.HCM', 1),
('IT03030002', N'CÔNG TY CỔ PHẦN CÔNG NGHỆ XYZ', '0209876543', 'xyz.tech@example.com', '2025-02-20', '0902345678', N'456 Đường Lê Lợi, Quận 3, TP.HCM', 1),
('IT03030003', N'DOANH NGHIỆP TƯ NHÂN THƯƠNG MẠI DEF', '0304567890', 'def.trade@example.com', '2025-03-10', '0903456789', N'789 Đường Hai Bà Trưng, Quận 1, TP.HCM', 1),
('IT03030004', N'CÔNG TY TNHH DỊCH VỤ VẬN TẢI GHI', '0401122334', 'ghi.transport@example.com', '2025-04-01', '0904567890', N'101 Đường Cộng Hòa, Quận Tân Bình, TP.HCM', 1),
('IT03030005', N'TNHH Nam Á', '012345432', 'rireland0@answers.com', '2025-05-25', '0147258369', N'Đường Lê Lợi, quận 10', 1),
('IT03030006', N'Công ty TNHH JKL', '012395432', 'contact@jkl.com', '2025-01-10', '0147258374', N'654 Đường Hai Bà Trưng, quận 2', 1),
('IT03030007', N'Công ty Cổ phần MNO', '012405432', 'info@mno.com', '2025-02-05', '0147258375', N'987 Đường Lê Văn Sỹ, quận 4', 1),
('IT03030008', N'Công ty TNHH PQR', '012415432', 'support@pqr.com', '2025-03-15', '0147258376', N'159 Đường Nguyễn Thị Minh Khai, quận 6', 1),
('IT03030009', N'Công ty Cổ phần STU', '012425432', 'sales@stu.com', '2025-04-20', '0147258377', N'753 Đường Điện Biên Phủ, quận 8', 1),
('IT03030010', N'Công ty TNHH VWX', '012435432', 'contact@vwx.com', '2025-05-25', '0147258378', N'852 Đường Phan Đình Phùng, quận 9', 1),
('IT03030011', N'Công ty Cổ phần YZA', '012445432', 'info@yza.com', NULL, '0147258379', N'951 Đường Hoàng Văn Thụ, quận 11', 0),
('IT03030012', N'Công ty TNHH BCD', '012455432', 'support@bcd.com', NULL, '0147258380', N'159 Đường Lý Chính Thắng, quận 12', 0),
('IT03030013', N'Công ty Cổ phần EFG', '012465432', 'sales@efg.com', NULL, '0147258381', N'357 Đường Nam Kỳ Khởi Nghĩa, quận Bình Thạnh', 0),
('IT03030014', N'Công ty TNHH HIJ', '012475432', 'contact@hij.com', NULL, '0147258382', N'753 Đường Võ Thị Sáu, quận Phú Nhuận', 0),
('IT03030015', N'Công ty Cổ phần KLM', '012485432', 'info@klm.com', NULL, '0147258383', N'456 Đường Nguyễn Văn Trỗi, quận Tân Bình', 0);


-- Sample Data for ACCOUNT
-- Dữ liệu mẫu cho bảng ACCOUNT đã được tạo tương ứng với các CUSTOMERID mới trong bảng COMPANY.
INSERT INTO ACCOUNT (CUSTOMERID, ROOTACCOUNT, ROOTNAME, RPHONENUMBER, DATEOFBIRTH, GENDER) VALUES
('IT03030001', 'root.abc@example.com', N'Nguyễn Văn A', '0910000001', '1980-01-01', 0),
('IT03030002', 'root.xyz@example.com', N'Trần Thị B', '0910000002', '1982-02-02', 1),
('IT03030003', 'root.def@example.com', N'Lê Minh C', '0910000003', '1985-03-03', 0),
('IT03030004', 'root.ghi@example.com', N'Phạm Thị D', '0910000004', '1988-04-04', 1),
('IT03030005', 'root.nama@example.com', N'Nguyễn Văn E', '0147258369', '1990-05-05', 0),
('IT03030006', 'root.jkl@example.com', N'Trần Thị F', '0147258374', '1983-06-06', 1),
('IT03030007', 'root.mno@example.com', N'Lê Văn G', '0147258375', '1987-07-07', 0),
('IT03030008', 'root.pqr@example.com', N'Phạm Thị H', '0147258376', '1991-08-08', 1),
('IT03030009', 'root.stu@example.com', N'Hoàng Văn I', '0147258377', '1984-09-09', 0),
('IT03030010', 'root.vwx@example.com', N'Đỗ Thị J', '0147258378', '1989-10-10', 1),
('IT03030011', 'root.yza@example.com', N'Phan Văn K', '0147258379', '1993-11-11', 0),
('IT03030012', 'root.bcd@example.com', N'Bùi Thị L', '0147258380', '1986-12-12', 1),
('IT03030013', 'root.efg@example.com', N'Trương Văn M', '0147258381', '1990-01-13', 0),
('IT03030014', 'root.hij@example.com', N'Đặng Thị N', '0147258382', '1981-02-14', 1),
('IT03030015', 'root.klm@example.com', N'Nguyễn Văn O', '0147258383', '1994-03-15', 0);


INSERT INTO LOGINCLIENT (CUSTOMERID, USERNAME, PASSWORDCLIENT) VALUES
('IT03030001', '0910000001', '$2b$12$nq4l8ufZ6g7yWoNgixN4Ae51f0da3DFRy70oMXjTO2FZ8kXGoB/Nu'),
('IT03030002', '0910000002', '$2b$12$ObgrNmxI2.G4r6/Obz/9ae0Nlz9pqbRb5iql3dhyBG54x7AnPfX/S'),
('IT03030003', '0910000003', '$2b$12$8UgU7PoympdAOocmksdeMuSEtxMPaGOXDRsXuvqvY0nmsjVEpN61W'),
('IT03030004', '0910000004', '$2b$12$F23RA1PXbfMSgLtEsW7RDePqPHk2uPSKZp47qf3IlScKHuYcqnVLC'),
('IT03030005', '0147258369', '$2b$12$u6D84OtXtDGC82AyAf1HmeFczg5NO4Pa9xM2XePQK4oFT5nEGHf8.'),
('IT03030006', '0147258374', '$2b$12$AZBaM7Z8mcydFa0gTDhQtOxFX4EloA0gCJTL0w6N0YUmkQVK1bM/6'),
('IT03030007', '0147258375', '$2b$12$f64AqKYTh1zYZ0HdvZ1WcOTKVuRYxjIJJ5nXw5zrQxHyZ2KrxfKaW'),
('IT03030008', '0147258376', '$2b$12$2CZaUeN/Bx7yLaCWDXIZ9OVlb7NDzW8TxZc2v47ErmpMcDHR7EMV.'),
('IT03030009', '0147258377', '$2b$12$ylAfZCGL8cygDrdCPlzvlObOywa/S2pUQoFL57K4UQbpRS4GVEzHe'),
('IT03030010', '0147258378', '$2b$12$5Qe1OEXQweWvWJ7f9gaxZeWnEt0SSmAp9B8lbvPV2qEMD6J8wM1eG');

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
INSERT INTO SERVICE_TYPE (SERVICE_GROUPID, SERVICE_TYPENAME,DESCRIPTION_SR) VALUES 
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

INSERT INTO CONTRACTS (CONTRACTNUMBER, STARTDATE, ENDDATE, SERVICE_TYPEID, CUSTOMERID, ORIGINAL, CUSTOMERTYPE, IS_ACTIVE, CONSTATUS, MESSAGEun)
VALUES
('SV0001', '2025-01-15', '2025-03-15', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Đầu số thoại'), 'IT03030001', NULL, 0, 0, 6, NULL),
('SV0002', '2025-02-20', '2025-04-20', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'An toàn thông tin'), 'IT03030002', NULL, 0, 0, 6, NULL),
('SV0003', '2025-03-10', '2025-05-10', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Cloud partner'), 'IT03030003', NULL, 0, 0, 6, NULL),
('SV0004', '2025-04-01', '2025-06-01', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Dịch vụ điện tử'), 'IT03030004', NULL, 0, 0, 6, NULL),
('SV0005', '2025-05-25', '2025-10-25', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Dịch vụ phần mềm (SaaS)'), 'IT03030005', NULL, 0, 1, 6, NULL),
('SV0006', '2025-01-10', '2026-01-10', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Điện toán đám mây'), 'IT03030006', NULL, 0, 1, 6, NULL),
('SV0007', '2025-02-05', '2025-07-25', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Giám sát'), 'IT03030007', NULL, 0, 1, 6, NULL),
('SV0008', '2025-03-15', '2025-05-15', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Trung tâm dữ liệu'), 'IT03030008', NULL, 0, 0, 6, NULL),
('SV0009', '2025-04-20', '2025-07-20', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Thiết bị'), 'IT03030009', NULL, 0, 1, 6, NULL),
('SV0010', '2025-05-25', '2025-07-25', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Hội nghị truyền hình'), 'IT03030010', NULL, 0, 1, 6, NULL);

--- Các công ty IS_ACTIVE = 0
INSERT INTO CONTRACTS (CONTRACTNUMBER, STARTDATE, ENDDATE, SERVICE_TYPEID, CUSTOMERID, ORIGINAL, CUSTOMERTYPE, IS_ACTIVE, CONSTATUS, MESSAGEun)
VALUES
('SV0011', '2025-06-25', '2025-07-25', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Kênh truyền'), 'IT03030011', NULL, 0, 0, 0, NULL),
('SV0012', '2025-06-10', '2025-08-10', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Tin nhắn'), 'IT03030012', NULL, 0, 0, 1, NULL),
('SV0013', '2025-06-05', '2025-09-05', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Tổng đài'), 'IT03030013', NULL, 0, 0, 2, NULL),
('SV0014', '2025-07-01', '2025-08-01', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Hỗ trợ CNTT'), 'IT03030014', NULL, 0, 0, 3, NULL),
('SV0015', '2025-07-01', '2025-08-01', (SELECT ID FROM SERVICE_TYPE WHERE SERVICE_TYPENAME = N'Hợp đồng tích hợp/dự án'), 'IT03030015', NULL, 0, 0, 4, NULL);


-- SV0001: STARTDATE = 2025-01-15
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-01-15 09:00:00', 0),
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '2025-01-15 09:05:00', 1),
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '2025-01-15 09:10:00', 2),
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '2025-01-15 09:15:00', 3),
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '2025-01-15 09:20:00', 4),
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '2025-01-15 09:25:00', 5),
('SV0001', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250115_090001.pdf', '2025-01-15 09:30:00', 6);

-- SV0002: STARTDATE = 2025-02-20
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-02-20 09:00:00', 0),
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '2025-02-20 09:05:00', 1),
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '2025-02-20 09:10:00', 2),
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '2025-02-20 09:15:00', 3),
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '2025-02-20 09:20:00', 4),
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '2025-02-20 09:25:00', 5),
('SV0002', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250220_090001.pdf', '2025-02-20 09:30:00', 6);

-- SV0003: STARTDATE = 2025-03-10
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-03-10 09:00:00', 0),
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '2025-03-10 09:05:00', 1),
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '2025-03-10 09:10:00', 2),
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '2025-03-10 09:15:00', 3),
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '2025-03-10 09:20:00', 4),
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '2025-03-10 09:25:00', 5),
('SV0003', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250310_090001.pdf', '2025-03-10 09:30:00', 6);

-- SV0004: STARTDATE = 2025-04-01
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-04-01 09:00:00', 0),
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '2025-04-01 09:05:00', 1),
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '2025-04-01 09:10:00', 2),
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '2025-04-01 09:15:00', 3),
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '2025-04-01 09:20:00', 4),
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '2025-04-01 09:25:00', 5),
('SV0004', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250401_090001.pdf', '2025-04-01 09:30:00', 6);

-- SV0005: STARTDATE = 2025-05-25
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-05-25 09:00:00', 0),
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:05:00', 1),
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:10:00', 2),
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:15:00', 3),
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:20:00', 4),
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:25:00', 5),
('SV0005', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:30:00', 6);

-- SV0006: STARTDATE = 2025-01-10
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-01-10 09:00:00', 0),
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '2025-01-10 09:05:00', 1),
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '2025-01-10 09:10:00', 2),
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '2025-01-10 09:15:00', 3),
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '2025-01-10 09:20:00', 4),
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '2025-01-10 09:25:00', 5),
('SV0006', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250110_090001.pdf', '2025-01-10 09:30:00', 6);

-- SV0007: STARTDATE = 2025-02-05
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-02-05 09:00:00', 0),
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '2025-02-05 09:05:00', 1),
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '2025-02-05 09:10:00', 2),
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '2025-02-05 09:15:00', 3),
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '2025-02-05 09:20:00', 4),
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '2025-02-05 09:25:00', 5),
('SV0007', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250205_090001.pdf', '2025-02-05 09:30:00', 6);

-- SV0008: STARTDATE = 2025-03-15
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-03-15 09:00:00', 0),
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '2025-03-15 09:05:00', 1),
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '2025-03-15 09:10:00', 2),
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '2025-03-15 09:15:00', 3),
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '2025-03-15 09:20:00', 4),
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '2025-03-15 09:25:00', 5),
('SV0008', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250315_090001.pdf', '2025-03-15 09:30:00', 6);

-- SV0009: STARTDATE = 2025-04-20
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-04-20 09:00:00', 0),
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '2025-04-20 09:05:00', 1),
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '2025-04-20 09:10:00', 2),
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '2025-04-20 09:15:00', 3),
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '2025-04-20 09:20:00', 4),
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '2025-04-20 09:25:00', 5),
('SV0009', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250420_090001.pdf', '2025-04-20 09:30:00', 6);

-- SV0010: STARTDATE = 2025-05-25
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS) VALUES
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-05-25 09:00:00', 0),
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:05:00', 1),
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:10:00', 2),
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:15:00', 3),
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:20:00', 4),
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:25:00', 5),
('SV0010', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250525_090001.pdf', '2025-05-25 09:30:00', 6);

-- SV0011: CONSTATUS = 0 → chỉ trạng thái 0
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS)
VALUES
('SV0011', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-06-25 09:00:00', 0);

-- SV0012: CONSTATUS = 1 → trạng thái 0, 1
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS)
VALUES
('SV0012', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-06-10 09:00:00', 0),
('SV0012', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250610_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250610_090001.pdf', '2025-06-10 09:05:00', 1);

-- SV0013: CONSTATUS = 2 → trạng thái 0, 1, 2
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS)
VALUES
('SV0013', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-06-05 09:00:00', 0),
('SV0013', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250605_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250605_090001.pdf', '2025-06-05 09:05:00', 1),
('SV0013', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250605_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250605_090001.pdf', '2025-06-05 09:10:00', 2);

-- SV0014: CONSTATUS = 3 → trạng thái 0 → 3
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS)
VALUES
('SV0014', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-07-01 09:00:00', 0),
('SV0014', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:05:00', 1),
('SV0014', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:10:00', 2),
('SV0014', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:15:00', 3);

-- SV0015: CONSTATUS = 4 → trạng thái 0 → 4
INSERT INTO CONTRACT_FILES (CONTRACTNUMBER, CONFILE_NAME, FILE_PATH, UPLOADED_AT, FILE_STATUS)
VALUES
('SV0015', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '/temp-pdfs/6daae6c2-80cd-4d81-b93a-04c278c1e2c3.pdf', '2025-07-01 09:00:00', 0),
('SV0015', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:05:00', 1),
('SV0015', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:10:00', 2),
('SV0015', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:15:00', 3),
('SV0015', '6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '/signed-contracts/6daae6c2-80cd-4d81-b93a-04c278c1e2c3_20250701_090001.pdf', '2025-07-01 09:20:00', 4);

-- SV0001 – root.abc@example.com – STARTDATE = 2025-01-15
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 0, 0, '2025-01-15 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 0, 1, '2025-01-15 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 1, 2, '2025-01-15 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 2, 3, '2025-01-15 09:15:00', 'root.abc@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 3, 4, '2025-01-15 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 4, 5, '2025-01-15 09:25:00', 'root.abc@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0001', 5, 6, '2025-01-15 09:30:00', 'Sta2');

-- SV0002 – root.xyz@example.com – STARTDATE = 2025-02-20
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 0, 0, '2025-02-20 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 0, 1, '2025-02-20 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 1, 2, '2025-02-20 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 2, 3, '2025-02-20 09:15:00', 'root.xyz@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 3, 4, '2025-02-20 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 4, 5, '2025-02-20 09:25:00', 'root.xyz@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0002', 5, 6, '2025-02-20 09:30:00', 'Sta2');

-- SV0003 – root.def@example.com – STARTDATE = 2025-03-10
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 0, 0, '2025-03-10 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 0, 1, '2025-03-10 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 1, 2, '2025-03-10 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 2, 3, '2025-03-10 09:15:00', 'root.def@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 3, 4, '2025-03-10 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 4, 5, '2025-03-10 09:25:00', 'root.def@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0003', 5, 6, '2025-03-10 09:30:00', 'Sta2');

-- SV0004 – root.ghi@example.com – STARTDATE = 2025-04-01
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 0, 0, '2025-04-01 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 0, 1, '2025-04-01 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 1, 2, '2025-04-01 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 2, 3, '2025-04-01 09:15:00', 'root.ghi@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 3, 4, '2025-04-01 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 4, 5, '2025-04-01 09:25:00', 'root.ghi@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0004', 5, 6, '2025-04-01 09:30:00', 'Sta2');

-- SV0005 – root.nama@example.com – STARTDATE = 2025-05-25
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 0, 0, '2025-05-25 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 0, 1, '2025-05-25 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 1, 2, '2025-05-25 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 2, 3, '2025-05-25 09:15:00', 'root.nama@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 3, 4, '2025-05-25 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 4, 5, '2025-05-25 09:25:00', 'root.nama@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0005', 5, 6, '2025-05-25 09:30:00', 'Sta2');

-- SV0006 – root.jkl@example.com – STARTDATE = 2025-01-10
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 0, 0, '2025-01-10 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 0, 1, '2025-01-10 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 1, 2, '2025-01-10 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 2, 3, '2025-01-10 09:15:00', 'root.jkl@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 3, 4, '2025-01-10 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 4, 5, '2025-01-10 09:25:00', 'root.jkl@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0006', 5, 6, '2025-01-10 09:30:00', 'Sta2');

-- SV0007 – root.mno@example.com – STARTDATE = 2025-02-05
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 0, 0, '2025-02-05 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 0, 1, '2025-02-05 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 1, 2, '2025-02-05 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 2, 3, '2025-02-05 09:15:00', 'root.mno@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 3, 4, '2025-02-05 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 4, 5, '2025-02-05 09:25:00', 'root.mno@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0007', 5, 6, '2025-02-05 09:30:00', 'Sta2');

-- SV0008 – root.pqr@example.com – STARTDATE = 2025-03-15
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 0, 0, '2025-03-15 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 0, 1, '2025-03-15 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 1, 2, '2025-03-15 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 2, 3, '2025-03-15 09:15:00', 'root.pqr@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 3, 4, '2025-03-15 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 4, 5, '2025-03-15 09:25:00', 'root.pqr@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0008', 5, 6, '2025-03-15 09:30:00', 'Sta2');

-- SV0009 – root.stu@example.com – STARTDATE = 2025-04-20
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 0, 0, '2025-04-20 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 0, 1, '2025-04-20 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 1, 2, '2025-04-20 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 2, 3, '2025-04-20 09:15:00', 'root.stu@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 3, 4, '2025-04-20 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 4, 5, '2025-04-20 09:25:00', 'root.stu@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0009', 5, 6, '2025-04-20 09:30:00', 'Sta2');

-- SV0010 – root.vwx@example.com – STARTDATE = 2025-05-25
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 0, 0, '2025-05-25 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 0, 1, '2025-05-25 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 1, 2, '2025-05-25 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 2, 3, '2025-05-25 09:15:00', 'root.vwx@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 3, 4, '2025-05-25 09:20:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 4, 5, '2025-05-25 09:25:00', 'root.vwx@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0010', 5, 6, '2025-05-25 09:30:00', 'Sta2');

INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0011', 0, 0, '2025-06-25 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0012', 0, 0, '2025-06-10 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0012', 0, 1, '2025-06-10 09:05:00', 'Sta1');

INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0013', 0, 0, '2025-06-05 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0013', 0, 1, '2025-06-05 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0013', 1, 2, '2025-06-05 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0014', 0, 0, '2025-07-01 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0014', 0, 1, '2025-07-01 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0014', 1, 2, '2025-07-01 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0014', 2, 3, '2025-07-01 09:15:00', 'root.hij@example.com');

INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0015', 0, 0, '2025-07-01 09:00:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0015', 0, 1, '2025-07-01 09:05:00', 'Sta1');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0015', 1, 2, '2025-07-01 09:10:00', 'Sta2');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0015', 2, 3, '2025-07-01 09:15:00', 'root.klm@example.com');
INSERT INTO CONTRACT_STATUS_HISTORY VALUES ('SV0015', 3, 4, '2025-07-01 09:20:00', 'Sta2');

insert into staff( STAFFID, STAFFNAME, STAFFADDRESS, STAFFDATE, STAFFEMAIL, STAFFGENDER, STAFFPHONE,DEPARTMENT)
values('Sta1', N'Nguyễn Thị Kim Liên', N'Quận 9', '2000-08-25', 'lien@gmail.com',1,'0365812848',N'Director'),
	  ('Sta2', N'Nguyễn Thành A', N'Quận 9', '2000-08-25', 'liennbtbt11@gmail.com',1,'0365812847',N'Admin');
	
INSERT INTO LOGINADMIN(STAFFID, USERNAMEAD, PASSWORDAD) VALUES ('Sta1', '0365812848','$2b$12$N1SXbDnA99tyVDL2ZbRKHeR59ou2F.O88hf9roeTrz.U4yHrswrye');
INSERT INTO LOGINADMIN(STAFFID, USERNAMEAD, PASSWORDAD) VALUES ('Sta2', '0365812847','$2a$11$WhusSTM44fBBxah.J17CIObhHKylM7PfbFW7jddYDWJ.RjFQlzT1G');


INSERT INTO REQUIREMENTS (REQUIREMENTSID, REQUIREMENTSSTATUS, DATEOFREQUEST, DESCRIPTIONOFREQUEST, CONTRACTNUMBER, SUPPORT_CODE, IsReviewed)
VALUES
('RS0001', 0, '2025-01-20', N'Gọi điện thoại trước khi đến', 'SV0001', 'SP0001', 0),
('RS0002', 0, '2025-02-25', N'Gọi điện thoại trước khi đến', 'SV0002', 'SP0001', 0),
('RS0003', 0, '2025-03-20', N'Gọi điện thoại trước khi đến', 'SV0003', 'SP0003', 0),
('RS0004', 0, '2025-04-15', N'Gọi điện thoại trước khi đến', 'SV0004', 'SP0002', 0),
('RS0005', 0, '2025-06-01', N'Gọi điện thoại trước khi đến', 'SV0005', 'SP0003', 0),
('RS0006', 0, '2025-06-10', N'Gọi điện thoại trước khi đến', 'SV0006', 'SP0001', 0),
('RS0007', 0, '2025-04-01', N'Gọi điện thoại trước khi đến', 'SV0007', 'SP0003', 0),
('RS0008', 0, '2025-04-01', N'Gọi điện thoại trước khi đến', 'SV0008', 'SP0001', 0),
('RS0009', 0, '2025-05-10', N'Gọi điện thoại trước khi đến', 'SV0009', 'SP0004', 0),
('RS0010', 0, '2025-06-15', N'Gọi điện thoại trước khi đến', 'SV0010', 'SP0001', 0);


INSERT INTO ASSIGN (REQUIREMENTSID, DEPARTMENT, STAFFID)
VALUES
('RS0001', N'Hành chính', NULL),
('RS0002', N'Hành chính', NULL),
('RS0003', N'Kỹ thuật', NULL),
('RS0004', N'Hành chính', NULL),
('RS0005', N'Kỹ thuật', NULL),
('RS0006', N'Hành chính', NULL),
('RS0007', N'Kỹ thuật', NULL),
('RS0008', N'Hành chính', NULL),
('RS0009', N'Kỹ thuật', NULL),
('RS0010', N'Hành chính', NULL);

INSERT INTO REVIEW_CRITERIA (CRITERIA_NAME) VALUES
(N'Thái độ phục vụ'),
(N'Tốc độ xử lý yêu cầu'),
(N'Chất lượng xử lý'),
(N'Mức độ hài lòng tổng thể');


insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0001','1000000');
insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0002','1500000');
insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0003','1800000');
insert into REGULATIONS (SERVICE_GROUPID,PRICE) values ('SER0004','1200000');

INSERT INTO ENDOW(ENDOWID,SERVICE_GROUPID,DISCOUNT,STARTDATE,ENDDATE)
VALUES ('ENDOW00001','SER0001',5,'2024-08-25','2026-08-25')

INSERT INTO PAYMENT (CONTRACTNUMBER, AMOUNT, PAYMENTSTATUS) VALUES
('SV0001', 3000000, 1),  -- 3 tháng × 1.000.000 (Đầu số thoại)
('SV0002', 5400000, 1),  -- 3.6 tháng × 1.800.000 (An toàn thông tin)
('SV0003', 4500000, 1),  -- 3 tháng × 1.500.000 (Cloud partner)
('SV0004', 4500000, 1),  -- 3 tháng × 1.500.000 (Dịch vụ điện tử)
('SV0005', 9000000, 1),  -- 6 tháng × 1.500.000 (Dịch vụ phần mềm (SaaS))
('SV0006', 19500000, 1), -- 13 tháng × 1.500.000 (Điện toán đám mây)
('SV0007', 10800000, 1), -- 6 tháng × 1.800.000 (Giám sát)
('SV0008', 5400000, 1),  -- 3 tháng × 1.800.000 (Trung tâm dữ liệu)
('SV0009', 7200000, 1),  -- 4 tháng × 1.800.000 (Thiết bị)
('SV0010', 3000000, 1);  -- 3 tháng × 1.000.000 (Hội nghị truyền hình)

INSERT INTO PAYMENT (CONTRACTNUMBER, AMOUNT, PAYMENTSTATUS) VALUES
('SV0011', 1000000, 0),  -- 1 tháng × 1.000.000 (Kênh truyền → SER0001)
('SV0012', 2000000, 0),  -- 2 tháng × 1.000.000 (Tin nhắn → SER0001)
('SV0013', 3000000, 0),  -- 3 tháng × 1.000.000 (Tổng đài → SER0001)
('SV0014', 1800000, 0),  -- 1 tháng × 1.800.000 (Hỗ trợ CNTT → SER0003)
('SV0015', 1200000, 0);  -- 1 tháng × 1.200.000 (Hợp đồng tích hợp/dự án → SER0004)

INSERT INTO PAYMENT_TRANSACTION 
(TRANSACTION_CODE, AMOUNT, BANK_CODE, BANK_TRANSACTION_CODE, CARD_TYPE, ORDER_INFO, PAYMENT_DATE, RESPONSE_CODE, TMN_CODE, PAYMENT_METHOD, PAYMENT_RESULT, EMAIL, PAYMENT_ID)
VALUES
('15029827', 3000000.00, N'NCB', N'VNP15029827', N'ATM', N'Thanh toan don #ORD20250621115219088', '2025-06-21 11:54:20', '00', 'GP9JOQAI', N'VNPAY', 1, N'liennbtbt11@gmail.com', 1),
('15029828', 5400000.00, N'VCB', N'VNP15029828', N'ATM', N'Thanh toan don #ORD20250622120000001', '2025-06-22 12:00:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 2),
('15029829', 4500000.00, N'ACB', N'VNP15029829', N'CREDIT', N'Thanh toan don #ORD20250623123000002', '2025-06-23 12:30:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 3),
('15029830', 4500000.00, N'TPBANK', N'VNP15029830', N'ATM', N'Thanh toan don #ORD20250624130000003', '2025-06-24 13:00:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 4),
('15029831', 9000000.00, N'BIDV', N'VNP15029831', N'CREDIT', N'Thanh toan don #ORD20250625133000004', '2025-06-25 13:30:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 5),
('15029832', 19500000.00, N'VIETINBANK', N'VNP15029832', N'ATM', N'Thanh toan don #ORD20250626140000005', '2025-06-26 14:00:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 6),
('15029833', 10800000.00, N'NCB', N'VNP15029833', N'ATM', N'Thanh toan don #ORD20250627143000006', '2025-06-27 14:30:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 7),
('15029834', 5400000.00, N'VCB', N'VNP15029834', N'CREDIT', N'Thanh toan don #ORD20250628150000007', '2025-06-28 15:00:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 8),
('15029835', 7200000.00, N'ACB', N'VNP15029835', N'ATM', N'Thanh toan don #ORD20250629153000008', '2025-06-29 15:30:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 9),
('15029836', 3000000.00, N'NCB', N'VNP15029836', N'ATM', N'Thanh toan don #ORD20250630160000009', '2025-06-30 16:00:00', '00', 'GP9JOQAI', N'VNPAY', 1, N'abc@example.com', 10);











