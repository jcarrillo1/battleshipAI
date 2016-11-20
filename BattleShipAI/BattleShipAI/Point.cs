namespace BattleShip
{
    public class Point
    {
        public int x;
        public int y;
        public int originX;
        public int originY;
        public string shipName;
        public Point(int x, int y, string shipName, int orX, int orY)
        {
            this.x = x;
            this.y = y;
            this.shipName = shipName;
            originX = orX;
            originY = orY;
        }
    }
}