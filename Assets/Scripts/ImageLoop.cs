using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoop : MonoBehaviour
{

    public Transform ImagePrefab;
    public float Speed;

    private List<Transform> _images = new List<Transform>();
    private float _speed;

    private Vector2 _imageSize;
    private Vector2 _screenSize;

    private Transform _transform;

    private void Awake()
    {

        if (ImagePrefab == null)
            return;

        if (ImagePrefab.GetComponent<SpriteRenderer>() == null)
            return;

        _imageSize = ImagePrefab.GetComponent<SpriteRenderer>().bounds.max - ImagePrefab.GetComponent<SpriteRenderer>().bounds.min;
        _screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)) * 2;

        _speed = Mathf.Abs(Speed);
        _transform = transform;

        int number = (_imageSize.y < _screenSize.y) ? (int)(_screenSize.y / _imageSize.y) + 1 : 2;

        for (int counter = 0; counter < number; counter++)
            _images.Add(Instantiate(ImagePrefab, new Vector2(0, _imageSize.y * counter + (_imageSize.y - _screenSize.y)), Quaternion.identity, _transform));

    }

    private void Update()
    {

        if (_images.Count == 0)
            return;

        if (_images[0].position.y <= (_imageSize.y - _screenSize.y) / 2 - _imageSize.y)
        {

            Transform image = _images[0];
            _images.RemoveAt(0);
            Destroy(image.gameObject);

            _images.Add(Instantiate(ImagePrefab, new Vector2(0, _images[_images.Count - 1].position.y + _imageSize.y), Quaternion.identity, _transform));

        }

    }

    private void LateUpdate()
    {
        for (int index = _images.Count - 1; index >= 0; index--)
            _images[index].position -= new Vector3(0, _speed, 0) * Time.deltaTime;
    }

    public void ChangeLoopSpeed(float speed)
    {
        _speed = Mathf.Abs(speed);
    }

}
