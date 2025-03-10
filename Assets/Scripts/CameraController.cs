﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float shakeSpeed = 40f;
    public float shakeDistance = 0.7f;
    public float shakeDuration = 0.25f;
    public float tolerance = 0.3f;
    private Vector3 normalPos;
    private Vector3 newPosition;
    private float time = 0;

    private void Awake()
    {
        normalPos = transform.localPosition;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private IEnumerator ShakeCoroutine(float shakeSpeed, float shakeDistance, float shakeDuration, float tolerance, Vector3 normalPos)
    {
        yield return new WaitForSecondsRealtime(0.05f);
        while (time < shakeDuration)
        {
            if ((newPosition - transform.localPosition).sqrMagnitude <= tolerance * tolerance)
            {
                newPosition = normalPos + Random.insideUnitSphere * shakeDistance;
            }
            transform.localPosition = Vector3.Lerp(base.transform.localPosition, newPosition, Time.deltaTime * shakeSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ResetPosSmooth(normalPos));
        time = 0;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine(shakeSpeed, shakeDistance, shakeDuration, tolerance, normalPos));
    }

    private IEnumerator ResetPosSmooth(Vector3 normalPos)
    {
        yield return new WaitForSecondsRealtime(0.03f);
        while (Vector3.Distance(transform.transform.localPosition, normalPos) > 0.01f)
        {
            transform.transform.localPosition = Vector3.Lerp(transform.transform.localPosition, normalPos, Time.deltaTime * 20f);
            yield return null;
        }
        transform.localPosition = normalPos;
    }
}
