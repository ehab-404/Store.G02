using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Baskets
{
    public class BasketDto
    {
        string Id { get; set; }

        IEnumerable<BasketItemDto> items { get; set; }





    }
}
