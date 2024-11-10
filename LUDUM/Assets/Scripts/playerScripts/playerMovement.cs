using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float maxStamina = 5.0f;
    public float staminaDrainRate = 1.0f;
    public Image staminaPanel;

    public AudioClip[] footstepSounds;
    public AudioClip staminaOffSound;
    private AudioSource audioSource;
    private float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    private CharacterController controller;
    private float currentStamina;
    private bool isExhausted = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = maxStamina;
        staminaPanel.color = new Color(staminaPanel.color.r, staminaPanel.color.g, staminaPanel.color.b, 0);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection.y -= 9.81f * Time.deltaTime;

        bool isMoving = moveDirection.x != 0 || moveDirection.z != 0;

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentStamina > 0 && !isExhausted && isMoving)
        {
            controller.Move(moveDirection * runSpeed * Time.deltaTime);
            currentStamina -= staminaDrainRate * Time.deltaTime;
            footstepInterval = 0.25f;
            PlayFootstepSound();
            if (currentStamina <= 0)
            {
                isExhausted = true;
                StartCoroutine(ShowExhaustionPanel());
            }
        }
        else if (isMoving)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            footstepInterval = 0.5f;
            PlayFootstepSound();
        }
    }

    private void PlayFootstepSound()
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepInterval)
        {
            footstepTimer = 0f;
            if (footstepSounds.Length > 0)
            {
                int index = Random.Range(0, footstepSounds.Length);
                audioSource.PlayOneShot(footstepSounds[index]);
            }
        }
    }

    private IEnumerator ShowExhaustionPanel()
    {
        if (staminaOffSound != null)
        {
            audioSource.PlayOneShot(staminaOffSound);
        }

        float elapsedTime = 0f;
        float duration = 5f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.PingPong(elapsedTime * 2, 1);
            staminaPanel.color = new Color(staminaPanel.color.r, staminaPanel.color.g, staminaPanel.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        staminaPanel.color = new Color(staminaPanel.color.r, staminaPanel.color.g, staminaPanel.color.b, 0);
        StartCoroutine(RecoverStaminaAfterDelay());
    }

    private IEnumerator RecoverStaminaAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        currentStamina = maxStamina;
        isExhausted = false;
    }
}