using System;
using CombatLogic;
using NPCLogic;
using UnityEngine;

namespace AnimationLogic {
    class NPCAnimation : MonoBehaviour {
        private Animator animator;
        private NPCombat combat;
        private NPCBehControl behaviourControl;
        private StateInfo.StatesId currentState;
        private 

        void Start() {
            animator = gameObject.GetComponentInChildren<Animator>();
            combat = gameObject.GetComponent<NPCombat>();
            behaviourControl = gameObject.GetComponent<NPCBehControl>();
        }

        void Update() {
            UpdateCurrentState();
            AnimateCurrentState();
            CheckHit();
        }

        private void AnimateCurrentState()
        {
            switch (currentState) {
                
                case StateInfo.StatesId.Chase:
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isAttacking", false);
                    break;
                
                case StateInfo.StatesId.Combat:
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isWalking", false);
                    AnimatePunchingState();
                    break;
                
                case StateInfo.StatesId.Return:
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isWalking", true);
                    break;
                
                case StateInfo.StatesId.Idle:
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isAttacking", false);
                    break;
                
                default:
                    break;
            }
        }

        private void AnimatePunchingState()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Combat Idle")) {
                animator.SetBool("isPunching", true);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punching")) {
                animator.SetBool("isPunching", false);
            }
        }

        private void CheckHit()
        {
            if (combat.Hit) {
                if (combat.Hp > 0) {
                    animator.SetBool("isHit", true);
                }
                else {
                    animator.SetBool("isDead", true);
                }
                combat.Hit = false;
            }
            else {
                animator.SetBool("isHit", false);
            }
        }

        private void UpdateCurrentState()
        {
            currentState = behaviourControl.currentState;
        }
    }
}