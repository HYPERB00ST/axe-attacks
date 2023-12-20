using CombatLogic;
using NPCMaker;
using UnityEngine;
using UnityEngine.AI;

namespace NPCLogic {
    class State {
        // Id is same as Enum states Id for correlating state
        public StateInfo.StatesId Id {get; private set;}
        public delegate void UpdateMethod(Transform transform, NPC npc, NavMeshAgent agent, bool hasDestination);
        public delegate void EnterMethod(NPCombat combat, NavMeshAgent agent);
        public delegate void ExitMethod(NPCombat combat, NavMeshAgent agent);
        public delegate int ChangeCheckMethod (Transform transformA, Transform transformB, Vector3 pos, NavMeshAgent agent);
        public EnterMethod OnEnter;
        public ExitMethod OnExit;
        public UpdateMethod OnUpdate;
        public ChangeCheckMethod CheckStateChange;

        // Constructors
        internal State(StateInfo.StatesId id) {
            Id = id;
        }
        internal State(StateInfo.StatesId id, 
            EnterMethod onEnter, UpdateMethod onUpdate, ExitMethod onExit, ChangeCheckMethod ChangeCheck) {
                Id = id;
                OnEnter = onEnter;
                OnUpdate = onUpdate;
                OnExit = onExit;
                CheckStateChange = ChangeCheck;
        }
        internal State(StateInfo.StatesId id, 
            ExitMethod onExit, UpdateMethod onUpdate, ChangeCheckMethod ChangeCheck) {
                Id = id;
                OnExit = onExit;
                OnUpdate = onUpdate;
                CheckStateChange = ChangeCheck;
        }
        internal State(StateInfo.StatesId id, 
            EnterMethod onEnter, UpdateMethod onUpdate, ChangeCheckMethod ChangeCheck) {
                Id = id;
                OnEnter = onEnter;
                OnUpdate = onUpdate;
                CheckStateChange = ChangeCheck;
        }
        internal State(StateInfo.StatesId id,
            UpdateMethod onUpdate, ChangeCheckMethod ChangeCheck) {
                Id = id;
                OnUpdate = onUpdate;
                CheckStateChange = ChangeCheck;
        }
    }
}