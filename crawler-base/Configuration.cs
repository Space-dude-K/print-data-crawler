﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace crawler_base
{
    public class MyConfiguration : IMyConfiguration
    {

        IConfigurationRoot _configurationRoot;
        public MyConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public string YourItem => _configurationRoot["YourItem"];
    }


    public interface IMyConfiguration
    {
        string YourItem { get; }
    }
}
