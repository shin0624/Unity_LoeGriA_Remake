using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    //��ƼŬ �ý����� ������Ʈ Ǯ������ �����Ͽ� ���� ����ȭ�� �õ��ϱ� ���� Ŭ����. ��ƼŬ �ý����� �۵��Ϸ��� Hierarchy�� ��ü�� �����ϰų� Instantiate�� �������־�� �ϴµ�, �̸� �ּ�ȭ�ϱ� ����
    [SerializeField] private GameObject particlePrefab;//��ƼŬ ������
    [SerializeField] private int poolSize = 10;//Ǯ ũ��
    private Queue<GameObject> pool = new Queue<GameObject>();//��ƼŬ ������Ʈ Ǯ

    private void Awake() {//�ʱ�ȭ : Ǯ ũ�⸸ŭ ������Ʈ ����
        for(int i=0; i<poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);//��ƼŬ ����
            particle.SetActive(false);//��Ȱ��ȭ
            pool.Enqueue(particle);//Ǯ�� �߰�
        }
    }
    
    public GameObject GetParticle()//��ƼŬ ������Ʈ Ǯ���� ��ƼŬ�� �������� �޼���
    {
        if(pool.Count>0)//Ǯ�� ��ƼŬ�� �����ϸ�
        {
            GameObject particle = pool.Dequeue();//Ǯ���� ��ƼŬ�� ������
            particle.SetActive(true);//Ȱ��ȭ
            return particle;
        }
        
        else{
            GameObject particle = Instantiate(particlePrefab, transform);//Ǯ�� ��ƼŬ�� ������ ���� ����
            particle.SetActive(true);//Ȱ��ȭ
            return particle;
        }
    }

    public void ReturnParticle(GameObject particle)//��ƼŬ�� Ǯ�� ��ȯ�ϴ� �޼���
    {
        particle.SetActive(false);//��Ȱ��ȭ
        pool.Enqueue(particle);//Ǯ�� ��ȯ
    }
}
