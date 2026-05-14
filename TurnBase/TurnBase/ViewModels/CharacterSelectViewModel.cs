using System.Collections.ObjectModel;
using TurnBase.Model;

namespace TurnBase.ViewModels
{
    public class CharacterSelectViewModel
    {
        public ObservableCollection<CharacterClass> Classes { get; set; }

        public CharacterSelectViewModel()
        {
            Classes = new ObservableCollection<CharacterClass>
            {
                new CharacterClass
                {
                    Name = "Wizard",
                    MaxHP = 70,
                    Attack = 10,
                    MaxMP = 30
                },

                new CharacterClass
                {
                    Name = "Warrior",
                    MaxHP = 100,
                    Attack = 15,
                    MaxMP = 10
                },

                new CharacterClass
                {
                    Name = "Knight",
                    MaxHP = 120,
                    Attack = 12,
                    MaxMP = 15
                }
            };
        }
    }
}