﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scambio.Logic
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string AvatarLocation { get; set; }
        public string OriginalAvatarLocation { get; set; }
    }
}
