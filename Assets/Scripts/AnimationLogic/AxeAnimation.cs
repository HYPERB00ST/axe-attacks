using System;
using UnityEngine;

namespace AnimationLogic {
    class AxeAnimation : MonoBehaviour {
        
        private Animator animator;
        void Start() {
            animator = gameObject.GetComponent<Animator>();
        }

        void Update() {
            ChangeAttackAnimation();
        }

        private void ChangeAttackAnimation()
        {
            if (Input.GetMouseButtonDown(0)) {
                animator.SetBool("isAttacking", true);
            }
            else {
                animator.SetBool("isAttacking", false);
            }
        }
    }
}