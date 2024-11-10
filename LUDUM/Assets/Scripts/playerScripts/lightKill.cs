using UnityEngine;

public class CubeVisibilityController : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject pointlight;
    public GameObject[] cubes;
    public Camera playerCamera;
    public float maxDistance = 10f;
    public AudioClip[] deathSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        Debug.Log("AudioSource added");
    }

    void Update()
    {
        if (flashlight.activeSelf && pointlight.activeSelf)
        {
            Debug.Log("Both lights are active");
            for (int i = 0; i < cubes.Length; i++)
            {
                GameObject cube = cubes[i];
                if (IsCubeInSight(cube))
                {
                    if (i < deathSounds.Length && deathSounds[i] != null)
                    {
                        audioSource.PlayOneShot(deathSounds[i]);
                    }

                    Debug.Log("Deactivating cube: " + cube.name);
                    cube.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("One or both lights are inactive");
        }
    }

    bool IsCubeInSight(GameObject cube)
    {
        Vector3 directionToCube = cube.transform.position - playerCamera.transform.position;
        float distanceToCube = directionToCube.magnitude;

        if (distanceToCube > maxDistance)
        {
            Debug.Log("Cube is too far: " + cube.name);
            return false;
        }

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(cube.transform.position);
        bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;

        if (isInView)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, directionToCube.normalized, out hit, maxDistance))
            {
                if (hit.collider.gameObject == cube)
                {
                    Debug.Log("Cube is in sight: " + cube.name);
                    return true;
                }
                else
                {
                    Debug.Log("Raycast hit something else: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything");
            }
        }
        else
        {
            Debug.Log("Cube is not in view: " + cube.name);
        }

        return false;
    }
}