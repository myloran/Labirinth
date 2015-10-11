using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartTimer : MonoBehaviour {

    public Text startText;
	// Use this for initialization
	void Start () 
    {
        StartCoroutine(MessageOnGameStart());
	}

    IEnumerator MessageOnGameStart()
    {
        startText.text = "3";
        yield return new WaitForSeconds(1);
        startText.text = "2";                       
        yield return new WaitForSeconds(1);
        startText.text = "1";
        yield return new WaitForSeconds(0.2f);
        startText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        startText.text = "";
    }
}
