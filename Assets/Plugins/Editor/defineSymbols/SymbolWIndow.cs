using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

/// <summary>
/// シンボルを設定するウィンドウを管理するクラス
/// </summary>
public class SymbolWindow : EditorWindow
{
    //===================================================================================================
    // クラス
    //===================================================================================================

    /// <summary>
    /// シンボルのデータを管理するクラス
    /// </summary>
    private class SymbolData
    {
        public string   Name        { get; private set; }   // 定義名を返します
        public string   Comment     { get; private set; }   // コメントを返します
        public bool     IsEnable    { get; set;         }   // 有効かどうかを取得または設定します

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SymbolData( XmlNode node )
        {
            Name    = node.Attributes[ "name"    ].Value;
            Comment = node.Attributes[ "comment" ].Value;
        }
    }
    
    //===================================================================================================
    // 定数
    //===================================================================================================

    private const string ITEM_NAME      = "Tools/Symbols";              // コマンド名
    private const string WINDOW_TITLE   = "Symbols";                    // ウィンドウのタイトル
    private const string XML_PATH       = "Assets/Plugins/Editor/defineSymbols/symbols.xml";  // 読み込む .xml のファイルパス
    
    //===================================================================================================
    // 変数
    //===================================================================================================

    private static Vector2      mScrollPos;     // スクロール座標
    private static SymbolData[] mSymbolList;    // シンボルのリスト
    
    //===================================================================================================
    // 静的関数
    //===================================================================================================

    /// <summary>
    /// ウィンドウを開きます
    /// </summary>
    [MenuItem( ITEM_NAME )]
    private static void Open()
    {
        var window = GetWindow<SymbolWindow>( true, WINDOW_TITLE );
        window.Init();
    }
    
    //===================================================================================================
    // 関数
    //===================================================================================================

    /// <summary>
    /// 初期化する時に呼び出します
    /// </summary>
    private void Init()
    {
        var document = new XmlDocument();
        document.Load( XML_PATH );

        var root        = document.GetElementsByTagName( "root" )[ 0 ];
        var symbolList  = new List<XmlNode>();
            
        foreach ( XmlNode n in root.ChildNodes )
        {
            if ( n.Name == "symbol" )
            {
                symbolList.Add( n );
            }
        }

        mSymbolList = symbolList
            .Select( c => new SymbolData( c ) )
            .ToArray();

        var defineSymbols = PlayerSettings
            .GetScriptingDefineSymbolsForGroup( EditorUserBuildSettings.selectedBuildTargetGroup )
            .Split( ';' );

        foreach ( var n in mSymbolList )
        {
            n.IsEnable = defineSymbols.Any( c => c == n.Name );
        }
    }
        
    /// <summary>
    /// GUI を表示する時に呼び出されます
    /// </summary>
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        mScrollPos = EditorGUILayout.BeginScrollView( 
            mScrollPos, 
            GUILayout.Height( position.height ) 
        );
        foreach ( var n in mSymbolList )
        {
            EditorGUILayout.BeginHorizontal( GUILayout.ExpandWidth( true ) );
            n.IsEnable = EditorGUILayout.Toggle( n.IsEnable, GUILayout.Width( 16 ) );
            if ( GUILayout.Button( "Copy" ) )
            {
                EditorGUIUtility.systemCopyBuffer = n.Name;
            }
            EditorGUILayout.LabelField( n.Name, GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 0 ) );
            EditorGUILayout.LabelField( n.Comment, GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 0 ) );
            EditorGUILayout.EndHorizontal();
        }
        if ( GUILayout.Button( "Save" ) )
        {
 
            var defineSymbols = mSymbolList
                .Where( c => c.IsEnable )
                .Select( c => c.Name )
                .ToArray();

            var str = string.Join( ";", defineSymbols );

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, 
                str
            );

            if(str.IndexOf("DEMO")>=0){
                Debug.Log("SetDemo");
                PlayerSettings.SplashScreen.show = true;
                PlayerSettings.applicationIdentifier = "com.kitasenjudesign.meisai.retaildemo";

            }else{

                PlayerSettings.SplashScreen.show = false;
                PlayerSettings.applicationIdentifier = "com.kitasenjudesign.mesai";
            }


            //追記
            if(str.IndexOf("VJ")>=0){
                
                PlayerSettings.productName = "VJ";
                PlayerSettings.allowedAutorotateToLandscapeLeft=true;
                PlayerSettings.allowedAutorotateToPortrait=false;
                PlayerSettings.allowedAutorotateToPortraitUpsideDown=false;                
                PlayerSettings.allowedAutorotateToLandscapeRight=false;
                PlayerSettings.applicationIdentifier = "com.kitasenjudesign.mesaiVJ";
  
                PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;

                //buildするシーン変える
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[]{
                    new EditorBuildSettingsScene("Assets/_Kitasenju/Scenes/VJ.unity", true),
                    new EditorBuildSettingsScene("Assets/_Kitasenju/Scenes/main.unity", true)
                };

                //GameViewSizeHelper.ChangeGameViewSize(
                //    GameViewSizeGroupType.iOS,
                //    GameViewSizeHelper.GameViewSizeType.FixedResolution, 2224, 1668, "2224x1668"
                //);

            }else if(str.IndexOf("DEMO")<0){
                
                PlayerSettings.productName = "MEISAI";
                                
                PlayerSettings.allowedAutorotateToPortrait=true;
                PlayerSettings.allowedAutorotateToPortraitUpsideDown=false;
                PlayerSettings.allowedAutorotateToLandscapeLeft=false;
                PlayerSettings.allowedAutorotateToLandscapeRight=false;
                PlayerSettings.applicationIdentifier = "com.kitasenjudesign.mesai";
                
                PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneOnly;

                //PlayerSettings.SplashScreen.show = true;
                
                //buildするシーン変える
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[]{
                    new EditorBuildSettingsScene("Assets/_Kitasenju/Scenes/VJ.unity", false),
                    new EditorBuildSettingsScene("Assets/_Kitasenju/Scenes/main.unity", true)
                };

                //var n = new Texture2D[]{
                //    Resources.Load("icon/icon") as Texture2D
                //};                
                //PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, n);  
                //GameViewSizeHelper.ChangeGameViewSize(
                //    GameViewSizeGroupType.iOS,
                //    GameViewSizeHelper.GameViewSizeType.FixedResolution, 1125, 2436, "1125x2436"
                //);

            }

            Close();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    
}