using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NWH
{
    [System.Serializable]
    public class Templer
    {
        public float t;
        public float d;

        public Templer(float d, float t)
        {
            this.t = t;
            this.d = d;
        }
    }
}
