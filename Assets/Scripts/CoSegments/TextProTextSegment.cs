using Common.Coroutines;
using Common.Coroutines.Segments;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Tanks.Animation
{
    [SegmentMenu("TextPro")]
    public class TextProTextSegment : TimedSegment
    {
        public TextMeshProUGUI text;
        public string target;

        public string Substring(string value, float t)
            => value.Substring(0, Mathf.CeilToInt(value.Length * t));

        public override IEnumerator GetSequence()
            => UCoroutine.YieldValue(t => text.text = Substring(target, t), UCoroutine.YieldTime(duration, easer.Evaluate));
    }
}