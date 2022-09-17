using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    [CreateAssetMenu(menuName = "SmallyGames/Data Handler/Ads Counter")]
    [InfoBox("Used to play ads on specific event conditions")]
    public class AdsCounter : ScriptableObject
    {
        [ShowInInspector, ReadOnly, BoxGroup("A", showLabel:false)]
        private int customerCheckedout = 0;

        [SerializeField, BoxGroup("A", showLabel: false)]
        private int max_customerCheckoutCount = 2;


        [ShowInInspector, ReadOnly, BoxGroup("B", showLabel: false)]
        private int itemUpgraded = 0;

        [SerializeField, BoxGroup("B", showLabel: false)]
        private int max_itemUpgradeCount = 1;


        [ShowInInspector, ReadOnly, BoxGroup("C", showLabel: false)]
        private int interactableUnlocked = 0;

        [SerializeField, BoxGroup("C", showLabel: false)]
        private int max_interactableUnlockCount = 2;


        [ShowInInspector, ReadOnly, BoxGroup("D", showLabel: false)]
        private int moneyCollected = 0;

        [SerializeField, BoxGroup("D", showLabel: false)]
        private int max_moneyCollectedCounnt = 5;

        [SerializeField, BoxGroup("E", showLabel: false)]
        private Store_XP[] store_XPs;



        public void AddEvent(AdsEvent adsEvent)
        {
            switch (adsEvent)
            {
                case AdsEvent.OnInteractableUnlocked:
                    OnInteractableUnlocked();
                    break;
                case AdsEvent.OnCustomerCheckedOut:
                    OnCustomerCheckedOut();
                    break;
                case AdsEvent.OnItemUpgraded:
                    OnItemUpgraded();
                    break;
                case AdsEvent.OnMoneyCollected:
                    OnMoneyCollected();
                    break;
                default:
                    break;
            }
        }

        private void OnCustomerCheckedOut()
        {
            customerCheckedout += 1;
            if (customerCheckedout >= max_customerCheckoutCount)
            {
                store_XPs[0].AddXP();
                //play ad
                customerCheckedout = 0;
                //AdsManager.Instance.PlayAd(AdType.Interstitial);
                Debug.Log("Play AD - customer Checkout");
            }
        }

        private void OnItemUpgraded()
        {
            itemUpgraded += 1;

            if (itemUpgraded >= max_itemUpgradeCount)
            {
                //play ad
                itemUpgraded = 0;
                //AdsManager.Instance.PlayAd(AdType.Interstitial);
                Debug.Log("Play AD - item upgrade");
            }
        }

        private void OnInteractableUnlocked()
        {
            interactableUnlocked += 1;
            store_XPs[0].AddXP();

            if (interactableUnlocked >= max_interactableUnlockCount)
            {
                //play ad
                interactableUnlocked = 0;
                //AdsManager.Instance.PlayAd(AdType.Interstitial);
                Debug.Log("Play AD - Interactable Unlocked");
            }
        }

        private void OnMoneyCollected()
        {
            moneyCollected += 1;
            store_XPs[0].AddXP();

            if (moneyCollected >= max_moneyCollectedCounnt)
            {
                //play ad
                moneyCollected = 0;
                //AdsManager.Instance.PlayAd(AdType.Interstitial);
                Debug.Log("Play AD - Money Collected");
            }
        }
    }
}
