using BepInEx;
using GearMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.SplitHairSliders.Plugin.Core
{
    [BepInPlugin("org.guest4168.plugins.splithairslidersplugin", "Split Hair Slider Plug-In", "1.0.0.0")]
    [BepInDependency("org.bepinex.plugins.unityinjectorloader", BepInDependency.DependencyFlags.SoftDependency)]
    class SplitHairSliders : BaseUnityPlugin
    {
        private UnityEngine.GameObject managerObject;
        private UnityEngine.GameObject menuButton;

        private static string newMenuPath = UTY.gameProjectPath + "\\Mod\\[SplitHairSliders]";

        private static bool inEdit = false;
        private static bool displayUI = false;
        private static string status = "";
        private static bool overwriteExisting = false;
        private static bool everyBone = false;
        private static bool includeMods = false;
        private static bool onlyMods = false;

        public void Awake()
        {
            //Copied from examples
            UnityEngine.Debug.Log("Split Hair Sliders: Awake");
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)this);
            this.managerObject = new UnityEngine.GameObject("splithairslidersManager");
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)this.managerObject);
            this.managerObject.AddComponent<SplitHairSlidersManager>().Initialize();
        }

        #region GearMenu
        

        static byte[] png = null;

        private static string gearIconBase64 = "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";
        public static byte[] GearIcon
        {
            get
            {
                if (png == null)
                {
                    png = Convert.FromBase64String(gearIconBase64);
                }
                return png;
            }
        }

        private void OnMenuButtonClickCallback(GameObject gearMenuButton)
        {
            //Open/Close the UI
            displayUI = !displayUI;
        }

        #endregion



        #region CORE
        public void OnLevelWasLoaded(int level)
        {
            inEdit = false;

            if (level == 5)
            {
                //Add the button
                if (GameMain.Instance != null && GameMain.Instance.SysShortcut != null && !Buttons.Contains("YotogiAnywhere"))
                {
                    menuButton = GearMenu.Buttons.Add("SplitHairSliders", "Split Hair Sliders Edit Only", GearIcon, OnMenuButtonClickCallback);
                }

                inEdit = true;
            }

            //Hide the UI when switching levels
            displayUI = false;
        }

        private void Update()
        {
            //If the button was pressed outside of Yotogi, disable UI
            if (!inEdit)
            {
                displayUI = false;
            }
        }


        public static void exportAllHairMenus()
        {
            //Create Directory
            if (!Directory.Exists(newMenuPath))
            {
                Directory.CreateDirectory(newMenuPath);
            }

            string[] fileNames;

            //COM
            UnityEngine.Debug.Log("Exporting COM Menus Start");
            fileNames = GameUty.FileSystem.GetFileListAtExtension(".menu");
            exportAllHairMenus(fileNames, false);
            UnityEngine.Debug.Log("Exporting COM Menus End");

            //CM
            UnityEngine.Debug.Log("Exporting CM Menus Start");
            fileNames = GameUty.FileSystemOld.GetFileListAtExtension(".menu");
            exportAllHairMenus(fileNames, false);
            UnityEngine.Debug.Log("Exporting CM Menus End");

            //Mods
            if (includeMods)
            {
                UnityEngine.Debug.Log("Exporting MOD Menus Start");
                fileNames = GameUty.FileSystemMod.GetFileListAtExtension(".menu");
                exportAllHairMenus(fileNames, true);
                UnityEngine.Debug.Log("Exporting MOD Menus End");
            }

            refreshModFolder();
        }

        private static void exportAllHairMenus(string[] fileNames, bool isModList)
        {
            Maid maid = GameMain.Instance.CharacterMgr.GetMaid(0);
            if (maid != null)
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    string fileNameFullPath = fileNames[i];
                    bool isMod = isModList || fileNameFullPath.IndexOf("mod_") == 0;
                    if ( ((isMod && includeMods) || (!isMod && !onlyMods)) && 
                        (fileNameFullPath.Contains("hairf") || fileNameFullPath.Contains("hairr") || fileNameFullPath.Contains("hairs") || fileNameFullPath.Contains("hairt") ))
                    {
                        string[] filePathSplit = fileNameFullPath.Split('\\');
                        string fileName = filePathSplit[filePathSplit.Length - 1];

                        if (!File.Exists(Path.Combine(newMenuPath, fileName)) || overwriteExisting)
                        {
                            MenuObj menu = getMenu(fileNameFullPath);

                            //Only process the file if it is a hair
                            if (new string[] { "hairf", "hairr", "hairs", "hairt" }.Contains(menu.category.ToLower()))
                            {
                                
                                TBody.SlotID slotID = TBody.SlotID.hairAho;
                                MPN mpnID = MPN.hairaho;
                                switch (menu.category.ToLower())
                                {
                                    case "hairf":
                                        slotID = TBody.SlotID.hairF;
                                        mpnID = MPN.hairf;
                                        break;
                                    case "hairr":
                                        slotID = TBody.SlotID.hairR;
                                        mpnID = MPN.hairr;
                                        break;
                                    case "hairs":
                                        slotID = TBody.SlotID.hairS;
                                        mpnID = MPN.hairs;
                                        break;
                                    case "hairt":
                                        slotID = TBody.SlotID.hairT;
                                        mpnID = MPN.hairt;
                                        break;
                                }

                                //Cache the current selection
                                string oldFileName = maid.GetProp(mpnID).strFileName;

                                //Set the hair to the file
                                Menu.ProcScript(maid, fileNameFullPath, false);

                                //Export the hair
                                if (everyBone && maid.body0.goSlot[(int)slotID] != null && maid.body0.goSlot[(int)slotID].morph != null && maid.body0.goSlot[(int)slotID].morph.BoneNames != null && maid.body0.goSlot[(int)slotID].morph.BoneNames.Count > 0)
                                {
                                    UnityEngine.Debug.Log("Creating Menu for all bones");
                                    List<HairLengthTargetObj> listTarget = new List<HairLengthTargetObj>();
                                    for (int j = 0; j < maid.body0.goSlot[(int)slotID].morph.BoneNames.Count; j++)
                                    {
                                        string boneName = maid.body0.goSlot[(int)slotID].morph.BoneNames[j];
                                        if (boneName.Contains("yure"))
                                        {
                                            HairLengthTargetObj target = new HairLengthTargetObj();
                                            target.trTarget = boneName;
                                            target.vScaleMin = new Vector3(0.3f, 1f, 1f);
                                            target.vScaleMax = new Vector3(2f, 1f, 1f);
                                            listTarget.Add(target);
                                        }
                                    }

                                    createNewHairMenu(fileName, listTarget, menu, false);
                                }
                                else
                                {
                                    if (slotID != TBody.SlotID.hairAho &&
                                    maid.body0.goSlot[(int)slotID] != null &&
                                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl != null && maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList != null &&
                                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList.ContainsKey("AutoConv") &&
                                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget != null &&
                                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget.Count != 0)
                                    {
                                        //CM
                                        UnityEngine.Debug.Log("Createing Menu for AutoConv");
                                        List<HairLengthTargetObj> listTarget = new List<HairLengthTargetObj>();
                                        for (int j = 0; j < maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget.Count; j++)
                                        {
                                            TBodySkin.HairLengthCtrl.HairLengthTarget orig = maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget[j];
                                            HairLengthTargetObj target = new HairLengthTargetObj();
                                            target.trTarget = orig.trTarget.name;
                                            target.vScaleMin = orig.vScaleMin;
                                            target.vScaleMax = orig.vScaleMax;
                                            listTarget.Add(target);
                                        }
                                        createNewHairMenu(fileName, listTarget, menu, true);
                                    }
                                    else
                                    {
                                        //COM
                                        UnityEngine.Debug.Log("Creating Menu for standard");
                                        createNewHairMenu(fileName, null, menu, true);
                                    }
                                }

                                //Revert to the old selection
                                Menu.ProcScript(maid, oldFileName, false);
                            }
                        }
                    }
                }
            }
        }
        private static MenuObj getMenu(string menuFileName)
        {
            UnityEngine.Debug.Log("Fetching Menu " + menuFileName);

            MenuObj menu = new MenuObj();
            byte[] cd;
            using (AFileBase afileBase = GameUty.FileOpen(menuFileName, (AFileSystemBase)null))
            {
                if (afileBase != null && afileBase.IsValid())
                {
                    UnityEngine.Debug.Log("Reading bytes");

                    cd = afileBase.ReadAll();

                    using (BinaryReader binaryReader = new BinaryReader((Stream)new MemoryStream(cd), Encoding.UTF8))
                    {
                        //Useless header info???
                        UnityEngine.Debug.Log("Reading header");
                        menu.header.CM3D2_MENU = binaryReader.ReadString();
                        menu.header.temp1 = binaryReader.ReadInt32();
                        menu.header.temp2 = binaryReader.ReadString();
                        menu.header.temp3 = binaryReader.ReadString();
                        menu.header.temp4 = binaryReader.ReadString();
                        menu.header.temp5 = binaryReader.ReadString();
                        menu.header.temp6 = binaryReader.ReadInt32();

                        bool end = false;

                        UnityEngine.Debug.Log("Reading commands");
                        //Blocks
                        do
                        {
                            MenuCommandObj command = new MenuCommandObj();
                            string str4;
                            do
                            {
                                byte byt = binaryReader.ReadByte();
                                command.byt = byt;
                                int num2 = (int)byt;

                                str4 = string.Empty;
                                if (num2 != 0)
                                {
                                    for (int index = 0; index < num2; ++index)
                                    {
                                        string str = binaryReader.ReadString();
                                        command.nativeStrings.Add(str);
                                        str4 = str4 + "\"" + str + "\" ";
                                    }
                                }
                                else
                                {
                                    end = true;
                                }
                            }
                            while (str4 == string.Empty && !end);

                            if (!end)
                            {
                                string stringCom = UTY.GetStringCom(str4);
                                string[] stringList = UTY.GetStringList(str4);

                                command.stringCom = stringCom;
                                command.stringList = stringList;
                                menu.commands.Add(command);

                                if (stringCom.Equals("category"))
                                {
                                    //Get the category
                                    menu.category = stringList[1];
                                }
                            }
                        }
                        while (!end);
                    }
                }
                else
                {
                    if (afileBase == null)
                    {
                        UnityEngine.Debug.Log("Split Hair Sliders: null AFileBase");
                    }
                    else
                    {
                        if (!afileBase.IsValid())
                        {
                            UnityEngine.Debug.Log("Split Hair Sliders: invalid AFileBase");
                        }
                    }

                }
            }

            return menu;
        }

        public static void createNewHairMenu(string hairMenuFileName, List<HairLengthTargetObj> listTarget, MenuObj menu, bool useExistingLengths)
        {
            //Delete Existing
            if (File.Exists(Path.Combine(newMenuPath, hairMenuFileName)))
            {
                UnityEngine.Debug.Log("Deleting old Menu");
                File.Delete(Path.Combine(newMenuPath, hairMenuFileName));
            }

            //Write new
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(Path.Combine(newMenuPath, "Ext_" + hairMenuFileName), FileMode.OpenOrCreate)))
            {
                List<string> sliderNames = new List<string>();

                //Write the header
                binaryWriter.Write(menu.header.CM3D2_MENU);
                binaryWriter.Write(menu.header.temp1);
                binaryWriter.Write(menu.header.temp2);
                binaryWriter.Write(menu.header.temp3);
                binaryWriter.Write(menu.header.temp4);
                binaryWriter.Write(menu.header.temp5);
                binaryWriter.Write(menu.header.temp6);

                //Write the commands
                for (int i = 0; i < menu.commands.Count; i++)
                {
                    MenuCommandObj command = menu.commands[i];                    

                    //Special case for the length sliders - need to adjust the data
                    if (command.stringCom.Equals("length"))
                    {
                        if (useExistingLengths)
                        {
                            //Write the byte indicating size of params
                            binaryWriter.Write(command.byt);

                            string sliderName = command.stringList[2];
                            while (sliderNames.Contains(sliderName))
                            {
                                sliderName += "_";
                            }
                            sliderNames.Add(sliderName);

                            //Write out commands
                            for (int j = 0; j < command.nativeStrings.Count; j++)
                            {
                                if (j != 2)
                                {
                                    //Normal Param
                                    binaryWriter.Write(command.nativeStrings[j]);
                                }
                                else
                                {
                                    //Slider
                                    binaryWriter.Write(sliderName);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Write the byte indicating size of params
                        binaryWriter.Write(command.byt);

                        //Write out normal commands
                        for (int j = 0; j < command.nativeStrings.Count; j++)
                        {
                            binaryWriter.Write(command.nativeStrings[j]);
                        }
                    }
                }

                //Add the hair length options
                if (listTarget != null)
                {
                    for (int i = 0; i < listTarget.Count; i++)
                    {
                        binaryWriter.Write(Convert.ToByte(11));
                        binaryWriter.Write("length");
                        binaryWriter.Write(menu.category);
                        binaryWriter.Write("髪" + i);
                        binaryWriter.Write("fbrother");
                        binaryWriter.Write(listTarget[i].trTarget);
                        binaryWriter.Write(listTarget[i].vScaleMin.x.ToString("N1"));
                        binaryWriter.Write(listTarget[i].vScaleMin.y.ToString("N1"));
                        binaryWriter.Write(listTarget[i].vScaleMin.z.ToString("N1"));
                        binaryWriter.Write(listTarget[i].vScaleMax.x.ToString("N1"));
                        binaryWriter.Write(listTarget[i].vScaleMax.y.ToString("N1"));
                        binaryWriter.Write(listTarget[i].vScaleMax.z.ToString("N1"));
                    }
                }

                //Write the end
                binaryWriter.Write(Convert.ToByte('\u0000'));
            }

            //Add the new menu to the file system
            UnityEngine.Debug.Log("Adding new Menu to MOD file system: Ext_" + hairMenuFileName);
            GameUty.FileSystemMod.FileOpen(Path.Combine(newMenuPath, "Ext_" + hairMenuFileName));
        }

        private static void refreshModFolder()
        {
            if (Directory.Exists(UTY.gameProjectPath + "\\Mod"))
            {
                ((FileSystemWindows)GameUty.FileSystemMod).AddFolder("Mod\\[SplitHairSliders]");
                ((FileSystemWindows)GameUty.FileSystemMod).GetList(String.Empty, AFileSystemBase.ListType.AllFolder);
                ((FileSystemWindows)GameUty.FileSystemMod).AddAutoPath("[splithairsliders]\\");
            }
            if (GameUty.FileSystemMod != null)
            {
                typeof(GameUty).GetField("m_aryModOnlysMenuFiles").SetValue(null, Array.FindAll<string>(GameUty.FileSystemMod.GetList(string.Empty, AFileSystemBase.ListType.AllFile), (Predicate<string>)(i => new System.Text.RegularExpressions.Regex(".*\\.menu$").IsMatch(i))));
            }
        }



        //public static void exportCurrentFrontHair(MPN category)
        //{
        //    Maid maid = GameMain.Instance.CharacterMgr.GetMaid(0);
        //    if (maid != null)
        //    {
        //        MaidProp maidProp = maid.GetProp(category);
        //        string hairMenuFileName = maidProp.strFileName.ToString();
        //        TBody.SlotID slotID = TBody.SlotID.hairAho;
        //        switch (category)
        //        {
        //            case MPN.hairf:
        //                slotID = TBody.SlotID.hairF;
        //                break;
        //            case MPN.hairr:
        //                slotID = TBody.SlotID.hairR;
        //                break;
        //            case MPN.hairs:
        //                slotID = TBody.SlotID.hairS;
        //                break;
        //            case MPN.hairt:
        //                slotID = TBody.SlotID.hairT;
        //                break;
        //        }
        //        if (hairMenuFileName != null)
        //        {
        //            if (hairMenuFileName.IndexOf("mod_") != 0)
        //            {
        //                if (slotID != TBody.SlotID.hairAho &&
        //                    maid.body0.goSlot[(int)slotID] != null &&
        //                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl != null && maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList != null &&
        //                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList.ContainsKey("AutoConv") &&
        //                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget != null &&
        //                    maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget.Count != 0)
        //                {
        //                    Console.WriteLine("Checking Menu File " + hairMenuFileName + " Category " + category);
        //                    createNewHairMenuCM(hairMenuFileName, maid.body0.goSlot[(int)slotID].m_HairLengthCtrl.HairLengthGroupList["AutoConv"].listTarget);
        //                    Console.WriteLine("Checking Menu File DONE " + hairMenuFileName + " Category " + category);
        //                }
        //                else
        //                {
        //                    maid.body0.goSlot[(int)slotID].morph.BoneNames;

        //                    Console.WriteLine("Checking Menu File " + hairMenuFileName + " Category " + category);
        //                    createNewHairMenuCOM(hairMenuFileName);
        //                    Console.WriteLine("Checking Menu File DONE " + hairMenuFileName + " Category " + category);
        //                }
        //            }
        //            else
        //            {
        //                UnityEngine.Debug.Log("Split Hair Sliders: Cannot use MOD hair");
        //            }
        //        }
        //        else
        //        {
        //            UnityEngine.Debug.Log("Split Hair Sliders: null hair");
        //        }
        //    }
        //    else
        //    {
        //        UnityEngine.Debug.Log("Split Hair Sliders: null maid");
        //    }
        //}
        //public static void createNewHairMenuCOM(string hairMenuFileName)
        //{
        //    MenuObj menu = new MenuObj();

        //    byte[] cd;
        //    using (AFileBase afileBase = GameUty.FileOpen(hairMenuFileName, (AFileSystemBase)null))
        //    {
        //        if (afileBase != null && afileBase.IsValid())
        //        {
        //            Console.WriteLine("Reading bytes");

        //            cd = afileBase.ReadAll();

        //            using (BinaryReader binaryReader = new BinaryReader((Stream)new MemoryStream(cd), Encoding.UTF8))
        //            {
        //                //Useless header info???
        //                Console.WriteLine("Reading header");
        //                menu.header.CM3D2_MENU = binaryReader.ReadString();
        //                menu.header.temp1 = binaryReader.ReadInt32();
        //                menu.header.temp2 = binaryReader.ReadString();
        //                menu.header.temp3 = binaryReader.ReadString();
        //                menu.header.temp4 = binaryReader.ReadString();
        //                menu.header.temp5 = binaryReader.ReadString();
        //                menu.header.temp6 = binaryReader.ReadInt32();

        //                bool end = false;

        //                Console.WriteLine("Reading commands");
        //                //Blocks
        //                do
        //                {
        //                    MenuCommandObj command = new MenuCommandObj();
        //                    string str4;
        //                    do
        //                    {
        //                        byte byt = binaryReader.ReadByte();
        //                        command.byt = byt;
        //                        int num2 = (int)byt;

        //                        str4 = string.Empty;
        //                        if (num2 != 0)
        //                        {
        //                            for (int index = 0; index < num2; ++index)
        //                            {
        //                                string str = binaryReader.ReadString();
        //                                command.nativeStrings.Add(str);
        //                                str4 = str4 + "\"" + str + "\" ";
        //                                Console.WriteLine(str);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            end = true;
        //                        }
        //                    }
        //                    while (str4 == string.Empty && !end);

        //                    if (!end)
        //                    {
        //                        string stringCom = UTY.GetStringCom(str4);
        //                        string[] stringList = UTY.GetStringList(str4);

        //                        command.stringCom = stringCom;
        //                        command.stringList = stringList;
        //                        menu.commands.Add(command);

        //                        if (stringCom.Equals("category"))
        //                        {
        //                            //Get the category
        //                            menu.category = stringList[1];
        //                        }
        //                    }
        //                }
        //                while (!end);
        //            }
        //        }
        //        else
        //        {
        //            if(afileBase == null)
        //            {
        //                UnityEngine.Debug.Log("Split Hair Sliders: null AFileBase");
        //            }
        //            else
        //            {
        //                if(!afileBase.IsValid())
        //                {
        //                    UnityEngine.Debug.Log("Split Hair Sliders: invalid AFileBase");
        //                }
        //            }

        //        }
        //    }

        //    //Only process the file if it is a hair
        //    if (new string[] { "hairf", "hairr", "hairs", "hairt" }.Contains(menu.category.ToLower()))
        //    {
        //        string newMenuPath = UTY.gameProjectPath + "\\Mod\\[SplitHairSliders]";
        //        if (!Directory.Exists(newMenuPath))
        //        {
        //            Directory.CreateDirectory(newMenuPath);
        //        }

        //        if(File.Exists(Path.Combine(newMenuPath, hairMenuFileName)))
        //        {
        //            File.Delete(Path.Combine(newMenuPath, hairMenuFileName));
        //        }

        //        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(Path.Combine(newMenuPath, "Ext_" + hairMenuFileName), FileMode.OpenOrCreate)))
        //        {
        //            List<string> sliderNames = new List<string>();

        //            //Write the header
        //            binaryWriter.Write(menu.header.CM3D2_MENU);
        //            binaryWriter.Write(menu.header.temp1);
        //            binaryWriter.Write(menu.header.temp2);
        //            binaryWriter.Write(menu.header.temp3);
        //            binaryWriter.Write(menu.header.temp4);
        //            binaryWriter.Write(menu.header.temp5);
        //            binaryWriter.Write(menu.header.temp6);

        //            //Write the commands
        //            for (int i = 0; i < menu.commands.Count; i++)
        //            {
        //                MenuCommandObj command = menu.commands[i];

        //                //Write the byte indicating size of params
        //                binaryWriter.Write(command.byt);

        //                //Special case for the length sliders - need to adjust the data
        //                if (command.stringCom.Equals("length"))
        //                {
        //                    string sliderName = command.stringList[2];
        //                    while (sliderNames.Contains(sliderName))
        //                    {
        //                        sliderName += "_";
        //                    }
        //                    sliderNames.Add(sliderName);

        //                    //Write out commands
        //                    for (int j = 0; j < command.nativeStrings.Count; j++)
        //                    {
        //                        if (j != 2)
        //                        {
        //                            //Normal Param
        //                            binaryWriter.Write(command.nativeStrings[j]);
        //                        }
        //                        else
        //                        {
        //                            //Slider
        //                            binaryWriter.Write(sliderName);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //Write out normal commands
        //                    for (int j = 0; j < command.nativeStrings.Count; j++)
        //                    {
        //                        binaryWriter.Write(command.nativeStrings[j]);
        //                    }
        //                }
        //            }

        //            //Write the end
        //            binaryWriter.Write(Convert.ToByte('\u0000'));
        //        }
        //    }
        //    else
        //    {
        //        UnityEngine.Debug.Log("Split Hair Sliders: Invalid category " + menu.category);
        //    }
        //}
        //public static void createNewHairMenuCM(string hairMenuFileName, List<TBodySkin.HairLengthCtrl.HairLengthTarget> listTarget)
        //{
        //    MenuObj menu = new MenuObj();

        //    byte[] cd;
        //    using (AFileBase afileBase = GameUty.FileOpen(hairMenuFileName, (AFileSystemBase)null))
        //    {
        //        if (afileBase != null && afileBase.IsValid())
        //        {
        //            Console.WriteLine("Reading bytes");

        //            cd = afileBase.ReadAll();

        //            using (BinaryReader binaryReader = new BinaryReader((Stream)new MemoryStream(cd), Encoding.UTF8))
        //            {
        //                //Useless header info???
        //                Console.WriteLine("Reading header");
        //                menu.header.CM3D2_MENU = binaryReader.ReadString();
        //                menu.header.temp1 = binaryReader.ReadInt32();
        //                menu.header.temp2 = binaryReader.ReadString();
        //                menu.header.temp3 = binaryReader.ReadString();
        //                menu.header.temp4 = binaryReader.ReadString();
        //                menu.header.temp5 = binaryReader.ReadString();
        //                menu.header.temp6 = binaryReader.ReadInt32();

        //                bool end = false;

        //                Console.WriteLine("Reading commands");
        //                //Blocks
        //                do
        //                {
        //                    MenuCommandObj command = new MenuCommandObj();
        //                    string str4;
        //                    do
        //                    {
        //                        byte byt = binaryReader.ReadByte();
        //                        command.byt = byt;
        //                        int num2 = (int)byt;

        //                        str4 = string.Empty;
        //                        if (num2 != 0)
        //                        {
        //                            for (int index = 0; index < num2; ++index)
        //                            {
        //                                string str = binaryReader.ReadString();
        //                                command.nativeStrings.Add(str);
        //                                str4 = str4 + "\"" + str + "\" ";
        //                                Console.WriteLine(str);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            end = true;
        //                        }
        //                    }
        //                    while (str4 == string.Empty && !end);

        //                    if (!end)
        //                    {
        //                        string stringCom = UTY.GetStringCom(str4);
        //                        string[] stringList = UTY.GetStringList(str4);

        //                        command.stringCom = stringCom;
        //                        command.stringList = stringList;
        //                        menu.commands.Add(command);

        //                        if (stringCom.Equals("category"))
        //                        {
        //                            //Get the category
        //                            menu.category = stringList[1];
        //                        }
        //                    }
        //                }
        //                while (!end);
        //            }
        //        }
        //        else
        //        {
        //            if (afileBase == null)
        //            {
        //                UnityEngine.Debug.Log("Split Hair Sliders: null AFileBase");
        //            }
        //            else
        //            {
        //                if (!afileBase.IsValid())
        //                {
        //                    UnityEngine.Debug.Log("Split Hair Sliders: invalid AFileBase");
        //                }
        //            }

        //        }
        //    }

        //    //Only process the file if it is a hair
        //    if (new string[] { "hairf", "hairr", "hairs", "hairt" }.Contains(menu.category.ToLower()))
        //    {
        //        string newMenuPath = UTY.gameProjectPath + "\\Mod\\[SplitHairSliders]";
        //        if (!Directory.Exists(newMenuPath))
        //        {
        //            Directory.CreateDirectory(newMenuPath);
        //        }

        //        if (File.Exists(Path.Combine(newMenuPath, hairMenuFileName)))
        //        {
        //            File.Delete(Path.Combine(newMenuPath, hairMenuFileName));
        //        }

        //        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(Path.Combine(newMenuPath, "Ext_" + hairMenuFileName), FileMode.OpenOrCreate)))
        //        {
        //            List<string> sliderNames = new List<string>();

        //            //Write the header
        //            binaryWriter.Write(menu.header.CM3D2_MENU);
        //            binaryWriter.Write(menu.header.temp1);
        //            binaryWriter.Write(menu.header.temp2);
        //            binaryWriter.Write(menu.header.temp3);
        //            binaryWriter.Write(menu.header.temp4);
        //            binaryWriter.Write(menu.header.temp5);
        //            binaryWriter.Write(menu.header.temp6);

        //            //Write the commands
        //            for (int i = 0; i < menu.commands.Count; i++)
        //            {
        //                MenuCommandObj command = menu.commands[i];

        //                //Write the byte indicating size of params
        //                binaryWriter.Write(command.byt);

        //                //Special case for the length sliders - need to adjust the data
        //                if (command.stringCom.Equals("length"))
        //                {
        //                    string sliderName = command.stringList[2];
        //                    while (sliderNames.Contains(sliderName))
        //                    {
        //                        sliderName += "_";
        //                    }
        //                    sliderNames.Add(sliderName);

        //                    //Write out commands
        //                    for (int j = 0; j < command.nativeStrings.Count; j++)
        //                    {
        //                        if (j != 2)
        //                        {
        //                            //Normal Param
        //                            binaryWriter.Write(command.nativeStrings[j]);
        //                        }
        //                        else
        //                        {
        //                            //Slider
        //                            binaryWriter.Write(sliderName);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //Write out normal commands
        //                    for (int j = 0; j < command.nativeStrings.Count; j++)
        //                    {
        //                        binaryWriter.Write(command.nativeStrings[j]);
        //                    }
        //                }
        //            }

        //            //Add the hair length options
        //            for(int i=0; i<listTarget.Count; i++)
        //            {
        //                binaryWriter.Write(Convert.ToByte(11));
        //                binaryWriter.Write("length");
        //                binaryWriter.Write(menu.category);
        //                binaryWriter.Write("髪" + i);
        //                binaryWriter.Write("fbrother");
        //                binaryWriter.Write(listTarget[i].trTarget.name);
        //                binaryWriter.Write(listTarget[i].vScaleMin.x.ToString("N1"));
        //                binaryWriter.Write(listTarget[i].vScaleMin.y.ToString("N1"));
        //                binaryWriter.Write(listTarget[i].vScaleMin.z.ToString("N1"));
        //                binaryWriter.Write(listTarget[i].vScaleMax.x.ToString("N1"));
        //                binaryWriter.Write(listTarget[i].vScaleMax.y.ToString("N1"));
        //                binaryWriter.Write(listTarget[i].vScaleMax.z.ToString("N1"));
        //            }

        //            //Write the end
        //            binaryWriter.Write(Convert.ToByte('\u0000'));
        //        }
        //    }
        //    else
        //    {
        //        UnityEngine.Debug.Log("Split Hair Sliders: Invalid category " + menu.category);
        //    }
        //}
        #endregion

        #region UI
        Rect uiWindow = new Rect(Screen.width / 8 + 20, 20, 120, 50);
        private void OnGUI()
        {
            if (displayUI)
            {
                uiWindow = GUILayout.Window(416803, uiWindow, DisplayUIWindow, "Split Hair Sliders", GUILayout.Height(Screen.height * 1 / 4 - 40));
            }
        }

        private void DisplayUIWindow(int windowId)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label(status);
            overwriteExisting = GUILayout.Toggle(overwriteExisting, "Overwrite Existing?");
            everyBone = GUILayout.Toggle(everyBone, "Every Bone?");
            includeMods = GUILayout.Toggle(includeMods, "Include Mods?\n(Must Restart Game)");
            if (!includeMods)
            {
                GUI.enabled = false;
                onlyMods = false;
            }
            onlyMods = GUILayout.Toggle(onlyMods, "Only Mods?");
            GUI.enabled = true;

            if (GUILayout.Button("Export"))
            {
                status = "STARTING...";
                exportAllHairMenus();
                status = "DONE Reload EDIT";
            }
            //if (GUILayout.Button("Hair Front"))
            //{
            //    exportCurrentFrontHair(MPN.hairf);
            //}
            //if (GUILayout.Button("Hair Rear"))
            //{
            //    exportCurrentFrontHair(MPN.hairr);
            //}
            //if (GUILayout.Button("Hair Top"))
            //{
            //    exportCurrentFrontHair(MPN.hairt);
            //}
            //if (GUILayout.Button("Hair Side"))
            //{
            //    exportCurrentFrontHair(MPN.hairs);
            //}

            GUILayout.EndVertical();

            //Make it draggable this must be last always
            GUI.DragWindow();
        }
        #endregion
    }
}
