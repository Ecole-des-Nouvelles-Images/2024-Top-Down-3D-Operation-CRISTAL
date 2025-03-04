using UnityEngine;

namespace Game.Foes
{
    public class XenolithSpawner: MonoBehaviour
    {
        public Side SideTag = Side.None;

        public Vector3 Position => transform.position;
    }
}
