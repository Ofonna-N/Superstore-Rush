using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class CustomerController : MonoBehaviour, I_AI
    {
        [SerializeField]
        private NavMeshAgent navAgent;
        //public NavMeshAgent NavAgent => navAgent;

        [SerializeField]
        private Animator anim;

        [SerializeField]
        private Rig handIK_Rig;

        [SerializeField]
        private CustomerKart kart;
        public CustomerKart Kart => kart;

        [SerializeField]
        private GameObject[] checkoutBags;

        [SerializeField, ReadOnly]
        private string state;

        private I_CustomerStates currentState;

        private Idle_Customer idleState;
        private ToShelf_Customer toShelfState;
        private GrabItem_Customer grabItemState;
        private ToRegister_Customer toRegisterState;
        private LeaveStore_Customer leavingStoreState;

       // public CustomerController QueNeighbor { get; set; } //customer behind me in Que

        public void Init()
        {
            idleState = new Idle_Customer(this, navAgent, anim);
            toShelfState = new ToShelf_Customer(navAgent, anim, this);
            grabItemState = new GrabItem_Customer(this, anim);
            toRegisterState = new ToRegister_Customer(navAgent, anim, this);
            leavingStoreState = new LeaveStore_Customer(navAgent, anim, this);

            SetState(CustomerStates.Idle);
        }

        public void Tick()
        {
            //Debug.Log("is moving: ");
            currentState.UpdateState();
        }

        public void SetState(CustomerStates states, Container container = null, Register register = null, bool registerWorked = false)
        {
            if (currentState != null)
            {
                if (currentState.GetType() == typeof(ToRegister_Customer))
                {
                    if (!(currentState as ToRegister_Customer).CloseState(registerWorked))
                    {
                        return;
                    }
                }
                else if (!currentState.CloseState())
                {
                    return;
                }

            }

            switch (states)
            {
                case CustomerStates.Idle:

                    currentState = idleState;
                    currentState.OpenState();
                    state = "Idle";
                    break;
                case CustomerStates.ToShelf:
                    currentState = toShelfState;
                    //Debug.Log(currentState == null);
                    (currentState as ToShelf_Customer).OpenState(container);
                    state = "To Shelf";
                    break;
                case CustomerStates.GrabItem:
                    currentState = grabItemState;
                    (currentState as GrabItem_Customer).OpenState(container);
                    state = "Grab Item";
                    break;
                case CustomerStates.GoToRegister:

                    currentState = toRegisterState;
                    (currentState as ToRegister_Customer).OpenState(register);
                    state = "To Register";
                    break;
                case CustomerStates.LeaveStore:
                    currentState = leavingStoreState;
                    currentState.OpenState();
                    state = "Leaving";
                    break;
                default:
                    break;
            }

        }

        public void ActivateCheckoutBags(bool value)
        {
            for (int i = 0; i < checkoutBags.Length; i++)
            {
                checkoutBags[i].SetActive(value);
            }
        }

        public void SetHandIkWeight(float value)
        {
            handIK_Rig.weight = value;
        }
    }
}
