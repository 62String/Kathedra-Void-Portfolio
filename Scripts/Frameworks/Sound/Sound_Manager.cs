using System;
using Colored.Foundations;
using UnityEngine;

namespace Colored.Frameworks.Sound
{
    public sealed class Manager : Foundations.Singleton< Manager >
    {
        public Manager() { }

        protected override void PostCreated()
        {
            _initialize();
        }
        
        private GameObject _bgmObject;
        private uint _bgmPlayingID;
        
        private void _initialize()
        {
            _bgmObject = new GameObject("[Sound][BGM]");
            UnityEngine.Object.DontDestroyOnLoad(_bgmObject);
            _bgmObject.AddComponent<AkGameObj>();
            
            var bankList = Resource.Manager.Instance.Load<BankList>("Sound/BankList");
            if (bankList == null)
            {
                Logger.Warning(LogCategory.System, "Sound,Manager: BankList를 찾을 수 없음");
                return;
            }
            
            foreach (var bank in bankList.Banks)
            {
                AkUnitySoundEngine.LoadBank(bank.Name, out uint _);
            }
            Logger.Info(LogCategory.System, "Sound.Manager initialize");
        }

        public void PlayBGM(string eventName)
        {
            StopBGM();
            _bgmPlayingID = AkUnitySoundEngine.PostEvent((eventName), _bgmObject);
        }

        public void StopBGM()
        {
            if (_bgmPlayingID != 0)
            {
                AkUnitySoundEngine.StopPlayingID(_bgmPlayingID);
                _bgmPlayingID = 0;
            }
        }
        
        public Object<T> CreateObject<T>(string tablePath, GameObject parent) where T : Table_Base
        {
            var table = Resource.Manager.Instance.Load<T>(tablePath);
            if (table == null)
            {
                return null;
            }
            
            var go = new GameObject(table.name);
            go.transform.SetParent(parent.transform);
            go.AddComponent<AkGameObj>();
            
            var type = Type.GetType(table.ClassName);
            var soundObject = go.AddComponent(type) as Object<T>;
            
            soundObject.Bind(table);
            
            return soundObject;
        }

        public Object_Base CreateObject(Table_Base table, GameObject parent)
        {
            var go = new GameObject(table.name);
            go.transform.SetParent(parent.transform);
            go.AddComponent<AkGameObj>();
            
            var type = Type.GetType(table.ClassName);
            var soundObject = go.AddComponent(type) as Object_Base;
            
            soundObject.Bind(table);
            
            return soundObject;
        }
        
        public void DestroyObject(Object_Base obj)
        {
            UnityEngine.Object.Destroy(obj.gameObject);
        }
        
        public uint PostEvent(string eventName, GameObject source)
        {
            return AkUnitySoundEngine.PostEvent(eventName, source);
        }
        public void StopEvent(uint id)
        {
            AkUnitySoundEngine.StopPlayingID(id);
        }

        public void LoadBank(string bankName)
        {
            AkUnitySoundEngine.LoadBank(bankName, out uint _);
        }

        public void UnloadBank(string bankName)
        {
            AkUnitySoundEngine.UnloadBank(bankName, IntPtr.Zero);
        }

        public void SetRTPC(string rtpcName, float value, GameObject source)
        {
            AkUnitySoundEngine.SetRTPCValue(rtpcName, value, source);
        }

        public void SetBGMState(string state)
        {
            SetState(TheGlobal.BGMStateGroup, state);
        }
        
        public void SetState(string stateGroup, string state)
        {
            AkUnitySoundEngine.SetState(stateGroup, state);
        }
    }
}