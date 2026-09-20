using UnityEngine;

namespace Colored.Logic.Sound
{
    [CreateAssetMenu(fileName= "SoundTable_UI_MainMenu")]
    public class SoundTable_UI_MainMenu : SoundTable_UI_Base
    {
        public override string ClassName => "Colored.Logic.Sound.SoundObject_UI_MainMenu";
        
        [SerializeField] 
        public AK.Wwise.Event StartGame;
    }
    
    public class SoundObject_UI_MainMenu : Frameworks.Sound.Object<SoundTable_UI_MainMenu>
    {

    }
}