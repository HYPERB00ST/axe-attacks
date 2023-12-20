using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DungeonMaker {
    public class RoomUtil
    {
        public Room SetDevelopingRoom(Dictionary<string, bool> settings)
        {
            Room DevelopingRoom = new()
            {
                Id = RoomInfo.NextRoomId,
                Type = settings["bigRoom"] ? "bigRoom" : "smallRoom"
            };
            //settings["elevation"] ? GetElevationAxis(settings["elevationType"]) : 0f;

            RoomInfo.NextRoomId++;

            return DevelopingRoom;
        }

        public float GetElevationAxis(bool v)
        {
            throw new NotImplementedException();
        }

        // Summary:
        // 0 - X axis, 1 - Z axis
        public float GetNext2dRoomCoordinate(char axis, Room currentRoom, Room nextRoom, int hole) {
            float x = 0; 
            float z = 0;
            
            switch (hole) // Set developing Room coordinates TODO: add y axis
            {
                case 0: // Top hole - z + Length/2
                    x += currentRoom.X;
                    z += currentRoom.Z + currentRoom.Length/2
                    + nextRoom.Length/2;
                    break;
                case 1: // Left hole - x - Width/2
                    x += currentRoom.X - currentRoom.Width/2
                    - nextRoom.Width/2;
                    z = currentRoom.Z;
                    break;
                case 2: // Right hole - x + Width/2
                    x += currentRoom.X + currentRoom.Width/2
                    + nextRoom.Width/2;
                    z = currentRoom.Z;
                    break;
                case 3: // Bottom hole - z - Length/2
                    x = currentRoom.X;
                    z += currentRoom.Z - currentRoom.Length/2
                    - nextRoom.Length/2;
                    break;
                default: // shouldn't happen / TODO: when implementing height difference
                    x = currentRoom.X;
                    z = currentRoom.Z;
                    break;
            }
            return axis == 'x' ? x : z;
        }

        public Dictionary<string, bool> SetDevelopingRoomSettings() // Expand if adding new settings
        {
            Dictionary<string, bool> settings = new()
            {
                ["elevation"] = UnityEngine.Random.Range(0f, 1f) >= 0.7f, // Elevation diff between current & developing room
                ["higherElevation"] = UnityEngine.Random.Range(0f, 1f) >= 0.5f, // true higher / false lower

                ["bigRoom"] = UnityEngine.Random.Range(0f, 1f) >= 0.5f, // Small room / Big room
                ["pillar"] = UnityEngine.Random.Range(0f, 1f) >= 0.7f // Spawn pillar on entrance to the room
            };

            return settings;
        }

        public Room CreateInitialRoomInfo()
        {
            Room Room = new() {
                Id = 0,
                X = 0,
                Y = 0,
                Z = 0,
                AmountOfHoles = RoomInfo.MAX_HOLE_COUNT,
                Type = "smallRoom"
            };
            for (int i = 0; i < Room.Holes.Length; i++)
            {
                Room.Holes[i] = Room.Hole.Empty;
            };
            
            return Room;
        }

        public bool RoomCheck(Room room, int currentHoleId, LayerMask mask, float radius)
        {
            Room tempRoom = new()
            {
                Width = RoomInfo.PrefabDimensions["bigRoomWidth"],
                Length = RoomInfo.PrefabDimensions["bigRoomLength"]
            };

            Vector3 Position = new()
            {
                x = GetNext2dRoomCoordinate('x', room, tempRoom, currentHoleId),
                z = GetNext2dRoomCoordinate('z', room, tempRoom, currentHoleId),
                y = room.Y

            };
            bool full = Physics.CheckSphere(Position, radius, mask);
            return full;            
        }
    }
}