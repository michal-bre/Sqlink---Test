using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Services.Data;

namespace Services
{
    public class TicketService : ITicketService
    {
        private readonly JsonTicketRepository _repository;
        private readonly EmailService _emailService;
        private readonly AiSummaryService _aiService;

        public TicketService(JsonTicketRepository repository, EmailService emailService, AiSummaryService aiService)
        {
            _repository = repository;
            _emailService = emailService;
            _aiService = aiService;
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
           // 1. קריאה לשירות ה-AI לקבלת תקציר לפני השמירה 
            ticket.AiSummary = await _aiService.GetSummaryAsync(ticket.Description);

            // 2. קריאה לרפוזיטורי לשמירת הנתונים בקובץ ה-JSON [cite: 7, 24]
            var tickets = await _repository.ReadAllAsync();
            tickets.Add(ticket);
            await _repository.WriteAllAsync(tickets);

            return ticket;
        }

        public async Task UpdateTicketStatusAsync(Guid id, TicketStatus newStatus, string? resolution)
        {
            var tickets = await _repository.ReadAllAsync();
            var ticket = tickets.FirstOrDefault(t => t.Id == id);

            if (ticket != null)
            {
                bool isStatusChanged = ticket.Status != newStatus;
                bool isResolutionChanged = ticket.Resolution != resolution;

                ticket.Status = newStatus;
                ticket.Resolution = resolution;
                ticket.UpdatedAt = DateTime.UtcNow;

                await _repository.WriteAllAsync(tickets);

                // שליחת מייל בכל שינוי סטטוס או טקסט פתרון [cite: 50, 51]
                if (isStatusChanged || isResolutionChanged)
                {
                    await _emailService.SendEmailAsync(ticket.Email,
                        "עדכון בטיקט שלך",
                        $"הסטטוס עודכן ל: {newStatus}. פתרון: {resolution}");
                }
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync() => await _repository.ReadAllAsync();

        public async Task<Ticket?> GetTicketByIdAsync(Guid id)
        {
            var tickets = await _repository.ReadAllAsync();
            return tickets.FirstOrDefault(t => t.Id == id);
        }
    }
}
