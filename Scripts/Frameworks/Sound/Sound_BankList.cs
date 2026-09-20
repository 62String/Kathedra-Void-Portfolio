using System.Collections.Generic;
using UnityEngine;

namespace Colored.Frameworks.Sound
{
    [CreateAssetMenu(fileName = "BankList", menuName = "Sound/BankList")]
    public class BankList : ScriptableObject
    {
        [SerializeField] 
        public List<AK.Wwise.Bank> Banks = new();
    }
}