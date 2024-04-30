using Core.Data;
using UnityEngine;

namespace Core.GamePlay
{
    public class _MySoundManager : SingletonMonoBehaviour<_MySoundManager>
    {
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private VibrationManager vibrationManager;
        [Header("Resources")]
        [SerializeField] private AudioClip[] _tapSounds;
        [SerializeField] private AudioClip[] _spawnSounds;
        [SerializeField] private AudioClip[] _tapFailSounds;
        [SerializeField] private AudioClip[] _winSounds;
        [SerializeField] private AudioClip[] _loseSounds;
        [SerializeField] private AudioClip[] _clickUIButtonSounds;
        [SerializeField] private AudioClip[] _backgroundMusics;

        private AudioClip _currentBackgroundMusic = null;
        private int _currentBackgroundMusicRequestIndex = -1;

        public void PlaySound(SoundType type)
        {
            if(!_PlayerData.UserData.IsTurnOnSound) return;
            var audioClip = GetAudioClip(type);
            if (audioClip == null) return;
            soundManager.PlaySfxOverride(audioClip);
        }

        public void PlayMusic(){
            if(!_PlayerData.UserData.IsTurnOnMusic) return;
            StopMusic();
            _currentBackgroundMusic = _backgroundMusics[Random.Range(0, _backgroundMusics.Length)];
            _currentBackgroundMusicRequestIndex = 0;
            soundManager.PlaySfxLoop(_currentBackgroundMusic, _currentBackgroundMusicRequestIndex);
        }

        public void StopMusic(){
            if(_currentBackgroundMusic != null)
                soundManager.StopLoopSound(_currentBackgroundMusic, _currentBackgroundMusicRequestIndex);
        }

        public void Vibrate(){
            if(!_PlayerData.UserData.IsTurnOnVibration) return;
            //VibrationManager.Vibrate();
        }

        private AudioClip GetAudioClip(SoundType type)
        {
            return type switch
            {
                SoundType.Tap => _tapSounds[Random.Range(0, _tapSounds.Length)],
                SoundType.Spawn => _spawnSounds[Random.Range(0, _spawnSounds.Length)],
                SoundType.TapFail => _tapFailSounds[Random.Range(0, _tapFailSounds.Length)],
                SoundType.Win => _winSounds[Random.Range(0, _winSounds.Length)],
                SoundType.Lose => _loseSounds[Random.Range(0, _loseSounds.Length)],
                SoundType.ClickUIButton => _clickUIButtonSounds[Random.Range(0, _clickUIButtonSounds.Length)],
                //SoundType.BackgroundMusic => _backgroundMusics[Random.Range(0, _backgroundMusics.Length)],
                _ => null
            };
        }
    }
}

public enum SoundType
{
    Tap,
    Spawn,
    TapFail,
    Win,
    Lose,
    ClickUIButton,
    BackgroundMusic
}