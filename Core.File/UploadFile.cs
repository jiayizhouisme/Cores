using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using SampleApp.Utilities;
using Core.File.Model;
using SqlSugar;
using Microsoft.Extensions.Hosting;
using Furion.TaskQueue;
using Furion.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace Core.File
{
    public class UploadFile : ISingleton
    {
        public async Task UploadPhysical(string type,
            Stream body,
            FileChunk fileChunk,
            [EnumeratorCancellation] CancellationTokenSource cancellationTokenSource = default)
        {
            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(type),
                64);
            var reader = new MultipartReader(boundary, body);
            var section = await reader.ReadNextSectionAsync();

            if (!Directory.Exists("Upload"))
            {
                Directory.CreateDirectory("Upload");
            }
            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        await Task.CompletedTask;
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        //var trustedFileNameForFileStorage = Path.GetRandomFileName();
                        var trustedFileNameForFileStorage = GetFileName(section.ContentDisposition);
                        fileChunk.FileName = trustedFileNameForFileStorage;
                        //var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                        //section, contentDisposition, size);

                        byte[] buffer = new byte[8192];
                        int bytesRead;

                        using (var targetStream = System.IO.File.Create(
                        Path.Combine("Upload", trustedFileNameForFileStorage)))
                        {
                            do
                            {
                                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                                bytesRead = await section.Body.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                                await targetStream.WriteAsync(buffer, 0, bytesRead, cancellationTokenSource.Token);
                            } while (bytesRead > 0);

                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }
        }

        public async Task MergeChunkFile(FileChunk chunk)
        {
            //文件上传目录名
            var uploadDirectoryName = Path.Combine("Upload", "", chunk.FileName);

            //分片文件命名约定
            var partToken = FileSort.PART_NUMBER;

            //上传文件实际名称
            var baseFileName = chunk.FileName.Substring(0, chunk.FileName.IndexOf(partToken));

            //根据命名约定查询指定目录下符合条件的所有分片文件
            var searchpattern = $"{Path.GetFileName(baseFileName)}{partToken}*";

            //获取所有分片文件列表
            var filesList = Directory.GetFiles(Path.GetDirectoryName(uploadDirectoryName), searchpattern);
            if (!filesList.Any()) { return; }

            var mergeFiles = new List<FileSort>();
            foreach (string file in filesList)
            {
                var sort = new FileSort
                {
                    FileName = file
                };

                baseFileName = file.Substring(0, file.IndexOf(partToken));

                var fileIndex = file.Substring(file.IndexOf(partToken) + partToken.Length);

                int.TryParse(fileIndex, out var number);
                if (number <= 0) { continue; }

                sort.PartNumber = number;

                mergeFiles.Add(sort);
            }  // 按照分片排序
            var mergeOrders = mergeFiles.OrderBy(s => s.PartNumber).ToList();

            // 合并文件
            using var fileStream = new FileStream(baseFileName, FileMode.Create);
            foreach (var fileSort in mergeOrders)
            {
                using FileStream fileChunk =
                   new FileStream(fileSort.FileName, FileMode.Open);
                await fileChunk.CopyToAsync(fileStream);
            }

            //删除分片文件
            DeleteFile(mergeFiles);

        }

        public void DeleteFile(List<FileSort> files)
        {
            foreach (var file in files)
            {
                System.IO.File.Delete(file.FileName);
            }
        }

        private string GetFileName(string contentDisposition)
        {
            return contentDisposition
              .Split(';')
              .SingleOrDefault(part => part.Contains("filename"))
              .Split('=')
              .Last()
              .Trim('"');
        }
    }

    public class FileSort
    {
        public const string PART_NUMBER = ".partNumber-";
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件分片号
        /// </summary>
        public int PartNumber { get; set; }
    }
}
