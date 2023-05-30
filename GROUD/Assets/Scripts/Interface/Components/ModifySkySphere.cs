using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifySkySphere : MonoBehaviour
{
    [SerializeField] private Transform skysphereTransform;
    [SerializeField] private Light[] lightsPrefab;
    [SerializeField] private Light light;
    [SerializeField] private float speedSkySphere = 15f;


    private IEnumerator ModifySky()
    {
        float skyRot = skysphereTransform.rotation.eulerAngles.z - 90f;
        
while (skysphereTransform.rotation.eulerAngles.z > skyRot)
        {
            skysphereTransform.Rotate(Vector3.forward * (speedSkySphere * Time.deltaTime));
            yield return null;
        }
        yield return new WaitForSeconds(5);
        
        StartCoroutine(ModifySky());
    }
    
    void Start()
    {
        StartCoroutine(ModifySky());
    }
}
