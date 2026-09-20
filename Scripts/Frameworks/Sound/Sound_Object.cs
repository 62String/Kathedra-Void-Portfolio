using UnityEngine;

namespace Colored.Frameworks.Sound
{
    public abstract class Object_Base : MonoBehaviour
    {
        internal abstract void Bind(Table_Base table);
    }
    public class Object<T> : Object_Base where T : Table_Base
    {
        [SerializeField] 
        private T _table;
        
        public T Table => _table;

        internal override void Bind(Table_Base table)
        {
            _table = table as T;
        }
    }
}