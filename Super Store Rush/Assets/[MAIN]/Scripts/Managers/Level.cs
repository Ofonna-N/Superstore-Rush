using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;

namespace SuperStoreRush
{
    public class Level : MonoBehaviour
    {
        [SerializeField, BoxGroup("Customer Data", centerLabel: true)]
        private AIInteractableHandler aiInteractableHandler;
        public AIInteractableHandler AIInteractableHandler => aiInteractableHandler;

        [SerializeField, BoxGroup("HR Data", centerLabel: true)]
        private HRDept hRDept;

        [SerializeField, BoxGroup("HR Data", centerLabel: true)]
        private GameObject hrCamera;



        [SerializeField, BoxGroup("HR Data", centerLabel: true)]
        private bool camActivated;
        public bool CamActivated => camActivated;


        [ShowInInspector, BoxGroup("Interactables", centerLabel:true)]
        private Interactable[] interactables;



        [SerializeField, BoxGroup("UI Data", centerLabel:true)]
        private Canvas levelCanvas;

        [SerializeField, BoxGroup("AI Data", centerLabel: true)]
        private NavMeshSurface storeNavSurface;


        public void Init()
        {
            //Debug.Log(GameManager.Instance == null);
            interactables = GetComponentsInChildren<Interactable>();
            aiInteractableHandler.Init();
            levelCanvas.worldCamera = GameManager.Instance.Main_Camera;

            for (int i = 0; i < interactables.Length; i++)
            {
                var interactable = interactables[i];
                if (interactable.gameObject.activeInHierarchy)
                {
                    interactable.Init(this);
                }
            }
        }

        public Transform GetCustomerStartPoint(AIType aIType)
        {
            return aiInteractableHandler.GetCustomerStartPoint(aIType);
        }


        public void ActivateHRCamera(bool activate)
        {
            hrCamera.SetActive(activate);
            camActivated = activate;
        }

        public HRDept GetHRDept()
        {
            return hRDept;
        }

        public void BakeNavmesh()
        {
            storeNavSurface.BuildNavMesh();
        }

    }
}
