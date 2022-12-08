using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteVsFileSystem {
    public abstract class FileDatabase {
        public abstract void WriteFile(string name, byte[] data);
        public abstract byte[] ReadFile(string name);
    }
}
