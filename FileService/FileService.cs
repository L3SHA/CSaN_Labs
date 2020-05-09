using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileService
{
    public class FileService : IFileService
    {
        private const string BasePath = "Files/";

        private int id = 0;

        private Dictionary<int, FileInfo> files;

        public FileService()
        {
            files = new Dictionary<int, FileInfo>();
        }

        public int DeleteFile(int fileID, int userID)
        {
            throw new NotImplementedException();
        }

        public int GetFileInfo(int fileID, int userID, ref FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }

        public int LoadFile(int fileID, int userID)
        {
            throw new NotImplementedException();
        }

        public int SaveFile(int userID, string fileName, MemoryStream fileData)
        {
            using (FileStream fileStream = File.Create(path + fileName))
            {
                fileStream.Write(fileData.GetBuffer(), 0, fileData.GetBuffer().Length);
            }
            
        }
    }
}
