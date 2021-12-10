using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBigFireBall : Bullet
{
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(ChargeTimer());
        StartCoroutine(Charge());
    }

    IEnumerator ChargeTimer()
    {
        yield return new WaitForSeconds(2.5f);
        isShoot = true;
    }

    IEnumerator Charge()
    {
        while (!isShoot)
        {
            angularPower += 0.2f;
            scaleValue += 0.03f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}