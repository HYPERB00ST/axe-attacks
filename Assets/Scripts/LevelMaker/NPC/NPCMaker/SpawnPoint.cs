using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCMaker {
    struct SpawnPoint {
        public int Id {get;} // ID based on amount of spawn points per room (local to each room)
        public float X {get;} // Local position in a room relative to room's center (0, 0)
        public float Z {get;}

        public bool Occupied {get; set;}

        public SpawnPoint(int id, float x, float z) {
            Id = id;
            X = x;
            Z = z;
            Occupied = false;
        }
    }
}