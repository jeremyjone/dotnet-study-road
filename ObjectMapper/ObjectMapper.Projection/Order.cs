using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Projection
{
    public class Order
    {
        private readonly IList<OrderLineItem> _orderLineItems = new List<OrderLineItem>();

        public Customer Customer { get; set; }

        public OrderLineItem[] GetOrderLineItems()
        {
            return _orderLineItems.ToArray();
        }

        public void AddOrderLineItem(Product product, int quantity)
        {
            _orderLineItems.Add(new OrderLineItem(product, quantity));
        }

        public decimal GetTotal()
        {
            return _orderLineItems.Sum(li => li.GetTotal());
        }
    }

    public class Product
    {
        public decimal Price { get; set; }
        public string Name { get; set; }
    }

    public class OrderLineItem
    {
        public OrderLineItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public decimal GetTotal()
        {
            return Quantity * Product.Price;
        }
    }

    public class Customer
    {
        public string Name { get; set; }
    }
}
