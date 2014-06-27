using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BlockingCollectionDemo.ConsoleUI
{
    public class Pulse
    {
        public int Pin { get; set; }
        public DateTime Timestamp { get; set; }

        public Pulse(int pin, DateTime timestamp)
        {
            Pin = pin;
            Timestamp = timestamp;
        }
    }
}
