using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class Money_Store : Money
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstantStrings.PlayerTag))
            {
                PlayerCollect(other.transform);
            }
        }

        protected override void SendMoneyCollectedEvent(Vector3 playerPos)
        {
            GameManager.Instance.GetMoneyHandler().OnMoneyCollected(transform.position);
            gameObject.SetActive(false);
        }
    }
}
