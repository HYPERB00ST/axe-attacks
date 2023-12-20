using System.Collections.Generic;
using NPCMaker;
using UnityEngine;

namespace NPCLogic {
    partial class StateInfo {
        
        // NPC States
        public static readonly Dictionary<StatesId, State> states = new() {
            {StatesId.Idle, new State(StatesId.Idle, IdleOnUpdate, IdleCheckMethod)},
            {StatesId.Chase, new State(StatesId.Chase, ChaseOnEnter, ChaseOnUpdate, ChaseOnExit, ChaseCheckMethod)},
            {StatesId.Combat, new State(StatesId.Combat, CombatOnEnter, CombatOnUpdate, CombatOnExit, CombatCheckMethod)},
            {StatesId.Return, new State(StatesId.Return, ReturnOnEnter, ReturnOnUpdate, ReturnOnExit, ReturnCheckMethod)}
        };
    }
}