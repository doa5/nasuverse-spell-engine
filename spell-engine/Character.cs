namespace NasuverseSpellEngine
{
    public class Character
    {
        public string Name { get; }
        public ResourcePool Resources { get; }

        public Character(string name, ResourcePool resources)
        {
            Name = name;
            Resources = resources;
        }
    }
}
