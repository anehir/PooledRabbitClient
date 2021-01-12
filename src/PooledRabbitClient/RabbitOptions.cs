using System;
using System.Collections.Generic;
using System.Text;

namespace PooledRabbitClient
{
    public class RabbitOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; } = 5672;
        public string VHost { get; set; } = "/";
        public int MaxChannelCount { get; set; } = Environment.ProcessorCount * 2;

        public void CopyTo(RabbitOptions otherOptions)
        {
            otherOptions.UserName = this.UserName;
            otherOptions.Password = this.Password;
            otherOptions.HostName = this.HostName;
            otherOptions.Port = this.Port;
            otherOptions.VHost = this.VHost;
            otherOptions.MaxChannelCount = this.MaxChannelCount;
        }
    }
}
