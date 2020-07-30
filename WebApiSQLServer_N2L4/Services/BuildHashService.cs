using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebApiSQLServer_N2L4.Models;

namespace WebApiSQLServer_N2L4.Services
{
    public class BuildHashService
    {
        public HashResult BuildHash(string input)
        {
            var salt = Crypto.GenerateSalt(32);
            var saltStrg = Encoding.ASCII.GetBytes(salt);
            var hash = KeyDerivation.Pbkdf2(
              password: input,
              salt: saltStrg,
              prf: KeyDerivationPrf.HMACSHA1,
              iterationCount: 10000,
              numBytesRequested: 64);

            string hashed = Convert.ToBase64String(hash);

            return new HashResult()
            {
                Input = input,
                Hash = hashed,
                Salt = salt
            };
        }

        public bool VerifyHash(HashResult password)
        {
            var saltStrg = Encoding.ASCII.GetBytes(password.Salt);

            var hash = KeyDerivation.Pbkdf2(
              password: password.Input,
              salt: saltStrg,
              prf: KeyDerivationPrf.HMACSHA1,
              iterationCount: 10000,
              numBytesRequested: 64);

            string hashed = Convert.ToBase64String(hash);

            if (password.Hash == hashed) { return true; }

            return false;
        }
    }
}
