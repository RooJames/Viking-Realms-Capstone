using UnityEngine;
using UnityEngine.UI;

public class AnimatedButton : MonoBehaviour
{
    public Sprite unpressedSprite;
    public Sprite pressedSprite;
    public float delayBeforeAnimation;
    public float unpressedDuration;
    public float pressedDuration;

    private Image buttonImage;
    private float timer;
    private float cycleTimer;
    private bool isAnimating = false;
    private bool isPressed = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null && unpressedSprite != null)
        {
            buttonImage.sprite = unpressedSprite;
        }
        timer = 0f;
    }

    void OnEnable()
    {
        // Reset when UI becomes active
        timer = 0f;
        isAnimating = false;
        isPressed = false;
        if (buttonImage != null && unpressedSprite != null)
        {
            buttonImage.sprite = unpressedSprite;
        }
    }

    void Update()
    {
        if (!isAnimating)
        {
            // Wait for delay
            timer += Time.deltaTime;
            if (timer >= delayBeforeAnimation)
            {
                isAnimating = true;
                cycleTimer = 0f;
            }
        }
        else
        {
            // Animate between sprites
            cycleTimer += Time.deltaTime;

            if (!isPressed)
            {
                // In unpressed state
                if (cycleTimer >= unpressedDuration)
                {
                    buttonImage.sprite = pressedSprite;
                    isPressed = true;
                    cycleTimer = 0f;
                }
            }
            else
            {
                // In pressed state
                if (cycleTimer >= pressedDuration)
                {
                    buttonImage.sprite = unpressedSprite;
                    isPressed = false;
                    cycleTimer = 0f;
                }
            }
        }
    }
}