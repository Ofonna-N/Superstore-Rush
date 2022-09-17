using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperStoreRush
{
    public class HRDept : MonoBehaviour, ITutorialListeners
    {
        [SerializeField]
        private string hrViewCategory, hrViewName;

        [SerializeField]
        private GameObject indicator;

        public void OnTutorialWatched()
        {
            indicator.SetActive(true);
        }

        public void PlayerEnquires()
        {
            Doozy.Runtime.UIManager.Containers.UIView.Show(hrViewCategory, hrViewName);
        }
    }
}
