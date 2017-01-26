using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XnaDarts.Gameplay.Modes
{
    [Serializable]
    public class RecordManager
    {
        public const string FileName = "records.sd";
        public List<Record> Records = new List<Record>();

        public static RecordManager Load()
        {
            var rm = new RecordManager();

            if (File.Exists(FileName))
            {
                var bf = new BinaryFormatter();
                var fs = new FileStream(FileName, FileMode.Open);
                rm.Records = (List<Record>) bf.Deserialize(fs);
                fs.Close();
            }

            return rm;
        }

        public void Save()
        {
            var bf = new BinaryFormatter();
            var fs = new FileStream(FileName, FileMode.Create);
            bf.Serialize(fs, Records);
            fs.Close();
        }
    }
}