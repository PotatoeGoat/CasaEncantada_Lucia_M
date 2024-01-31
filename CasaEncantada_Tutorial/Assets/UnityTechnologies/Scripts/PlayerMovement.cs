using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_Movement;

    Quaternion m_Rotation = Quaternion.identity;

    Animator m_Animator;

    Rigidbody m_Rigidbody;

    AudioSource m_AudioSource;

    public float turnSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        //accedo al componente Animator para crear la referencia a este componente y que otras funciones puedan acceder a él
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //movimiento del personaje
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //declaro el valor del Vector3 y normalizo la diagonal para que vaya a la misma velocidad en todas las direcciones
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        //DETECTAR SI HAY ENTRADAS DEL TECLADO PARA ASÍ SABER SI SE TIENE QUE MOVER O NO EN LA ANIMACIÓN

        //regresa una variable de tipo bool: VERDADERA si los dos parámetros de la función Aproximately (Mathf es la clase) son aproximadamente iguales; o FALSO si no lo son.
        //En este caso será FALSE si "horizontal" se aproxima a 0 ya que contiene el operador lógico ! (negación) delante de la sentencia, así que devuelve los valores contrarios.
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); //si se está pulsando una tecla de input horizontal, entonces el valor se aleja de 0, y es igual a TRUE
        //hago lo mismo con el eje vertical
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f); //si se está pulsando una tecla de input vertical, entonces el valor se aleja de 0, y es igual a TRUE

        //hago que si uno de los dos, o los dos, son TRUE (osea, tienen alguna entrada de input de las teclas), entonces IsWalking también es TRUE
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        //le asigno el valor que devuelve el bool "isWalking" al parámetro bool que creé en el Animator de unity "IsWalking" que contiene las animaciones del personaje.
        m_Animator.SetBool("IsWalking", isWalking);

        //audio de los pasos
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        //ROTAR AL PERSONAJE SEGUN QUE TECLA ESTEMOS PULSANDO

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        //crear y almacenar la rotación del personaje:
        //llama a la función LookRotation y crea una rotación mirando a la dirección que le da el parámetro dado, en este caso "desiredForward"
        m_Rotation = Quaternion.LookRotation(desiredForward);

        

    }

    //APLICAR EL MOVIMIENTO Y ROTACIÓN AL PERSONAJE

    //declaramos una nueva función
    private void OnAnimatorMove()
    {
        //llamamos a la función MovePosition y le asignamos un nuevo valor a sus parámetros
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);



        //lo mismo, pero aplicado a la rotación, aunque esta vez no le aplicamos un cambio a los parámetros de la función; simplemente, estamos configurándola
        m_Rigidbody.MoveRotation(m_Rotation);


    }

    
}
