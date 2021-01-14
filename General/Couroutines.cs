using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Couroutines : Singleton<Couroutines>
{
    public void RunAfter(Action action, float time) {
        StartCoroutine(RunAfterTimeCouroutine(action, time));
    }

    private IEnumerator RunAfterTimeCouroutine(Action action, float time) 
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public void RunUntil(Action action, Func<bool> predicate) {
        StartCoroutine(RunUntilCouroutine(action, predicate));
    }

    private IEnumerator RunUntilCouroutine(Action action, Func<bool> predicate) 
    {
        while(!predicate()) {
            action();
            yield return null;
        }
    }
}