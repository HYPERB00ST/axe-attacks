using System;
using CombatLogic;
using NPCMaker;
using UnityEngine;
using UnityEngine.AI;

namespace NPCLogic {
    // Attach this to each NPC
    public class NPCBehControl : MonoBehaviour
    {
        private NPCStateMachine NpcStateMachine;
        private NPC Npc;
        private GameObject PlayerObject;
        private NPCombat NpcCombat;
        private NavMeshAgent NpcAgent;
        internal StateInfo.StatesId currentState {get; private set;}
        void Start()
        {
            InitializeNPC();
        }

        void Update()
        {
            currentState = NpcStateMachine.UpdateState(PlayerObject.transform, gameObject.transform);
        }

        void InitializeNPC() {
            
            // Find Info on this NPC
            Npc = NPCInfo.NpcsAll[int.Parse(gameObject.name)];

            // Find Player Object / every NPC needs to know of the player
            PlayerObject = GameObject.Find("/Player Controller");
            
            // Get Combat script
            NpcCombat = gameObject.GetComponent<NPCombat>();

            // Get NavMeshAgent for moving
            NpcAgent = gameObject.GetComponent<NavMeshAgent>();
            
            // Make state machine for itself
            NpcStateMachine = new(StateInfo.states, Npc, NpcCombat, NpcAgent);
            if (NpcStateMachine == null) {Debug.Log("NULL");}
        }
    }
}