using LibraryMySCADA.Virt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMySCADA
{
    class ClassTemplate : ClassVirtualAdd
    {
        //-----------------------------------------------------------------------------------------------------------------
        //[Serializable]
        //public struct SaveData
        //{
        //    public int mode;
        //    public int min;
        //    public int max;
        //    public int maxAmperage;

        //}
        //-----------------------------------------------------------------------------------------------------------------
        public override DataSave GetSaveData()
        {
            SaveData sd = new SaveData();
            //далее инициализация свойств
            //sd.max = 78; 
            return ToStroke<SaveData>(sd, GetType(), Name);
        }

        //-----------------------------------------------------------------------------------------------------------------
        public override bool SetSaveData(DataSave data)
        {
            if (data == null || data.typ != GetType() || data.NameDataId != Name) return false;
            SaveData sd;
            sd = DataSaveSCADA.SerializableStringToObject<SaveData>(data.data);
            return true;

        }

    }
}
