using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    //1er CAMBIO: Creo una variable tipo float para hacer un timer que determine si han pasado 2 segundos o no
    public float timeToBeCaught;

    //3er CAMBIO: al haber hecho el signo de exclamación con una imagen, necesito referenciarla para poder activarla y desactivarla
    public GameObject caughtAlert;

    private void Start()
    {
        caughtAlert.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            caughtAlert.SetActive(false);
        }
    }

    void Update()
    {

       

        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);

            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    Debug.Log("passed " + timeToBeCaught);

                    gameEnding.CaughtAlert();
                    caughtAlert.SetActive(true);

                    timeToBeCaught += Time.deltaTime;
                    if (timeToBeCaught >= 2)
                    {
                        gameEnding.CaughtPlayer();
                        Debug.Log("2 sec");
                        
                    }
                    
                }
                


            }
            
        }
    }
}
