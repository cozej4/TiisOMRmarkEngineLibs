//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace GIIS.DataLayer
{
    public class Helper
    {
        public static DateTime ConvertToDate(object o)
        {
            DateTime dt = new DateTime();
            System.DateTime.TryParse(o.ToString(), out dt);
            return dt;
        }

        public static int ConvertToInt(object o)
        {
            int i = 0;
            int.TryParse(o.ToString(), out i);
            return i;
        }

        public static double ConvertToDecimal(object o)
        {
            // To do: maybe is to be changed
            double d = new double();
            double.TryParse(o.ToString(), out d);
            return d;
        }

        public static bool ConvertToBoolean(object o)
        {
            bool b = false;
            bool.TryParse(o.ToString(), out b);
            return b;
        }

        /// <summary>
        /// The password must be longer than 5 chars and contain at least one uppercase, 
        /// one lowercase, one numeric and one symbolic character.
        /// </summary>
        /// <param name="password">Plain text inputed from user</param>
        /// <returns>True if all requirements are met, false otherwise.</returns>
        public static bool IsValidPassword(string password)
        {
            int minPasswordLen = 6;

            Regex regExPattern = new Regex("^.*(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$");
            return password.Length >= minPasswordLen & regExPattern.IsMatch(password);

        }

        /// <summary>
        /// Generates a hash for the given plainText and returns a base64-encoded string
        /// </summary>
        /// <param name="plainText"> Plaintext value to be hashed.</param>
        /// <returns>Hash value formatted as a base64-encoded string.</returns>
        /// <remarks></remarks>
        public static string ComputeHash(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            SHA1Managed hash = new SHA1Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            string hashValue = Convert.ToBase64String(hashBytes);

            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash value.
        /// </summary>
        /// <param name="plainText">Plain text to be verified against the specified hash.</param>
        /// <param name="hashValue"></param>
        /// <returns>If computed hash mathes the specified hash the function the return true; otherwise false.</returns>
        public static bool VerifyHash(string plainText, string hashValue)
        {
            byte[] hashBytes = Convert.FromBase64String(hashValue);
            int hashSizeInBytes = 20;

            // Make sure that the specified hash value is long enough.
            if ((hashBytes.Length < hashSizeInBytes))
            {
                return false;
            }

            string expectedHashString = ComputeHash(plainText);
            // If the computed hash matches the specified hash, the plain text value must be correct.
            return (hashValue == expectedHashString);
        }
    }
}
