namespace TurnBase.Model
{
    public class Skill
    {
        public string Name { get; set; }
        public string Dname { get; set; }
        public string SkillType { get; set; }
        public int MPCost { get; set; }
        public int Power { get; set; }
        public string Effect { get; set; }
        public string Description { get; set; }
        public int SelfDamage { get; set; }
        public int Duration { get; set; }
        public List<string> ExtraEffects { get; set; } = new();
        public List<Skill> UnlockableSkills { get; set; } = new();
        public int UnlockCost { get; set; } = 5;

    }
}