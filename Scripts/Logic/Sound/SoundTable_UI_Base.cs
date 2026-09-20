using UnityEngine;

namespace Colored.Logic.Sound
{
    public abstract class SoundTable_UI_Base : Frameworks.Sound.Table_Base
    {
        [SerializeField] 
        public AK.Wwise.Event WindowOpen;
        [SerializeField] 
        public AK.Wwise.Event ButtonClick;
    }
}