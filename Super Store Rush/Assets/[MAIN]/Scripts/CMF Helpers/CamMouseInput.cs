using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using Sirenix.OdinInspector;

namespace SuperStoreRush
{
    [InfoBox("Chamera controller requires mouse input so, this is created to avoid camera movement")]
    public class CamMouseInput : CMF.CameraInput
    {
        public override float GetHorizontalCameraInput()
        {
            //throw new System.NotImplementedException();
            return 0f;
        }

        public override float GetVerticalCameraInput()
        {
            //throw new System.NotImplementedException();
            return 0f;
        }
    }
}
