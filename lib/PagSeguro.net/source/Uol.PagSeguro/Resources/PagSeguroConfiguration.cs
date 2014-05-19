// Copyright [2011] [PagSeguro Internet Ltda.]
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Configuration;
using Uol.PagSeguro.Domain;

namespace Uol.PagSeguro.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public static class PagSeguroConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public static AccountCredentials Credentials
        {
            get
            {
                var email = ConfigurationManager.AppSettings["pagseguro-email"];
                var token = ConfigurationManager.AppSettings["pagseguro-token"];

                return new AccountCredentials(email, token);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ModuleVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string CmsVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string LanguageEngineDescription
        {
            get
            {
                return Environment.Version.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Uri NotificationUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["pagseguro-notificationurl"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Uri PaymentUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["pagseguro-paymenturl"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Uri PaymentRedirectUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["pagseguro-paymentoredirecturl"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Uri SearchUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["pagseguro-searchurl"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int RequestTimeout
        {
            get 
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["pagseguro-timeout"] ?? "10000");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FormUrlEncoded
        {
            get
            {
                return "application/x-www-form-urlencoded";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string Encoding
        {
            get
            {
                return "ISO-8859-1";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string LibVersion
        {
            get
            {
                return "2.0.4c";
            }
        }
    }
}
