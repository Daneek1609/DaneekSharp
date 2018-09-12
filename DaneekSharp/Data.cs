using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Daneek
{
    public static class Data
    {
        public static dynamic ExecuteReaderCommand(DbConnection connection, string commandText)
        {
            try
            {
                connection.Open();
                var result = new List<dynamic>();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                var r = command.ExecuteReader();
                result = ToObject(r).ToList();
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }

        private static dynamic Convert(IDataRecord record)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < record.FieldCount; i++)
                expandoObject.Add(record.GetName(i), record[i]);

            return expandoObject;
        }

        public static IEnumerable<dynamic> ToObject(DbDataReader reader)
        {
            while (reader.Read())
            {
                yield return Convert(reader);
            }
        }
    }
}