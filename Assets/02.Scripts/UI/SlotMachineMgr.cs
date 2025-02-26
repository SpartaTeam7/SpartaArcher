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

    public List<int> StartList = new List<int>();
    public List<int> ResultIndexList = new List<int>();
    int ItemCnt = 3;
    
    private Dictionary<int, int> slotFinalResults = new Dictionary<int, int>(); // 슬롯 결과 저장

    void Start()
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
                int randomIndex = Random.Range(0, StartList.Count);
                int skillIndex = StartList[randomIndex];
                DisplayItemSlots[i].SlotSprite[j].sprite = SkillSprite[skillIndex];
                StartList.RemoveAt(randomIndex);

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

        yield return StartCoroutine(SlowDown(slotIndex, slotObject));
        Slot[slotIndex].interactable = true;
    }

    IEnumerator SlowDown(int slotIndex, GameObject slotObject)
    {
        float currentY = slotObject.transform.localPosition.y;
        float targetY = Mathf.Round(currentY / 100) * 100;
        targetY = Mathf.Clamp(targetY, 0, 300);

        while (Mathf.Abs(slotObject.transform.localPosition.y - targetY) > 1f)
        {
            slotObject.transform.localPosition = Vector3.Lerp(slotObject.transform.localPosition, 
                                                              new Vector3(slotObject.transform.localPosition.x, targetY, 0), 
                                                              0.2f);
            yield return new WaitForSeconds(0.05f);
        }

        slotObject.transform.localPosition = new Vector3(slotObject.transform.localPosition.x, targetY, 0);

        // 슬롯이 멈춘 위치 기반으로 결과 결정
        int resultIndex = Mathf.RoundToInt(targetY / 100) % ItemCnt;
        Sprite finalSprite = DisplayItemSlots[slotIndex].SlotSprite[resultIndex].sprite;

        // 슬롯 결과 저장
        slotFinalResults[slotIndex] = resultIndex;

        // 버튼에 클릭 이벤트 추가
        Slot[slotIndex].onClick.RemoveAllListeners();
        Slot[slotIndex].onClick.AddListener(() => OnSlotButtonClick(slotIndex, finalSprite));
    }

    void OnSlotButtonClick(int slotIndex, Sprite sprite)
    {
        Debug.Log($"슬롯 {slotIndex} 클릭! 선택된 이미지: {sprite.name}");
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
