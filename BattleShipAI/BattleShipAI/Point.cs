namespace BattleShip
{
    public class Point
    {
        public int x;
        public int y;
        public Ship ship;
        public string shipName;
        public Point(int x, int y, string shipName)
        {
            this.x = x;
            this.y = y;
            this.shipName = shipName;
        }
    }
}