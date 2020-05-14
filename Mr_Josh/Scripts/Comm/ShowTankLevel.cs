using UnityEngine;
using UnityEngine.UI;

public class ShowTankLevel : MonoBehaviour
{
    Text Water_level;
    // Start is called before the first frame update
    void Start()
    {
        Water_level = GetComponent<Text>();
    }

    // Update is called once per frame
    public void TextUpdate(float Value)
    {
        Water_level.text = Mathf.RoundToInt(Value).ToString();
    }
}
