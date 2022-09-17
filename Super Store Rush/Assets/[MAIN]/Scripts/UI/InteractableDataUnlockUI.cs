using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class InteractableDataUnlockUI : MonoBehaviour
    {

        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        private TextMeshProUGUI priceText;
        public TextMeshProUGUI PriceText => priceText;


        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        private UnityEngine.UI.Image delaySlider;
        public UnityEngine.UI.Image DelaySlider => delaySlider;


        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        private Canvas uiCanvas;
        public Canvas UIcanvas => uiCanvas;


        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        private MMF_Player unlockFb;
        public MMF_Player UnlockFb => unlockFb;
    }
}
