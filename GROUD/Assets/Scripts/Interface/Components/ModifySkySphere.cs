using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifySkySphere : MonoBehaviour
{
    [SerializeField] private Transform skysphereTransform;
    [SerializeField] private Light[] lightsPrefab;
    [SerializeField] private Light light;
    [SerializeField] private float speedSkySphere = 15f;
    [SerializeField] private float speedSkyLight = 15f;
    int index = 0;

    private IEnumerator ModifySky()
    {
        index++;
        StartCoroutine(ModifyLight());
        
        float rotate = 90f;
        while (rotate > 0f)
        {
            float rotation = (speedSkySphere * Time.deltaTime);
            rotate -= rotation;
            if (rotate < 0f)
                rotation += rotate;
            skysphereTransform.Rotate(Vector3.back * rotation);
            yield return null;
        }
        yield return new WaitForSeconds(1);

        StartCoroutine(ModifySky());
    }
    private IEnumerator ModifyLight()
    {
        if (index >= lightsPrefab.Length)
            index = 0;

        Color basicColor = light.color;
        Color prefabColor = lightsPrefab[index].color;
        
        float basicIntensity = light.intensity;
        float prefabIntensity = lightsPrefab[index].intensity;
        
        float basicstrength = light.shadowStrength;
        float prefabstrength = lightsPrefab[index].shadowStrength;
        Quaternion basicRotation = light.transform.rotation;
        Quaternion prefabRotation = lightsPrefab[index].transform.rotation;

        float time = 0f;
        
        while (basicColor != prefabColor && Math.Abs(basicIntensity - prefabIntensity) > 0.01f)
        {
            basicColor = Color.Lerp(basicColor, prefabColor, time);
            light.color = basicColor;
            basicIntensity = Mathf.Lerp(basicIntensity, prefabIntensity, time);
            light.intensity = basicIntensity;
            basicstrength = Mathf.Lerp(basicstrength, prefabstrength, time);

            light.transform.rotation = new Quaternion(Mathf.Lerp(basicRotation.x, prefabRotation.x, time),
                Mathf.Lerp(basicRotation.y, prefabRotation.y, time),
                Mathf.Lerp(basicRotation.z, prefabRotation.z, time),
                Mathf.Lerp(basicRotation.w, prefabRotation.w, time));
            
            time += Time.deltaTime * speedSkyLight;
            if (time > 1f)
            {
                time = 1f;
                basicColor = prefabColor;
                basicIntensity = prefabIntensity;
                basicstrength = prefabstrength;
                light.transform.rotation = lightsPrefab[index].transform.rotation;
            }
            yield return null;
        }
        
        //light.transform.rotation = lightsPrefab[index].transform.rotation;
    }

    void Start()
    {
        //StartCoroutine(ModifySky());
        
    }
}