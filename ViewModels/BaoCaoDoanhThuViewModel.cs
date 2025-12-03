using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class BaoCaoDoanhThuResult
    {
        public string MaHoaDon { get; set; }
        public string HoTenBenhNhan { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}