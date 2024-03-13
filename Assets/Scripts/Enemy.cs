using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
   [Header("Movimiento")]
   [SerializeField] private float velocidad;
   [SerializeField] private Transform controladorSuelo;
   [SerializeField] private float distancia;
   [SerializeField] private bool mD;

   private Rigidbody2D rb2D;

   [Header("Animacion")]
   private Animator animator;

   private void Start()
   {
       rb2D = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
   }

   private void FixedUpdate()
   {
    RaycastHit2D iS = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distancia);
    animator.SetFloat("Horizontal", Mathf.Abs(velocidad));
    rb2D.velocity = new Vector2(velocidad, rb2D.velocity.y);
    if(iS == false)
    {
        Girar();
    }
   }

   private void Girar()
   {
    mD = !mD;
    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    velocidad *= -1;
   }
}
