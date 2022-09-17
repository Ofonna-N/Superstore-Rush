using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using UnityEngine.InputSystem;

namespace SuperStoreRush
{
    public class CharacterInput : CMF.CharacterInput
    {
        [SerializeField]
        private InputActionMap inputActions;

       // private MoveCommand moveCommand;
        private InputAction onMove_action;

        private bool canMove = true;

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        void Start()
        {
            onMove_action = inputActions["Move"];
        }


        public override float GetHorizontalMovementInput()
        {
            //throw new System.NotImplementedException();
            return (canMove) ? onMove_action.ReadValue<Vector2>().x : 0f;
        }

        public override float GetVerticalMovementInput()
        {
            //throw new System.NotImplementedException();
            return (canMove) ? onMove_action.ReadValue<Vector2>().y : 0f;
        }

        public override bool IsJumpKeyPressed()
        {
            //throw new System.NotImplementedException();
            return false;
        }

        public bool IsMoving()
        {
            return Mathf.Abs(GetHorizontalMovementInput()) > 0.1f || Mathf.Abs(GetVerticalMovementInput()) > 0.1f;
        }

        public void SetCanMove(bool flag)
        {
            canMove = flag;
        }

        // Start is called before the first frame update
        
    }

    /*public abstract class Command
    {
        protected Character character;

        public Command(Character c)
        {
            character = c;
        }

        public abstract void Execute();
    }

    public class MoveCommand : Command
    {
        public MoveCommand(Character c) : base(c)
        {
            
        }

        public override void Execute()
        {
            //throw new System.NotImplementedException();
        }

        public void Execute(float moveValue)
        {
            character.Move(moveValue);
            character.Rotate(moveValue);
        }
    }*/
}
