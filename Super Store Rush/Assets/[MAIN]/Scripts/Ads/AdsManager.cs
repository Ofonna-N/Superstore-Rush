using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace SuperStoreRush
{
    public class AdsManager : MonoBehaviour
    {
        private static AdsManager instance;
        public static AdsManager Instance => instance;

        [ShowInInspector, ReadOnly]
        private string gameId;

        [SerializeField] 
        private string _androidGameId;

        [SerializeField]
        private string _iOSGameId;


        [SerializeField]
        private bool testMode = true;

        [SerializeField]
        private InterstitialAd interstitialAd;

        [SerializeField]
        private BannerAd bannerAd;

        [SerializeField]
        private RewardedVideoAd rewardedVideoAd;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;

            }

        }

        private void Start()
        {
            //interstitialAd.Init();
            //bannerAd.Init();
            //rewardedVideoAd.Init();

            //Advertisement.Initialize(gameId, testMode);

            /*DOVirtual.Float(0f, 1f, 2f, (x) => { })
                .OnComplete(()=> PlayAd(AdType.Banner));*/
        }

        public void PlayAd(AdType type)
        {
            switch (type)
            {
                case AdType.Interstitial:
                    interstitialAd.LoadAd();
                    break;
                case AdType.Banner:
                    bannerAd.LoadAd();
                    break;
                case AdType.Rewarded:
                    rewardedVideoAd.LoadAd();
                    break;
                default:
                    break;
            }
        }

        public void PlayHRRewardedAd(UpgradaleData upgradaleData, UpgradeID upgradableDataID, System.Action updateUI)
        {
            rewardedVideoAd.LoadAd(upgradaleData, upgradableDataID, updateUI);

        }

    }

    public enum AdType { Interstitial, Banner, Rewarded }
    
    public abstract class SmallyAd : IUnityAdsShowListener, IUnityAdsLoadListener, IUnityAdsInitializationListener
    {
        [SerializeField]
        private string _androidAdUnitId;

        [SerializeField]
        private string _iosAdUnitId;

        [ShowInInspector, ReadOnly]
        protected bool canPlayAd = false;

        protected string _adUnitId;

        public virtual void Init()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iosAdUnitId
            : _androidAdUnitId;

            //Advertisement.Initialize(id, testMode);
        }

        public virtual void LoadAd()
        {
            Advertisement.Load(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            canPlayAd = true;
            ShowAd();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
           // throw new System.NotImplementedException();
        }

        protected virtual void ShowAd()
        {
            Advertisement.Show(_adUnitId, this);
            //Debug.Log($"{_adUnitId} Ad Showed");
            /*if (canPlayAd)
            {
                Advertisement.Show(_adUnitId, this);
            }*/
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            //throw new System.NotImplementedException();
        }

        public virtual void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
           // throw new System.NotImplementedException();
        }

        public void OnInitializationComplete()
        {
            canPlayAd = true;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            //throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    public class InterstitialAd : SmallyAd
    {

    }

    [System.Serializable]
    public class BannerAd : SmallyAd
    {
        /*public override void LoadAd()
        {
            //Advertisement.Banner.islo
            Debug.Log("Called Load Banner");
            canPlayAd = true;
            Advertisement.Banner.Load(_adUnitId, new BannerLoadOptions { loadCallback = ShowAd});
            *//*DOVirtual.Float(0f, 1f, 2f, (x) => { }).OnComplete(() =>
              {
                  if (Advertisement.Banner.isLoaded)
                  {
                      ShowAd();
                  }
              });*//*

        }*/

        protected override void ShowAd()
        {
            Debug.Log("Show Banner");
            Advertisement.Show(_adUnitId);
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }
    }

    [System.Serializable]
    public class RewardedVideoAd : SmallyAd
    {
        private UpgradaleData upgradaleData;
        private UpgradeID upgradableDataID;
        private System.Action updateUI;

        private bool upgradeReward;

        public void LoadAd(UpgradaleData upgradaleData, UpgradeID upgradableDataID, System.Action updateUI)
        {
            this.upgradaleData = upgradaleData;
            this.upgradableDataID = upgradableDataID;
            this.updateUI = updateUI;
            upgradeReward = true;
            base.LoadAd();
        }

        public override void LoadAd()
        {
            upgradeReward = false;
            base.LoadAd();
        }

        public override void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            //base.OnUnityAdsShowComplete(placementId, showCompletionState);

            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    Debug.Log("Rewarded Ad Played");
                    if (upgradeReward)
                    {
                        upgradaleData.UpgradeData(upgradableDataID, true);
                        updateUI();
                    }
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    break;
                default:
                    break;
            }
        }
    }

}
