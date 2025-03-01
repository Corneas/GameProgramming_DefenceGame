using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PenguinArea : MonoBehaviour
{
    public PenguinAgent penguinAgent;
    public GameObject penguinBaby;
    public TextMeshPro cumulativeRewardText;
    public Fish fishPrefab;
    private List<GameObject> fishList = new List<GameObject>();

    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if(maxRadius > minRadius)
        {
            radius = Random.Range(minRadius, maxRadius);
        }

        if( maxAngle > minAngle)
        {
            angle = Random.Range(minAngle, maxAngle);
        }

        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }

    private void Start()
    {
        ResetArea();
    }

    private void Update()
    {
        cumulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString("0.00");
    }

    private void RemoveAllFish()
    {
        if(fishList != null)
        {
            for(int i = 9; i < fishList.Count; ++i)
            {
                if(fishList[i] != null)
                {
                    Destroy(fishList[i]);
                }
            }
        }

        fishList = new List<GameObject>();
    }

    public void ResetArea()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        SpawnFish(4, 0.5f);
    }

    public void RemoveSpecificFish(GameObject fishObject)
    {
        fishList.Remove(fishObject);
        Destroy(fishObject);
    }

    public int FishRemaining
    {
        get => fishList.Count;
    }

    private void PlacePenguin()
    {
        Rigidbody rigidbody = penguinAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * 0.5f;
        penguinAgent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    private void PlaceBaby()
    {
        Rigidbody rigidbody = penguinBaby.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * 0.5f;
        penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void SpawnFish(int count, float fishSpeed)
    {
        for(int i = 0; i < count; ++i)
        {
            GameObject fishObject = Instantiate(fishPrefab.gameObject);
            fishObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * 0.5f;
            fishObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            fishObject.transform.SetParent(transform);

            fishList.Add(fishObject);

            fishObject.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }
}
