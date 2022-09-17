using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public abstract class Interactable : MonoBehaviour, IUnlockable
    {
        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        protected InteractableData interactableData;

        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        protected InteractableDataUnlockUI interactableDataUnlockUI;

        //[SerializeField]
        protected Level level;

        public virtual void Init(Level level)
        {
            //Debug.Log(name);
            this.level = level;
            interactableDataUnlockUI.PriceText.text = $"{SmallyTools.CurrencyController.CurrencyFormat(interactableData.Price)}\n<sprite=0>";
            interactableDataUnlockUI.UIcanvas.worldCamera = GameManager.Instance.Main_Camera;


            /*if (interactableData.IsUnlocked)
            {
                Unlock(true);
            }*/
        }

        public virtual void Unlock(bool init)
        {
            
            if (init)
            {
                interactableDataUnlockUI.UnlockFb.Initialization();
                //var feedback = unlockFb.GetFeedbackOfType<MMF_Feedback>("MMSoundManager Sound");
                var feedback = interactableDataUnlockUI.UnlockFb.FeedbacksList.Find((x) => x.Label == "MMSoundManager Sound");
                //Debug.Log(feedback == null);
                feedback.Active = false;
                interactableDataUnlockUI.UnlockFb.PlayFeedbacks();
                feedback.Active = true;
                //GameManager.Instance.PlayHaptic();
            }
            else
            {
                interactableDataUnlockUI.UnlockFb.Initialization();
                interactableDataUnlockUI.UnlockFb.PlayFeedbacks();
                GameManager.Instance.OnAdEvent(AdsEvent.OnInteractableUnlocked);
                GameManager.Instance.PlayHaptic();
            }
        }

        public virtual bool IsUnlocked()
        {
            return interactableData.IsUnlocked;
        }

        public virtual void AddPrice(float value)
        {
            interactableData.Price += value;
            interactableDataUnlockUI.PriceText.text = $"{SmallyTools.CurrencyController.CurrencyFormat(interactableData.Price)}\n<sprite=0> ";
            if (interactableData.Price <= 0)
            {
                Unlock(false);
            }
        }

        public virtual float GetPrice()
        {
            return interactableData.Price;
        }

        public void UpdateUnlockDelayIndicator(float fillValue)
        {
            interactableDataUnlockUI.DelaySlider.fillAmount = fillValue;
        }
    }
}
