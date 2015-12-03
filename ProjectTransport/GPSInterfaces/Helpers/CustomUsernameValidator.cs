﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel;
using System.IdentityModel.Services;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace GPSInterfaces.Helpers
{
    public class CustomUsernameValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            using (var db = new GPSInterfaces.DAL.GPSContext())
            {
                bool valid = db.ValidLogin(userName, password);
                if (valid) return;
            }

            throw new SecurityTokenException("Denied");
        }
    }
}