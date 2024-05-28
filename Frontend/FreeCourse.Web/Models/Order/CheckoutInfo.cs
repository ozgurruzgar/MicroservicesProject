using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Order
{
    public class CheckoutInfo
    {
        [Display(Name = "İl")]
        public string Province { get; set; }
        [Display(Name = "İlçe")]
        public string District { get; set; }
        [Display(Name = "Cadde")]
        public string Street { get; set; }
        [Display(Name = "Posta kodu")]
        public string ZipCode { get; set; }
        [Display(Name = "Adres")]
        public string Line{ get; set; }
        [Display(Name = "Kart isim ve soyisim")]
        public string CardName{ get; set; }
        [Display(Name = "Kart numarası")]
        public string CardNumber { get; set; }
        [Display(Name = "Son kullanma tarihi")]
        public string Expiration { get; set; }
        [Display(Name = "CVV/CVC2 numarası")]
        public string CVV { get; set; }
        [Display(Name = "Toplam fiyat")]
        public decimal TotalPrice { get; set; }
    }
}
