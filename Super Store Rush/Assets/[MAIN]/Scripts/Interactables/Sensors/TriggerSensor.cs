using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public abstract class TriggerSensor : MonoBehaviour
    {
        protected abstract void OnTriggerEnter(Collider other);

        protected abstract void OnTriggerExit(Collider other);
    }
}
