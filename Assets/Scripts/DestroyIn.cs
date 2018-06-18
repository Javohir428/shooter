using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
	public sealed class DestroyIn : MonoBehaviour 
	{
        public float _seconds = 1;
		private void Start() 
		{
            Destroy(gameObject, _seconds);
		}
	}
}