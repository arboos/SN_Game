using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HonestReactions : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float animLength;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float shakeLengthCoef;

    public void PlayNeutral()
    {
        animator.SetTrigger("Neutral");
    }
	public void PlayAngry()
    {
        animator.SetTrigger("Angry");
    }

    public void PlayHappy()
    {
        animator.SetTrigger("Happy");
    }

    public void Shake(float damage)
    {
        StartCoroutine(CharacterShake(damage));
    }

    public IEnumerator CharacterShake(float damage)
    {
        float length = shakeLengthCoef * damage;
        float timespan = 0;
        Vector2 startPosition = transform.position;
        while (timespan < length)
        {
            transform.position = startPosition + Random.insideUnitCircle * shakeStrength;
            timespan += Time.deltaTime;
            yield return null;
        }
    }
}
