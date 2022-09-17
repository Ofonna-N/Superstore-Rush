using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class Money_Reward : Money, IOffloadable
    {
        [SerializeField]
        private OffloadableItemState state;
        public OffloadableItemState State => state;

        [ShowInInspector, ReadOnly]
        private Transform parent;

        [ShowInInspector, ReadOnly]
        private float price = 5f;

        [ShowInInspector, ReadOnly]
        private Vector3 startPostion;

        /*public float Price 
        {
            set 
            { 

                price = value; 
            }
        }*/

        public void Init()
        {
            startPostion = transform.localPosition;
            parent = transform.parent;
            SetOffloadState(OffloadableItemState.Idle);
        }
        public void Init(float price, PackageOffloader_Spawn moneyOffloader)
        {
            //startPostion = transform.localPosition;
            //this.price = price;
            //this.parent = parent;
            //ActivateTrigger(false);
        }

      /*  protected override void SendMoneyCollectedEvent()
        {
            GameManager.Instance.GetMoneyHandler().OnMoneyCollected(transform.position, price);
        }*/

        /*public override void ActivateTrigger(bool value)
        {
            base.ActivateTrigger(value);

            if (!value)
            {
                transform.parent = parent;
                transform.localPosition = startPostion;
            }
        }*/

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
                    transform.parent = parent;
                    transform.localPosition = startPostion;
                    this.state = OffloadableItemState.Offloaded;
                    gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        protected override void SendMoneyCollectedEvent(Vector3 playerPos)
        {
            SetOffloadState(OffloadableItemState.Idle);
            GameManager.Instance.GetMoneyHandler().OnMoneyCollected(playerPos, price);
        }

    }
}
