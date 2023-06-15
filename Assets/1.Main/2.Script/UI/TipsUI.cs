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
    const string howToPlay = "\n<size=60>플레이방법</size>\n\n<size=40>라운드별로 몰려오는 좀비들로부터 생존하세요. \n좀비들은 날이 지날 수록 끝없이 강해집니다.</size>\n\n<size=60>낮과 밤</size>\n\n<size=40>좀비 서바이벌에는 낮과 밤이 존재합니다. \n낮 시간에는 좀비들의 공격이 멈추게 되고 정비를 할 시간이 주어집니다. 이 시간을 최대한 활용하여 침공에 대비하세요.\n시간이 지나 밤이 되면 좀비들의 공격이 시작됩니다.\n좀비들을 모두 처치하여 안전한 낮까지의 생존을 이어가세요.</size>" +
        "\n\n<size=60>상점</size>\n\n<size=40>좀비를 처치하면 돈를 얻을 수 있습니다.\n돈을 모아 스타트 지점에 있는 상점에서 무기, 방어구, 아이템을 구매해 전투에 활용하세요.\n상점의 아이템은 10라운드, 20라운드에 갱신됩니다.</size>\n\n<size=60>레벨업</size>\n\n<size=40>좀비를 사냥하면 경험치를 획득합니다.\n이를 통해 레벨업을 할 경우 체력 완전 회복 효과와 더불어 특성 포인트를 획득하게 됩니다.\n획득한 특성포인트로 특성을 강화하여 생존에 유리한 효과를 얻으세요.</size>";
    const string howToControll = "\n\n<size=60>조작법</size>\n\n<size=50>이동 : W A S D 혹은 방향키\n\n조준 : 마우스 커서의 위치\n\n공격 : 마우스 좌클릭\n\n재장전 : R\n\n스테이터스창 : I\n\n스킬창 : K\n\n메뉴 : ESC</size>";
    const string items = "\n\n<size=60>무기</size>\n\n<size=40>좀비 서바이벌에는 7종류의 무기가 존재합니다.\n플레이어는 한개의 무기를 들 수 있고 무기를 구입 시 기존 무기는 삭제처리 됩니다.\n무기 각각의 특징은 다음과 같습니다 \n\n권총\n\n매우 빠른 이동속도,느린 공격속도, 강력한 한방 데미지, 강력한 치명타 데미지가 특징입니다." +
        "\n\n기관단총\n\n가벼운 무게로 빠른 이동속도, 매우 빠른 공격속도가 특징입니다.\n\n라이플\n\n가장 평균적인 무기로 모든 부분에 있어 평균적인 모습을 보여줍니다.\n\n기관총\n\n무거운 무게로 이동속도에 패널티가 있지만 높은 장탄량, 매우빠른 공격속도가 특징입니다.\n\n샷건\n\n다른 총기 대비 짧은 사정거리를 가지고 있지만 그만큼 강력한 데미지, 높은 저지력을 가지고 있습니다.\n\n야구배트\n\n가장 짧은 사정거리를 가지고 있지만 범위공격, 괜찮은 공격 속도를 가지고 있어 밸런스가 좋습니다.\n\n도끼\n\n짧은 사정거리를 가지고 있지만 범위공격, 높은 데미지를 가지고 있습니다.  " +
        "</size>\n\n<size=60>방어구</size>\n\n<size=40>방어구는 기본적으로 방어력을 제공하고 부위별로 추가적인 효과를 제공합니다 추가 효과의 성능은 다음과 같습니다.\n헬멧 : 치명타 확률을 높여줍니다.\n장갑 : 공격속도를 높여줍니다.\n상의 : 추가 최대체력을 제공합니다.\n바지 : 공격력을 높여줍니다.\n신발 : 이동속도를 높여줍니다.\n" +
        "</size>\n\n<size=60>소모품</size>\n\n<size=40>소모품은 구급상자, 바리케이드(미구현), 포탑(미구현), 함정(미구현) 등이 있으며 다음과 같은 효과를 가지고 있습니다.\n구급상자 : 최대체력의 n%를 회복합니다.\n바리케이드 : 좀비의 공격 대상이 되며 일정한 만큼의 피해를 막아줍니다.\n포탑 : 근처 좀비들을 공격합니다. 좀비의 공격에 파괴될 수 있습니다.\n함정 : 좀비에게 피해, 둔화 등을 주는 함정이 있습니다\n</size>";
    const string abilitys = "\n\n<size=60>특성의 종류</size>\n\n<size=40>좀비 서바이벌의 특성은 3가지 종류가 존재합니다.\n각각의 특성은 1단계, 2단계, 3단계, 마스터 특성으로 나뉘어 지며 선행 조건이 만족되면 상위 특성을 익힐 수 있습니다.\n특성의 개방 조건은 다음과 같습니다.\n1단계 특성 : 조건없이 습득 가능합니다.(소모 포인트 1)\n2단계 특성 : 같은 종류의 1단계 특성 8개를 습득 후 습득 가능합니다.(소모 포인트 1)\n3단계 특성 : 같은 종류의 2단계 특성 6개 습득 후 개방 가능합니다.\n무기 한종류를 골라 특화할 수 있습니다(한번 고른 후 변경 불가능, 습득 포인트 3)\n마스터 스킬 : 3단계 특화 스킬 모두 습득 후 습득 가능합니다.(소모 포인트 0)\n사격술 : 권총, 기관단총, 라이플 등 \"개인화기\"로 분류되는 무기를 강화해 줍니다.\n신체 : 플레이어의 내구도 강화, 회복 관련 특성과 샷건, 기관총, 근접무기 등 \"중화기\"로 분류되는 무기를 강화해 줍니다.\n유틸리티(미구현) : 바리케이드. 포탑, 함정 등의 \"설치물\"의 강화와 아군을 강화시켜 줍니다.</size>\n\n" +
        "<size=60>사격술</size>\n\n<size=40>권총, 기관단총, 라이플 등 \"개인화기\"로 분류되는 무기를 강화해 줍니다.\n3단계 특화 특성의 전체적인 효과는 다음과 같습니다.\n\n권총\n\n치명타 확률, 치명타 데미지, 넉백 확률, 마지막 탄환의 강화.\n\n기관단총\n\n장탄량, 공격속도, 재장전시간, 범위 폭발데미지\n\n라이플\n\n방어력 관통, 데미지 증가, 관통공격(범위)</size>\n\n" +
        "<size=60>신체</size>\n\n<size=40>플레이어의 내구도 강화, 회복 관련 특성과 샷건, 기관총, 근접무기 등 \"중화기\"로 분류되는 무기를 강화해 줍니다.\n3단계 특화 특성의 전체적인 효과는 다음과 같습니다.\n\n샷건\n\n공격력, 넉백 증가, 이동속도 대폭 증가, 관통 범위 공격\n\n기관총\n\n무한탄창,공격속도 증가, 방어력 무시, 화상 지속 데미지\n\n근접무기\n\n생명력 흡수, 최대체력, 피해감소 증가, 공격력 증가, 분쇄 효과(방어력감소, 이동속도 감소)</size>\n\n<size=60>유틸리티</size>\n\n<size=40>미 구 현</size>";
    const string monsters = "\n\n<size=60>몬스터 종류</size>\n\n<size=40>일반좀비\n\n가장 기본적인 좀비로 모든 부분에서 평균적인 스탯을 가지고 있다.\n\n응애좀비\n\n낮은 체력을 가지고 있지만 빠른 이동속도를 가지고 있다.\n\n헤비좀비\n\n높은 체력과 넉백저항, 방어력을 가지고 있지만 이동속도가 느리다.\n\n" +
        "댕댕이좀비\n\n매우 낮은 체력을 가지고 있지만 매우 빠른 이동속도와 강한 공격력을 가지고 있다.\n\n스피터 좀비\n\n먼 거리에서 투사체를 날려 공격한다.\n\n보스좀비\n\n매우 높은 체력 매우높은 공격력 넉백저항을 가지고 있고 다양한 스킬을 사용해 플레이어를 위협한다.</size>";
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
