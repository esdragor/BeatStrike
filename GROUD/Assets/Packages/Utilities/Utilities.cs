using System;
using System.Diagnostics;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Utilities
{
    public static class Enums
    {
        public enum InteractionType
        {
            Tap,
            Swipe,
        }
        
        public enum EngineState
        {
            Menu,
            Game
        }

        public enum TimeState
        {
            Play,
            Pause
        }
     
        public enum LevelState
        {
            Exploration,
            Combat
        }

        public enum MelodyType
        {
            Exploration,
            Enemy,
            Boss
        }
    }

    public static class Delegates
    {
        public delegate void OnUpdated();
    }
    
    public static class Logs
    {
        public enum LogColor
        {
            None,
            Red,
            Blue,
            Green,
            Yellow,
            Black
        }

        [Conditional("DEBUG")]
        public static void Log(string title, string log, LogType logType = LogType.Log, LogColor titleColor = LogColor.None, LogColor logColor = LogColor.None)
        {
            title = $"<b>[{title}]</b>";

            if (titleColor != LogColor.None)
            {
                title = $"<color={GetColorByLogColor(titleColor)}>{title}</color>";
            }

            if (logColor != LogColor.None)
            {
                log = $"<color={GetColorByLogColor(logColor)}>{log}</color>";
            }
        
            Debug.unityLogger.Log(logType, $"{title} -- {log}");
        }
    
        [Conditional("DEBUG")]
        public static void Log(string title, string log, LogType logType = LogType.Log, LogColor logColor = LogColor.None)
        {
            title = $"<b>[{title}]</b>";
        
            if (logColor != LogColor.None)
            {
                log = $"<color={GetColorByLogColor(logColor)}>{log}</color>";
            }
        
            Debug.unityLogger.Log(logType, $"{title} -- {log}");
        }

        private static string GetColorByLogColor(LogColor logColor)
        {
            return logColor switch
            {
                LogColor.None => "",
                LogColor.Red => "#D45656",
                LogColor.Blue => "#71A7C2",
                LogColor.Yellow =>"#F1C232",
                LogColor.Green => "#8FCE00",
                LogColor.Black => "#000000",
                _ => throw new ArgumentOutOfRangeException(nameof(logColor), logColor, null)
            };
        }
    }

    public static class Helpers
    {
        #region TRANSFORM

        #region Position
        
        public static void SetPosX(this Transform t, float pos)
        {
            Vector3 position = t.position;
            position = new Vector3(pos, position.y, position.z);
            t.position = position;
        }

        public static void SetPosY(this Transform t, float pos)
        {
            Vector3 position = t.position;
            position = new Vector3(position.x, pos, position.z);
            t.position = position;
        }

        public static void SetLocalPosX(this Transform t, float pos)
        {
            Vector3 position = t.localPosition;
            position = new Vector3(pos, position.y, position.z);
            t.localPosition = position;
        }
        
        public static void SetLocalPosY(this Transform t, float pos)
        {
            Vector3 position = t.localPosition;
            position = new Vector3(position.x, pos, position.z);
            t.localPosition = position;
        }
        #endregion

        #region Scale

        public static void SetScale(this Transform t, float globalScale)
        {
            var scale = new Vector3(globalScale, globalScale, globalScale);
            t.localScale = scale;
        }

        #endregion

        #region Rotate

        public static void RotateX(this Transform t, float rot)
        {
            t.Rotate(rot,0,0);
        }

        public static void RotateY(this Transform t, float rot)
        {
            t.Rotate(0,rot,0);
        }

        public static void RotateZ(this Transform t, float rot)
        {
            t.Rotate(0,0,rot);
        }
        
        #endregion
        
        #endregion
        
        #region RECT TRANSFORM

        #region Size Delta

        public static void SetWidth(this RectTransform rt, float w)
        {
            rt.sizeDelta = new Vector2(w, rt.sizeDelta.y);
        }

        public static void SetHeight(this RectTransform rt, float h)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, h);
        }

        public static void SetSize(this RectTransform rt, float size)
        {            
            rt.sizeDelta = new Vector2(size, size);
        }

        public static float GetWidth(this RectTransform rt)
        {
            return rt.sizeDelta.x;
        }

        public static float GetHeight(this RectTransform rt)
        {
            return rt.sizeDelta.y;
        }


        #endregion

    #endregion
    
        #region CONVERTERS
    
    public static int ConvertToInt<T>(this T param)
    {
        return Convert.ToInt32(param);
    }

    #endregion
    
        #region MATHS

    public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }
    
    public static bool IsEvenNumber(this int i)
    {
        return i % 2 == 0;
    }

    public static bool IsMultipleOf(this int i, int count)
    {
        return i % count == 0;
    }

    public static int GetRandomRange(int a, int b)
    {
        return Random.Range(a, b);
    }
    
    #endregion
    }

    public static class RemoteHelpers
    {
        public static float GetRemoteFloat(this float i, string varName)
        {
            return RemoteConfigService.Instance.appConfig.GetFloat(varName);
        }
        
        public static float GetRemoteInt(this int i, string varName)
        {
            return RemoteConfigService.Instance.appConfig.GetInt(varName);
        }

        public static long GetRemoteLong(this long i, string varName)
        {
            return RemoteConfigService.Instance.appConfig.GetLong(varName);
        }
        
        public static bool GetRemoteBool(this bool i, string varName)
        {
            return RemoteConfigService.Instance.appConfig.GetBool(varName);
        }
        
        public static string GetRemoteJson(this long i, string varName)
        {
            return RemoteConfigService.Instance.appConfig.GetJson(varName);
        }

        public static string GetRemoteString(this string i, string varName)
        {
            return RemoteConfigService.Instance.appConfig.GetString(varName);
        }
    }
}

