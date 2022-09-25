using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarContainer;
    public Image healthBarImage;
    public Player player;
    public float factor = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        healthBarContainer.transform.localScale = new Vector3(factor * (player.GetHealth() * 1.0f / player.GetMaxHealth()), 0.2f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBar()
    {
        healthBarImage.transform.localScale = new Vector3(factor * (player.GetHealth() * 1.0f / player.GetMaxHealth()), 0.2f, 1.0f);
    }
}
