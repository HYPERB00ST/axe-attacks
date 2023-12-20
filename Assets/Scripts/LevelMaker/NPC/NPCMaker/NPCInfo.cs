using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCMaker {
    
    class NPCInfo {
        internal const int NPC_MAX_HP = 100;
        internal const int MAX_NPCS_BIGROOM = 6;
        internal const int MAX_NPCS_SMALLROOM = 4;
        internal const int NPC_LENGTH = 2;
        internal const int NPC_WIDTH = 2;

        public static readonly List<NPC> NpcsAll = new();
    }
}