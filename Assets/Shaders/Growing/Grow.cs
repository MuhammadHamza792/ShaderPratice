using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shaders.Growing
{
    public class Grow : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _renderer;
        [SerializeField] private float _refreshRate = .05f;
        [SerializeField] private float _timeToGrow;
        [SerializeField] private float _minGrowth = 0.01f;
        [SerializeField] private float _maxGrowth = 0.99f;

        private List<Material> _material;
        private float _growFill;
        private static readonly int GrowValue = Shader.PropertyToID("_Panner");

        private void Awake()
        {
            _material ??= new List<Material>();
            
            foreach (var rend in _renderer)
            {
                _material.Add(rend.material);
            }
        }

        private void Start() => StartCoroutine(ToggleGrowing());

        private IEnumerator ToggleGrowing()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    foreach (var mat in _material)
                    {
                        var growthValue  = mat.GetFloat(GrowValue);
                        StartCoroutine(growthValue >= _maxGrowth
                            ? ChangeGrowth(mat, growthValue, _minGrowth, _timeToGrow, _refreshRate, true)
                            : ChangeGrowth(mat, growthValue, _maxGrowth, _timeToGrow, _refreshRate, false));
                    }
                }

                yield return null;
            }
        }

        private IEnumerator ChangeGrowth(Material mat,float currentValue, float endValue, float duration, float refreshRate, bool shrink)
        {
            var growValue = shrink ? 1f : 0f;
            while(true)
            {
                switch (shrink)
                {
                    case true when growValue <= 0:
                    case false when growValue >= 1:
                        yield break;
                }
                growValue += shrink ? - 1 /(duration / refreshRate) : 1 / (duration / refreshRate);
                mat.SetFloat(GrowValue, growValue);
                yield return new WaitForSeconds(_refreshRate);
            }
        }
    }
}
