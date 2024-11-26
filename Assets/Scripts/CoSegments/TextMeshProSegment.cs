using Common.Coroutines;
using Common.Coroutines.Segments;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Tanks.Animation
{
    [SegmentMenu("TextMeshPro")]
    public class TextMeshProSegment : TimedSegment
    {
        public TextMeshProUGUI text;
        public string target;

        public string Substring(string value, float t)
            => value.Substring(0, Mathf.CeilToInt(value.Length * t));

        public override IEnumerator Build()
            => Yield.Value(t => text.text = Substring(target, t), Yield.Time(duration, easer.Evaluate));
    }
}