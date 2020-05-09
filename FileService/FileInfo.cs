using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService
{
    public class FileInfo
    {
        public FileInfo(int size, string name, DateTime saveTime, int userID)
        {
            Size = size;
            Name = name;
            SaveTime = SaveTime;
            Users = new List<int>();
            Users.Add(userID);
        }

        public int Size { private set; get;}

        public string Name { private set; get;}

        public DateTime SaveTime { private set; get; }

        public List<int> Users { set; get; }
    }
}
