using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeChanger : MonoBehaviour
{
    private GameObject primaryPrefab;
    [SerializeField]
    private GameObject secondaryPrefab;
    private TapManager tapManager;

    private bool isPrimary = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        tapManager = GetComponent<TapManager>();
        primaryPrefab = tapManager.objectToPlace;
    }

    public void ToggleShape()
    {
        tapManager.objectToPlace = isPrimary ? secondaryPrefab : primaryPrefab;
        isPrimary = !isPrimary;
    }
}
