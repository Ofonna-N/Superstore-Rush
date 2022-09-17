using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using MoreMountains.Feedbacks;

namespace SuperStoreRush
{
    public abstract class Container : Interactable, IPlaceable
    {
        [SerializeField]
        protected Transform contentHolder;

        [SerializeField]
        private ParticleSystem packagesIndicator;

        [SerializeField]
        private MMF_Player placeItemFb;

        [ShowInInspector]
        protected ShelfProduct[] products;

        [ShowInInspector]
        protected Stack<ShelfProduct> productsStack;

        private bool contentAnimating = false;

        public virtual void PlaceItem(GameObject item)
        {
            if (contentAnimating) return;
            contentAnimating = true;
            placeItemFb.PlayFeedbacks();
            contentHolder.DOScaleY(0.9f, 0.15f)
                .OnComplete(()=>
                {
                    contentHolder.DOScaleY(1f, 0.15f);
                    contentAnimating = false;
                });
        }

        public virtual bool IsFull()
        {
            var isFull = productsStack.Count >= products.Length;

            if (isFull)
            {
                if (packagesIndicator.isPlaying)
                {
                    packagesIndicator.Stop();
                }
            }

            return isFull;
        }

        public virtual bool IsEmpty()
        {
            var isEmpty = productsStack.Count <= 0;

            if (isEmpty)
            {
                if (!packagesIndicator.isPlaying)
                {
                    packagesIndicator.Play();
                }
            }

            return isEmpty;
        }

        public abstract GameObject GetItemToPlace();

        public abstract ShelfProduct GetItemToGrab();

        public abstract void ItemRemovedFromShelf(ShelfProduct item);
    }
}
