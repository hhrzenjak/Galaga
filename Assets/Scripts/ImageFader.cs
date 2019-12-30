using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{

    public Vector2 AlphaValues;
    public float Period;

    public bool IsContinuous;

    private bool _fading;
    private float _timer;

    private bool _finished;

    private Image _image;

    private void Awake()
    {

        _fading = true;
        _image = GetComponent<Image>();

        if (_image == null)
            Destroy(gameObject);

    }

    private void Update()
    {

        if (_finished == true)
            return;

        _timer += Time.deltaTime;

        float startingValue = _fading ? AlphaValues.x : AlphaValues.y;
        float endingValue = _fading ? AlphaValues.y : AlphaValues.x;

        Color color = _image.color;
        color.a = Mathf.Lerp(startingValue, endingValue, _timer / Period);
        _image.color = color;

        if (_timer > Period)
        {

            _fading = _fading ? false : true;
            _timer = 0;

            if (IsContinuous == false)
                _finished = true;
                

        }

    }

    public void StartFading()
    {
        _finished = false;
        _timer = 0;
    }

    public void StopFading()
    {
        _finished = true;
    }

}
