using UnityEngine;
using UnityEngine.AI;

namespace NPCLogic {
    partial class StateInfo {
        // While in this state, this method will be called to check if it's supposed to change
        // Idle check
        private static int IdleCheckMethod(Transform playerTransform, Transform npcTransform, Vector3 originalPos, NavMeshAgent agent) {
            
            // Chase check
            if (Vector3.Distance(playerTransform.position, npcTransform.position) <= CHASE_RANGE) {
                return (int)StatesId.Chase;
            }

            return NO_CHANGE_OF_STATE;
        }
        // Chase check
        private static int ChaseCheckMethod(Transform playerTransform, Transform npcTransform, Vector3 originalPos, NavMeshAgent agent) {
            
            float distance = Vector3.Distance(playerTransform.position, npcTransform.position);

            // Combat check
            if (distance <= COMBAT_RANGE) {
                return (int)StatesId.Combat;
            }
            // Return check
            else if (distance >= RETURN_RANGE) {
                return (int)StatesId.Return;
            }
            
            return NO_CHANGE_OF_STATE;
        }
        // Combat check
        private static int CombatCheckMethod(Transform playerTransform, Transform npcTransform, Vector3 originalPos, NavMeshAgent agent) {
            
            float distance = Vector3.Distance(playerTransform.position, npcTransform.position);
            
            // Chase check
            if (distance > COMBAT_RANGE) {
                return (int)StatesId.Chase;
            }

            // Cannot go into return state straight from combat state
            return NO_CHANGE_OF_STATE;
        }
        // Return check
        private static int ReturnCheckMethod(Transform playerTransform, Transform npcTransform, Vector3 originalPos, NavMeshAgent agent) {
            
            float distance = Vector3.Distance(playerTransform.position, npcTransform.position);
            float destinationDistance = Vector3.Distance(npcTransform.position, agent.destination);
            
            // Chase check
            if (distance <= CHASE_RANGE) {
                return (int)StatesId.Chase;
            }
            // Idle check
            else if (destinationDistance < 0.5f) {
                return (int)StatesId.Idle;
            }

            return NO_CHANGE_OF_STATE;
        }
    }
}