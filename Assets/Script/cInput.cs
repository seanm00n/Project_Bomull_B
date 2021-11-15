﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cInput : MonoBehaviour
{
    public void CharacterSelectInputProcess() {
        int index01;
        Vector3 tPosition;
        Vector2 tDirection;
        RaycastHit2D tHit;

        if (Input.GetMouseButtonUp(0) != true) {
            return;
        }
        tPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tDirection.x = 0.0f;
        tDirection.y = 0.0f;
        tHit = Physics2D.Raycast(tPosition, tDirection);
        if (tHit.collider == null) {
            return;
        }

        for (index01 = 0; index01 < cGV.MAX_CHARACTER_NUM; index01++) {
            if (tHit.collider.name == cGV.I.vCharacterName[index01]) {
                cGV.I.vCharacterIndex = index01;//??
                cInit.I.Destroy_Character_Select();
            }
        }
        if (cInit.I.Initialize_Game() != true) {
            cGV.I.QuitProcess("Error::Initialize_Game() == false");
            return;
        }
    }
    public void GameInputProcess() {
        float tValueX;
        float tValueY;
        Vector3 tVector3;
        Vector2 tVector2;
        if (!cGV.I.sCharacter[cGV.I.vCharacterIndex].cGameObject.activeSelf) {//true상태인지 확인
            return;
        }

        tValueX = 0.0f;
        tValueX = Input.GetAxisRaw("Horizontal");//입력값 백터값으로 저장
        tValueY = 0.0f;
        tValueY = Input.GetAxisRaw("Vertical");

        /*이 구문의 위치를 잘 생각 할 것*/

        if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_HIT) {
            tVector3.x = 0.0f;
            tVector3.y = 0.0f;
            tVector3.z = 0.0f;
            cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity = tVector3; //캐릭터 정지
            if (cGV.I.GetMessage(cGV.SUB_MESSAGE_TYPE_HIT_END, 0, 0, cGV.I.sCharacter[cGV.I.vCharacterIndex].vMessage, null, false)) {//메시지에 hitend가 있으면
                if (cGV.I.GetMessage(cGV.SUB_MESSAGE_TYPE_DEATH, 0, 0, cGV.I.sCharacter[cGV.I.vCharacterIndex].vMessage, null, false)) {

                    cGV.I.SetAnimation(cGV.ANIMATION_STATE_DEATH, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                } else {
                    cGV.I.SetAnimation(cGV.ANIMATION_STATE_IDLE, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                }
            }
            return;
        }
//1111/1159
        if (cGV.I.GetMessage(cGV.SUB_MESSAGE_TYPE_COLLISION, 0, 0, cGV.I.sCharacter[cGV.I.vCharacterIndex].vMessage, null, false)) {
            if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex != cGV.ANIMATION_STATE_JUMP) {
                if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_FALL) {//
                    tVector2.x = 0.0f;
                    tVector2.y = cGV.I.sCharacter[cGV.I.vCharacterIndex].vJumpSpeed;
                    cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.AddForce(tVector2, ForceMode2D.Impulse);
                    cGV.I.SetAnimation(cGV.ANIMATION_STATE_JUMP, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                } else {
                    cGV.I.SetAnimation(cGV.ANIMATION_STATE_HIT, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                    tVector2.x = 0.0f;
                    tVector2.y = 0.0f;
                    cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity = tVector2;
                }
            }
            return;
        }
        if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_JUMP) {
            if (cGV.I.GetMessage(cGV.SUB_MESSAGE_TYPE_FALL, 0, 0, cGV.I.sCharacter[cGV.I.vCharacterIndex].vMessage, null, false) == true) {
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_FALL, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                return;
            }
        }//Fall
        if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_FALL) {
            if (cGV.I.GetMessage(cGV.SUB_MESSAGE_TYPE_CLIPPING, 0, 0, cGV.I.sCharacter[cGV.I.vCharacterIndex].vMessage, null, false) == true) {
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_IDLE, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                return;
            }
        }


        if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_IDLE || cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_RUN) {
            if (tValueY == 1.0f) {
                tVector2.x = 0.0f;
                tVector2.y = cGV.I.sCharacter[cGV.I.vCharacterIndex].vJumpSpeed;
                cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.AddForce(tVector2, ForceMode2D.Impulse);
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_JUMP, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                return;
            }
            if (tValueX == 1.0f || tValueX == -1.0f) {//애니메이션 상태가 걷거나 기본상태이고 입력값이 좌(-1) 또는 우(1) 일때
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_RUN, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
            } else {
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_IDLE, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
            }
        }


        if (tValueX == 1.0f || tValueX == -1.0f) {
            cGV.I.SetDirection((int)tValueX, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);//방향선택
        }

        if ((cGV.I.sCharacter[cGV.I.vCharacterIndex].cGameObject.transform.position.x < cGV.I.sCharacter[cGV.I.vCharacterIndex].vHalfSizeX && tValueX < 0.0f) ||
            (cGV.I.sCharacter[cGV.I.vCharacterIndex].cGameObject.transform.position.x > cGV.I.sWorld[cGV.I.vWorldIndex].vWorldWidth - cGV.I.sCharacter[cGV.I.vCharacterIndex].vHalfSizeX && tValueX > 0.0f)) {
            tVector3.x = 0.0f;
            tVector3.y = cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity.y;
            tVector3.z = 0.0f;
            cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity = tVector3;
            //맵을 넘어 갈 시 이동하지 않고 제자리
            return;
        }

        tVector3.x = tValueX * cGV.I.sCharacter[cGV.I.vCharacterIndex].vRunSpeed;
        tVector3.y = cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity.y;
        tVector3.z = 0.0f;
        cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity = tVector3;
        //문제 없을 시 이동처리

        if (cGV.I.sCharacter[cGV.I.vCharacterIndex].cGameObject.transform.position.x > cGV.I.sScreen.vScreenWidth / 2.0f &&
            cGV.I.sCharacter[cGV.I.vCharacterIndex].cGameObject.transform.position.x < cGV.I.sWorld[cGV.I.vWorldIndex].vWorldWidth - (cGV.I.sScreen.vScreenWidth / 2.0f)) {
            tVector3.x = cGV.I.sCharacter[cGV.I.vCharacterIndex].cGameObject.transform.position.x;
            tVector3.y = Camera.main.transform.position.y;
            tVector3.z = Camera.main.transform.position.z;
            Camera.main.transform.position = tVector3;
        }//스크린 절반 이상 이동시 카메라 같이 움직임
        if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_DEATH) {
            cGV.I.sCharacter[cGV.I.vCharacterIndex].vRunSpeed = 0;
            return;
        }

        //캐릭터 동작 처리 마우스
        if (Input.GetMouseButtonUp(0)) {
            if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_IDLE || cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_RUN) {
                cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity = Vector3.zero;
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_ATTACK_A, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                cGV.I.sCharacter[cGV.I.vCharacterIndex].vHitCheck = true;
                return;
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            if (cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_IDLE || cGV.I.sCharacter[cGV.I.vCharacterIndex].vAnimationIndex == cGV.ANIMATION_STATE_RUN) {
                cGV.I.sCharacter[cGV.I.vCharacterIndex].cRigidBody.velocity = Vector3.zero;
                cGV.I.SetAnimation(cGV.ANIMATION_STATE_ATTACK_B, ref cGV.I.sCharacter[cGV.I.vCharacterIndex]);
                cGV.I.sCharacter[cGV.I.vCharacterIndex].vHitCheck = true;
                return;
            }
        }
    }
    void Update()
    {
        switch (cGV.I.vApplicationState) {
            case cGV.APPLICATION_STATE_MAIN:
                break;
            case cGV.APPLICATION_STATE_CHSL:
                CharacterSelectInputProcess();
                break;
            case cGV.APPLICATION_STATE_GAME:
                this.GameInputProcess();
                break;
            case cGV.APPLICATION_STATE_PAUS:
                break;
            case cGV.APPLICATION_STATE_GAOV:
                break;
        }
    }
    public void OnClickButton01() {
        cInit.I.Destroy_Main();
        cInit.I.Initialize_CharacterSelect();
    }
    public void OnClickButton02() {
        Application.Quit();
    }
    public void OnClickButton03() {
        cInit.I.Destroy_Game();
        cInit.I.Initialize_Pause();
    }
    public void OnClickButton04() {
        cInit.I.Destroy_Pause();
        cInit.I.Initialize_Game();
    }
    public void OnClickButton05() {
        cInit.I.Destroy_Pause();
        cInit.I.Initialize_Main();
    }
    public void OnClickButton06() {
        cInit.I.Destroy_GameOver();
        cInit.I.Initialize_Game();
    }
    public void OnClickButton07() {
        cInit.I.Destroy_GameOver();
        cInit.I.Initialize_Main();
    }

}