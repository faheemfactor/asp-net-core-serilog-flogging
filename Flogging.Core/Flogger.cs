namespace Flogging.Core
{
    using Serilog;
    using Serilog.Events;
    using Serilog.Models;
    using Serilog.Sinks.MSSqlServer;
    using System;
    using System.Collections.ObjectModel;

    public static class Flogger
    {
        public static readonly ILogger _perfLogger;
        public static readonly ILogger _usageLogger;
        public static readonly ILogger _errorLogger;
        public static readonly ILogger _diagnosticLogger;

        static Flogger()
        {
            //testDb();

            var connectionObject =
              new
              {
                  server = "",
                  database = "",
                  username = "",
                  password = ""
              };
            string connectionString = $"Data Source={connectionObject.server};Initial Catalog={connectionObject.database};User Id={connectionObject.username};Password={connectionObject.password};Persist Security Info=False;";//should get from settings
            connectionString = $"Server ={connectionObject.server};Database={connectionObject.database};User Id={connectionObject.username};Password={connectionObject.password};";

            //For Logging information to File
            string directoryPath = $"{ Environment.CurrentDirectory}\\log";

            
            _perfLogger = new LoggerConfiguration()
                //.WriteTo.File(path: $"{directoryPath}\\perf.txt")
                .WriteTo.MSSqlServer
                (
                        connectionString : connectionString,
                        tableName : "PerfLogs",
                        autoCreateSqlTable : true,
                        batchPostingLimit : 1,
                        columnOptions: GetSqlColumnOptions()
                        
                 )
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                //.WriteTo.File(path: $"{directoryPath}\\usage.txt")
                .WriteTo.MSSqlServer
                (
                        connectionString: connectionString,
                        tableName: "UsageLogs",
                        autoCreateSqlTable: true,
                        columnOptions: GetSqlColumnOptions(),
                        batchPostingLimit: 1
                 )
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                //.WriteTo.File(path: $"{directoryPath}\\error.txt")
                .WriteTo.MSSqlServer
                (
                        connectionString: connectionString,
                        tableName: "ErrorLogs",
                        autoCreateSqlTable: true,
                        columnOptions: GetSqlColumnOptions(),
                        batchPostingLimit: 1
                 )
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                //.WriteTo.File(path: $"{directoryPath}\\diagnostic.txt")
                .WriteTo.MSSqlServer
                (
                        connectionString: connectionString,
                        tableName: "DiagnosticLogs",
                        autoCreateSqlTable : true,                        
                        columnOptions: GetSqlColumnOptions(),
                        batchPostingLimit: 1
                 )
                .CreateLogger();
            
        }

        public static ColumnOptions GetSqlColumnOptions()
        {
            var colOptions = new ColumnOptions();
            colOptions.Store.Remove(StandardColumn.Message);
            colOptions.Store.Remove(StandardColumn.MessageTemplate);
            colOptions.Store.Remove(StandardColumn.Level);
            colOptions.Store.Remove(StandardColumn.TimeStamp);
            colOptions.Store.Remove(StandardColumn.Exception);
            colOptions.Store.Remove(StandardColumn.Properties);

            colOptions.AdditionalDataColumns = new Collection<DataColumn>
            {
                new DataColumn {AllowDBNull=true, DataType = typeof(DateTime), ColumnName ="Timestamp" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="Product" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="Layer" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="Location" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=1000, ColumnName ="Message" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="Hostname" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="UserId" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="UserName" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=2000, ColumnName ="Exception" },
                new DataColumn {AllowDBNull=true, DataType = typeof(int), ColumnName ="ElapsedMilliseconds" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=200, ColumnName ="CorrelationId" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=2000, ColumnName ="CustomException" },
                new DataColumn {AllowDBNull=true, DataType = typeof(string), MaxLength=2000, ColumnName ="AdditionalInfo" }
            };

            return colOptions;

        }

        public static void WritePerft(FlogDetail infoToLog)
        {
            //_perfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
            _perfLogger.Write
                (
                    LogEventLevel.Information,
                    "{Timestamp}{Message}{Layer}{Location}{Product}" +
                    "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                    "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                    infoToLog.Timestamp,
                    infoToLog.Message,
                    infoToLog.Layer,
                    infoToLog.Location,
                    infoToLog.Product,
                    "",//infoToLog.CustomException
                    (infoToLog.ElapsedMilliseconds==null? 0: infoToLog.ElapsedMilliseconds),
                    infoToLog.Exception?.ToString(),//.ToBetterString()
                    infoToLog.Hostname,
                    infoToLog.UserId,
                    infoToLog.UserName,
                    infoToLog.CorrelationId,
                    infoToLog.AdditionalInfo
                 );                 
        }

        public static void WriteUsage(FlogDetail infoToLog)
        {
            //_usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
            _usageLogger.Write
                (
                    LogEventLevel.Information,
                    "{Timestamp}{Message}{Layer}{Location}{Product}" +
                    "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                    "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                    infoToLog.Timestamp,
                    infoToLog.Message,
                    infoToLog.Layer,
                    infoToLog.Location,
                    infoToLog.Product,
                    "",//infoToLog.CustomException
                    (infoToLog.ElapsedMilliseconds == null ? 0 : infoToLog.ElapsedMilliseconds),
                    infoToLog.Exception?.ToString(),//.ToBetterString()
                    infoToLog.Hostname,
                    infoToLog.UserId,
                    infoToLog.UserName,
                    infoToLog.CorrelationId,
                    infoToLog.AdditionalInfo
                 );
        }

        public static void WriteError(FlogDetail infoToLog)
        {
            //_errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
            _errorLogger.Write
                (
                    LogEventLevel.Information,
                    "{Timestamp}{Message}{Layer}{Location}{Product}" +
                    "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                    "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                    infoToLog.Timestamp,
                    infoToLog.Message,
                    infoToLog.Layer,
                    infoToLog.Location,
                    infoToLog.Product,
                    "",//infoToLog.CustomException
                    (infoToLog.ElapsedMilliseconds == null ? 0 : infoToLog.ElapsedMilliseconds),
                    infoToLog.Exception?.ToString(),//.ToBetterString()
                    infoToLog.Hostname,
                    infoToLog.UserId,
                    infoToLog.UserName,
                    infoToLog.CorrelationId,
                    infoToLog.AdditionalInfo
                 );
        }

        public static void WriteDiagnostic(FlogDetail infoToLog)
        {            
            Boolean writeDiagnostics = Convert.ToBoolean("true"); // should pick from configuraiton file
            if (!writeDiagnostics)
                return;
            //_diagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
            _diagnosticLogger.Write
                (
                    LogEventLevel.Information,
                    "{Timestamp}{Message}{Layer}{Location}{Product}" +
                    "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
                    "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
                    infoToLog.Timestamp,
                    infoToLog.Message,
                    infoToLog.Layer,
                    infoToLog.Location,
                    infoToLog.Product,
                    "",//infoToLog.CustomException
                    (infoToLog.ElapsedMilliseconds == null ? 0 : infoToLog.ElapsedMilliseconds),
                    infoToLog.Exception?.ToString(),//.ToBetterString()
                    infoToLog.Hostname,
                    infoToLog.UserId,
                    infoToLog.UserName,
                    infoToLog.CorrelationId,
                    infoToLog.AdditionalInfo
                 );
        }
        
    }
}
