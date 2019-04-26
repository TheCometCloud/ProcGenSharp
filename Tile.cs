namespace ProcGenSharp
{
    public struct Tile
    {
        public int x;
        public int y;
        public char? value;

        public Tile(int x, int y, char? value = null)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }

        public override bool Equals(object o)
        {
            if (o is Tile)
            {
                Tile t = (Tile) o;
                return this == t;
            }
            
            return false;
        }

        public override int GetHashCode() 
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ value.GetHashCode();
        }

        public static bool operator ==(Tile a, Tile b)
        {
            return (a.x == b.x && a.y == b.y);
        }

        public static bool operator !=(Tile a, Tile b)
        {
            return (a.x != b.x || a.y != b.y);
        }
    }
}