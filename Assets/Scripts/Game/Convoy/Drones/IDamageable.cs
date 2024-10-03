using UnityEngine;

namespace Game.Convoy.Drones
{
    public interface IDamageable {
        
        bool IsTargetable { get; }
        GameObject GameObject { get; }
        Transform Transform { get; }
        void TakeDamage(int damage);

    }
}