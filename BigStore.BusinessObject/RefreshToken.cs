﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.BusinessObject
{
    public class RefreshToken
    {
        [Key]
        public Guid RefreshTokenId { get; set; }

        public string? UserId { get; set; }

        public string Token { get; set; } = null!;

        public string JwtId { get; set; } = null!;

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime? IssuedAt { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public virtual User? User { get; set; } = null!;
    }
}