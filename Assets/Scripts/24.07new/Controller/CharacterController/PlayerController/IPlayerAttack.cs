using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
   //�÷��̾� ��Ʈ�ѷ��� ���� ���� å�� �и��� ���� ���ο� �������̽� ����. 
   //���� ������ PlayerController���� �����ϴ� �ڷ�ƾ�� ����, ������ PlayerAttack���� ����ϵ��� �ϱ� ����, Composition����� ����Ͽ� �������̽��� ���� �� ������Ʈ�� ������ ������ �����Ѵ�.
    
    void PerformAttack(int attackNumber);//���� ���� �޼���
    bool IsAttack {get;}// ���� ���� ���� �������� Ȯ���ϴ� ������Ƽ
}
