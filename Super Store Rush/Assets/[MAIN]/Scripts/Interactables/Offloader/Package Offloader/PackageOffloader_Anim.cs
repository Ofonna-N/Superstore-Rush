using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class PackageOffloader_Anim : Offloader
    {
        [SerializeField, BoxGroup("Package Offloader Data", showLabel: false)]
        protected UpgradaleData upgradaleData;

        [SerializeField, BoxGroup("Package Offloader Data", showLabel: false)]
        protected Transform packageStartPoint;

        [SerializeField, BoxGroup("Package Offloader Data", showLabel: false)]
        protected Transform refPackageToAnim;

        [SerializeField, BoxGroup("Offloader Data", showLabel: false)]
        protected AnimationCurve animPackageCurve_y;
        //[SerializeField, BoxGroup("Package Offloader Data", showLabel: false)]
        // protected AnimationCurve animPackageCurve_z;

        /*

        //[ReadOnly, ShowInInspector, BoxGroup("Package Offloader Data", showLabel: false)]
        //protected int offloadedCount = 0;

        [ShowInInspector, BoxGroup("Package Offloader Data", showLabel: false), ReadOnly]
        protected Package[] allPackages;

        protected Package packageToOffload;

        protected int numOfPackagesOffloaded;

        protected bool packageOffloadInit; //indicating game has started and we've offloaded packages for the first time
        //public bool PackagesAvailable => numOfPackagesOffloaded > 0;

        protected bool offloading;*/



        /* public override void Init(Level level)
         {
             //allPackages = GetComponentsInChildren<Package>(true);
             //allPackages = new GameObject[p.Length];
             base.Init(level);

             *//*for (int i = 0; i < allPackages.Length; i++)
             {
                 var item = allPackages[i];

                 item.SetState(PackageOffloadState.Idle);

             }
             base.Init(level);*//*
             //BeginOffload();
         }*/

        public override void Unlock(bool init)
        {
            level.AIInteractableHandler.AddToAvailableInteractables(this);
            base.Unlock(init);
            //BeginOffload();
        }

        protected override void BeginOffload()
        {
            if (offloading) return;
            offloading = true;
            /*if (packageToOffload == null)
            {
                Debug.Log("Check Recurssion Code!");
                return;
            }*/
            DOVirtual.Float(0f, 1f, 0.15f,
                (x) =>
                {
                    //Can animate Offloader or do something before upload
                    refPackageToAnim.position = Vector3.Lerp(packageStartPoint.transform.position,
                        new Vector3((itemToOffload as Package).transform.position.x,
                        refPackageToAnim.transform.position.y * animPackageCurve_y.Evaluate(x), (itemToOffload as Package).transform.position.z), x);
                    //refPackageToAnim.position = refPackageToAnim.position + (Vector3.up * animPackageCurve_y.Evaluate(x));

                    refPackageToAnim.rotation = Quaternion.Slerp(refPackageToAnim.rotation, (itemToOffload as Package).transform.rotation, x);
                })
                .SetDelay(((System.Func<float>)(() =>
                {
                    if (!packageOffloadInit)
                    {
                        packageOffloadInit = true;
                        return 0f;
                    }
                    else
                    {
                        return upgradaleData.OffloadTime.Value;
                    }
                }))())
                .OnStart(() =>
                {
                    //Debug.Log("Offloading...");
                    
                    refPackageToAnim.gameObject.SetActive(true);
                    for (int i = 0; i < upgradaleData.OffloadedPackageCount.Value; i++)
                    {
                        if (/*!allPackages[i].gameObject.activeInHierarchy && */allItems[i].State == OffloadableItemState.Idle)
                        {
                            itemToOffload = allItems[i];
                            itemToOffload.SetOffloadState(OffloadableItemState.Offloading);
                            break;
                        }
                    }
                })
                .OnComplete(() =>
                {
                    refPackageToAnim.gameObject.SetActive(false);
                    offloading = false;
                    Offload();
                });
        }


        //initiate offload
        protected override void Offload()
        {
            //Debug.Log("Offloaded!");
            //packageToOffload.gameObject.SetActive(true);
            itemToOffload.SetOffloadState(OffloadableItemState.Offloaded);
            packagesStack.Push(itemToOffload);
            //offloadedCount += 1;
            numOfItemsOffloaded += 1;

            //Debug.Log($"{(itemToOffload as Package).name} is the ({numOfItemsOffloaded}) to be offloaded");
            //packageToOffload = null;
            if (numOfItemsOffloaded < upgradaleData.OffloadedPackageCount.Value)
            {
                BeginOffload();
            }
        }
        

        //character retrieving the package position to begin animation at start of package position
        

        /// <summary>
        /// once stacking animation is complete player then says package has been retrieved!
        /// </summary>
        /*public void PackageRetreived()
        {
            numOfPackagesOffloaded -= 1;
            BeginOffload();
        }*/
    }
}
