using UnityEngine;

namespace Colored.Logic.Sound
{
    [CreateAssetMenu(fileName= "Table_TestSFX")]
    public class Table_TestSFX : Frameworks.Sound.Table_Base
    {
        public override string ClassName => "Colored.Logic.Sound.SoundObject_TestSFX";
        [SerializeField] 
        public AK.Wwise.Event TestSFX;
    }

    public class SoundObject_TestSFX : Frameworks.Sound.Object<Table_TestSFX>
    {
        
    }
}
