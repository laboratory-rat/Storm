using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LoadingAnimation : MonoBehaviour
{
    public Sprite[] Sprites;
    public float Sleep = 0.1f;

    private Image _image;
    private int count;

    // Use this for initialization
    private void Start()
    {
        _image = GetComponent<Image>();
        count = Sprites.Length;

        StartCoroutine(AnimUpdate(Sleep));
    }

    private IEnumerator AnimUpdate(float sleep)
    {
        int index = 0;
        while (true)
        {
            _image.sprite = Sprites[index];
            yield return new WaitForFixedUpdate(); //WaitForSeconds(sleep);

            index++;

            if (index == count)
            {
                index = 0;
                //yield return new WaitForSeconds(sleep * 2);
            }
        }
    }
}