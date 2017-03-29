using System;
using System.Collections.Generic;
using GPOrder.Models;

namespace GPOrder.ViewModels
{
    public class ShopGroupedOrders
    {
        public Guid? ShopId { get; set; }
        public IEnumerable<GroupedOrder> GroupOrders { get; set; }
    }
}