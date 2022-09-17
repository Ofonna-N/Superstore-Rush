using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{

    public class RegisterCashier : Register
    {
        [BoxGroup("Cash Register", showLabel: false), SerializeField]
        private BoxCollider registerSensor;

        [BoxGroup("Cash Register", showLabel: false), SerializeField]
        protected GameObject targetZoneAnimation;

        private CashierController cashier;
        
        public bool HashCashier => cashier != null;

        public override void AttendToMe()
        {
            base.AttendToMe();
            if (HashCashier)
            {
                WorkRegister();
            }
            else
            {
                navigationIndicator.SetActive(true);
                targetZoneAnimation.SetActive(true);
                GameManager.Instance.ActivateRegisterInstructionText(true);
            }
        }

        protected override void OnWorkRegisterstart()
        {
            cashier?.WorkRegister(true);
            base.OnWorkRegisterstart();
        }

        protected override void OnWorkRegisterComplete()
        {
            cashier?.WorkRegister(false);
            if (!HashCashier)
            {
                targetZoneAnimation.SetActive(false);
                GameManager.Instance.ActivateRegisterInstructionText(false);
            }
            base.OnWorkRegisterComplete();
        }


        public void SetCashier(GameObject cashierPrefab)
        {
            cashier = Instantiate(cashierPrefab, registerSensor.transform).GetComponent<CashierController>();
            registerSensor.enabled = false;
            //this.cashier = cashier;
            cashier.transform.position = registerSensor.transform.position;
            cashier.transform.rotation = registerSensor.transform.rotation;
            targetZoneAnimation.SetActive(false);
            GameManager.Instance.ActivateRegisterInstructionText(false);
            if (GetNoOfCustemersInQue() > 0)
            {
                WorkRegister();
            }
        }
    }
}
