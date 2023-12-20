using UnityEngine;
using DungeonMaker;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections.Generic;
using System;

namespace LevelMaker {
    class LevelMakerController : MonoBehaviour {
    [SerializeField] int numberOfRoomsToMake;
    [SerializeField] LayerMask groundMask;
    [SerializeField] NavMeshSurface navMeshSurface;
    private LevelManagerScript managerScript;

        void Start() {
            managerScript = GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();
            numberOfRoomsToMake = 4 * managerScript.Level;
            SetNextLevel();
            
            PrefabLoader Prefabs = GetComponent<PrefabLoader>();

            // Create Dungeon (Environment) Generator
            DungeonGenerator dungeonGenerator = new();
            
            // Generate Rooms
            List<Room> Rooms = dungeonGenerator.GenerateDungeon(Prefabs, numberOfRoomsToMake, groundMask);

            // Bake Nav Mesh for given Rooms
            NavMeshControl.NavMeshController.BakeNavMeshLevel(navMeshSurface);

            // Spawn NPCs
            NPCMaker.NPCSpawner.GenerateNpcs(Rooms, Prefabs);
        }

        private void SetNextLevel()
        {
            managerScript.Level++;
        }
    }
}



