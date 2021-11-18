//using Microsoft.Extensions.Logging;

using NReco.Csv;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;


namespace Fyk.C19.C19DataLoader
{
    internal class Transfer
    {
        protected SqlConnection sqlConnection;
        protected IC19Logger logger;

        public Transfer(SqlConnection connection, IC19Logger logger)
        {
            sqlConnection = connection;
            this.logger = logger;
        }
        protected CsvReader GetCSV(string url, string columnSeparator, string encoding)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr;
            //https://docs.microsoft.com/en-us/dotnet/api/system.text.encodinginfo.getencoding?view=net-5.0
            if (!string.IsNullOrEmpty(encoding))
                sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.GetEncoding(encoding));
            else
                sr = new StreamReader(resp.GetResponseStream());


            return new CsvReader(sr, columnSeparator);
        }
        public void RunTransfer(string tableName, string sourceUrl, string encoding, int batchSize = 1000, string columnSeparator = ",", string decimalSeparator = ".")
        {
            LogInfo($"Transfer from {sourceUrl} to {tableName}");
            try
            {
                WriteData(tableName, sourceUrl, encoding, batchSize, columnSeparator, decimalSeparator);
            }
            catch (SqlException sex)
            {
                LogError(sex.Message);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

        }
        protected void WriteData(string tableName, string sourceUrl, string encoding, int batchSize, string columnSeparator, string decimalSeparator)
        {
            var data = new DataTable(tableName);
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = sqlConnection;
                cmd.CommandText = $"TRUNCATE TABLE {data.TableName};";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                logger.LogDebug($"Table {data.TableName} truncated");

                cmd.CommandText = $"SELECT * FROM {data.TableName} WHERE 1=2";
                var da = new SqlDataAdapter(cmd);
                da.Fill(data);
            }
            var sw = new Stopwatch();
            logger.LogDebug($"Read data from {sourceUrl}");
            sw.Start();
            var reader = GetCSV(sourceUrl, columnSeparator, encoding);
            TimeSpan ts = sw.Elapsed;
            logger.LogDebug($"Data downloaded from {sourceUrl}: {ts}");
            long rowCounter = 0;
            if (reader.Read())
            {
                while (reader.Read())
                {
                    rowCounter++;
                    var nr = data.NewRow();
                    for (int i = 0; i < reader.FieldsCount; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(reader[i]))
                            {
                                nr[i] = DBNull.Value;
                            }
                            else
                            {
                                if (data.Columns[i].DataType == typeof(decimal) ||
                                    data.Columns[i].DataType == typeof(double) ||
                                    data.Columns[i].DataType == typeof(Double) ||
                                    data.Columns[i].DataType == typeof(Decimal) ||
                                    data.Columns[i].DataType == typeof(float))
                                    nr[i] = decimal.Parse(reader[i], new NumberFormatInfo() { NumberDecimalSeparator = decimalSeparator });
                                else
                                    nr[i] = Convert.ChangeType(reader[i], data.Columns[i].DataType);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"{ex.Message} on row {rowCounter} in column {i + 1}");
                        }
                    }
                    data.Rows.Add(nr);
                }
                logger.LogDebug($"Data loaded into memory: {sw.Elapsed - ts}");
                ts = sw.Elapsed;
                using (var sbc = new SqlBulkCopy(sqlConnection))
                {
                    sbc.DestinationTableName = data.TableName;
                    sbc.BatchSize = batchSize;
                    foreach (DataColumn c in data.Columns)
                        sbc.AddColumnMapping(c.ColumnName, c.ColumnName);

                    sbc.WriteToServer(data);
                    logger.LogInformation($"Data loaded in {data.TableName}: {sw.Elapsed - ts}");
                }
            }
            sw.Stop();
        }

        protected void LogInfo(string message)
        {
            //var msg = $"{DateTime.Now}\tINFO\t{message}";
            if (logger != null)
                logger.LogInformation(message);
            else
                Console.Out.WriteLine(message);
        }
        protected void LogError(string message)
        {
            //var msg = $"{DateTime.Now}\tERROR\t{message}";
            if (logger != null)
                logger.LogError(message);
            else
                Console.Error.WriteLine(message);

        }
    }
}
