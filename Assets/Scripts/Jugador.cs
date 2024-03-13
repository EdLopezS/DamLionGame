using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float mH = 0f;
    [SerializeField] private float velocidadMovimiento;
    [Range (0,0.3f)][SerializeField] private float suavizadorDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto = false;

    [Header("Animacion")]
    private Animator animator;

    [Header("Escalar")]
    private Vector2 input;
    [SerializeField] private float velocidadEscalar;
    private CapsuleCollider2D boxCollider2D;
    private float gravedadInicial;
    private bool escalando;


    private void Start(){
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<CapsuleCollider2D>();
        gravedadInicial = rb2D.gravityScale;
    }
    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        mH = Input.GetAxisRaw("Horizontal") * velocidadMovimiento;
        animator.SetFloat("Horizontal", Mathf.Abs(mH));
        if(Input.GetButtonDown("Jump")){
            salto = true;
        }
    }
    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        animator.SetBool("enSuelo",enSuelo);
        Mover(mH * Time.fixedDeltaTime, salto);
        Escalar();
        salto = false;
    }
    private void Mover(float mover, bool saltar)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadorDeMovimiento);

        if (mover > 0 &&!mirandoDerecha)
        {
            Flip();
        }else if (mover < 0 && mirandoDerecha)
        {
            Flip();
        }

        if (enSuelo && saltar)
        {
            enSuelo = false;
            rb2D.AddForce(new Vector2(0f, fuerzaSalto));
        }
    }
    private void Escalar()
    {
        if((input.y != 0 || escalando) && (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Escaleras"))))
        {
            Vector2 velocidadSubida = new Vector2(rb2D.velocity.x, input.y * velocidadEscalar);
            rb2D.velocity = velocidadSubida;
            rb2D.gravityScale = 0;
            escalando = true;
        }else{
            rb2D.gravityScale = gravedadInicial;
            escalando = false;
        }

        if(enSuelo)
        {
            escalando = false;
        }
    }
    private void Flip()
    {
        mirandoDerecha =!mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}
