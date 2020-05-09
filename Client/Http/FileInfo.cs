using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http
{
    public class FileInfo
    {
        public FileInfo()
        {

        }

        public FileInfo(int size, string name)
        {
            Name = name;
            Size = size;
        }
        public string Name { set; get;}
        public int Size { set; get; }
    }
}
