using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class CustomerKart : MonoBehaviour
    {
        [SerializeField]
        private GameObject item;

        public Vector3 KartPosition => item.transform.position;

        public void ActivateItem(bool value)
        {
            item.SetActive(value);
        }

    }
}
