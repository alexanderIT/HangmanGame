using System.Collections.Generic;
using System.Data.SqlClient;


namespace Game.DAL.Core
{
    internal interface IDbManager<T>
    {
        IEnumerable<T> ExecuteSpCollection(string spName, params SqlParameter[] sqlParameters);

        IEnumerable<T> ExecuteSpCollectionLazy(string query, params SqlParameter[] sqlParameters);

        bool ExecuteSpNonQuery(string spName, params SqlParameter[] parameters);

        bool ExecuteSpNonQuery(string spName, T obj, HashSet<string> ignoreList = null);

        T ExecuteSpVector(string spName, params SqlParameter[] parameters);

        T ExecutSpVector(string spName, T obj, HashSet<string> ignoreList = null);

        TResult ExecuteSpScalar<TResult>(string spName, params SqlParameter[] parameters);

        TResult ExecuteSpScalar<TResult>(string spName, T obj, HashSet<string> ignoreList);

        IEnumerable<T> ExecuteCollection(string query, params SqlParameter[] sqlParameters);

        bool ExecuteNonQuery(string query, T obj, HashSet<string> ignoreList = null);

        bool ExecuteNonQuery(string query, params SqlParameter[] sqlParameters);

        T ExecuteVector(string query, params SqlParameter[] parameters);

        T ExecuteVector(string query, T obj, HashSet<string> ignoreList = null);

        TResult ExecuteScalar<TResult>(string query, params SqlParameter[] parameters);

        TResult ExecuteScalar<TResult>(string query, T obj, HashSet<string> ignoreList);

        bool BulkInsert(string tableName, IEnumerable<T> data, HashSet<string> ignoreList = null, int batchSize = 1000);

        SqlParameter CreateParameter(string parameterName, object parameterValue);
    }
}
