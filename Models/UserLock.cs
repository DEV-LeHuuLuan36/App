using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class UserLock
    {
        public string LockKey { get; set; } = null!;
        public string? TableName { get; set; }
        public string? RecordId { get; set; }
        public string? LockedBy { get; set; }
        public DateTime? LockTime { get; set; }
        public int? TimeoutMinutes { get; set; }
    }
}
