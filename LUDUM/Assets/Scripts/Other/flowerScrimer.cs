using UnityEngine;
using System.Collections; // Add this line to use IEnumerator

public class FlowerBehavior : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject eyes;
    public AudioSource flowerAudio;
    public float checkInterval = 10f;
    public float turnChance = 0.05f;
    public float fovThreshold = 10f;
    public Vector3 rotationOffset = new Vector3(0, 68.27f, 0);

    private Quaternion originalRotation;
    private bool isTurned = false;
    private Coroutine volumeCoroutine;

    void Start()
    {
        originalRotation = transform.rotation;
        eyes.SetActive(true);
        InvokeRepeating("TryTurnTowardsCamera", checkInterval, checkInterval);
    }

    void Update()
    {
        if (isTurned)
        {
            if (IsFlowerInCameraView())
            {
                ReturnToOriginalPosition();
            }
            else
            {
                FollowCamera();
            }
        }
    }

    void TryTurnTowardsCamera()
    {
        if (!IsFlowerInCameraView() && Random.value < turnChance)
        {
            TurnTowardsCamera();
        }
    }

    void TurnTowardsCamera()
    {
        isTurned = true;
        eyes.SetActive(false);
        if (volumeCoroutine != null)
        {
            StopCoroutine(volumeCoroutine);
        }
        volumeCoroutine = StartCoroutine(ChangeVolume(1f, 0.5f)); // Increase volume to 1 over 0.5 seconds
    }

    void FollowCamera()
    {
        Vector3 directionToCamera = playerCamera.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
        lookRotation *= Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
    }

    void ReturnToOriginalPosition()
    {
        transform.rotation = originalRotation;
        isTurned = false;
        eyes.SetActive(true);
        if (volumeCoroutine != null)
        {
            StopCoroutine(volumeCoroutine);
        }
        volumeCoroutine = StartCoroutine(ChangeVolume(0f, 0.5f)); // Decrease volume to 0 over 0.5 seconds
    }

    bool IsFlowerInCameraView()
    {
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        bool isInView = viewportPoint.z > 0 &&
                        viewportPoint.x > 0 && viewportPoint.x < 1 &&
                        viewportPoint.y > 0 && viewportPoint.y < 1;

        if (isInView)
        {
            Vector3 directionToFlower = transform.position - playerCamera.transform.position;
            float angle = Vector3.Angle(playerCamera.transform.forward, directionToFlower);
            return angle < fovThreshold;
        }

        return false;
    }

    private IEnumerator ChangeVolume(float targetVolume, float duration)
    {
        float startVolume = flowerAudio.volume;
        float time = 0;

        if (targetVolume > 0 && !flowerAudio.isPlaying)
        {
            flowerAudio.Play();
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            flowerAudio.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        flowerAudio.volume = targetVolume;

        if (targetVolume == 0)
        {
            flowerAudio.Stop();
        }
    }
}