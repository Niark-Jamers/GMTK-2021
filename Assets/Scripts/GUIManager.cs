using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Slider lifeBar;
    public GameObject pausePanel;
    public bool pause;

    public void SetLife(float lifeBetween01)
    {
        lifeBar.value = lifeBetween01;
    }


    public void Pause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        pause = !pause;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
