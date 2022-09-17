using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class Section : Interactable
    {
        [SerializeField, BoxGroup("Interactable Data", showLabel: false)]
        //private Transform interactablesHolder;

        public override void Unlock(bool init)
        {
            Debug.Log("Section Unlocked");
            base.Unlock(init);

            var interactables = GetComponentsInChildren<Interactable>();

            for (int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i] == this) continue;
                if (interactables[i].gameObject.activeInHierarchy)
                {
                    interactables[i].Init(level);
                }
                
            }

            level.BakeNavmesh();
        }
    }
}
