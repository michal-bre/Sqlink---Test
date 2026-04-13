using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using Core.Entities;

namespace Services.Data
{
    public class JsonTicketRepository
    {
        private readonly string _filePath = "tickets.json";

        public async Task<List<Ticket>> ReadAllAsync()
        {
            if (!File.Exists(_filePath)) return new List<Ticket>();
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Ticket>>(json) ?? new List<Ticket>();
        }

        public async Task WriteAllAsync(List<Ticket> tickets)
        {
            var json = JsonSerializer.Serialize(tickets, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
