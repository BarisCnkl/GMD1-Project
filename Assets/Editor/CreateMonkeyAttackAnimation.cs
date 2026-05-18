using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreateMonkeyAttackAnimation
{
    private const string OutputFolder = "Assets/Animations";
    private const string OutputPath = OutputFolder + "/Monkey_Attack.anim";

    [MenuItem("Tools/Monkey/Create Attack Animation Clip")]
    public static void CreateAttackClip()
    {
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }

        AnimationClip clip = new AnimationClip
        {
            name = "Monkey_Attack",
            frameRate = 60f,
            wrapMode = WrapMode.Once
        };

        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = false;
        settings.keepOriginalOrientation = true;
        settings.keepOriginalPositionY = true;
        settings.keepOriginalPositionXZ = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // These paths are relative to the Monkey_B1 object that has the Animator component.
        string spine2 = "RL_BoneRoot/SK_Mesh_Macaque/macaque_Pelvis_bone/macaque_Spine1_bone/macaque_Spine2_bone";
        string spine3 = spine2 + "/macaque_Spine3_bone";
        string neck = spine3 + "/macaque_Neck_bone";
        string head = neck + "/macaque_Head_bone";

        string rScapula = spine3 + "/macaque_r_Scapula_bone";
        string rUpper = rScapula + "/macaque_r_Humerus_bone";
        string rForearm = rUpper + "/macaque_r_Forearm_bone";
        string rHand = rForearm + "/macaque_r_Hand_bone";

        string lScapula = spine3 + "/macaque_l_Scapula_bone";
        string lUpper = lScapula + "/macaque_l_Humerus_bone";
        string lForearm = lUpper + "/macaque_l_Forearm_bone";

        // Simple swipe/punch attack timing in seconds.
        // 0.00 = idle pose, 0.12 = wind-up, 0.25 = hit/swing, 0.45 = return to idle.
        AddEuler(clip, spine2, "z", 0, -6, 10, 0);
        AddEuler(clip, spine3, "x", 0, -8, 12, 0);
        AddEuler(clip, neck, "x", 0, 5, -6, 0);
        AddEuler(clip, head, "x", 0, 4, -8, 0);

        // Right arm does the attack swipe.
        AddEuler(clip, rScapula, "x", 0, -15, 20, 0);
        AddEuler(clip, rScapula, "z", 0, -10, 18, 0);
        AddEuler(clip, rUpper, "x", 0, -65, 55, 0);
        AddEuler(clip, rUpper, "y", 0, 20, -20, 0);
        AddEuler(clip, rUpper, "z", 0, -25, 35, 0);
        AddEuler(clip, rForearm, "x", 0, -55, 35, 0);
        AddEuler(clip, rForearm, "z", 0, 10, -25, 0);
        AddEuler(clip, rHand, "x", 0, 15, -20, 0);

        // Left arm counter balances a bit.
        AddEuler(clip, lScapula, "x", 0, 8, -8, 0);
        AddEuler(clip, lUpper, "x", 0, 20, -10, 0);
        AddEuler(clip, lForearm, "x", 0, 15, -10, 0);

        AssetDatabase.CreateAsset(clip, OutputPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Created monkey attack animation at: " + OutputPath + ". Add this clip as the Motion for your Attack state in EnemyAnimator.");
    }

    private static void AddEuler(AnimationClip clip, string path, string axis, float v0, float v1, float v2, float v3)
    {
        Keyframe[] keys =
        {
            new Keyframe(0.00f, v0),
            new Keyframe(0.12f, v1),
            new Keyframe(0.25f, v2),
            new Keyframe(0.45f, v3)
        };

        AnimationCurve curve = new AnimationCurve(keys);

        for (int i = 0; i < curve.keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Auto);
            AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Auto);
        }

        EditorCurveBinding binding = new EditorCurveBinding
        {
            path = path,
            type = typeof(Transform),
            propertyName = "localEulerAnglesRaw." + axis
        };

        AnimationUtility.SetEditorCurve(clip, binding, curve);
    }
}
