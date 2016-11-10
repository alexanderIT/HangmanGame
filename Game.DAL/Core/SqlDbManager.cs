using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DAL.Core
{

    internal class SqlDbManager<T> : IDbManager<T>
    {
        #region Private properties

        private readonly string _connectionString;

        private readonly ParserBase<T> _parser;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates instance of the SqlDbManager class to connect with the given sql connection string and uses the provided parser class
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="parser">The implementation of ParserBase class</param>        
        public SqlDbManager(string connectionString, ParserBase<T> parser)
        {
            _connectionString = connectionString;
            _parser = parser;
        }

        #endregion

        #region Public Methods

        #region Stored Procedures execution methods

        /// <summary>
        /// Executes a SQL stored procedure with specified parameters
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed</param>
        /// <param name="sqlParameters">The parameters, needed by the stored procedure</param>
        /// <returns>A collection of elements of type T</returns>
        public IEnumerable<T> ExecuteSpCollection(string spName, params SqlParameter[] sqlParameters)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command => _parser.MappAllRecords(command.ExecuteReader()), sqlParameters);
        }

        /// <summary>
        /// Executes a SQL stored procedure with specified parameters lazily
        /// </summary>
        /// <param name="query">The name of the SQL stored procedure to be executed</param>
        /// <param name="sqlParameters">The SQL parameters to be added to the SQL command</param>
        /// <returns>A List of type T</returns>
        public IEnumerable<T> ExecuteSpCollectionLazy(string query, params SqlParameter[] sqlParameters)
        {
            return ExecuteSqlQueryCollectionLazy(query, CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// Executes a stored procedure with specified parameters
        /// </summary>
        /// <param name="spName">The SQL stored procedure to be executed</param>
        /// <param name="parameters">The SQL parameters to be added to the SQL command</param>
        /// <returns>Returns bool value that indicates if any rows were affected</returns>
        public bool ExecuteSpNonQuery(string spName, params SqlParameter[] parameters)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command => command.ExecuteNonQuery() != 0, parameters);
        }

        /// <summary>
        /// Executes a SQL stored procedure with an object of type <T/>, serializaed to SQL parameters and skipes the given ordinal during serialization
        /// </summary>
        /// <param name="spName">The SQL stored procedure to be executed</param>
        /// <param name="obj">The object to be converted to SQL parameters</param>
        /// <param name="ignoreList">The list of SQL parameters to be skipped during serialization of the object into SQL parameters</param>
        /// <returns>Return a bolean value idicating if the query affected any rows</returns>
        public bool ExecuteSpNonQuery(string spName, T obj, HashSet<string> ignoreList = null)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command => command.ExecuteNonQuery() != 0, _parser.MapToSqlParameters(obj, ignoreList));
        }

        /// <summary>
        /// Executes a SQL stored procedure with specified parameters
        /// </summary>
        /// <param name="spName">The SQL parameters to be added to the SQL command</param>
        /// <param name="parameters">The SQL parameters to be added to the SQL command</param>
        /// <returns>Returns a single row represented as an object of type <T/></returns>
        public T ExecuteSpVector(string spName, params SqlParameter[] parameters)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command =>
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return _parser.MapToObject(reader);
                    }
                }

                return default(T);

            }, parameters);
        }

        /// <summary>
        /// Executes a query with parameters extracted from and object of type T and returns a single row as object of type T, ignoring the given ordinal
        /// </summary>
        /// <param name="spName">The SQl query to be executed</param>
        /// <param name="obj">The object ot be serializaed into SQL parameters with the using the method of the parser class</param>
        /// <param name="ignoreList">The list of SQL parameters to be skipped during serialization of the object into SQL parameters</param>
        /// <returns>object of type <T/></returns>
        public T ExecutSpVector(string spName, T obj, HashSet<string> ignoreList = null)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command =>
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return _parser.MapToObject(reader);
                    }
                }

                return default(T);

            }, _parser.MapToSqlParameters(obj, ignoreList));
        }

        /// <summary>
        /// Executes a SQL stored procedure and returns the first column of the first row
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="spName">The SQL stored procedure to be executed</param>
        /// <param name="parameters">The SQL parameters to be added to the SQL stored procedure</param>
        /// <returns>Returns a scalar value of type<TResult/></returns>
        public TResult ExecuteSpScalar<TResult>(string spName, params SqlParameter[] parameters)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command => (TResult)command.ExecuteScalar(), parameters);
        }

        /// <summary>
        /// Executes a SQL query and returns the first column of the first row
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="obj">The objet that will be serialized into SQL Parameters</param>
        /// <param name="ignoreList">The list of property names that will be ignored during serialization</param>
        /// <returns>The first column of the first row as type <TResult/></returns>
        public TResult ExecuteSpScalar<TResult>(string spName, T obj, HashSet<string> ignoreList)
        {
            return ExecuteSqlQuery(spName, CommandType.StoredProcedure, command => (TResult)command.ExecuteScalar(), _parser.MapToSqlParameters(obj, ignoreList));
        }

        #endregion

        #region TSQL Query Execution Methods

        /// <summary>
        /// Executes a query with specified parameters
        /// </summary>
        /// <param name="query">The SQL query to be executed</param>
        /// <param name="sqlParameters">The SQL parameters to be added to the SQL command</param>
        /// <returns>A List of type T</returns>
        public IEnumerable<T> ExecuteCollection(string query, params SqlParameter[] sqlParameters)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command => _parser.MappAllRecords(command.ExecuteReader()), sqlParameters);
        }

        /// <summary>
        /// Executes an SQL query with an object of type T, serialized to SQL parameters and skips the given list of fields during serialization.
        /// </summary>
        /// <param name="query">The SQL query to be executed</param>
        /// <param name="obj">The object to be converted to SQL parameters</param>
        /// <param name="ignoreList">The list of SQL parameters to be skipped during serialization of the object into SQL parameters</param>
        /// <returns>Returns bool value that indicates if any rows were affected</returns>
        public bool ExecuteNonQuery(string query, T obj, HashSet<string> ignoreList = null)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command => command.ExecuteNonQuery() != 0, _parser.MapToSqlParameters(obj, ignoreList));
        }

        /// <summary>
        /// Executes an SQL query, with a given list of parameters.
        /// </summary>
        /// <param name="query">The SQL query to be executed</param>
        /// <param name="sqlParameters">The list of SQL parameters to be used</param>
        /// <returns>Returns bool value that indicates if any rows were affected</returns>
        public bool ExecuteNonQuery(string query, params SqlParameter[] sqlParameters)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command => command.ExecuteNonQuery() != 0, sqlParameters);
        }

        /// <summary>
        /// Executes a query with specified parameters
        /// </summary>
        /// <param name="query">The SQL parameters to be added to the SQL command</param>
        /// <param name="parameters">The SQL parameters to be added to the SQL command</param>
        /// <returns>Returns a single row represented as an object of type T</returns>
        public T ExecuteVector(string query, params SqlParameter[] parameters)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command =>
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return _parser.MapToObject(reader);
                    }
                }

                return default(T);

            }, parameters);
        }

        /// <summary>
        /// Executes a query with parameters extracted from and object of type T and returns a single row as object of type T, skipping the given list of fields during serialization.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <param name="obj">The object to be converted to SQL parameters.</param>
        /// <param name="ignoreList">The list of SQL parameters to be skipped during serialization of the object into SQL parameters.</param>
        /// <returns>An object of type T.</returns>
        public T ExecuteVector(string query, T obj, HashSet<string> ignoreList = null)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command =>
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return _parser.MapToObject(reader);
                    }
                }

                return default(T);

            }, _parser.MapToSqlParameters(obj, ignoreList));
        }

        /// <summary>
        /// Executes a SQL query and returns the first column of the first row
        /// </summary>
        /// <typeparam name="TResult">The type of teh result</typeparam>
        /// <param name="query">The SQL query to be executed</param>
        /// <param name="parameters">The SQL parameters to be added to the SQL query</param>
        /// <returns>Returns a scalar value of type<TResult/></returns>
        public TResult ExecuteScalar<TResult>(string query, params SqlParameter[] parameters)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command => (TResult)command.ExecuteScalar(), parameters);
        }

        /// <summary>
        /// Executes a SQL query and returns the first column of the first row
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="query">The T-SQL Query to be executed</param>
        /// <param name="obj">The objet that will be serialized into SQL Parameters</param>
        /// <param name="ignoreList">The list of property names that will be ignored during serialization</param>
        /// <returns>The first column of the first row as type <TResult/></returns>
        public TResult ExecuteScalar<TResult>(string query, T obj, HashSet<string> ignoreList)
        {
            return ExecuteSqlQuery(query, CommandType.Text, command => (TResult)command.ExecuteScalar(), _parser.MapToSqlParameters(obj, ignoreList));
        }

        #endregion

        /// <summary>
        /// Bulk inserts a collection of data the target table with the specified batchSize, converting the IEnumerable to datatable.
        /// The bulk insert is executed in transaction.
        /// </summary>
        /// <param name="tableName">The target table on the SQL Server</param>
        /// <param name="data">The collection of data to be inserted</param>
        /// <param name="ignoreList">The list of property names that will be ignored during serialization</param>
        /// <param name="batchSize">The size of the insert batch</param>
        public bool BulkInsert(string tableName, IEnumerable<T> data, HashSet<string> ignoreList = null, int batchSize = 10000)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    using (var bulkCopyObject = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        List<SqlBulkCopyColumnMapping> mapping;
                        var datatable = _parser.ConvertToDataTable(data, out mapping, ignoreList);
                        bulkCopyObject.BatchSize = batchSize;
                        bulkCopyObject.DestinationTableName = tableName;

                        foreach (var sqlBulkCopyColumnMapping in mapping)
                        {
                            bulkCopyObject.ColumnMappings.Add(sqlBulkCopyColumnMapping);
                        }

                        bulkCopyObject.WriteToServer(datatable);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle exception here.
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a SQL Parameter with a given name and value
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string parameterName, object parameterValue)
        {
            return parameterValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, parameterValue);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Executes and SQL query and performs a specified action with the SQL command
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="query">The SQL query string ot be executed</param>
        /// <param name="cmdType">The type of the command</param>
        /// <param name="action">The action to be performed with the SQL command</param>
        /// <param name="parameters">The list of SQl parameters</param>
        /// <returns>An object of type TResult</returns>
        private TResult ExecuteSqlQuery<TResult>(string query, CommandType cmdType, Func<SqlCommand, TResult> action, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = cmdType;
                connection.Open();

                if (parameters != null && parameters.Length != 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return action(cmd);
            }
        }

        /// <summary>
        /// Executes lazy and SQL query and performs a specified action with the SQL command
        /// </summary>
        /// <param name="query">The query to be executed</param>
        /// <param name="cmdType">The type of the command</param>
        /// <param name="parameters">The list of SQl parameters</param>
        /// <returns>An object of type TResult</returns>
        private IEnumerable<T> ExecuteSqlQueryCollectionLazy(string query, CommandType cmdType, params SqlParameter[] parameters)
        {           

            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = cmdType;
                connection.Open();

                if (parameters != null && parameters.Length != 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return _parser.MapToObject(reader);
                    }
                }
            }
        }

        #endregion
    }
}
