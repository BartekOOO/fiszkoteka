using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Models
{
    public sealed class RevokedToken
    {
        public int Id { get; set; }
        public string Jti { get; set; }
        public int UserId { get; set; }
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
