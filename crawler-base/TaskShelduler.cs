using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace crawler_base
{
    class TaskShelduler
    {
        private readonly ILogger<ConsoleApp> logger;

        public TaskShelduler(ILogger<ConsoleApp> logger)
        {
            this.logger = logger;
        }

    }
}
