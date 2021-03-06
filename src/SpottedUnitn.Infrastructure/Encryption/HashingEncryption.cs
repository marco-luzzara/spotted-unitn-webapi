﻿using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Encryption
{
    public class HashingEncryption
    {
        private const int DEFAULT_BCRYPT_COST = 12;

        public static string EncryptWithBCrypt(string s, int cost = DEFAULT_BCRYPT_COST)
        {
            var pwd = BCrypt.Net.BCrypt.HashPassword(s, cost);
            return pwd;
        }

        public static bool VerifyWithBCrypt(string pwd, string hashedPwd)
        {
            var pwdCheck = BCrypt.Net.BCrypt.Verify(pwd, hashedPwd);
            return pwdCheck;
        }
    }
}
