using CombatLogic;
using NPCMaker;
using UnityEngine;
using UnityEngine.AI;

namespace NPCLogic {
    partial class StateInfo {
        private static void IdleOnUpdate(Transform player, NPC originalNpcInfo, 
            NavMeshAgent npcObj, bool hasDestination) {
            
            // TODO: Add patrol around original position attained from NPCMaker.NPC
            hasDestination = false;
        }

        private static void ChaseOnEnter(NPCombat combat, NavMeshAgent npcObj) {
            npcObj.isStopped = false;
        }

        private static void ChaseOnUpdate(Transform player, NPC originalNpcInfo, 
            NavMeshAgent npcObj, bool hasDestination) {
            
            // NPC movement towards player transform
            npcObj.SetDestination(player.transform.position);
        }
        private static void ChaseOnExit(NPCombat combat, NavMeshAgent npcObj) {
            npcObj.isStopped = true;
        }
       
        private static void CombatOnEnter(NPCombat combat, NavMeshAgent agent) {
            combat.InAttackRange = true;
        }
        private static void CombatOnUpdate(Transform player, NPC originalNpcInfo, 
            NavMeshAgent npcObj, bool hasDestination) {}
        private static void CombatOnExit(NPCombat combat, NavMeshAgent npcObj) {
            combat.InAttackRange = false;
        }

        private static void ReturnOnEnter(NPCombat combat, NavMeshAgent agent) {
            agent.isStopped = false;
        }

        private static void ReturnOnUpdate(Transform player, NPC originalNpcInfo, 
            NavMeshAgent npcObj, bool hasDestination) {
            
            // Move Npc back to its original coords attained from NPCMaker.NPC
            if (!hasDestination) {
            npcObj.SetDestination(new (originalNpcInfo.X, originalNpcInfo.Y, originalNpcInfo.Z));
                hasDestination = true;
            }
        }

        private static void ReturnOnExit(NPCombat combat, NavMeshAgent agent) {
            agent.isStopped = true;
        }
    }
}