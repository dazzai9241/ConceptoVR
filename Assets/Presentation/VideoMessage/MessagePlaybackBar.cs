using UnityEngine;
using UnityEngine.UI;


public class MessagePlaybackBar : MonoBehaviour
{
    [SerializeField]
    private Slider m_Slider;

    [SerializeField]
    private TMPro.TMP_Text startText;

    [SerializeField]
    private TMPro.TMP_Text endText;


    public void StartPlayback(float start, float end, float current = 0.0f)
    {

    }
}
