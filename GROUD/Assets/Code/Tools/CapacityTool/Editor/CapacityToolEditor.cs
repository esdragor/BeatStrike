using System;
using System.Collections.Generic;
using System.Linq;
using DMTimeArea;
using UnityEditor;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

public class CapacityToolEditor : SimpleTimeArea
{
   private Event eventListener;
   public Pattern currentPattern;
   private InteractionKey selectedInteractionKey;
   private InteractionTimelineKey selectedKey;
   public List<InteractionTimelineKey> allInteractions;

   private int selectedPatternInDirectory = 0;
   private string[] patternsInDirectory;
   
   private string directoryPath;

   private PatternToolsInterfaceData interfaceData;
   

   #region Assets
   
   public Texture _tapIcon;

   #endregion
   
   #region Areas

   private Rect rectTotalArea;
   private Rect rectContent;
   private Rect rectTimeRuler;

   private Rect rectTopBar;
   private Rect rectLeft;
   private Rect rectLeftTopToolBar;
   
   private Rect botContent;
   private Rect topContent;
   private Rect contentSeparator;

   private Rect previewRect;
   private Rect patternDropdownRect => interfaceData.patternDropdownRect;
   private Rect selectedInteractionRect => interfaceData.selectedInteractionRect;
   private Rect patternDataRect => interfaceData.patternDataRect;
   private Rect interactionListRect => interfaceData.interactionListRect;

   #endregion
   
   #region Timeline Parameters

   private float lastUpdateTime;

   private double runningTime = 10.0f;
   protected override double RunningTime { get => runningTime; set => runningTime = value; }

   private static double cutOffTime = 15.0f;
   protected override double CutOffTime { get => cutOffTime; set => cutOffTime = value; }

   private float LEFTWIDTH => interfaceData.leftWidth;

   private bool IsPlaying { get; set; }

   protected override bool IsLockedMoveFrame => IsPlaying || Application.isPlaying;

   protected override bool IsLockDragHeaderArrow => IsPlaying;

   public override Rect _rectTimeAreaTotal => rectTotalArea;

   public override Rect _rectTimeAreaContent => rectContent;

   public override Rect _rectTimeAreaRuler => rectTimeRuler;

   protected override float sequencerHeaderWidth => LEFTWIDTH;

   protected override int toolbarHeight { get; } = 530;
   

   #endregion
   
   #region Initialization

   [MenuItem("Tools/Capacity Tool Editor")]
   public static void InitWindow()
   {
      var window = GetWindow<CapacityToolEditor>(false, "Capacity Tool Editor");
      window.minSize = new Vector2(1170f, 760f);
      window.maxSize = new Vector2(1170f, 760f);
      window.Show();
   }

   private void OnEnable()
   {
      allInteractions = new List<InteractionTimelineKey>();
      selectedKey = null;
      selectedInteractionKey = null;
      EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
      lastUpdateTime = (float)EditorApplication.timeSinceStartup;
   }

   private void OnDisable()
   {
      EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
   }

   private void OnEditorUpdate()
   {
      if (!Application.isPlaying && this.IsPlaying)
      {
         double fTime = (float)EditorApplication.timeSinceStartup - lastUpdateTime;
         RunningTime += Math.Abs(fTime) * 1.0f;
         if (RunningTime >= CutOffTime)
         {
            PausePreView();
         }
      }

      lastUpdateTime = (float)EditorApplication.timeSinceStartup;
      Repaint();
   }

   #endregion
   
   private void OnGUI()
   {
      SetPatternsInDropDown();
      LoadAssets(); 
      EditorEventListener();
      DrawPreviewArea();
      DrawPatternListDropdown();
      DrawSelectedInteractionArea(); 
      DrawPatternDataArea(); 
      DrawInteractionListArea(); 
      DrawTimeline();
   }
   

   #region Editor Drawing

   void DrawPreviewArea()
   {
      previewRect = interfaceData.previewRect;
      EditorGUI.DrawRect(previewRect, Color.red);
   }
   
   void DrawPatternListDropdown()
   {
      EditorGUI.DrawRect(patternDropdownRect, Color.magenta);

      GUILayout.BeginArea(patternDropdownRect);
      selectedPatternInDirectory = EditorGUILayout.Popup("Select a pattern...", selectedPatternInDirectory, patternsInDirectory);
      LoadPattern(patternsInDirectory[selectedPatternInDirectory]);
      GUILayout.EndArea();
   }

   void DrawPatternDataArea()
   {
      EditorGUI.DrawRect(patternDataRect, Color.yellow); //Draw background
      
      if(currentPattern == null) return;

      GUILayout.BeginArea(patternDataRect);
      GUILayout.Space(20);
      
      currentPattern.patternName = EditorGUILayout.TextField("Pattern Name", currentPattern.patternName);

      GUILayout.EndArea();

   }

   void DrawInteractionListArea()
   {
      EditorGUI.DrawRect(interactionListRect, Color.green);
      
      GUILayout.BeginArea(interactionListRect, "Pattern List in Capacity");
      GUILayout.Space(20);

      
      GUILayout.EndArea();
   }

   void DrawSelectedInteractionArea()
   {
      EditorGUI.DrawRect(selectedInteractionRect, Color.cyan);
      GUILayout.BeginArea(selectedInteractionRect);
      
      DrawSelectedKeyData();
      
      GUILayout.EndArea();

   }
   
   private void DrawSelectedKeyData()
   {
      GUILayout.Space(20f);
      if (selectedInteractionKey != null)
      {
         selectedInteractionKey.timeCode = TimeAsString(selectedInteractionKey.time);
         EditorGUILayout.LabelField($"Time Code : {selectedInteractionKey.timeCode}");
         selectedInteractionKey.time = EditorGUILayout.Slider((float)selectedInteractionKey.time, 0f, 10f);
      }
   }

   private void DrawTimeline()
   {
      Rect rectMainBodyArea = new Rect(0, toolbarHeight, base.position.width, this.position.height - toolbarHeight);
      rectTopBar = new Rect(0, 0, this.position.width, toolbarHeight);
      rectLeft = new Rect(rectMainBodyArea.x, rectMainBodyArea.y + timeRulerHeight, LEFTWIDTH, rectMainBodyArea.height);
      rectLeftTopToolBar = new Rect(rectMainBodyArea.x, rectMainBodyArea.y, LEFTWIDTH, timeRulerHeight);

      rectTotalArea = new Rect(rectMainBodyArea.x + LEFTWIDTH, rectMainBodyArea.y, base.position.width - LEFTWIDTH, rectMainBodyArea.height);
      rectTimeRuler = new Rect(rectMainBodyArea.x + LEFTWIDTH, rectMainBodyArea.y, base.position.width - LEFTWIDTH, timeRulerHeight);
      rectContent = new Rect(rectMainBodyArea.x + LEFTWIDTH, rectMainBodyArea.y + timeRulerHeight, base.position.width - LEFTWIDTH, rectMainBodyArea.height - timeRulerHeight);

      InitTimeArea(false, false, true, true);
      DrawTimeAreaBackGround();
      DrawSplitAreaContent();
      DrawKeysOnTimeline();
      OnTimeRulerCursorAndCutOffCursorInput();
      DrawTimeRulerArea();
      DrawLeftContent();
      DrawLeftTopToolBar();
   }
   
   #endregion
   
   #region Editor Utilities
   void LoadAssets()
   {
      interfaceData =
         (EditorGUIUtility.Load("Assets/Code/Tools/CapacityTool/EditorResources/PatternInterfaceData.asset") as PatternToolsInterfaceData);
      _tapIcon = (EditorGUIUtility.Load("Assets/Code/Tools/CapacityTool/EditorResources/tapimage.png") as Texture);
   }
   void EditorEventListener()
   {
      eventListener = Event.current;
      
      if (botContent.Contains(eventListener.mousePosition) && eventListener.type == EventType.ContextClick)
      {
         Vector2 mousePositionWhenClick = eventListener.mousePosition;
         GenericMenu menu = new GenericMenu();
      
         menu.AddDisabledItem(new GUIContent("Timeline Actions List")); 
         menu.AddItem(new GUIContent("Add a bot pattern key."), false, AddKeyOnTimeline(1, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick)));
         menu.ShowAsContext();
      
         eventListener.Use();
      }

      if (topContent.Contains(eventListener.mousePosition) && eventListener.type == EventType.ContextClick)
      {
         Vector2 mousePositionWhenClick = eventListener.mousePosition;
         GenericMenu menu = new GenericMenu();
      
         menu.AddDisabledItem(new GUIContent("Timeline Actions List")); 
         menu.AddItem(new GUIContent("Add a top pattern key."), false, AddKeyOnTimeline(0, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick)));
         menu.ShowAsContext();
      
         eventListener.Use();
      }
   }

   #endregion

   #region Directory Utilities

   void SetPatternsInDropDown()
   {
      directoryPath = $"Scriptable/Pattern/";
      Pattern[] patterns = Resources.LoadAll<Pattern>(directoryPath);
      patternsInDirectory = new string[patterns.Length + 1];
      
      for (int i = 0; i < patterns.Length; i++)
      {
         patternsInDirectory[i] = patterns[i].patternName;
      }

      patternsInDirectory[^1] = "Add New Pattern";
   }

   void CreatePattern()
   {
      currentPattern = new Pattern();
   }
   
   void SavePattern()
   {
      directoryPath = $"Assets/Scriptable/Capacities/{currentPattern.patternName}.asset";
      
      foreach (InteractionTimelineKey t in allInteractions)
      {
         currentPattern.interactions.Add(t.interactionKey);
      }

      AssetDatabase.CreateAsset(currentPattern, directoryPath);
      SetPatternsInDropDown();
   }

   void LoadPattern(string patternName)
   {
      if (currentPattern == null || currentPattern.patternName != patternName)
      {
         SavePattern();
         directoryPath = $"Assets/Resources/Scriptable/Pattern/{patternName}.asset";
         currentPattern = (Pattern) EditorGUIUtility.Load(directoryPath);
         allInteractions = new List<InteractionTimelineKey>();
         for (int i = 0; i < currentPattern.interactions.Count; i++)
         {
            allInteractions[i].interactionKey = currentPattern.interactions[i];
         }
      }
   }

   #endregion
   
   #region Timeline Drawing

   private void DrawSplitAreaContent()
   {
      topContent = new Rect(rectContent.x, rectContent.y, rectContent.width, rectContent.height * 0.5f);
      contentSeparator = new Rect(rectContent.x, rectContent.y + (rectContent.height * 0.5f) - 2f, rectContent.width, 2f);
      botContent = new Rect(rectContent.x, rectContent.y + (rectContent.height * 0.5f), rectContent.width, rectContent.height * 0.5f);
      
      EditorGUI.DrawRect(topContent, new Color(0,0,0,0));
      EditorGUI.DrawRect(contentSeparator, Color.grey);
      EditorGUI.DrawRect(botContent, new Color(0,0,0,0));
   }
   
   
   
   private void DrawKeysOnTimeline()
   {
      if(allInteractions.Count <= 0) return;

      for (int i = 0; i < allInteractions.Count; i++)
      {
         InteractionTimelineKey iKey = allInteractions[i];
         double timeToPos = TimeToPixel(iKey.interactionKey.time);
         float positionY = iKey.row == 1 ? botContent.y + (botContent.height * 0.5f) : topContent.y + (topContent.height * 0.5f);
         Rect tapIconPosition = new Rect((float)timeToPos - 13f, positionY - 15f, 30, 30);
         Rect verticalLine = new Rect((float)timeToPos,  rectContent.y, 2f, rectContent.height);
         EditorGUI.DrawRect(verticalLine, Color.blue);
         GUIStyle interactionStyle = new GUIStyle();
         interactionStyle.normal.background = null;
         
         GUI.DrawTexture(tapIconPosition, _tapIcon);

         if (tapIconPosition.Contains(eventListener.mousePosition))
         {
            switch (eventListener.type)
            {
               case EventType.MouseDown:
                  if(eventListener.button == 0) SelectKeyOnTimeline(iKey);
                  SelectKeyOnTimeline(iKey);
                  break;
               
               case EventType.ContextClick:
                  GenericMenu menu = new GenericMenu();
 
                  menu.AddDisabledItem(new GUIContent("Interactions Actions List"));
                  menu.AddItem(new GUIContent("Delete"), false, DestroyKey);
                  menu.ShowAsContext();
                  
                  eventListener.Use();
                  break;
            }
            
         }

         void DestroyKey()
         {
            allInteractions.Remove(iKey);
         }
      }
   }
   protected override void DrawVerticalTickLine()
   {
      Color preColor = Handles.color;
      Color color = Color.white;
      color.a = 0.3f;
      Handles.color = color;
      
      // draw vertical ticks
      float step = 10;
      float preStep = GetTimeArea.drawRect.height / 20f;
      // step = GetTimeArea.drawRect.y;
      
      step = 0f;
      while (step <= GetTimeArea.drawRect.height + GetTimeArea.drawRect.y)
      {
         Vector2 pos = new Vector2(rectContent.x, step + GetTimeArea.drawRect.y);
         Vector2 endPos = new Vector2(position.width, step + GetTimeArea.drawRect.y);
         step += preStep;
         float height = PixelToY(step);
         Rect rect = new Rect(rectContent.x + 5f, step - 10f + GetTimeArea.drawRect.y, 100f, 20f);
         GUI.Label(rect, height.ToString("0"));
         Handles.DrawLine(pos, endPos);
      }
      Handles.color = preColor;
   }
   protected virtual void DrawLeftContent()
   {
      GUILayout.BeginArea(rectLeft);
      GUILayout.BeginHorizontal();
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
   }
   protected virtual void DrawTopToolBar()
   {
      GUILayout.BeginArea(rectTopBar);
      Rect rect = new Rect(rectTopBar.width - 32, rectTopBar.y, 30, 30);
      if (!Application.isPlaying && GUI.Button(rect, ResManager.SettingIcon, EditorStyles.toolbarDropDown))
      {
         OnClickSettingButton();
      }
      GUILayout.EndArea();
   }
   private void DrawLeftTopToolBar()
   {
      GUILayout.BeginArea(rectLeftTopToolBar, string.Empty, EditorStyles.toolbarButton);
      GUILayout.BeginHorizontal();

      if (GUILayout.Button(ResManager.prevKeyContent, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
      {
         PreviousTimeFrame();
      }

      bool playing = IsPlaying;
      playing = GUILayout.Toggle(playing, ResManager.playContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!Application.isPlaying)
      {
         if (IsPlaying != playing)
         {
            IsPlaying = playing;
            if (IsPlaying)
               PlayPreview();
            else
               PausePreView();
         }
      }

      if (GUILayout.Button(ResManager.nextKeyContent, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
      {
         NextTimeFrame();
      }

      if (GUILayout.Button(ResManager.StopIcon, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false))
          && !Application.isPlaying)
      {
         PausePreView();
         RunningTime = 0.0f;
      }

      GUILayout.FlexibleSpace();
      string timeStr = TimeAsString((double)this.RunningTime, "F2");
      GUILayout.Label(timeStr);
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
   }

   #endregion
   
   #region Timeline Options

   private GenericMenu.MenuFunction AddKeyOnTimeline(int rowToAdd, float timeCode)
   {
      return () => allInteractions.Add(new InteractionTimelineKey(rowToAdd, timeCode,TimeAsString(timeCode, "F2")));
   }
   private void SelectKeyOnTimeline(InteractionTimelineKey iKey)
   {
      selectedKey = iKey;
      selectedInteractionKey = iKey.interactionKey;
   }

   public void UnselectKey()
   {
      selectedKey = null;
      selectedInteractionKey = null;
   }
   
   private void PlayPreview()
   {
      IsPlaying = true;
   }
   
   private void PausePreView()
   {
      IsPlaying = false;
   }

   #endregion
   
   public class InteractionTimelineKey
   {
      public InteractionKey interactionKey;
      public int row;

      public InteractionTimelineKey(int row, float time, string timeCode, Enums.InteractionType keyType = Enums.InteractionType.Tap)
      {
         switch (keyType)
         {
            case Enums.InteractionType.Tap:
               interactionKey = CreateInstance<InteractionKey>();
               break;
         }

         interactionKey.timeCode = timeCode;
         interactionKey.time = time;
         this.row = row;
      }
   }
}


