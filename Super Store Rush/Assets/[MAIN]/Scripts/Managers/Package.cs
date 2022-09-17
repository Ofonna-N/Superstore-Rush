using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class Package : MonoBehaviour, IOffloadable
    {
        [SerializeField]
        private OffloadableItemState state;
        public OffloadableItemState State => state;

        public void Init()
        {
            SetOffloadState(OffloadableItemState.Idle);
        }

        public void SetOffloadState(OffloadableItemState state)
        {
            //Debug.Log($"Setting {name} state to {state}");
            switch (state)
            {
                case OffloadableItemState.Idle:
                    this.state = OffloadableItemState.Idle;
                    gameObject.SetActive(false);
                    break;
                case OffloadableItemState.Offloading:
                    this.state = OffloadableItemState.Offloading;
                    break;
                case OffloadableItemState.Offloaded:
                    this.state = OffloadableItemState.Offloaded;
                    gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
