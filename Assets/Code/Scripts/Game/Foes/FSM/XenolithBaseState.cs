using UnityEngine;

namespace Game.Foes.FSM
{
    public abstract class XenolithBaseState
    {
        public abstract void EnterState(Xenolith xenolith);
        public abstract void UpdateState(Xenolith xenolith);
        public abstract void OnTriggerEnter(Xenolith xenolith, Collider collider);
        
        public virtual void OnTriggerExit(Xenolith xenolith, Collider collider) {}
    }
}
