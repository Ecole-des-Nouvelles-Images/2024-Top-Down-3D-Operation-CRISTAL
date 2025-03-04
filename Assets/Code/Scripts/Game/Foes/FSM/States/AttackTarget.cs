using Game.Convoy.Drones;
using UnityEngine;

namespace Game.Foes.FSM.States
{
    public class AttackTarget : BaseState
    {
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        public AttackTarget(Xenolith xenolith) : base(xenolith) { }

        public override void EnterState() {
            Xenolith.Agent.SetDestination(Xenolith.transform.position);
        }
        
        public override void UpdateState() {
            // Rotate toward the target
            // Xenolith.transform.LookAt(Xenolith.Target.transform.position);
            
            if (Xenolith.AttackReady) {
                Xenolith.Target.GetComponent<IDamageable>().TakeDamage(Xenolith.AttackDamage);
                Xenolith.Animator.SetTrigger(AttackTrigger);
                Xenolith.ResetAttackTimer();
            }
        }
        
        public override void ExitState() { }
    }
}
