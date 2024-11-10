using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VHSTapeEffectController : MonoBehaviour
{
    public Material VHSMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, VHSMaterial);
    }
}