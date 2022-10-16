namespace Sakiy.Type
{
    public struct Identifier
    {
        public readonly string Space;
        public readonly string Name;
        public Identifier(string name)
        {
            if (!name.Contains(':'))
            {
                Space = "minecraft";
                Name = name;
            }
            else
            {
                Space = name.Split(':', 2).First();
                Name = name.Split(':', 2).Last();
            }
        }
        public Identifier(string space, string name)
        {
            Space = space;
            Name = name;
        }
        public override string ToString()
        {
            return Space + ":" + Name;
        }
    }
}
