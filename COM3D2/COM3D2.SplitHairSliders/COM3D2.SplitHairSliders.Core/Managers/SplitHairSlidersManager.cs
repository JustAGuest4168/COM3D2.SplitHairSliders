using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.SplitHairSliders.Plugin.Core
{
    class SplitHairSlidersManager : MonoBehaviour
    {
        public bool Initialized { get; private set; }
        public void Initialize()
        {
            //Copied from examples
            if (this.Initialized)
                return;
            SplitHairSlidersHooks.Initialize();
            this.Initialized = true;
            UnityEngine.Debug.Log("Split Hair Sliders: Manager Initialize");
        }

        public void Awake()
        {
            //Copied from examples
            UnityEngine.Debug.Log("Split Hair Sliders: Manager Awake");
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)this);
        }
    }
}
