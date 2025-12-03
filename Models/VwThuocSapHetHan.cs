using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwThuocSapHetHan
    {
        public string TenThuoc { get; set; } = null!;
        public int? SoLuongTon { get; set; }
        public DateTime? HanSuDung { get; set; }
        public int? SoNgayConLai { get; set; }
    }
}
