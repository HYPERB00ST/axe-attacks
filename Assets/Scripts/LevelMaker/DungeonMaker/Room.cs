using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMaker {
    public class Room
    {
        public string Type {get; set;} // room, corridor or ramp
        public int Id {get; set;}
        public float X {get; set;}
        public float Y {get; set;}
        public float Z {get; set;}
        public int AmountOfHoles {get; set;}

        public enum Hole
        {
            Empty,
            Occupied,
            Skipped,
            Filled
        }
        public Hole[] Holes = new Hole[RoomInfo.MAX_HOLE_COUNT];

        public float Width {get; set;} = RoomInfo.PrefabDimensions["smallRoomWidth"];
        public float Length {get; set;} = RoomInfo.PrefabDimensions["smallRoomLength"];
        public bool IsEmpty {get; set;}
    }
}
