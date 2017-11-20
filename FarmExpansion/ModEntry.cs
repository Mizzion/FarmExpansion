﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using FarmExpansion.Framework;
using System;
using System.Collections.Generic;

namespace FarmExpansion
{

    public class ModEntry : Mod, IAssetEditor
    {
        private FEFramework framework;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\Locations") || asset.AssetNameEquals(@"LooseSprites\map");
        }

        public void Edit<T>(IAssetData asset)
        {
            switch(asset.AssetName)
            {
                case @"Data\Locations":
                    asset.AsDictionary<string, string>().Data.Add(new KeyValuePair<string, string>("FarmExpansion", "-1/-1/-1/-1/-1/-1/-1/-1/382 .05 770 .1 390 .25 330 1"));
                    break;
                case @"LooseSprites\map":
                    Texture2D customTexture;
                    try
                    {
                        customTexture = this.Helper.Content.Load<Texture2D>("WorldMap.xnb");

                        Rectangle defaultFarm = new Rectangle(32, 64, 35, 25);
                        Rectangle fishingFarm = new Rectangle(32, 203, 34, 23);
                        Rectangle foragingFarm = new Rectangle(163, 202, 35, 24);
                        Rectangle miningFarm = new Rectangle(32, 262, 34, 27);
                        Rectangle combatFarm = new Rectangle(163, 262, 34, 25);
                        
                        if (customTexture != null)
                        {
                            asset.AsImage().PatchImage(customTexture, defaultFarm, defaultFarm);
                            asset.AsImage().PatchImage(customTexture, fishingFarm, fishingFarm);
                            asset.AsImage().PatchImage(customTexture, foragingFarm, foragingFarm);
                            asset.AsImage().PatchImage(customTexture, miningFarm, miningFarm);
                            asset.AsImage().PatchImage(customTexture, combatFarm, combatFarm);
                        }
                    }
                    catch(Exception ex)
                    {
                        this.Monitor.Log($"Could not load WorldMap.xnb from the mod folder, world map will not be patched.\n{ex}", LogLevel.Error);
                    }
                    break;
            }
        }

        public override void Entry(IModHelper helper)
        {
            framework = new FEFramework(helper, Monitor);
            //ControlEvents.KeyPressed += framework.ControlEvents_KeyPress;
            ControlEvents.ControllerButtonPressed += framework.ControlEvents_ControllerButtonPressed;
            ControlEvents.MouseChanged += framework.ControlEvents_MouseChanged;
            //LocationEvents.CurrentLocationChanged += framework.LocationEvents_CurrentLocationChanged;
            MenuEvents.MenuChanged += framework.MenuEvents_MenuChanged;
            MenuEvents.MenuClosed += framework.MenuEvents_MenuClosed;
            SaveEvents.AfterLoad += framework.SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += framework.SaveEvents_BeforeSave;
            SaveEvents.AfterSave += framework.SaveEvents_AfterSave;
            SaveEvents.AfterReturnToTitle += framework.SaveEvents_AfterReturnToTitle;
            TimeEvents.AfterDayStarted += framework.TimeEvents_AfterDayStarted;
        }

    }
}