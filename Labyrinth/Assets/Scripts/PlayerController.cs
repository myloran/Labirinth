using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    public float speed = 100.0F;
    Rigidbody rb;
    public float v, h;
    bool gameStarted = false;
    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(WaitTillGameStarts());
    }

    IEnumerator WaitTillGameStarts()
    {
        yield return new WaitForSeconds(2.2f);
        gameStarted = true;
    }
    void Update()
    {
        if (gameStarted)
        {     
            v = Input.GetAxis("Vertical") * Time.deltaTime;
            h = Input.GetAxis("Horizontal") * Time.deltaTime;
            //if (Application.platform == RuntimePlatform.Android)
            {
                v = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime;
                h = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime;
            }
            rb.AddForce(new Vector3(h, 0, v) * speed);
        }
        
    }
}
