using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//���̍s��ǋL

public class Reset : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //����if����ǋL
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(0);
        }
    }
}
