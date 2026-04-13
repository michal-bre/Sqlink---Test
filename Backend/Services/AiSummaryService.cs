using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class AiSummaryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        // עדכון ה-URL לגרסה שעובדת ב-Free Tier בוודאות
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent";
        public AiSummaryService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // שליפת המפתח - ודאי שב-appsettings זה תחת AiSettings -> GeminiApiKey
            _apiKey = configuration["AiSettings:GeminiApiKey"] ?? "";
        }

        public async Task<string> GetSummaryAsync(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return "תיאור ריק";

            if (string.IsNullOrWhiteSpace(_apiKey)) return "מפתח API חסר ב-Config";

            try
            {
                var requestBody = new
                {
                    contents = new[] {
                        new {
                            parts = new[] {
                                new { text = $"תמצת את תקלת המחשב הבאה במשפט אחד קצר בעברית: {description}" }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // ביצוע הקריאה
                var response = await _httpClient.PostAsync($"{ApiUrl}?key={_apiKey.Trim()}", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // זה ידפיס לחלונית ה-Output את השגיאה המדויקת אם זה שוב נכשל
                    System.Diagnostics.Debug.WriteLine($"GOOGLE API ERROR: {jsonResponse}");
                    return "לא ניתן לייצר תקציר כרגע";
                }

                using var doc = JsonDocument.Parse(jsonResponse);

                // שליפת הטקסט מתוך המבנה של Gemini
                var summary = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return summary?.Trim() ?? "לא התקבל תקציר";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}");
                return "שגיאה בתהליך הסיכום";
            }
        }
    }
}