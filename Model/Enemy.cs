using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBase.Model
{
    public class Enemy : Character
    {
        public string Sprite { get; set; }
        public string EnemyType { get; set; }
        public bool IsGuarding { get; set; } = false;
        public List<string> StatusEffects { get; set; } = new();
    }
}
