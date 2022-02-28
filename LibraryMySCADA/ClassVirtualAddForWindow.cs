using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryMySCADA.Virt
{
    public class ClassVirtualAddForWindow : Window
    {
        [Category("Setting")]
        public bool isBlockedRecursiveSave { get; set; } = false;

        //-------------------------------------------------------------------------------------------------------------
        virtual public DataSave GetSaveData()
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        virtual public bool SetSaveData(DataSave data)
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public DataSave ToStroke<T>(object obj, Type type, string NameID)
        {
            DataSave ds = new DataSave();
            ds.NameDataId = NameID;
            ds.typ = type;

            ds.NameDataId = Name;
            ds.typ = GetType();
            ds.data = DataSaveSCADA.SerializableToString<T>(obj);
            return ds;
        }
    }


}
