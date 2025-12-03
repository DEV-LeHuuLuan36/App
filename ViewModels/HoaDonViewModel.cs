using App.Models;

namespace App.ViewModels
{
    public class HoaDonViewModel
    {
        public HoaDon HoaDon { get; set; }
        public IEnumerable<ChiTietHoaDon> ChiTietHoaDon { get; set; }
    }
}   