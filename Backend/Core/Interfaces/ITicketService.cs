using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface ITicketService
    {
        // עבור המשתמש הציבורי
         Task<Ticket> CreateTicketAsync(Ticket ticket); 
         Task<Ticket?> GetTicketByIdAsync(Guid id); 

        // עבור ממשק הניהול (Admin)
         Task<IEnumerable<Ticket>> GetAllTicketsAsync(); 
         Task UpdateTicketStatusAsync(Guid id, TicketStatus newStatus, string? resolution);
    }
}
