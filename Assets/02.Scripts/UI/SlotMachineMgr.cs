using System.Collections;
using System.Collections.Generic;
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

    public List<int> StartList = new List<int>(); // 랜덤 이미지를 위한 리스트
    public List<int> ResultIndexList = new List<int>();
    int ItemCnt = 3;

    void Start()
    {
        InitializeStartList(); // StartList 초기화
        AssignRandomImages(); // 슬롯에 랜덤 이미지 할당

        // 슬롯 버튼 비활성화 후 개별적으로 회전 시작
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

    // StartList 초기화 (SkillSprite 인덱스를 전부 추가)
    void InitializeStartList()
    {
        StartList.Clear();
        for (int i = 0; i < SkillSprite.Length; i++)
        {
            StartList.Add(i);
        }

        // 리스트를 랜덤하게 섞기
        ShuffleList(StartList);
    }

    // 슬롯에 랜덤 이미지 할당
    void AssignRandomImages()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            for (int j = 0; j < ItemCnt; j++)
            {
                int randomIndex = Random.Range(0, StartList.Count);
                int skillIndex = StartList[randomIndex]; // 랜덤한 스킬 인덱스 가져오기
                DisplayItemSlots[i].SlotSprite[j].sprite = SkillSprite[skillIndex];

                // 중복 방지를 위해 리스트에서 제거
                StartList.RemoveAt(randomIndex);

                // 만약 StartList가 비었다면 다시 초기화
                if (StartList.Count == 0)
                {
                    InitializeStartList();
                }
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

        yield return StartCoroutine(SlowDown(slotObject));
        Slot[slotIndex].interactable = true;
    }

    IEnumerator SlowDown(GameObject slotObject)
{
    float currentY = slotObject.transform.localPosition.y;
    
    // 0, 100, 200, 300 중 가장 가까운 위치로 정렬
    float targetY = Mathf.Round(currentY / 100) * 100;

    // 최대 300을 넘지 않도록 보장
    targetY = Mathf.Clamp(targetY, 0, 300);

    while (Mathf.Abs(slotObject.transform.localPosition.y - targetY) > 1f)
    {
        slotObject.transform.localPosition = Vector3.Lerp(slotObject.transform.localPosition, 
                                                          new Vector3(slotObject.transform.localPosition.x, targetY, 0), 
                                                          0.2f);
        yield return new WaitForSeconds(0.05f);
    }

    slotObject.transform.localPosition = new Vector3(slotObject.transform.localPosition.x, targetY, 0);
}


    // 리스트를 랜덤하게 섞는 함수
    void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap
        }
    }
}
