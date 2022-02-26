using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Infrastructure.Core.File
{
    public interface IFileService
    {
        Task<string> SaveFile(Stream fileStream);
        Task<string> SaveBase64StringToFile(string base64String);
        void DeleteFileById(string oldFileId);
        Task<Stream> GetFileStreamById(string Id);
        Task<Stream> CompressDirectory(string tempDirPath, string tempZipFilePath);
    }
}









