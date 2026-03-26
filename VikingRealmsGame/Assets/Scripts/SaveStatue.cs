using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SaveStatue : MonoBehaviour, IInteractable
{
    private SaveController saveController;
    public Sprite activatedSprite;
    public Light2D statueLight;
    public bool WasUsed { get; private set; }
    [SerializeField] private string statueID;
    public string StatueID => statueID;

    public float fadeSpeed = 1f; // Adjust fade speed
    private Coroutine lightCoroutine;

    void Start()
    {
        saveController = FindFirstObjectByType<SaveController>();

        if (string.IsNullOrEmpty(statueID))
        {
            statueID = GlobalHelper.GenerateUniqueID(gameObject);
        }

        if (statueLight != null)
        {
            statueLight.intensity = 0f;
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        SaveGame();

        if (lightCoroutine != null)
        {
            StopCoroutine(lightCoroutine);
        }
        lightCoroutine = StartCoroutine(FlashLight());
    }

    private IEnumerator FlashLight()
    {
        if (statueLight != null)
        {
            // Ramp up to 1
            while (statueLight.intensity < 3f)
            {
                statueLight.intensity += Time.deltaTime * fadeSpeed;
                yield return null;
            }
            statueLight.intensity = 3f;

            // Hold for 1 seconds
            yield return new WaitForSeconds(1f);

            // Ramp down to 0
            while (statueLight.intensity > 0f)
            {
                statueLight.intensity -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
            statueLight.intensity = 0f;
        }
    }

    private void SaveGame()
    {
        SetUsed(true);
        saveController.SaveGame();
        Debug.Log("Game Saved at Statue!");
    }

    public void SetUsed(bool used)
    {
        WasUsed = used;
        if (WasUsed && activatedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = activatedSprite;
        }
    }
}