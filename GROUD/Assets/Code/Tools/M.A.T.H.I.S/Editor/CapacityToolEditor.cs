using System;
using DMTimeArea;
using UnityEditor;
using UnityEngine;
using Utilities;

public class CapacityToolEditor : SimpleTimeArea
{
   private Event eventListener;
   
   public SymphonySO selectedSymphony;
   public PatternSO selectedPattern;
   private InteractionKey selectedInteractionKey;

   public string[] levelOptions = new[] {"Level 1", "Level 2", "Level 3"};
   public string[] difficultyOptions = new[] {"1", "2", "3"};

   private int selectedIndex = 0;
   private string[] symphonies;
   
   private string directoryPath;

   private PatternToolsInterfaceData interfaceData;

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
   private Rect SymphonyDropdownRect => interfaceData.symphonyDropdownRect;
   private Rect SelectedInteractionRect => interfaceData.selectedInteractionRect;
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
      //window.minSize = new Vector2(1170f, 760f);
      //window.maxSize = new Vector2(1170f, 760f);
      window.Show();
   }

   private void OnEnable()
   {
      Unintialize();
      isDrawingSymphonyPopup = false;
      EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
      lastUpdateTime = (float)EditorApplication.timeSinceStartup;
   }

   private SerializedObject _serializedObject;
   void Unintialize()
   {
      selectedPattern = null;
      selectedIndex = 0;
      _serializedObject = new SerializedObject(this);
      selectedInteractionKey = null;
   }
   
   private void OnDisable()
   {
      Unintialize();
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

   private bool isDrawingSymphonyPopup;
   private void OnGUI()
   {
      GetSymphonyInDirectory();
      GetReferencesInDirectories();
      
      EditorEventListener();
      DrawSymphonyDropdown();
      
      if (selectedSymphony != null)
      {
         DrawMelodyData(selectedSymphony.exploration, interfaceData.explorationMelodyContent);
         DrawMelodyData(selectedSymphony.enemy, interfaceData.enemyMelodyContent);
         DrawMelodyData(selectedSymphony.boss, interfaceData.bossMelodyContent);
      }
      
      DrawSelectedInteractionData();

      DrawTimeline();
      
      if (isDrawingSymphonyPopup)
      {
         DisplayCreateSymphonyPopup();
      }
   }
   
   #region Editor Drawing
   
   void DrawSymphonyDropdown()
   {
      EditorGUI.DrawRect(SymphonyDropdownRect, interfaceData.contentBackgroundColor);
      GUILayout.BeginArea(SymphonyDropdownRect);
      
      selectedIndex = EditorGUILayout.Popup(selectedIndex, symphonies);

      if (selectedIndex != 0)
      {
         LoadSymphonyFromDirectory(symphonies[selectedIndex]);
      }
      else
      {
         selectedPattern = null;
      }

      if (GUILayout.Button("Create new symphony..."))
      {
         if (!isDrawingSymphonyPopup)
         {
            isDrawingSymphonyPopup = true;
         }
      }
      
      GUILayout.EndArea();
   }

   void SetSymphony(SymphonySO symphony)
   {
      selectedSymphony = symphony;
   }

   private string tempSymphName;
   private int tempSymphBPM;
   void DisplayCreateSymphonyPopup()
   {
      EditorGUI.DrawRect(interfaceData.symphonyCreatorPopupRect, interfaceData.contentBackgroundColor);
      GUILayout.BeginArea(interfaceData.symphonyCreatorPopupRect);
      GUILayout.Label("Create Symphony");
      tempSymphName = EditorGUILayout.TextField("Symphony Name",tempSymphName);
      tempSymphBPM = EditorGUILayout.IntField("BPM", tempSymphBPM);
      
      if (GUILayout.Button("Create"))
      {
         CreateSymphony();
      }
      
      if (GUILayout.Button("Close"))
      {
         isDrawingSymphonyPopup = false;
      }
      
      GUILayout.EndArea();
   }

   void CreateSymphony()
   {
      if (tempSymphName == "" || tempSymphBPM == 0)
      {
         
      }
      else
      {
         SymphonySO newSymphony = CreateInstance<SymphonySO>();
         newSymphony.sName = tempSymphName;
         newSymphony.bpm = tempSymphBPM;
         newSymphony.exploration = CreateInstance<MelodySO>();
         newSymphony.enemy = CreateInstance<MelodySO>();
         newSymphony.boss = CreateInstance<MelodySO>();
         AssetDatabase.CreateAsset(newSymphony, $"Assets/Resources/Scriptable/Symphony/{newSymphony.sName}.asset");
         isDrawingSymphonyPopup = false;
      }

   }
   
   void DrawMelodyData(MelodySO melody, Rect rect)
   {
      EditorGUI.DrawRect(rect, interfaceData.contentBackgroundColor);
      GUILayout.BeginArea(rect);
      
      if (melody != null)
      {
         Debug.Log("Kebab");
         if (melody.patterns.Count > 0)
         {
            for (int i = 0; i < melody.patterns.Count; i++)
            {
               if (GUILayout.Button($"Pattern #{i}"))
               {
                  selectedPattern = melody.patterns[i];
               }
            }
         }

         GUILayout.BeginHorizontal();
      
         if (GUILayout.Button("Create Pattern"))
         {
            CreatePattern(melody);  
         }

         GUILayout.EndHorizontal();

         melody.seed = EditorGUILayout.IntField("Seed",melody.seed);

         if (GUILayout.Button("Generate Seed"))
         {
            melody.GenerateSeed();
         }
      
         GUILayout.Label(melody.GetSeedPatternsPreview());
      }
      
    
      

      GUILayout.EndArea();
   }

   void DrawSelectedInteractionData()
   {
      EditorGUI.DrawRect(SelectedInteractionRect, interfaceData.contentBackgroundColor);
      GUILayout.BeginArea(SelectedInteractionRect);
      
      GUILayout.Space(20f);
      
      if (selectedInteractionKey != null)
      {
         selectedInteractionKey.timeCode = TimeAsString(selectedInteractionKey.time);
         EditorGUILayout.LabelField($"Time Code : {selectedInteractionKey.timeCode}");
         selectedInteractionKey.time = EditorGUILayout.Slider((float)selectedInteractionKey.time, 0f, (float)selectedPattern.maxTime);
         selectedInteractionKey.interactionType = (Enums.InteractionType) EditorGUILayout.EnumPopup("Type", selectedInteractionKey.interactionType);

         switch ( selectedInteractionKey.interactionType)
         {
            case Enums.InteractionType.Tap:
               selectedInteractionKey.interactionColor = (InteractionKey.InteractionColor)EditorGUILayout.EnumPopup("Color", selectedInteractionKey.interactionColor);
               break;
            
            case Enums.InteractionType.Swipe:
               selectedInteractionKey.swipeDirection = (ScreenListener.SwipeDirection)EditorGUILayout.EnumPopup("Swipe Direction", selectedInteractionKey.swipeDirection);
               break;
         }
         
         if (GUILayout.Button("Delete Key"))
         {
            RemoveKeyOnTimeline();
         }
      }      
      GUILayout.EndArea();
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
      DrawEndVerticalLine();
      DrawKeysOnTimeline();
      OnTimeRulerCursorAndCutOffCursorInput();
      DrawTimeRulerArea();
      DrawLeftContent();
      DrawLeftTopToolBar();
   }
   
   #endregion
   
   #region Editor Utilities
   void GetReferencesInDirectories()
   {
      interfaceData = (EditorGUIUtility.Load("Assets/Code/Tools/M.A.T.H.I.S/EditorResources/PatternInterfaceData.asset") as PatternToolsInterfaceData);
   }

   void EditorEventListener()
   {
      eventListener = Event.current;
      
      if (botContent.Contains(eventListener.mousePosition) && eventListener.type == EventType.ContextClick)
      {
         Vector2 mousePositionWhenClick = eventListener.mousePosition;
         GenericMenu menu = new GenericMenu();
      
         menu.AddDisabledItem(new GUIContent("Timeline Actions List")); 
         menu.AddItem(new GUIContent("Add a red tap."), false, MenuAddKeyOnTimeline(1, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Tap));
         menu.AddItem(new GUIContent("Add a slide."), false, MenuAddKeyOnTimeline(1, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Swipe));
         menu.ShowAsContext();
      
         eventListener.Use();
      }

      if (topContent.Contains(eventListener.mousePosition) && eventListener.type == EventType.ContextClick)
      {
         Vector2 mousePositionWhenClick = eventListener.mousePosition;
         GenericMenu menu = new GenericMenu();
      
         menu.AddDisabledItem(new GUIContent("Timeline Actions List")); 
         menu.AddItem(new GUIContent("Add a blue tap."), false, MenuAddKeyOnTimeline(0, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Tap));
         menu.AddItem(new GUIContent("Add a slide."), false, MenuAddKeyOnTimeline(0, (float)GetSnappedTimeAtMousePosition(mousePositionWhenClick), Enums.InteractionType.Swipe));
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

   void GetSymphonyInDirectory()
   {
      directoryPath = $"Scriptable/Symphony/";
      SymphonySO[] symphony = Resources.LoadAll<SymphonySO>(directoryPath);
      symphonies = new string[symphony.Length + 1];
      symphonies[0] = "Select a symphony here...";
      for (int i = 1; i < symphony.Length + 1; i++)
      {
         symphonies[i] = symphony[i - 1].sName;
      }
   }

   void LoadSymphonyFromDirectory(string sName)
   {
      selectedSymphony = (SymphonySO) Resources.Load($"Scriptable/Symphony/{sName}", typeof(SymphonySO));
      Debug.Log(selectedSymphony);
   }

   void CreatePattern(MelodySO targetMelody)
   {
      selectedPattern = CreateInstance<PatternSO>();
      targetMelody.patterns.Add(selectedPattern);
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
      if(selectedPattern == null || selectedPattern.interactions == null || selectedPattern.interactions.Count <= 0) return;

      for (int i = 0; i < selectedPattern.interactions.Count; i++)
      {
         InteractionKey iKey = selectedPattern.interactions[i];
         double timeToPos = TimeToPixel(iKey.time);
         float positionY = 0;
         switch (iKey.interactionType)
         {
            case Enums.InteractionType.Tap:
               positionY = iKey.row == 1 ? botContent.y + (botContent.height * 0.5f) : topContent.y + (topContent.height * 0.5f);
               break;
            case Enums.InteractionType.Swipe:
               positionY = contentSeparator.y;
               break;
            
         }
         Rect interactionIconRect = new Rect((float)timeToPos - (interfaceData.interactionIconWidth * 0.5f), positionY - 15f, interfaceData.interactionIconWidth, interfaceData.interactionIconHeight);
         Rect verticalLine = new Rect((float)timeToPos - (interfaceData.lineThickness * 0.5f),  rectContent.y, interfaceData.lineThickness, rectContent.height);
         Texture interactionTexture = null;
         Color lineColor = Color.black;
         
         switch (iKey.interactionType)
         {
            case Enums.InteractionType.Tap:
               interactionTexture = iKey.interactionColor == InteractionKey.InteractionColor.Blue
                  ? interfaceData.blueTapTexture
                  : interfaceData.redTapTexture;
               lineColor = interfaceData.tapLine;
               break;
            
            case Enums.InteractionType.Swipe:
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
      if(selectedPattern == null) return;
      
      float timeToPos = TimeToPixel(selectedPattern.maxTime);
      Rect endHandler = new Rect(timeToPos - (interfaceData.interactHandlerWidth * 0.5f), rectTimeRuler.y, interfaceData.interactHandlerWidth,interfaceData.interactionHandlerHeight);
      Rect endLine = new Rect(timeToPos - (interfaceData.endLineThickness * 0.5f), rectTimeRuler.y, interfaceData.endLineThickness, rectContent.height + rectTimeRuler.height);
      
      EditorGUI.DrawRect(endLine, interfaceData.endLine);
      GUI.DrawTexture(endHandler, interfaceData.endHandlerTexture);

      if (dragVerticalLine)
      {
         selectedPattern.maxTime = GetSnappedTimeAtMousePosition(eventListener.mousePosition);
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
      selectedPattern.interactions.Add(new InteractionKey(rowToAdd, timeCode,TimeAsString(timeCode, "F2"), interactionType));
   }

   private void RemoveKeyOnTimeline()
   {
      selectedPattern.interactions.Remove(selectedInteractionKey);
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


