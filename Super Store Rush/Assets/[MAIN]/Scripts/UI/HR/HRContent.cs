using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using SmallyTools;

namespace SuperStoreRush
{
    public class HRContent : MonoBehaviour
    {
        [SerializeField, BoxGroup("HR UI Data")]
        private UpgradaleData upgradaleData;

        [SerializeField, BoxGroup("HR UI Data")]
        private UpgradeID upgradeID;

        /*[ShowInInspector, ReadOnly, BoxGroup("HR UI Data")]
        private float price;

        [ShowInInspector, ReadOnly, BoxGroup("HR UI Data")]
        private float value;*/

        [SerializeField, BoxGroup("HR Components")]
        private Image iconImg;

        [SerializeField, BoxGroup("HR Components")]
        private TextMeshProUGUI titleText;

        [SerializeField, BoxGroup("HR Components")]
        private TextMeshProUGUI priceText;

        [SerializeField, BoxGroup("HR Components")]
        private UIButton upgradeButton;

        [SerializeField, BoxGroup("HR Components")]
        private UIButton adUpgradeButton;

        // Start is called before the first frame update
        void Start()
        {
            UpdateUI();
        }

        protected virtual void UpdateUI()
        {
            var upgradeData = upgradaleData.InitUpgradableDataUI(upgradeID);

            titleText.text = upgradeData.title;
            iconImg.sprite = upgradeData.icon;
            priceText.text = $"{CurrencyController.CurrencyFormat(upgradeData.price)}<sprite=0>";

            if (upgradeData.isMaxedOut)
            {
                upgradeButton.interactable = false;
                adUpgradeButton.interactable = false;
            }
        }

        public void UpgradeButtonClicked()
        {
            Debug.Log($"{upgradeID} upgrade");
            upgradaleData.UpgradeData(upgradeID, false);

            UpdateUI();
        }

        public void UpgradeAdClicked()
        {
            Debug.Log($"{upgradeID} Ad upgrade");
            //AdsManager.Instance.PlayHRRewardedAd(upgradaleData, upgradeID, UpdateUI);
        }
    }
}
