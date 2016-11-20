namespace BattleShip
{
    public class Point
    {
        public int x;
        public int y;
        public Ship ship;
        public Point(int x, int y, Ship ship)
        {
            this.x = x;
            this.y = y;
            this.ship = ship;
        }
    }
}