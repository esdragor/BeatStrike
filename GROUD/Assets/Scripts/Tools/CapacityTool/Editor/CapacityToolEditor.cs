using System;
using System.Collections.Generic;
using System.Globalization;
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
   public string[] levelOptions = new[] {"Level 1", "Level 2", "Level 3"};
   public string[] difficultyOptions = new[] {"1", "2", "3"};
   
   private InteractionKey selectedInteractionKey;

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
      Unintialize();
      EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
      lastUpdateTime = (float)EditorApplication.timeSinceStartup;
   }

   void Unintialize()
   {
      currentPattern = null;
      selectedPatternInDirectory = 0;
      _serializedObject = new SerializedObject(this);
      selectedInteractionKey = null;
   }
   
   private void OnDisable()
   {
      SavePattern();
      Unintialize();
      EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
   }
   
   private void OnDestroy()
   {
      SavePattern();
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
      EditorGUI.DrawRect(previewRect, interfaceData.previewBackgroundColor);
      GUILayout.BeginArea(previewRect);
      GUILayout.Label("Work in progress");
      GUILayout.EndArea();
   }
   
   void DrawPatternListDropdown()
   {
      EditorGUI.DrawRect(patternDropdownRect, interfaceData.patternDropdownBackgroundColor);
      GUILayout.BeginArea(patternDropdownRect);
      selectedPatternInDirectory = EditorGUILayout.Popup("Select a pattern...", selectedPatternInDirectory, patternsInDirectory);

      if (selectedPatternInDirectory != 0)
      {
         LoadPattern(patternsInDirectory[selectedPatternInDirectory]);
      }
      else
      {
         currentPattern = null;
      }

      if (GUILayout.Button("Create New..."))
      {
         CreatePattern();
      }
      
      GUILayout.EndArea();
   }

   private SerializedObject _serializedObject;
   
   void DrawPatternDataArea()
   {
      EditorGUI.DrawRect(patternDataRect, interfaceData.patternDataBackgroundColor); //Draw background
      
      if(currentPattern == null) return;
      GUILayout.BeginArea(patternDataRect);
      GUILayout.Space(20);
      
      currentPattern.patternName = EditorGUILayout.TextField("Pattern Name", currentPattern.patternName);
      currentPattern.targetLevel = EditorGUILayout.Popup("Target Level", currentPattern.targetLevel, levelOptions); 
      currentPattern.difficultyIndex = EditorGUILayout.Popup("Difficulty", currentPattern.difficultyIndex, difficultyOptions);
      currentPattern.maxTime = EditorGUILayout.FloatField("Max Time", (float) currentPattern.maxTime);
      currentPattern.BPM = EditorGUILayout.IntField("BPM", currentPattern.BPM);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      
      if (GUILayout.Button(interfaceData.saveTexture, GUILayout.Width(interfaceData.buttonIconWidth), GUILayout.Height(interfaceData.buttonIconHeight)))
      {
         SavePattern();
      }
      
      if (GUILayout.Button(interfaceData.deleteTexture, GUILayout.Width(interfaceData.buttonIconWidth), GUILayout.Height(interfaceData.buttonIconHeight)))
      {
         DeletePattern();
      }

      GUILayout.EndHorizontal();
      GUILayout.EndArea();

   }

   void DrawInteractionListArea()
   {
      EditorGUI.DrawRect(interactionListRect, interfaceData.interactionListBackgroundDropdown);
      
      GUILayout.BeginArea(interactionListRect);
      GUILayout.Space(20);

      GUILayout.BeginHorizontal();
      
      if (GUILayout.Button("Add Tap On Top Timeline"))
      {
         AddKeyOnTimeline(0, 0, Enums.InteractionType.Attack);
      }
      if (GUILayout.Button("Add Tap On Bot Timeline"))
      {
         AddKeyOnTimeline(1, 0, Enums.InteractionType.Attack);
      }
      
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();
      
      if (GUILayout.Button("Add Slide On Top Timeline"))
      {
         AddKeyOnTimeline(0, 0, Enums.InteractionType.Dodge);
      }
      if (GUILayout.Button("Add Slide On Bot Timeline"))
      {
         AddKeyOnTimeline(1, 0, Enums.InteractionType.Dodge);
      }
      
      GUILayout.EndHorizontal();

      GUILayout.EndArea();
   }



   void DrawSelectedInteractionArea()
   {
      EditorGUI.DrawRect(selectedInteractionRect, interfaceData.selectedInteractionBackgroundColor);
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
         selectedInteractionKey.frame = (float)Convert.ToDouble(selectedInteractionKey.timeCode);
         EditorGUILayout.LabelField($"Frame : {selectedInteractionKey.frame}");
         selectedInteractionKey.time = EditorGUILayout.Slider((float)selectedInteractionKey.time, 0f, (float)currentPattern.maxTime);
         selectedInteractionKey.interactionType = (Enums.InteractionType) EditorGUILayout.EnumPopup("Type", selectedInteractionKey.interactionType);

         switch ( selectedInteractionKey.interactionType)
         {

            case Enums.InteractionType.Dodge:
             selectedInteractionKey.swipeDirection = (ScreenListener.SwipeDirection)EditorGUILayout.EnumPopup("Swipe Direction", selectedInteractionKey.swipeDirection);
               break;
         }
         
         if (GUILayout.Button("Delete Key"))
         {
            RemoveKeyOnTimeline();
         }
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
      
      if (currentPattern != null && currentPattern.BPM > 0)
      {
         _frameRate = currentPattern.BPM / 60f;
         _frameSnap = false;
         _timeInFrames = true;
      }
      
      InitTimeArea(false, false, true, true);
   
      DrawTimeAreaBackGround();
      DrawSplitAreaContent();
      DrawEndVerticalLine();
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
         (EditorGUIUtility.Load("Assets/Scripts/Tools/CapacityTool/EditorResources/PatternInterfaceData.asset") as PatternToolsInterfaceData);
      _tapIcon = (EditorGUIUtility.Load("Assets/Scripts/Tools/CapacityTool/EditorResources/tapimage.png") as Texture);
   }
   void EditorEventListener()
   {
      eventListener = Event.current;
      
      if (botContent.Contains(eventListener.mousePosition) && eventListener.type == EventType.ContextClick)
      {
         Vector2 mousePositionWhenClick = eventListener.mousePosition;
         GenericMenu menu = new GenericMenu();
      
         menu.AddDisabledItem(new GUIContent("Timeline Actions List")); 
         menu.AddItem(new GUIContent("Add a red tap."), false, MenuAddKeyOnTimeline(1, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Attack));
         menu.AddItem(new GUIContent("Add a slide."), false, MenuAddKeyOnTimeline(1, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Dodge));
         menu.ShowAsContext();
      
         eventListener.Use();
      }

      if (topContent.Contains(eventListener.mousePosition) && eventListener.type == EventType.ContextClick)
      {
         Vector2 mousePositionWhenClick = eventListener.mousePosition;
         GenericMenu menu = new GenericMenu();
      
         menu.AddDisabledItem(new GUIContent("Timeline Actions List")); 
         menu.AddItem(new GUIContent("Add a blue tap."), false, MenuAddKeyOnTimeline(0, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Attack));
         menu.AddItem(new GUIContent("Add a slide."), false, MenuAddKeyOnTimeline(0, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Dodge));
         menu.ShowAsContext();
      
         eventListener.Use();
      }

      if (selectedInteractionKey != null && eventListener.type == EventType.KeyDown && eventListener.keyCode == KeyCode.Delete)
      {
         RemoveKeyOnTimeline();
      }
   }

   #endregion

   #region Directory Utilities

   void SetPatternsInDropDown()
   {
      directoryPath = $"Scriptable/Pattern/";
      Pattern[] patterns = Resources.LoadAll<Pattern>(directoryPath);
      patternsInDirectory = new string[patterns.Length + 1];
      patternsInDirectory[0] = "Select a pattern here...";
      for (int i = 1; i < patterns.Length + 1; i++)
      {
         patternsInDirectory[i] = patterns[i - 1].patternName;
      }
   }

   void CreatePattern()
   {
      currentPattern = ScriptableObject.CreateInstance<Pattern>();
      currentPattern.patternName = "New Pattern";
      directoryPath = $"Assets/Resources/Scriptable/Pattern/{currentPattern.patternName}.asset";
      AssetDatabase.CreateAsset(currentPattern, directoryPath);
   }
   
   void SavePattern()
   {
      if(currentPattern == null) return;
      
      AssetDatabase.RenameAsset($"Assets/Resources/Scriptable/Pattern/{currentPattern.name}.asset",
         currentPattern.patternName);
      currentPattern.patternName = currentPattern.name;

      EditorUtility.SetDirty(currentPattern);
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
   }

   void LoadPattern(string patternName)
   {
      if (currentPattern == null || currentPattern.patternName != patternName)
      {
         directoryPath = $"Assets/Resources/Scriptable/Pattern/{patternName}.asset";
         currentPattern = (Pattern) EditorGUIUtility.Load(directoryPath);
      }
   }

   void DeletePattern()
   {
      directoryPath = $"Assets/Resources/Scriptable/Pattern/{currentPattern.patternName}.asset";
      AssetDatabase.DeleteAsset(directoryPath);

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


   private bool dragKey;
   private void DrawKeysOnTimeline()
   {
      if(currentPattern == null || currentPattern.interactions == null || currentPattern.interactions.Count <= 0) return;

      for (int i = 0; i < currentPattern.interactions.Count; i++)
      {
         InteractionKey iKey = currentPattern.interactions[i];
         double timeToPos = TimeToPixel(iKey.time);
         float positionY = 0;
         switch (iKey.interactionType)
         {
            case Enums.InteractionType.Attack:
               positionY =  contentSeparator.y;
               break;
            case Enums.InteractionType.Dodge:
               positionY = contentSeparator.y;
               break;
            
         }
         Rect verticalLine = new Rect((float)timeToPos - (interfaceData.lineThickness * 0.5f),  rectContent.y, interfaceData.lineThickness, rectContent.height);
         Rect interactionIconRect = new Rect((float)timeToPos - (interfaceData.interactionIconWidth * 0.5f), positionY - 15f, interfaceData.interactionIconWidth, interfaceData.interactionIconHeight);
         Texture interactionTexture = null;
         Color lineColor = Color.black;
         
         switch (iKey.interactionType)
         {
            case Enums.InteractionType.Attack:
               interactionTexture =  interfaceData.redTapTexture;
               lineColor = interfaceData.tapLine;
               break;
            
            case Enums.InteractionType.Dodge:
               interactionTexture = interfaceData.swipeTexture;
               lineColor = interfaceData.swipeLine;
               break;
         }
         
         EditorGUI.DrawRect(verticalLine, lineColor);
         GUI.DrawTexture(interactionIconRect, interactionTexture);

         if (dragKey)
         {
            selectedInteractionKey.time = GetSnappedTimeAtMousePosition(eventListener.mousePosition);
         }

         if (eventListener.type == EventType.MouseUp)
         {
            dragKey = false;
         }
         
         if (interactionIconRect.Contains(eventListener.mousePosition))
         {
            switch (eventListener.type)
            {
               case EventType.MouseDown:
                  if(eventListener.button == 0) SelectKeyOnTimeline(iKey);
                  break;
               
               case EventType.MouseDrag:
                  if(!dragVerticalLine) dragKey = true;
                  break;
            }
         }
      }
   }

   private bool dragVerticalLine;
   void DrawEndVerticalLine()
   {
      if(currentPattern == null) return;
      
      float timeToPos = TimeToPixel(currentPattern.maxTime);
      Rect endHandler = new Rect(timeToPos - (interfaceData.interactHandlerWidth * 0.5f), rectTimeRuler.y, interfaceData.interactHandlerWidth,interfaceData.interactionHandlerHeight);
      Rect endLine = new Rect(timeToPos - (interfaceData.endLineThickness * 0.5f), rectTimeRuler.y, interfaceData.endLineThickness, rectContent.height + rectTimeRuler.height);
      
      EditorGUI.DrawRect(endLine, interfaceData.endLine);
      GUI.DrawTexture(endHandler, interfaceData.endHandlerTexture);

      if (dragVerticalLine)
      {
         currentPattern.maxTime = GetSnappedTimeAtMousePosition(eventListener.mousePosition);
      }

      if (eventListener.type == EventType.MouseUp)
      {
         dragVerticalLine = false;
      }
      
      if (endHandler.Contains(eventListener.mousePosition))
      {
         switch (eventListener.type)
         {
            case EventType.MouseDown:
               if(!dragKey) dragVerticalLine = true;
               break;
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

   private GenericMenu.MenuFunction MenuAddKeyOnTimeline(int rowToAdd, float timeCode, Enums.InteractionType interactionType)
   {
      return () => AddKeyOnTimeline(rowToAdd, timeCode, interactionType);
   }

   private void AddKeyOnTimeline(int rowToAdd, float timeCode, Enums.InteractionType interactionType)
   {
      currentPattern.interactions.Add(new InteractionKey(rowToAdd, timeCode,TimeAsString(timeCode, "F2"), interactionType));
      SavePattern();
   }

   private void RemoveKeyOnTimeline()
   {
      currentPattern.interactions.Remove(selectedInteractionKey);
      selectedInteractionKey = null;
   }
   
   private void SelectKeyOnTimeline(InteractionKey iKey)
   {
      selectedInteractionKey = iKey;
   }

   public void UnselectKey()
   {
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
   

   public static class EditorList
   {
      public static void Show(SerializedProperty list)
      {
         EditorGUILayout.PropertyField(list);
      }
   }
}


