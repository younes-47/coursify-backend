﻿using MimeKit;
using MimeKit.Text;

namespace coursify_backend.DTO.INTERNAL
{
    public class EmailDTO
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
