using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private static float bulletDecalLifetime = 5f;
    public static PostProcessVolume globalPostProcessVolume;

    private static LayerMask bulletCollisionLayerMask = (1 << 0 | 1 << 20 | 1 << 22 | 1 << 17);

    private static int[] player3PLayers = new int[]{ 9, 10, 11, 12 };
    private static int[] player1PLayers = new int[]{ 13, 14, 15, 16 };

    private static PlayerInfo[] playerInfoList = new PlayerInfo[4];
    private static HumanoidTransferData[] playerTransferDataList = new HumanoidTransferData[4];
    private static HumanoidBaseScript[] playerInstanceList = new HumanoidBaseScript[4];
    private static int playerCount = 0;
    private static bool verticalSplitScreen = false;

    private static int timeOfDay = 0;
    

    // loaded animations clips
    public static List<AnimationClip> LoadedAnimationClips = new List<AnimationClip>();

    /*
     *  index #:        bone name
     *  
     *      0:          Hips   
     *      1:          Left Leg Upper
     *      2:          Right Leg Upper
     *      3:          Left Leg Lower   
     *      4:          Right Leg Lower  
     *      5:          Left Foot 
     *      6:          Right Foot
     *      7:          Spine
     *      8:          Chest
     *      9:          Neck
     *      10:         Head
     *      11:         Left Shoulder
     *      12:         Right Shoulder
     *      13:         Left Arm Upper
     *      14:         Right Arm Upper
     *      15:         Left Arm Lower
     *      16:         Right Arm Lower
     *      17:         Left Hand
     *      18:         Right hand
     * 
     */
    public static HumanBodyBones[] humanBodyBoneArray = new HumanBodyBones[]
    {
        HumanBodyBones.Hips,
        HumanBodyBones.LeftUpperLeg,
        HumanBodyBones.RightUpperLeg,
        HumanBodyBones.LeftLowerLeg,
        HumanBodyBones.RightLowerLeg,
        HumanBodyBones.LeftFoot,
        HumanBodyBones.RightFoot,
        HumanBodyBones.Spine,
        HumanBodyBones.Chest,
        HumanBodyBones.Neck,
        HumanBodyBones.Head,
        HumanBodyBones.LeftShoulder,
        HumanBodyBones.RightShoulder,
        HumanBodyBones.LeftUpperArm,
        HumanBodyBones.RightUpperArm,
        HumanBodyBones.LeftLowerArm,
        HumanBodyBones.RightLowerArm,
        HumanBodyBones.LeftHand,
        HumanBodyBones.RightHand

    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        globalPostProcessVolume = gameObject.GetComponent<PostProcessVolume>();

        loadScene(2);

        
        // TEST_CODE: spawn player manually
        addPlayer(0, 0);
        loadPlayerInstances();

        playerInstanceList[0].transform.position = new Vector3(0,10,-0);
        playerInstanceList[0].gameObject.SetActive(true);

        // TEST_CODE: equip weapons to player
        GameObject w = ItemHelper.loadWeaponPrefab(WeaponIndex.BurstRifle);
        w.SetActive(true);
        w.GetComponent<WeaponData>().initialize();
        playerInstanceList[0].equipWeaponToSlot(w, 1);
        

        GameObject x = ItemHelper.loadWeaponPrefab(WeaponIndex.TestAssaultRifle);
        x.SetActive(true);
        x.GetComponent<WeaponData>().initialize();
        playerInstanceList[0].equipWeaponToSlot(x, 0);
        
        

    }

    private void Update()
    {
        // add player on input

        if (Input.GetKeyDown(KeyCode.X) && playerInfoList[0]==null)
            addPlayerNumber( 0, 0, 1 );


        if (Input.GetButtonDown("SubmitJ1"))
        {
            //Debug.Log("Pressed");
            if (playerInfoList[0] == null)
            {
                addPlayerNumber(1, 0, 1);
            }
            else
            {
                if(playerInfoList[0].getJoyconNumber()!=1 && playerInfoList[1] == null)
                    addPlayerNumber(1, 0, 2);
            }
        }




    }




    public static bool addPlayerNumber( int joyconNum, int teamNum, int playerNum)
    {
        if (playerInfoList[playerNum - 1] == null)
        {
            playerInfoList[playerNum - 1] = new PlayerInfo(playerNum, joyconNum, teamNum);
            playerCount++;
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool addPlayer( int joyconNumber, int teamNumber)
    {
        if (playerCount == 4) return false;

        for(int i=0; i<4; i++)
        {
            if (playerInfoList[i] == null)
            {
                playerInfoList[i] = new PlayerInfo( i+1, joyconNumber, teamNumber );
                playerCount++;
                return true;
            }
        }
        return false;
    }

    public static bool playerJoined(int playerNum) { return playerInfoList[playerNum-1] != null; }

    public static int getPlayerCount() { return playerCount; }

    public static PlayerInfo GetPlayerInfo( int playerNum) { return playerInfoList[playerNum-1]; }

    public static HumanoidBaseScript getPlayerInstance( int playerNum) { return playerInstanceList[playerNum-1]; }
    




    public static Vector2 getPlayerScreenRectAnchor( int playerNum )
    {
        //Debug.Log(playerNum);
        switch (playerCount)
        {
            case 2:
                if (verticalSplitScreen)
                {
                    if (playerNum == 1)
                        return new Vector2(0.25f, 0.5f);
                    else
                        return new Vector2(0.75f,0.5f);
                }
                else
                {
                    if (playerNum == 1)
                        return new Vector2(0.5f, 0.75f);
                    else
                        return new Vector2(0.5f, 0.25f);
                }
            case 3:
                if (verticalSplitScreen)
                {
                    switch (playerNum)
                    {
                        case 2:
                            return new Vector2(0.75f,0.75f);
                        case 3:
                            return new Vector2(0.75f,0.25f);
                        default:
                            return new Vector2(0.25f,0.5f);
                    }
                }
                else
                {
                    switch (playerNum)
                    {
                        case 2:
                            return new Vector2(0.25f,0.25f);
                        case 3:
                            return new Vector2(0.75f, 0.25f);
                        default:
                            return new Vector2(0.5f,0.75f);
                    }
                }
            case 4:
                switch (playerNum)
                {
                    case 2:
                        return new Vector2(0.75f,0.75f);
                    case 3:
                        return new Vector2(0.25f, 0.25f);
                    case 4:
                        return new Vector2(0.75f,0.25f);
                    default:
                        return new Vector2(0.25f,0.75f);
                }
            default:
                return new Vector2(0.5f, 0.5f);

        }
    }

    public static Vector2 getPlayerScreenRectScale( int playerNum )
    {
        switch (playerCount)
        {
            case 2:
                if (verticalSplitScreen)
                    return new Vector2(0.5f, 1);
                else
                    return new Vector2(1, 0.5f);
            case 3:
                if (playerNum == 1)
                {
                    if (verticalSplitScreen)
                        return new Vector2(0.5f, 1);
                    else
                        return new Vector2(1, 0.5f);
                }
                else
                {
                    return new Vector2(0.5f,0.5f);
                }
            case 4:
                return new Vector2(0.5f,0.5f);
            default:
                return new Vector2(1,1);
        }
    }

    public static Vector2 getCameraViewMin( int playerNum)
    {
        switch (playerCount)
        {
            case 2:
                if (verticalSplitScreen)
                {
                    if (playerNum == 1)
                        return new Vector2();
                    else
                        return new Vector2(0.5f, 0);
                }
                else
                {
                    if (playerNum == 1)
                        return new Vector2(0,0.5f);
                    else
                        return new Vector2();
                }
            case 3:
                if (verticalSplitScreen)
                {
                    switch (playerNum)
                    {
                        case 2:
                            return new Vector2(0.5f, 0.5f);
                        case 3:
                            return new Vector2(0.5f, 0);
                        default:
                            return new Vector2();
                    }
                }
                else
                {
                    switch (playerNum)
                    {
                        case 2:
                            return new Vector2();
                        case 3:
                            return new Vector2(0.5f, 0);
                        default:
                            return new Vector2(0,0.5f);
                    }
                }
            case 4:
                switch (playerNum)
                {
                    case 2:
                        return new Vector2(0.5f, 0.5f);
                    case 3:
                        return new Vector2();
                    case 4:
                        return new Vector2(0.5f, 0);
                    default:
                        return new Vector2(0f, 0.5f);
                }
            default:
                return new Vector2();
        }
    }

    public static Vector2 getCameraViewMax(int playerNum)
    {
        switch (playerCount)
        {
            case 2:
                if (verticalSplitScreen)
                {
                    if (playerNum == 1)
                        return new Vector2(0.5f,1);
                    else
                        return new Vector2(1,1);
                }
                else
                {
                    if (playerNum == 1)
                        return new Vector2(1, 1);
                    else
                        return new Vector2(1,0.5f);
                }
            case 3:
                if (verticalSplitScreen)
                {
                    switch (playerNum)
                    {
                        case 2:
                            return new Vector2(1,1);
                        case 3:
                            return new Vector2(1, 0.5f);
                        default:
                            return new Vector2(0.5f,1);
                    }
                }
                else
                {
                    switch (playerNum)
                    {
                        case 2:
                            return new Vector2(0.5f,0.5f);
                        case 3:
                            return new Vector2(1, 0.5f);
                        default:
                            return new Vector2(1,1);
                    }
                }
            case 4:
                switch (playerNum)
                {
                    case 2:
                        return new Vector2(1, 1);
                    case 3:
                        return new Vector2(0.5f,0.5f);
                    case 4:
                        return new Vector2(1, 0.5f);
                    default:
                        return new Vector2(0.5f, 1);
                }
            default:
                return new Vector2(1,1);
        }
    }






    public static void loadPlayerInstance( int playerNumber)
    {

        GameObject wep;
        WeaponData wd;
        WeaponTransferData[] wtds;
        if (playerInfoList[playerNumber-1] != null)
        {
            playerInstanceList[playerNumber - 1] = loadPrefab("Mech1PlayerPref", "Characters/Humanoid/Mech1/").GetComponent<HumanoidBaseScript>();
            //playerInstanceList[playerNumber - 1] = loadPrefab("HumanoidPlayerPref", "Characters/Humanoid/Base/").GetComponent<HumanoidBaseScript>();
            playerInstanceList[playerNumber - 1].setup(
                    playerInfoList[playerNumber - 1].getPlayerNumber() ,
                    playerInfoList[playerNumber - 1].getJoyconNumber()
                );

            if (playerTransferDataList[playerNumber - 1])
            {
                wtds = playerTransferDataList[playerNumber - 1].getWeaponTransferDatas();
                for (int j = 0; j < wtds.Length; j++)
                {
                    if (wtds[j])
                    {
                        wep = ItemHelper.loadWeaponPrefab(wtds[j].getWeaponIndex());
                        wd = wep.GetComponent<WeaponData>();
                        wd.initialize();
                        wd.setAmmo(wtds[j].getAmmo());
                        wd.setClipAmmo(wtds[j].getClipAmmo());
                        playerInstanceList[playerNumber - 1].equipWeaponToSlot(wep, j);
                    }

                }
            }

        }
    }

    public static void loadPlayerInstances()
    {
        

        for (int i = 1; i < 5; i++)
        {
            loadPlayerInstance(i);
        }
    }

    public static void loadScene(int index)
    {
        if (!SceneManager.GetSceneByBuildIndex(index).isLoaded)
            SceneManager.LoadScene(index, LoadSceneMode.Additive);
    }

    public static void asyncLoadScene(int index)
    {
        if (!SceneManager.GetSceneByBuildIndex(index).isLoaded)
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }

    public static void unloadScene(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index).isLoaded)
            SceneManager.UnloadSceneAsync(index);
    }






    public static GameObject loadPrefab(string name, string directory)
    {
        GameObject loadedObject = GameObject.Find(name);
        //Debug.Log(directory + name);

        if (loadedObject == null)
        {
            loadedObject = Instantiate(Resources.Load(directory+name)) as GameObject;
        }
        else
        {
            loadedObject = Instantiate(loadedObject, null, false);
        }
        loadedObject.SetActive(false);

        loadedObject.name = name;
        return loadedObject;
    }
    

    public static AnimationClip loadAnimationClip(string name, string directory)
    {
        AnimationClip loadedClip = Instantiate(Resources.Load(directory+name)) as AnimationClip;
        loadedClip.name = loadedClip.name.Remove(loadedClip.name.Length - 7);
        //Debug.Log("Loaded Humanoid Animation: " + loadedClip.name);


        return loadedClip;
    }
    






    public static bool playerNumberIsValid(int player)
    {
        if (player > 0 && player < 5)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Get the 3rd person layers of the other players.
    /// </summary>
    /// <param name="player">this player's layer</param>
    /// <returns></returns>
    public static LayerMask getPlayerLayerMask( int player )
    {
        LayerMask playerLayerMask = 0;
        if (playerNumberIsValid(player))
        {
            for (int j = 1; j < 5; j++)
            {
                if (player != j)
                {
                    playerLayerMask |= (1 << get3PLayer(j));
                }
            }
        }
        return playerLayerMask;
    }

    public static LayerMask getAllPlayersLayerMask()
    {
        LayerMask playerLayerMask = 0;
        for (int j = 1; j < 5; j++)
            playerLayerMask |= (1 << get3PLayer(j));
        return playerLayerMask;
    }


    public static AnimatorOverrideController SwapAnimationInState( Animator animator, int state, AnimationClip animationClip, AnimatorOverrideController inAOC ) {

        AnimatorOverrideController aoc;
        if (inAOC == null)
            aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        else
            aoc = inAOC;
        
        List<KeyValuePair<AnimationClip, AnimationClip>> anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        if (aoc != null)
        {
            for (int j = 0; j < aoc.animationClips.Length; j++)
            {
                if (j == state)
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[j], animationClip ));
                else
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[j], aoc.animationClips[j]));

            }
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;

        }
        return aoc;
    }

    public static void printAnimations( Animator animator )
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        Debug.Log( animator.name + "Number of clips: "+ aoc.animationClips.Length);
        if (aoc != null)
        {
            for (int j = 0; j < aoc.animationClips.Length; j++)
            {
                Debug.Log(j + " : " + aoc.animationClips[j]);

            }

        }
    }

    public static HumanBodyBones getHumanBodyBone(int index) { return humanBodyBoneArray[index]; }
    public static float getBulletDecalLifeTime() { return bulletDecalLifetime; }
    public static LayerMask getBulletCollisionLayerMask() { return bulletCollisionLayerMask; }

    public static int get3PLayer( int player)
    {
        if (playerNumberIsValid(player))
            return player3PLayers[player - 1];
        else
            return 20;
    }
    public static int get1PLayer( int player)
    {
        if (playerNumberIsValid(player))
            return player1PLayers[player - 1];
        else
            return 20;
    }
    

    public static Vector3 slerpDirections(Vector3 oldVec, Vector3 newVec, float t)
    {
        return slerpDirectionsQuaternion(oldVec, newVec, t) * new Vector3(0, 0, 1f);
    }

    public static Quaternion slerpDirectionsQuaternion(Vector3 oldVec, Vector3 newVec, float t)
    {
        return Quaternion.Slerp(
            Quaternion.FromToRotation(new Vector3(0, 0, 1), oldVec),
            Quaternion.FromToRotation(new Vector3(0, 0, 1), newVec),
            t
            );
    }

}
