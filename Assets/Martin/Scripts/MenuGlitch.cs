using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGlitch : MonoBehaviour
{
    public Sprite[] titleImage = new Sprite[3];
    private Image title;
    public float hard, normal, randomMin, randomMax;
    public bool isGlitching;
    private void Start()
    {
        title = GetComponent<Image>();
    }
    private void Update()
    {
        
    }
    private void NormalTitle()
    {
        title.sprite = titleImage[0];
    }
    private void SoftGlitch()
    {
        title.sprite = titleImage[1];
    }
    private void HardGlitch()
    {
        title.sprite = titleImage[2];
    }
    void Timer()
    {
        isGlitching = false;
    }
    void Glitching()
    {
        SoftGlitch();
        Invoke("HardGlitch", hard);
        Invoke("NormalTitle", normal);
        isGlitching = true;
    }
    void RandomGlitching()
    {
        float timer = Random.Range(randomMin, randomMax);
        Invoke("Glitching", timer);
        Invoke("Timer", timer + 1);
    }
}
