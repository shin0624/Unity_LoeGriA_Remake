using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    //파티클 시스템을 오브젝트 풀링으로 관리하여 성능 최적화를 시도하기 위한 클래스. 파티클 시스템이 작동하려면 Hierarchy에 객체가 존재하거나 Instantiate로 생성해주어야 하는데, 이를 최소화하기 위함
    [SerializeField] private GameObject particlePrefab;//파티클 프리팹
    [SerializeField] private int poolSize = 10;//풀 크기
    private Queue<GameObject> pool = new Queue<GameObject>();//파티클 오브젝트 풀

    private void Awake() {//초기화 : 풀 크기만큼 오브젝트 생성
        for(int i=0; i<poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);//파티클 생성
            particle.SetActive(false);//비활성화
            pool.Enqueue(particle);//풀에 추가
        }
    }
    
    public GameObject GetParticle()//파티클 오브젝트 풀에서 파티클을 가져오는 메서드
    {
        if(pool.Count>0)//풀에 파티클이 존재하면
        {
            GameObject particle = pool.Dequeue();//풀에서 파티클을 꺼내옴
            particle.SetActive(true);//활성화
            return particle;
        }
        
        else{
            GameObject particle = Instantiate(particlePrefab, transform);//풀에 파티클이 없으면 새로 생성
            particle.SetActive(true);//활성화
            return particle;
        }
    }

    public void ReturnParticle(GameObject particle)//파티클을 풀로 반환하는 메서드
    {
        particle.SetActive(false);//비활성화
        pool.Enqueue(particle);//풀로 반환
    }
}
