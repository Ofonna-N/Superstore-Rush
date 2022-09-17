using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class TutorialSensor : TriggerSensor
    {
        private TutorialManager tutorialManager;

        private bool hasEntered;

        private void Start()
        {
            tutorialManager = GetComponentInParent<TutorialManager>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (hasEntered) return;
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                hasEntered = true;
                tutorialManager.NextStep();
            }
            //throw new System.NotImplementedException();
        }

        protected override void OnTriggerExit(Collider other)
        {
            //throw new System.NotImplementedException();
        }

        
    }
}
