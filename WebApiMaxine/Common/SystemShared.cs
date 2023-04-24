using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace WebApiMaxine.Common
{
    public static class SystemShared
    {
        /// <summary>
        /// 定義具有索引鍵/值組態屬性
        /// </summary>
        private static IConfiguration config { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static SystemShared()
        {
            //ReloadOnChange = true; //當appsettings.json被修改時重新加載            

            //TODO:2022/12/01 Added by Roger:取得開發環境變數，決定用哪一種配置檔
            //取得專案環境變數
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            config = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = $"appsettings.{env}.json", ReloadOnChange = true })
            .Build();
        }
        public static string dbName;
        public static string providerString;
        public static void Init()
        {
            //2023/01/09 Updated by Roger:統一用系統環境變數載入對應的DB連線
            dbName = config.GetValue<string>("Database:Connection:DBName")!;
        }

        /// <summary>
        /// 格式化SQL連線資訊字串
        /// </summary>
        /// <remarks>Author:Maxine 20221226 變更成AES256加密因調整連線字串</remarks>
        /// <returns>Server完整連線字串</returns>
        public static string BuildConnectionString(DatabaseConfigDetail[] details)
        {
            foreach (var item in details)
            {
                if (item.DbName.ToString() == dbName)
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.ConnectionString = item.ConnectionString;
                    builder.IntegratedSecurity = false;

                    builder.TrustServerCertificate = true;

                    // Build the SqlConnection connection string.
                    providerString = builder.ToString();

                    return providerString;
                }
            }
            return String.Empty;
        }
    }
}
