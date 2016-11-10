using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;


namespace Game.DAL.Core
{
    public abstract class ParserBase<T>
    {
        public abstract T MapToObject(IDataRecord record);

        public virtual SqlParameter[] MapToSqlParameters(T obj, HashSet<string> ignoreList = null)
        {
            var personType = typeof(T);
            var properties = personType.GetProperties();
            var sqlParameters = new List<SqlParameter>();

            if (ignoreList == null)
            {
                ignoreList = new HashSet<string>();
            }

            foreach (var property in properties)
            {
                if (!ignoreList.Contains(property.Name))
                {
                    sqlParameters.Add(new SqlParameter(property.Name, property.GetValue(obj) ?? DBNull.Value));
                }
            }

            return sqlParameters.ToArray();
        }

        public List<T> MappAllRecords(IDataReader reader)
        {
            var list = new List<T>();

            while (reader.Read())
            {
                list.Add(MapToObject(reader));
            }

            return list;
        }

        public virtual DataTable ConvertToDataTable(IEnumerable<T> list, out List<SqlBulkCopyColumnMapping> mapping, HashSet<string> ignoreList = null)
        {
            var propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            mapping = new List<SqlBulkCopyColumnMapping>();

            for (var i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                var propertyDescriptor = propertyDescriptorCollection[i];
                var propType = propertyDescriptor.PropertyType;

                if (ignoreList != null && !ignoreList.Contains(propertyDescriptor.Name))
                {
                    mapping.Add(new SqlBulkCopyColumnMapping(propertyDescriptor.Name, propertyDescriptor.Name));
                }

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    table.Columns.Add(propertyDescriptor.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(propertyDescriptor.Name, propType);
                }
            }

            var values = new object[propertyDescriptorCollection.Count];

            foreach (var listItem in list)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(listItem);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
}
