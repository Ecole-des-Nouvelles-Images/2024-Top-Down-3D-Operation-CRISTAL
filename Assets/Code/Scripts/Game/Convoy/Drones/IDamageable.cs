using UnityEngine;

namespace Code.Scripts.Game.Convoy.Drones
{
    public interface IDamageable
    {
        bool IsTargetable { get; }
        GameObject GameObject { get; }
        Transform Transform { get; }
        void TakeDamage(int damage);

    }
}