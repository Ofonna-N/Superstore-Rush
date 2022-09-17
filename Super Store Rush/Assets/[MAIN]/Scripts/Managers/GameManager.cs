using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using SmallyTools;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using DG.Tweening;
using UnityEngine.SceneManagement;
using SickscoreGames.HUDNavigationSystem;

namespace SuperStoreRush
{
    public class GameManager : MonoBehaviour, ISmallySettings
    {
        #region Variables
        private static GameManager _gameManager;
        public static GameManager Instance => _gameManager;


        [SerializeField, BoxGroup("Camera Data", centerLabel:true)]
        private Camera mainCamera;
        public Camera Main_Camera => mainCamera;


        //public PackagesHandler PackagesHandler => packagesHandler;
        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private IOffloadablePool packagesPool;
        public IOffloadablePool PackagesPool => packagesPool;

        [SerializeField, BoxGroup("Money Reward Data", centerLabel: true)]
        private IOffloadablePool moneyRewardPool;
        public IOffloadablePool MoneyRewardPool => moneyRewardPool;

        [SerializeField, BoxGroup("Level Data", centerLabel: true)]
        private string[] levels;

        [SerializeField, BoxGroup("Level Data", centerLabel: true)]
        private Level currentLevel;

        [SerializeField, BoxGroup("Level Data", centerLabel: true)]
        private SmallyGames.SDKImplementation.LoadScene levelLoader;

        [SerializeField, BoxGroup("Money Data", centerLabel: true)]
        private MoneyHandler moneyHandler;

        [SerializeField, BoxGroup("Character Data", centerLabel: true)]
        private Transform playerHolder;

        [SerializeField, BoxGroup("Character Data", centerLabel: true)]
        private GameObject characterControllerPrefab;

        [SerializeField, BoxGroup("Character Data", centerLabel: true)]
        private CharacterInput characterInput;

        [SerializeField, BoxGroup("Game Feedbacks", centerLabel: true)]
        private MMF_Player purchaseHrUpgrade_Fb;

        [SerializeField, BoxGroup("Game Feedbacks", centerLabel: true)]
        private Lofelt.NiceVibrations.HapticSource haptic_Source;

        [SerializeField, BoxGroup("UI Data", centerLabel: true)]
        private SettingsController settingsController;

        [SerializeField, BoxGroup("UI Data", centerLabel: true)]
        private GameObject registerInstructionText;

        [SerializeField, BoxGroup("UI Data", centerLabel: true)]
        private HUDNavigationSystem HNS;

        [SerializeField, BoxGroup("UI Data", centerLabel: true)]
        private GameObject loadingScreen;

        [SerializeField, BoxGroup("Ads Data", centerLabel: true)]
        private AdsCounter adsCounter;

        [SerializeField, BoxGroup("Tutorial Data", centerLabel: true)]
        private TutorialManager tutorialHandler;

        [ShowInInspector, BoxGroup("Settings Data", centerLabel: true)]
        private bool hapticOn;
        #endregion


        #region Main Function Calls
        private void Awake()
        {
            if (_gameManager == null)
            {
                _gameManager = this;
            }
            else
            {
                Debug.LogError("Duplicate Managers exists!");
            }
        }

        private void Start()
        {
            packagesPool.Init();
            moneyRewardPool.Init();
            settingsController.Init(this);


            levelLoader.OnLoadSceneAsync(levels[0], LoadSceneMode.Additive);

            SceneManager.sceneLoaded += (Scene s, LoadSceneMode loadMode) =>
                {
                    //Debug.Log($"{s.name} loaded");
                    switch (loadMode)
                    {
                        case LoadSceneMode.Single:
                            break;
                        case LoadSceneMode.Additive:
                            //Debug.Log("New Level Loaded");
                            var objs = s.GetRootGameObjects();
                            currentLevel = objs[0].GetComponent<Level>();
                            currentLevel.Init();
                            tutorialHandler.Init(new List<ITutorialListeners> { currentLevel.GetHRDept() });
                            var character = Instantiate(characterControllerPrefab, playerHolder, false);
                            characterInput = character.GetComponent<CharacterInput>();
                            HNS.PlayerController = character.transform;
                            HNS.EnableSystem(true);
                            loadingScreen.SetActive(false);
                            break;
                        default:
                            break;
                    }
                };



        }

        private void Update()
        {
            //Debug.Log("Update function game manager");
            currentLevel?.AIInteractableHandler.Tick();
        }
        #endregion

        /*public IOffloadablePool GetPackagesHandler()
        {
            return packagesPool;
        }*/

        #region GameManager Functions
        public AIInteractableHandler GetAIInteractableHandler()
        {
            return currentLevel.AIInteractableHandler;
        }

        public Transform GetCustomerStartPoint(AIType aIType)
        {
            return currentLevel.GetCustomerStartPoint(aIType);
        }

        public void AddToMoney(float value)
        {
            CurrencyController.Instance.AddMoney(value);
        }

        public void AddToMoney(float value, Vector3 position)
        {
            CurrencyController.Instance.AddMoney(mainCamera, position, value);
            Debug.Log("Money Added");
        }

        public MoneyHandler GetMoneyHandler()
        {
            return moneyHandler;
        }

        public Level GetLevel()
        {
            return currentLevel;
        }

        public void SetCharacterMoveStatus(bool canMove)
        {
            currentLevel?.ActivateHRCamera(false);
            characterInput?.SetCanMove(canMove);
        }

        public void PlayHaptic()
        {
            if (!hapticOn) return;
            haptic_Source.Play();
        }

        /*public void PlaySfx(AudioClip clip)
        {
            MMSoundManager.Instance.PlaySound(clip, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
        }*/

        public void ActivateRegisterInstructionText(bool value)
        {
            registerInstructionText.gameObject.SetActive(value);
        }

        /// <summary>
        /// what happens when we click on the hr content item button
        /// to unlock new ai like stocker or customer
        /// </summary>
        public void PurchasedAI(AIType type)
        {
            currentLevel.AIInteractableHandler.AddAI(type);
        }

        public void OnPurchasedHRUpgrade()
        {
            purchaseHrUpgrade_Fb.PlayFeedbacks();
        }

        public void OnSfxButtonClicked(bool value)
        {
            if (value)
            {
                MMSoundManager.Instance.UnmuteSfx();
            }
            else
            {
                MMSoundManager.Instance.MuteSfx();
            }
        }

        public void OnHapticButtonClicked(bool value)
        {
            hapticOn = value;
            if (value) PlayHaptic();
            //MoreMountains.Tools.
            //throw new System.NotImplementedException();
        }

        public void OnAdEvent(AdsEvent adsEvent)
        {
            adsCounter.AddEvent(adsEvent);
        }

        public void SetNextTutorialStep()
        {
            if (tutorialHandler.gameObject.activeInHierarchy)
            {
                tutorialHandler.NextStep();
            }
        }
        #endregion
        /*public CMF.AdvancedWalkerController GetAdvancedWalkerController()
        {
            return advancedWalker;
        }*/

    }

    /*[System.Serializable]
    public class PackagesPool
    {
        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private int packagePoolCount = 20;

        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private GameObject packagePrefab;

        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private Transform packaesHolder;
        public Transform PackagesHolder => packaesHolder;

        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private List<Package> packages;
        //public Package[] Packages => packages;


        public void Init()
        {
            packages = new List<Package>();

            for (int i = 0; i < packagePoolCount; i++)
            {
                packages.Add(CreateNewPackage());
            }
        }

        private Package CreateNewPackage()
        {
            var package = MonoBehaviour.Instantiate(packagePrefab, packaesHolder, false).GetComponent<Package>();
            
            package.gameObject.SetActive(false);

            return package;
        }

        public Package GetPackage()
        {
            for (int i = 0; i < packages.Count; i++)
            {
                if (!packages[i].gameObject.activeInHierarchy)
                {
                    return packages[i];
                }
            }

            return CreateNewPackage();
        }
    }*/

    [System.Serializable]
    public class AIInteractableHandler
    {
        [SerializeField]
        private UpgradaleData upgradaleData;

        [SerializeField]
        private GameObject[] customerPrefabSkins;

        [SerializeField]
        private Transform customersHolder;

        [ShowInInspector, ReadOnly]
        private List<I_AI> customerControllers;

        [SerializeField]
        private Transform[] customerStartPoints;

        [SerializeField]
        private Transform[] stockerStartPoints;

        [SerializeField]
        private GameObject stockerPrefab;

        [SerializeField]
        private GameObject cashierPrefab;

        [SerializeField]
        private Transform stockersHolder;

        //[ShowInInspector, ReadOnly]
        //private Level level;

        [ShowInInspector, ReadOnly]
        private List<I_AI> stockerControllers;

        [ShowInInspector, ReadOnly]
        private List<Container> availableContainers;

        [ShowInInspector, ReadOnly]
        private List<Container> stockedContainers;

        [ShowInInspector, ReadOnly]
        private List<Register> availableRegisters;

        [ShowInInspector, ReadOnly]
        private List<PackageOffloader_Anim> availableOffloaders;

        private bool isInit;

        public void Init()
        {
            availableContainers = new List<Container>();
            availableRegisters = new List<Register>();
            availableOffloaders = new List<PackageOffloader_Anim>();
            stockedContainers = new List<Container>();
            customerControllers = new List<I_AI>();
            stockerControllers = new List<I_AI>();

            availableOffloaders.Capacity = 9;
            availableRegisters.Capacity = 8;
            
           // this.level = level;
        }

        public void Tick()
        {
            if (!isInit) return;
            for (int i = 0; i < customerControllers.Count; i++)
            {
                customerControllers[i].Tick();
            }

            for (int j = 0; j < stockerControllers.Count; j++)
            {
                stockerControllers[j].Tick();
            }
        }
        

        public void AddToAvailableInteractables(Interactable interactable)
        {
            //Debug.Log(interactable.GetType());
            if (interactable.GetType().Equals(typeof(Register)) || interactable.GetType().IsSubclassOf(typeof(Register)))
            {
                if (!availableRegisters.Contains((interactable as Register)))
                {
                    availableRegisters.Add((interactable as Register));

                    var registersWithCashiers = availableRegisters.FindAll((x) => (x as RegisterCashier).HashCashier).Count;

                    if (upgradaleData.CashierCount.Value > registersWithCashiers)
                    {
                        (interactable as RegisterCashier).SetCashier(cashierPrefab);
                        /*for (int c = 0; c < availableRegisters.Count; c++)
                        {
                            if ((availableRegisters[c] as RegisterCashier).HashCashier)
                            {
                                continue;
                            }
                            else
                            {
                                (interactable as RegisterCashier).SetCashier(MonoBehaviour.Instantiate(cashierPrefab).GetComponent<CashierController>());
                                registersWithCashiers += 1;
                                if ((int)upgradaleData.CashierCount.Value == registersWithCashiers)
                                {
                                    break;
                                }
                            }
                        }*/
                    }
                }
            }
            else if (interactable.GetType().Equals(typeof(Container)) || interactable.GetType().IsSubclassOf(typeof(Container)))
            {
                if (!availableContainers.Contains((interactable as Container)))
                {
                    availableContainers.Add((interactable as Container));
                }
            }
            else if (interactable.GetType().Equals(typeof(PackageOffloader_Anim)) || interactable.GetType().IsSubclassOf(typeof(PackageOffloader_Anim)))
            {
                if (!availableOffloaders.Contains((interactable as PackageOffloader_Anim)))
                {
                    availableOffloaders.Add((interactable as PackageOffloader_Anim));
                }
            }
            else
            {
                throw new System.TypeAccessException();
            }

            if (!isInit)
            {
                if (IsInteractableAvailable<Register>() && IsInteractableAvailable<PackageOffloader_Anim>() && IsInteractableAvailable<PackageOffloader_Anim>())
                {
                    ActivateAI();
                    Debug.Log("We have activated register, container and offloader now AIs can proceed");
                }
            }
        }

        public void RemoveFromAvailableInteractables(Interactable interactable)
        {
            if (interactable.GetType().Equals(typeof(Register)) || interactable.GetType().IsSubclassOf(typeof(Register)))
            {
                availableRegisters.Remove(interactable as Register);
            }
            else if (interactable.GetType().Equals(typeof(Container)) || interactable.GetType().IsSubclassOf(typeof(Container)))
            {
                availableContainers.Remove(interactable as Container);
            }
            else if (interactable.GetType().Equals(typeof(PackageOffloader_Anim)) || interactable.GetType().IsSubclassOf(typeof(PackageOffloader_Anim)))
            {
                availableOffloaders.Remove(interactable as PackageOffloader_Anim);
            }
            else
            {
                throw new System.TypeAccessException();
            }
        }

        public bool IsInteractableAvailable<T>() where T : Interactable
        {
            if (typeof(T).Equals(typeof(Register)) || typeof(T).IsSubclassOf(typeof(Register)))
            {
                return availableRegisters.Count > 0;
            }
            else if (typeof(T).Equals(typeof(Container)) || typeof(T).IsSubclassOf(typeof(Container)))
            {
                return availableContainers.Count > 0;
            }
            else if (typeof(T).Equals(typeof(PackageOffloader_Anim)) || typeof(T).IsSubclassOf(typeof(PackageOffloader_Anim)))
            {
                return availableOffloaders.Count > 0;
            }
            else
            {
                throw new System.TypeAccessException();
            }
        }

        public Interactable GetAvailableInteractable<T>() where T : Interactable
        {
            if (typeof(T) == typeof(Register) || typeof(T).IsSubclassOf(typeof(Register)))
            {
                int rand = Random.Range(0, availableRegisters.Count);

                return availableRegisters[rand];
            }
            else if (typeof(T) == typeof(Container) || typeof(T).IsSubclassOf(typeof(Container)))
            {
                int rand = Random.Range(0, availableContainers.Count);

                return availableContainers[rand];
            }
            else if (typeof(T) == typeof(PackageOffloader_Anim) || typeof(T).IsSubclassOf(typeof(PackageOffloader_Anim)))
            {
                int rand = Random.Range(0, availableOffloaders.Count);

                return availableOffloaders[rand];
            }
            else
            {
                throw new System.TypeAccessException();
            }
        }

        public int GetAvailableInteractableCount<T>()
        {
            if (typeof(T) == typeof(Register) || typeof(T).IsSubclassOf(typeof(Register)))
            {
                return availableRegisters.Count;
            }
            else if (typeof(T) == typeof(Container) || typeof(T).IsSubclassOf(typeof(Container)))
            {
                return availableContainers.Count;
            }
            else if (typeof(T) == typeof(PackageOffloader_Anim) || typeof(T).IsSubclassOf(typeof(PackageOffloader_Anim)))
            {
                return availableOffloaders.Count;
            }
            else
            {
                throw new System.TypeAccessException();
            }
        }

        public bool StockedContainerAvailable()
        {
            return stockedContainers.Count > 0;
        }

        public Container GetStockedContainer()
        {
            int rand = Random.Range(0, stockedContainers.Count);

            return stockedContainers[rand];
        }

        public void AddToStockedContainers(Container container)
        {
            if (stockedContainers.Contains(container))
            {
                //Debug.Log("Trying to double stock!");
                return;
            }
            stockedContainers.Add(container);
        }

        public void RemoveFromStockedContainer(Container container)
        {
            stockedContainers.Remove(container);
        }

        public void AddAI(AIType _AI)
        {
            switch (_AI)
            {
                case AIType.Customer:
                    var customer = MonoBehaviour.Instantiate(customerPrefabSkins[Random.Range(0, customerPrefabSkins.Length)], GetCustomerStartPoint(AIType.Customer).position, Quaternion.identity,
                        customersHolder).GetComponent<CustomerController>();
                    customer.Init();
                    customerControllers.Add(customer);
                    break;
                case AIType.Stocker:
                    var stocker = MonoBehaviour.Instantiate(stockerPrefab, GetCustomerStartPoint(AIType.Stocker).position, Quaternion.identity,
                        stockersHolder).GetComponent<StockerController>();
                    stocker.Init();
                    stockerControllers.Add(stocker);
                    break;
                case AIType.Cashier:

                    var registersWithCashiers = availableRegisters.FindAll((x) => (x as RegisterCashier).HashCashier).Count;

                    if (upgradaleData.CashierCount.Value > registersWithCashiers)
                    {
                        //(interactable as RegisterCashier).SetCashier(MonoBehaviour.Instantiate(cashierPrefab).GetComponent<CashierController>());
                        for (int c = 0; c < availableRegisters.Count; c++)
                        {
                            if ((availableRegisters[c] as RegisterCashier).HashCashier)
                            {
                                continue;
                            }
                            else
                            {
                                (availableRegisters[c] as RegisterCashier).SetCashier(cashierPrefab);
                                //registersWithCashiers += 1;
                                break;
                                /*if ((int)upgradaleData.CashierCount.Value == registersWithCashiers)
                                {
                                    break;
                                }*/
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void ActivateAI()
        {

            for (int i = 0; i < upgradaleData.CustomerCount.Value; i++)
            {
                AddAI(AIType.Customer);
            }

            for (int j = 0; j < upgradaleData.StockerCount.Value; j++)
            {
                AddAI(AIType.Stocker);
            }

            isInit = true;
        }

        public Transform GetCustomerStartPoint(AIType aIType)
        {
            switch (aIType)
            {
                case AIType.Customer:
                    return customerStartPoints[Random.Range(0, customerStartPoints.Length)];
                case AIType.Stocker:
                    return stockerStartPoints[Random.Range(0, stockerStartPoints.Length)];
                default:
                    return customerStartPoints[Random.Range(0, customerStartPoints.Length)];
            }
            
        }


    }

    [System.Serializable]
    public class MoneyHandler
    {
        [SerializeField]
        private MMMiniObjectPooler moneyPooler;

        [SerializeField]
        private MMF_Player moneyCollectedFb;

        [SerializeField]
        private UpgradaleData upgradaleData;


        [SerializeField]
        private float moneyForwardJumpOffset = 5f;

        public void OnMoneyCollected(Vector3 pos)
        {
            moneyCollectedFb.PlayFeedbacks(pos);
            //CurrencyController.Instance.AddMoney();
            GameManager.Instance.AddToMoney(upgradaleData.SuperMarketProductPrice.Value);
        }

        public void OnMoneyCollected(Vector3 pos, float amount)
        {
            moneyCollectedFb.PlayFeedbacks(pos);
            Debug.Log($"${amount} is added from vending machine");
            GameManager.Instance.AddToMoney(amount, pos);
           // CurrencyController.Instance.AddMoney(amount);
        }

        public void SpawnMoneyAtRegister(Register register)
        {
            var money = moneyPooler.GetPooledGameObject().GetComponent<Money>();
            money.gameObject.SetActive(true);
            money.ActivateTrigger(false);
            money.transform.position = register.MoneySpawnPoint.position;

            money.transform.DOJump(register.MoneySpawnPoint.position + 
                (Vector3.forward * Random.Range(moneyForwardJumpOffset, moneyForwardJumpOffset)) + (Vector3.right * Random.Range(2f, 3f)), 5f, 1, 1f)
                .OnComplete(()=>
                {
                    money.ActivateTrigger(true);
                }).SetEase(Ease.OutBounce);
        }
    }
}
