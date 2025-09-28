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
        [SerializeField] private AudioClip[] _tapBoosterSounds;
        [SerializeField] private AudioClip[] _purchasedStartSounds;
        [SerializeField] private AudioClip[] _purchasedEndSounds;
        [SerializeField] private AudioClip[] _explodeSounds;
        [SerializeField] private AudioClip[] _coinSounds;

        private AudioClip _currentBackgroundMusic = null;
        private int _currentBackgroundMusicRequestIndex = -1;
        public static float defaultPitch = 1f;

        /// <summary>
        /// Play sound with type and default configured value
        /// </summary>
        public void PlaySound(_SoundType type)
        {
            if (!_PlayerData.UserData.IsTurnOnSound) return;
            var audioClip = GetAudioClip(type);
            if (audioClip == null) return;
            soundManager.PlaySfxOverride(audioClip);
        }

        /// <summary>
        /// Play sound with type and pitch
        /// </summary>
        public void PlaySound(_SoundType type, float pitch)
        {
            if (!_PlayerData.UserData.IsTurnOnSound) return;
            var audioClip = GetAudioClip(type);
            if (audioClip == null) return;
            soundManager.PlaySfxConfig(audioClip, pitch);
        }

        public void PlaySoundPitchIncrease(_SoundType type, bool isComplete = false)
        {
            if (!_PlayerData.UserData.IsTurnOnSound) return;
            var audioClip = GetAudioClip(type);
            if (audioClip == null) return;
            soundManager.PlaySfxConfig(audioClip, defaultPitch);
            defaultPitch += 0.2f;
            if (isComplete)
                defaultPitch = 1f;
        }
        public void PlayMusic()
        {
            if (!_PlayerData.UserData.IsTurnOnMusic) return;
            StopMusic();
            _currentBackgroundMusic = _backgroundMusics[Random.Range(0, _backgroundMusics.Length)];
            _currentBackgroundMusicRequestIndex = 0;
            soundManager.PlaySfxLoop(_currentBackgroundMusic, _currentBackgroundMusicRequestIndex);
        }

        public void StopMusic()
        {
            if (_currentBackgroundMusic != null)
                soundManager.StopLoopSound(_currentBackgroundMusic, _currentBackgroundMusicRequestIndex);
        }

        public void Vibrate()
        {
            if (!_PlayerData.UserData.IsTurnOnVibration) return;
            //Vibration.VibrateAndroid(50);
        }

        private AudioClip GetAudioClip(_SoundType type)
        {
            return type switch
            {
                _SoundType.Tap => _tapSounds[Random.Range(0, _tapSounds.Length)],
                _SoundType.Spawn => _spawnSounds[Random.Range(0, _spawnSounds.Length)],
                _SoundType.TapFail => _tapFailSounds[Random.Range(0, _tapFailSounds.Length)],
                _SoundType.Win => _winSounds[Random.Range(0, _winSounds.Length)],
                _SoundType.Lose => _loseSounds[Random.Range(0, _loseSounds.Length)],
                _SoundType.ClickUIButton => _clickUIButtonSounds[Random.Range(0, _clickUIButtonSounds.Length)],
                _SoundType.TapBooster => _tapBoosterSounds[Random.Range(0, _tapBoosterSounds.Length)],
                _SoundType.PurchasedStart => _purchasedStartSounds[Random.Range(0, _purchasedStartSounds.Length)],
                _SoundType.PurchasedEnd => _purchasedEndSounds[Random.Range(0, _purchasedEndSounds.Length)],
                _SoundType.Explode => _explodeSounds[Random.Range(0, _explodeSounds.Length)],
                _SoundType.Coin => _coinSounds[Random.Range(0, _coinSounds.Length)],
                //SoundType.BackgroundMusic => _backgroundMusics[Random.Range(0, _backgroundMusics.Length)],
                _ => null
            };
        }
    }
}

public enum _SoundType
{
    Tap,
    Spawn,
    TapFail,
    Win,
    Lose,
    ClickUIButton,
    BackgroundMusic,
    TapBooster,
    PurchasedStart,
    PurchasedEnd,
    Explode,
    Coin
}