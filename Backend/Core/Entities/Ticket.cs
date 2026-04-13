using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Enums;

namespace Core.Entities
{
    public class Ticket
    {
        // מזהה ייחודי מסוג GUID כפי שנדרש בדרישות [cite: 27]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 

       
        public string? ImageUrl { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.New;

       // בונוס: תקציר שנוצר על ידי AI [cite: 10, 22, 31]
        public string? AiSummary { get; set; }

        // טקסט פתרון שהוזן על ידי מנהל [cite: 34, 44]
        public string? Resolution { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
