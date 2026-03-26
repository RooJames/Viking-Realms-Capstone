using UnityEngine;

public class FogDrift : MonoBehaviour
{
    public Transform fogA;
    public Transform fogB;

    public float speed = 0.2f;      // small number = slow cinematic fog
    public bool moveLeft = true;

    private float fogWidth;

    void Start()
    {
        // Measure the sprite width in WORLD units (works even if you scale it)
        SpriteRenderer sr = fogA.GetComponent<SpriteRenderer>();
        fogWidth = sr.bounds.size.x;

        // Place fogB to the right of fogA
        fogB.position = fogA.position + new Vector3(fogWidth, 0f, 0f);
    }

    void Update()
    {
        float dir = moveLeft ? -1f : 1f;
        Vector3 delta = new Vector3(dir * speed * Time.deltaTime, 0f, 0f);

        fogA.position += delta;
        fogB.position += delta;

        // If fogA moved fully off-screen to the left, move it behind fogB
        if (moveLeft && fogA.position.x <= fogB.position.x - fogWidth)
        {
            fogA.position = fogB.position + new Vector3(fogWidth, 0f, 0f);
            Swap();
        }
        // If moving right, do the opposite
        else if (!moveLeft && fogA.position.x >= fogB.position.x + fogWidth)
        {
            fogA.position = fogB.position - new Vector3(fogWidth, 0f, 0f);
            Swap();
        }
    }

    void Swap()
    {
        // Swap references so we always recycle the one that went offscreen
        Transform temp = fogA;
        fogA = fogB;
        fogB = temp;
    }
}