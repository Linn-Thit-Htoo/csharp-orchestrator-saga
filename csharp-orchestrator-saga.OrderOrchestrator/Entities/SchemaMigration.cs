using System;
using System.Collections.Generic;

namespace csharp_orchestrator_saga.OrderOrchestrator.Entities;

public partial class SchemaMigration
{
    public long Version { get; set; }

    public DateTime? InsertedAt { get; set; }
}
