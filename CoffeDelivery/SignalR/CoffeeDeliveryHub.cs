using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeDelivery.SignalR
{
    public class CoffeeDeliveryHub:Hub
    {
        private readonly IHubContext<CoffeeDeliveryHub> _context;
        public CoffeeDeliveryHub(IHubContext<CoffeeDeliveryHub> context)
        {
            _context = context;
        }
        public async Task SendMessageAsync(string message)
        {
            await _context.Clients.All.SendAsync(message);
        }
    }
}
