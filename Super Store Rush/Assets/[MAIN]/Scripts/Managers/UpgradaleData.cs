using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using SmallyTools;

namespace SuperStoreRush
{
    [CreateAssetMenu(menuName = "SmallyGames/Data Handler/Upgradable Data", fileName = "Upgradable Data")]
    public class UpgradaleData : ScriptableObject
    {

        [BoxGroup("Offloader Upgrades", centerLabel: true)]
        [SerializeField, BoxGroup("Offloader Upgrades/Time")]
        private UpgradableDataItem offloadTime;
        public UpgradableDataItem OffloadTime => offloadTime;

        [SerializeField, BoxGroup("Offloader Upgrades/Items", showLabel: false)]
        private UpgradableDataItem offloadedPackageCount;
        public UpgradableDataItem OffloadedPackageCount => offloadedPackageCount;

        //========================================
        [SerializeField, BoxGroup("Player Upgrades", centerLabel: true)]
        private UpgradableDataItem playerStackCapacity;
        public UpgradableDataItem PlayerStackCapacity => playerStackCapacity;

        //========================
        [SerializeField, BoxGroup("Register Upgrades", centerLabel: true)]
        private UpgradableDataItem registerWorkDuration;
        public UpgradableDataItem RegisterWorkDuratoin => registerWorkDuration;


        //=============================
        [SerializeField, BoxGroup("Products Upgrades", centerLabel: true)]
        private UpgradableDataItem supermarketProductPrice;
        public UpgradableDataItem SuperMarketProductPrice => supermarketProductPrice;


        [SerializeField, BoxGroup("Stocker Upgrades", centerLabel: true)]
        private UpgradableDataItem stockerCount;
        public UpgradableDataItem StockerCount => stockerCount;

        [SerializeField, BoxGroup("Stocker Upgrades", centerLabel: true)]
        private UpgradableDataItem stockerStackCapacity;
        public UpgradableDataItem StockerStackCapactity => stockerStackCapacity;

        [SerializeField, BoxGroup("Customer Upgrades", centerLabel: true)]
        private UpgradableDataItem customerCount;
        public UpgradableDataItem CustomerCount => customerCount;

        [SerializeField, BoxGroup("Stocker Upgrades", centerLabel: true)]
        private UpgradableDataItem workerSpeed;
        public UpgradableDataItem WorkerSpeed => workerSpeed;

        [SerializeField, BoxGroup("Cashier Upgrades", centerLabel: true)]
        private UpgradableDataItem cashierCount;
        public UpgradableDataItem CashierCount => cashierCount;



        private int upgradePriceValue = 600;

        public void UpgradeData(UpgradeID id, bool isReward)
        {
            switch (id)
            {
                case UpgradeID.HireStocker:
                    if (CurrencyController.Instance.GetMoney() >= stockerCount.Price && !stockerCount.IsMaxedOut || isReward)
                    {
                        Debug.Log("purchasing stocker");
                        stockerCount.Upgrade(upgradePriceValue, isReward);
                        GameManager.Instance.PurchasedAI(AIType.Stocker);

                    }
                    break;
                case UpgradeID.supermarketProductPrice:
                    if (CurrencyController.Instance.GetMoney() >= supermarketProductPrice.Price && !supermarketProductPrice.IsMaxedOut || isReward)
                    {
                        supermarketProductPrice.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.registerWorkDuration:
                    if (CurrencyController.Instance.GetMoney() >= registerWorkDuration.Price && !registerWorkDuration.IsMaxedOut || isReward)
                    {
                        registerWorkDuration.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.CustomerCount:
                    if (CurrencyController.Instance.GetMoney() >= customerCount.Price && !customerCount.IsMaxedOut || isReward)
                    {
                        customerCount.Upgrade(upgradePriceValue, isReward);
                        GameManager.Instance.PurchasedAI(AIType.Customer);
                    }
                    break;
                case UpgradeID.OffloaderTime:
                    if (CurrencyController.Instance.GetMoney() >= offloadTime.Price && !offloadTime.IsMaxedOut || isReward)
                    {
                        offloadTime.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.StockersStackCapacity:
                    if (CurrencyController.Instance.GetMoney() >= stockerStackCapacity.Price && !stockerStackCapacity.IsMaxedOut || isReward)
                    {
                        stockerStackCapacity.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.OffloaderPackageCount:
                    if (CurrencyController.Instance.GetMoney() >= offloadedPackageCount.Price && !offloadedPackageCount.IsMaxedOut || isReward)
                    {
                        offloadedPackageCount.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.PlayerStackCapacity:
                    if (CurrencyController.Instance.GetMoney() >= playerStackCapacity.Price && !playerStackCapacity.IsMaxedOut || isReward)
                    {
                        playerStackCapacity.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.WorkerSpeed:
                    if (CurrencyController.Instance.GetMoney() >= workerSpeed.Price && !workerSpeed.IsMaxedOut || isReward)
                    {
                        workerSpeed.Upgrade(upgradePriceValue, isReward);
                    }
                    break;
                case UpgradeID.HireCashier:
                    if (CurrencyController.Instance.GetMoney() >= cashierCount.Price && !cashierCount.IsMaxedOut || isReward)
                    {
                        cashierCount.Upgrade(upgradePriceValue, isReward);
                        GameManager.Instance.PurchasedAI(AIType.Cashier);
                    }
                    break;
                default:
                    throw new System.ArgumentNullException("Button clicked but Hr upgradable content not available");
                    //break;
            }
        }

        public (string title, Sprite icon, float price, bool isMaxedOut) InitUpgradableDataUI(UpgradeID id)
        {
            switch (id)
            {
                
                case UpgradeID.HireStocker:
                    return ($"{stockerCount.Title} ({stockerCount.Value})", stockerCount.Icon, stockerCount.Price, stockerCount.IsMaxedOut);

                
                case UpgradeID.supermarketProductPrice:
                    return ($"{supermarketProductPrice.Title}", supermarketProductPrice.Icon, supermarketProductPrice.Price, supermarketProductPrice.IsMaxedOut);

               
                case UpgradeID.registerWorkDuration:
                    return ($"{registerWorkDuration.Title}", registerWorkDuration.Icon, registerWorkDuration.Price, registerWorkDuration.IsMaxedOut);

                
                case UpgradeID.CustomerCount:
                    return ($"{customerCount.Title} ({customerCount.Value})", customerCount.Icon, customerCount.Price, customerCount.IsMaxedOut);

                
                case UpgradeID.OffloaderTime:
                    return ($"{offloadTime.Title}", offloadTime.Icon, offloadTime.Price, offloadTime.IsMaxedOut);

                
                case UpgradeID.OffloaderPackageCount:
                    return ($"{offloadedPackageCount.Title}", offloadedPackageCount.Icon, offloadedPackageCount.Price, offloadedPackageCount.IsMaxedOut);

                
                case UpgradeID.StockersStackCapacity:
                    return ($"{stockerStackCapacity.Title}  ({stockerStackCapacity.Value})", stockerStackCapacity.Icon, stockerStackCapacity.Price, stockerStackCapacity.IsMaxedOut);

                
                case UpgradeID.PlayerStackCapacity:
                    return ($"{playerStackCapacity.Title} ({PlayerStackCapacity.Value})", PlayerStackCapacity.Icon, PlayerStackCapacity.Price, PlayerStackCapacity.IsMaxedOut);
                
                case UpgradeID.WorkerSpeed:
                    return ($"{workerSpeed.Title}", workerSpeed.Icon, workerSpeed.Price, workerSpeed.IsMaxedOut);
                
                case UpgradeID.HireCashier:
                    return ($"{cashierCount.Title} ({cashierCount.Value})", cashierCount.Icon, cashierCount.Price, cashierCount.IsMaxedOut);
                default:
                    return ($"Error ({stockerCount.Value})", stockerCount.Icon, stockerCount.Price, stockerCount.IsMaxedOut);
            }
        }


        [System.Serializable]
        public class UpgradableDataItem
        {
            [SerializeField, BoxGroup("Static", showLabel:false)]
            private string key;

            [SerializeField, BoxGroup("Static", showLabel: false)]
            private string title;
            public string Title => title;

            [SerializeField, BoxGroup("Static", showLabel: false)]
            private Sprite icon;
            public Sprite Icon => icon;

            [ShowInInspector,ReadOnly, BoxGroup("NonStatic", showLabel: false)]
            private string price_key_suffix = "_price";

            [SerializeField, BoxGroup("NonStatic", showLabel:false)]
            private int price = 600;

            public int Price
            {
                get
                {
                    var priceKey = key + price_key_suffix;
                   
                    if (SaveGameManager.Instance.KeyExists(priceKey))
                    {
                        return SaveGameManager.Instance.GetInt(priceKey);
                    }
                    else
                    {
                        return price;
                    }
                }
                set
                {
                    var priceKey = key + price_key_suffix;
                    SaveGameManager.Instance.SaveValue(priceKey, price = value);    
                }
            }

            [ShowInInspector, ReadOnly, BoxGroup("NonStatic", showLabel: false)]
            private string value_key_suffix = "_value";

            [SerializeField, BoxGroup("NonStatic", showLabel: false)]
            private float _value;

            public float Value
            {
                
                get
                {
                    var valKey = key + value_key_suffix;
                    if (SaveGameManager.Instance.KeyExists(valKey))
                    {
                        return _value = SaveGameManager.Instance.GetFloat(valKey);
                    }
                    else
                    {
                        return _value;
                    }
                }
                set
                {
                    var valKey = key + value_key_suffix;
                    if (!isTimeBased)
                    {
                        if (value >= maxValue)
                        {
                            SaveGameManager.Instance.SaveValue(valKey, _value = maxValue);
                        }
                        else
                        {
                            SaveGameManager.Instance.SaveValue(valKey, _value = value);
                        }
                    }
                    else
                    {
                        if (value <= maxValue)
                        {
                            SaveGameManager.Instance.SaveValue(valKey, _value = maxValue);
                        }
                        else
                        {
                            SaveGameManager.Instance.SaveValue(valKey, _value = value);
                        }
                    }
                }
            }

            [SerializeField, BoxGroup("Static", showLabel: false)]
            private float upgradeValue;

            [SerializeField, BoxGroup("Static", showLabel: false)]
            private float maxValue;

            [SerializeField, BoxGroup("Static", showLabel: false)]
            private bool isTimeBased;

            public bool IsMaxedOut
            {
                get
                {
                    if (!isTimeBased)
                    {
                        return Value >= maxValue;
                    }
                    else
                    {
                        return Value <= maxValue;
                    }
                }
            }

            public void Upgrade(int upgradePrice, bool isReward)
            {
                /*if(isTimeBased)
                {
                    if (Value <= maxValue)
                    {
                        Debug.Log("Max Upgrade Capacity");
                        return;
                    }
                }
                else
                {
                    if (Value >= maxValue)
                    {
                        Debug.Log("Max Upgrade Capacity");
                        return;
                    }
                }*/
                if (!isReward)
                {
                    //CurrencyController.Instance.AddMoney(-Price);
                    GameManager.Instance.AddToMoney(-Price);
                    Price += upgradePrice;
                    GameManager.Instance.OnAdEvent(AdsEvent.OnItemUpgraded);
                }
                Value += (isTimeBased) ? -upgradeValue : upgradeValue;
                GameManager.Instance.OnPurchasedHRUpgrade();
                GameManager.Instance.PlayHaptic();
                
            }
        }
    }
}
