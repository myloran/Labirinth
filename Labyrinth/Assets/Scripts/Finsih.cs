using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finsih : MonoBehaviour 
{
    GameEngine gameEngine;

    void Start()    
    {
        gameEngine = GameObject.Find("Game").GetComponent<GameEngine>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            gameEngine.KeyFounded();
            Destroy(this.gameObject);
        }
        
    }
}
