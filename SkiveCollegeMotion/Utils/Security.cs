using Microsoft.AspNetCore.Identity.UI.Services;
using SkiveCollegeMotion.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace SkiveCollegeMotion.Utils
{
    public static class Security
    {
        private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();

        private static int getRandomInt(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

            long diff = (long)maxExclusiveValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            // Combining loop and modulo so that modulo is used if applicable without bias and if not it loops instead.
            // This ensures optimal performance without modulo bias.
            do
            {
                byte[] randomBytes = new byte[sizeof(uint)];
                csp.GetBytes(randomBytes);
                ui = BitConverter.ToUInt32(randomBytes, 0);
            } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }
        
        public static string generatePassword()
        {
            int length = 8;
            // Create 2D array with info on the different types of wanted characters with following format:
            // n = type number.
            // types[n, 0] = start index(inclusive) of wanted char range.
            // types[n, 1] = end index(exclusive) of wanted char range.
            int[,] types = new int[,]
            {
                { 97, 123 }, // Lowercase a-z
                { 65, 91 }, // Uppercase A-Z
                { 48, 58 }, // Digits 0-9
            };

            if (length < types.Length)
                throw new ArgumentOutOfRangeException("length must be big enough for at least 1 of each type");

            // Generate password while ensuring enough space for potentially missed types.
            bool[] usedTypes = Enumerable.Repeat(false, types.GetLength(0)).ToArray();
            int remainingTypes = types.GetLength(0);
            string password = "";
            while (password.Length < length - remainingTypes)
            {
                int type = getRandomInt(0, types.GetLength(0));
                usedTypes[type] = true;
                password += (char)getRandomInt(types[type, 0], types[type, 1]);
                // Count remaining types.
                remainingTypes = usedTypes.Aggregate(0, (total, next) => {
                    if (!next)
                    {
                        total++;
                    }
                    return total;
                });
            }

            for (int i = 0; i < usedTypes.Length; i++)
            {
                if (!usedTypes[i])
                {
                    // Add 1 as max is an exclusive function but inclusivity is wanted here.
                    int index = getRandomInt(0, password.Length + 1);
                    char character = (char)getRandomInt(types[i, 0], types[i, 1]);
                    password = password.Insert(index, character.ToString());
                }
            }
            return password;
        }
    }
}