using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class RegisterSensor : TriggerSensor
    {
        private Register regiser;

        private void Start()
        {
            regiser = GetComponentInParent<Register>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                Debug.Log("Work Register Trigger");
                other.GetComponent<InteractionStatesController>().SetState(PlayerStates.WorkingRegister, register:regiser);
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

