using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Products
{
    public class ProductQueryParamters
    {
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string? sort { get; set; }
        public string? search { get; set; }

        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 5; 








    }
}
