using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class Page
    {
        [DefaultValue(1), Range(1, int.MaxValue, ErrorMessage = "页数值应大于0")]
        public int pageNo { get; set; }
        [DefaultValue(10), Range(1, 200, ErrorMessage = "数值不在范围内!")]
        public int pageSize { get; set; }
    }
}
