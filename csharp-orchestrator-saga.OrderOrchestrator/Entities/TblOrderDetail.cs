﻿using System;
using System.Collections.Generic;

namespace csharp_orchestrator_saga.OrderOrchestrator.Entities;

public partial class TblOrderDetail
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public Guid InvoiceNo { get; set; }

    public long TotalItems { get; set; }

    public double SubTotal { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual TblOrder Order { get; set; } = null!;
}
