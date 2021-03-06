//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopGiay.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            this.CHITIETDONHANGs = new HashSet<CHITIETDONHANG>();
            this.CHITIETSPs = new HashSet<CHITIETSP>();
        }
    
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string Anh { get; set; }
        public Nullable<int> MaNhanHieu { get; set; }
        public Nullable<int> MaLoai { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public string Anh2 { get; set; }
        public string Anh3 { get; set; }
        public string MoTa { get; set; }
        public System.DateTime NgayCapNhat { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETDONHANG> CHITIETDONHANGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETSP> CHITIETSPs { get; set; }
        public virtual LOAISP LOAISP { get; set; }
        public virtual NHANHIEU NHANHIEU { get; set; }
    }
}
