﻿using System;
using System.Collections.Generic;

namespace csharp_orchestrator_saga.OrderOrchestrator.Entities;

public partial class TblProduct
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public double Price { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<TblStock> TblStocks { get; set; } = new List<TblStock>();
}
