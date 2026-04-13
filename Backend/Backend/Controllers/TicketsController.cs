using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Backend.Dtos;
using Core.Enums;
using Core.Interfaces;
using Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly FileService _fileService;
        private readonly EmailService _emailService; // הזרקת שירות המייל

        public TicketsController(ITicketService ticketService, FileService fileService, EmailService emailService)
        {
            _ticketService = ticketService;
            _fileService = fileService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TicketCreateDto dto, IFormFile? image)
        {
            string? imageUrl = null;

            if (image != null && image.Length > 0)
            {
                imageUrl = await _fileService.SaveFileAsync(image.OpenReadStream(), image.FileName);
            }

            var ticket = new Ticket
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Description = dto.Description,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                Status = TicketStatus.New
            };

            var created = await _ticketService.CreateTicketAsync(ticket);

            // --- שליחת מייל עם לינק לחיץ ---
            try
            {
                // הכתובת של דף הסטטוס באנגולר (נבנה אותו עוד מעט)
                string trackingUrl = $"http://localhost:4200/ticket-status/{created.Id}";

                string emailBody = $@"
                    <div style='direction: rtl; font-family: Arial, sans-serif; text-align: right;'>
                        <h2 style='color: #2c3e50;'>שלום {created.FullName},</h2>
                        <p>הפנייה שלך בנושא: <b>{created.Description}</b> התקבלה במערכת.</p>
                        <p>תוכלי לעקוב אחר סטטוס הטיפול וסיכום ה-AI בקישור הבא:</p>
                        <p>
                            <a href='{trackingUrl}' 
                               style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                               לחצי כאן למעקב אחר הפנייה
                            </a>
                        </p>
                        <hr />
                        <p style='font-size: 0.8em; color: gray;'>מזהה פנייה: {created.Id}</p>
                    </div>";

                await _emailService.SendEmailAsync(created.Email, "טיקט חדש נפתח - אישור קבלה", emailBody);
            }
            catch (Exception ex)
            {
                // אם המייל נכשל, אנחנו לא רוצים להכשיל את כל היצירה, רק נתעד
                System.Diagnostics.Debug.WriteLine($"Failed to send email: {ex.Message}");
            }

            return Ok(created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto updateDto)
        {
            // 1. עדכון הסטטוס ב-DB
            await _ticketService.UpdateTicketStatusAsync(id, updateDto.Status, updateDto.Resolution);

            // 2. שליפת הטיקט המעודכן כדי לשלוח מייל עם הפרטים הנכונים
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();

            // 3. בניית הקישור והמייל
            string trackingUrl = $"http://localhost:4200/ticket-status/{id}";
            string emailBody = $@"
        <div style='direction: rtl; font-family: Arial;'>
            <h2>עדכון בטיקט שלך!</h2>
            <p>הסטטוס עודכן ל: <b>{ticket.Status}</b></p>
            <p>פתרון/הערות: {updateDto.Resolution}</p>
            <br>
            <a href='{trackingUrl}' 
               style='background-color: #28a745; color: white; padding: 10px; text-decoration: none; border-radius: 5px;'>
               לחצי כאן לצפייה בטיקט המלא
            </a>
        </div>";

            await _emailService.SendEmailAsync(ticket.Email, "עדכון בטיקט שלך - " + id, emailBody);

            return NoContent();
        }
    }

    public class UpdateTicketDto
    {
        public TicketStatus Status { get; set; }
        public string? Resolution { get; set; }
    }
}