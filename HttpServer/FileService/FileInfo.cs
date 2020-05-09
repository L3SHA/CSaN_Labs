using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.FileService
{
    public class FileInfo
    {
        public FileInfo(long size, string name, string path, DateTime saveTime, int userID)
        {
            Size = size;
            Name = name;
            Path = path;
            SaveTime = SaveTime;
            Users = new List<int>();
            Users.Add(userID);
        }

        public long Size { private set; get; }

        public string Name { private set; get; }

        public string Path { private set; get; }

        public DateTime SaveTime { private set; get; }

        public List<int> Users { set; get; }
    }
}
