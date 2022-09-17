using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class ShelfProduct : MonoBehaviour
    {
        [SerializeField]
        private Transform content;
        public Transform Content => content;
    }
}
