using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StatusEffect))]
public class StatusEffectEditor : Editor 
{
	StatusEffect mTarget;

	void OnEnable()
	{
		mTarget = (StatusEffect)target;
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical("Box");	
			mTarget.DotDamage = EditorGUILayout.FloatField("Normal DoT Damage", mTarget.DotDamage);
			mTarget.DotDuration = EditorGUILayout.FloatField("Normal DoT Duration", mTarget.DotDuration) ;
			mTarget.DotInterval = EditorGUILayout.FloatField("Normal DoT Interval", mTarget.DotInterval);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Box");
			mTarget.SlowRate = EditorGUILayout.FloatField("Slow Rate", mTarget.SlowRate);
			mTarget.SlowDuration = EditorGUILayout.FloatField("Slow Duration", mTarget.SlowDuration);
			mTarget.SlowStackLimit = EditorGUILayout.FloatField("Max Slow Stack", mTarget.SlowStackLimit);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Box");
			mTarget.StunDamage = EditorGUILayout.FloatField("Stun Damage", mTarget.StunDamage);
			mTarget.StunTime = EditorGUILayout.FloatField("Stun Time", mTarget.StunTime);
			mTarget.StunInterval = EditorGUILayout.FloatField("Stun Interval", mTarget.StunInterval);
			mTarget.StunMaxCount = EditorGUILayout.FloatField("Max Stun Count", mTarget.StunMaxCount);
			mTarget.stunPrefab = (GameObject)EditorGUILayout.ObjectField("Stun prefab: ", mTarget.stunPrefab, typeof(GameObject), false);			
			
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Box");
			mTarget.DoubleDotDamagePercentage = EditorGUILayout.FloatField("Double DoT Damage", mTarget.DoubleDotDamagePercentage);
			mTarget.DoubleDotDuration = EditorGUILayout.FloatField("Double DoT Duration", mTarget.DoubleDotDuration);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Box");
			mTarget.DisablerDotDamage = EditorGUILayout.FloatField("Disabler DoT Damage", mTarget.DisablerDotDamage);
			mTarget.DisablerDotDuration = EditorGUILayout.FloatField("Disabler DoT Duration", mTarget.DisablerDotDuration);
			mTarget.DisablerDotInterval = EditorGUILayout.FloatField("Disabler DoT Interval", mTarget.DisablerDotInterval);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Box");
			mTarget.DoubleAoeDamage = EditorGUILayout.FloatField("Double AoE Damage", mTarget.DoubleAoeDamage);
			mTarget.DoubleAoeDuration = EditorGUILayout.FloatField("Double AoE Duration", mTarget.DoubleAoeDuration);
			mTarget.DoubleAoeRadius = EditorGUILayout.FloatField("Double AoE Radius", mTarget.DoubleAoeRadius);
		EditorGUILayout.EndVertical();
	}
}
