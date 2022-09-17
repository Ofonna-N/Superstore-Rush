using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class PaymentSensor : TriggerSensor
    {

        private IUnlockable unlockable;

        private void Start()
        {
            unlockable = GetComponentInParent<IUnlockable>();
            //Debug.Log(unlockable.ToString());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Paying, unlockable:unlockable);
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Idle);
            }
        }
    }
}
