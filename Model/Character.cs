using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TurnBase.Model
{
    public class Character : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string Name { get; set; }
        public string ID { get; set; }

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

        // ===== Potion =====
        private int potions = 35;
        public int Potions
        {
            get => potions;
            set
            {
                potions = value;
                OnPropertyChanged();
            }
        }

        // ===== Ether =====
        private int ethers = 35;
        public int Ethers
        {
            get => ethers;
            set
            {
                ethers = value;
                OnPropertyChanged();
            }
        }

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
        public bool IsShielded { get; set; }
        public bool ArcaneShieldActive { get; set; }
    }
}

