using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseHoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private float duration = 0.7f;
    private float targetScaleUp = 1.2f;
    private float targetScaleDown = 1f;
    private Animator animator;
    public static bool onHover = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover = true;
        StartCoroutine(ScaleUp());
        animator = GetComponentInChildren<Animator>();

        if(gameObject.name == "BookBtn")
        {
            animator.SetBool("isHover", true);

        }

        else if(gameObject.name == "ExitBtn")
        {
            animator.SetBool("isHover", true);
        }

        

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHover = false;
        StartCoroutine(ScaleDown());

        animator = GetComponentInChildren<Animator>();

        if (gameObject.name == "BookBtn")
        {
            animator.SetBool("isHover", false);

        }

        else if(gameObject.name == "ExitBtn")
        {
            animator.SetBool("isHover", false);

        }



    }

    IEnumerator ScaleUp()
    {
        float startScale = transform.localScale.x;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float currentScale = Mathf.Lerp(startScale, targetScaleUp, elapsed / duration);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(targetScaleUp, targetScaleUp, targetScaleUp);

    }

    IEnumerator ScaleDown()
    {
        float startScale = transform.localScale.x;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float currentScale = Mathf.Lerp(startScale, targetScaleDown, elapsed / duration);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(targetScaleDown, targetScaleDown, targetScaleDown);
    }
}