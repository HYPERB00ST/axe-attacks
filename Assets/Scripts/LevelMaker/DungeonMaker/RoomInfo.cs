using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonMaker {
    public class RoomInfo
    {
        private RoomInfo() {}
        public static readonly Dictionary<string, float> PrefabDimensions = new() {
            ["bigRoomLength"] = 10f,
            ["bigRoomWidth"] = 10f,
            ["smallRoomLength"] = 8f,
            ["smallRoomWidth"] = 6f
        };
        public static readonly Dictionary<string, Dictionary<int, Vector3>> FillerCoords = new() {
            ["bigRoom"] = new Dictionary<int, Vector3> {
            [0] = new Vector3(-1, 0, 5),
            [1] = new Vector3(-5, 0, -1),
            [2] = new Vector3(4, 0, -1),
            [3] = new Vector3(-1, 0, -4)
            },
            
            // Top & bottom need a 90 rotation on Y axis
            ["smallRoom"] = new Dictionary<int, Vector3> {
            [0] = new Vector3(-1, 0, 4),
            [1] = new Vector3(-3, 0, -1),
            [2] = new Vector3(2, 0, -1),
            [3] = new Vector3(-1, 0, -3)
            }
        };
        public static readonly float[] FillerRotations = {
            90f, 0f, 0f, 90f
        };
        
        // Specific hole info
        // Top hole - 0; Left Hole - 1; Right Hole - 2; Bottom Hole - 3
        // based on looking from Y axis, with Z on top (forward axis)
        public const int ERROR_CODE = 5678;
        public const int MAX_HOLE_COUNT = 4;
        public const int RAMP_HOLE_COUNT = 2;
        public const int SETTINGS_VARIETY = 3;
        public const float ROOM_CHECK_RADIUS = 4f;
        public const float OCCUPY_CHECK_RADIUS = 0.3f;
        public const float DEFAULT_ROOM_WIDTH = 6f;
        public const float DEFAULT_ROOM_LENGTH = 8f;

        // Room Generating Info
        public static float RoomChance {get; set;} = 0.5f;
        public static int NewRoomHoleCount {get; set;} = MAX_HOLE_COUNT;
        public static int NumberOfHoles {get; set;} = MAX_HOLE_COUNT;

        // Room, whose holes we are currently checking
        public static int CurrentRoomId {get; set;} = 0;
        public static int CurrentHoleId {get; set;} = 0;

        // Room we will be making
        public static int NextRoomId {get; set;} = 1;
    }
}