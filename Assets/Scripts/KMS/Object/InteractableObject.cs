using UnityEngine;
using System.Collections;
using System;

public class InteractableObject : MonoBehaviour
{
    public float maxDurability;
    public float currentDurability;

    public Transform mountPoint;
    public ObjectSpecificData objectData;

    public IMovement movementController;
    public IInputHandler inputHandler;
    public RGTHpBar hpBar;
    
    // HP ������Ʈ�� ���� ��������Ʈ ����
    public delegate void OnHPUpdateDelegate();
    public OnHPUpdateDelegate onHPUpdate;

    public event Action OnDestroyCalled;
    public event Action OnHpBarTr;

    public virtual void Awake()
    {
        Init();
        currentDurability = objectData.durability;
        maxDurability = objectData.durability;
        
        if (hpBar != null)
        {
            hpBar.UpdateHpBar(maxDurability, maxDurability);
        }
    }

    private void Update()
    {
        if(onHPUpdate != null && OnDestroyCalled != null)
        {
            UpdateHpBarTr();
        }
    }

    public void Init()
    {
        movementController = GetComponent<IMovement>();
        inputHandler = GetComponent<IInputHandler>();
        hpBar = FindFirstObjectByType<RGTHpBar>();
        if(mountPoint == null)
        {
            mountPoint = transform;
        }
    }

    // �ܺο��� ȣ���ϸ� �ڷ�ƾ�� ���� HP�� �ε巴�� ���ҽ�ŵ�ϴ�.
    public void StartHpDecrease()
    {
        // Debug.Log("ü�� �Լ� �����");
        // currentDurability -= 1f;
        // hpBar.UpdateHpBar(currentDurability, maxDurability);
        StartCoroutine(DecreaseHpCoroutine());
    }

    // 1�� ���� 5��ŭ HP�� �ε巴�� ���ҽ�Ű�� �ڷ�ƾ
    private IEnumerator DecreaseHpCoroutine()
    {
        // // ������ �� �� �ڷ�ƾ ���� �ð� (1��)
        // float decreaseAmount = 5f;
        // float duration = 1f;
        // float elapsed = 0f;
        
        // // ���� HP�� ���� HP �� ���
        // float startHP = currentDurability;
        // float targetHP = Mathf.Max(currentDurability - decreaseAmount, 0f);

        // // 1�� ���� �� �����Ӹ��� ���� ����(Lerp)�� HP ����
        // while (elapsed < duration)
        // {
        //     elapsed += Time.deltaTime;
        //     currentDurability = Mathf.Lerp(startHP, targetHP, elapsed / duration);
        //     hpBar.UpdateHpBar(currentDurability, maxDurability);
        //     yield return null;
        // }

        // // �ڷ�ƾ ���� �� Ȯ���� targetHP�� ����
        // currentDurability = targetHP;
        while(currentDurability > 0){
        Debug.Log("ü�� �Լ� �����");

        currentDurability -= 1f;

        Debug.Log("���� ü��" + currentDurability);

        hpBar.UpdateHpBar(maxDurability, currentDurability);

        yield return new WaitForSeconds(1f);
        }
        
        if (currentDurability <= 0)
        {
            DestroyObject();
        }
    }

    // hp�� ��ġ�� ������Ʈ �ϴ� �Լ�
    public void UpdateHpBarTr()
    {   
        hpBar.UpdatePosition(transform);

        // �̺�Ʈ�� ��ϵǾ��ִ°� �ִٸ� ����
        OnHpBarTr?.Invoke();
    }

    // ü���� 0�� �Ǹ� �ı��Ǵ� �Լ� ���� �� ������Ʈ ����
    public void DestroyObject()
    {
        Debug.Log("������Ʈ �ı���");

        OnDestroyCalled?.Invoke();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // ��������Ʈ ����
        onHPUpdate = null;
    }
}