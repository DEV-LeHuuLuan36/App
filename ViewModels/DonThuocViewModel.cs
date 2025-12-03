using App.Models;

namespace App.ViewModels
{
    public class DonThuocViewModel
    {
        public DieuTri DieuTri { get; set; }
        public IEnumerable<DonThuoc> DanhSachDonThuoc { get; set; }
    }
}