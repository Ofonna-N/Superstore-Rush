using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public static class ConstantStrings
    {
        //Tags
        public const string PlayerTag = "Player";

        public const string StockerTag = "Stocker";

        //Animator Parameters
        public static int Lifting_Param = Animator.StringToHash("IsLifting");

        //Ai Anim Parameters
        public static int Movement_Param = Animator.StringToHash("Movement");

    }
}
