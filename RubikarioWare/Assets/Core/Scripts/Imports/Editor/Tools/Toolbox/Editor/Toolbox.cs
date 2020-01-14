using UnityEditor;
using UnityEngine;

namespace Game
{
	public static class Toolbox
    {
		private const string GameObjectFolder = "GameObject/";
		private const string Folder = GameObjectFolder + "Create Custom/";
		private const int Order = -1;

		private const string graphName = "Graph";
		private const string physicsName = "Physics";

		private const string UndoName = "Created Custom GameObject";

		[MenuItem(Folder + nameof(Empty), false, Order)]
        static void Empty()
        {
			GameObject gameObject = new GameObject(nameof(Empty));
			GameObject graph = new GameObject(graphName);
			GameObject physics = new GameObject(physicsName);

			gameObject.transform.SetParent(Selection.activeTransform);
			graph.transform.SetParent(gameObject.transform);
			physics.transform.SetParent(gameObject.transform);

			Undo.RegisterCreatedObjectUndo(gameObject, UndoName);
		}

		[MenuItem(Folder + nameof(Cube), false, Order)]
		static void Cube() => Create(PrimitiveType.Cube);

		[MenuItem(Folder + nameof(Sphere), false, Order)]
		static void Sphere() => Create(PrimitiveType.Sphere);

		[MenuItem(Folder + nameof(Capsule), false, Order)]
		static void Capsule() => Create(PrimitiveType.Capsule);

		[MenuItem(Folder + nameof(Cylinder), false, Order)]
		static void Cylinder() => Create(PrimitiveType.Cylinder);

		[MenuItem(Folder + nameof(Plane), false, Order)]
		static void Plane() => Create(PrimitiveType.Plane);

		[MenuItem(Folder + nameof(Quad), false, Order)]
		static void Quad() => Create(PrimitiveType.Quad);

		[MenuItem(Folder + nameof(Sprite), false, Order)]
		static void Sprite()
		{
			GameObject gameObject = new GameObject(nameof(Sprite));
			GameObject graph = new GameObject(graphName);
			GameObject physics = new GameObject(physicsName);

			graph.AddComponent<SpriteRenderer>();
			physics.AddComponent<CircleCollider2D>();

			gameObject.transform.SetParent(Selection.activeTransform);
			graph.transform.SetParent(gameObject.transform);
			physics.transform.SetParent(gameObject.transform);

			Undo.RegisterCreatedObjectUndo(gameObject, UndoName);
		}

		static void Create(PrimitiveType type)
		{
			GameObject gameObject = GameObject.CreatePrimitive(type);
			GameObject graph = new GameObject(graphName);
			GameObject physics = new GameObject(physicsName);

			var meshFilter = gameObject.GetComponent<MeshFilter>();
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			var collider = gameObject.GetComponent<Collider>();

			gameObject.transform.SetParent(Selection.activeTransform);

			graph.transform.SetParent(gameObject.transform);
			graph.AddComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
			graph.AddComponent<MeshRenderer>().sharedMaterial = meshRenderer.sharedMaterial;

			physics.transform.SetParent(gameObject.transform);
			physics.AddComponent(collider.GetType());

			Object.DestroyImmediate(meshFilter);
			Object.DestroyImmediate(meshRenderer);
			Object.DestroyImmediate(collider);

			Undo.RegisterCreatedObjectUndo(gameObject, UndoName);
		}
	}
}