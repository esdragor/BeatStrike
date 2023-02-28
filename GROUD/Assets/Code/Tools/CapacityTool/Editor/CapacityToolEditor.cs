using System;
using System.Collections.Generic;
using System.Linq;
using DMTimeArea;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CapacityToolEditor : SimpleTimeArea
{
   private Pattern currentPattern;
   private InteractionKey selectedInteractionKey;
   private string directoryPath = $"Assets/Packages/SimpleTimeLineWindow/EditorResources/StopIcon.png";

   #region VISUAL
   
   private Rect rectTotalArea;
   private Rect rectContent;
   private Rect rectTimeRuler;

   private Rect rectTopBar;
   private Rect rectLeft;
   private Rect rectLeftTopToolBar;
   
   #endregion
   #region PARAMETERS

   private float lastUpdateTime;

   private double runningTime = 10.0f;
   protected override double RunningTime { get => runningTime; set => runningTime = value; }

   private static double cutOffTime = 15.0f;
   protected override double CutOffTime { get => cutOffTime; set => cutOffTime = value; }

   private float LEFTWIDTH = 250f;

   private bool IsPlaying { get; set; }

   protected override bool IsLockedMoveFrame => IsPlaying || Application.isPlaying;

   protected override bool IsLockDragHeaderArrow => IsPlaying;

   public override Rect _rectTimeAreaTotal => rectTotalArea;

   public override Rect _rectTimeAreaContent => rectContent;

   public override Rect _rectTimeAreaRuler => rectTimeRuler;

   protected override float sequencerHeaderWidth => LEFTWIDTH;

   protected override int toolbarHeight { get; } = 300;
   

   #endregion
   #region INITIALISATION

   [MenuItem("Tools/Capacity Tool Editor")]
   public static void InitWindow()
   {
      var window = GetWindow<CapacityToolEditor>(false, "Capacity Tool Editor");
      window.minSize = new Vector2(400f, 200f);
      window.Show();
   }

   private void OnEnable()
   {
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

   public string capacityName;
   public string capacityDescription;
   public int capacityRarityRank;

   private void OnGUI()
   {
      Event current = Event.current;

      GUILayout.Label("Capacity Tool Editor", EditorStyles.boldLabel);
      
      if (rectContent.Contains(current.mousePosition) && current.type == EventType.ContextClick)
      {
         GenericMenu menu = new GenericMenu();
 
         menu.AddDisabledItem(new GUIContent("Timeline Actions List"));
         menu.AddItem(new GUIContent("Add a pattern key."), false, YourCallback);
         menu.ShowAsContext();
 
         current.Use();
      }
      
 
      void YourCallback()
      {
         Debug.Log("Hi there");
      }
      //Draw the top toolbar with selection capacity dropdown

      //Draw the capacity data area
      
      Rect capacityDataArea = new Rect(10, 20, (position.width / 3) - 10, position.height - rectTotalArea.height - 40);
      EditorGUI.DrawRect(capacityDataArea, Color.yellow);
      GUILayout.BeginArea(capacityDataArea, "Capacity Data");
      GUILayout.Space(20);
      capacityName = EditorGUILayout.TextField("Name", capacityName);
      capacityDescription = EditorGUILayout.TextField("Description", capacityDescription);
      capacityRarityRank = EditorGUILayout.IntField("Rarity Rank", capacityRarityRank); 
      GUILayout.EndArea();
      
      //Draw the capacity pattern list area
      Rect capacityPatternListArea = new Rect(5 + capacityDataArea.width + capacityDataArea.x, 20, (position.width / 3) - 10, position.height - rectTotalArea.height - 40);
      EditorGUI.DrawRect(capacityPatternListArea, Color.green);
      GUILayout.BeginArea(capacityPatternListArea, "Pattern List in Capacity");
      GUILayout.EndArea();
      
      //Draw the capacity selected pattern area
      Rect selectPatternDataArea = new Rect(5 + capacityPatternListArea.width + capacityPatternListArea.x, 20, (position.width / 3) - 10, position.height - rectTotalArea.height - 40);
      EditorGUI.DrawRect(selectPatternDataArea, Color.cyan);
      GUILayout.BeginArea(selectPatternDataArea, "Selected Pattern Data");
      GUILayout.EndArea();
      
      DrawTimeline();
   }
   
   public static void DrawDropdown(Rect position, GUIContent label)
   {
      if (!EditorGUI.DropdownButton(position, label, FocusType.Passive))
      {
         return;
      }
 
      void handleItemClicked(object parameter)
      {
         Debug.Log(parameter);
      }
 
      GenericMenu menu = new GenericMenu();
      menu.AddItem(new GUIContent("Item 1"), false, handleItemClicked, "Item 1");
      menu.AddItem(new GUIContent("Item 2"), false, handleItemClicked, "Item 2");
      menu.AddItem(new GUIContent("Item 3"), false, handleItemClicked, "Item 3");
      menu.DropDown(position);
   }

   void SetCapacityInDirectoryDropdown()
   {
      directoryPath = $"Assets/Scriptable/Capacities";
      List<Object> objectsInDirectory = AssetDatabase.LoadAllAssetsAtPath(directoryPath).ToList();
   }
   
   void SaveCapacity()
   {
      directoryPath = $"Assets/Scriptable/Capacities/{currentPattern.capacityName}.asset";
      AssetDatabase.CreateAsset(currentPattern, directoryPath);
      SetCapacityInDirectoryDropdown();
   }

   void LoadCapacity(string path)
   {
      directoryPath = $"Assets/Scriptable/Capacities/{path}.asset";
      currentPattern = (Pattern) EditorGUIUtility.Load(directoryPath);
   }
   
   #region TIMELINE DRAW

   private void DrawTimeline()
   {
      Rect rectMainBodyArea = new Rect(0, position.height - toolbarHeight, position.width, position.height - (position.height - toolbarHeight));
      rectTopBar = new Rect(0, 0, this.position.width, toolbarHeight);
      rectLeft = new Rect(rectMainBodyArea.x, rectMainBodyArea.y + timeRulerHeight, LEFTWIDTH, rectMainBodyArea.height);
      rectLeftTopToolBar = new Rect(rectMainBodyArea.x, rectMainBodyArea.y, LEFTWIDTH, timeRulerHeight);

      rectTotalArea = new Rect(rectMainBodyArea.x + LEFTWIDTH, rectMainBodyArea.y, base.position.width - LEFTWIDTH, rectMainBodyArea.height);
      rectTimeRuler = new Rect(rectMainBodyArea.x + LEFTWIDTH, rectMainBodyArea.y, base.position.width - LEFTWIDTH, timeRulerHeight);
      rectContent = new Rect(rectMainBodyArea.x + LEFTWIDTH, rectMainBodyArea.y + timeRulerHeight, base.position.width - LEFTWIDTH, rectMainBodyArea.height - timeRulerHeight);

      InitTimeArea(false, false, true, true);
      DrawTimeAreaBackGround();
      OnTimeRulerCursorAndCutOffCursorInput();
      DrawTimeRulerArea();
      DrawTopToolBar();
      DrawLeftContent();
      DrawLeftTopToolBar();

      GUILayout.BeginArea(rectContent);
      //Draw each patterns in capacity in the right column and row
      
      GUILayout.EndArea();
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
      
      GUILayout.Label("Pattern Keys");
      GUILayout.Button("Add");
      
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
   
   private void PlayPreview()
   {
      IsPlaying = true;
   }
   
   private void PausePreView()
   {
      IsPlaying = false;
   }

   #endregion
   
   
}
