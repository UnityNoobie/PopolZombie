using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipsUI : MonoBehaviour
{
    #region Constants and Fields
    Button m_play;
    Button m_control;
    Button m_item;
    Button m_ability;
    Button m_monster;
    Button m_return;
    TextMeshProUGUI m_infoText;
    RectTransform m_rectTransform;
    Scrollbar m_scrollbar;
    const string howToPlay = "\n<size=60>�÷��̹��</size>\n\n<size=40>���庰�� �������� �����κ��� �����ϼ���. \n������� ���� ���� ���� ������ �������ϴ�.</size>\n\n<size=60>���� ��</size>\n\n<size=40>���� �����̹����� ���� ���� �����մϴ�. \n�� �ð����� ������� ������ ���߰� �ǰ� ���� �� �ð��� �־����ϴ�. �� �ð��� �ִ��� Ȱ���Ͽ� ħ���� ����ϼ���.\n�ð��� ���� ���� �Ǹ� ������� ������ ���۵˴ϴ�.\n������� ��� óġ�Ͽ� ������ �������� ������ �̾����.</size>" +
        "\n\n<size=60>����</size>\n\n<size=40>���� óġ�ϸ� ���� ���� �� �ֽ��ϴ�.\n���� ��� ��ŸƮ ������ �ִ� �������� ����, ��, �������� ������ ������ Ȱ���ϼ���.\n������ �������� 10����, 20���忡 ���ŵ˴ϴ�.</size>\n\n<size=60>������</size>\n\n<size=40>���� ����ϸ� ����ġ�� ȹ���մϴ�.\n�̸� ���� �������� �� ��� ü�� ���� ȸ�� ȿ���� ���Ҿ� Ư�� ����Ʈ�� ȹ���ϰ� �˴ϴ�.\nȹ���� Ư������Ʈ�� Ư���� ��ȭ�Ͽ� ������ ������ ȿ���� ��������.</size>";
    const string howToControll = "\n\n<size=60>���۹�</size>\n\n<size=50>�̵� : W A S D Ȥ�� ����Ű\n\n���� : ���콺 Ŀ���� ��ġ\n\n���� : ���콺 ��Ŭ��\n\n������ : R\n\n�������ͽ�â : I\n\n��ųâ : K\n\n�޴� : ESC</size>";
    const string items = "\n\n<size=60>����</size>\n\n<size=40>���� �����̹����� 7������ ���Ⱑ �����մϴ�.\n�÷��̾�� �Ѱ��� ���⸦ �� �� �ְ� ���⸦ ���� �� ���� ����� ����ó�� �˴ϴ�.\n���� ������ Ư¡�� ������ �����ϴ� \n\n����\n\n�ſ� ���� �̵��ӵ�,���� ���ݼӵ�, ������ �ѹ� ������, ������ ġ��Ÿ �������� Ư¡�Դϴ�." +
        "\n\n�������\n\n������ ���Է� ���� �̵��ӵ�, �ſ� ���� ���ݼӵ��� Ư¡�Դϴ�.\n\n������\n\n���� ������� ����� ��� �κп� �־� ������� ����� �����ݴϴ�.\n\n�����\n\n���ſ� ���Է� �̵��ӵ��� �г�Ƽ�� ������ ���� ��ź��, �ſ���� ���ݼӵ��� Ư¡�Դϴ�.\n\n����\n\n�ٸ� �ѱ� ��� ª�� �����Ÿ��� ������ ������ �׸�ŭ ������ ������, ���� �������� ������ �ֽ��ϴ�.\n\n�߱���Ʈ\n\n���� ª�� �����Ÿ��� ������ ������ ��������, ������ ���� �ӵ��� ������ �־� �뷱���� �����ϴ�.\n\n����\n\nª�� �����Ÿ��� ������ ������ ��������, ���� �������� ������ �ֽ��ϴ�.  " +
        "</size>\n\n<size=60>��</size>\n\n<size=40>���� �⺻������ ������ �����ϰ� �������� �߰����� ȿ���� �����մϴ� �߰� ȿ���� ������ ������ �����ϴ�.\n��� : ġ��Ÿ Ȯ���� �����ݴϴ�.\n�尩 : ���ݼӵ��� �����ݴϴ�.\n���� : �߰� �ִ�ü���� �����մϴ�.\n���� : ���ݷ��� �����ݴϴ�.\n�Ź� : �̵��ӵ��� �����ݴϴ�.\n" +
        "</size>\n\n<size=60>�Ҹ�ǰ</size>\n\n<size=40>�Ҹ�ǰ�� ���޻���, �ٸ����̵�(�̱���), ��ž(�̱���), ����(�̱���) ���� ������ ������ ���� ȿ���� ������ �ֽ��ϴ�.\n���޻��� : �ִ�ü���� n%�� ȸ���մϴ�.\n�ٸ����̵� : ������ ���� ����� �Ǹ� ������ ��ŭ�� ���ظ� �����ݴϴ�.\n��ž : ��ó ������� �����մϴ�. ������ ���ݿ� �ı��� �� �ֽ��ϴ�.\n���� : ���񿡰� ����, ��ȭ ���� �ִ� ������ �ֽ��ϴ�\n</size>";
    const string abilitys = "\n\n<size=60>Ư���� ����</size>\n\n<size=40>���� �����̹��� Ư���� 3���� ������ �����մϴ�.\n������ Ư���� 1�ܰ�, 2�ܰ�, 3�ܰ�, ������ Ư������ ������ ���� ���� ������ �����Ǹ� ���� Ư���� ���� �� �ֽ��ϴ�.\nƯ���� ���� ������ ������ �����ϴ�.\n1�ܰ� Ư�� : ���Ǿ��� ���� �����մϴ�.(�Ҹ� ����Ʈ 1)\n2�ܰ� Ư�� : ���� ������ 1�ܰ� Ư�� 8���� ���� �� ���� �����մϴ�.(�Ҹ� ����Ʈ 1)\n3�ܰ� Ư�� : ���� ������ 2�ܰ� Ư�� 6�� ���� �� ���� �����մϴ�.\n���� �������� ��� Ưȭ�� �� �ֽ��ϴ�(�ѹ� �� �� ���� �Ұ���, ���� ����Ʈ 3)\n������ ��ų : 3�ܰ� Ưȭ ��ų ��� ���� �� ���� �����մϴ�.(�Ҹ� ����Ʈ 0)\n��ݼ� : ����, �������, ������ �� \"����ȭ��\"�� �з��Ǵ� ���⸦ ��ȭ�� �ݴϴ�.\n��ü : �÷��̾��� ������ ��ȭ, ȸ�� ���� Ư���� ����, �����, �������� �� \"��ȭ��\"�� �з��Ǵ� ���⸦ ��ȭ�� �ݴϴ�.\n��ƿ��Ƽ(�̱���) : �ٸ����̵�. ��ž, ���� ���� \"��ġ��\"�� ��ȭ�� �Ʊ��� ��ȭ���� �ݴϴ�.</size>\n\n" +
        "<size=60>��ݼ�</size>\n\n<size=40>����, �������, ������ �� \"����ȭ��\"�� �з��Ǵ� ���⸦ ��ȭ�� �ݴϴ�.\n3�ܰ� Ưȭ Ư���� ��ü���� ȿ���� ������ �����ϴ�.\n\n����\n\nġ��Ÿ Ȯ��, ġ��Ÿ ������, �˹� Ȯ��, ������ źȯ�� ��ȭ.\n\n�������\n\n��ź��, ���ݼӵ�, �������ð�, ���� ���ߵ�����\n\n������\n\n���� ����, ������ ����, �������(����)</size>\n\n" +
        "<size=60>��ü</size>\n\n<size=40>�÷��̾��� ������ ��ȭ, ȸ�� ���� Ư���� ����, �����, �������� �� \"��ȭ��\"�� �з��Ǵ� ���⸦ ��ȭ�� �ݴϴ�.\n3�ܰ� Ưȭ Ư���� ��ü���� ȿ���� ������ �����ϴ�.\n\n����\n\n���ݷ�, �˹� ����, �̵��ӵ� ���� ����, ���� ���� ����\n\n�����\n\n����źâ,���ݼӵ� ����, ���� ����, ȭ�� ���� ������\n\n��������\n\n����� ���, �ִ�ü��, ���ذ��� ����, ���ݷ� ����, �м� ȿ��(���°���, �̵��ӵ� ����)</size>\n\n<size=60>��ƿ��Ƽ</size>\n\n<size=40>�� �� ��</size>";
    const string monsters = "\n\n<size=60>���� ����</size>\n\n<size=40>�Ϲ�����\n\n���� �⺻���� ����� ��� �κп��� ������� ������ ������ �ִ�.\n\n��������\n\n���� ü���� ������ ������ ���� �̵��ӵ��� ������ �ִ�.\n\n�������\n\n���� ü�°� �˹�����, ������ ������ ������ �̵��ӵ��� ������.\n\n" +
        "���������\n\n�ſ� ���� ü���� ������ ������ �ſ� ���� �̵��ӵ��� ���� ���ݷ��� ������ �ִ�.\n\n������ ����\n\n�� �Ÿ����� ����ü�� ���� �����Ѵ�.\n\n��������\n\n�ſ� ���� ü�� �ſ���� ���ݷ� �˹������� ������ �ְ� �پ��� ��ų�� ����� �÷��̾ �����Ѵ�.</size>";
    #endregion
    #region Methods
    void HowToPlay()
    {
        m_scrollbar.value = 1;
        m_infoText.text = howToPlay;
    }
    void HowToControll()
    {
        m_scrollbar.value = 1;
        m_infoText.text = howToControll;
    }
    void WhatKindsOfItems()
    {
        m_scrollbar.value = 1;
        m_infoText.text = items;
    }
    void WhatKindsOfAbility()
    {
        m_scrollbar.value = 1;
        m_infoText.text = abilitys;
    }
    void WhatKindsOfMonsters()
    {
        m_scrollbar.value = 1;
        m_infoText.text = monsters;
    }
    void ReturnToGame()
    {
        gameObject.SetActive(false);
    }
    public void ActiveUI()
    {
        gameObject.SetActive(true);
        HowToPlay();
    }
    void SetAddListener()
    {
        m_play.onClick.AddListener(HowToPlay);
        m_control.onClick.AddListener(HowToControll);
        m_item.onClick.AddListener(WhatKindsOfItems);
        m_ability.onClick.AddListener(WhatKindsOfAbility);
        m_monster.onClick.AddListener(WhatKindsOfMonsters);
        m_return.onClick.AddListener(ReturnToGame);
        m_play.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_control.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_item.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_ability.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_monster.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
        m_return.onClick.AddListener(UGUIManager.Instance.PlayClickSFX);
    }
    public void SetTransform()
    {
        m_play = Utill.GetChildObject(gameObject,"GameSequance").GetComponent<Button>();
        m_control = Utill.GetChildObject(gameObject, "GameControll").GetComponent<Button>();
        m_item = Utill.GetChildObject(gameObject, "Item").GetComponent<Button>();
        m_ability = Utill.GetChildObject(gameObject, "Ability").GetComponent<Button>();
        m_monster = Utill.GetChildObject(gameObject, "Monster").GetComponent<Button>();
        m_return = Utill.GetChildObject(gameObject, "Return").GetComponent<Button>();
        m_infoText = Utill.GetChildObject(gameObject, "InfoText").GetComponent<TextMeshProUGUI>();
        m_scrollbar = Utill.GetChildObject(gameObject,"Scrollbar Vertical").GetComponent<Scrollbar>();
        SetAddListener();
    }
    #endregion

}
