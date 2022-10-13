namespace Sakiy.Util
{
    public struct Identification
    {
        public string Space;
        public string Name;
        public Identification(string name)
        {
            Space = "minecraft";
            Name = name;
        }
        public Identification(string space, string name)
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
