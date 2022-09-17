using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStoreRush
{
    public class PlayerStatesController : MonoBehaviour
    {

        [SerializeField]
        private InteractionStatesController interactionStatesController;

        private void Awake()
        {
            //anim = GetComponentInChildren<Animator>();
            //interactionStatesController = new InteractionStatesController();
        }

        // Start is called before the first frame update
        void Start()
        {
            interactionStatesController.Init();
        }

        // Update is called once per frame
        void Update()
        {
            interactionStatesController.Tick();
        }
    }
}
