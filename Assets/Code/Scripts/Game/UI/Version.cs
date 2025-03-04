using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class Version : MonoBehaviour
    {
        public TMP_Text Text;
        
        void Awake()
        {
            Text.text = "v" + Application.version;
        }
    }
}
