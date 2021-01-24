using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbox.UI.Presenters
{

    [RequireComponent(typeof(Image))]
    public class FillBar : MonoBehaviour
    {
        [SerializeField] private Image image;
        public void etProgress(float factor) {
            Debug.Log(factor);
            Vector3 scale = image.transform.localScale;
            scale.x = factor;
            image.transform.localScale = scale;
        }
    }
}