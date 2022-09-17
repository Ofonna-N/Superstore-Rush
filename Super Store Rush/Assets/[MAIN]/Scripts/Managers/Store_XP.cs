using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using SmallyTools;

namespace SuperStoreRush
{
    [CreateAssetMenu(fileName = "Store XP", menuName = "SmallyGames/Data/Store XP")]
    public class Store_XP : ScriptableObject
    {
        [ShowInInspector, ReadOnly]
        private int xp = 0;

        public int XP
        {
            get
            {
                if (SaveGameManager.Instance.KeyExists(name))
                {
                    return xp = SaveGameManager.Instance.GetInt(name);
                }
                else
                {
                    return xp;
                }
                //return isUnlocked;
            }
            set
            {
                SaveGameManager.Instance?.SaveValue(name, xp = value);
            }
        }


        [SerializeField]
        private UpgradaleData storeUpgradableData;

        public void AddXP()
        {
            XP += 1;
            switch (XP)
            {
                case 10:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 50:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 200:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 300:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 450:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 700:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 1000:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 1400:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 2000:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 2500:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 5000:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                case 10000:
                    storeUpgradableData.UpgradeData(UpgradeID.CustomerCount, true);
                    break;
                default:
                    break;
            }
        }
    }
}
