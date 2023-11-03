using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Hellmade.Sound;

namespace HorrorJam.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] List<AudioRecord> recordList = new List<AudioRecord>();
        [SerializeField] float soundEffectVolume = 0.75f;
        [SerializeField] float bgmVolume = 0.5f;
        
        protected override void InitAfterAwake()
        {

        }

        void Update()
        {
            EazySoundManager.GlobalMusicVolume = bgmVolume;
            EazySoundManager.GlobalSoundsVolume = soundEffectVolume;
        }

        [Button]
        public void PlayOneShot(string id)
        {
            var rec = FindRecord(id);
            if (rec != null)
                EazySoundManager.PlaySound(rec.clip);
        }

        [Button]
        public void FadeInMusic(string id)
        {
            var rec = FindRecord(id);
            if (rec != null)
                EazySoundManager.PlayMusic(rec.clip, bgmVolume, true, false);
        }
        
        [Button]
        public void FadeOutMusic()
        { 
            EazySoundManager.StopAllMusic();
        }

        AudioRecord FindRecord(string id)
        {
            return recordList.FirstOrDefault(rec => rec.id == id);
        }
    }

    [Serializable]
    public class AudioRecord
    {
        public string id;
        public AudioClip clip;
    }
}
