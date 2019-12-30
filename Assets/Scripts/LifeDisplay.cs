using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LifeDisplay : MonoBehaviour
{

    public LifeImage LifeImagePrefab;

    private List<LifeImage> _images = new List<LifeImage>();

    public void RefreshLifeDisplay(int lives)
    {

        for (int counter = 0; counter < _images.Count; counter++)
        {

            LifeImage image = _images[counter];

            if (counter < lives)
                image.UpdateLifeImage(true);
            else
                image.UpdateLifeImage(false);

        }

    }

    public void SetUpLifeDisplay(int maximum)
    {

        if (_images.Count > 0)
            return;

        Transform parent = transform;

        for (int counter = 0; counter < maximum; counter++)
            _images.Add(Instantiate(LifeImagePrefab, parent));

    }

}
