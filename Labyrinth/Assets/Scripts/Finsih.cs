using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finsih : MonoBehaviour 
{
    Text startText;

    void Start()    
    {
        startText = GameObject.Find("GameStartsText").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.name == "Player")
        {
            Destroy(this.gameObject);
            Time.timeScale = 0;
            startText.text = "You win!";
        }
        
    }
}
