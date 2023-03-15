using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0.5f;

    private Material skyboxMaterial;

    private void Awake()
    {
        skyboxMaterial = RenderSettings.skybox;
    }

    private void Update()
    {
        float rotation = skyboxMaterial.GetFloat("_Rotation");
        rotation = Mathf.Repeat(rotation + rotateSpeed * Time.deltaTime, 360.0f);
        skyboxMaterial.SetFloat("_Rotation", rotation);
    }

}
