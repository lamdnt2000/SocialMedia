﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.SubscriptionModel
{
    public class UpdateSubscriptionDto : InsertSubscriptionDto
    {
        public int Status { get; set; }
    }
}
