using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Sirenix.OdinInspector;


namespace SuperStoreRush
{
    public class InteractionStatesController : MonoBehaviour
    {

        [SerializeField]
        private UpgradaleData upgradaleData;

        [SerializeField]
        private MoreMountains.Feedbacks.MMF_Player stackPackage_Fb;

        private CharacterRigRef characterRig;

        private CharacterInput characterInput;

        [ShowInInspector]
        private Stack<Package> packagesStack;


        private I_InteractionStates currentState;

        private Idle_Player idleState;
        private Stacking_Player StackState;
        private Placing_Player placeState;
        private Unlocking_Player unlockingState;
        private WorkingRegister_Player workingRegisterState;
        private CollectReward_Player collectingRewardState;

        public void Init()
        {
            characterRig = GetComponentInChildren<CharacterRigRef>();
            characterInput = GetComponentInChildren<CharacterInput>();
            packagesStack = new Stack<Package>();

            idleState = new Idle_Player();
            StackState = new Stacking_Player(characterRig, packagesStack, upgradaleData, characterInput);
            placeState = new Placing_Player(packagesStack, characterRig, characterInput);
            unlockingState = new Unlocking_Player(characterInput);
            workingRegisterState = new WorkingRegister_Player(characterInput);
            collectingRewardState = new CollectReward_Player(characterInput);

            SetState(PlayerStates.Idle);
        }

        public void Tick()
        {
            //Debug.Log("is moving: " + characterInput.IsMoving());
            currentState.UpdateState();
        }

        public void SetState(PlayerStates states, Offloader offloader = null, IUnlockable unlockable = null, 
            IPlaceable container = null, Register register = null)
        {
            currentState?.CloseState();

            switch (states)
            {
                case PlayerStates.Idle:
                    currentState = idleState;
                    currentState.OpenState();
                    break;
                case PlayerStates.Stacking:
                    currentState = StackState;
                    (currentState as Stacking_Player).OpenState(offloader, stackPackage_Fb);
                    break;
                case PlayerStates.Placing:
                    currentState = placeState;
                    (currentState as Placing_Player).OpenState(container);
                    break;
                case PlayerStates.Paying:
                    currentState = unlockingState;
                    (currentState as Unlocking_Player).OpenState(unlockable);
                    break;
                case PlayerStates.WorkingRegister:
                    currentState = workingRegisterState;
                    (currentState as WorkingRegister_Player).OpenState(register);
                    break;
                case PlayerStates.CollectingReward:
                    currentState = collectingRewardState;
                    (currentState as CollectReward_Player).OpenState(offloader);
                    break;
                default:
                    break;
            }


        }
    }
}
