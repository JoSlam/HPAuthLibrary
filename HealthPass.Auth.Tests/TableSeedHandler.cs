using System.Collections.Generic;

namespace HealthPass.Auth.Tests
{
    public class TableSeedHandler
    {
        private readonly Dictionary<string, int> TableDictionary = new Dictionary<string, int>();

        public int GetNewID(string tableName)
        {
            if (!TableDictionary.ContainsKey(tableName))
            {
                TableDictionary.Add(tableName, 1);
                return 1;
            }

            return TableDictionary[tableName];
        }
    }
}
