using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Game
{
	public static class ArrayUtility
	{
		/// <summary>
		/// Swap two elements in array
		/// </summary>
		public static void Swap<T>(this IList<T> array, int a, int b)
		{
			T x = array[a];
			array[a] = array[b];
			array[b] = x;
		}


		public static T GetPrevious<T>(this IList<T> array, int index, bool loop = false)
		{
			if (array.IsNull()) throw new Exception("GetPrevious() Error");
			index--;
			if (index < 0)
				if (loop)
					index = array.Count - 1;
				else
					index = 0;
			return array[index];
		}

		public static T GetNext<T>(this IList<T> array, int index, bool loop = false)
		{
			if (array.IsNull()) throw new Exception("GetNext() Error");
			index++;
			if (index >= array.Count)
				if (loop)
					index = 0;
				else
					index = array.Count - 1;
			return array[index];
		}

		/// <summary>
		/// (WARNING: Heavy) Check if all items are the same between list (even if scrambled)
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list1"></param>
		/// <param name="list2"></param>
		/// <returns>if equals</returns>
		public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
		{
			var cnt = new Dictionary<T, int>();
			foreach (T s in list1)
			{
				if (cnt.ContainsKey(s))
					cnt[s]++;
				else
					cnt.Add(s, 1);
			}
			foreach (T s in list2)
			{
				if (cnt.ContainsKey(s))
					cnt[s]--;
				else
					return false;
			}
			return cnt.Values.All(c => c == 0);
		}

		public static bool IsNull<T>(this IList<T> list)
		{
			if (list == null || list.Count == 0) return true;
			return false;
		}

		public static T RandomItem<T>(this IList<T> list, int maxIndex = -1)
		{
			if (list.IsNull()) throw new Exception("RandomItem() Error");
			if (maxIndex < 0) return list[Random.Range(0, list.Count)];
			return list[Random.Range(0, Math.Min(maxIndex, list.Count))];
		}

		public static IList<T> RandomItems<T>(this IList<T> list, int count)
		{
			if (list.IsNull() || count > list.Count) throw new Exception("RandomItems() Error");
			if (count == list.Count) return list;
			IList<T> items = new List<T>(count);
			while(items.Count < count)
			{
				T item = list.RandomItem();
				if (items.Contains(item))
					continue;
				items.Add(item);
			}
			return items;
		}
	}
}