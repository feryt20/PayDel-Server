using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Api
{
    public enum BankEnums
    {
        [Display(Name = "سامان")]
        Saman = 1,
        [Display(Name = "ملت")]
        Mellat = 2,
        [Display(Name = "پارسیان")]
        Persian = 3,
        [Display(Name = "پاسارگاد")]
        Pasargad = 4,
        [Display(Name = "ایران کیش")]
        IranKish = 5,
        [Display(Name = "ملی")]
        Melli = 6,
        [Display(Name = "آسان پرداخت")]
        AsanPardakht = 7,
        [Display(Name = "زرین پال")]
        ZarinPal = 8,
        [Display(Name = "مجازی")]
        Virtual = 9
    }
}
