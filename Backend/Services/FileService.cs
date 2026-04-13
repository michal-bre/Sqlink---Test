using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileService
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            // יצירת התיקייה אם היא לא קיימת
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);

            // יצירת שם ייחודי למניעת דריסת קבצים
            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = Path.Combine(_storagePath, uniqueName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return $"/uploads/{uniqueName}"; // מחזירים נתיב יחסי
        }
    }
}
