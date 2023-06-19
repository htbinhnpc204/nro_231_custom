using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Code
{
    public static bool isAutoHS;

    public static int wishStar;
    public static int addHP;
    public static MyVector vItemCombine;
    public static List<Item> vItemAuto = new List<Item>();
    public static List<BossSpawn> vBossSpawn = new List<BossSpawn>();

    public static int cSpeed;
    public static int Potential = -1;
    public static int delayAK = 200;
    public static int[] oldPos = new int[2];

    public static bool isAutoHSME;
    public static bool isAutoAttack;
    public static bool isAutoDapdo;
    public static bool isAutoAdd;
    public static bool isCallDragon;
    public static bool isOpenUIPet;
    public static bool isOpenUIZone;
    public static bool isKhoaViTri;
    public static bool isAutoLogin;
    public static bool isOpenCSKB;
    public static bool isUsing;
    public static bool isAutoTake = true;
    public static bool canAutoPlay;
    public static bool isKMT;
    public static bool isChangeSpecialSkill;
    public static bool isUnlockBag;

    public static object objKMT;
    public static readonly string RMSDataPath = "rms.data";

    public static bool isKillBoss;


    public static AccountLogin account = new AccountLogin();
    public static int[] idItemAutoUse = { 379, 381, 382, 383, 384, 385 };

    public static string mobName { get; set; }
    public static bool SaleTrash { get; set; }
    public static bool StartUpgrade { get; set; }

    public static void getAccount()
    {
        if (File.Exists("login"))
        {
            try
            {
                account = JsonMapper.ToObject<AccountLogin>(File.ReadAllText("login"));
                File.Delete("login");
                isAutoLogin = account.Config[0];
            }
            catch
            {

            }
        }
    }

    public static long getMyDame()
    {
        long result;
        if (Char.myCharz().cgender == 0)
        {
            result = 0;
        }
        else if (Char.myCharz().cgender == 2)
        {
            if (Char.myCharz().getSkill(14) == null || Char.myCharz().getSkill(14).isCooldown())
                result = 0;
            else
                result = (long)(Char.myCharz().cHPFull * 0.5);
        }
        else
        {
            if (Char.myCharz().getSkill(11).isCooldown() || Char.myCharz().getSkill(11) == null)
                result = 0;
            else
                result = (long)(Char.myCharz().cMP * (1.3 + (Char.myCharz().isSetPicolo() ? 0.0 : 0)));
        }

        return result + addHP;
    }

    public static int rgbToInt(int r, int g, int b)
    {
        return 65536 * r + 256 * g + b;
    }

    public static void Use1Star()
    {
        UseItem(14);
    }

    public static bool UseItem(Item i) => UseItem(i.template.id);

    public static bool UseItem(int id)
    {
        var item = Char.myCharz().arrItemBag.SingleOrDefault(item => item != null && item.template.id == id);
        if(item == null) return false;
        Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
        return true;
    }

    public static bool getPotential()
    {
        switch (Potential)
        {
            case 0: return (Char.myCharz().cTiemNang >= 100 * (2 * Char.myCharz().cHPGoc + 1000) + 1980 / 2);
            case 1: return (Char.myCharz().cTiemNang >= 100 * (2 * Char.myCharz().cMPGoc + 1000) + 1980 / 2);
            case 2: return (Char.myCharz().cTiemNang >= 100 * (2 * Char.myCharz().cDamGoc + 99) / 2 * Char.myCharz().expForOneAdd);
            default: return false;
        }
    }

    public static bool ExistsItemAuto(Item item)
    {
        return vItemAuto.Where(i => i.template.id == item.template.id).Count() > 0;
    }

    public static bool ExistsItemAuto(int id)
    {
        return vItemAuto.Where(i => i.template.id == id).Count() > 0;
    }

    public static Item GetItemAuto(int id)
    {
        return vItemAuto.SingleOrDefault(i => i.template.id == id);
    }

    public static void AttackMob(Mob mob)
    {
        MyVector myVector = new();
        myVector.addElement(mob);
        Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
    }

    public static void AttackChar(Char vchar)
    {
        MyVector myVector = new MyVector();
        myVector.addElement(vchar);
        Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
    }

    public static void HSME()
    {
        if (Char.myCharz().cgender == 1)
        {
            Skill curr = Char.myCharz().myskill;
            Service.gI().selectSkill(7);
            for (int j = 0; j < GameScr.onScreenSkill.Length; j++)
            {
                Skill skill = GameScr.onScreenSkill[j];
                if ((int)skill.template.id == 7)
                {
                    Char.myCharz().myskill = skill;
                    skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                }
            }
            MyVector myVector = new MyVector();
            myVector.addElement(Char.myCharz());
            Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
            Char.myCharz().myskill = curr;
            Service.gI().selectSkill(curr.template.id);
        }
    }

    public static void SetFocus(object p)
    {
        objKMT = p;
    }

    public static bool chatMod(string text)
    {
        if (text == "kmt")
        {
            if (Char.myCharz().charFocus != null) SetFocus(Char.myCharz().charFocus);
            else if (Char.myCharz().mobFocus != null) SetFocus(Char.myCharz().mobFocus);
            else SetFocus(Char.myCharz().npcFocus);
            isKMT = !isKMT;
            if (!isKMT) SetFocus(null);
            Paint("Đã " + (isKMT ? "bật" : "tắt") + " khóa mục tiêu!");
            return false;
        }
        else if (text == "unlockht" && TileMap.mapID == 5)
        {
            isUnlockBag = !isUnlockBag;
            new Thread(() =>
            {
                while (isUnlockBag)
                {
                    Service.gI().buyItem(1, 517, 0);
                    Thread.Sleep(20);
                }
            })
            { IsBackground = true }.Start();
            Paint("Mở rộng hành trang!");
            return false;
        }
        else if (text == "noitai")
        {
            isChangeSpecialSkill = !isChangeSpecialSkill;
            Paint("Đã " + (isChangeSpecialSkill ? "bật" : "tắt") + " auto đổi nội tại!");
            return false;
        }
        else if (text == "saletrash")
        {
            SaleTrash = !SaleTrash;
            Paint("Đã " + (SaleTrash ? "bật" : "tắt") + " bán rác!");
            return false;
        }
        else if (text == "anhat")
        {
            isAutoTake = !isAutoTake;
            Paint("Đã " + (isAutoTake ? "bật" : "tắt") + " hút vật phẩm!");
            return false;
        }
        else if (text == "opencskb")
        {
            isOpenCSKB = !isOpenCSKB;
            Paint("Đã " + (isOpenCSKB ? "bật" : "tắt") + " tự động mở cskb!");
            return false;
        }
        else if (text == "xa") //deleteAll
        {
            GameCanvas.panel.perform(30005, null);
            return false;
        }

        else if (text == "na") //receiveAll
        {
            Service.gI().buyItem(2, 0, 0);
            return false;
        }
        else if (text == "xr") //deleteTrash
        {
            GameCanvas.panel.perform(30004, null);
            return false;
        }
        else if (text.StartsWith("cb"))
        {
            int num;
            text = text.Replace("cb", "");
            if (int.TryParse(text.Trim(), out num))
            {
                if (num < 0) return true;
                for (int i = 0; i <= num; i++)
                    Service.gI().SendCrackBall(2, 7);
                Paint("Quay ngọc rồng");
            }
            return false;
        }
        else if (text.StartsWith("ahp"))
        {
            int num;
            text = text.Replace("ahp", "");
            if (int.TryParse(text.Trim(), out num))
            {
                if (num < 0) return true;
                addHP = num;
            }
            return false;
        }
        else if (text == "kvt")
        {
            isKhoaViTri = !isKhoaViTri;
            if (isKhoaViTri)
            {
                oldPos[0] = Char.myCharz().cx;
                oldPos[1] = Char.myCharz().cy;
            }
            Paint("Đã " + (isKhoaViTri ? "Bật" : "Tắt") + " khóa vị trí");
            return false;
        }
        else if (text.StartsWith("at") && text.Length == 4)
        {
            text = text.Replace("at", "");
            switch (text)
            {
                case "hp": Potential = 0; break;
                case "ki": Potential = 1; break;
                case "sd": Potential = 2; break;
            }
            isAutoAdd = !isAutoAdd;
            Paint("Đã " + (isAutoAdd ? "Bật" : "Tắt") + " Tự động tăng chỉ số");
            return false;
        }
        else if (text == "ak")
        {
            isAutoAttack = !isAutoAttack;
            Paint("Đã " + (isAutoAttack ? "Bật" : "Tắt") + " Tự động tấn công");
            if (isAutoAttack)
            {
                Thread thread = new(() =>
                {
                    while (isAutoAttack)
                    {
                        if (Char.myCharz().mobFocus != null && Char.myCharz().mobFocus.hp > 0)
                        {
                            AttackMob(Char.myCharz().mobFocus);
                        }
                        if (Char.myCharz().charFocus != null && Char.myCharz().charFocus != Char.myPetz() && Char.myCharz().charFocus != Char.myCharz() && Char.myCharz().charFocus.cHP > 0)
                        {
                            AttackChar(Char.myCharz().charFocus);
                        }
                        Thread.Sleep(delayAK);
                    }
                })
                {
                    IsBackground = true
                };
                thread.Start();
            }
            return false;
        }
        else if (text.StartsWith("ak") && (delayAK = int.Parse(text.Replace("ak", String.Empty))) != 0)
        {
            if (delayAK < 100) delayAK = 100;
            Paint("Delay auto attack " + delayAK);
            return false;
        }
        else if (text == "ahs")
        {
            isAutoHS = !isAutoHS;
            Paint((isAutoHS ? "Đã bật" : "Đã tắt") + " hồi sinh ngọc");
            return false;
        }
        else if (text == "hsme")
        {
            HSME();
            return false;
        }
        if (text == "ahsme")
        {
            if (global::Char.myCharz().cgender == 1)
            {
                isAutoHSME = !isAutoHSME;
                Paint("Đã " + (isAutoHSME ? "Bật" : "Tắt") + " Tự động hồi sinh bản thân");
                if (isAutoHSME)
                {
                    Thread thread = new Thread(() =>
                    {
                        while (isAutoHSME)
                        {
                            HSME();
                            for (int j = 0; j < GameScr.onScreenSkill.Length; j++)
                            {
                                Skill skill = GameScr.onScreenSkill[j];
                                if ((int)skill.template.id == 7)
                                {
                                    Thread.Sleep(skill.coolDown);
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            return false;
        }
        else if (text.StartsWith("speed") && float.TryParse(text.Replace("speed", String.Empty), out float speed))
        {
            Time.timeScale = ((speed <= 0) ? 1 : speed);
            Paint("Speed Game: " + Time.timeScale);
            return false;
        }
        else if (text.StartsWith("cs") && int.TryParse(text.Replace("cs", String.Empty), out int charSpeed))
        {
            Paint("Tốc độ chạy: " + (cSpeed = charSpeed < 0 ? 1 : charSpeed));
        }
        else if (text.StartsWith("cz") && int.TryParse(text.Replace("cz", String.Empty), out int zoneChange))
        {
            Service.gI().requestChangeZone((zoneChange = (zoneChange < 0 ? -1 : zoneChange)), -1);
            Paint("Chuyển tới khu: " + zoneChange);
            return false;
        }
        else if (text.StartsWith("boss "))
        {
            string name = text.Replace("boss ", String.Empty);
            Char boss = GameScr.FindBOSSInMap(name);
            if (boss != null)
            {
                Char.myCharz().clearFocus(1);
                Char.myCharz().charFocus = boss;
                Paint("Focus to boss: " + boss.cName);
                return false;
            }
            else
            {
                Paint("Boss " + name + " is not found");
                return false;
            }
        }
        else if (text.StartsWith("npc") && int.TryParse(text.Replace("npc", String.Empty), out int npcId))
        {
            Service.gI().openMenu(npcId);
            return false;
        }
        switch (text)
        {
            case "di theo":
            case "follow": Service.gI().petStatus(0); return false;
            case "bao ve":
            case "protect": Service.gI().petStatus(1); return false;
            case "tan cong":
            case "attack": Service.gI().petStatus(2); return false;
            case "ve nha":
            case "go home": Service.gI().petStatus(3); return false;

        }
        return true;
    }
    public static bool CheckRMS()
    {
        FileInfo[] files = new DirectoryInfo(Application.persistentDataPath + "/").GetFiles();
        string[] arr = File.ReadAllLines(RMSDataPath);
        foreach (var file in files)
        {
            if (!arr.Contains(file.Name)) return false;
        }
        return true;
    }

    public static void Paint(string text)
    {
        Char.myCharz().addInfo(text);
    }

    public static void MoveTo(int x, int y)
    {
        Char @char = Char.myCharz();
        @char.cx = x; @char.cy = y;
        Service.gI().charMove();
    }

    static Code()
    {
        vItemCombine = new MyVector();
        cSpeed = Char.myCharz().cspeed;
    }

    public static void PrintMinMaxZone(mGraphics g)
    {
        int min = 999, max = 0;
        int idmin = 0, idmax = 0;
        foreach (var i in GameScr.gI().zones)
        {
            if (GameScr.gI().numPlayer[i] < min)
            {
                min = GameScr.gI().numPlayer[i];
                idmin = i;
            }
            if (GameScr.gI().numPlayer[i] > max)
            {
                max = GameScr.gI().numPlayer[i];
                idmax = i;
            }
        }
        mFont.tahoma_7_yellow.drawString(g, String.Format("Khu ít nhất: {0}[{1}/{2}]", idmin, GameScr.gI().numPlayer[idmin], GameScr.gI().maxPlayer[idmin]), 30, 90, 0, mFont.tahoma_7_grey);
        mFont.tahoma_7_yellow.drawString(g, String.Format("Khu đông nhất: {0}[{1}/{2}]", idmax, GameScr.gI().numPlayer[idmax], GameScr.gI().maxPlayer[idmin]), 30, 100, 0, mFont.tahoma_7_grey);
    }

    internal static Item GetCskb()
    {
        var itemBag = Char.myCharz().arrItemBag;
        for(int i = itemBag.Length - 1; i >= 0; i++)
        {
            if (itemBag[0].template.id == 380)
            {
                return itemBag[0];
            }
        }
        return null;
    }

    internal static void openCSKB()
    {
        Item cskb = Char.myCharz().arrItemBag.FirstOrDefault(x => x.template.id == 380);
        if (cskb != null)
        {
            new Thread(() =>
            {
                isUsing = true;
                while ((cskb = GetCskb()) != null && cskb.quantity > 0)
                {
                    Service.gI().useItem(0, 1, (sbyte)cskb.indexUI, -1);
                }
                isUsing = false;
            })
            { IsBackground = true }.Start();
        }
    }

    public static void doKillBoss(Char boss)
    {
        var temp = Char.myCharz().myskill.template.id;
        if (Char.myCharz().cgender == 0)
        {
            return;
        }
        else if (Char.myCharz().cgender == 2)
        {
            Service.gI().selectSkill(14);
            AttackChar(Char.myCharz());
            AttackChar(boss);
            AttackMob(GameScr.vMob.a.Cast<Mob>().FirstOrDefault());
            Service.gI().selectSkill(temp);
            AttackChar(boss);
        }
        else
        {
            Service.gI().selectSkill(11);
            AttackChar(boss);
            Service.gI().selectSkill(temp);
            AttackChar(boss);
        }
    }

    public static bool IsMyItem(ItemMap itemMap)
    {
        return itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1;
    }

    public static bool CanPickItem(ItemMap itemMap)
    {
        return itemMap.template.type != 30 || itemMap.template.id == 447 || itemMap.template.id == 441 || itemMap.template.id == 442;
    }

    public static void DoPickItem(ItemMap itemMap)
    {
        if (!CanPickItem(itemMap) || !IsMyItem(itemMap)) { return; }
        int cx = Char.myCharz().cx, cy = Char.myCharz().cy;
        MoveTo(itemMap.getX(), itemMap.getY());
        Service.gI().pickItem(itemMap.itemMapID);
        MoveTo(cx, cy);
    }
}
