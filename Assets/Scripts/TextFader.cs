using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour
{

    public Vector2 AlphaValues;
    public float Duration;

    public bool IsContinuous;

    private bool _fading;
    private float _timer;

    private bool _finished;

    private Text _text;

    private void Awake()
    {

        _fading = true;
        _text = GetComponent<Text>();

        if (_text == null)
            Destroy(gameObject);

    }

    private void Update()
    {

        if (_finished == true)
            return;

        _timer += Time.deltaTime;

        float startingValue = _fading ? AlphaValues.x : AlphaValues.y;
        float endingValue = _fading ? AlphaValues.y : AlphaValues.x;

        Color color = _text.color;
        color.a = Mathf.Lerp(startingValue, endingValue, _timer / Duration);
        _text.color = color;

        if (_timer > Duration)
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
