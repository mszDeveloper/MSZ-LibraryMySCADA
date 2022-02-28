using LibraryMySCADA.Virt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Xml.Serialization;

namespace LibraryMySCADA
{
    [Serializable]
    public struct SaveData
    {
        public int mode;
        public int min;
        public int max;
        public int maxAmperage;

    }

    [Serializable]
    public class DataSave
    {
        public DataSave(string Named = null) { NameDataId = Named; }
        public string NameDataId;
        public string data;
        public Type typ;
    }

    enum Command
    {
        Save,
        Load
    }
    //----------------------------------------------------------------------------------------------------------------------
    public static class DataSaveSCADA
    {
        private static List<ClassVirtualAdd> vObjs;

        //----------------------------------------------------------------------------------------------------------------------
        public static string SerializableToString<T>(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        //----------------------------------------------------------------------------------------------------------------------
        public static T SerializableStringToObject<T>(string str)
        {   StringReader sr;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            try
            {
                sr = new StringReader(str);
                return (T)serializer.Deserialize(sr); 
            }
            catch
            {
                return default(T);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public static bool LoadData<T>(string name, ref T obj)
        {
            List<object> restored = null;
            try
            {
                using (Stream stream = File.Open("Data//" + name, FileMode.Open))
                {
                    using (GZipStream str = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        BinaryFormatter bformatter = new BinaryFormatter();
                        restored = (List<object>)bformatter.Deserialize(str);
                        str.Close();
                    }
                    stream.Close();
                }
                obj = (T)restored[0];
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("No load file!n " + e.Message);
                return false;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public static void SaveData<T>(string name, object obj)
        {
            try
            {
                Directory.CreateDirectory("Data");

                using (Stream stream = File.Open("Data//" + name, FileMode.Create))
                {
                    using (GZipStream str = new GZipStream(stream, CompressionMode.Compress))
                    {
                        List<object> values = new List<object>();
                        values.Add(obj);
                        BinaryFormatter bformatter = new BinaryFormatter();
                        bformatter.Serialize(str, values);
                        str.Close();
                    }
                    stream.Close();
                }
            }
            catch (Exception e) { MessageBox.Show("No save file!\n " + e.Message); }
        }

        //---------------------------------------------------------------------------------------------------------
        public static void FindAndLoadUserControl(FrameworkElement e, string fileName = "scada.data")
        {
            vObjs = new List<ClassVirtualAdd>();
            recFindControls(e, Command.Load);
            List<DataSave> dataSave = new List<DataSave>();
            DataSaveSCADA.LoadData<List<DataSave>>(fileName, ref dataSave);
            bool rez = false;
            foreach (var item in vObjs)
            {
                for (int i = dataSave.Count-1; i >= 0; i--)
                {
                    if (rez = item.SetSaveData(dataSave[i])) break;

                }
                if (rez) {  rez = false; continue; }
            }
        }

        //---------------------------------------------------------------------------------------------------------
         public static void FindAndSaveUserControl(FrameworkElement e, string fileName = "scada.data")
        {
            vObjs = new List<ClassVirtualAdd>();
            vObjs.Clear();
            vObjs = new List<ClassVirtualAdd>();
            DataSaveSCADA.SaveData<List<DataSave>>(fileName, recFindControls(e, Command.Save));
        }

        //---------------------------------------------------------------------------------------------------------
        private static List<DataSave> recFindControls(FrameworkElement e, Command cmd, bool isOnlyVirtAddClassesRecursiv = false)
        {
            List<DataSave> ds = new List<DataSave>();
            
            foreach (var item in LogicalTreeHelper.GetChildren(e))
            {
                if ((item as ClassVirtualAdd) != null) vObjs.Add((ClassVirtualAdd)item);
                if ((item as FrameworkElement) != null)
                    if ((item as ClassVirtualAdd) != null)
                        { if (!(item as ClassVirtualAdd).isBlockedRecursiveSave || cmd== Command.Load) recursiveFind((FrameworkElement)item, cmd); }
                    else recursiveFind((FrameworkElement)item, cmd);
            }

            if (cmd == Command.Save)
            {
                foreach (ClassVirtualAdd item in vObjs) ds.Add(item.GetSaveData());
            }
            return ds;
        }

        //-------------------------------------------------------------------------------------------------------------
        private static void recursiveFind(FrameworkElement obg, Command cmd)
        {
            IEnumerable o;
            if ((o = LogicalTreeHelper.GetChildren(obg)) != null)
            {
                foreach (var item in o)
                {
                    if ((item as ClassVirtualAdd) != null) vObjs.Add((ClassVirtualAdd)item);
                    if ((item as FrameworkElement) != null)
                        if ((item as ClassVirtualAdd) != null)
                        { if (!(item as ClassVirtualAdd).isBlockedRecursiveSave || cmd == Command.Load) recursiveFind((FrameworkElement)item, cmd); }
                    else recursiveFind((FrameworkElement)item, cmd);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------
    }
}

 