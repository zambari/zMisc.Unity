using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;
using Random = UnityEngine.Random;

using Swing.Editor;

	public class TestEditorCoroutine
	{
		[MenuItem("Swing/Test/Test Editor Coroutine")]
		static void testEditorCoroutine()
		{
			EditorCoroutine.start(testRoutine());
		}
		static IEnumerator testRoutine()
		{
			Debug.Log("hello " + DateTime.Now.Ticks);
			yield return null;
			Debug.Log("done " + DateTime.Now.Ticks);
		}

		[MenuItem("Swing/Test/Test Editor Coroutine With Exception")]
		static void testEditorCoroutineWithException()
		{
			EditorCoroutine.start(testRoutineWithException());
		}
		static IEnumerator testRoutineWithException()
		{
			Debug.Log("hello " + DateTime.Now.Ticks);
			yield return null;

			for (int i = 0; i < 10; i++)
			{
				testRandomException();
				yield return null;
			}

			Debug.Log("done " + DateTime.Now.Ticks);
		}
		static void testRandomException()
		{
			if (Random.value < 0.3f)
			{
				throw new Exception("ahah! " + DateTime.Now.Ticks);
			}
			else
			{
				Debug.Log("ok " + DateTime.Now.Ticks);
			}
		}
	}

