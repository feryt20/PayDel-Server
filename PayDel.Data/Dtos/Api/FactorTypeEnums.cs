﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Api
{
    public enum FactorTypeEnums
    {
        [Display(Name = "فاکتور")]
        Factor = 1,
        [Display(Name = "ایزی پی")]
        EasyPay = 2,
        [Display(Name = "حمایتی")]
        Support = 3,
        [Display(Name = "افزایش موجودی")]
        IncInventory = 4
    }
}
