using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SpawnManager))]
public class SpawnManagerEditor : Editor
{
	SpawnManager mTarget;

	void OnEnable()
	{
		mTarget = (SpawnManager)target;
	}

	public override void OnInspectorGUI()
	{
		foreach (SpawnManager.Levels level in mTarget.Level)
		{
			EditorGUILayout.BeginVertical("Box");
			
			EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("X", GUILayout.Width(30.0f)))
				{
					mTarget.Level.Remove(level);
					return;
				}
				level.Name = EditorGUILayout.TextField("Level name", level.Name);
			EditorGUILayout.EndHorizontal();
			
			mTarget.preparationTime = EditorGUILayout.Toggle("Preparation Time: ", mTarget.preparationTime);
			
			for (int i = 0; i < level.Wave.Count; ++i)
			{
				SpawnManager.Waves wave = level.Wave[i];
				
				BeginIndent();
					EditorGUILayout.BeginHorizontal();
						if(GUILayout.Button("X", GUILayout.Width(30.0f)))
						{
							level.Wave.Remove(wave);
							return;
						}
						GUILayout.Label("Wave " + (i + 1));
					EditorGUILayout.EndHorizontal();
					
						wave.waveStart = EditorGUILayout.Toggle("Wave Start: ", wave.waveStart);
												
						for (int j = 0; j < wave.Group.Count; ++j)
						{
							SpawnManager.Groups group = wave.Group[j];
							BeginIndent();
					
								EditorGUILayout.BeginHorizontal();
									if(GUILayout.Button("X", GUILayout.Width(30.0f)))
									{
										wave.Group.Remove(group);
										return;
									}
									GUILayout.Label("Group " + (j + 1));
								EditorGUILayout.EndHorizontal();
								
								group.groupSpawnInterval = EditorGUILayout.FloatField("Group Spawn Interval: ", group.groupSpawnInterval);
								
								group.enemyType = (GameObject)EditorGUILayout.ObjectField("Enemy Type: ", group.enemyType, typeof(GameObject), false);			
								group.spawnAmount = EditorGUILayout.IntField("Spawn Amount: ", group.spawnAmount); 
								group.enemySpawnInterval = EditorGUILayout.FloatField("Spawn Interval: ", group.enemySpawnInterval); 
								
					
					
							EndIndent();							
						}
						EditorGUILayout.BeginHorizontal();
							GUILayout.Space(175.0f);
							if(GUILayout.Button("Add Group"))
							{
								wave.Group.Add(new SpawnManager.Groups());
								return;
							}
						EditorGUILayout.EndHorizontal();
				EndIndent();
			}
			
			GUILayout.Space(10.0f);
		
			EditorGUILayout.BeginHorizontal();
				GUILayout.Space(30.0f);
				if (GUILayout.Button("Add Wave"))
				{
					level.Wave.Add(new SpawnManager.Waves());
					return;
				}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(10.0f);
			
			EditorGUILayout.EndVertical();
		}
		
		
		if (GUILayout.Button("Add Level"))
		{
			mTarget.Level.Add(new SpawnManager.Levels());
		}
	}
	
	void BeginIndent()
	{
		EditorGUILayout.BeginHorizontal("Box");
		GUILayout.Space(25.0f);
		EditorGUILayout.BeginVertical();
	}
	
	void EndIndent()
	{
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}
}
