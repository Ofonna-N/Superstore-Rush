using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using SmallyTools;
using UnityEngine.AI;



namespace SuperStoreRush
{
    public interface I_InteractionStates
    {
        public bool StateON { get; }
        public void OpenState();
        public void UpdateState();
        public void CloseState();
    }

    public interface I_AI
    {
        public void Init();
        public void Tick();
    }

    public interface I_CustomerStates
    {
        public bool StateON { get; }
        public void OpenState();
        public void UpdateState();
        public bool CloseState();
    }

    public interface I_StockerStates
    {
        public bool StateON { get; }
        public void OpenState();
        public void UpdateState();
        public bool CloseState();
    }

    public interface IUnlockable
    {
        public void UpdateUnlockDelayIndicator(float fillValue);
        public void Unlock(bool init);
        public bool IsUnlocked();
        public void AddPrice(float value);

        public float GetPrice();
    }

    public interface IPlaceable
    {
        public GameObject GetItemToPlace();

        public void PlaceItem(GameObject item);

        public bool IsFull();
    }

    public interface ITutorialListeners
    {
        public void OnTutorialWatched();
    }

    public interface IOffloadable
    {
        public OffloadableItemState State { get; }

        public void Init();

        public void SetOffloadState(OffloadableItemState state);
    }

    /*public interface IPurchaseable
    {
        public GameObject GetItemToPurchase();

        public void PlaceItemInCart();
    }*/

    //========ENUMS===========
    public enum PlayerStates { Idle, Stacking, Placing, Paying, WorkingRegister, CollectingReward };

    public enum CustomerStates { Idle, ToShelf, GrabItem, GoToRegister, LeaveStore };

    public enum StockerStates { Idle, ToOffloader, StackPackages, ToShelf, PlacePackages };

    public enum UpgradeID { HireStocker, StockersStackCapacity, supermarketProductPrice, registerWorkDuration, CustomerCount, OffloaderTime, OffloaderPackageCount, PlayerStackCapacity, WorkerSpeed, HireCashier }

    public enum AIType { Customer, Stocker, Cashier }

    public enum AdsEvent { OnInteractableUnlocked, OnCustomerCheckedOut, OnItemUpgraded, OnMoneyCollected }

    public enum OffloadableItemState { Idle, Offloading, Offloaded }

    //------------Implementations

    //----------INTERACTION

    #region PLAYER STATES
    public class Idle_Player : I_InteractionStates
    {
        private bool stateOn;
        public bool StateON { get => stateOn; }

        public void OpenState()
        {
            stateOn = true;
            //throw new System.NotImplementedException();
            // Debug.Log("Idle State ON...");
        }


        public void UpdateState()
        {
            //throw new System.NotImplementedException();
            if (!stateOn) return;
            //OnIdle();
        }

        public void CloseState()
        {
            stateOn = false;
            //Debug.Log("Idle OFF...");
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// what does the customer do while they are in an idle state
        /// </summary>


    }

    public class Stacking_Player : I_InteractionStates
    {
        private bool stateOn;
        public bool StateON => stateOn;
        private bool isStacking = false;

        CharacterRigRef characterRig;
        PackageOffloader_Anim offloader;
        Stack<Package> packagesStack;
        UpgradaleData upgradaleData;
        CharacterInput characterInput;
        MoreMountains.Feedbacks.MMF_Player stackPackage_Fb;

        private float placementOffset = 0.312f;

        public Stacking_Player(CharacterRigRef rig, Stack<Package> packages, UpgradaleData ud, CharacterInput charInp)
        {
            characterRig = rig;
            packagesStack = packages;
            upgradaleData = ud;
            characterInput = charInp;
        }

        public void OpenState()
        {
            stateOn = true;
            Debug.Log("Stacking State ON...");
        }

        public void OpenState(Offloader o, MoreMountains.Feedbacks.MMF_Player stackPackage_Fb)
        {
            stateOn = true;
            offloader = o as PackageOffloader_Anim;
            SetAnimRigData(true, 1f);
            this.stackPackage_Fb = stackPackage_Fb;
        }

        private void SetAnimRigData(bool lift, float weight)
        {
            characterRig.Anim.SetBool(ConstantStrings.Lifting_Param, lift);

            DOVirtual.Float(characterRig.CarryIkRig.weight, weight, 0.25f, ((x) =>
            {
                characterRig.CarryIkRig.weight = x;
            }));
        }

        public void UpdateState()
        {
            //throw new System.NotImplementedException();
            //Debug.Log(isStacking);
            if (!stateOn || isStacking || characterInput.IsMoving() || !offloader.IsItemAvailable()) return;
            Stack();
            //Debug.Log("Stacking...");
        }

        public void CloseState()
        {
            stateOn = false;
            isStacking = false;
            if (packagesStack.Count <= 0)
            {
                SetAnimRigData(false, 0f);
            }
            //Debug.Log("No longer Stacking");
            //throw new System.NotImplementedException();
        }

        private void Stack()
        {
            isStacking = true;

            if (packagesStack.Count < upgradaleData.PlayerStackCapacity.Value)
            {
                var package = GameManager.Instance.PackagesPool.GetPackage() as Package;
                
                package.transform.position = (offloader.RetreivePackagePosition() as Package).transform.position;
                package.transform.parent = characterRig.PackagesStackHolder;
                package.SetOffloadState(OffloadableItemState.Offloaded);

                package.transform.DOLocalJump((packagesStack.Count > 0) ? new Vector3(0f, placementOffset + packagesStack.Peek().transform.localPosition.y, 0f)
                    : Vector3.zero, 1f, 1, .15f)
                    .OnComplete(() =>
                    {
                        package.transform.localRotation = Quaternion.identity;
                        offloader.ItemRetreived();
                        packagesStack.Push(package);
                        GameManager.Instance.PlayHaptic();
                        GameManager.Instance.SetNextTutorialStep();
                        stackPackage_Fb.PlayFeedbacks();
                        isStacking = false;
                    });
            }
            else
            {
                Debug.Log("Max Capacity");
            }

        }
    }

    public class CollectReward_Player : I_InteractionStates
    {
        private bool stateOn;
        public bool StateON => stateOn;
        private bool collecting = false;

        //CharacterRigRef characterRig;
        PackageOffloader_Spawn offloader;
        //Stack<IOffloadable> packagesStack;
        //UpgradaleData upgradaleData;
        CharacterInput characterInput;
        //MoreMountains.Feedbacks.MMF_Player stackPackage_Fb;

        //private float placementOffset = 0.312f;

        public CollectReward_Player(CharacterInput charInp)
        {
            //characterRig = rig;
            //packagesStack = packages;
            //upgradaleData = ud;
            characterInput = charInp;
        }

        public void OpenState()
        {
            stateOn = true;
            Debug.Log("Stacking State ON...");
        }

        public void OpenState(Offloader o)
        {
            stateOn = true;
            offloader = o as PackageOffloader_Spawn;
            offloader.PlayerCollecting = true;
            //SetAnimRigData(true, 1f);
            //this.stackPackage_Fb = stackPackage_Fb;
        }

        /*private void SetAnimRigData(bool lift, float weight)
        {
            //characterRig.Anim.SetBool(ConstantStrings.Lifting_Param, lift);

            DOVirtual.Float(characterRig.CarryIkRig.weight, weight, 0.25f, ((x) =>
            {
                characterRig.CarryIkRig.weight = x;
            }));
        }*/

        public void UpdateState()
        {
            //throw new System.NotImplementedException();
            //Debug.Log(isStacking);
            if (!stateOn || collecting || characterInput.IsMoving() || !offloader.IsItemAvailable()) return;
            Collect();
            //Debug.Log("Stacking...");
        }

        public void CloseState()
        {
            stateOn = false;
            collecting = false;
            offloader.PlayerCollecting = false;
            offloader.ItemRetreived();
            /*if (packagesStack.Count <= 0)
            {
                SetAnimRigData(false, 0f);
            }*/
            //Debug.Log("No longer Stacking");
            //throw new System.NotImplementedException();
        }


        private void OnCollected()
        {
            //item.transform.localRotation = Quaternion.identity;
            //offloader.ItemRetreived();
            Debug.Log("Money Reward Collected");
            //packagesStack.Push(package);
            //GameManager.Instance.PlayHaptic();
            //item.SetOffloadState(OffloadableItemState.Idle);
            //GameManager.Instance.SetNextTutorialStep();
            //stackPackage_Fb.PlayFeedbacks();
            collecting = false;
        }
        //Money_Reward item;
        private void Collect()
        {
            collecting = true;

            var item = GameManager.Instance.MoneyRewardPool.GetPackage() as Money_Reward;

            item.transform.position = (offloader.RetreivePackagePosition() as Money_Reward).transform.position;

            item.SetOffloadState(OffloadableItemState.Offloading);
            item.gameObject.SetActive(true);
            item.PlayerCollect(characterInput.transform, OnCollected);

            //package.transform.parent = characterRig.PackagesStackHolder;
            //item.gameObject.SetActive(true);

            /*item.transform.DOLocalJump(, 1f, 1, .15f)
                .OnComplete(() =>
                {
                    item.transform.localRotation = Quaternion.identity;
                    offloader.ItemRetreived();
                    //packagesStack.Push(package);
                    GameManager.Instance.PlayHaptic();
                    GameManager.Instance.SetNextTutorialStep();
                    //stackPackage_Fb.PlayFeedbacks();
                    collecting = false;
                });*/

            /*if (offloader.IsItemAvailable())
            {
                var package = GameManager.Instance.GetPackagesHandler().GetPackage();

                package.transform.position = offloader.RetreivePackagePosition();
                //package.transform.parent = characterRig.PackagesStackHolder;
                package.gameObject.SetActive(true);

                package.transform.DOLocalJump((packagesStack.Count > 0) ? new Vector3(0f, placementOffset + packagesStack.Peek().transform.localPosition.y, 0f)
                    : Vector3.zero, 1f, 1, .15f)
                    .OnComplete(() =>
                    {
                        package.transform.localRotation = Quaternion.identity;
                        offloader.ItemRetreived();
                        packagesStack.Push(package);
                        GameManager.Instance.PlayHaptic();
                        GameManager.Instance.SetNextTutorialStep();
                        stackPackage_Fb.PlayFeedbacks();
                        isStacking = false;
                    });
            }
            else
            {
                Debug.Log("Max Capacity");
            }*/

        }
    }

    public class Placing_Player : I_InteractionStates
    {
        private bool stateOn;
        public bool StateON { get => stateOn; }
        private bool isPlacing = false;

        IPlaceable container;
        Stack<Package> playerPackages;
        CharacterRigRef characterRig;
        CharacterInput characterInput;

        public Placing_Player(Stack<Package> packages, CharacterRigRef characterRig, CharacterInput charInp)
        {
            playerPackages = packages;
            this.characterRig = characterRig;
            characterInput = charInp;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(IPlaceable container)
        {
            stateOn = true;
            this.container = container;
            //Debug.Log("In placement state");
        }

        public void UpdateState()
        {
            //throw new System.NotImplementedException();
            if (!stateOn || isPlacing || container.IsFull() || playerPackages.Count <= 0 || characterInput.IsMoving()) return;
            Place();
        }

        public void CloseState()
        {
            stateOn = false;
            isPlacing = false;
            /*if (playerPackages.Count <= 0)
            {
                SetAnimRigData(false, 0f);
            }*/
        }

        private void Place()
        {
            isPlacing = true;

            var package = playerPackages.Pop();

            package.transform.parent = GameManager.Instance.PackagesPool.transform;

            var itemToPlace = container.GetItemToPlace();

            package.transform.DOJump(itemToPlace.transform.position, 1f, 1, .15f)
                    .OnComplete(() =>
                    {
                        container.PlaceItem(itemToPlace);
                        //package.gameObject.SetActive(false);
                        package.SetOffloadState(OffloadableItemState.Idle);
                        if (playerPackages.Count <= 0) SetAnimRigData(false, 0f);
                        GameManager.Instance.SetNextTutorialStep();
                        isPlacing = false;
                    });
        }

        private void SetAnimRigData(bool lift, float weight)
        {
            characterRig.Anim.SetBool(ConstantStrings.Lifting_Param, lift);

            DOVirtual.Float(characterRig.CarryIkRig.weight, weight, 0.25f, ((x) =>
            {
                characterRig.CarryIkRig.weight = x;
            }));
        }
    }

    public class Unlocking_Player : I_InteractionStates
    {
        private bool stateOn;
        public bool StateON => stateOn;
        private bool isPaying = false;

        private bool canUnlock; // used to control delay before we start unlocking

        private float unlockDelayLerpValue = 0f;

        CharacterInput characterInput;
        IUnlockable unlockableItem;
        float step;

        public Unlocking_Player(CharacterInput charInp)
        {
            characterInput = charInp;
        }

        public void OpenState()
        {
            stateOn = true;
            Debug.LogError("Should be opening other open state method instead");
        }

        public void OpenState(IUnlockable unlockable)
        {
            unlockableItem = unlockable;
            stateOn = true;
        }

        public void UpdateState()
        {
            if (!canUnlock && !characterInput.IsMoving())
            {
                step += Time.deltaTime * 2f;

                unlockDelayLerpValue = Mathf.Lerp(0f, 1f, step);
                unlockableItem.UpdateUnlockDelayIndicator(unlockDelayLerpValue);
                if (unlockDelayLerpValue >= 1f)
                {
                    step = 0f;
                    unlockableItem.UpdateUnlockDelayIndicator(0f);
                    canUnlock = true;
                }
            }
            if (!stateOn || characterInput.IsMoving() || isPaying || unlockableItem.IsUnlocked() || CurrencyController.Instance.GetMoney() <= 0 || !canUnlock) return;
            Unlock();
            //Debug.Log("Is Paying...");
        }

        public void CloseState()
        {
            stateOn = false;
            isPaying = false;
            canUnlock = false;
            unlockableItem.UpdateUnlockDelayIndicator(0f);
            step = 0f;
            //throw new System.NotImplementedException();
        }

        private void Unlock()
        {
            isPaying = true;

            var unlockDedoctor = GetDedoctorValue();

            //Debug.Log($"Dedoctor: {unlockDedoctor}");

            DOVirtual.Float(0f, 1f, .025f, (x) =>
            {

            }).OnComplete(() =>
            {
                var tempDedoctor = unlockDedoctor;
                if(Mathf.Abs(unlockDedoctor) > CurrencyController.Instance.GetMoney())
                {
                    tempDedoctor = -CurrencyController.Instance.GetMoney();
                }
               // CurrencyController.Instance.AddMoney();
                GameManager.Instance.AddToMoney(tempDedoctor);
                unlockableItem.AddPrice(tempDedoctor);
                GameManager.Instance.PlayHaptic();
                isPaying = false;
            });

            float GetDedoctorValue()
            {
                if (unlockableItem.GetPrice() >= 10000)
                {
                    return -1000f;
                }
                else if (unlockableItem.GetPrice() >= 1000)
                {
                    return -200;
                }
                else if (unlockableItem.GetPrice() >= 500)
                {
                    return -50;
                }
                else
                {
                    return -50;
                }
            }
        }

    }

    public class WorkingRegister_Player : I_InteractionStates
    {
        private bool stateOn;
        public bool StateON => stateOn;
        private bool isWorkingRegister = false;

        CharacterInput characterInput;
        Register register;

        public WorkingRegister_Player(CharacterInput charInp)
        {
            characterInput = charInp;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
            //stateOn = true;
            //Debug.LogError("Should be opening other open state method instead");
        }

        public void OpenState(Register register)
        {
            Debug.Log("Register State");
            this.register = register;
            stateOn = true;
        }

        public void UpdateState()
        {
            if (!stateOn || characterInput.IsMoving() || isWorkingRegister || register.GetNoOfCustemersInQue() <= 0) return;
            WorkRegister();
            //Debug.Log("Is Paying...");
        }

        public void CloseState()
        {
            stateOn = false;
            isWorkingRegister = false;
            //throw new System.NotImplementedException();
        }

        private void WorkRegister()
        {
            isWorkingRegister = true;

            register.WorkRegister();

            isWorkingRegister = false;
            /*DOVirtual.Float(0f, 1f, .015f, (x) =>
            {

            }).OnComplete(() =>
            {
                register.get
                isWorkingRegister = false;
            });*/
        }

    }
    #endregion

    //----------------CUSTOMER--------------------

    #region CUSTOMER STATES
    public class Idle_Customer : I_CustomerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;

        private CustomerController controller;
        private NavMeshAgent agent;
        private Animator anim;

        public Idle_Customer(CustomerController controller, NavMeshAgent agent, Animator anim)
        {
            this.agent = agent;
            this.anim = anim;
            this.controller = controller;
        }

        public void OpenState()
        {
            anim.SetFloat(ConstantStrings.Movement_Param, 0f);
            agent.SetDestination(agent.transform.position);
            isStateOn = true;
            //agent.isStopped = true;
        }

        public bool CloseState()
        {
            isStateOn = false;
            return true;
            //throw new System.NotImplementedException();
        }


        public void UpdateState()
        {
            if (!isStateOn) return;
            OnIdle();
        }

        private void OnIdle()
        {
           // Debug.Log("IDle");
            if (GameManager.Instance.GetAIInteractableHandler().StockedContainerAvailable())
            {
                var shelf = GameManager.Instance.GetAIInteractableHandler().GetStockedContainer();
                Debug.Log(shelf.name);
                
                controller.SetState(CustomerStates.ToShelf, container: shelf);
                //Debug.Log("Leave Idle State");
            }
        }
    }

    public class ToShelf_Customer : I_CustomerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;

        private bool isGoingToShelf = false;

        private CustomerController controller;
        private NavMeshAgent agent;
        private Animator anim;

        private Container container;



        public ToShelf_Customer(NavMeshAgent agent, Animator anim, CustomerController controller)
        {
            this.agent = agent;
            this.anim = anim;
            this.controller = controller;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(Container container)
        {
            if (isGoingToShelf) return;
            DOVirtual.Float(0f, 1f, Random.Range(0f, 2f), (x) => isStateOn = true);
            anim.SetFloat(ConstantStrings.Movement_Param, 1f);
            agent.SetDestination(container.transform.position);
            this.container = container;
        }

        public void UpdateState()
        {
            if (!isStateOn) return;
            GoToContainer();
        }

        public bool CloseState()
        {
            //throw new System.NotImplementedException();
            isStateOn = false;
            isGoingToShelf = false;
            return true;
        }

        private void GoToContainer()
        {
            //isStateOn = false;

            //Debug.Log("Agent go to destination");
            /*if (Vector3.Distance(agent.transform.position, container.transform.position) < agent.stoppingDistance)
            {
                controller.SetState(CustomerStates.Idle);
            }*/

            //Debug.Log("Agent Reached Destination!" + agent.velocity.magnitude);
            if (agent.velocity.magnitude < 0.08f && agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
            {
                //Debug.Log("Agent Reached Destination!" + agent.remainingDistance);
                controller.SetState(CustomerStates.GrabItem, container);
            }

            if (container.IsEmpty())
            {
                //var newContainer = GameManager.Instance.GetCustomerHandler().GetAvailableContainer();
                Debug.Log("Container empty");
                controller.SetState(CustomerStates.Idle);
            }
        }
    }

    public class GrabItem_Customer : I_CustomerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;
        private bool isGrabbing = false;


        Container container;
        CustomerController controller;
        Animator anim;

        public GrabItem_Customer(CustomerController controller, Animator anim)
        {
            this.controller = controller;
            this.anim = anim;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(Container container)
        {
            isStateOn = true;
            this.container = container;
            anim.SetFloat(ConstantStrings.Movement_Param, 0f);
        }

        public void UpdateState()
        {
            if (!isStateOn || isGrabbing) return;
            Grab();
            //throw new System.NotImplementedException();
        }

        public bool CloseState()
        {
            //throw new System.NotImplementedException();
            if (isGrabbing)
            {
                return false;
            }
            else
            {
                isStateOn = false;
                return true;
            }
            /*isStateOn = false;
            return !isGrabbing;*/
        }

        Vector3 itemStartPos;

        private void Grab()
        {
            isGrabbing = true;

            var item = container.GetItemToGrab();

            if (item == null)
            {
                isGrabbing = false;
                Debug.Log("Shelft Item unavailable, look for a different shelf");
                controller.SetState(CustomerStates.Idle);
                return;
            }
            //var returnPos = item.transform.localPosition;

            item.Content.DOJump(controller.Kart.KartPosition, 2f, 1, .25f).OnStart(()=> itemStartPos = item.Content.localPosition)
                .OnComplete(() =>
                {
                    item.Content.localPosition = itemStartPos;
                    container.ItemRemovedFromShelf(item);
                    controller.Kart.ActivateItem(true);
                    isGrabbing = false;
                    controller.SetState(CustomerStates.GoToRegister, register: GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<Register>() as Register);
                });
        }
    }

    public class ToRegister_Customer : I_CustomerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;
        private bool isGoingToRegister = false;
        private bool waitAtRegister = false;
        private bool isGoingToQue = false;
        //private bool joinedQueAtRegister = false;
        // private bool advancingInQuePosition = false; // for everytime the que moves

        private CustomerController controller;
        private NavMeshAgent agent;
        private Animator anim;

        private CustomerController customerInFrontOfMe;

        private Register register;
        private int noOffCustomerInQue = 0;
        //private Vector3 customerInfrontOfMeQuePos; // usng this as our new que advance position instead of waiting for customer to move, solves glitch issue
        private float quePositionOffset = 2f;
        //private float stoppingDistance;

        //private bool awaitedPathpending = false;

        public ToRegister_Customer(NavMeshAgent agent, Animator anim, CustomerController controller)
        {
            this.agent = agent;
            this.anim = anim;
            this.controller = controller;
           // stoppingDistance = agent.stoppingDistance;
        }


        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(Register register)
        {
            if (isStateOn) return;
            isStateOn = true;
            isGoingToRegister = true;
            waitAtRegister = true;
            isGoingToQue = true;
            anim.SetFloat(ConstantStrings.Movement_Param, 1f);
            this.register = register;
            //noOffCustomerInQue = register.GetNoOfCustemersInQue();
            register.GetRegisterNotifications(CustomerAddedToQue, CustomerRemovedFromQue, true);
            //customerInFrontOfMe = register.ViewLastCustomerInQue();
            //register.AddCustomerToQue(controller);


            if (register.GetNoOfCustemersInQue() > 0)
            {
                agent.SetDestination(register.ViewLastCustomerInQue().transform.position + (Vector3.forward * quePositionOffset));
            }
            else
            {
                agent.SetDestination(register.CustomerStandPoint.position);
            }

            //this.isGoingToRegister = register;
        }

        public void UpdateState()
        {
            

            if (!isStateOn || !isGoingToRegister || agent.pathPending) return;
            //Debug.Log("Going to register");
            GoToRegister();
        }

        public bool CloseState()
        {
            if (waitAtRegister)
            {
                return false;
            }
            else
            {
                isStateOn = false;

                return true;
            }
        }

        public bool CloseState(bool registerWorked)
        {
            if (registerWorked) waitAtRegister = false;
            if (waitAtRegister)
            {
                return false;
            }
            else
            {
                isStateOn = false;
                //isGoingToQue = false;
                register.GetRegisterNotifications(CustomerAddedToQue, CustomerRemovedFromQue, false);
                //advancingInQuePosition = false;
                return true;
            }
        }



        private void GoToRegister()
        {
            //what happens if we reach our destination
            if (agent.velocity.magnitude < 0.08f && agent.remainingDistance < agent.stoppingDistance)
            {
                isGoingToQue = false;

                anim.SetFloat(ConstantStrings.Movement_Param, 0f);

                if (!register.IsCustomerStillInCue(controller))
                {
                    Debug.Log($"{controller.name} about joined Que at register");
                    Debug.Log(noOffCustomerInQue);


                    //if (register.GetNoOfCustemersInQue() > 0) customerInFrontOfMe = register.ViewLastCustomerInQue();

                    

                    register.AddCustomerToQue(controller);
                }

                /*if (customerInFrontOfMe == null)
                {
                    register.AttendToMe();
                    isGoingToRegister = false;
                }*/
                //DOVirtual.ve
                if (register.GetCustomerPositionInQue(controller) == 0)
                {
                    register.AttendToMe();
                    isGoingToRegister = false;
                }
            }

        }

        private void CustomerRemovedFromQue(CustomerController customer)
        {
            if (isGoingToQue)
            {
                //Debug.Log(register.GetCustomerPositionInQue(controller));
                anim.SetFloat(ConstantStrings.Movement_Param, 1f);
                if (register.GetNoOfCustemersInQue() > 0)
                {
                    agent.SetDestination(register.ViewLastCustomerInQue().transform.position);
                }
                else
                {
                    agent.SetDestination(register.CustomerStandPoint.transform.position);
                }
            }
            else if(isGoingToRegister)
            {
                anim.SetFloat(ConstantStrings.Movement_Param, 1f);
                if (register.GetCustomerPositionInQue(controller) > 0)
                {
                    agent.SetDestination(register.CustomerStandPoint.transform.position + (Vector3.forward * quePositionOffset * register.GetCustomerPositionInQue(controller)));
                }
                else
                {
                    agent.SetDestination(register.CustomerStandPoint.transform.position);
                }
            }
        }

        private void CustomerAddedToQue(CustomerController customer)
        {
            if (isGoingToQue)
            {
                agent.SetDestination(register.ViewLastCustomerInQue().transform.position + (Vector3.forward * quePositionOffset));
            }
        }

        /*private void CustomerAddedtoQue(CustomerController customer)
        {
            noOffCustomerInQue = register.GetNoOfCustemersInQue();

            //if we are currently on our way to the register then the customer who got to the que would be placed in front of us
            if (!register.IsCustomerStillInCue(controller) && customer != controller)
            {
                customerInFrontOfMe = register.ViewLastCustomerInQue();
                if (customerInFrontOfMe == null) throw new System.NullReferenceException();
                agent.SetDestination(customerInFrontOfMe.transform.position + (Vector3.forward * quePositionOffset));
                Debug.Log($"{controller.name} On our way to que, customer got there first so updating new position");
            }

            if (!register.IsCustomerStillInCue(controller))
            {
                Debug.Log($"{controller.name} Is in que: {register.IsCustomerStillInCue(controller)}");
            }
            Debug.Log("new customer in line");
        }*/

        /*private void CustomerRemovedFromQue(CustomerController customer)
        {
            Debug.Log($"{controller.name} advancing in position");
            //noOffCustomerInQue -= 1;
            //if we get removed from the que then that means we were just attended to by the cashier and is leaving store

            if (register.IsCustomerStillInCue(controller))
            {
                if (customerInFrontOfMe != null && register.IsCustomerStillInCue(customerInFrontOfMe))
                {
                    //Advance in que position
                    customerInfrontOfMeQuePos = customerInFrontOfMe.transform.position;
                    agent.stoppingDistance = 0.1f;
                    Debug.Log($"{controller.name} to go to {customerInFrontOfMe.name} position at {customerInfrontOfMeQuePos}");
                    //noOffCustomerInQue = register.GetNoOfCustemersInQue();
                    agent.SetDestination(customerInfrontOfMeQuePos + (Vector3.forward * quePositionOffset));
                    anim.SetFloat(ConstantStrings.Movement_Param, 1f);
                }
                else if (customerInFrontOfMe != null && !register.IsCustomerStillInCue(customerInFrontOfMe))
                {
                    customerInFrontOfMe = null;
                    Debug.Log("no more Customer in front of me, i'm goint to register");
                    agent.SetDestination(register.CustomerStandPoint.position);
                    anim.SetFloat(ConstantStrings.Movement_Param, 1f);
                }
            }
            else
            {
                if (register.GetNoOfCustemersInQue() > 0)
                {
                    Debug.Log($"{controller.name} Somebody at que, going to stand behind them");
                    anim.SetFloat(ConstantStrings.Movement_Param, 1f);
                    noOffCustomerInQue = register.GetNoOfCustemersInQue();
                    agent.SetDestination(register.ViewLastCustomerInQue().transform.position + (Vector3.forward * quePositionOffset));
                }
                else
                {
                    customerInFrontOfMe = null;
                    Debug.Log("no body at que, heading straight to register");
                    agent.SetDestination(register.CustomerStandPoint.position);
                    anim.SetFloat(ConstantStrings.Movement_Param, 1f);
                }
            }
        }*/
    }

    public class LeaveStore_Customer : I_CustomerStates
    {
        private bool isStateOn = false;
        public bool StateON => isStateOn;

        private bool isLeavingStore = false;

        private CustomerController controller;
        private NavMeshAgent agent;
        private Animator anim;
        //private charac 

        Transform destination;

        public LeaveStore_Customer(NavMeshAgent agent, Animator anim, CustomerController controller)
        {
            this.agent = agent;
            this.anim = anim;
            this.controller = controller;
        }



        public void OpenState()
        {
            isStateOn = true;
            isLeavingStore = true;

            anim.SetFloat(ConstantStrings.Movement_Param, 1f);
            destination = GameManager.Instance.GetCustomerStartPoint(AIType.Customer);

            controller.ActivateCheckoutBags(true);
            controller.Kart.gameObject.SetActive(false);
            //Debug.Log($"{controller.name} leaving store");
            DOVirtual.Float(1f, 0f, 0.15f, controller.SetHandIkWeight);

            agent.SetDestination(destination.position);
        }

        public void UpdateState()
        {
            if (!isStateOn || !isLeavingStore || agent.pathPending) return;
            ExitStore();
        }
        public bool CloseState()
        {
            if (isLeavingStore)
            {
                return false;
            }
            else
            {
                controller.SetHandIkWeight(1f);
                return true;
            }
        }

        private void ExitStore()
        {
            if (agent.velocity.magnitude < 0.08f && agent.remainingDistance < agent.stoppingDistance)
            {
                controller.Kart.ActivateItem(false);
                controller.Kart.gameObject.SetActive(true);
                controller.ActivateCheckoutBags(false);
                anim.SetFloat(ConstantStrings.Movement_Param, 0f);
                Debug.Log($"{controller.name} left store");
                isLeavingStore = false;
                controller.SetState(CustomerStates.Idle);
            }
        }

    }
    #endregion


    //------------ STOCKER STATES -------------
    #region STOCKER STATES
    public class Idle_Stocker : I_StockerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;

        private StockerController controller;
        private NavMeshAgent agent;
        //private CharacterRigRef rigRef;
        private Stack<Package> stockerPackages;
        //private Transform[] waitPoints;
        private UpgradaleData upgradaleData;

        public Idle_Stocker(StockerController controller, NavMeshAgent agent, CharacterRigRef rigRef, Stack<Package> packages, UpgradaleData upgradaleData)
        {
            this.agent = agent;
            //this.rigRef = rigRef;
            this.controller = controller;
            stockerPackages = packages;
            this.upgradaleData = upgradaleData;
        }

        public void OpenState()
        {
            /*rigRef.Anim.SetFloat(ConstantStrings.Movement_Param, 0f);
            if (stockerPackages.Count > 0)
            {
                rigRef.Anim.SetBool(ConstantStrings.Lifting_Param, true);
                rigRef.CarryIkRig.weight = 1f;
            }*/
            agent.SetDestination(agent.transform.position);
            isStateOn = true;
            //agent.isStopped = true;
        }

        public bool CloseState()
        {
            isStateOn = false;
            return true;
            //throw new System.NotImplementedException();
        }


        public void UpdateState()
        {
            if (!isStateOn) return;
            OnIdle();
        }

        private void OnIdle()
        {
            if (GameManager.Instance.GetAIInteractableHandler().IsInteractableAvailable<Container>()
                && GameManager.Instance.GetAIInteractableHandler().IsInteractableAvailable<PackageOffloader_Anim>())
            {


                var offloader = GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<PackageOffloader_Anim>();
                Debug.Log($"Checking for another offloader {offloader.name}");

                if (stockerPackages.Count > 0)
                {

                    if (GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractableCount<PackageOffloader_Anim>() <= 1)
                    {
                        Debug.Log("Only 1 offloader available dropping packages I have");
                        var container = GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<Container>();
                        controller.SetState(StockerStates.ToShelf, container: container as IPlaceable);
                    }
                    else
                    {
                        if (stockerPackages.Count < upgradaleData.StockerStackCapactity.Value && (offloader as PackageOffloader_Anim).IsItemAvailable())
                        {
                            //offloader = GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<PackageOffloader_Anim>();
                            Debug.Log("Going to another offloader since mines not enough");
                            controller.SetState(StockerStates.ToOffloader, offloader: offloader as PackageOffloader_Anim);
                        }
                        else
                        {
                            Debug.Log($"Never mind stack not full but no other offloader available and {offloader.name} isn't available:" +
                                $" {(offloader as PackageOffloader_Anim).IsItemAvailable()} packages and {stockerPackages.Count} is not " +
                                $"less than {upgradaleData.StockerStackCapactity.Value}");
                            var container = GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<Container>();
                            controller.SetState(StockerStates.ToShelf, container: container as IPlaceable);
                        }
                    }

                }
                else
                {
                    //var offloader = GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<PackageOffloader_Anim>();
                    controller.SetState(StockerStates.ToOffloader, offloader: offloader as PackageOffloader_Anim);

                }


                //Debug.Log("Stocker going to Offloader");
            }
        }
    }

    public class GoToOflloader_Stocker : I_StockerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;

        private bool isGoingToOffloader = false;

        private StockerController controller;
        private NavMeshAgent agent;
       private CharacterRigRef characterRig;
        private Stack<Package> packages;

        private PackageOffloader_Anim offloader;



        public GoToOflloader_Stocker(NavMeshAgent agent, CharacterRigRef characterRig, Stack<Package> packages, StockerController controller)
        {
            this.agent = agent;
            this.characterRig = characterRig;
            this.controller = controller;
            this.packages = packages;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(PackageOffloader_Anim offloader)
        {
            /*if (packages.Count > 0)
            {
                SetAnimRigData(true, 1f);
            }
            else
            {
                SetAnimRigData(false, 0f);
            }*/
            //Debug.Log($"{characterRig.name} Going to offloader, carrying {packages.Count} packages");
            if (isGoingToOffloader) return;
            isStateOn = true;
            
            characterRig.Anim.SetFloat(ConstantStrings.Movement_Param, 1f);
            agent.SetDestination(offloader.transform.position);
            this.offloader = offloader;
        }

        public void UpdateState()
        {
            if (!isStateOn) return;
            GoToOffloader();
        }

        public bool CloseState()
        {
            //throw new System.NotImplementedException();
            //characterRig.Anim.SetFloat(ConstantStrings.Movement_Param, 1f);
            if (isGoingToOffloader)
            {
                return true;
            }
            else
            {
                isStateOn = false;
                return false;
            }
        }

        private void GoToOffloader()
        {
            //isStateOn = false;
            isGoingToOffloader = true;
            //Debug.Log("Agent go to destination");
            /*if (Vector3.Distance(agent.transform.position, container.transform.position) < agent.stoppingDistance)
            {
                controller.SetState(CustomerStates.Idle);
            }*/

            //Debug.Log("Agent Reached Destination!" + agent.velocity.magnitude);
            if (agent.velocity.magnitude < 0.08f && agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
            {
                //Debug.Log("Stocker Reached Destination!" + agent.remainingDistance);
               // characterRig.Anim.SetFloat(ConstantStrings.Movement_Param, 0f);
                controller.SetState(StockerStates.StackPackages, offloader);
                isGoingToOffloader = false;
            }

            /*if (offloader.IsPackageAvailable())
            {
                //var newContainer = GameManager.Instance.GetCustomerHandler().GetAvailableContainer();
                Debug.Log("Container empty");
                if (!GameManager.Instance.GetAIInteractableHandler().IsInteractableAvailable<PackageOffloader>())
                {
                    controller.SetState(StockerStates.Idle);
                }
                else
                {
                    //go to new container
                }
            }*/
        }

        /*private void SetAnimRigData(bool lift, float weight)
        {
            characterRig.Anim.SetBool(ConstantStrings.Lifting_Param, lift);
            //if (lift == false && characterRig.CarryIkRig.weight > 0.9f) return;
            DOVirtual.Float(characterRig.CarryIkRig.weight, weight, 0.25f, ((x) =>
            {
                characterRig.CarryIkRig.weight = x;
            }));
        }*/
    }

    public class StackPackages_Stocker : I_StockerStates
    {
        private bool stateOn;
        public bool StateON => stateOn;
        private bool isStacking = false;

       CharacterRigRef characterRig;
        PackageOffloader_Anim offloader;
        Stack<Package> packagesStack;
        UpgradaleData upgradaleData;
        StockerController controller;

        private float placementOffset = 0.312f;

        public StackPackages_Stocker(CharacterRigRef rig, Stack<Package> packages, UpgradaleData ud, StockerController controller)
        {
            characterRig = rig;
            packagesStack = packages;
            upgradaleData = ud;
            this.controller = controller;
        }

        public void OpenState()
        {
            /*stateOn = true;
            Debug.Log("Stocker Stacking State ON...");*/
            throw new System.NotImplementedException();
        }

        public void OpenState(PackageOffloader_Anim o)
        {
            stateOn = true;
            offloader = o;
            characterRig.Anim.SetFloat(ConstantStrings.Movement_Param, 0f);
            // SetAnimRigData(true, 1f);
        }

     /*   private void SetAnimRigData(bool lift, float weight)
        {
            characterRig.Anim.SetBool(ConstantStrings.Lifting_Param, lift);

            DOVirtual.Float(characterRig.CarryIkRig.weight, weight, 0.25f, ((x) =>
            {
                characterRig.CarryIkRig.weight = x;
            }));
        }*/

        public void UpdateState()
        {
            //throw new System.NotImplementedException();
            //Debug.Log(isStacking);
            /*if (packagesStack.Count < upgradaleData.StockerStackCapactity.Value && !offloader.IsItemAvailable())
            {
                controller.SetState(StockerStates.Idle);
                //return;
            }*/

            if ((!offloader.IsItemAvailable()) && packagesStack.Count > 0)
            {
                controller.SetState(StockerStates.Idle);
            }
            else if (packagesStack.Count <= 0 && GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractableCount<PackageOffloader_Anim>() > 1 
                && !offloader.IsItemAvailable())
            {
                controller.SetState(StockerStates.Idle);
            }

            if (!stateOn || isStacking || !offloader.IsItemAvailable()) return;
            Stack();
            //Debug.Log("Stacking...");
        }

        public bool CloseState()
        {
            stateOn = false;
            isStacking = false;
           /* if (packagesStack.Count <= 0)
            {
                SetAnimRigData(false, 0f);
            }*/
            return true;
            //Debug.Log("No longer Stacking");
            //throw new System.NotImplementedException();
        }

        private void Stack()
        {
            isStacking = true;

            if (packagesStack.Count < upgradaleData.StockerStackCapactity.Value)
            {
                var package = GameManager.Instance.PackagesPool.GetPackage() as Package;

                package.transform.position = (offloader.RetreivePackagePosition() as Package).transform.position;
                package.transform.parent = characterRig.PackagesStackHolder;
                package.SetOffloadState(OffloadableItemState.Offloaded);

                package.transform.DOLocalJump((packagesStack.Count > 0) ? new Vector3(0f, placementOffset + packagesStack.Peek().transform.localPosition.y, 0f)
                    : Vector3.zero, 1f, 1, .15f)
                    .OnComplete(() =>
                    {
                        package.transform.localRotation = Quaternion.identity;
                        offloader.ItemRetreived();
                        packagesStack.Push(package);
                        if (packagesStack.Count < upgradaleData.StockerStackCapactity.Value && offloader.IsItemAvailable())
                        {
                            controller.SetState(StockerStates.StackPackages, offloader);
                            //return;
                        }
                       /* else
                        {
                            controller.SetState(StockerStates.Idle);
                        }*/
                        isStacking = false;
                        Debug.Log("Returned to function");
                    });
            }
            else
            {
                Debug.Log("Stocker Max Capacity");
                if (GameManager.Instance.GetAIInteractableHandler().IsInteractableAvailable<Container>())
                {
                    controller.SetState(StockerStates.ToShelf, container: GameManager.Instance.GetAIInteractableHandler().GetAvailableInteractable<Container>() as IPlaceable);
                }
                else
                {
                    controller.SetState(StockerStates.Idle);
                }
            }

        }
    }

    public class GoToShelf_Stocker : I_StockerStates
    {
        private bool isStateOn;
        public bool StateON => isStateOn;

        private bool isGoingToShelf = false;

        private StockerController controller;
        private NavMeshAgent agent;
        private Animator anim;

        private Container container;



        public GoToShelf_Stocker(NavMeshAgent agent, Animator anim, StockerController controller)
        {
            this.agent = agent;
            this.anim = anim;
            this.controller = controller;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(Container container)
        {
            if (isGoingToShelf) return;
            isStateOn = true;
            isGoingToShelf = true;
            anim.SetFloat(ConstantStrings.Movement_Param, 1f);
            agent.SetDestination(container.transform.position);
            this.container = container;
        }

        public void UpdateState()
        {
            if (!isStateOn) return;
            GoToContainer();
        }

        public bool CloseState()
        {
            //throw new System.NotImplementedException();
            if (isGoingToShelf)
            {
                return false;
            }
            else
            {
                isStateOn = false;
                return true;
            }
        }

        private void GoToContainer()
        {
            //isStateOn = false;
           // isGoingToShelf = true;
            //Debug.Log("Agent go to destination");
            /*if (Vector3.Distance(agent.transform.position, container.transform.position) < agent.stoppingDistance)
            {
                controller.SetState(CustomerStates.Idle);
            }*/

            //Debug.Log("Agent Reached Destination!" + agent.velocity.magnitude);
            if (agent.velocity.magnitude < 0.08f && agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
            {
                //Debug.Log("Stocker Reached Destination!" + agent.remainingDistance);
                isGoingToShelf = false;
                controller.SetState(StockerStates.PlacePackages, container: container as IPlaceable);
            }

            /*if (container.IsEmpty())
            {
                //var newContainer = GameManager.Instance.GetCustomerHandler().GetAvailableContainer();
                Debug.Log("Container empty");
                if (!GameManager.Instance.GetAIInteractableHandler().IsInteractableAvailable<Container>())
                {
                    controller.SetState(CustomerStates.Idle);
                }
                else
                {
                    //go to new container
                }
            }*/
        }
    }

    public class PlacePackages_Stocker : I_StockerStates
    {
        private bool stateOn;
        public bool StateON { get => stateOn; }
        private bool isPlacing = false;

        IPlaceable container;
        Stack<Package> stockerPackages;
        //CharacterRigRef characterRig;
        StockerController controller;


        public PlacePackages_Stocker(Stack<Package> packages, CharacterRigRef characterRig, StockerController controller)
        {
            stockerPackages = packages;
           // this.characterRig = characterRig;
            this.controller = controller;
        }

        public void OpenState()
        {
            throw new System.NotImplementedException();
        }

        public void OpenState(IPlaceable container)
        {
            stateOn = true;
            this.container = container;
            //characterRig.Anim.SetFloat(ConstantStrings.Movement_Param, 0f);
            //Debug.Log("In placement state");
        }

        public void UpdateState()
        {
            //throw new System.NotImplementedException();
/*
            if (!stateOn) Debug.Log("Placing states is off");                

            if (isPlacing) Debug.Log("is placing already");*/
            
            if (container.IsFull())
            {
                controller.SetState(StockerStates.Idle);
                Debug.Log("shelf is full");
                return;
            }

            //if (stockerPackages.Count <= 0) Debug.Log("stocker has no packages in stack");

            
            if (!stateOn || isPlacing || stockerPackages.Count <= 0)
            {
                //Debug.Log("Cant place for some reason");
                return;
            }
            Place();
        }

        public bool CloseState()
        {
            if (isPlacing)
            {
                return false;
            }
            else
            {
                stateOn = false;
                return true;
            }
            /*if (playerPackages.Count <= 0)
            {
                SetAnimRigData(false, 0f);
            }*/
        }

        private void Place()
        {
            if (container.IsFull())
            {
                isPlacing = false;
                return;
            }
            else
            {
                isPlacing = true;
            }
            //Debug.Log("Placing packages");
            var itemToPlace = container.GetItemToPlace();

            if(itemToPlace == null)
            {
                isPlacing = false;
                return;
            }    

            var package = stockerPackages.Pop();

            package.transform.parent = GameManager.Instance.PackagesPool.transform;


            package.transform.DOJump(itemToPlace.transform.position, 1f, 1, .15f)
                    .OnComplete(() =>
                    {
                        container.PlaceItem(itemToPlace);
                        // package.gameObject.SetActive(false);
                        package.SetOffloadState(OffloadableItemState.Idle);

                        if(stockerPackages.Count <= 0)
                        {
                            //Debug.Log("All packages placed!");
                            isPlacing = false;
                            controller.SetState(StockerStates.Idle);
                            //SetAnimRigData(false, 0f);
                        }
                        else
                        {
                            Place();
                        }
                        //isPlacing = false;
                    });
        }

       /* private void SetAnimRigData(bool lift, float weight)
        {
            characterRig.Anim.SetBool(ConstantStrings.Lifting_Param, lift);

            DOVirtual.Float(characterRig.CarryIkRig.weight, weight, 0.25f, ((x) =>
            {
                characterRig.CarryIkRig.weight = x;
            }));
        }*/
    }

    #endregion


}