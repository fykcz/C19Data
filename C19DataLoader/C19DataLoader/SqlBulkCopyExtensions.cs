using System.Data.SqlClient;

namespace Fyk.C19.C19DataLoader
{
    public static class SqlBulkCopyExtensions
    {
        public static SqlBulkCopyColumnMapping AddColumnMapping(this SqlBulkCopy sbc, int sourceColumnOrdinal, int targetColumnOrdinal)
        {
            var map = new SqlBulkCopyColumnMapping(sourceColumnOrdinal, targetColumnOrdinal);
            sbc.ColumnMappings.Add(map);

            return map;
        }

        public static SqlBulkCopyColumnMapping AddColumnMapping(this SqlBulkCopy sbc, string sourceColumn, string targetColumn)
        {
            var map = new SqlBulkCopyColumnMapping(sourceColumn, targetColumn);
            sbc.ColumnMappings.Add(map);

            return map;
        }
    }
}
