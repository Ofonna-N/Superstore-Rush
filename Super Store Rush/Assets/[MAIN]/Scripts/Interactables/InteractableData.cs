using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using SmallyTools;

namespace SuperStoreRush
{
    [CreateAssetMenu(menuName = "SmallyGames/Data/Interactable Unlock Data", fileName = "Interactable Unlock Data")]
    public class InteractableData : ScriptableObject
    {
        [SerializeField, InlineButton("Reset")]
        private float price = 500;

        public float Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
                if (price <= 0)
                {
                    price = 0;
                    IsUnlocked = true;
                }
            }
        }

        [SerializeField]
        private bool isUnlocked = false;

        public bool IsUnlocked
        {
            get
            {
                if (SaveGameManager.Instance.KeyExists(name))
                {
                    return isUnlocked = SaveGameManager.Instance.GetBool(name);
                }
                else
                {
                    return isUnlocked;
                }
                //return isUnlocked;
            }
            set
            {
                SaveGameManager.Instance?.SaveValue(name, isUnlocked = value);
            }
        }


#if UNITY_EDITOR
        [BoxGroup("Editor"), SerializeField]
        private bool showEditorValues = false;

        [BoxGroup("Editor", showLabel: false), SerializeField, ShowIf("showEditorValues")]
        private int resetValue = 500;

        private void Reset()
        {
            Price = resetValue;
            isUnlocked = false;
        }
#endif
    }
}
