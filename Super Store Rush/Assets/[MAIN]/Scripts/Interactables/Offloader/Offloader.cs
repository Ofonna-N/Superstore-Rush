using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public abstract class Offloader : Interactable
    {

        [ShowInInspector, BoxGroup("Offloader Data", showLabel: false), ReadOnly]
        protected Stack<IOffloadable> packagesStack;


        [ShowInInspector, BoxGroup("Offloader Data", showLabel: false), ReadOnly]
        protected IOffloadable[] allItems;

        [ShowInInspector, BoxGroup("Offloader Data", showLabel: false), ReadOnly]
        protected IOffloadable itemToOffload;

        [ShowInInspector, BoxGroup("Offloader Data", showLabel: false), ReadOnly]
        protected int numOfItemsOffloaded;

        [ShowInInspector, BoxGroup("Offloader Data", showLabel: false), ReadOnly]
        protected bool offloading;

        protected bool packageOffloadInit;
        
        
        public override void Init(Level level)
        {
            allItems = GetComponentsInChildren<IOffloadable>(true);
            packagesStack = new Stack<IOffloadable>();

            for (int i = 0; i < allItems.Length; i++)
            {
                var item = allItems[i];

                item.Init();

            }
            base.Init(level);
            //BeginOffload();
        }

        public override void Unlock(bool init)
        {
            base.Unlock(init);
            
            BeginOffload();
        }

        protected abstract void BeginOffload();


        //initiate offload
        protected abstract void Offload();

        public IOffloadable RetreivePackagePosition()
        {
            //Debug.Log("RETRIEVING PACKAGE!!!!");
            IOffloadable retVal = null;

            if (packagesStack.Count > 0)
            {
                var p = packagesStack.Pop();
                p.SetOffloadState(OffloadableItemState.Idle);
                //retVal = (p as Package).transform.position;
                retVal = p;
            }

            return retVal;
        }
        //character retrieving the package position to begin animation at start of package position


        /// <summary>
        /// once stacking animation is complete player then says package has been retrieved!
        /// </summary>
        public void ItemRetreived()
        {
            numOfItemsOffloaded -= 1;
            BeginOffload();
        }

        public int OffloadedItemsCount()
        {
            return packagesStack.Count;
        }

        public bool IsItemAvailable()
        {
            
            return packagesStack.Count > 0;
        }
    }
}
