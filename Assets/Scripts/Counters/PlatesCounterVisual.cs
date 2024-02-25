using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjcetList;

    private void Awake()
    {
        plateVisualGameObjcetList = new List<GameObject>();
    }

    private void Start()
    {
        plateCounter.OnPlateSpawn += PlateCounter_OnPlateSpawn;
        plateCounter.OnPlateRemove += PlateCounter_OnPlateRemove;
    }

    private void PlateCounter_OnPlateRemove(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjcetList[plateVisualGameObjcetList.Count - 1];
        plateVisualGameObjcetList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlateCounter_OnPlateSpawn(object sender, System.EventArgs e)
    {
       Transform plateVisualTransform =  Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjcetList.Count, 0);

        plateVisualGameObjcetList.Add(plateVisualTransform.gameObject);
    }
}
