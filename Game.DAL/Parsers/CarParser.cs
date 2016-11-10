using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Game.DAL.Core;
using Game.DAL.DataObject;

namespace Game.DAL.Parsers
{
    internal class CarParser : ParserBase<Car>
    {
        private enum Ordinals
        {
            Id,
            Name,
            Tooltip
        }

        public override Car MapToObject(IDataRecord record)
        {
            return new Car
            {
                Id = record.GetInt32((int)Ordinals.Id),
                Model = record.GetString((int)Ordinals.Name),
                Tooltip = record.GetString((int)Ordinals.Tooltip)
            };
        }

        public override SqlParameter[] MapToSqlParameters(Car obj, HashSet<string> ignoreList = null)
        {
            if (ignoreList == null)
                ignoreList = new HashSet<string>();

            var parameters = new List<SqlParameter>();
            if (!ignoreList.Contains(nameof(obj.Id)))
                parameters.Add(new SqlParameter(nameof(obj.Id), SqlDbType.Int) { Value = obj.Id });

            if (!ignoreList.Contains(nameof(obj.Model)))
                parameters.Add(new SqlParameter(nameof(obj.Model), SqlDbType.NVarChar) { Value = obj.Model });

            if (!ignoreList.Contains(nameof(obj.Tooltip)))
                parameters.Add(new SqlParameter(nameof(obj.Tooltip), SqlDbType.NVarChar) { Value = obj.Tooltip });

            return parameters.ToArray();
        }
    }
}
