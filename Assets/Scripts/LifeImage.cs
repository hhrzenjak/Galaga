using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LifeImage : MonoBehaviour
{

    public Sprite LifeFullSprite;
    public Sprite LifeMissingSprite;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void UpdateLifeImage(bool full)
    {
        _image.sprite = full ? LifeFullSprite : LifeMissingSprite;
    }

}
