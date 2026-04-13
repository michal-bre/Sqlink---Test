using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum TicketStatus
    {
        New,          // טיקט שנפתח זה עתה
        InProgress,   // בטיפול
        Resolved,     // נפתר (ממתין לאישור)
        Closed        // סגור סופית
    }
}
