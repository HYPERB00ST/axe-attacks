using System;
using System.Collections;
using System.Collections.Generic;
using CombatLogic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask npcMask;
    [SerializeField] Transform PlayerCamera;
    private Animator animator;
    
    int Hp {get; set;} = 100;
    bool wasHit {get; set;} = false;
    bool hasAttacked {get; set;} = false;
    bool hasHit {get; set;} = false;
    bool canDamage {get; set;} = false;

    void Start() {
        // Might need updating in the future
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {   
        hasAttacked = GetAttack();
        
        if (hasAttacked) {
            NPCombat hitNpc = Attack();
            
            if (hasHit) {
                DealDamage(hitNpc);
            }
        }
        
        if (wasHit) {
            SustainDamage();
            CheckHp();
        }
    }

    private void DealDamage(NPCombat hitNpc)
    {
        hitNpc.WasAttacked();
        hasHit = false;
    }

    private NPCombat Attack()
    {
        RaycastHit hitInfo;
        
        // Player camera so that its angled correctly
        bool hit = Physics.Raycast(transform.position, PlayerCamera.TransformDirection(Vector3.forward), 
            out hitInfo, CombatInfo.PLAYER_MELEE_RANGE, npcMask);

        if (hit) {
            NPCombat hitNpc = hitInfo.collider.gameObject.GetComponent<NPCombat>();
            hasHit = true;

            return hitNpc;
       }
       // If missed, or hit a wall, null
       return null;
    }

    private bool GetAttack()
    {
        // Cant spam attack if not in idle state
        if (Input.GetMouseButtonDown(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return true;
        }

        return false;
    }

    private void CheckHp()
    {
        if (Hp <= 0) {
            // Reset
            GameObject.Find("LevelManager").GetComponent<LevelManagerScript>().Level = 1;
            SceneManager.LoadScene("GameoverScene");
        }
    }

    private void SustainDamage()
    {
        Hp -= CombatInfo.DEFAULT_DAMAGE;
        
        // Reset
        wasHit = false;
    }

    internal void HitPlayer() {
        wasHit = true;
    }
}
