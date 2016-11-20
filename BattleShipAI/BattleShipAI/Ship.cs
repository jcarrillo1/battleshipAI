using BattleShip;
using System;
using System.Collections.Generic;

namespace BattleShip
{
    class Ship
    {
        public int[] size;
        public Tuple<int, int, int, int> loc;
        public string name;
        public bool sunk = false;
        public bool placed = false;

        public Ship(int sz, string nme)
        {
            size = new int[sz];
            name = nme;
        }

        public void SetLocation(int x, int y, int x2, int y2)
        {
            loc = new Tuple<int, int, int, int>(x, y, x2, y2);
        }
    }
}
