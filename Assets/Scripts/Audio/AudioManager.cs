using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using FMODUnity;
using FMOD.Studio;

namespace HorrorJam.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Volume")]
        [Range(0f,1f)]
        public float masterVolume = 1;

        Bus masterBus;

        Dictionary<string,EventInstance> eventInstanceDic;
        

        protected override void InitAfterAwake()
        {
            Init();
        }

        void Init()
        {
            eventInstanceDic = new Dictionary<string, EventInstance>();

            // masterBus = RuntimeManager.GetBus("bus:/");
        }

        void Update()
        {
            // masterBus.setVolume(masterVolume);
        }

        public void PlayOneShot(string soundID)
        {
            RuntimeManager.PlayOneShot(AudioEvent.Instance.audioEventDictionary[soundID]);
        }

        public void PlayOneShot(string soundID, Vector3 position)
        {
            RuntimeManager.PlayOneShot(AudioEvent.Instance.audioEventDictionary[soundID],position);
        }

        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }

        public void PlayOneShot(EventReference sound, Vector3 position)
        {
            RuntimeManager.PlayOneShot(sound, position);
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            return eventInstance;
        }

        public void PlayLoop(string soundID,string id)
        {
            var _audioEvent = CreateInstance(AudioEvent.Instance.audioEventDictionary[soundID]);
            _audioEvent.start();
            eventInstanceDic.Add(id,_audioEvent);
        }

        public void PlayLoop(EventReference eventReference,string id)
        {
            var _audioEvent = CreateInstance(eventReference);
            _audioEvent.start();
            eventInstanceDic.Add(id,_audioEvent);
        }

        // public void FadeOutLoop(string id)
        // {
        //     eventInstanceDic[id].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        // }

        public void StopLoop(string id)
        {
            eventInstanceDic[id].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstanceDic[id].release();
            eventInstanceDic.Remove(id);
        }

        public void CleanUp()
        {
            foreach(var n in eventInstanceDic.ToList())
            {
                StopLoop(n.Key);
                n.Value.release();
            }
        }

        void OnDestroy() 
        {
            CleanUp();    
        }
    }
}
