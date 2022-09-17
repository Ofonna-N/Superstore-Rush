using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class OffloaderSensor : TriggerSensor
    {

        [SerializeField]
        private Offloader offloader;

        [SerializeField]
        private OffloaderType offloaderType;

        private enum OffloaderType { anim, spawn }

        private void Start()
        {
            offloader = GetComponentInParent<Offloader>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {

                switch (offloaderType)
                {
                    case OffloaderType.anim:
                        other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Stacking, offloader);
                        break;
                    case OffloaderType.spawn:
                        other.GetComponent<InteractionStatesController>().SetState(PlayerStates.CollectingReward, offloader);
                        break;
                    default:
                        break;
                }

            }

            /*if (other.CompareTag(ConstantStrings.StockerTag))
            {
                other.GetComponent<StockerController>().SetState(StockerStates.StackPackages, packageOffloader);
            }*/
        }  

        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Idle);
                //Debug.Log("Player no longer stacking");
            }
        }
    }
}
