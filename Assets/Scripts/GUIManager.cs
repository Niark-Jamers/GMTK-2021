using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Slider lifeBar;

    public void SetLife(float lifeBetween01)
    {
        lifeBar.value = lifeBetween01;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
