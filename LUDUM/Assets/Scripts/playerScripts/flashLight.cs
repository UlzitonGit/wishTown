using UnityEngine;
using TMPro;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject pointLight;
    public GameObject[] sheepObjects;
    public TMP_Text Instr;

    public AudioClip pointLightOnClip;
    public AudioClip pointLightOffClip;
    private AudioSource audioSource;

    private bool isFlashlightActive = false;
    private bool isPointLightActive = false;
    private bool isFirstTimeFlashlightOn = true;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        foreach (GameObject sheep in sheepObjects)
        {
            if (sheep.activeSelf)
            {
                if (isFlashlightActive)
                {
                    isFlashlightActive = false;
                    flashlight.SetActive(false);
                    isPointLightActive = false;
                    pointLight.SetActive(false);
                }
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isFlashlightActive && Input.GetMouseButtonDown(0))
        {
            TogglePointLight();
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightActive = !isFlashlightActive;
        flashlight.SetActive(isFlashlightActive);

        if (isFlashlightActive && isFirstTimeFlashlightOn)
        {
            Instr.gameObject.SetActive(false);
            isFirstTimeFlashlightOn = false;
        }

        if (!isFlashlightActive)
        {
            isPointLightActive = false;
            pointLight.SetActive(false);
        }
    }

    void TogglePointLight()
    {
        isPointLightActive = !isPointLightActive;
        pointLight.SetActive(isPointLightActive);

        if (isPointLightActive)
        {
            audioSource.PlayOneShot(pointLightOnClip);
        }
        else
        {
            audioSource.PlayOneShot(pointLightOffClip);
        }
    }
}