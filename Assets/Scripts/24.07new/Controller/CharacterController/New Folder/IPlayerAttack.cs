using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
   //�÷��̾� ��Ʈ�ѷ��� å�� ������ ���� ���� å�� �и��� ���� ���ο� �������̽� ����. 
   //���� ������ PlayerController���� �����ϴ� �ڷ�ƾ�� ����, ������ PlayerAttack���� ����ϵ��� �ϱ� ����, Composition����� ����Ͽ� �������̽��� ���� �� ������Ʈ�� ������ ������ �����Ѵ�.
    
    void PerformAttack();//���� ���� �޼���
    bool IsAttack {get;}// ���� ���� ���� �������� Ȯ���ϴ� ������Ƽ
}
