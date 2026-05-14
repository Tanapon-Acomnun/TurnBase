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
        public int MaxMP { get; set; }
        public int Level { get; set; } = 1;
        public int EXP { get; set; } = 0;
        public int Currency { get; set; } = 0;
        public bool IsAlive => CurrentHP > 0;
        public bool IsGuarding { get; set; } = false;
        public int Potions { get; set; } = 3;
        public int Ethers { get; set; } = 2;
        public string Sprite { get; set; }
        public int BattleNumber { get; set; } = 1;
        public Dictionary<string, int> StatusEffects
        { get; set; } = new();
        public int AttackModifier { get; set; } = 0;
        public int DefenseModifier { get; set; } = 0;
        public List<Skill> Skills { get; set; } = new();
        public List<Skill> UnlockableSkills { get; set; } = new();
        public int Stage { get; set; } = 0;
        public bool HasLifesteal { get; set; } = false;
        public bool CounterStanceActive { get; set; }
    }
}
