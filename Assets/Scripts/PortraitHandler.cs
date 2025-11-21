using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortraitHandler : MonoBehaviour
{
    public Fighter fighter;
    public TMP_Text hpText;

    private void Update()
    {
        if (fighter && hpText)
        {
            hpText.text = fighter.health.ToString() + "/" + fighter.maxHealth.ToString();
        }
    }
}
