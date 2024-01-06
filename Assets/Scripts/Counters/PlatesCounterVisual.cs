using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private float plateOffsetY = 0.1f;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        platesCounter.OnPlateTaken += PlatesCounterOnOnPlateTaken;
    }

    private void PlatesCounterOnOnPlateTaken(object sender, EventArgs e)
    {
        GameObject takenPlate = plateVisualGameObjectList[^1];
        plateVisualGameObjectList.Remove(takenPlate);
        Destroy(takenPlate);
    }

    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
