using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //�����܂łɂ����鎞��
    private float timeTaken = 0.2f;
    //�o�ߎ���
    private float timeErapsed;
    //�ړI�n
    private Vector3 destination;
    //�o���n
    private Vector3 origin;
    public void MoveTo(Vector3 newDestination)
    {
        //�o�ߎ��Ԃ�������
        timeErapsed = 0;
        //�ړ����̉\��������̂ŁA���ݒn��position�ɑO��ړ��̖ړI�n����
        origin = destination;
        transform.position = origin;
        //�V�����ړI�n����
        destination = newDestination;
    }
    // Start is called before the first frame update
    void Start()
    {
        //�ړI�n�E�o���n�����ݒn�ŏ�����
        destination = transform.position;
        origin = destination;
    }

    // Update is called once per frame
    void Update()
    {
        if (origin == destination) { return; }

        timeErapsed += Time.deltaTime;

        float timeRate = timeErapsed / timeTaken;

        if (timeRate > 1) { timeRate = 1; }

        float easing = 1 - Mathf.Pow(1 - timeRate, 5);

        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);

        transform.position = currentPosition;



    }
}
