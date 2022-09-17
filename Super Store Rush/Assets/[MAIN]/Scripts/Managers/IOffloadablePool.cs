using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class IOffloadablePool : MonoBehaviour
    {
        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private int packagePoolCount = 20;

        [SerializeField, BoxGroup("Package Data", centerLabel: true)]
        private GameObject prefab;

        //[SerializeField, BoxGroup("Package Data", centerLabel: true)]
        //private Transform itemsHolder;
        //public Transform PackagesHolder => itemsHolder;

        [ShowInInspector, ReadOnly, BoxGroup("Package Data", centerLabel: true)]
        private List<IOffloadable> packages;
        //public Package[] Packages => packages;


        public void Init()
        {
            packages = new List<IOffloadable>();

            for (int i = 0; i < packagePoolCount; i++)
            {
                packages.Add(CreateNewPackage());
            }
        }

        private IOffloadable CreateNewPackage()
        {
            var package = Instantiate(prefab, transform, false).GetComponent<IOffloadable>();

            package.SetOffloadState(OffloadableItemState.Idle);

            return package;
        }

        public IOffloadable GetPackage()
        {
            for (int i = 0; i < packages.Count; i++)
            {
                if (packages[i].State == OffloadableItemState.Idle)
                {
                    //packages[i].SetOffloadState(OffloadableItemState.Offloading);
                    return packages[i];
                }
            }

            return CreateNewPackage();
        }
    }
}
