using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class freqvalue : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;

    public TextMeshProUGUI valueText1;
    public TextMeshProUGUI valueText2;
    public TextMeshProUGUI valueText3;
    public TextMeshProUGUI valueText4;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        valueText1.text = slider1.value.ToString("F2"); 
        valueText2.text = slider2.value.ToString("F2");
        valueText3.text = slider3.value.ToString("F2");
        valueText4.text = slider4.value.ToString("F2");
    }
}
