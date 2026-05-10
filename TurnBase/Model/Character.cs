using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBase.Model
{
    public class Character
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }

        public bool IsAlive => CurrentHP > 0;
    }
}
