using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject cursorObject;
    public GameObject pauseIcon;
    public Scrollbar mouseSensitivityScrollbar;
    public Scrollbar volumeScrollbar;
    public CameraController cameraController;
    public GameObject taskObject;

    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    private bool isPaused = false;
    private float originalMouseSensitivity = 2.0f;
    private float originalVolume = 1.0f;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        cursorObject.SetActive(true);
        pauseIcon.SetActive(true);

        audioSource = gameObject.AddComponent<AudioSource>();

        float savedSensitivity = SettingsManager.LoadMouseSensitivity(originalMouseSensitivity);
        float savedVolume = SettingsManager.LoadVolume(originalVolume);

        mouseSensitivityScrollbar.value = savedSensitivity / 10.0f;
        volumeScrollbar.value = savedVolume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused)
        {
            UpdateMouseSensitivity();
            UpdateVolume();
        }
    }

    void PauseGame()
    {
        taskObject.SetActive(false);
        pauseMenuUI.SetActive(true);
        cursorObject.SetActive(false);
        pauseIcon.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;

        if (openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        if (cameraController != null)
        {
            cameraController.enabled = false;
        }
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        taskObject.SetActive(true);
        cursorObject.SetActive(true);
        pauseIcon.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;

        if (closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }

        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        float currentSensitivity = mouseSensitivityScrollbar.value * 10.0f;
        float currentVolume = volumeScrollbar.value;
        SettingsManager.SaveSettings(currentSensitivity, currentVolume);
    }

    void UpdateMouseSensitivity()
    {
        if (cameraController != null)
        {
            float sensitivity = mouseSensitivityScrollbar.value * 10.0f;
            cameraController.SetSensitivity(sensitivity);
        }
    }

    void UpdateVolume()
    {
        float volume = volumeScrollbar.value;
        AudioListener.volume = volume;
    }
}