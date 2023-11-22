using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スカイボックスを回転させる
/// </summary>
public class RotateSkybox : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0.5f;

    private Material skyboxMaterial;

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        skyboxMaterial = RenderSettings.skybox;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // UVアニメでスカイボックスを回す
        float rotation = skyboxMaterial.GetFloat("_Rotation");
        rotation = Mathf.Repeat(rotation + rotateSpeed * Time.deltaTime, 360.0f);
        skyboxMaterial.SetFloat("_Rotation", rotation);
    }

}
