using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.File.Model
{
    public class FileChunk
    {
        //文件名
        public string FileName { get; set; }
        /// <summary>
        /// 当前分片
        /// </summary>
        public int PartNumber { get; set; }
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 分片总数
        /// </summary>
        public int Chunks { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Total { get; set; }
    }
}
