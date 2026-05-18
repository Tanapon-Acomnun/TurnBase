using System.Collections.Generic;

namespace TurnBase.Model
{
    public class SaveData
    {
        public string SelectedName { get; set; }
        public string Sprite { get; set; }
        public int CurrentStage { get; set; }
        public int BattleNumber { get; set; }
        public int TotalCurrency { get; set; }
        public int MaxHP { get; set; }
        public int Attack { get; set; }
        public int MaxMP { get; set; } // สำคัญมาก ป้องกันบั๊กตีสกิลดาเมจ 0          
        public int Potions { get; set; }
        public int Ethers { get; set; } 

        // เก็บแค่ "ชื่อ" ของสกิล
        public List<string> UnlockedSkills { get; set; } = new();
        public List<string> AvailableUnlockableSkills { get; set; } = new();
    }
}