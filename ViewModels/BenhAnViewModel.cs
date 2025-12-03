using App.Models;

namespace App.ViewModels
{
    public class BenhAnViewModel
    {
        public BenhNhan BenhNhan { get; set; }
        public IEnumerable<HoSoBenhAn> DanhSachHoSoBenhAn { get; set; }
        public IEnumerable<DieuTri> DanhSachDieuTri { get; set; }
    }
}