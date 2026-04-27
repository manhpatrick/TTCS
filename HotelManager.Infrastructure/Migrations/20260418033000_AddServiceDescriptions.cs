using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [Service] SET [Description] = N'Dịch vụ hỗ trợ đặc biệt 24/7 cho khách hàng VIP. Đội ngũ concierge chuyên nghiệp sẵn sàng đáp ứng mọi yêu cầu của bạn bất cứ lúc nào, từ đặt phòng hàng không, xếp lịch sự kiện cho đến sắp xếp các trải nghiệm độc quyền tại địa phương.', [Unit] = N'lần' WHERE [Name] = N'Dịch vụ hỗ trợ đặc biệt';
                UPDATE [Service] SET [Description] = N'Dịch vụ cung cấp bữa trưa ngon miệng với đa dạng lựa chọn từ ẩm thực truyền thống đến hiện đại. Được chuẩn bị bởi đầu bếp hàng đầu với nguyên liệu tươi sạch, phục vụ trong không gian thoáng mát và tiện nghi của khách sạn.', [Unit] = N'suất' WHERE [Name] = N'Ăn trưa';
                UPDATE [Service] SET [Description] = N'Dịch vụ giặt quần áo nhanh chóng và chất lượng cao với công nghệ hiện đại. Chúng tôi sử dụng các chất tẩy rửa cao cấp để đảm bảo quần áo của bạn luôn sạch sẽ, mềm mại và bền màu. Dịch vụ gấp và ủi được thực hiện bởi các nhân viên giàu kinh nghiệm.', [Unit] = N'kg' WHERE [Name] = N'Giặt quần áo';
                UPDATE [Service] SET [Description] = N'Dịch vụ ủi quần áo chuyên nghiệp với trang thiết bị hiện đại. Mỗi trang phục được ủi cẩn thận để đạt kết quả hoàn hảo, từ áo sơ mi sang trọng đến váy áo tinh tế. Chúng tôi cam kết trả lại quần áo của bạn với hình dáng như mới.', [Unit] = N'lần' WHERE [Name] = N'Ủi quần áo';
                UPDATE [Service] SET [Description] = N'Dịch vụ massage toàn thân thư giãn với các chuyên gia hàng đầu. Sử dụng các kỹ thuật truyền thống và hiện đại kết hợp, massage giúp giảm căng thẳng, cải thiện tuần hoàn máu và phục hồi năng lượng cho cơ thể sau những ngày làm việc vất vả.', [Unit] = N'60 phút' WHERE [Name] = N'Massage toàn thân';
                UPDATE [Service] SET [Description] = N'Dịch vụ xông hơi sauna để thư giãn và khỏe mạnh. Hơi nước nóng giúp mở lỗ chân lông, loại bỏ độc tố và giãn cơ bắp. Phòng xông hơi của chúng tôi được thiết kế với tiêu chuẩn quốc tế, mang lại trải nghiệm spa cao cấp.', [Unit] = N'lần' WHERE [Name] = N'Xông hơi';
                UPDATE [Service] SET [Description] = N'Dịch vụ đưa đón khách tới/từ sân bay an toàn và đúng giờ. Xe được lái bởi tài xế chuyên nghiệp, trang thiết bị hiện đại, luôn sạch sẽ và thoải mái. Chúng tôi theo dõi chuyến bay của bạn để đảm bảo đến đúng giờ.', [Unit] = N'chuyến' WHERE [Name] = N'Đưa đón sân bay';
                UPDATE [Service] SET [Description] = N'Cho thuê xe máy chất lượng cao để khám phá thành phố một cách tự do. Các xe được bảo trì thường xuyên, an toàn và thích hợp cho cả người mới bắt đầu lẫn tay chơi có kinh nghiệm. Bao gồm bảo hiểm và hỗ trợ 24/7.', [Unit] = N'ngày' WHERE [Name] = N'Thuê xe máy';
                UPDATE [Service] SET [Description] = N'Dịch vụ trang trí phòng đặc biệt cho sinh nhật hoặc sự kiện đặc biệt. Đội ngũ trang trí sáng tạo sẽ biến phòng của bạn thành không gian lộng lẫy với hoa, bóng bay, nến và các trang trí độc đáo theo chủ đề của bạn.', [Unit] = N'gói' WHERE [Name] = N'Trang trí phòng sinh nhật';
                UPDATE [Service] SET [Description] = N'Dịch vụ chăm sóc trẻ em chuyên nghiệp và an toàn được thực hiện bởi nhân viên được đào tạo và kiểm tra nền tảng. Chúng tôi cung cấp môi trường an toàn, giáo dục và vui vẻ để trẻ em thoải mái trong khi bạn thư giãn.', [Unit] = N'giờ' WHERE [Name] = N'Dịch vụ trông trẻ';
                UPDATE [Service] SET [Description] = N'Cho thuê các loại xe với giá cạnh tranh và dịch vụ chuyên nghiệp. Từ xe con tinh gọn cho khám phá thành phố, đến xe 7 chỗ cho gia đình, tất cả đều được bảo dưỡng tốt. Hỗ trợ tài xế hoặc tự lái theo sở thích của bạn.', [Unit] = N'giờ' WHERE [Name] = N'Dịch vụ thuê xe';
                UPDATE [Service] SET [Description] = N'Bữa sáng buffet đầy đủ và bổ dưỡng với lựa chọn phong phú từ ẩm thực Việt đến quốc tế. Các món ăn được chuẩn bị tươi mỗi sáng, bao gồm cháo, bánh mì, trái cây, sữa chua và nước ép hoa quả tự nhiên. Bạn sẽ có một ngày tốt nhất sau bữa sáng này.' WHERE [Name] = N'Phục vụ bữa sáng';
                UPDATE [Service] SET [Description] = N'Dịch vụ giặt ủi chuyên nghiệp với công nghệ tiên tiến và nhân viên giàu kinh nghiệm. Từ giặt nhẹ nhàng cho đồ lụa mỏng manh đến ủi hoàn hảo cho các trang phục chính thức, chúng tôi đảm bảo mỗi chi tiết được chăm sóc kỹ lưỡng.' WHERE [Name] = N'Giặt ủi quần áo';
                UPDATE [Service] SET [Description] = N'Dịch vụ massage thư giãn toàn thân được thực hiện bởi các nhà massage chuyên nghiệp. Kỹ thuật massage kết hợp từ các truyền thống Đông Tây giúp giải phóng căng thẳng cơ bắp, cải thiện lưu thông máu và tâm trạng tích cực.' WHERE [Name] = N'Massage Spa';
                UPDATE [Service] SET [Description] = N'Dịch vụ cho thuê xe du lịch cao cấp với tài xế chuyên nghiệp, phục vụ toàn bộ các nhu cầu perjalanan của bạn. Xe được trang bị đầy đủ tiện nghi, điều hòa, wifi, và có sức chứa từ 4 đến 16 chỗ tùy theo nhu cầu nhóm du lịch.' WHERE [Name] = N'Thuê xe du lịch';
                UPDATE [Service] SET [Description] = N'Phục vụ trong phòng 24/7 với dịch vụ chuyên nghiệp và tận tâm. Chúng tôi cung cấp thực phẩm, đồ uống, và các tiện nghi theo yêu cầu của bạn bất kỳ lúc nào trong ngày. Chỉ cần gọi và chúng tôi sẽ có mặt ngay.' WHERE [Name] = N'Dịch vụ phòng';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [Service] SET [Description] = NULL WHERE [Name] IN (
                    N'Dịch vụ hỗ trợ đặc biệt',
                    N'Ăn trưa',
                    N'Giặt quần áo',
                    N'Ủi quần áo',
                    N'Massage toàn thân',
                    N'Xông hơi',
                    N'Đưa đón sân bay',
                    N'Thuê xe máy',
                    N'Trang trí phòng sinh nhật',
                    N'Dịch vụ trông trẻ',
                    N'Dịch vụ thuê xe',
                    N'Phục vụ bữa sáng',
                    N'Giặt ủi quần áo',
                    N'Massage Spa',
                    N'Thuê xe du lịch',
                    N'Dịch vụ phòng'
                );
            ");
        }
    }
}
