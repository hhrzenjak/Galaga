using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Vector2 ShakeAmount;
    public Vector2 ShakeDuration;

    private Vector3 _originalPosition;
    private Vector3 _originalRotation;

    private float _timer;

    private Transform _transform;

    private void Awake()
    {

        _transform = transform;

        _originalPosition = _transform.position;
        _originalRotation = _transform.eulerAngles;

    }

    private void Update()
    {

        if (GameManager.GM && GameManager.GM.Paused)
            _timer = 0;
        
        if (_timer > 0)
        {
            _transform.eulerAngles = _originalRotation + Vector3.forward * new Vector2(-0.5f, 0.5f).RandomValue();
            _transform.position = _originalPosition + Random.insideUnitSphere * ShakeAmount.RandomValue();
            _timer -= Time.deltaTime;
        }

        else
        {
            _transform.eulerAngles = _originalRotation;
            _transform.position = _originalPosition;
            _timer = 0;
        }

    }

    public void Shake()
    {
        _transform.eulerAngles = _originalRotation;
        _transform.position = _originalPosition;
        _timer = ShakeDuration.RandomValue();
    }

}
