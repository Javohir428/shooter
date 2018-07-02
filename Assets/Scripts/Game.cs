using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets
{
	public sealed class Game : MonoBehaviour 
	{
        private void Start()
        {
            StartCoroutine(DimLights());
		}

        private IEnumerator DimLights()
        {
            const float duration = 2;
            float timer = 2;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
                RenderSettings.ambientLight = Color.white * (timer / duration);
            }
        }
	}
}