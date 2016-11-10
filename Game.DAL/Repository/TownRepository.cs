using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.DAL.Core;
using Game.DAL.DataObject;
using Game.DAL.Parsers;

namespace Game.DAL.Repository
{
   public static class TownRepository
    {
        #region ConstantSQLScript

        static readonly string SelectQuery = $"SELECT [{Id}], [{Name}], [{Tooltip}] FROM [dbo].[Town]";

        static readonly string InsertQuery = $"INSERT INTO [dbo].[Town] ([{Id}], [{Name}], [{Tooltip}] VALUES (@{Id}, @{Name}, @{Tooltip})";

        static readonly string UpdateQuery = $"UPDATE [dbo].[Town] SET [{Id}] = @{Id}, [{Name}] = @{Name}, [{Tooltip}] = @{Tooltip} WHERE [{Id}] = @{Id}";

        #endregion

        #region ConstantColumns

        const string Id = nameof(Car.Id);
        const string Name = nameof(Car.Model);
        const string Tooltip = nameof(Car.Tooltip);

        #endregion

        private static readonly IDbManager<Town> DbManager = new SqlDbManager<Town>(SqlServerConnection.ConnectionString, new TownParser());

        public static IEnumerable<Town> GetAll()
        {
            return DbManager.ExecuteCollection(SelectQuery);
        }

        public static Town GetById(int id)
        {
            var query = $"{SelectQuery} WHERE [{Id}] = @{Id}";
            return DbManager.ExecuteVector(query, DbManager.CreateParameter(Id, id));
        }

        public static Town GetByName(string name)
        {
            var query = $"{SelectQuery} WHERE [{Name}] = @{Name}";
            return DbManager.ExecuteVector(query, DbManager.CreateParameter(Name, name));
        }

        public static int InsertProtocol(Town newTown)
        {
            var query = $"{InsertQuery};SELECT {Id} FROM [dbo].[Town] WHERE {Id} = SCOPE_IDENTITY()";
            return DbManager.ExecuteScalar<int?>(query, newTown, Town.IgnoreList) ?? -1;
        }
    }
}
