using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class PlacementSensor : TriggerSensor
    {
        IPlaceable container;

        private void Start()
        {
            container = GetComponentInParent<IPlaceable>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Placing, container: this.container);
            }

            /*if (other.CompareTag(ConstantStrings.StockerTag))
            {
                other.GetComponent<StockerController>().SetState(StockerStates.PlacePackages, container: this.container);
            }*/
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
