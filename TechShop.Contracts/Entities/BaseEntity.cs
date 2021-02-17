using System;
using System.Collections.Generic;
using System.Text;

namespace TechShop.Contracts.Entities
{
    public abstract class BaseEntity<TKey>
        where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
