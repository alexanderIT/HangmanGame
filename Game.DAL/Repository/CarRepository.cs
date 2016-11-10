using System;
using System.Collections.Generic;
using System.Linq;
using Game.DAL.Core;
using Game.DAL.DataObject;
using Game.DAL.Parsers;

namespace Game.DAL.Repository
{
   public static class CarRepository
    {
        #region ConstantSQLScript

        static readonly string SelectQuery = $"SELECT [{Id}], [{Model}], [{Tooltip}] FROM [dbo].[Car]";

        static readonly string InsertQuery = $"INSERT INTO [dbo].[Car] ([{Id}], [{Model}], [{Tooltip}] VALUES (@{Id}, @{Model}, @{Tooltip})";

        static readonly string UpdateQuery = $"UPDATE [dbo].[Car] SET [{Id}] = @{Id}, [{Model}] = @{Model}, [{Tooltip}] = @{Tooltip} WHERE [{Id}] = @{Id}";

        #endregion

        #region ConstantColumns

        const string Id = nameof(Car.Id);
        const string Model = nameof(Car.Model);
        const string Tooltip = nameof(Car.Tooltip);      

        #endregion

        private static readonly IDbManager<Car> DbManager = new SqlDbManager<Car>(SqlServerConnection.ConnectionString, new CarParser());

        public static IEnumerable<Car> GetAll()
        {
            return DbManager.ExecuteCollection(SelectQuery);
        }

        public static Car GetById(int id)
        {
            var query = $"{SelectQuery} WHERE [{Id}] = @{Id}";
            return DbManager.ExecuteVector(query, DbManager.CreateParameter(Id, id));
        }

        public static Car GetByModel(string model)
        {
            var query = $"{SelectQuery} WHERE [{Model}] = @{Model}";
            return DbManager.ExecuteVector(query, DbManager.CreateParameter(Model, model));
        }

        public static int InsertProtocol(Car newCar)
        {
            var query = $"{InsertQuery};SELECT {Id} FROM [dbo].[Car] WHERE {Id} = SCOPE_IDENTITY()";
            return DbManager.ExecuteScalar<int?>(query, newCar, Car.IgnoreList) ?? -1;
        }
    }
}
