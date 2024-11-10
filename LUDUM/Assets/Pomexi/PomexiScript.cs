using System.Collections;
using UnityEngine;

public class ToggleGameObjects : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float toggleInterval = 0.1f;

    private void Start()
    {
        StartCoroutine(ToggleObjects());
    }

    private IEnumerator ToggleObjects()
    {
        int index = 0;

        while (true)
        {
            gameObjects[index].SetActive(false);

            index = (index + 1) % gameObjects.Length;

            gameObjects[index].SetActive(true);

            yield return new WaitForSeconds(toggleInterval);
        }
    }
}