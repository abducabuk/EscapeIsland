using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CbkSDK.Util.General
{
    public static class Utils
    {
        #region Randomization

        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        public static bool RandomBool() => UnityEngine.Random.Range(0, 2) == 0;

        public static bool RandomBool(float trueRatio) => UnityEngine.Random.Range(0f, 1f) <= trueRatio;

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T RandomEnum<T>() where T : Enum
        {
            var enums = Enum.GetValues(typeof(T));
            int random = UnityEngine.Random.Range(0, enums.Length);
            return (T)enums.GetValue(random);
        }

        public static Vector3 RandomNormalizedVector(bool randX = true, bool randY = true, bool randZ = true)
        {
            var randVec = new Vector3(
                randX ? UnityEngine.Random.Range(-1f, 1f) : 0f,
                randY ? UnityEngine.Random.Range(-1f, 1f) : 0f,
                randZ ? UnityEngine.Random.Range(-1f, 1f) : 0f);
            return randVec.normalized;
        }

        #endregion

        #region List Operations

        public static T MinItem<T>(this IEnumerable<T> enumerable, Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0)
                return default(T);
            else
            {
                var min = list[0];
                for (int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(min)) < 0)
                    {
                        min = list[i];
                    }
                }

                return min;
            }
        }

        public static T MaxItem<T>(this IEnumerable<T> enumerable, Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0)
                return default(T);
            else
            {
                var max = list[0];
                for (int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(max)) > 0)
                    {
                        max = list[i];
                    }
                }

                return max;
            }
        }

        public static int MinItemIndex<T>(this IEnumerable<T> enumerable, Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0)
                return -1;
            else
            {
                var minIndex = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(list[minIndex])) < 0)
                    {
                        minIndex = i;
                    }
                }

                return minIndex;
            }
        }

        public static int MaxItemIndex<T>(this IEnumerable<T> enumerable, Func<T, IComparable> compareFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count == 0)
                return -1;
            else
            {
                var maxIndex = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    if (compareFunc.Invoke(list[i]).CompareTo(compareFunc.Invoke(list[maxIndex])) > 0)
                    {
                        maxIndex = i;
                    }
                }

                return maxIndex;
            }
        }

        public static List<T> Clamp<T>(this List<T> list, T min, T max) where T : IComparable<T>
        {
            for (var i = 0; i < list.Count; i++)
            {
                var val = list[i];
                if (val.CompareTo(min) < 0) val = min;
                else if (val.CompareTo(max) > 0) val = max;
                list[i] = val;
            }

            return list;
        }

        #endregion

        #region Other

        public static float HitDirection(Vector3 hitFwd, Vector3 targetDir, Vector3 up)
        {
            return Math.Sign(Vector3.Dot(Vector3.Cross(hitFwd, targetDir), up));
        }

        public static bool IsThereAnyUIObject(this EventSystem eventSystem, Vector2 screenPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;
            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static string GetFloatingText(float n, int maxFloatCount = 1, bool isPositivePrefix = false)
        {
            var format = "0";
            if (maxFloatCount > 0) format = "0.";
            for (var i = 0; i < maxFloatCount; i++)
            {
                format += "#";
            }

            var str = n.ToString(format);
            if (isPositivePrefix && n > 0) str = "+" + str;
            return str;
        }

        public static int GetDigitOfPI(int index)
        {
            const string pi =
                "3141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273724587006606315588174881520920962829254091715364367892590360011330530548820466521384146951941511609433057270365759591953092186117381932611793105118548074462379962749567351885752724891227938183011949129833673362440656643086021394946395224737190702179860943702770539217176293176752384674818467669405132000568127145263560827785771342757789609173637178721468440";
            var c = pi[index % pi.Length];
            var d = c - '0';
            return d;
        }
        #endregion

        #region Monobehaviour Extensions

        public static Coroutine StartWaitForSecondCoroutine(this MonoBehaviour monob, float seconds, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(Utils) + "." + nameof(StartWaitForSecondCoroutine) + "() " +
                                                nameof(action) + " is null");
            }

            return monob.StartCoroutine(WaitForSecondCoroutine(seconds, action));
        }

        public static Coroutine StartWaitForEndOfFrameCoroutine(this MonoBehaviour monob, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(Utils) + "." + nameof(StartWaitForEndOfFrameCoroutine) + "() " +
                                                nameof(action) + " is null");
            }

            return monob.StartCoroutine(WaitForEndOfFrameCoroutine(action));
        }

        public static Coroutine StartWaitUntilCoroutine(this MonoBehaviour monob, Func<bool> predicate, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(Utils) + "." + nameof(StartWaitUntilCoroutine) + "() " +
                                                nameof(action) + " is null");
            }

            return monob.StartCoroutine(WaitUntilCoroutine(predicate, action));
        }

        public static Coroutine StartWhileCoroutine(this MonoBehaviour monob, Func<bool> predicate, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(Utils) + "." + nameof(StartWhileCoroutine) + "() " +
                                                nameof(action) + " is null");
            }

            return monob.StartCoroutine(WaitWhileCoroutine(predicate, action));
        }


        private static IEnumerator WaitForSecondCoroutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }

        private static IEnumerator WaitForEndOfFrameCoroutine(Action action)
        {
            yield return new WaitForEndOfFrame();
            action?.Invoke();
        }

        private static IEnumerator WaitUntilCoroutine(Func<bool> predicate, Action action)
        {
            yield return new WaitUntil(predicate);
            action?.Invoke();
        }

        private static IEnumerator WaitWhileCoroutine(Func<bool> predicate, Action action)
        {
            yield return new WaitWhile(predicate);
            action?.Invoke();
        }

        #endregion
    }
}