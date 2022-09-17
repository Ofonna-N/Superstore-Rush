using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace SuperStoreRush
{
    public class CharacterRigRef : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;
        public Animator Anim => anim;

        [SerializeField]
        private Transform packagesStackHolder;
        public Transform PackagesStackHolder => packagesStackHolder;


        [SerializeField]
        private Rig carryIkRig;
        public Rig CarryIkRig => carryIkRig;
    }
}
