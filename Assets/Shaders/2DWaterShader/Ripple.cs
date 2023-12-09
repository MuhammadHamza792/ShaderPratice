using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripple : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int _numberOfRipples = 4;
    [SerializeField] private float _rippleStrength = .5f;
    [SerializeField] private float _rippleSpreadSpeed = .5f;
    [SerializeField] private float _rippleDecipateRate = .99f;
    private Material _material;
    private Plane _plane;
    
    private Camera _camera;
    private Coroutine _showRippleCo;

    private int _rippleIndex;
    private float[] _waveAmplitude;
    private float[] _waveDistance;
    private bool[] _creatingRipple;
    
    private void Awake()
    {
        _material = _renderer.material;
        _waveAmplitude = new float[_numberOfRipples];
        _waveDistance = new float[_numberOfRipples];
        _creatingRipple = new bool[_numberOfRipples];
        _camera = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < _numberOfRipples; i++)
        {
            if (_waveAmplitude[i] > 0)
            {
                _waveAmplitude[i] *= _rippleDecipateRate;
                _waveDistance[i] += _rippleSpreadSpeed;
                _material.SetFloat($"_Distance{i + 1}" , _waveDistance[i]);
                _material.SetFloat($"_WaveAmplitude{i + 1}" , _waveAmplitude[i]);
            }

            if (_waveAmplitude[i] < 0.01f)
            {
                _material.SetFloat($"_WaveAmplitude{i + 1}" , 0);
                _waveDistance[i] = 0;
            }
        }
        
        if(!Input.GetMouseButtonDown(0)) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if(!hit.collider.TryGetComponent(out Ripple _)) return;
            CreateRipple(hit.point);
        }
    }

    private void CreateRipple(Vector3 rippleToCreateAt)
    {
        if (_rippleIndex >= _numberOfRipples)
        {
            _rippleIndex = 0;
        }

        _waveAmplitude[_rippleIndex] = _rippleStrength;
        _waveDistance[_rippleIndex] = 0;
        
        _material.SetVector($"_RippleOrigin{_rippleIndex + 1}" , rippleToCreateAt);
        _material.SetFloat($"_WaveAmplitude{_rippleIndex + 1}" , 0);
        //_creatingRipple[_rippleIndex] = true;*/
        _rippleIndex++;
    }
    
}
