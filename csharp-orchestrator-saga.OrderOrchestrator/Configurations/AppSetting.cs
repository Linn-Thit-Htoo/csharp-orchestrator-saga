﻿namespace csharp_orchestrator_saga.OrderOrchestrator.Configurations
{
    public class AppSetting
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
    }

    public class Connectionstrings
    {
        public string DbConnection { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }
}
