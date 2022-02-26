using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Core;
using Dci.Mnm.Mwa.Infrastructure.Core.File;
using Microsoft.Extensions.Logging;

namespace Dci.Mnm.Mwa.Infrastructure.File
{
    public class FileService : IFileService
    {
        readonly AppConfig appConfig;
        readonly ILogger<FileService> logger;

        public FileService(AppConfig appConfig, ILogger<FileService> logger)
        {
            this.appConfig = appConfig;
            this.logger = logger;
        }
        public void DeleteFileById(string fileId)
        {
            logger.LogInformation("Deleting file : {fileId}", fileId);
            if (string.IsNullOrEmpty(fileId))
            {
                return;
            }

            string fullFilesPath = GetFilePath();

            var path = Path.Combine(fullFilesPath, fileId);

            try
            {
                System.IO.File.Delete(path);
                logger.LogInformation("Deleted file : {fileId}", fileId);

            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "failed to delete file : {fileId}: {errorMessage}", fileId, ex.GetInnerMessages());
                throw ex;
            }
        }

        public string GetFileUrlById(string fileId, string fileName = "File", bool fullPath = true)
        {
            if (string.IsNullOrEmpty(fileId)) return null;

            var relativepath = string.Format(appConfig.Links.FileUrl, fileId);

            if (fullPath)
            {
                return appConfig.Links.BaseUrl + relativepath;
            }
            else
            {
                return relativepath;
            }

        }

        public FileInfo GetFileInfoById(string Id)
        {
            string fullFilesPath = GetFilePath();

            var path = Path.Combine(fullFilesPath, Id);

            var file = new FileInfo(path);

            return file;
        }

        public async Task<Stream> GetFileStreamById(string Id)
        {
            string fullFilesPath = GetFilePath();

            var path = Path.Combine(fullFilesPath, Id);

            var file = new FileInfo(path);

            var memoryStream = new MemoryStream();

            using (var fileStream = file.OpenRead())
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public async Task<string> SaveFile(Stream fileStream)
        {
            string fullFilesPath = GetFilePath();

            var fileId = Utility.CreateNewId().ToString();

            var path = Path.Combine(fullFilesPath, fileId);

            using (var saveFileStream = System.IO.File.Create(path, 200000, FileOptions.Asynchronous))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                await fileStream.CopyToAsync(saveFileStream);
            }

            return fileId;
        }

        public async Task<string> SaveBase64StringToFile(string base64String)
        {
            string fullFilesPath = GetFilePath();

            var fileId = Utility.CreateNewId().ToString();

            var path = Path.Combine(fullFilesPath, fileId);

            using (var saveFileStream = System.IO.File.Create(path, 200000, FileOptions.Asynchronous))
            {
                var cleansString = base64String.Substring(base64String.IndexOf(",") + 1);
                var bytes = Convert.FromBase64String(cleansString);
                await saveFileStream.WriteAsync(bytes, 0, bytes.Length);
            }
            return fileId;
        }

        private string GetFilePath()
        {
            var defaultFilesPath = appConfig.Files.filePath;
            var createFolder = appConfig.Files.CreateFolderIfDoesExist;
            var fullFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultFilesPath);

            if (createFolder && !System.IO.Directory.Exists(fullFilesPath))
            {
                System.IO.Directory.CreateDirectory(fullFilesPath);
            }
            return fullFilesPath;
        }

        public async Task<Stream> CompressDirectory(string tempDirPath, string tempZipFilePath)
        {
            try
            {
                ZipFile.CreateFromDirectory(tempDirPath, tempZipFilePath);
                logger.LogInformation("Deleting temp directory {directoryName} after zip is created ", Path.GetFileName(tempDirPath));
                Directory.Delete(tempDirPath, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Issue while compressing directory to zip", ex.GetInnerMessages());
                throw ex;
            }

            var file = new FileInfo(tempZipFilePath);

            var memoryStream = new MemoryStream();

            using (var fileStream = file.OpenRead())
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            logger.LogInformation("Deleting zip file {fileName} from temp  after zip is created ", Path.GetFileName(tempZipFilePath));
            System.IO.File.Delete(tempZipFilePath);

            return memoryStream;
        }
    }
}









