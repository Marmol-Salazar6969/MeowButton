using BepInEx;
using System;
using UnityEngine;
using Menu;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;

namespace MeowButton;
[BepInPlugin("MeowButton", "Meow Button", "1.0")]

public class MeowButton : BaseUnityPlugin{

    static bool _initialized;
    private void LogInfo(object data){
        Logger.LogInfo(data);
    }

    public void OnEnable(){
        LogInfo("HA, MY SILLY BUTTON IN NOW ACTIVE!! BOING BOING BOING");
        On.RainWorld.OnModsInit += RainWorld_OnModsInit;

    }
    private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self){
        try{
            if (_initialized) return;
            _initialized = true;

            On.Menu.MainMenu.ctor += MainMenu_ctor;
            MeowB.RegisterValues();
        }
        catch (Exception data){
            LogInfo(data);
            throw;
        }
        finally{
            orig.Invoke(self);
        }
    }

    private void MainMenu_ctor(On.Menu.MainMenu.orig_ctor orig, Menu.MainMenu self, ProcessManager manager, bool showRegionSpecificBkg)
    {
        orig(self, manager, showRegionSpecificBkg);

        float buttonWidth = Menu.MainMenu.GetButtonWidth(self.CurrLang);
        Vector2 pos = new Vector2(683f - buttonWidth / 2f, 0f);
        Vector2 size = new Vector2(buttonWidth, 30f);

        Debug.Log("Adding Button");
        self.AddMainMenuButton(new SimpleButton(self, self.pages[0], "MEOW~", "MEOW MEOW~", pos, size), () => {
            int randomIndex = UnityEngine.Random.Range(0, MeowB.meows.Length);
            self.PlaySound(MeowB.meows[randomIndex]);
        }, 0);
    }
}
public static class MeowB
{
    public static void RegisterValues()
    {
        meows = new SoundID[] {
             new SoundID("meow1", true),
             new SoundID("meow2", true),
             new SoundID("meow3", true),
             new SoundID("meow4", true),
             new SoundID("catf1", true),
             new SoundID("catf2", true),
             new SoundID("catf3", true),
             new SoundID("catf4", true),
             new SoundID("susmeow", true),
        };
    }
    public static void UnregisterValues()
    {
        foreach (var meow in meows)
        {
            Unregister(meow);
        }
    }

    private static void Unregister<T>(ExtEnum<T> extEnum) where T : ExtEnum<T>
    {
        extEnum?.Unregister();
    }

    public static SoundID[] meows;

}