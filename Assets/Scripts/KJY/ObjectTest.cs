using UnityEngine;

public class ObjectTest : MonoBehaviour
{

    [SerializeField] private GameObject obj;
    //[SerializeField] private RGTADKey ADCon;
    //[SerializeField] private RGTADKeyObjection ADObjection;
    //[SerializeField] private RGTArrowKey Arrow;
    //[SerializeField] private RGTUIKey UI;
    [SerializeField] private RGTMouseV2 mouseV2;

    private void Update()
    {
        //������ �ɾ�� �� ���࿡ Ȱ��ȭ �Ǿ� ������??
        //ADCon.ADController(obj);
        //ADObjection.ADObjection(obj);
        //Arrow.ArrowKey(obj);
        //UI.UIKey(obj);
        mouseV2.FallowThePos(obj);
    }



}
