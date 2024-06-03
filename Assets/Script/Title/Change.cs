using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image _PanelImage;
    [SerializeField] private float _speed;

    private bool isSceneChange;
    private Color PanelColor;

    private void Awake()
    {
        isSceneChange = false;
        PanelColor = _PanelImage.color;
    }
    public void blackout()
    {
        StartCoroutine(Sceneblackout());
    }
    private IEnumerator Sceneblackout()
    {
        while (!isSceneChange)
        {
            PanelColor.a += 0.1f;
            _PanelImage.color = PanelColor;
            if (PanelColor.a >= 1)
                isSceneChange = true;
            yield return new WaitForSeconds(_speed);
        }
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            blackout();
        }
    }
}
