namespace SQLiteVsFileSystem {
    public class FileDatabaseFileSystem : FileDatabase {
        private string _pathPrefix;
        public FileDatabaseFileSystem(string databaseRootDirectory, bool eraseDbOnStart) {
            databaseRootDirectory = databaseRootDirectory.Replace("\\", "/");
            if (!databaseRootDirectory.EndsWith("/")) {
                databaseRootDirectory += "/";
            }
            _pathPrefix = databaseRootDirectory + "db/";

            if (Directory.Exists(_pathPrefix)) {
                if (eraseDbOnStart) {
                    Directory.Delete(_pathPrefix, true);
                    Directory.CreateDirectory(_pathPrefix);
                }
            } else {
                Directory.CreateDirectory(_pathPrefix);
            }
        }
        public override byte[] ReadFile(string name) {
            return File.ReadAllBytes(_pathPrefix + name);
        }

        public override void WriteFile(string name, byte[] data) {
            File.WriteAllBytes(_pathPrefix + name, data);
        }
    }
}
