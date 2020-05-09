using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;

namespace Client.Http
{
    public class HttpClientService
    {
        private HttpClient client;
        private const string FileServerURI = "http://localhost:8080/";

        public HttpClientService()
        {
            client = new HttpClient();
        }

        public async Task<int> LoadFile(string path)
        {
            HttpContent fileStreamContent;
            
            int attempt = 0;
            HttpResponseMessage response = null;
            do
                {
                FileStream fileStream = new FileStream(path, FileMode.Open);
                fileStreamContent = new StreamContent(fileStream);
                string httpRequest;
                if (attempt == 0)
                {
                    var fileName = Path.GetFileName(path);
                    httpRequest = FileServerURI + "?" + "type=UploadFile" + "&filename=" + fileName + "&userid=" + ClientRepositoryService.GetInstance().GetClientID();
                }
                else
                {
                    var fileName = Path.GetFileName(path);
                    httpRequest = FileServerURI + "?" + "type=UploadFile" + "&filename=" + "(" + attempt + ")" + fileName + "&userid=" + ClientRepositoryService.GetInstance().GetClientID();
                }
                attempt++;
                    response = await client.PostAsync(httpRequest, fileStreamContent);
                fileStreamContent.Dispose();
                fileStream.Close();
            } while (response.StatusCode != HttpStatusCode.OK);
            
            return -1;
        }

        public async Task<bool> DeleteFile(int fileID)
        {
            string httpRequest = FileServerURI + "?" + "type=DeleteFile" + "&fileid=" + fileID + "&userid=" + ClientRepositoryService.GetInstance().GetClientID();
            var response = await client.DeleteAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else 
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public async Task<FileInfo> GetFileInfo(int fileID)
        {
            string httpRequest = FileServerURI + "?" + "type=GetFileInfo" + "&fileid=" + fileID + "&userid=" + ClientRepositoryService.GetInstance().GetClientID();
            var response = await client.GetAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return new FileInfo(int.Parse(response.Headers.GetValues("size").ToList().ElementAt(0)), response.Headers.GetValues("name").ToList().ElementAt(0));
            }
        }

        public async Task<Stream> GetFile(int fileID)
        {
            string httpRequest = FileServerURI + "?" + "type=GetFile" + "&fileid=" + fileID + "&userid=" + ClientRepositoryService.GetInstance().GetClientID();
            var response = await client.GetAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return await response.Content.ReadAsStreamAsync();
            }
        }

    }
}
