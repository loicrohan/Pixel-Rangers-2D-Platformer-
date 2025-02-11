using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enumeration for different background types
public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

// Class that handles animated background behavior
public class AnimatedBackground : MonoBehaviour
{
    // Serialized field to define the direction and speed of background movement
    [SerializeField] private Vector2 movementDirection;

    // Reference to the MeshRenderer component
    private MeshRenderer mesh;

    // Grouping fields under a header "Color" in the Unity Inspector
    [Header("Color")]

    // Serialized field to select the background type from the enum
    [SerializeField] private BackgroundType backgroundType;

    // Array of textures for different background types
    [SerializeField] private Texture2D[] textures;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the MeshRenderer component attached to the GameObject
        mesh = GetComponent<MeshRenderer>();

        // Update the texture of the background based on the selected type
        UpdateBackgroundTexture();
    }

    // Update is called once per frame
    private void Update()
    {
        // Animate the texture offset to create a scrolling background effect
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    // Context menu to manually update the background texture in the Unity Editor
    [ContextMenu("Update background")]
    private void UpdateBackgroundTexture()
    {
        // Ensure the mesh reference is assigned
        if (mesh == null)
            mesh = GetComponent<MeshRenderer>();

        // Set the texture of the material based on the selected background type
        mesh.sharedMaterial.mainTexture = textures[((int)backgroundType)];
    }
}