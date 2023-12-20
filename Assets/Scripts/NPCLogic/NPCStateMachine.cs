using System.Collections.Generic;
using CombatLogic;
using NPCMaker;
using UnityEngine;
using UnityEngine.AI;

namespace NPCLogic {
    class NPCStateMachine
    {
        State currentState;
        Dictionary<StateInfo.StatesId, State> states;
        NPC Npc;
        NPCombat NpcCombat;
        Vector3 npcOriginalPos;
        Transform NpcTransform;
        Transform PlayerTransform;
        NavMeshAgent Agent;
        private int stateChange;
        private bool returnCheck = false;
     
        public NPCStateMachine(Dictionary<StateInfo.StatesId, State> pStates,
        NPC npc, NPCombat nPCombat, NavMeshAgent agent) {
            Initialize(pStates, npc, nPCombat, agent);
        }

        private void Initialize(Dictionary<StateInfo.StatesId, State> pStates,
        NPC npc, NPCombat nPCombat, NavMeshAgent agent)
        {
            states = pStates;
            Npc = npc;
            npcOriginalPos = new Vector3(Npc.X, Npc.Y, Npc.Z);
            currentState = states[StateInfo.StatesId.Idle];
            NpcCombat = nPCombat;
            Agent = agent;
        }

        public void ChangeState(StateInfo.StatesId stateId)
        {
            // if current state isnt null, exit it
            currentState.OnExit?.Invoke(NpcCombat, Agent);

            currentState = states[stateId];
            currentState.OnEnter?.Invoke(NpcCombat, Agent);
        }

        public StateInfo.StatesId UpdateState(Transform newPlayerTransform, Transform newNpcTransform) {
            
            UpdatePlayerPosition(newPlayerTransform);
            UpdateNpcPosition(newNpcTransform);

            stateChange = currentState.CheckStateChange(PlayerTransform, NpcTransform, npcOriginalPos, Agent);
            
            if (stateChange != StateInfo.NO_CHANGE_OF_STATE) {
                ChangeState((StateInfo.StatesId)stateChange);
            }
            currentState.OnUpdate?.Invoke(PlayerTransform, Npc, Agent, returnCheck);

            return currentState.Id;
        }

        private void UpdateNpcPosition(Transform newNpcTransform)
        {
            NpcTransform = newNpcTransform;
        }

        private void UpdatePlayerPosition(Transform playerPos)
        {
            PlayerTransform = playerPos;
        }
    }
}