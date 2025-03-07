﻿using System;
using System.Collections.Generic;

namespace csharp_orchestrator_saga.OrderOrchestrator.Entities;

/// <summary>
/// auth: stores authenticator method reference claims for multi factor authentication
/// </summary>
public partial class MfaAmrClaim
{
    public Guid SessionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string AuthenticationMethod { get; set; } = null!;

    public Guid Id { get; set; }

    public virtual Session Session { get; set; } = null!;
}
