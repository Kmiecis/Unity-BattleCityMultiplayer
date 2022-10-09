using Common.Extensions;
using Common.Mathematics;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;

namespace Tanks
{
    [ExecuteInEditMode]
    public class SquareObjectPainter : MonoBehaviour
    {
#if UNITY_EDITOR
        private static readonly Random kRandom = new Random();
        private static readonly Plane kPlane = new Plane { normal = Vector3.forward };
        private static readonly Vector2Int EmptyCoordinate = Vector2Int.one * int.MaxValue;

        private Dictionary<Vector2Int, GameObject> _objects = new();

        public Vector2 size = Vector2.one;
        public GameObject[] prefabs;

        private bool _isPainting;
        private Vector2Int _paintCoordinate = EmptyCoordinate;
        private Vector2Int _eraseCoordinate = EmptyCoordinate;

        public bool IsPainting
        {
            get => _isPainting;
            set => _isPainting = value;
        }

        public void StartPainting()
        {
            if (_isPainting)
                return;

            _isPainting = true;
        }

        public void StopPainting()
        {
            if (!_isPainting)
                return;

            _isPainting = false;
        }

        public void OnSceneGUI()
        {
            const int kLeftMouseButton = 0;
            var currentEvent = Event.current;

            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    if (currentEvent.button == kLeftMouseButton)
                    {
                        if (_isPainting)
                        {
                            if (currentEvent.shift)
                            {
                                Erase(currentEvent.mousePosition);
                            }
                            else
                            {
                                Paint(currentEvent.mousePosition);
                            }
                        }
                        currentEvent.Use();
                    }
                    break;

                case EventType.MouseMove:
                    HandleUtility.Repaint();
                    break;

                case EventType.MouseDrag:
                    if (currentEvent.button == kLeftMouseButton)
                    {
                        if (_isPainting)
                        {
                            if (currentEvent.shift)
                            {
                                Erase(currentEvent.mousePosition);
                            }
                            else
                            {
                                Paint(currentEvent.mousePosition);
                            }
                        }
                        currentEvent.Use();
                    }
                    break;

                case EventType.Repaint:
                    DrawSceneGizmos(currentEvent.mousePosition);
                    break;

                case EventType.Layout:
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    break;
            }
        }

        private bool TryGetHitPosition(Vector2 mousePosition, out Vector3 hitPosition)
        {
            var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            if (kPlane.Raycast(ray, out float distance))
            {
                var hitPoint = ray.GetPoint(distance);
                hitPosition = transform.InverseTransformPoint(hitPoint);
                return true;
            }

            hitPosition = default;
            return false;
        }

        private void DrawSquareGizmo(Vector2Int coordinate)
        {
            var position = Rects.Convert(coordinate, size);
            var vertices = Rects.GetVertices(position, size, 0.0f);
            var points = new List<Vector3>(vertices.Select(v2 => v2.XY_()));
            points.Add(points[0]);

            Handles.zTest = CompareFunction.Always;
            Handles.DrawPolyLine(points.ToArray());
        }

        private void Paint(Vector2 mousePosition)
        {
            if (TryGetHitPosition(mousePosition, out var hitPosition))
            {
                var hitCoordinate = Rects.Convert(hitPosition, size);

                if (hitCoordinate != _paintCoordinate)
                {
                    if (prefabs.Length > 0)
                    {
                        PaintItem(hitCoordinate, kRandom.NextItem(prefabs));
                    }

                    _paintCoordinate = hitCoordinate;
                }

                _eraseCoordinate = EmptyCoordinate;
            }
        }

        private void PaintItem(Vector2Int coordinate, GameObject item)
        {
            if (!_objects.ContainsKey(coordinate) && item != null)
            {
                var position = Rects.Convert(coordinate, size);
                var instance = PrefabUtility.InstantiatePrefab(item, transform) as GameObject;
                instance.transform.position = position;

                _objects.Add(coordinate, instance);
            }
        }

        private void Erase(Vector2 mousePosition)
        {
            if (TryGetHitPosition(mousePosition, out var hitPosition))
            {
                var hitCoordinate = Rects.Convert(hitPosition, size);

                if (hitCoordinate != _eraseCoordinate)
                {
                    EraseItem(hitCoordinate);

                    _eraseCoordinate = hitCoordinate;
                }

                _paintCoordinate = EmptyCoordinate;
            }
        }

        private void EraseItem(Vector2Int coordinate)
        {
            if (_objects.TryGetValue(coordinate, out var item))
            {
                DestroyImmediate(item);
                _objects.Remove(coordinate);
            }
        }

        private void DrawSceneGizmos(Vector2 mousePosition)
        {
            if (TryGetHitPosition(mousePosition, out var hitPosition))
            {
                var hitCoordinate = Rects.Convert(hitPosition, size);

                DrawSquareGizmo(hitCoordinate);
            }
        }

        private void ReadObjects(Transform transform)
        {
            var childCount = transform.childCount;
            var objectCount = _objects.Count;
            if (objectCount != childCount)
            {
                var removed = new List<GameObject>();

                _objects.Clear();
                for (int i = 0; i < childCount; ++i)
                {
                    var child = transform.GetChild(i);
                    var coordinate = Rects.Convert(child.position, size);

                    if (!_objects.ContainsKey(coordinate))
                    {
                        _objects.Add(coordinate, child.gameObject);
                    }
                    else
                    {
                        removed.Add(child.gameObject);
                    }
                }

                foreach (var gameObject in removed)
                {
                    DestroyImmediate(gameObject);
                }
            }
        }

        private void Update()
        {
            ReadObjects(transform);
        }
#endif
    }
}
