using TMPro;
using UnityEngine;

namespace Code.Scripts.Game.UI
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
