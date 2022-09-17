using System.Collections;
using UnityEngine;

namespace SuperStoreRush
{
    public class CashierController : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;
        //public Animator Anim => anim;

        private int workRegisterAnimParam = Animator.StringToHash("WorkRegister");

        public void WorkRegister(bool flag)
        {
            anim.SetBool(workRegisterAnimParam, flag);
        }

        /*public void Init()
        {
            //throw new System.NotImplementedException();
        }

        public void Tick()
        {
            //throw new System.NotImplementedException();
        }*/
    }
}