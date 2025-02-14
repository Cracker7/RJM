using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RGTHpBar : MonoBehaviour
{
    [SerializeField] private RectTransform yellowRectTr = null;
    [SerializeField] private RectTransform redImgTr = null;
    [SerializeField] private RectTransform transparentTr = null;

    private float maxWidth = 100f;
    private float maxHeight = 100f;

    //�÷��̾� ��ũ��Ʈ�� �־�� �� -> ��ġ ������Ʈ
    //    private void Update()
    //{
    //    if (hpBar != null)
    //        hpBar.UpdatePosition(transform.position);
    //}


    private void Awake()
    {
        maxWidth = yellowRectTr.sizeDelta.x;
        maxHeight = yellowRectTr.sizeDelta.y;

        // ��Ŀ�� �ǹ��� �������� ����
        yellowRectTr.pivot = new Vector2(0f, 0.5f);  // ���� �߾� ����
        yellowRectTr.anchorMin = new Vector2(0f, 0.5f);  // ���� ����
        yellowRectTr.anchorMax = new Vector2(0f, 0.5f);  // ���� ����

        // redImg�� ���� ������� ����
        redImgTr.pivot = new Vector2(0f, 0.5f);  // ���� �߾� ����
        redImgTr.anchorMin = new Vector2(0f, 0.5f);  // ���� ����
        redImgTr.anchorMax = new Vector2(0f, 0.5f);

        transparentTr.pivot = new Vector2(0f, 0.5f);  // ���� �߾� ����
        transparentTr.anchorMin = new Vector2(0f, 0.5f);  // ���� ����
        transparentTr.anchorMax = new Vector2(0f, 0.5f);  // ���� ����
    }


    public void UpdateHpBar(float _maxHp, float _curHp)
    {
        UpdateHpBar(_curHp / _maxHp);
    }

    public void UpdateHpBar(float _amount)
    {
        float prevWidth = yellowRectTr.sizeDelta.x;
        float newWidth = maxWidth * _amount;

        StopAllCoroutines();
        if (newWidth < prevWidth)
        {
            StartCoroutine(UpdateHpBarCoroutine(prevWidth, newWidth));
        }
        else
        {
            yellowRectTr.sizeDelta = new Vector2(newWidth, maxHeight);
        }

        redImgTr.sizeDelta = new Vector2(newWidth, maxHeight);
    }

    private IEnumerator UpdateHpBarCoroutine(float _prevWidth, float _newWidth)
    {
        Vector2 size = new Vector2(_prevWidth, maxHeight);
        yellowRectTr.sizeDelta = size;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            size.x = Mathf.Lerp(_prevWidth, _newWidth, t);
            yellowRectTr.sizeDelta = size;
            yield return null;
        }
    }

    public void UpdatePosition(Transform tr)
    {
        //Vector3 worldToScreen = Camera.main.WorldToScreenPoint(_pos);
        //worldToScreen.y += 50f;

        transform.position = tr.position + new Vector3(0f, 5f, 0f);
        transform.rotation = tr.rotation;
    }


}


//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class RGTHpBar : MonoBehaviour
//{
//    [SerializeField] private RectTransform yellowRectTr = null;
//    [SerializeField] private Image redImg = null;
//    [SerializeField] private Image transparent = null;

//    private float maxWidth = 0f;
//    private float maxHeight = 0f;

//    //�÷��̾� ��ũ��Ʈ�� �־�� �� -> ��ġ ������Ʈ
//    //    private void Update()
//    //{
//    //    if (hpBar != null)
//    //        hpBar.UpdatePosition(transform.position);
//    //}


//    private void Awake()
//    {
//        maxWidth = yellowRectTr.sizeDelta.x;
//        maxHeight = yellowRectTr.sizeDelta.y;

//        // ��Ŀ�� �ǹ��� �������� ����
//        yellowRectTr.pivot = new Vector2(0f, 0.5f);  // ���� �߾� ����
//        yellowRectTr.anchorMin = new Vector2(0f, 0.5f);  // ���� ����
//        yellowRectTr.anchorMax = new Vector2(0f, 0.5f);  // ���� ����

//        // redImg�� ���� ������� ����
//        redImg.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);  // ���� �߾� ����
//        redImg.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);  // ���� ����
//        redImg.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);  // ���� ����

//        transparent.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);  // ���� �߾� ����
//        transparent.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);  // ���� ����
//        transparent.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);  // ���� ����
//    }


//    public void UpdateHpBar(float _maxHp, float _curHp)
//    {
//        UpdateHpBar(_curHp / _maxHp);
//    }

//    public void UpdateHpBar(float _amount)
//    {
//        float prevWidth = yellowRectTr.sizeDelta.x;
//        float newWidth = maxWidth * _amount;

//        StopAllCoroutines();
//        if (newWidth < prevWidth)
//        {
//            StartCoroutine(UpdateHpBarCoroutine(prevWidth, newWidth));
//        }
//        else
//        {
//            yellowRectTr.sizeDelta = new Vector2(newWidth, maxHeight);
//        }

//        redImg.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, maxHeight);
//    }

//    private IEnumerator UpdateHpBarCoroutine(float _prevWidth, float _newWidth)
//    {
//        Vector2 size = new Vector2(_prevWidth, maxHeight);
//        yellowRectTr.sizeDelta = size;

//        float t = 0f;
//        while (t < 1f)
//        {
//            t += Time.deltaTime;
//            size.x = Mathf.Lerp(_prevWidth, _newWidth, t);
//            yellowRectTr.sizeDelta = size;
//            yield return null;
//        }
//    }

//    public void UpdatePosition(Vector3 _pos)
//    {
//        //Vector3 worldToScreen = Camera.main.WorldToScreenPoint(_pos);
//        Vector3 PlayerPos = _pos;
//        PlayerPos.y += 5f;
//        transform.position = PlayerPos;
//    }


//}
