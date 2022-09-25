using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedCredits : MonoBehaviour
{
    public Text txtCredits;
    public float speed = 0.5f;
    public int yInitPosition = -400;
    public int yLimitPosition = 3750;

    // Start is called before the first frame update
    void Start()
    {
        txtCredits.transform.position = new Vector3(txtCredits.transform.position.x,
                                                        yInitPosition,
                                                        txtCredits.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(txtCredits.transform.position.y < yLimitPosition)
        {
            txtCredits.transform.position = new Vector3(txtCredits.transform.position.x, 
                                                        txtCredits.transform.position.y + speed, 
                                                        txtCredits.transform.position.z);
        }
        else
        {
            txtCredits.transform.position = new Vector3(txtCredits.transform.position.x,
                                                        yInitPosition,
                                                        txtCredits.transform.position.z);
        }
    }
}
