using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallyTools;


namespace SuperStoreRush
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private bool hasWatchedTutrial;

        [SerializeField]
        private bool HasWatchedTutorial
        {
            get
            {
                if (SaveGameManager.Instance.KeyExists(hasWatchedTuorial_Key))
                {
                    return hasWatchedTutrial = SaveGameManager.Instance.GetBool(hasWatchedTuorial_Key);
                }
                else
                {
                    return hasWatchedTutrial;
                }
            }
            set
            {
                SaveGameManager.Instance.SaveValue(hasWatchedTuorial_Key, hasWatchedTutrial = value);
            }
        }

        //public bool

        private string hasWatchedTuorial_Key = "HasWatchedTutorial_Key";

        [SerializeField]
        private TutorialStep[] steps;

        private TutorialStep curStep;

        private int index;

        private List<ITutorialListeners> listeners;

        // Start is called before the first frame update
        public void Init(List<ITutorialListeners> l)
        {
            listeners = l;
            if (!HasWatchedTutorial)
            {
                //HasWatchedTutorial = true;
                curStep = steps[0];
                index = 0;
                InitStep();
            }
            else
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].OnTutorialWatched();
                }
                gameObject.SetActive(false);
            }
        }

        public void NextStep()
        {
            //if (hasWatchedTutorial) return;
            curStep?.Close();
            if (index + 1 >= steps.Length)
            {
                HasWatchedTutorial = true;
                gameObject.SetActive(false);
                return;
            }

            index += 1;
            var newStep = steps[index];
            curStep = newStep;
            InitStep();
        }

        private void InitStep()
        {
            curStep.Init();
        }

        [System.Serializable]
        public class TutorialStep
        {
            [SerializeField]
            private GameObject textInstruction;

            [SerializeField]
            private GameObject StepBoundaries;

            public void Init()
            {
                StepBoundaries?.SetActive(true);
                if (textInstruction != null)
                {
                    textInstruction.SetActive(true);
                }
            }

            public void Close()
            {
                StepBoundaries?.SetActive(false);
                if (textInstruction != null)
                {
                    textInstruction.SetActive(false);
                }
            }
        }
    }
}
