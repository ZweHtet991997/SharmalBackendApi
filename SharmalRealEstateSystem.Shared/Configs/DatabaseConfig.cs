using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Shared.Configs
{
    public static class DatabaseConfig
    {
        //UAT Database Configuration
        private static string UATServer = "SQL5113.site4now.net";
        private static string UATDatabase = "db_a9a6b3_sharmaluat";
        private static string UATUserId = "db_a9a6b3_sharmaluat_admin";
        private static string UATPassword = "NKsoftwarehouse*11";

        //PROD Database Configuration
        private static string Server = "SQL8006.site4now.net";
        private static string Database = "db_a9a6b3_sharmal";
        private static string UserId = "db_a9a6b3_sharmal_admin";
        private static string Password = "NKsoftwarehouse*11";

        public static string UATDbConnectionString()
        {
            return $"Data Source={UATServer};Initial Catalog={UATDatabase};User Id={UATUserId};Password={UATPassword};TrustServerCertificate=True";
        }

        public static string ProdDbConnectionString()
        {
            return $"Data Source={Server};Initial Catalog={Database};User Id={UserId};Password={Password};TrustServerCertificate=True";
        }
    }
}
