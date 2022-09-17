using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;

namespace SuperStoreRush
{
    public abstract class Register : Interactable
    {
        [SerializeField]
        private UpgradaleData upgradaleData;

        [SerializeField]
        private Transform customerStandPoint;
        public Transform CustomerStandPoint => customerStandPoint;

        [SerializeField]
        private Transform moneySpawnPoint;
        public Transform MoneySpawnPoint => moneySpawnPoint;

        [SerializeField]
        protected GameObject navigationIndicator;

        [SerializeField]
        private InteractableProgress registerUIProgress;

        [SerializeField]
        private MMF_Player onPurchasedFb;
        
        [SerializeField]
        private MMF_Player onStartRegisterFb;

        [ShowInInspector]
        protected List<CustomerController> customerQue;

        private bool isWorkingRegister = false;

        private bool customerWaiting = false;

        private System.Action<CustomerController> OnCustomerAddedToQue;

        private System.Action<CustomerController> OnCustomerRemovedFromQue;

        public override void Init(Level level)
        {
            customerQue = new List<CustomerController>();

            base.Init(level);
        }


        public void AddCustomerToQue(CustomerController customer)
        {
            if (customerQue.Contains(customer))
            {
                Debug.Log("Adding Duplicate customer to que");
            }
            customerQue.Add(customer);
            if (OnCustomerAddedToQue != null) OnCustomerAddedToQue(customer);
        }

        public void GetRegisterNotifications(System.Action<CustomerController> onAdded, System.Action<CustomerController> onRemoved, bool notify)
        {
            //if(oncus)
            if (notify)
            {
                OnCustomerAddedToQue += onAdded;
                OnCustomerRemovedFromQue += onRemoved;
            }
            else
            {
                OnCustomerAddedToQue -= onAdded;
                OnCustomerRemovedFromQue -= onRemoved;

            }
        }

        public override void Unlock(bool init)
        {
            base.Unlock(init);
            level.AIInteractableHandler.AddToAvailableInteractables(this);
            GameManager.Instance.SetNextTutorialStep();
        }

        public int GetNoOfCustemersInQue()
        {
            return customerQue.Count;
        }

        public int GetCustomerPositionInQue(CustomerController customer)
        {
            return customerQue.IndexOf(customer);
        }

        public CustomerController ViewLastCustomerInQue()
        {
            if (customerQue.Count > 0)
            {
                return customerQue[customerQue.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public bool IsCustomerStillInCue(CustomerController customer)
        {
            return customerQue.Contains(customer);
        }

        //Next customer in que has advance in position
        public virtual void AttendToMe()
        {
            Debug.Log("Customer At Register, Attend to me!");
            customerWaiting = true;
            
        }

        public void WorkRegister()
        {
            Debug.Log("Trying to work register");
            if (isWorkingRegister || !customerWaiting) return;

            isWorkingRegister = true;

            DOVirtual.Float(0f, 1f, upgradaleData.RegisterWorkDuratoin.Value, (x) =>
            {
                registerUIProgress.SetProgress(x);
            })
                .OnStart(()=>
                {
                    OnWorkRegisterstart();
                })
                .OnComplete(()=>
            {
                OnWorkRegisterComplete();
            });
        }

        protected virtual void OnWorkRegisterstart()
        {
            registerUIProgress.Acitvate(true);
            onStartRegisterFb.PlayFeedbacks();
        }

        protected virtual void OnWorkRegisterComplete()
        {
            Debug.Log("Register Worked!");
            var customer = customerQue[0];
            customerQue.Remove(customer);
            isWorkingRegister = false;
            customerWaiting = false;
            GameManager.Instance.GetMoneyHandler().SpawnMoneyAtRegister(this);
            Debug.Log($"Just removed {customer.name} from que");
            OnCustomerRemovedFromQue(customer);
            customer.SetState(CustomerStates.LeaveStore, registerWorked: true);
            registerUIProgress.Acitvate(false);
            onPurchasedFb.PlayFeedbacks();
            navigationIndicator.SetActive(false);
            
            GameManager.Instance.PlayHaptic();
            GameManager.Instance.OnAdEvent(AdsEvent.OnCustomerCheckedOut);
            //GameManager.Instance.AddToMoney(10);
            //take out of customer stack
            // customerStack.
        }

    }
}
