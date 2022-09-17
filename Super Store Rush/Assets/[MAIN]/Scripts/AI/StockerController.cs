using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    public class StockerController : MonoBehaviour, I_AI
    {
        [SerializeField]
        private NavMeshAgent navAgent;
        //public NavMeshAgent NavAgent => navAgent;

        [SerializeField]
        private CharacterRigRef stockerRigRef;

        [SerializeField]
        private UpgradaleData upgradaleData;

        [ShowInInspector]
        private Stack<Package> packagesStack;


        [SerializeField, ReadOnly]
        private string state;

        private I_StockerStates currentState;

        private Idle_Stocker idleState;
        private GoToOflloader_Stocker toOffloader;
        private StackPackages_Stocker stackPackages;
        private GoToShelf_Stocker toShelf;
        private PlacePackages_Stocker placePackages;

        //private Transform[] waitPoints;


        public void Init()
        {
            packagesStack = new Stack<Package>();

            idleState = new Idle_Stocker(this, navAgent, stockerRigRef, packagesStack, upgradaleData);
            toOffloader = new GoToOflloader_Stocker(navAgent, stockerRigRef, packagesStack, this);
            stackPackages = new StackPackages_Stocker(stockerRigRef, packagesStack, upgradaleData, this);
            toShelf = new GoToShelf_Stocker(navAgent, stockerRigRef.Anim, this);
            placePackages = new PlacePackages_Stocker(packagesStack, stockerRigRef, this);

            navAgent.speed = upgradaleData.WorkerSpeed.Value;

            SetState(StockerStates.Idle);
        }

        private float step = 0f;
        private float ikWeight = 0f;
        private bool isLifting = false;

        public void Tick()
        {
            //Debug.Log("is moving: ");
            currentState.UpdateState();

            step += Time.deltaTime * 8f;

            if (packagesStack.Count > 0)
            {
                Debug.Log("Is Lifting");
                isLifting = true;
                ikWeight = Mathf.Lerp(ikWeight, 1f, step);

                if (ikWeight >= 1f) step = 0f;
            }
            else
            {
                Debug.Log("Is not lifting");
                isLifting = false;
                ikWeight = Mathf.Lerp(ikWeight, 0f, step);

                if (ikWeight <= 0f) step = 0f;
            }

            stockerRigRef.Anim.SetBool(ConstantStrings.Lifting_Param, isLifting);
            stockerRigRef.CarryIkRig.weight = ikWeight;

        }


        public void SetState(StockerStates states, PackageOffloader_Anim offloader = null, IPlaceable container = null)
        {
            /*if (currentState != null)
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

            }*/
            if (currentState != null)
            {
                if (!currentState.CloseState())
                {
                    return;
                }
            }

            switch (states)
            {
                case StockerStates.Idle:
                    currentState = idleState;
                    (currentState as Idle_Stocker).OpenState();
                    state = "Idle";
                    break;
                case StockerStates.ToOffloader:
                    currentState = toOffloader;
                    (currentState as GoToOflloader_Stocker).OpenState(offloader);
                    state = "toOffloader";
                    break;
                case StockerStates.StackPackages:
                    currentState = stackPackages;
                    (currentState as StackPackages_Stocker).OpenState(offloader);
                    state = "stacking Packages";
                    break;
                case StockerStates.ToShelf:
                    currentState = toShelf;
                    (currentState as GoToShelf_Stocker).OpenState(container as Container);
                    state = "going to shelf";
                    break;
                case StockerStates.PlacePackages:
                    currentState = placePackages;
                    (currentState as PlacePackages_Stocker).OpenState(container);
                    state = "Placing packages in shelf";
                    break;
                default:
                    break;
            }

        }
    }
}
