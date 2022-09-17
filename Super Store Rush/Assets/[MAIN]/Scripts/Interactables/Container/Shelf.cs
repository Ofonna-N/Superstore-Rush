using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using MoreMountains.Feedbacks;

namespace SuperStoreRush
{
    public class Shelf : Container
    {
      

        public override void Init(Level level)
        {
            base.Init(level);
            products = GetComponentsInChildren<ShelfProduct>(true);
            productsStack = new Stack<ShelfProduct>();

            for (int i = 0; i < products.Length; i++)
            {
                products[i].gameObject.SetActive(false);
            }

        }


        public override GameObject GetItemToPlace()
        {
            for (int i = 0; i < products.Length; i++)
            {
                if (!products[i].gameObject.activeInHierarchy && !productsStack.Contains(products[i]))
                {
                    var product = products[i];

                    //product.gameObject.SetActive(true);
                    productsStack.Push(product);
                    return product.gameObject;
                }
            }

            return null;
        }

        public override void Unlock(bool init)
        {
            base.Unlock(init);
            
            level.AIInteractableHandler.AddToAvailableInteractables(this);
        }

        public override void PlaceItem(GameObject item)
        {
            base.PlaceItem(item);
            item.SetActive(true);
            GameManager.Instance.PlayHaptic();
            if (IsFull())
            {
                level.AIInteractableHandler.AddToStockedContainers(this);
                level.AIInteractableHandler.RemoveFromAvailableInteractables(this);
            }
            else
            {
                level.AIInteractableHandler.AddToAvailableInteractables(this);
                level.AIInteractableHandler.AddToStockedContainers(this);
            }
        }

        public override ShelfProduct GetItemToGrab()
        {
            var item = (productsStack.Count > 0) ? productsStack.Pop() : null;

            return item;
        }

        public override void ItemRemovedFromShelf(ShelfProduct item)
        {
            //throw new System.NotImplementedException();
            if (!productsStack.Contains(item))
            {
                item.gameObject.SetActive(false);
            }

            level.AIInteractableHandler.AddToAvailableInteractables(this);

            if (IsEmpty())
            {
                level.AIInteractableHandler.RemoveFromStockedContainer(this);
            }
            else
            {
                level.AIInteractableHandler.AddToStockedContainers(this);
            }

        }
    }
}
