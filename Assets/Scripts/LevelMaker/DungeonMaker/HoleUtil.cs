using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

namespace DungeonMaker {
    public class HoleUtil
    {
        public int SetCurrentHoleId(Room Room) {
            
            // Find first hole that's open in the given Room
            for (int i = 0; i < Room.Holes.Length; i++) {
                if (Room.Holes[i] == Room.Hole.Empty) {
                    return i; // Returns hole ID
                }
            }
            Debug.Log("Set Current Hole Id");
            return RoomInfo.ERROR_CODE; // TODO ERROR
        }

        public int SetCurrentRoomId(List<Room> Rooms) {
            
            // Find a Room with open holes / i = RoomInfo.CurrentRoomId
            for (int i = 0; i<Rooms.Count; i++) {
                if (Rooms[i].AmountOfHoles > 0) {
                    return i;
                }
            }
            Debug.Log("Set Current Room Id");
            return RoomInfo.ERROR_CODE;
        }

        // TODO: 2 methods
        public int SkipCurrentHole(Room room, int hole, int numberOfHoles)
        {
            room.Holes[hole] = Room.Hole.Skipped;
            room.AmountOfHoles--;
            return --numberOfHoles;
        }

        public void FillHole(Room room, int hole, GameObject fullHoleFiller, Transform fillerParent)
        {
            Vector3 roomPos = new(room.X, room.Y, room.Z);

            UnityEngine.Object.Instantiate(fullHoleFiller,
                roomPos + RoomInfo.FillerCoords[room.Type][hole],
                Quaternion.AngleAxis(RoomInfo.FillerRotations[hole], Vector3.up), fillerParent);

            room.Holes[hole] = Room.Hole.Filled;
        }

        public int FillRemainingHoles(List<Room> rooms, GameObject fullHoleFiller, Transform fillerParent)
        {
            // TODO
            // Iterate over all rooms
            for (int i = 0; i < rooms.Count; i++)
            {
                // Find skipped holes
                for (int j = 0; j < rooms[i].Holes.Length; j++)
                {
                    if (rooms[i].Holes[j] == Room.Hole.Skipped || rooms[i].Holes[j] == Room.Hole.Empty)
                    {
                        Vector3 roomPos = new Vector3(rooms[i].X, rooms[i].Y, rooms[i].Z);
                        
                        // Instantiate FullHoleFillers
                        UnityEngine.Object.Instantiate(fullHoleFiller,
                            roomPos + RoomInfo.FillerCoords[rooms[i].Type][j],
                            Quaternion.AngleAxis(RoomInfo.FillerRotations[j], 
                            Vector3.up), fillerParent);
                        
                        // Set filled Skipped hole to Filled
                        rooms[i].Holes[j] = Room.Hole.Filled;
                        rooms[i].AmountOfHoles--;
                        
                        RoomInfo.NumberOfHoles--;
                    }
                }
            }
            return -1;
        }

        public int SetHoles(Room Room)
        {   
            // DO NOT DELETE / TODO: Use for getting potential multiple room openings

            // Checking coordinates of potentially already existing Rooms
            // for (int i=0; i<Rooms.Count; i++) {
            //     if (Rooms[i].Z == z + Rooms[RoomInfo.CurrentRoomId].Length/2 && Rooms[i].X == x) { // Top hole
            //         Room.Holes[0] = Room.Hole.Filled;
            //         RoomInfo.NewRoomHoleCount--;
            //     }
            //     else if (Rooms[i].Z == z - Rooms[RoomInfo.CurrentRoomId].Length/2 && Rooms[i].X == x) { // Bottom hole
            //         Room.Holes[3] = Room.Hole.Filled;
            //         RoomInfo.NewRoomHoleCount--;
            //     }
            //     else if (Rooms[i].X == x + Rooms[RoomInfo.CurrentRoomId].Width/2 && Rooms[i].Z == z) { // Right Hole
            //         Room.Holes[2] = Room.Hole.Filled;
            //         RoomInfo.NewRoomHoleCount--;
            //     }
            //     else if (Rooms[i].X == x - Rooms[RoomInfo.CurrentRoomId].Width/2 && Rooms[i].Z == z) { // Left hole
            //         Room.Holes[1] = Room.Hole.Filled;
            //         RoomInfo.NewRoomHoleCount--;
            //     }
            // }

            Room.AmountOfHoles = RoomInfo.NewRoomHoleCount;

            return Room.AmountOfHoles; // Returns the amount of holes left
        }

        public int ResetSkippedHoles(List<Room> rooms)
        {
            int holes = 0;
            for (int i=0; i<rooms.Count; i++) {
                for (int j=0; j<RoomInfo.MAX_HOLE_COUNT; j++) { // TODO: Update when implementing corridors
                    if (rooms[i].Holes[j] == Room.Hole.Skipped) {
                        rooms[i].Holes[j] = Room.Hole.Empty;
                        rooms[i].AmountOfHoles++;
                        holes++;
                    }
                }
            }
            return holes;
        }

        public int OccupyCurrentHole(Room room, int currentHoleId, int numberOfHoles)
        {
            room.Holes[currentHoleId] = Room.Hole.Occupied;
            room.AmountOfHoles--;
            return --numberOfHoles;
        }

        public int GetOppositeHole(int hole) {
            if (hole == 0) return 3; // 0 - Top / 3 - Bottom
            else if (hole == 1) return 2; // 1 - Left / 2 Right
            else if (hole == 2) return 1;
            else if (hole == 3) return 0;
            
            else return RoomInfo.ERROR_CODE;
        }

        public float GetCurrent2dCoordinate(char axis, Room room, int hole, bool check) {
            float x = 0; 
            float z = 0;
            
            switch (hole) // Set developing Room coordinates TODO: add y axis
            {
                case 0: // Top hole - z + Length/2
                    if (check) { z += 0.5f; } // TODO: Add 0.5f to RoomInfo as a constant
                    x += room.X;
                    z += room.Z + room.Length/2;
                    break;
                case 1: // Left hole - x - Width/2
                    if (check) { x -= 0.5f; }
                    x += room.X - room.Width/2;
                    z = room.Z;
                    break;
                case 2: // Right hole - x + Width/2
                    if (check) { x += 0.5f; }
                    x += room.X + room.Width/2;
                    z = room.Z;
                    break;
                case 3: // Bottom hole - z - Length/2
                    if (check) { z -= 0.5f; }
                    x = room.X;
                    z += room.Z - room.Length/2;
                    break;
                default: // shouldn't happen / TODO: when implementing height difference
                    Debug.Log("DEFAULT HoleUtil: 174");
                    x = room.X;
                    z = room.Z;
                    break;
            }
            return axis == 'x' ? x : z;
        }

        public bool OccupyCheck(Room room, int holeid, LayerMask mask, float radius)
        {
            Vector3 Position = new()
            {
                x = GetCurrent2dCoordinate('x', room, holeid, true),
                z = GetCurrent2dCoordinate('z', room, holeid, true),
                y = room.Y

            };
            bool full = Physics.CheckSphere(Position, radius, mask);
            return full;     
        }
    }
}
