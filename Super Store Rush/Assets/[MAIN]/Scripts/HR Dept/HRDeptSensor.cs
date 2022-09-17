using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class HRDeptSensor : TriggerSensor
    {
        //[SerializeField]
        private HRDept hrDept;

        private void Start()
        {
            hrDept = GetComponentInParent<HRDept>();
        }


        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                hrDept.PlayerEnquires();
                GameManager.Instance.GetLevel().ActivateHRCamera(true);
                //GameManager.Instance.GetAdvancedWalkerController().InHROffice = true;
                //other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Stacking, packageOffloader);
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                GameManager.Instance.GetLevel().ActivateHRCamera(false);
                //GameManager.Instance.GetAdvancedWalkerController().InHROffice = false;
               // other.GetComponent<InteractionStatesController>().SetState(PlayerStates.Idle);
                //Debug.Log("Player no longer stacking");
            }
        }
    }
}
