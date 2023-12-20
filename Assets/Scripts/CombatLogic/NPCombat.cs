using System;
using System.Collections;
using NPCMaker;
using UnityEngine;


namespace CombatLogic {
    public class NPCombat : MonoBehaviour
    {
        NPC Npc;
        public int Hp {get; set;} = 40;
        internal bool Hit {get; set;} = false;
        public bool InAttackRange {get; set;} = false;
        private bool canDamage {get; set;} = false;
        private bool punched {get; set;} = false;
        private Animator animator;
        private PlayerCombat playerCombat;
        private AnimatorStateInfo animatorStateInfo;


        void Start() {
            Npc = NPCInfo.NpcsAll[int.Parse(gameObject.name)];
            animator = gameObject.GetComponentInChildren<Animator>();
            playerCombat = GameObject.Find("Player Controller").GetComponent<PlayerCombat>();
        }

        void Update() {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
            if (Hit) {
                SustainDamage();
            }
            if (InAttackRange) {
                Attack();
            }
            CheckHp();
        }

        private void Attack()
        {
            // TODO: Raycast 2f in forward direction, if hit player after 0.5 (attack duration) sec
            // Set transform.LookAt in StatesActual to make npc always look at player while in combat
            canDamage = CheckCombatStates();
            if (canDamage) {
                playerCombat.HitPlayer();
                canDamage = false;
            }
            
        }

        private bool CheckCombatStates()
        {
            if (animatorStateInfo.IsName("Punching") && !punched) {
                if (animatorStateInfo.normalizedTime >= CombatInfo.PUNCH_ANIM_DMG_MOMENT) {
                    
                    // Player can be damaged once per punching animation
                    punched = true;
                    return true;
                }
                // canDamage = true;
            }
            else if (animatorStateInfo.IsName("Combat Idle")) {
                punched = false;
            }
            return false;
        }

        private void SustainDamage()
        {
            Hp -= CombatInfo.DEFAULT_DAMAGE;
        }

        private void CheckHp()
        {
            if (Hp <= 0) {
                WaitForDeathAnim();
            }
        }
        public void WasAttacked() {
            Hit = true;
        }

        private void WaitForDeathAnim() {

            Invoke(nameof(Die), animator.GetCurrentAnimatorStateInfo(0).length);

        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}

