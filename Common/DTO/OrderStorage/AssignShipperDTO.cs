﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.OrderStorage
{
    public class AssignShipperDTO
    {
        public required Guid OrderStorageId { get; set; }
        public required Guid ShipperId { get; set; }
    }
}