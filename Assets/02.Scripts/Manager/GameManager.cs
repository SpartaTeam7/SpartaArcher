using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void StartGame()
    {
        //  ȣ��Ǹ� ������ ���۵�
    }

    public void EndGame()
    {
        //  ������ ������ ������ ����ǰ� ����ȭ������ ���ư�
    }

    public void GameOver()
    {
        //  �÷��̾��� HP�� 0�̵Ǹ� GameOver���� UI�� �����
    }
}
