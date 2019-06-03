using System;
using System.ComponentModel;
using System.Linq;
using ColossalFramework.Plugins;
using UnityEngine;

namespace ParallelRoadTool.Utils
{
    public class DebugUtils
    {
        private static readonly string[] AllowedMethodsNames = { };
        private static string ModPrefix => ModInfo.ModName;

        public static void Message(string message)
        {
            Log(message);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, ModPrefix + message);
        }

        public static void Warning(string message)
        {
            Debug.LogWarning(ModPrefix + message);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, ModPrefix + message);
        }

        public static void Log(string message, [CallerMemberName] string callerName = null)
        {
#if DEBUG
            if (AllowedMethodsNames.Any() && !AllowedMethodsNames.Contains(callerName)) return;
            if (message == m_lastLog)
            {
                m_duplicates++;
            }
            else if (m_duplicates > 0)
            {
                Debug.Log(ModPrefix + " [" + callerName + "] " + m_lastLog + "(x" + (m_duplicates + 1) + ")");
                Debug.Log(ModPrefix + " [" + callerName + "] " + message);
                m_duplicates = 0;
            }
            else
            {
                Debug.Log(ModPrefix + "[" + callerName + "] " + message);
            }

            m_lastLog = message;
#endif
        }

        public static void LogException(Exception e)
        {
            Log("Intercepted exception (not game breaking):");
            Debug.LogException(e);
        }

        public static void DumpObject(object myObject, string objectDescription, [CallerMemberName] string callerName = null)
        {
            string myObjectDetails = objectDescription + "\n";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myObject))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(myObject);
                myObjectDetails += name + ": " + value + "\n";
            }
            Debug.Log(ModPrefix + "[" + callerName + "] " + myObjectDetails);
        }

#if DEBUG
        private static string m_lastLog;
        private static int m_duplicates;
#endif
    }
}