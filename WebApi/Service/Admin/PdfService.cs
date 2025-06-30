using WebApi.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using iText.Kernel.Pdf;
using iText.Signatures;
using iText.Kernel.Geom;
using Org.BouncyCastle.Pkcs;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;
using iText.Forms;
using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Globalization;
using System.Text;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;
using ImgSharp = SixLabors.ImageSharp;
using ImgProc = SixLabors.ImageSharp.Processing;
using ImgPixel = SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using System.Text.RegularExpressions;
using iText.IO.Font;


namespace WebApi.Service.Admin
{
    public class PdfService
    {
        public byte[] GenerateContractPdf(CompanyAccountDTO dto)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    page.Header().Element(header =>
                    {
                        header.Column(col =>
                        {
                            col.Item().AlignCenter().Text("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM").Bold().FontSize(13);
                            col.Item().AlignCenter().Text("Độc lập - Tự do - Hạnh phúc").Italic().FontSize(12);
                            col.Item().AlignCenter().Text("─────────────").FontSize(12);
                            col.Item().PaddingTop(10).AlignCenter().Text("HỢP ĐỒNG CUNG CẤP DỊCH VỤ").Bold().FontSize(16);
                            col.Item().AlignCenter().Text($"Số: HD-{dto.CustomerId}/{DateTime.Now:yyyy}").FontSize(12);
                        });
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Text("Căn cứ vào nhu cầu và khả năng của hai bên, chúng tôi cùng thỏa thuận ký kết hợp đồng cung cấp dịch vụ với các điều khoản sau đây:").Italic();

                        // Thông tin Bên A
                        column.Item().Text("🔹 BÊN A: BÊN SỬ DỤNG DỊCH VỤ").Bold();
                        column.Item().Text($"- Tên công ty: {dto.CompanyName}");
                        column.Item().Text($"- Mã số thuế: {dto.TaxCode}");
                        column.Item().Text($"- Email: {dto.CompanyAccount}");
                        column.Item().Text($"- Số điện thoại: {dto.CPhoneNumber}");
                        column.Item().Text($"- Địa chỉ: {dto.CAddress}");

                        // Đại diện bên A
                        column.Item().Text("🔹 Người đại diện").Bold();
                        column.Item().Text($"- Họ tên: {dto.RootName}");
                        column.Item().Text($"- Email: {dto.RootAccount}");
                        column.Item().Text($"- Số điện thoại: {dto.RPhoneNumber}");

                        // Dịch vụ
                        column.Item().Text("🔹 DỊCH VỤ").Bold();
                        column.Item().Text($"- Phân loại: {(dto.CustomerType == true ? "VIP" : "Bình thường")}");
                        column.Item().Text($"- Loại dịch vụ: {dto.ServiceType}");
                        column.Item().Text($"- Thời hạn: Từ {dto.Startdate:dd/MM/yyyy} đến {dto.Enddate:dd/MM/yyyy}");
                        column.Item().Text($"- Giá: {dto.Amount:N0} VND");

                        column.Item().PaddingTop(10).Text("🔹 BÊN B: BÊN CUNG CẤP DỊCH VỤ").Bold();
                        column.Item().Text($"- Tên đơn vị: Công ty TNHH Dịch Vụ Viễn thông");
                        column.Item().Text($"- Mã số thuế: 0123456789");
                        column.Item().Text($"- Địa chỉ: 123 Đường ABC, Quận 1, TP.HCM");
                        column.Item().Text($"- Số điện thoại: (028) 1234 5678");
                        column.Item().Text($"- Email: lienhe@abcservice.vn");
                        column.Item().Text($"- Đại diện: Nguyễn Văn B – Giám đốc");

                        // Điều khoản hợp đồng
                        column.Item().PaddingTop(10).Text("🔹 ĐIỀU KHOẢN HỢP ĐỒNG").Bold().FontSize(13);
                        column.Item().Text("1. Bên B sẽ cung cấp dịch vụ như đã nêu cho Bên A theo điều kiện hai bên thống nhất.");
                        column.Item().Text("2. Phạm vi cung cấp dịch vụ:");
                        column.Item().Text("   - Bao gồm nội dung, thông số kỹ thuật theo mô tả hoặc yêu cầu.");
                        column.Item().Text("   - Điều chỉnh sẽ được thống nhất qua văn bản.");
                        column.Item().Text("3. Hiệu lực hợp đồng:");
                        column.Item().Text($"   - Từ {dto.Startdate:dd/MM/yyyy} đến {dto.Enddate:dd/MM/yyyy}, gia hạn nếu có thỏa thuận trước 15 ngày.");
                        column.Item().Text("4. Giá trị và thanh toán:");
                        column.Item().Text($"   - Giá trị hợp đồng: {dto.Amount:N0} VND.");
                        column.Item().Text("   - Thanh toán theo phụ lục/hóa đơn.");
                        column.Item().Text("5. Hỗ trợ kỹ thuật:");
                        column.Item().Text("   - Gồm xử lý sự cố, cước phí, bảo hành (nếu có), cập nhật dịch vụ.");
                        column.Item().Text("6. Trách nhiệm và bảo mật:");
                        column.Item().Text("   - Cung cấp đúng cam kết, phối hợp triển khai và bảo mật thông tin.");
                        column.Item().Text("7. Giải quyết tranh chấp:");
                        column.Item().Text("   - Thỏa thuận trước, nếu không được thì đưa ra Tòa án có thẩm quyền.");

                        column.Item().PaddingTop(15).Text($"Ngày lập hợp đồng: {DateTime.Now:dd/MM/yyyy}").AlignRight().Italic();

                        // Ký kết
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().AlignCenter().Text("ĐẠI DIỆN BÊN A").Bold();
                                col.Item().AlignCenter().Text("##SIGN_HERE_BENA##")
                                    .FontSize(1)
                                    .FontColor(Colors.White);
                            });
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().AlignCenter().Text("ĐẠI DIỆN BÊN B").Bold();
                                // Dòng từ khóa ẩn để định vị vị trí ký
                                col.Item().AlignCenter().Text("##SIGN_HERE_BENB##")
                                    .FontSize(1) // nhỏ nhất có thể
                                    .FontColor(Colors.White); // hoặc .Opacity(0.01f)
                            });

                        });

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => c.Border(1).Height(100).Padding(5));
                            row.RelativeItem().Element(c => c.Border(1).Height(100).Padding(5));
                        });
                    });

                    page.Footer().AlignCenter().Text("Hợp đồng được tạo bởi hệ thống quản lý").FontSize(10);
                });
            });

            return document.GeneratePdf();
        }


        private (Rectangle rect, int page) FindTextPosition(byte[] pdfBytes, string keyword)
        {
            using var pdfReader = new PdfReader(new MemoryStream(pdfBytes));
            using var pdfDoc = new PdfDocument(pdfReader);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var strategy = new TextLocationStrategy(keyword);
                var processor = new PdfCanvasProcessor(strategy);
                processor.ProcessPageContent(pdfDoc.GetPage(i));

                if (strategy.Locations.Any())
                {
                    return (strategy.Locations.First(), i);
                }
            }

            throw new Exception($"Không tìm thấy từ khóa '{keyword}' trong PDF.");
        }

        //client ký
        public byte[] SignPdfWithClientCertificate(byte[] originalPdfBytes, Stream clientPfxStream, string pfxPassword, string clientName)
        {
            Pkcs12Store store = new Pkcs12StoreBuilder().Build();

            try
            {
                store.Load(clientPfxStream, pfxPassword.ToCharArray());
            }
            catch (Exception ex)
            {
                throw new Exception("Mật khẩu chứng thư số không chính xác hoặc file không hợp lệ.");
            }

            string alias = store.Aliases.Cast<string>().FirstOrDefault(store.IsKeyEntry);
            if (alias == null)
                throw new Exception("Không tìm thấy khóa bí mật trong file chứng thư số.");

            // Lấy khóa và chuỗi chứng chỉ
            AsymmetricKeyParameter privateKey = store.GetKey(alias).Key;

            var certChain = store.GetCertificateChain(alias);
            var chain = certChain
                .Select(c => new X509CertificateBC(c.Certificate))
                .Cast<IX509Certificate>()
                .ToList();

            var iPrivateKey = new PrivateKeyBC(privateKey);

            var cert = certChain[0].Certificate;
            DateTime now = DateTime.Now;

            if (now < cert.NotBefore.ToLocalTime() || now > cert.NotAfter.ToLocalTime())
            {
                throw new Exception("Chứng thư số đã hết hạn hoặc chưa có hiệu lực.");
            }

            string signerName = cert.SubjectDN.ToString();
            DateTime notBefore = cert.NotBefore.ToLocalTime();
            DateTime notAfter = cert.NotAfter.ToLocalTime();

            // Ký PDF
            using var signedPdfStream = new MemoryStream();
            using var originalPdfStream = new MemoryStream(originalPdfBytes);
            var reader = new PdfReader(originalPdfStream);
            var signer = new PdfSigner(reader, signedPdfStream, new StampingProperties());


            //// Tìm vị trí chữ ký theo từ khóa
            //(string keyword, float offsetY) = ("Đại diện Bên A", 113f);
            //var (textRect, page) = FindTextPosition2(originalPdfBytes, keyword);

            //float offsetX = 30f;
            //float signatureWidth = 200f;
            //float signatureHeight = 70f;

            //Rectangle rect = new Rectangle(
            //    textRect.GetX() + offsetX,
            //    textRect.GetY() - offsetY,
            //    signatureWidth,
            //    signatureHeight
            //);
            // Tìm vị trí ký dựa trên từ khóa ẩn
            var (textRect, page) = FindTextPosition2(originalPdfBytes, "##SIGN_HERE_BENA##");

            float signatureWidth = 200f;
            float signatureHeight = 70f;

            Rectangle rect = new Rectangle(
                textRect.GetX(),
                textRect.GetY(),
                signatureWidth,
                signatureHeight
            );

            // Font chữ Unicode
            var fontPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "tahoma.ttf");
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            string layerText = $"Ký bởi: {clientName}\nNgười dùng: {signerName}\nHiệu lực: {notBefore:dd/MM/yyyy} - {notAfter:dd/MM/yyyy}\nNgày ký: {DateTime.Now:dd/MM/yyyy}";

            var appearance = signer.GetSignatureAppearance();
            appearance
                .SetPageRect(rect)
                .SetPageNumber(page)
                .SetLocation("Client Upload")
                .SetLayer2Font(font)
                .SetLayer2FontSize(9f)
                .SetReason("Ký bởi Khách hàng")
                .SetLayer2Text(layerText)
                .SetRenderingMode(PdfSignatureAppearance.RenderingMode.DESCRIPTION);

            signer.SetFieldName("SignatureClient");

            IExternalSignature externalSignature = new PrivateKeySignature(iPrivateKey, DigestAlgorithms.SHA256);
            IExternalDigest digest = new BouncyCastleDigest();

            signer.SignDetached(
                digest, externalSignature,
                chain.ToArray(), null, null, null,
                0, PdfSigner.CryptoStandard.CADES
            );

            return signedPdfStream.ToArray();
        }
        private (Rectangle rect, int page) FindTextPosition2(byte[] pdfBytes, string keywordPart)
        {
            using var pdfReader = new PdfReader(new MemoryStream(pdfBytes));
            using var pdfDoc = new PdfDocument(pdfReader);

            string normalizedKeyword = NormalizeText(keywordPart);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                Console.WriteLine($"📄 Đang xử lý trang {i}");

                var page = pdfDoc.GetPage(i);
                var strategy = new GeneralTextLocationStrategy();

                var processor = new PdfCanvasProcessor(strategy);
                processor.ProcessPageContent(page);

                var locations = strategy.Locations;
                if (locations == null || locations.Count == 0)
                {
                    Console.WriteLine($"Không tìm thấy đoạn text nào ở trang {i}");
                    continue;
                }

                for (int j = 0; j < locations.Count; j++)
                {
                    string combinedText = "";
                    Rectangle? firstRect = null;

                    // Ghép tối đa 3 đoạn liên tiếp lại để tìm
                    for (int k = j; k < Math.Min(j + 3, locations.Count); k++)
                    {
                        if (string.IsNullOrWhiteSpace(locations[k].Text)) continue;

                        combinedText += locations[k].Text;
                        if (firstRect == null)
                            firstRect = locations[k].Rect;

                        string normalizedCombined = NormalizeText(combinedText);
                        Console.WriteLine($"Ghép đoạn: '{combinedText}' Normalized: '{normalizedCombined}'");

                        if (normalizedCombined.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Tìm thấy từ khóa '{keywordPart}' tại trang {i}, vị trí x={firstRect.GetX()}, y={firstRect.GetY()}");
                            return (firstRect, i);
                        }
                    }
                }
            }

            throw new Exception($"❌ Không tìm thấy từ khóa gần đúng '{keywordPart}' trong PDF.");
        }


        // Helper để normalize text
        private string NormalizeText(string text)
        {
            return string.Concat(text.Where(c => !char.IsWhiteSpace(c)))
                 .ToLowerInvariant();
        }

        
        public byte[] InsertSignatureImageToPdf(byte[] originalPdfBytes, string signatureBase64, string keyword, float offsetX = 30f, float offsetY = 113f, float width = 200f, float height = 70f)
        {
            using var pdfStream = new MemoryStream(originalPdfBytes);
            using var outputPdfStream = new MemoryStream();

            // Decode base64 image
            var base64Data = Regex.Replace(signatureBase64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
            byte[] imageBytes = Convert.FromBase64String(base64Data);

            PdfReader reader = new PdfReader(pdfStream);
            PdfWriter writer = new PdfWriter(outputPdfStream);
            PdfDocument pdfDoc = new PdfDocument(reader, writer);
            iText.Layout.Document doc = new iText.Layout.Document(pdfDoc);

            // Tìm vị trí từ khóa để chèn ảnh
            (Rectangle textRect, int pageNum) = FindTextPosition2(originalPdfBytes, keyword); 
            float x = textRect.GetX() + offsetX;
            float y = textRect.GetY() - offsetY;

            // Tạo hình ảnh iText7
            iText.Layout.Element.Image signImg = new iText.Layout.Element.Image(ImageDataFactory.Create(imageBytes));
            signImg.SetFixedPosition(pageNum, x, y);
            signImg.SetWidth(width);
            signImg.SetHeight(height);

            // Chèn ảnh lên PDF
            doc.Add(signImg);

            doc.Close();
            return outputPdfStream.ToArray();
        }

        //boss ký
        public byte[] SignPdfWithAdminCertificate(byte[] originalPdfBytes, Stream clientPfxStream, string pfxPassword, string staffId)
        {
            // Load PFX từ client
            Pkcs12Store store = new Pkcs12StoreBuilder().Build();

            try
            {
                store.Load(clientPfxStream, pfxPassword.ToCharArray());
            }
            catch (Exception ex)
            {
                throw new Exception("Mật khẩu chứng thư số không chính xác hoặc file không hợp lệ.");
            }

            string alias = store.Aliases.Cast<string>().FirstOrDefault(store.IsKeyEntry);
            if (alias == null)
                throw new Exception("Không tìm thấy khóa bí mật trong file chứng thư số.");

            // Lấy khóa và chuỗi chứng chỉ
            AsymmetricKeyParameter privateKey = store.GetKey(alias).Key;

            var certChain = store.GetCertificateChain(alias);
            var chain = certChain
                .Select(c => new X509CertificateBC(c.Certificate))
                .Cast<IX509Certificate>()
                .ToList();

            var iPrivateKey = new PrivateKeyBC(privateKey);

            var cert = certChain[0].Certificate;
            DateTime now = DateTime.Now;

            if (now < cert.NotBefore.ToLocalTime() || now > cert.NotAfter.ToLocalTime())
            {
                throw new Exception("Chứng thư số đã hết hạn hoặc chưa có hiệu lực.");
            }

            string signerName = cert.SubjectDN.ToString();
            DateTime notBefore = cert.NotBefore.ToLocalTime();
            DateTime notAfter = cert.NotAfter.ToLocalTime();

            // Ký PDF
            using var signedPdfStream = new MemoryStream();
            using var originalPdfStream = new MemoryStream(originalPdfBytes);
            var reader = new PdfReader(originalPdfStream);
            var signer = new PdfSigner(reader, signedPdfStream, new StampingProperties());

            //// Tìm vị trí ký
            //(string keyword, float offsetY) = ("##SIGN_HERE_BENB##", 80f);
            //var (textRect, page) = FindTextPosition(originalPdfBytes, keyword);

            //float offsetX = -40f;
            //float signatureWidth = 200f;
            //float signatureHeight = 70f;

            //Rectangle rect = new Rectangle(
            //    textRect.GetX() + offsetX,
            //    textRect.GetY() - offsetY,
            //    signatureWidth,
            //    signatureHeight
            //);
            // Tìm vị trí ký

            (string keyword, float offsetY) = ("##SIGN_HERE_BENB##", 80f);
            var (textRect, page) = FindTextPosition(originalPdfBytes, keyword);

            float offsetX = -40f;
            float signatureWidth = 200f;
            float signatureHeight = 70f;

            Rectangle rect = new Rectangle(
                textRect.GetX() + offsetX,
                textRect.GetY() - offsetY,
                signatureWidth,
                signatureHeight
            );

            // Font chữ Unicode
            var fontPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "tahoma.ttf");
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            string layerText = $"Ký bởi: {staffId}\nNgười dùng: {signerName}\nHiệu lực: {notBefore:dd/MM/yyyy} - {notAfter:dd/MM/yyyy}\nNgày ký: {DateTime.Now:dd/MM/yyyy}";

            var appearance = signer.GetSignatureAppearance();
            appearance
                .SetPageRect(rect)
                .SetPageNumber(page)
                .SetLocation("Hệ thống")
                .SetLayer2Font(font)
                .SetLayer2FontSize(8)
                .SetReason("Ký bởi Admin")
                .SetLayer2Text(layerText)
                .SetRenderingMode(PdfSignatureAppearance.RenderingMode.DESCRIPTION);

            signer.SetFieldName("Signature2");

            IExternalSignature externalSignature = new PrivateKeySignature(iPrivateKey, DigestAlgorithms.SHA256);
            IExternalDigest digest = new BouncyCastleDigest();

            signer.SignDetached(
                digest, externalSignature,
                chain.ToArray(), null, null, null,
                0, PdfSigner.CryptoStandard.CADES
            );

            return signedPdfStream.ToArray();
        }

    }
}
