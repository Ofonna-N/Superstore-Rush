using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperStoreRush
{
    public class InteractableProgress : MonoBehaviour
    {
        [SerializeField]
        private GameObject Indicator;

        [SerializeField]
        private Image slider;

        public void Acitvate(bool value)
        {
            Indicator.SetActive(value);
        }

        public void SetProgress(float value)
        {
            slider.fillAmount = value;
        }
    }
}
