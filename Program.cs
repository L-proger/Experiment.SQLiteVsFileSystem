using System.Diagnostics;

namespace SQLiteVsFileSystem {

    class App {
        public static void Main() {
            string testDirectory = "E:/FsTest/";
            if(!Directory.Exists(testDirectory)) {
                Console.WriteLine($"Test directory '{testDirectory}' does not exist!");
                return;
            }
            int testFilesCount = 10000;
            TestDatabase(new FileDatabaseLevelDB(testDirectory + "level.db", true), testFilesCount);
            //TestDatabase(new FileDatabaseSqlite(testDirectory + "sqlite.db", true), testFilesCount);
            TestDatabase(new FileDatabaseFileSystem(testDirectory, true), testFilesCount);
        }

        static void TestDatabase(FileDatabase db, int testFilesCount) {
            Console.WriteLine($"### Testing {db.GetType().Name}: ");

            //Generate unique file names
            var testFileNames = Enumerable.Range(0, testFilesCount).Select(i => $"File{i}.bin").ToArray();

            //File data
            byte[] testFileContent = new byte[] { 1, 2, 3, 4, 5 };

            //Write files into database
            Console.WriteLine($"Writing files in {db.GetType().Name}...");
            foreach (string fileName in testFileNames) {
                db.WriteFile(fileName, testFileContent);
            }

            //Run READ test
            Console.WriteLine($"Reading files from {db.GetType().Name}...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int testIterationsCount = 10000;
            for (int i = 0; i < testIterationsCount; ++i) {
                var result = db.ReadFile(testFileNames[i % testFileNames.Length]);
            }
            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            Console.WriteLine("Read test total time ms: " + ms);
            Console.WriteLine("Read test iteration time ms: " + (ms / (double)testIterationsCount));
            Console.WriteLine();
        }
    }
}