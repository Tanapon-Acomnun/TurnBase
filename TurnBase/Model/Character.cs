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
        public int MP { get; set; }
        public int Level { get; set; } = 1;
        public int EXP { get; set; } = 0;
        public int StatPoints { get; set; } = 3;

        public bool IsAlive => CurrentHP > 0;
        public bool IsGuarding { get; set; } = false;

        public int Potions { get; set; } = 3;

        public int Ethers { get; set; } = 2;
    }
}
