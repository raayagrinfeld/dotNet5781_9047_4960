﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class User
    {
        public int Salt { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ManagementPermission { get; set; }
        public bool IsActive { get; set; }
        public gender Gender { get; set; }
        public string imagePath { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
