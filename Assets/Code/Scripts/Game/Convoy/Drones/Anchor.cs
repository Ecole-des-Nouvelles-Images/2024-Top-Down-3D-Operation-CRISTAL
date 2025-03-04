using UnityEngine;

namespace Game.Convoy.Drones
{
    public class Anchor: MonoBehaviour
    {
        public Drone Occupant { get; set; }
        
        public Vector3 Position => transform.position;
        public bool Available => Occupant == null;

        private void Awake()
        {
            Occupant = null;
        }
    }
}
