using App.Models; // <- THÊM DÒNG NÀY

namespace App.ViewModels         // <- ĐỔI TÊN NAMESPACE THÀNH ViewModels
{
    public class DashboardViewModel
    {
        // Dùng cho 4 thẻ KPI (từ vw_ThongKeHoatDong)
        public VwThongKeHoatDong ThongKeHoatDong { get; set; }

        // Dùng cho Bảng Lịch hẹn (từ vw_LichHenHomNay)
        public List<VwLichHenHomNay> LichHenHomNay { get; set; }

        // Dùng cho Bảng Thuốc (từ vw_ThuocSapHetHan)
        public List<VwThuocSapHetHan> ThuocSapHetHan { get; set; }
    }
}