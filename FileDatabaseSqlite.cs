using Microsoft.Data.Sqlite;

namespace SQLiteVsFileSystem {
    public class FileDatabaseSqlite : FileDatabase {
        private const string Table0Name = "Dictionary0";
        private SqliteConnection _connection;
        public FileDatabaseSqlite(string databaseFileLocation, bool eraseDbOnStart) {
            if (eraseDbOnStart) {
                if (File.Exists(databaseFileLocation)) {
                    File.Delete(databaseFileLocation);
                }
            }

            _connection = new SqliteConnection($"Data Source={databaseFileLocation}");
            _connection.Open();

            if (!IsTableExists(Table0Name)) {
                CreateTable0();
            }
        }

        public override byte[] ReadFile(string name) {
            var selectCommand = _connection.CreateCommand();
            selectCommand.CommandText = $"SELECT Data FROM {Table0Name} WHERE Name = '{name}' LIMIT 1";

            var outputStream = new MemoryStream();

            using (var reader = selectCommand.ExecuteReader()) {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) {
                        using (var readStream = reader.GetStream(0)) {
                            readStream.CopyTo(outputStream);
                        }
                    }
                }
            }
            return outputStream.ToArray();
        }

        public override void WriteFile(string name, byte[] data) {

            using var insertCommand = _connection.CreateCommand();

            insertCommand.CommandText = $"INSERT INTO {Table0Name}(Name, Data) VALUES ('{name}', zeroblob($length));  SELECT last_insert_rowid();";

            insertCommand.Parameters.AddWithValue("$length", data.Length);
            var rowid = (long)insertCommand.ExecuteScalar();
            using (var writeStream = new SqliteBlob(_connection, Table0Name, "Data", rowid)) {
                var stream = new MemoryStream(data);
                stream.CopyTo(writeStream);
            }
        }

        private bool IsTableExists(string tableName) {
            var sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
            if (_connection.State == System.Data.ConnectionState.Open) {
                SqliteCommand command = new SqliteCommand(sql, _connection);
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows) {
                    return true;
                }
                return false;
            } else {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }

        private void CreateTable0() {
            var command = _connection.CreateCommand();
            string Createsql = $"CREATE TABLE {Table0Name} (Name VARCHAR(256), Data BLOB)";
            command.CommandText = Createsql;
            command.ExecuteNonQuery();
        }
    }
}
