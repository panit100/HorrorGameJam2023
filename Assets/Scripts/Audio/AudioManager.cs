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
        
        EventInstance backgroundMusic;
        EventInstance ambient;
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

        public void PlayAudioOneShot(string soundID)
        {
            RuntimeManager.PlayOneShot(AudioEvent.Instance.audioEventDictionary[soundID]);
        }

        public void PlayAudioOneShot(string soundID, Vector3 position)
        {
            RuntimeManager.PlayOneShot(AudioEvent.Instance.audioEventDictionary[soundID],position);
        }

        public void PlayAudioOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }

        public void PlayAudioOneShot(EventReference sound, Vector3 position)
        {
            RuntimeManager.PlayOneShot(sound, position);
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            return eventInstance;
        }

        public void PlayAudio(string soundID)
        {
            var _audioEvent = CreateInstance(AudioEvent.Instance.audioEventDictionary[soundID]);
            _audioEvent.start();
            eventInstanceDic.Add(soundID,_audioEvent);
        }

        public void PlayAudio(EventReference eventReference,string id)
        {
            var _audioEvent = CreateInstance(eventReference);
            _audioEvent.start();
            eventInstanceDic.Add(id,_audioEvent);
        }

        public void PlayAudio3D(string soundID,GameObject attachGameobject)
        {
            var _audioEvent = CreateInstance(AudioEvent.Instance.audioEventDictionary[soundID]);
            RuntimeManager.AttachInstanceToGameObject(_audioEvent,attachGameobject.transform);
            _audioEvent.start();
            eventInstanceDic.Add(soundID,_audioEvent);
        }

        public void StopAudio(string id)
        {
            eventInstanceDic[id].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstanceDic[id].release();
            eventInstanceDic.Remove(id);
        }

        public void PlayBGM(string soundID)
        {
            StopBGM();

            backgroundMusic = CreateInstance(AudioEvent.Instance.audioEventDictionary[soundID]);
            backgroundMusic.start();
        }

        public void StopBGM()
        {
            if(backgroundMusic.isValid())
                backgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void PlayAmbient(string soundID)
        {
            StopAmbient();

            ambient = CreateInstance(AudioEvent.Instance.audioEventDictionary[soundID]);
            ambient.start();
        }

        public void StopAmbient()
        {
            if(ambient.isValid())
                ambient.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void CleanUp()
        {
            foreach(var n in eventInstanceDic.ToList())
            {
                StopAudio(n.Key);
            }

            backgroundMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            backgroundMusic.release();
            
            ambient.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ambient.release();
        }

        void OnDestroy() 
        {
            CleanUp();    
        }
    }
}
