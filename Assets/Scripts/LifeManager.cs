using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class LifeManager : MonoBehaviour
{

    //public bool Active2;
    public int MaximumLives;

    public bool _active;
    public int _lives;
    public float currentHealthPercentage = 1;

    protected CameraShake _camera;

    private void Awake()
    {

        //_active = Active2;
        _lives = MaximumLives;
        currentHealthPercentage = 1;

        if (Camera.main)
            _camera = Camera.main.GetComponent<CameraShake>();

    }

    protected void Shake()
    {
        if (_camera)
            _camera.Shake();
    }
    
    public void Activate()
    {
        _active = true;
    }

    public void Deactivate()
    {
        _active = false;
    }


    public abstract void Hit();

}
