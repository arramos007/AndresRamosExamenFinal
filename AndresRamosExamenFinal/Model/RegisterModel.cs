using System;
using System.Collections.Generic;
using System.Text;

namespace AndresRamosExamenFinal.Model
{
    public class RegisterModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string password_confirmation { get; set; }
    }
}
