using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public string nextLevelName;
    bool activated = false;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (activated)
            return;

        if (collider2D.tag == "Player")
        {
            activated = true;
            StartCoroutine(LoadLevel());
        }
    }
    
    IEnumerator LoadLevel()
    {
        FindObjectOfType<Player>().freeMovements = true;
        GUIManager.Instance.LoadNextLevel();

        yield return new WaitForSeconds(1);
        GameManager.Instance.NextLevel();
    }
}
