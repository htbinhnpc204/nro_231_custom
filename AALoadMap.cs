using System;
using System.Threading;

public class AALoadMap
{
    public static int[] posLeft;

    public static int[] posMid;

    public static int[] posRight;

    public static sbyte idPlanetEnd;

    public static int idMapEndXmap;

    public static int[,] MapTraiDat;

    public static int[,] MapNamec;

    public static int[,] MapSayda;

    public static int[,] idMapFide;

    public static void ChatAction(string text)
    {
        try
        {
            if (text == "xmap")
            {
                MyVector myVector = new MyVector();
                myVector.addElement(new Command("Xayda", 240201));
                myVector.addElement(new Command("Trái Đất", 240202));
                myVector.addElement(new Command("Namec", 240203));
                myVector.addElement(new Command("Nappa", 240204));
                GameCanvas.menu.startAt(myVector, 3);
            }
        }
        catch (Exception)
        {
        }
    }

    public static void resetPosMap()
    {
        posLeft = new int[2];
        posMid = new int[2];
        posRight = new int[2];
    }

    public static void getPosMap()
    {
        resetPosMap();
        int num = TileMap.vGo.size();
        if (num != 2)
        {
            for (int i = 0; i < num; i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                if (waypoint.maxX < 60)
                {
                    posLeft[0] = waypoint.minX;
                    posLeft[1] = waypoint.maxY;
                }
                else if (waypoint.maxX > TileMap.pxw - 60)
                {
                    posRight[0] = waypoint.maxX;
                    posRight[1] = waypoint.maxY;
                }
                else
                {
                    posMid[0] = waypoint.minX + (waypoint.maxX - waypoint.minX) / 2;
                    posMid[1] = waypoint.maxY;
                }
            }
            return;
        }
        Waypoint waypoint2 = (Waypoint)TileMap.vGo.elementAt(0);
        Waypoint waypoint3 = (Waypoint)TileMap.vGo.elementAt(1);
        if ((waypoint2.maxX < 60 && waypoint3.maxX < 60) || (waypoint2.minX > TileMap.pxw - 60 && waypoint3.minX > TileMap.pxw - 60))
        {
            posLeft[0] = waypoint2.minX;
            posLeft[1] = waypoint2.maxY;
            posRight[0] = waypoint3.maxX;
            posRight[1] = waypoint3.maxY;
        }
        else if (waypoint2.maxX < waypoint3.maxX)
        {
            posLeft[0] = waypoint2.minX;
            posLeft[1] = waypoint2.maxY;
            posRight[0] = waypoint3.maxX;
            posRight[1] = waypoint3.maxY;
        }
        else
        {
            posLeft[0] = waypoint3.minX;
            posLeft[1] = waypoint3.maxY;
            posRight[0] = waypoint2.maxX;
            posRight[1] = waypoint2.maxY;
        }
    }

    static AALoadMap()
    {
        idPlanetEnd = -1;
        idMapEndXmap = -1;
        MapTraiDat = new int[3, 10]
        {
            { 42, 0, 1, 2, 3, 4, 5, 6, -1, -1 },
            { 42, 0, 1, 2, 3, 27, 28, 29, 30, -1 },
            { 30, 29, 5, 6, -1, 27, 28, 29, 5, 6 }
        };
        MapNamec = new int[3, 10]
        {
            { 43, 7, 8, 9, 11, 12, 13, 10, -1, -1 },
            { 43, 7, 8, 9, 11, 31, 32, 33, 34, -1 },
            { 34, 33, 13, 10, -1, 31, 32, 33, 13, 10 }
        };
        MapSayda = new int[3, 10]
        {
            { 44, 14, 15, 16, 17, 18, 20, 19, -1, -1 },
            { 44, 14, 15, 16, 17, 35, 36, 37, 38, -1 },
            { 38, 37, 20, 19, -1, 31, 32, 33, 20, 19 }
        };
        idMapFide = new int[3, 20]
        {
            {
                68, 69, 70, 71, 72, 64, 65, 63, 66, 67,
                73, 74, 75, 76, 77, 81, 82, 83, 79, 80
            },
            {
                -1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                0, 0, 1, 1, 1, 1, 1, 1, 1, 1
            },
            {
                0, 0, 1, 1, 1, 1, 1, 1, 1, 1,
                0, 0, 0, 0, 0, 0, 0, 0, 0, -1
            }
        };
        posLeft = new int[2];
        posMid = new int[2];
        posRight = new int[2];
    }

    public static void gotoMove(int x, int y)
    {
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();
    }

    public static int getYSd(int xSd)
    {
        int num = 50;
        int num2 = 0;
        while (num2 < 30)
        {
            num2++;
            num += 24;
            if (TileMap.tileTypeAt(xSd, num, 2))
            {
                if (num % 24 != 0)
                {
                    num -= num % 24;
                }
                break;
            }
        }
        return num;
    }

    public static void loadMap(int type)
    {
        switch (type)
        {
            case 0:
                if (posLeft[0] != -1 && posLeft[1] != -1)
                {
                    gotoMove(posLeft[0], posLeft[1]);
                }
                else
                {
                    gotoMove(60, getYSd(60));
                }
                break;
            case 1:
                if (posRight[0] != 0 && posRight[1] != 0)
                {
                    gotoMove(posRight[0], posRight[1]);
                }
                else
                {
                    gotoMove(TileMap.pxw - 60, getYSd(TileMap.pxw - 60));
                }
                break;
            case 2:
                if (posMid[0] != 0 && posMid[1] != 0)
                {
                    gotoMove(posMid[0], posMid[1]);
                }
                else
                {
                    gotoMove(TileMap.pxw / 2, getYSd(TileMap.pxw / 2));
                }
                GameScr.gI().auto = 0;
                if (Char.myCharz().isInEnterOfflinePoint() != null)
                {
                    Service.gI().charMove();
                    InfoDlg.showWait();
                    Service.gI().getMapOffline();
                    Char.ischangingMap = true;
                }
                else if (Char.myCharz().isInEnterOnlinePoint() != null)
                {
                    Service.gI().charMove();
                    Service.gI().requestChangeMap();
                    Char.isLockKey = true;
                    Char.ischangingMap = true;
                    GameCanvas.clearKeyHold();
                    GameCanvas.clearKeyPressed();
                    InfoDlg.showWait();
                }
                return;
        }
        if (!Char.ischangingMap && Char.myCharz().isInWaypoint())
        {
            Service.gI().charMove();
            if (TileMap.isTrainingMap())
            {
                Char.ischangingMap = true;
                Service.gI().getMapOffline();
            }
            else
            {
                Service.gI().requestChangeMap();
            }
            Char.isLockKey = true;
            Char.ischangingMap = true;
            GameCanvas.clearKeyHold();
            GameCanvas.clearKeyPressed();
            InfoDlg.showWait();
            return;
        }
    }

    public static int[] TimDuong(int IdXmapStart, int IdXmapEnd, int idPlanet)
    {
        int num = 0;
        if (IdXmapEnd == 24)
        {
            num = 24;
            IdXmapEnd = 2;
        }
        if (IdXmapEnd == 25)
        {
            num = 25;
            IdXmapEnd = 9;
        }
        if (IdXmapEnd == 26)
        {
            num = 24;
            IdXmapEnd = 16;
        }
        int num2 = 0;
        if (IdXmapStart == 24)
        {
            IdXmapStart = 2;
            num2 = 24;
        }
        if (IdXmapStart == 25)
        {
            IdXmapStart = 9;
            num2 = 25;
        }
        if (IdXmapStart == 26)
        {
            IdXmapStart = 16;
            num2 = 26;
        }
        int[] array = new int[2] { -1, -1 };
        int[] array2 = new int[2] { -1, -1 };
        int[] array3 = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 9; j++)
            {
                switch (idPlanet)
                {
                    case 0:
                        if (MapTraiDat[i, j] == IdXmapStart)
                        {
                            array[0] = i;
                            array[1] = j;
                            if (array[0] != -1 && array2[0] != -1)
                            {
                                break;
                            }
                        }
                        if (MapTraiDat[i, j] != IdXmapEnd)
                        {
                            continue;
                        }
                        array2[0] = i;
                        array2[1] = j;
                        if (array[0] == -1 || array2[0] == -1)
                        {
                            continue;
                        }
                        break;
                    case 1:
                        if (MapNamec[i, j] == IdXmapStart)
                        {
                            array[0] = i;
                            array[1] = j;
                            if (array[0] != -1 && array2[0] != -1)
                            {
                                break;
                            }
                        }
                        if (MapNamec[i, j] != IdXmapEnd)
                        {
                            continue;
                        }
                        array2[0] = i;
                        array2[1] = j;
                        if (array[0] == -1 || array2[0] == -1)
                        {
                            continue;
                        }
                        break;
                    default:
                        if (MapSayda[i, j] == IdXmapStart)
                        {
                            array[0] = i;
                            array[1] = j;
                            if (array[0] != -1 && array2[0] != -1)
                            {
                                break;
                            }
                        }
                        if (MapSayda[i, j] != IdXmapEnd)
                        {
                            continue;
                        }
                        array2[0] = i;
                        array2[1] = j;
                        if (array[0] == -1 || array2[0] == -1)
                        {
                            continue;
                        }
                        break;
                }
                break;
            }
            if (array2[0] != -1 && array[0] != -1 && array2[1] != -1 && array[1] != -1)
            {
                break;
            }
            array2[0] = -1;
            array[0] = -1;
            array2[1] = -1;
            array[1] = -1;
        }
        int num3 = Math.abs(array2[1] - array[1]) - 1;
        if (array[1] < array2[1])
        {
            for (int k = 0; k <= num3; k++)
            {
                if ((array2[0] == 1 && k == 4 - array[1]) || (array2[0] == 2 && k == 7 - array[1]) || (array2[0] == 2 && k == 1 - array[1] && array2[1] <= 3))
                {
                    array3[k] = 2;
                }
                else if (array2[0] == 2 && k == 0 && array2[1] <= 3)
                {
                    array3[k] = 0;
                }
                else
                {
                    array3[k] = 1;
                }
            }
        }
        else
        {
            for (int l = 0; l <= num3; l++)
            {
                if ((array2[0] == 2 && l == array[1] - 8) || (array2[0] == 2 && l == array[1] - 2 && array2[1] <= 3))
                {
                    array3[l] = 2;
                }
                else if (array2[0] == 2 && l == array[1] - 1 && array2[1] <= 3)
                {
                    array3[l] = 1;
                }
                else
                {
                    array3[l] = 0;
                }
            }
        }
        if (num == 24 || num == 25 || num == 26)
        {
            array3[num3 + 1] = 2;
        }
        if ((num2 == 24 || num2 == 25 || num2 == 26) && idPlanetEnd == TileMap.planetID)
        {
            for (int num4 = num3 + 1; num4 >= 1; num4--)
            {
                array3[num4] = array3[num4 - 1];
            }
            array3[0] = 2;
        }
        return array3;
    }

    public static int[] TimDuongFD(int idStart, int idEnd)
    {
        int[] array = new int[19]
        {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1
        };
        if (idStart == idEnd)
        {
            return array;
        }
        for (int i = 0; i < 20; i++)
        {
            if (idStart == idMapFide[0, i] && idStart >= 20)
            {
                idStart = i;
            }
            if (idEnd == idMapFide[0, i] && idEnd >= 20)
            {
                idEnd = i;
            }
            if (idStart < 20 && idEnd < 20)
            {
                break;
            }
        }
        if (idStart < idEnd)
        {
            for (int j = 0; j <= idEnd - idStart - 1; j++)
            {
                array[j] = idMapFide[1, idStart + j + 1];
            }
        }
        else
        {
            for (int num = idStart - idEnd - 1; num >= 0; num--)
            {
                array[idStart - idEnd - 1 - num] = idMapFide[2, idEnd + num];
            }
        }
        return array;
    }

    public static void NextMapXmap(int IdXmapEnd)
    {
        if (TileMap.mapID == IdXmapEnd)
        {
            return;
        }
        int[] array = TimDuong(TileMap.mapID, IdXmapEnd, TileMap.planetID);
        for (int i = 0; i <= array.Length - 1 && array[i] != -1; i++)
        {
            int mapID = TileMap.mapID;
            loadMap(array[i]);
            Thread.Sleep(200);
            while (TileMap.mapID == mapID)
            {
                Thread.Sleep(500);
                if (TileMap.mapID == mapID)
                {
                    loadMap(array[i]);
                }
            }
        }
    }

    public static void NextMapXmapFD(int IdXmapEnd)
    {
        if (TileMap.mapID == IdXmapEnd)
        {
            return;
        }
        int[] array = TimDuongFD(TileMap.mapID, IdXmapEnd);
        for (int i = 0; i < array.Length && array[i] != -1; i++)
        {
            int mapID = TileMap.mapID;
            loadMap(array[i]);
            Thread.Sleep(200);
            while (TileMap.mapID == mapID)
            {
                Thread.Sleep(500);
                if (TileMap.mapID == mapID)
                {
                    loadMap(array[i]);
                }
            }
        }
    }

    public static void outFide()
    {
        NextMapXmapFD(68);
        Thread.Sleep(500);
        Service.gI().openMenu(12);
        Service.gI().confirmMenu(0, 0);
        int mapID = TileMap.mapID;
        while (mapID == TileMap.mapID)
        {
            Thread.Sleep(500);
        }
        Thread.Sleep(200);
    }

    public static void gotoFide()
    {
        idPlanetEnd = 2;
        Xmap3HT(19);
        Thread.Sleep(500);
        Service.gI().openMenu(12);
        Service.gI().confirmMenu(0, 1);
        int mapID = TileMap.mapID;
        while (mapID == TileMap.mapID)
        {
            Thread.Sleep(500);
        }
        Thread.Sleep(200);
    }

    public static void ChangeHT(sbyte idPlanet)
    {
        if (TileMap.planetID == 0)
        {
            Service.gI().openMenu(10);
            if (idPlanet == 1)
            {
                Service.gI().confirmMenu(1, 0);
            }
            if (idPlanet == 2)
            {
                Service.gI().confirmMenu(1, 1);
            }
        }
        else if (TileMap.planetID == 1)
        {
            Service.gI().openMenu(11);
            if (idPlanet == 0)
            {
                Service.gI().confirmMenu(1, 0);
            }
            if (idPlanet == 2)
            {
                Service.gI().confirmMenu(1, 1);
            }
        }
        else if (TileMap.planetID == 2)
        {
            Service.gI().openMenu(12);
            if (idPlanet == 0)
            {
                Service.gI().confirmMenu(1, 0);
            }
            if (idPlanet == 1)
            {
                Service.gI().confirmMenu(1, 1);
            }
        }
        while (idPlanet != TileMap.planetID)
        {
            Thread.Sleep(500);
        }
        Thread.Sleep(200);
    }

    public static void xmapSD()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command(TileMap.mapNames[14], 7082014));
        myVector.addElement(new Command(TileMap.mapNames[15], 7082015));
        myVector.addElement(new Command(TileMap.mapNames[16], 7082016));
        myVector.addElement(new Command(TileMap.mapNames[17], 7082017));
        myVector.addElement(new Command(TileMap.mapNames[18], 7082018));
        myVector.addElement(new Command(TileMap.mapNames[19], 7082019));
        myVector.addElement(new Command(TileMap.mapNames[20], 7082020));
        myVector.addElement(new Command(TileMap.mapNames[26], 7082026));
        myVector.addElement(new Command(TileMap.mapNames[35], 7082035));
        myVector.addElement(new Command(TileMap.mapNames[36], 7082036));
        myVector.addElement(new Command(TileMap.mapNames[37], 7082037));
        myVector.addElement(new Command(TileMap.mapNames[38], 7082038));
        myVector.addElement(new Command(TileMap.mapNames[44], 7082044));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void xmapTD()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command(TileMap.mapNames[0], 7082000));
        myVector.addElement(new Command(TileMap.mapNames[1], 7082001));
        myVector.addElement(new Command(TileMap.mapNames[2], 7082002));
        myVector.addElement(new Command(TileMap.mapNames[3], 7082003));
        myVector.addElement(new Command(TileMap.mapNames[4], 7082004));
        myVector.addElement(new Command(TileMap.mapNames[5], 7082005));
        myVector.addElement(new Command(TileMap.mapNames[6], 7082006));
        myVector.addElement(new Command(TileMap.mapNames[24], 7082024));
        myVector.addElement(new Command(TileMap.mapNames[27], 7082027));
        myVector.addElement(new Command(TileMap.mapNames[28], 7082028));
        myVector.addElement(new Command(TileMap.mapNames[29], 7082029));
        myVector.addElement(new Command(TileMap.mapNames[30], 7082030));
        myVector.addElement(new Command(TileMap.mapNames[42], 7082042));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void xmapNM()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command(TileMap.mapNames[7], 7082007));
        myVector.addElement(new Command(TileMap.mapNames[8], 7082008));
        myVector.addElement(new Command(TileMap.mapNames[9], 7082009));
        myVector.addElement(new Command(TileMap.mapNames[10], 7082010));
        myVector.addElement(new Command(TileMap.mapNames[11], 7082011));
        myVector.addElement(new Command(TileMap.mapNames[12], 7082012));
        myVector.addElement(new Command(TileMap.mapNames[13], 7082013));
        myVector.addElement(new Command(TileMap.mapNames[25], 7082025));
        myVector.addElement(new Command(TileMap.mapNames[31], 7082031));
        myVector.addElement(new Command(TileMap.mapNames[32], 7082032));
        myVector.addElement(new Command(TileMap.mapNames[33], 7082033));
        myVector.addElement(new Command(TileMap.mapNames[34], 7082034));
        myVector.addElement(new Command(TileMap.mapNames[43], 7082043));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void xmapNappa()
    {
        MyVector myVector = new MyVector();
        for (int i = 0; i < 20; i++)
        {
            myVector.addElement(new Command(TileMap.mapNames[idMapFide[0, i]], 7082000 + idMapFide[0, i]));
        }
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void Xmap3HT(int IdXmapEnd)
    {
        if (TileMap.mapID >= 63 && TileMap.mapID <= 83)
        {
            outFide();
            Thread.Sleep(1000);
        }
        if (idPlanetEnd == TileMap.planetID)
        {
            NextMapXmap(IdXmapEnd);
            return;
        }
        if (TileMap.planetID == 0 && TileMap.mapID != 24)
        {
            NextMapXmap(24);
        }
        else if (TileMap.planetID == 1 && TileMap.mapID != 25)
        {
            NextMapXmap(25);
        }
        else if (TileMap.planetID == 2 && TileMap.mapID != 26)
        {
            NextMapXmap(26);
        }
        Thread.Sleep(1000);
        ChangeHT(idPlanetEnd);
        Thread.Sleep(1000);
        NextMapXmap(IdXmapEnd);
    }

    public static void XmapFide(int idMapEnd)
    {
        if (TileMap.mapID < 63 || TileMap.mapID > 83)
        {
            gotoFide();
            Thread.Sleep(1000);
        }
        NextMapXmapFD(idMapEnd);
    }

    public static void actionPerform(int idAction)
    {
        if (idAction == 240201)
        {
            idPlanetEnd = 2;
            new Thread(xmapSD).Start();
        }
        if (idAction == 240202)
        {
            idPlanetEnd = 0;
            new Thread(xmapTD).Start();
        }
        if (idAction == 240203)
        {
            idPlanetEnd = 1;
            new Thread(xmapNM).Start();
        }
        if (idAction == 240204)
        {
            new Thread(xmapNappa).Start();
        }
        if (idAction >= 7082000 && idAction <= 7082063)
        {
            idMapEndXmap = idAction - 7082000;
            new Thread((ThreadStart)delegate
            {
                Xmap3HT(idMapEndXmap);
            }).Start();
        }
        if (idAction >= 7082063 && idAction <= 7082083)
        {
            idMapEndXmap = idAction - 7082000;
            new Thread((ThreadStart)delegate
            {
                XmapFide(idMapEndXmap);
            }).Start();
        }
    }

    public static void KeyPress()
    {
        switch (GameCanvas.keyAsciiPress)
        {
            case 106:
                loadMap(0);
                break;
            case 107:
                loadMap(2);
                break;
            case 108:
                loadMap(1);
                break;
        }
    }
}
