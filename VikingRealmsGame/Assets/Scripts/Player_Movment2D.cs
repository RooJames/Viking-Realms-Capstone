using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //varibles for movemnt and mapping
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
   
    void Fixedpdate() {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        rb.linearVelocity = new UnityEngine.Vector2(hori, vert) * moveSpeed;
    
    }


}
