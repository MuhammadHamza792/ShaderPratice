using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RadialLoading : MonoBehaviour
{
    [SerializeField] private Material _radialMat;
    [SerializeField] private float _startFillValue;
    [FormerlySerializedAs("_rotationSpeed")]
    [SerializeField] private float _rotationSpeedValue;
    [FormerlySerializedAs("_fillRate")] 
    [SerializeField] private float _fillRateValue;
    [SerializeField] private bool _changeFillValue;
    [SerializeField] private bool _changeRotation;

    private Coroutine _radialLoadingCo;

    private Transform _transform;
    private static readonly int Fill = Shader.PropertyToID("_Fill");
    private static readonly int Rotate = Shader.PropertyToID("_Rotate");
    private static readonly int Reverse = Shader.PropertyToID("_Reverse");

    private float _rotationSpeed;
    private float _fillRate;
    private bool _reverse;
    
    private void Awake() => _transform = transform;
    
    [ContextMenu("Play Radial Loading")]
    public void PlayRadialLoading()
    {
        _radialMat.SetFloat(Rotate,-3.14f);
        _radialMat.SetFloat(Fill, _startFillValue);
        if(_radialLoadingCo != null) StopCoroutine(_radialLoadingCo); 
        _radialLoadingCo = StartCoroutine(RadialLoadingAnim());
    }

    public IEnumerator RadialLoadingAnim()
    {
        var rotationSpeed = 0f;
        
        while (true)
        {
            ChangeRotation();
            ChangeFillRate();
            yield return null;
        }
    }

    private void ChangeRotation()
    {
        if(!_changeRotation) return;
        
        if (_radialMat.GetFloat(Rotate) >= 3.14f)
        {
            _radialMat.SetFloat(Rotate, -3.14f);
        }

        _rotationSpeed = Time.deltaTime * _rotationSpeedValue;
        _radialMat.SetFloat(Rotate, _radialMat.GetFloat(Rotate) + _rotationSpeed);
    }

    private void ChangeFillRate()
    {
        if (!_changeFillValue) return;

        if (_radialMat.GetFloat(Fill) >= 1f)
        {
            _reverse = !_reverse;
            _radialMat.SetInt(Reverse, _reverse ? 1 : 0);
            _radialMat.SetFloat(Fill, !_reverse ? .2f : 0);
        }

        _fillRate = Time.deltaTime * _fillRateValue;
        _radialMat.SetFloat(Fill, _radialMat.GetFloat(Fill) + _fillRate);
    }


    [ContextMenu("Stop Radial Loading")]
    public void StopRadialLoading()
    {
        if (_radialLoadingCo != null) StopCoroutine(_radialLoadingCo);
        _radialMat.SetFloat(Rotate,-3.14f);
        _radialMat.SetFloat(Fill,0);
    }
}
