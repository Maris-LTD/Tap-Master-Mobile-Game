using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Core.Data;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;
using MyTools.ParticleSystem;
using System.Collections;
using DG.Tweening;

namespace Core.UI.ExtendPopup
{
    public class _WinGamePopup : BasePopup
    {
        [SerializeField] private TMPro.TMP_Text _coinText;
        [SerializeField] private TMP_Text _multiCoinText;
        [SerializeField] private GameObject _watchAdsButton;
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] Image _nativeAdImage;
        private bool _isCollected;

        public void Show()
        {
            base.Show(
                () =>
                {
                    AnimWinGame();
                    StartCoroutine(DelayShowContinueButton(1.5f));
                }
            );
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            _coinText.gameObject.SetActive(false);
            int currentCoin = _PlayerData.UserData.CurrentCollectCoin;
            int coin = currentCoin;
            _coinText.text = "+" + coin.ToString();
            _multiCoinText.text = "+" + (coin * 2).ToString();
            _continueButton.SetActive(false);
            if (coin > 0)
            {
                _coinText.gameObject.SetActive(true);
                _watchAdsButton.SetActive(true);
            }
            else
            {
                _watchAdsButton.SetActive(false);
            }
            AddNativeAd();
            _isCollected = false;
        }

        public void OnClickClose()
        {
            base.Hide();
        }

        public void OnClickWatchAds()
        {
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) =>
                {
                    if (x)
                    {
                        GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel, "win_game_watch_ads_to_multi_coin");
                        _PlayerData.UserData.Coin += _PlayerData.UserData.CurrentCollectCoin;
                        _coinText.text = "+" + _PlayerData.UserData.CurrentCollectCoin * 2;
                        _watchAdsButton.SetActive(false);
                    }
                }, null, location: "win_game_watch_ads_to_multi_coin"
            );
        }

        public void OnClickToContinue()
        {
            if (_isCollected) return;
            _isCollected = true;
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            //base.Hide(() => {_GameManager.Instance.NextLevel();}); 
            if (_coinText.IsActive())
            {
                _MySoundManager.Instance.PlaySound(_SoundType.Coin);
                _ParticleSystemManager.Instance.ShowParticle(_ParticleTypeEnum.CoinSpawn, _coinText.transform.position, true,
                    () =>
                    {
                        base.Hide();
                        _GameManager.Instance.NextLevel();
                    }
                );
            }
            else
            {
                base.Hide();
                _GameManager.Instance.NextLevel();
            }

        }

        private void AnimWinGame()
        {
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Win-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Win-Idle", true, 0);
        }

        private void AddNativeAd()
        {
            var nativeAd = AdsManager.Instance.GetNativeAd();
            if (nativeAd != null)
            {
                _nativeAdImage.sprite = Sprite.Create(nativeAd.GetIconTexture(), new Rect(0, 0, nativeAd.GetIconTexture().width, nativeAd.GetIconTexture().height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                _nativeAdImage.gameObject.SetActive(false);
            }
        }

        private IEnumerator DelayShowContinueButton(float time)
        {
            yield return new WaitForSeconds(time);
            _continueButton.transform.localScale = Vector3.zero;
            _continueButton.SetActive(true);
            _continueButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }
}