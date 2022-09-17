using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuperStoreRush
{
    public abstract class Money : MonoBehaviour
    {


        [SerializeField]
        protected float animDuration = .35f;

        [SerializeField]
        protected Ease animEase = Ease.OutBack;

        [SerializeField]
        protected float heightOffset = 0.35f;

        [SerializeField]
        protected SphereCollider moneyCollider;

        [SerializeField]
        protected bool animate = true;


        public void PlayerCollect(Transform playerT, System.Action OnComplete = null)
        {
            var parent = transform.parent;
            if (animate)
            {

                transform.parent = playerT;

                transform.DOLocalMove(/*other.transform.position + */(Vector3.up * heightOffset), animDuration)
                    .OnComplete(() =>
                    {
                        transform.parent = parent;
                        GameManager.Instance.PlayHaptic();
                        GameManager.Instance.OnAdEvent(AdsEvent.OnMoneyCollected);
                        SendMoneyCollectedEvent(playerT.position);
                        if(OnComplete != null)OnComplete();
                        

                    }).SetEase(animEase);
            }
            else
            {
                transform.parent = parent;
                GameManager.Instance.PlayHaptic();
                GameManager.Instance.OnAdEvent(AdsEvent.OnMoneyCollected);
                SendMoneyCollectedEvent(playerT.position);
                if (OnComplete != null) OnComplete();
                
            }
        }

        protected abstract void SendMoneyCollectedEvent(Vector3 playerPos);

        public virtual void ActivateTrigger(bool value)
        {
            if (this is IOffloadable)
            {
                Debug.Log("Offloadable money, not affected by Function");
            }
            else
            {
                moneyCollider.enabled = value;
            }

            /*if (value)
            {
                spinAnim.DOPlay();
            }*/
        }
    }
}
