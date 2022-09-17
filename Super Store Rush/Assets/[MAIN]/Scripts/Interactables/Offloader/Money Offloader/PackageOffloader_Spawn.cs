using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


namespace SuperStoreRush
{
    public class PackageOffloader_Spawn: Offloader
    {
        public bool PlayerCollecting { get; set; }  
        protected override void BeginOffload()
        {
            if (offloading || PlayerCollecting) return;
            offloading = true;
            /*if (packageToOffload == null)
            {
                Debug.Log("Check Recurssion Code!");
                return;
            }*/
            DOVirtual.Float(0f, 1f, 3f,
                (x) =>
                {
                    /*//Can animate Offloader or do something before upload
                    refPackageToAnim.position = Vector3.Lerp(packageStartPoint.transform.position,
                        new Vector3((itemToOffload as Package).transform.position.x,
                        refPackageToAnim.transform.position.y * animPackageCurve_y.Evaluate(x), (itemToOffload as Package).transform.position.z), x);
                    //refPackageToAnim.position = refPackageToAnim.position + (Vector3.up * animPackageCurve_y.Evaluate(x));

                    refPackageToAnim.rotation = Quaternion.Slerp(refPackageToAnim.rotation, (itemToOffload as Package).transform.rotation, x);*/
                })
                /*.SetDelay(((System.Func<float>)(() =>
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
                }))())*/
                .OnStart(() =>
                {
                    //Debug.Log("Offloading...");

                    //refPackageToAnim.gameObject.SetActive(true);
                    for (int i = 0; i < allItems.Length; i++)
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
                    //refPackageToAnim.gameObject.SetActive(false);
                    offloading = false;
                    Offload();
                });
        }

        protected override void Offload()
        {
            itemToOffload.SetOffloadState(OffloadableItemState.Offloaded);
            packagesStack.Push(itemToOffload);
            //offloadedCount += 1;
            numOfItemsOffloaded += 1;

           // Debug.Log($"{(itemToOffload as Money_Reward).name} is the ({numOfItemsOffloaded}) to be offloaded");
            //packageToOffload = null;
            if (packagesStack.Count < allItems.Length)
            {
                BeginOffload();
            }
        }

        // Start is called before the first frame update




    }
}
