using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileService
{
    interface IFileService
    {
        int SaveFile(int userID, string fileName, MemoryStream fileData);

        int LoadFile(int fileID, int userID);

        int DeleteFile(int fileID, int userID);

        int GetFileInfo(int fileID, int userID, ref FileInfo fileInfo);//return value FileInfo
    }
}
