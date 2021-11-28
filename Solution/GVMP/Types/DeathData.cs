using System;

namespace GVMP
{
    public class DeathData
    {
        public bool IsDead { get; set; } = false;
        public DateTime DeathTime { get; set; } = new DateTime(0);
        
        public DeathData() { }
    }
}