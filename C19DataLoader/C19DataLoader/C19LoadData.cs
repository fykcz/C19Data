using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fyk.C19.C19DataLoader.Config;
using System.Data.SqlClient;
using System.Diagnostics;
//using Microsoft.Extensions.Logging;

namespace Fyk.C19.C19DataLoader
{
    internal class C19LoadData
    {
        public void TransferData(StreamReader config, IC19Logger logger)
        {
            var c = Config.Config.FromJson(config.ReadToEnd());
            var tgt = c.C19.Target;
            var scb = new SqlConnectionStringBuilder()
            {
                DataSource = tgt.Server,
                InitialCatalog = tgt.Database,
                IntegratedSecurity = true
            };
            if (tgt.AuthenticationType.Equals(C19DataLoader.Config.Target.SQL))
            {
                scb.IntegratedSecurity = false;
                scb.UserID = tgt.SqlAuthentication.Login;
                scb.Password = Crypto.Decrypt(tgt.SqlAuthentication.Password, c.TenantTd);
            }
            logger.LogDebug($"Connecting to {tgt.Server}:{tgt.Database}");
            var conn = new SqlConnection(scb.ConnectionString);
            conn.Open();
            logger.LogInformation($"Connected to {tgt.Server}:{tgt.Database}");
            var trans = new Transfer(conn, logger);

            if (!string.IsNullOrEmpty(tgt.PreLoad))
            {
                logger.LogInformation($"Running preload command {tgt.PreLoad}");
                using (var cmd = new SqlCommand(tgt.PreLoad))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }

            foreach (var x in c.C19.Transfers)
            {
                var tr = x.Transfer;
                logger.LogInformation($"Loading {tr.Name}");
                logger.LogInformation(tr.LogInfo);
                trans.RunTransfer(tr.Target, tr.Source.Url.AbsoluteUri, tr.Source.Encoding, (int)tr.BatchSize, tr.Source.ColumnSeparator, tr.Source.DecimalSeparator);
            }

            if (!string.IsNullOrEmpty(tgt.PostLoad))
            {
                logger.LogInformation($"Running postload command {tgt.PostLoad}");
                using (var cmd = new SqlCommand(tgt.PostLoad))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }

            conn.Close();
        }
    }
}
