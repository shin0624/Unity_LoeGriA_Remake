using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
   //플레이어 컨트롤러의 공격 로직 책임 분리를 위해 새로운 인터페이스 생성. 
   //공격 판정은 PlayerController에서 관리하는 코루틴을 유지, 실행은 PlayerAttack에서 담당하도록 하기 위해, Composition방식을 사용하여 인터페이스를 통한 두 컴포넌트의 느슨한 결합을 구현한다.
    
    void PerformAttack(int attackNumber);//공격 실행 메서드
    bool IsAttack {get;}// 현재 공격 중인 상태인지 확인하는 프로퍼티
}
