using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class RegisterSelfCheckout : Register
    {
        public override void AttendToMe()
        {
            Debug.Log("Self Check out called!");
            base.AttendToMe();
            WorkRegister();
            /*navigationIndicator.SetActive(true);
            GameManager.Instance.ActivateRegisterInstructionText(true);*/
        }
    }
}
