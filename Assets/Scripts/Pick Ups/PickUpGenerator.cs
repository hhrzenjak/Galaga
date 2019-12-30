using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickUpGenerator : MonoBehaviour
{

    #region PICK UP GENERATOR PROPERTY

    private static PickUpGenerator _generator;

    public static PickUpGenerator Generator
    {
        get
        {
            if (_generator == null)
                _generator = FindObjectOfType<PickUpGenerator>();
            return _generator;
        }
    }

    #endregion

    public List<PickUp> PickUpPrefabs = new List<PickUp>();

    private void Awake()
    {

        #region PICK UP GENERATOR PROPERTY SET-UP

        if (_generator == null)
            _generator = this;

        if (_generator.Equals(this) == false)
            Destroy(gameObject);

        #endregion

    }

    public PickUp GeneratePickUp()
    {

        if (PickUpPrefabs.Count == 0)
            return null;

        return PickUpPrefabs[Random.Range(0, PickUpPrefabs.Count)];

    }

}