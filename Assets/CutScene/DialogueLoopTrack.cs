using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
// 커스텀 트랙 클래스
[TrackColor(0.855f, 0.8623f, 0.87f)] // 트랙 색상 지정
[TrackClipType(typeof(DialogueLoopPlayableAsset))] // 타임라인에 추가될 수 있는 클립 타입 지정
public class DialogueLoopTrack : TrackAsset
{
    // 기본적으로 TrackAsset을 상속받아 추가 로직 없이도 동작 가능
}
