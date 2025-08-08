#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // target은 SkillData 타입임
        SkillData skillData = (SkillData)target;

        // 기본적으로 skillType 먼저 표시
        skillData.skillType = (SkillType)EditorGUILayout.EnumPopup("Skill Type", skillData.skillType);

        // 스킬 타입에 따라 해당 enum만 노출
        switch (skillData.skillType)
        {
            case SkillType.PassiveSkill:
                skillData.passiveSkillList = (PassiveSkillList)EditorGUILayout.EnumPopup("Passive Skill", skillData.passiveSkillList);
                break;
            case SkillType.NormalSkill:
                skillData.normalSkillList = (NormalSkillList)EditorGUILayout.EnumPopup("Normal Skill", skillData.normalSkillList);
                break;
            case SkillType.UltimateSkill:
                skillData.ultimateSkillList = (UltimateSkillList)EditorGUILayout.EnumPopup("Ultimate Skill", skillData.ultimateSkillList);
                break;
        }

        // 나머지 공통 필드들 표시
        skillData.skillName = EditorGUILayout.TextField("Skill Name", skillData.skillName);
        skillData.skillPower = EditorGUILayout.FloatField("Skill Power", skillData.skillPower);
        skillData.skillEffectPrefab = (GameObject)EditorGUILayout.ObjectField("Skill Effect Prefab", skillData.skillEffectPrefab, typeof(GameObject), false);

        skillData.targetScope = (TargetScope)EditorGUILayout.EnumPopup("Target Scope", skillData.targetScope);
        skillData.targetCount = EditorGUILayout.IntField("Target Count", skillData.targetCount);
        skillData.isTargetingDeadMonster = EditorGUILayout.Toggle("Target Dead Monster", skillData.isTargetingDeadMonster);
        
        skillData.icon = (Sprite)EditorGUILayout.ObjectField("Icon", skillData.icon, typeof(Sprite), false);
        skillData.upgradeIcon = (Sprite)EditorGUILayout.ObjectField("Upgrade Icon", skillData.upgradeIcon, typeof(Sprite), false);

        skillData.damageSound = (AudioData)EditorGUILayout.ObjectField("Damage Sound", skillData.damageSound, typeof(AudioClip), false);
        
        EditorGUILayout.LabelField("Description");
        skillData.description = EditorGUILayout.TextArea(skillData.description);

        EditorGUILayout.LabelField("Upgrade Description");
        skillData.upgradeDescription = EditorGUILayout.TextArea(skillData.upgradeDescription);

        // 저장 상태로 표시
        if (GUI.changed)
        {
            EditorUtility.SetDirty(skillData);
        }
    }
}
#endif
