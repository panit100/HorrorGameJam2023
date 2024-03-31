using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using FMODUnity;
using FMOD.Studio;

namespace HorrorJam.Audio
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        [Header("Volume")]
        [Range(0f, 1f)]
        public float masterVolume = 1;
        [Range(0f, 1f)]
        public float musicVolume = 1;
        [Range(0f, 1f)]
        public float SFXVolume = 1;

        Bus masterBus;
        Bus ambientBus;
        Bus SFXBus;

        AudioEventScriptableObject auidoEvent_SFX;
        AudioEventScriptableObject auidoEvent_Cutscene;
        AudioEventScriptableObject auidoEvent_BGM;
        Dictionary<string, EventReference> auidoEvents;

        Dictionary<string, EventInstance> eventInstanceDic;

        EventInstance backgroundMusic;
        EventInstance ambient;
        protected override void InitAfterAwake()
        {
            Init();
        }

        void Init()
        {
            GetAudioEvent();

            eventInstanceDic = new Dictionary<string, EventInstance>();

            masterBus = RuntimeManager.GetBus("bus:/");
            ambientBus = RuntimeManager.GetBus("bus:/AmbientBus");
            SFXBus = RuntimeManager.GetBus("bus:/SFXBus");
        }

        void Update()
        {
            masterBus.setVolume(masterVolume);
            ambientBus.setVolume(musicVolume);
            SFXBus.setVolume(SFXVolume);
        }

        void GetAudioEvent()
        {
            auidoEvent_SFX = Resources.Load<AudioEventScriptableObject>("AudioEvent/SFX");
            auidoEvent_Cutscene = Resources.Load<AudioEventScriptableObject>("AudioEvent/Cutscene");
            auidoEvent_BGM = Resources.Load<AudioEventScriptableObject>("AudioEvent/BGM");

            foreach (var n in auidoEvent_SFX.eventList)
            {
                auidoEvents.Add(n.key, n.sound);
            }

            foreach (var n in auidoEvent_Cutscene.eventList)
            {
                auidoEvents.Add(n.key, n.sound);
            }

            foreach (var n in auidoEvent_BGM.eventList)
            {
                auidoEvents.Add(n.key, n.sound);
            }
        }

        public void PlayAudioOneShot(string soundID)
        {
            RuntimeManager.PlayOneShot(auidoEvents[soundID]);
        }

        public void PlayAudioOneShot(string soundID, Vector3 position)
        {
            RuntimeManager.PlayOneShot(auidoEvents[soundID], position);
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
            var _audioEvent = CreateInstance(auidoEvents[soundID]);
            _audioEvent.start();
            eventInstanceDic.Add(soundID, _audioEvent);
        }
        public void PlayAudio(string soundID, out EventInstance outInstance)
        {
            var _audioEvent = CreateInstance(auidoEvents[soundID]);
            outInstance = _audioEvent;
            _audioEvent.start();
            eventInstanceDic.Add(soundID, _audioEvent);
        }

        public void PlayAudio(EventReference eventReference, string id)
        {
            var _audioEvent = CreateInstance(eventReference);
            _audioEvent.start();
            eventInstanceDic.Add(id, _audioEvent);
        }

        public void PlayAudio3D(string soundID, GameObject attachGameobject)
        {
            var _audioEvent = CreateInstance(auidoEvents[soundID]);
            RuntimeManager.AttachInstanceToGameObject(_audioEvent, attachGameobject.transform);
            _audioEvent.start();
            eventInstanceDic.Add(soundID, _audioEvent);
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

            backgroundMusic = CreateInstance(auidoEvents[soundID]);
            backgroundMusic.start();
        }

        public void StopBGM()
        {
            if (backgroundMusic.isValid())
                backgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void PlayAmbient(string soundID)
        {
            StopAmbient();

            ambient = CreateInstance(auidoEvents[soundID]);
            ambient.start();
        }

        public void StopAmbient()
        {
            if (ambient.isValid())
                ambient.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void CleanUp()
        {
            foreach (var n in eventInstanceDic.ToList())
            {
                StopAudio(n.Key);
            }

            if (backgroundMusic.isValid())
            {
                backgroundMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                backgroundMusic.release();
            }

            if (ambient.isValid())
            {
                ambient.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                ambient.release();
            }
        }

        void OnDestroy()
        {
            CleanUp();
        }

        public void ChangeMasterVolume(float Volume)
        {
            masterVolume = Volume;
        }

        public void ChangeMusicVolume(float Volume)
        {
            musicVolume = Volume;
        }

        public void ChangeSFXVolume(float Volume)
        {
            SFXVolume = Volume;
        }
    }
}
