

namespace FloggingConsole
{
    using System;
    using Flogging.Core;
    using System.Diagnostics;

    class Program
    {
        
        
        static void Main(string[] args)
        {

            //Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            Serilog.Debugging.SelfLog.Enable(Console.Error);
            //TestDatabaseConnection();

            var fd = GetFlogDetail("string applciation", null);
            Flogger.WriteDiagnostic(fd);

            var tracker = new PerfTracker 
                (
                   name : "FloggerConsole_Execution",
                   userId : "",
                   userName : fd.UserName,
                   location: fd.Location,
                   product: fd.Product,
                   layer:fd.Layer
                 );

            try
            {
                var ex = new Exception("Something bad has happend!");
                ex.Data.Add("input Param", "nothing to see here");
                throw ex;
            }
            catch (Exception ex)
            {
                fd = GetFlogDetail("", ex);
                Flogger.WriteError(fd);
            }

            fd = GetFlogDetail("used flogging console", null);
            Flogger.WriteUsage(fd);

            fd = GetFlogDetail("stopping app", null);
            Flogger.WriteDiagnostic(fd);

            tracker.Stop();

            Console.ReadLine();
        }

        private static FlogDetail GetFlogDetail(string message, Exception ex)
        {
            return new FlogDetail
            {
                Product = "Flogger",
                Location = "FloggerConsole",
                Layer = "Job",
                UserName = Environment.UserName,
                Hostname = Environment.MachineName,
                Message = message,
                Exception = ex                
            };
        }

        #region Utility Functions
        static void TestDatabaseConnection()
        {
            // this will crate a table named testing on the server
            var connectionObject =
                new
                {
                    server = "",
                    database = "",
                    username = "",
                    password = ""
                };
            string connectionString = $"Data Source={connectionObject.server};Initial Catalog={connectionObject.database};User Id={connectionObject.username};Password={connectionObject.password};Persist Security Info=False;";//should get from settings

            try
            {
                var databaseConnection = new System.Data.SqlClient.SqlConnection(connectionString);
                string sqlCommandText = "CREATE TABLE testDatabaseConnection ( c1 int) ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sqlCommandText, databaseConnection);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {

            }
        }

        #endregion

    }
}
