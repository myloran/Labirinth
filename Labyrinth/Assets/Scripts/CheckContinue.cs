using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckContinue : MonoBehaviour {
    public GameObject campaignButton;
	// Use this for initialization
	void Start () {
        if (Data.lastGame != null)
        {
            (campaignButton.GetComponent("Image") as Image).color = Color.white;
            campaignButton.GetComponent<Button>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
