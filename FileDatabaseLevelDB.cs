using LevelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteVsFileSystem {
    public class FileDatabaseLevelDB : FileDatabase {
        private DB _db;

        public FileDatabaseLevelDB(string databaseFilePath, bool eraseDbOnStart) {
            var options = new Options { CreateIfMissing = true };
            _db = new DB(options, databaseFilePath);
        }

        public override byte[] ReadFile(string name) {
            return _db.Get(System.Text.Encoding.Unicode.GetBytes(name));
        }

        public override void WriteFile(string name, byte[] data) {
            _db.Put(System.Text.Encoding.Unicode.GetBytes(name), data);
        }
    }
}
