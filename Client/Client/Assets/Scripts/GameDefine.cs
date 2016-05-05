using UnityEngine;
using System.Collections;

public class Tags 
{
    public const string AvatarCamTag = "AvatarCamera";
    public const string PlayerTag = "Player";
    public const string WeaponTag = "Weapon";
}

public class GameDefine
{
    public const string GirlPrefabPath = "Prefabs/Avatar/Girl";
    public const string LadyPrefabPath = "Prefabs/Avatar/Lady";
    public const string GunPrefabPrefix = "Prefabs/Gun/";
    public const string WeaponTypeTemplatePath = "Prefabs/UI/weapontypetemplate";
    public const string LoadingSceneUIPath = "Prefabs/UI/loadingscene";
    public const string WeaponToggleUIPath = "Prefabs/UI/weapontoggle";
    public const string FightUIPath = "Prefabs/UI/fight";
    public const string JoystickUIPath = "Prefabs/UI/joystick";
    public const string EscUIPath = "Prefabs/UI/esc";
    public const string FightSettleUIPath = "Prefabs/UI/fightsettle";

    public const string GunIconPathPrefix = "UI/Image/Gun_icon/";
    public const string FightClockNumPrefix = "UI/Image/Common_Word_Shuzi_";

    public const string MinimapPathPrefix = "UI/Minimap/SnapShots/";
    public const string MinimapShaderName = "SpraySoldier/Function/Minimap";

    public enum eSceneType
    {
        eStartScene,
        eLoadingScene,
        eWeaponLoadingScene,
        eFightScene
    }
}

public class CsvDefine
{
    public const string GunCfgPath = "Csv/Gun";
    public const string LoadingDescCfgPath = "Csv/LoadingDesc";
    public const string MinimapCfgPath = "Csv/Minimap";
}

