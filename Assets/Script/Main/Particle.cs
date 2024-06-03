using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float lifeTime;
    private float leftLifeTime;
    private Vector3 velocity;
    private Vector3 defaultScale;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 0.3f;
        leftLifeTime = lifeTime;
        defaultScale = transform.localScale;
        float maxVelocity = 5;
        velocity = new Vector3(
            Random.Range(-maxVelocity, maxVelocity),
            Random.Range(-maxVelocity, maxVelocity),
            0
          );


    }

    // Update is called once per frame
    void Update()
    {
        //���̃^�C�}�[��1�i�߂�
        leftLifeTime-=Time.deltaTime;
        //�����_���Ɉړ�����
        transform.position += velocity * Time.deltaTime;
       //�k������
        transform.localScale = Vector3.Lerp
            (
            new Vector3(0, 0, 0),
            defaultScale,
            leftLifeTime / lifeTime
            );
        //���ԂɒB�����玀
        if(leftLifeTime <= 0) { Destroy(gameObject); }
    }
}
