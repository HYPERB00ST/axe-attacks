using System.Collections;
using System.Collections.Generic;
using DungeonMaker;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace NPCMaker {
    public class NPCSpawner
    {
        private static int npcCounter = 0;
        public static void GenerateNpcs(List<Room> rooms, PrefabLoader prefabs)
        {
            List<NPC> CurrentRoomNpcs;
            
            foreach (Room room in rooms) {
                
                // Skip starting room
                if (room.Id == 0) {
                    room.IsEmpty = true;
                    continue;
                }

                // Randomize empty rooms
                if (Random.Range(0f, 1f) > 0.5f) {
                    room.IsEmpty = true;
                    continue;
                }
                room.IsEmpty = false;

                // Get spawn points for this room
                SpawnPoint[] spawnPoints = MakeRoomSpawnPoints(room);

                // Make NPCs for this room
                CurrentRoomNpcs = CreateNpcs(room, spawnPoints);
                
                // Spawn Npcs in their respective rooms based on room IDs
                SpawnNpcs(CurrentRoomNpcs, prefabs);

                // Prepare for next room
                // CurrentRoomNpcs.Clear();
            }
        }

        private static SpawnPoint[] MakeRoomSpawnPoints(Room room)
        {
            // Room spawn area
            float spawnRoomLength = room.Length - 2;
            float spawnRoomWidth = room.Width - 2;

            // Total amount of spawn points in this room
            int spawnPointsAmount = (int)(spawnRoomLength/2 * spawnRoomWidth/2);
            SpawnPoint[] spawnPoints = new SpawnPoint[spawnPointsAmount];

            // Spawn point counter
            int counter = 0;

            for (int i = 0; i < spawnRoomLength; i+=2) {
                
                float localCoordZ = -spawnRoomLength/2 + i + 1;  
                float globalCoordZ = localCoordZ + room.Z;
                
                for (int j = 0; j < spawnRoomWidth; j+=2) {

                    float localCoordX = -spawnRoomWidth/2 + j + 1;
                    float globalCoordX = localCoordX + room.X;

                    SpawnPoint spawnPoint = new SpawnPoint(counter, globalCoordX, globalCoordZ);
                    
                    spawnPoints[counter] = spawnPoint;
                    
                    counter++;
                }
            }

            return spawnPoints;
        }

        private static void SpawnNpcs(List<NPC> currentRoomNpcs, PrefabLoader prefabs)
        {
            // Iterate over all the NPCs in current room to spawn
            for (int i = 0; i < currentRoomNpcs.Count; i++) {
                
                // NPC we are currently instatiating
                NPC npc = currentRoomNpcs[i];
                
                // Instantiate the NPC
                Object instantiated = Object.Instantiate(prefabs.NPC, new Vector3(npc.X, npc.Y, npc.Z), 
                    Quaternion.identity, prefabs.NPCparent);

                // Change the name of NPC
                instantiated.name = npc.Id.ToString();
            }
        }

        private static List<NPC> CreateNpcs(Room room, SpawnPoint[] spawnPoints)
        {
            // Amount of NPCs per room based on room type
            int NpcsToSpawn = SetAmountOfNpcs(room.Type);
            
            // NPC we're making
            NPC CurrentNpc;
            
            // List of NPCs we've made
            List<NPC> Npcs = new(NpcsToSpawn);

            // Create NPCs
            for (int i = 0; i < NpcsToSpawn; i++) {
                CurrentNpc = MakeNpc(room, spawnPoints);
                
                CurrentNpc.Id = npcCounter;
                npcCounter++;
                
                CurrentNpc.RoomId = room.Id;
                
                Npcs.Add(CurrentNpc);
                NPCInfo.NpcsAll.Add(CurrentNpc);
            }
            
            return Npcs;
        }

        private static NPC MakeNpc(Room room, SpawnPoint[] spawnPoints)
        {
            NPC Npc = new();
            SpawnPoint spawnPoint = spawnPoints[GetRandomFreeSpawnPoint(spawnPoints)];
            spawnPoints[spawnPoint.Id].Occupied = true;
            
            SetNpcCoords(Npc, room, spawnPoint);
            
            return Npc;
        }

        // TODO: Handle infinite recursion
        private static int GetRandomFreeSpawnPoint(SpawnPoint[] spawnPoints)
        {
            int randomSpawnPointId = Random.Range(0, spawnPoints.Length);
            
            if (spawnPoints[randomSpawnPointId].Occupied) {
                randomSpawnPointId = GetRandomFreeSpawnPoint(spawnPoints);
            }
            
            return randomSpawnPointId;
        }

        private static void SetNpcCoords(NPC npc, Room room, SpawnPoint spawnPoint)
        {
            npc.X = spawnPoint.X;
            npc.Z = spawnPoint.Z;
            
            npc.Y = room.Y + 2f;
        }

        private static int SetAmountOfNpcs(string roomType)
        {
            if (roomType == "bigRoom") {
                return Random.Range(1, NPCInfo.MAX_NPCS_BIGROOM);
            }
            else if (roomType == "smallRoom") {
                return Random.Range(1, NPCInfo.MAX_NPCS_SMALLROOM);
            }
            else return 0; // Dont spawn anything in corridors, ramps
        }
    }

}

