using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.SplitHairSliders.Plugin.Core
{
    class SplitHairSlidersHooks
    {

        private static bool initialized;
        private static HarmonyLib.Harmony instance;

        public static void Initialize()
        {
            //Copied from examples
            if (SplitHairSlidersHooks.initialized)
                return;

            SplitHairSlidersHooks.instance = HarmonyLib.Harmony.CreateAndPatchAll(typeof(SplitHairSlidersHooks), "org.guest4168.splithairslidersplugin.hooks.base");
            SplitHairSlidersHooks.initialized = true;


            UnityEngine.Debug.Log("Split Hair Sliders: Hooks Initialize");
        }

        //[HarmonyPatch(typeof(Menu), nameof(Menu.ProcScript), new Type[] { typeof(Maid), typeof(MaidProp), typeof(bool), typeof(SubProp) })]
        //[HarmonyPrefix]
        //static void Menu_ProcScript_Prefix(Maid maid, MaidProp mp, bool f_bTemp = false, SubProp f_SubProp = null)
        //{
        //    string str1 = !f_bTemp ? mp.strFileName : mp.strTempFileName;
        //    if (str1.IndexOf("mod_") != 0)
        //    {
        //        Console.WriteLine("Checking Menu File " + str1);
        //        createNewHairMenu(str1);
        //    }

        
        //}

        //[HarmonyPatch(typeof(TBodySkin.HairLengthCtrl), nameof(TBodySkin.HairLengthCtrl.SearchAndAddHairLengthTarget), new Type[] { typeof(string), typeof(string), typeof(string), typeof(Vector3), typeof(Vector3) })]
        //[HarmonyPrefix]
        //static bool SearchAndAddHairLengthTarget(string f_strGroupName, string f_strBoneSearchType, string f_strBoneName, Vector3 f_vScaleMin, Vector3 f_vScaleMax, TBodySkin.HairLengthCtrl __instance)
        //{
        //    TBodySkin m_tbskin = (TBodySkin)typeof(TBodySkin.HairLengthCtrl).GetField("m_tbskin", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        //    Dictionary<string, TBodySkin.HairLengthCtrl.HairLength> m_dicHairLenght = (Dictionary<string, TBodySkin.HairLengthCtrl.HairLength>)typeof(TBodySkin.HairLengthCtrl).GetField("m_dicHairLenght", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        //    Maid m_maid = (Maid)typeof(TBodySkin.HairLengthCtrl).GetField("m_maid", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

        //    if (m_tbskin == null || m_tbskin.obj_tr == null)
        //        NDebug.Assert("髪ボーン拡縮検索 親が未だありません。", false);
        //    string pattern = f_strBoneName.Replace("*", ".*");

        //    int mode = 0; // TBodySkin.HairLengthCtrl.SerchMode.ALL;
        //    if (f_strBoneSearchType == "fbrother")
        //        mode = 1; //TBodySkin.HairLengthCtrl.SerchMode.FIRST_BROTHER;
        //    else if (f_strBoneSearchType == "fchild")
        //        mode = 2; // TBodySkin.HairLengthCtrl.SerchMode.FIRST_CHILD;
        //    else if (f_strBoneSearchType == "all")
        //        mode = 0; //TBodySkin.HairLengthCtrl.SerchMode.ALL;
        //    else
        //        NDebug.Assert("髪ボーン検索タイプが不正です。 " + f_strBoneSearchType, false);

        //    //CUSTOM CODE INJECTED
        //    while (m_dicHairLenght.ContainsKey(f_strGroupName))
        //    {
        //        f_strGroupName += "_";
        //    }
        //    //CUSTOM CODE END

        //    SearchObjAndAdd(f_strGroupName, m_tbskin.obj_tr, new System.Text.RegularExpressions.Regex(pattern), mode, f_vScaleMin, f_vScaleMax, __instance);
        //    TBodySkin.HairLengthCtrl.HairLength hairLength;
        //    if (!m_dicHairLenght.TryGetValue(f_strGroupName, out hairLength))
        //    {
        //        UnityEngine.Debug.LogError((object)"髪ボーングループがありません。");
        //    }
        //    else
        //    {
        //        float f_fOutValue = 0.5f;
        //        if (m_maid.GetHairLengthFromMP(m_tbskin.m_ParentMPN, m_tbskin.SlotId, f_strGroupName, out f_fOutValue))
        //            hairLength.SetLengthRate(f_fOutValue);
        //        else
        //            m_maid.ClearHairLengthMP(m_tbskin.m_ParentMPN, m_tbskin.SlotId);
        //        __instance.HairLenghtBlend();
        //    }

        //    return false;
        //}

        //private static Transform SearchObjAndAdd(string f_strGroupName, Transform t, System.Text.RegularExpressions.Regex regex, int mode, Vector3 f_vScaleMin, Vector3 f_vScaleMax, TBodySkin.HairLengthCtrl __instance)
        //{
        //    TBodySkin m_tbskin = (TBodySkin)typeof(TBodySkin.HairLengthCtrl).GetField("m_tbskin", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        //    Dictionary<string, TBodySkin.HairLengthCtrl.HairLength> m_dicHairLenght = (Dictionary<string, TBodySkin.HairLengthCtrl.HairLength>)typeof(TBodySkin.HairLengthCtrl).GetField("m_dicHairLenght", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

        //    string name = t.name;
        //    if (regex.IsMatch(name))
        //    {
        //        TBodySkin.HairLengthCtrl.HairLength hairLength;

        //        if (!m_dicHairLenght.TryGetValue(f_strGroupName, out hairLength))
        //        {
        //            hairLength = new TBodySkin.HairLengthCtrl.HairLength(m_tbskin, f_strGroupName);
        //            m_dicHairLenght[f_strGroupName] = hairLength;
        //        }
        //        hairLength.listTarget.Add(new TBodySkin.HairLengthCtrl.HairLengthTarget()
        //        {
        //            trTarget = t,
        //            vScaleDef = t.localScale,
        //            vScaleMin = f_vScaleMin,
        //            vScaleMax = f_vScaleMax
        //        });
        //        if (mode == 1 || mode == 2)
        //            return t;
        //    }
        //    for (int index = 0; index < t.childCount; ++index)
        //    {
        //        Transform child = t.GetChild(index);
        //        Transform transform = SearchObjAndAdd(f_strGroupName, child, regex, mode, f_vScaleMin, f_vScaleMax, __instance);
        //        if (transform != null && mode == 2)
        //            return transform;
        //    }
        //    return (Transform)null;
        //}
    }

    public class MenuObj
    {
        public string category { get; set; }
        public MenuHeaderObj header { get; set; }
        public List<MenuCommandObj> commands { get; set; }

        public MenuObj()
        {
            category = "";
            header = new MenuHeaderObj();
            commands = new List<MenuCommandObj>();
        }
    }
    public class MenuHeaderObj
    {
        public string CM3D2_MENU { get; set; }
        public int temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public int temp6 { get; set; }

        public MenuHeaderObj()
        {

        }
    }
    public class MenuCommandObj
    {
        public byte byt { get; set; }
        public List<string> nativeStrings { get; set; }

        public string stringCom { get; set; }
        public string[] stringList { get; set; }

        public MenuCommandObj()
        {
            nativeStrings = new List<string>();
        }
    }
    
    public class HairLengthTargetObj
    {
        public string trTarget { get; set; }
        public Vector3 vScaleMin { get; set; }
        public Vector3 vScaleMax { get; set; }

        public HairLengthTargetObj()
        {
            trTarget = "";
            vScaleMin = new Vector3(0.3f, 1f, 1f);
            vScaleMax = new Vector3(2f, 1f, 1f);
        }
    }
}
