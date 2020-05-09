using System.Net;
using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HttpServer
{
    public class Server
    {
        private const string HttpPrefix = "http://localhost:8080/";
        private const string StoragePath = @"C:\Fourth semester\CSaN\CSaN_Labs\Storage\";
        private Dictionary<int, FileService.FileInfo> files;
        private int FileID =  0;
        public Server()
        {
            files = new Dictionary<int, FileService.FileInfo>();
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(HttpPrefix);
            listener.Start();
            while(true)
            {   
                RequestHandlerAsync(listener.GetContext());      
            }
        }

        private async void RequestHandlerAsync(HttpListenerContext context)
        {
            await Task.Run(() => HandleHTTPMethod(context));
        }

        private void UploadFile(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string fileName = request.QueryString.Get("filename");
            string userID = request.QueryString.Get("userid");
            string path = StoragePath + userID + @"\" + fileName;
            if (Directory.Exists(StoragePath + userID + @"\"))
            {
                if (!File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                    {
                        request.InputStream.CopyTo(fs);
                        var fileInfo = new FileInfo(path);
                        files.Add(++FileID, new FileService.FileInfo(fileInfo.Length, fileInfo.Name, fileInfo.FullName, fileInfo.CreationTime, int.Parse(userID)));
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Close();
                    }
                }
                else
                {
                    foreach(int id in files.Keys)
                    {
                        Console.WriteLine(files[id].Name);
                        if (files[id].Name == fileName)
                        {
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            Console.WriteLine("File exists" + (int)HttpStatusCode.InternalServerError);
                            Stream answer = response.OutputStream;
                            byte[] buffer = Encoding.Default.GetBytes(id.ToString());
                            answer.Write(buffer, 0, buffer.Length);
                            answer.Close();
                            response.Close();
                            break;
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(StoragePath + request.QueryString.Get("userid") + @"\");
                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {
                    request.InputStream.CopyTo(fs);
                    var fileInfo = new FileInfo(path);
                    files.Add(FileID, new FileService.FileInfo(fileInfo.Length, fileInfo.Name, fileInfo.FullName, fileInfo.CreationTime, int.Parse(request.QueryString.Get("userid"))));
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Close();
                }
            }
        }

        private void HandleHTTPMethod(HttpListenerContext context)
        {
            switch(context.Request.QueryString.Get("type"))
            {
                case "DeleteFile":
                    DeleteFile(context);
                    break;
                case "GetFileInfo":
                    GetFileInfo(context);
                    break;
                case "GetFile":
                    GetFile(context);
                    break;
                case "UploadFile":
                    UploadFile(context);
                    break;
            }
        }

        private void DeleteFile(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string userID = request.QueryString.Get("userid");
            int fileID = int.Parse(request.QueryString["fileid"]);
            if(files.ContainsKey(fileID) && files[fileID].Users.Contains(int.Parse(userID)))
            {
                string path = files[fileID].Path;
                Console.WriteLine(path);
                File.Delete(path);
                files.Remove(fileID);
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                Console.WriteLine("OK");
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            response.Close();
        }

        private void GetFileInfo(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string userID = request.QueryString.Get("userid");
            int fileID = int.Parse(request.QueryString["fileid"]);
            if (files.ContainsKey(fileID) && files[fileID].Users.Contains(int.Parse(userID)))
            {
                string path = files[fileID].Path;
                Console.WriteLine(path);
                var fileInfo = files[fileID];
                response.Headers.Set("name", fileInfo.Name);
                response.Headers.Set("size", fileInfo.Size.ToString());
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                Console.WriteLine("OK");
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            response.Close();
        }

        private void GetFile(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            int fileID = int.Parse(request.QueryString["fileid"]);
            if(files.ContainsKey(fileID))
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                using (var fs = new FileStream(files[fileID].Path, FileMode.Open))
                {
                    Console.WriteLine("shittty");
                    fs.CopyTo(response.OutputStream);
                }
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            response.Close();
        }
    }
}
