using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DungeonMaker {
    public class DungeonGenerator
    {
        // Initialize Room & hole methods
        PrefabLoader Prefabs;
        readonly List<Room> Rooms = new();
        readonly RoomUtil RoomUtil = new();
        readonly HoleUtil HoleUtil = new();
        int numberOfRoomsToMake;
        LayerMask groundMask;
        
        public List<Room> GenerateDungeon(PrefabLoader prefabs, int roomsToMake, LayerMask mask) {

            // Debug Reset
            ResetPrevGeneration();

            // Assign params
            Prefabs = prefabs;
            numberOfRoomsToMake = roomsToMake;
            groundMask = mask;

            // Create info for initial Room
            Rooms.Add(RoomUtil.CreateInitialRoomInfo());

            while (numberOfRoomsToMake > -1) {
                
                if (RoomInfo.NumberOfHoles <= 0 && numberOfRoomsToMake > 0) {

                    // Reset Skipped Holes if we have more rooms to make
                    RoomInfo.NumberOfHoles = HoleUtil.ResetSkippedHoles(Rooms);
                    ResetCurrentCounters();
                    IncreaseRoomChance();
                    continue;
                }

                RoomInfo.CurrentRoomId = HoleUtil.SetCurrentRoomId(Rooms);
                RoomInfo.CurrentHoleId = HoleUtil.SetCurrentHoleId(Rooms[RoomInfo.CurrentRoomId]);
                
                if (RoomInfo.NumberOfHoles > 0 && numberOfRoomsToMake == 0) {
                    RoomInfo.NumberOfHoles = HoleUtil.FillRemainingHoles(Rooms, Prefabs.FullHoleFiller,
                        Prefabs.HoleFillerParent);
                    break;
                }
                if (Random.Range(0f, 1f) >= RoomInfo.RoomChance) { // 50% chance for a Room to be made
                    MakeARoom();
                }
                else if (RoomInfo.NumberOfHoles == 1) { // If its a last hole make a Room
                    MakeARoom();
                }
                else {
                    RoomInfo.NumberOfHoles = HoleUtil.SkipCurrentHole(Rooms[RoomInfo.CurrentRoomId],
                        RoomInfo.CurrentHoleId, RoomInfo.NumberOfHoles); // Skip hole
                }
            }

            SpawnPortal();

            DestroyRoomColliders();
            // Next iteration of dungeon is bigger / TODO: Implement complexity by level

            return Rooms;
        }

        private void ResetPrevGeneration()
        {
            Rooms.Clear();

            // Clear RoomInfo
            RoomInfo.RoomChance = 0.5f;
            RoomInfo.NumberOfHoles = 4;
            RoomInfo.CurrentRoomId = 0;
            RoomInfo.CurrentHoleId = 0;
            RoomInfo.NextRoomId = 1;
        }

        private void SpawnPortal()
        {
            Vector3 position = new Vector3(Rooms[Rooms.Count - 1].X, Rooms[Rooms.Count - 1].Y, Rooms[Rooms.Count - 1].Z);
            Object.Instantiate(Prefabs.VictoryPortal, position, Quaternion.identity);
        }

        private void DestroyRoomColliders()
        {
            BoxCollider[] colliders = Prefabs.RoomParent.GetComponentsInChildren<BoxCollider>();
            
            for (int i=0; i<colliders.Length; i++) {
                if (colliders[i].gameObject.CompareTag("Room")) {
                    Object.Destroy(colliders[i]);
                }
            }
        }

        private void IncreaseRoomChance()
        {
            RoomInfo.RoomChance -= 0.1f;
        }

        private void ResetCurrentCounters()
        {
            RoomInfo.CurrentHoleId = 0;
            RoomInfo.CurrentRoomId = 0;
        }

        private void MakeARoom() // Make a Room
        {
            // Last check if there is no other room on the given position
            bool toFill = RoomUtil.RoomCheck(Rooms[RoomInfo.CurrentRoomId], RoomInfo.CurrentHoleId,
                groundMask, RoomInfo.ROOM_CHECK_RADIUS);
            
            if (toFill) {
                
                HoleUtil.FillHole(Rooms[RoomInfo.CurrentRoomId], RoomInfo.CurrentHoleId,
                    Prefabs.FullHoleFiller, Prefabs.HoleFillerParent);

                Rooms[RoomInfo.CurrentRoomId].AmountOfHoles--;
                RoomInfo.NumberOfHoles--;

                return;
            }

            bool toOccupy = HoleUtil.OccupyCheck(Rooms[RoomInfo.CurrentRoomId], RoomInfo.CurrentHoleId,
                groundMask, RoomInfo.OCCUPY_CHECK_RADIUS);
            
            if (toOccupy) {
                RoomInfo.NumberOfHoles = HoleUtil.OccupyCurrentHole(Rooms[RoomInfo.CurrentRoomId], 
                    RoomInfo.CurrentHoleId, RoomInfo.NumberOfHoles);
                    
                    return;
            }
            
            // If checks passed, start creating room settings
            Dictionary<string, bool> RoomSettings = RoomUtil.SetDevelopingRoomSettings();
            
            // Relative to current Room & hole we are currently checking, 
            // new Room's coordinates are calculated
            Room Room = RoomUtil.SetDevelopingRoom(RoomSettings);
            
            SetRoomDimensions(Room);
            SetRoom2dCoords(Room); // Order of method execution is important

            RoomInfo.NumberOfHoles += HoleUtil.SetHoles(Room);

            // Set current hole to occupied
            RoomInfo.NumberOfHoles = HoleUtil.OccupyCurrentHole(Rooms[RoomInfo.CurrentRoomId],
                RoomInfo.CurrentHoleId, RoomInfo.NumberOfHoles); // Parent room

            RoomInfo.NumberOfHoles = HoleUtil.OccupyCurrentHole(Room,
                HoleUtil.GetOppositeHole(RoomInfo.CurrentHoleId), RoomInfo.NumberOfHoles); // Developing Room

            HandleHoles(Room);

            if (RoomSettings["bigRoom"]) {
                InstantiateBigRoom(Room);
            }
            else {
                InstantiateSmallRoom(Room);
            }
            numberOfRoomsToMake--;
        }

        private void HandleHoles(Room room) 
        {
            HandleOccupiedHoles(room);
            HandleFilledHoles(room);
        }

        private void HandleOccupiedHoles(Room room) 
        {
            // Instanciate a very small sphere checker right in from of a given room,
            // to see if another room is connected
            bool isColliding;
            Vector3 Position = new Vector3(room.X, room.Y, room.Z);

            for (int i = 0; i < room.Holes.Length; i++)
            {
                Position.x = HoleUtil.GetCurrent2dCoordinate('x', room, i, true);
                Position.z = HoleUtil.GetCurrent2dCoordinate('z', room, i, true);

                isColliding = Physics.CheckSphere(Position, 0.3f, groundMask);
                
                if (isColliding) {
                    
                    // Occupy current hole
                    if (room.Holes[i] == Room.Hole.Empty || room.Holes[i] == Room.Hole.Skipped) {
                        RoomInfo.NumberOfHoles = HoleUtil.OccupyCurrentHole(room, i, RoomInfo.NumberOfHoles);
                    }
                }
            }
            
        }

        private void HandleFilledHoles(Room room)
        {
            // Instanciate collider to the position of the hole we are checking (big room checker)
            // if any collision set hole to false
            bool isColliding;
            Room tempRoom = new()
            {
                Width = RoomInfo.PrefabDimensions["bigRoomWidth"],
                Length = RoomInfo.PrefabDimensions["bigRoomLength"]
            };
            
            Vector3 Position = new()
            {
                y = room.Y + 2
            };
            
            for (int i = 0; i < room.Holes.Length; i++)
            {   
                Position.x = RoomUtil.GetNext2dRoomCoordinate('x', room, tempRoom, i);
                Position.z = RoomUtil.GetNext2dRoomCoordinate('z', room, tempRoom, i);
                
                isColliding = Physics.CheckSphere(Position, 
                    RoomInfo.PrefabDimensions["bigRoomWidth"]/2, groundMask);

                if (isColliding) {
                    // Fill current hole
                    if (room.Holes[i] == Room.Hole.Empty) {
                        HoleUtil.FillHole(room, i, Prefabs.FullHoleFiller, Prefabs.HoleFillerParent);

                        --room.AmountOfHoles;
                        --RoomInfo.NumberOfHoles;
                    }
                }
            }
        }

        private void SetRoom2dCoords(Room Room)
        {
            Room.Y = 0f; // TODO: height difference
            Room.X = RoomUtil.GetNext2dRoomCoordinate('x', Rooms[RoomInfo.CurrentRoomId], 
                Room, RoomInfo.CurrentHoleId);
            Room.Z = RoomUtil.GetNext2dRoomCoordinate('z', Rooms[RoomInfo.CurrentRoomId],
                Room, RoomInfo.CurrentHoleId);
        }

        private void InstantiateBigRoom(Room Room) {
            GameObject newRoom = Object.Instantiate(Prefabs.BigRoomPrefab, new Vector3(Room.X, Room.Y, Room.Z),
                Quaternion.identity, Prefabs.RoomParent);

            newRoom.name = Rooms.Count.ToString();
            Rooms.Add(Room);
        }

        private void InstantiateSmallRoom(Room Room) {
            GameObject newRoom = Object.Instantiate(Prefabs.SmallRoomPrefab, new Vector3(Room.X, Room.Y, Room.Z),
                Quaternion.identity, Prefabs.RoomParent);
            
            newRoom.name = Rooms.Count.ToString();
            Rooms.Add(Room);
        }

        private void SetRoomDimensions(Room Room)
        {
            // TODO: add corridors & ramps
            if (Room.Type == "bigRoom") {
                Room.Width = RoomInfo.PrefabDimensions["bigRoomWidth"];
                Room.Length = RoomInfo.PrefabDimensions["bigRoomLength"];
            }
            else if (Room.Type == "smallRoom") {
                Room.Width = RoomInfo.PrefabDimensions["smallRoomWidth"];
                Room.Length = RoomInfo.PrefabDimensions["smallRoomLength"];
            }
        }
    }
}
