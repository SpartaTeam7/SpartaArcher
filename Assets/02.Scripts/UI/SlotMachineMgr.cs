using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineMgr : MonoBehaviour
{
    public GameObject[] SlotSkillObject;
    public Button[] Slot;
    public Sprite[] SkillSprite;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> SlotSprite = new List<Image>();
    }
    public DisplayItemSlot[] DisplayItemSlots;

    public Image DisplayItemImage;

    public List<int> StartList = new List<int>();
    public List<int> ResultIndexList = new List<int>();
    int ItemCnt = 3;

    public GameObject upgradeFrame;
    public Text upgradeTxt;

    private Dictionary<int, int> slotFinalResults = new Dictionary<int, int>(); // 슬롯 결과 저장

    void OnEnable()
    {
        StartCoroutine(StartSlotWithDelay());
        upgradeFrame.SetActive(false);
    }

    IEnumerator StartSlotWithDelay()
    {
        yield return new WaitForSeconds(1f); // 활성화 후 1초 대기
        StartSlotMachine();
    }

    void StartSlotMachine()
    {
        InitializeStartList();
        AssignRandomImages();

        foreach (var btn in Slot)
        {
            btn.interactable = false;
        }

        for (int i = 0; i < Slot.Length; i++)
        {
            float randomSpeed = Random.Range(0.03f, 0.07f);
            float randomStopDelay = Random.Range(2.0f, 4.0f);
            StartCoroutine(StartSlot(i, randomSpeed, randomStopDelay));
        }
    }

    void InitializeStartList()
    {
        StartList.Clear();
        for (int i = 0; i < SkillSprite.Length; i++)
        {
            StartList.Add(i);
        }
        ShuffleList(StartList);
    }

   void AssignRandomImages()
{
    for (int i = 0; i < Slot.Length; i++)
    {
        for (int j = 0; j < ItemCnt; j++)
        {
            // 무작위로 스프라이트를 선택
            int randomIndex = Random.Range(0, SkillSprite.Length);
            DisplayItemSlots[i].SlotSprite[j].sprite = SkillSprite[randomIndex];
        }
    }
}

    IEnumerator StartSlot(int slotIndex, float speed, float stopDelay)
    {
        GameObject slotObject = SlotSkillObject[slotIndex];
        float elapsedTime = 0f;

        while (elapsedTime < stopDelay)
        {
            slotObject.transform.localPosition -= new Vector3(0, 50f, 0);

            if (slotObject.transform.localPosition.y < -100f)
            {
                slotObject.transform.localPosition += new Vector3(0, 400f, 0);
            }

            elapsedTime += speed;
            yield return new WaitForSeconds(speed);
        }

        yield return StartCoroutine(SlowDown(slotIndex, slotObject));
        Slot[slotIndex].interactable = true;
    }

   IEnumerator SlowDown(int slotIndex, GameObject slotObject)
{
    // 첫 번째 스프라이트에서 멈추게 고정
    float currentY = slotObject.transform.localPosition.y;
    float targetY = 0f;  // 첫 번째 스프라이트 위치로 고정 (0 위치로 멈추게)

    // 부드럽게 멈추는 효과
    while (Mathf.Abs(slotObject.transform.localPosition.y - targetY) > 1f)
    {
        slotObject.transform.localPosition = Vector3.Lerp(
            slotObject.transform.localPosition,
            new Vector3(slotObject.transform.localPosition.x, targetY, 0),
            0.2f
        );
        yield return new WaitForSeconds(0.05f);
    }

    slotObject.transform.localPosition = new Vector3(slotObject.transform.localPosition.x, targetY, 0);

    // 슬롯이 멈추면 첫 번째 스프라이트를 선택
    int spriteIndex = 0; // 항상 첫 번째 스프라이트로 고정
    slotFinalResults[slotIndex] = spriteIndex; // 슬롯 결과 저장

    // 버튼 클릭 이벤트 설정
    Slot[slotIndex].onClick.RemoveAllListeners();
    Slot[slotIndex].onClick.AddListener(() => OnSlotButtonClick(slotIndex, spriteIndex));
}

void OnSlotButtonClick(int slotIndex, int spriteIndex)
{
    string upgradeMessage = "";
    string spriteName = DisplayItemSlots[slotIndex].SlotSprite[0].sprite.name;
    Debug.Log("Clicked Sprite: " + spriteName); // 클릭한 스프라이트 이름을 출력

    // upgradeFrame 활성화
    if (upgradeFrame != null)
    {
        upgradeFrame.SetActive(true);
    }

    // 스프라이트 이름에 따라 업그레이드 처리
    if(spriteName == "Back_Arrow")
    {
        SkillManager.Instance.ChangeNumberOfBackwardProjectiles(1);
        upgradeMessage = "후방 발사 화살이 1개 증가했습니다!";
    }
    else if(spriteName == "Heal")
    {
        SkillManager.Instance.HealPlayer(10);
        upgradeMessage = "플레이어의 체력을 10 회복했습니다";
    }
    else if(spriteName == "HealthUp")
    {
        SkillManager.Instance.ChangeMaxHealth(5);
        upgradeMessage = "플레이어의 체력이 5 증가했습니다";
    }
    else if(spriteName == "Multi_Arrow")
    {
        SkillManager.Instance.ChangeNumberOfForwardProjectiles(1);
        upgradeMessage = "전방 화살이 한 개 증가했습니다";
    }
    else if(spriteName == "PowerUp")
    {
        SkillManager.Instance.ChangePower(5);
        upgradeMessage = "공격력이 5 증가했습니다";
    }
    else if(spriteName == "Side")
    {
        SkillManager.Instance.ChangeNumberOfSideProjectiles(1);
        upgradeMessage = "좌우 화살이 한 개 증가하였습니다";
    }
    else if(spriteName == "Reflection")
    {
        SkillManager.Instance.ChangeReflectionCount(1);
        upgradeMessage = "화살이 한 번 더 튕깁니다";
    }
    else if(spriteName == "Critical")
    {
        SkillManager.Instance.ChangeCriticalChance(20);
        upgradeMessage = "크리티컬 확률 20퍼 증가";
    }
    else if(spriteName == "CriDamage")
    {
        SkillManager.Instance.ChangeCriticalDamage(10);
        upgradeMessage = "크리티컬 데미지 10퍼센트 증가";
    }
    else if(spriteName == "Dps")
    {
        SkillManager.Instance.ChangeDelay(0.1f);
        upgradeMessage = "공속 증가";
    }

    upgradeTxt.text = upgradeMessage;

    // 다른 슬롯 비활성화
    DisableOtherSlots(slotIndex);
}


    void DisableOtherSlots(int selectedSlotIndex)
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            if (i != selectedSlotIndex)
            {
                Slot[i].interactable = false;
            }
        }
    }

    void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
