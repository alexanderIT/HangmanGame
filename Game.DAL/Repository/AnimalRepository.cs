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
   public static class AnimalRepository
    {
        #region ConstantSQLScript

        static readonly string SelectQuery = $"SELECT [{Id}], [{Breed}], [{Tooltip}] FROM [dbo].[Animal]";

        static readonly string InsertQuery = $"INSERT INTO [dbo].[Animal] ([{Id}], [{Breed}], [{Tooltip}] VALUES (@{Id}, @{Breed}, @{Tooltip})";

        static readonly string UpdateQuery = $"UPDATE [dbo].[Animal] SET [{Id}] = @{Id}, [{Breed}] = @{Breed}, [{Tooltip}] = @{Tooltip} WHERE [{Id}] = @{Id}";

        #endregion

        #region ConstantColumns

        const string Id = nameof(Animal.Id);
        const string Breed = nameof(Animal.Breed);
        const string Tooltip = nameof(Animal.Tooltip);

        #endregion

        private static readonly IDbManager<Animal> DbManager = new SqlDbManager<Animal>(SqlServerConnection.ConnectionString, new AnimalParser());

        public static IEnumerable<Animal> GetAll()
        {
            return DbManager.ExecuteCollection(SelectQuery);
        }

        public static Animal GetById(int id)
        {
            var query = $"{SelectQuery} WHERE [{Id}] = @{Id}";
            return DbManager.ExecuteVector(query, DbManager.CreateParameter(Id, id));
        }

        public static Animal GetByBreed(string breed)
        {
            var query = $"{SelectQuery} WHERE [{Breed}] = @{Breed}";
            return DbManager.ExecuteVector(query, DbManager.CreateParameter(Breed, breed));
        }

        public static int InsertProtocol(Animal newAnimal)
        {
            var query = $"{InsertQuery};SELECT {Id} FROM [dbo].[Animal] WHERE {Id} = SCOPE_IDENTITY()";
            return DbManager.ExecuteScalar<int?>(query, newAnimal, Car.IgnoreList) ?? -1;
        }
    }
}
