using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 movementDirection;
    private MeshRenderer mesh;

    [Header("Color")]
    [SerializeField] private BackgroundType backgroundType;
    [SerializeField] private Texture2D[] textures;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }

    private void Update()
    {
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    [ContextMenu("Update background")]
    private void UpdateBackgroundTexture()
    {
        if (mesh == null)
            mesh = GetComponent<MeshRenderer>();

        mesh.sharedMaterial.mainTexture = textures[((int)backgroundType)];
    }

}