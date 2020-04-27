using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Most of this code was taken from the Commandos Resolution Hacker
// TODO: Rewrite so that a different output directory can be selected

namespace SoldierTactics.GameFormats
{
    public class DIR
    {
        struct DIR_ITEM
        {
            public string itemName;
            public int itemType;
            public int itemSize;
            public int itemOffset;
            public List<DIR_ITEM> children;
        }

        /// <summary>
        /// Exports the WARGAME.DIR from Commandos.
        /// </summary>
        /// <param name="workingDirectory"></param>
        /// <param name="fileName"></param>
        public static void ExportDIR(string workingDirectory, string fileName)
        {
            if (!workingDirectory.EndsWith("\\"))
                workingDirectory += "\\";
            FileStream fs = new FileStream(workingDirectory + fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((int)new FileInfo(workingDirectory + fileName).Length);
            br.Close();
            fs.Close();

            int curPos = 0;
            DIR_ITEM root = iterateDIR(workingDirectory, bytes, ref curPos);
            Trace.WriteLine("Wargame: Export DIR completed");
        }

        private static DIR_ITEM iterateDIR(string path, byte[] dir, ref int curPos)
        {
            DIR_ITEM temp = new DIR_ITEM();
            temp.itemName = Encoding.ASCII.GetString(dir, curPos, 32); curPos += 32;
            temp.itemName = temp.itemName.Substring(0, temp.itemName.IndexOf('\0'));
            temp.itemType = dir[curPos]; curPos += 4;
            temp.itemSize = dir[curPos] | dir[curPos + 1] << 8 | dir[curPos + 2] << 16 | dir[curPos + 3] << 24; curPos += 4;
            temp.itemOffset = dir[curPos] | dir[curPos + 1] << 8 | dir[curPos + 2] << 16 | dir[curPos + 3] << 24; curPos += 4;
            if (temp.itemType != 0xff && temp.itemType != 1)
            {
                if (!File.Exists(path + temp.itemName))
                {
                    FileStream fs = new FileStream(path + temp.itemName, FileMode.Create, FileAccess.Write);
                    BinaryWriter wr = new BinaryWriter(fs);
                    //really slow you are, linq!
                    //wr.Write(dir.Skip(temp.itemOffset).Take(temp.itemSize).ToArray());
                    wr.Write(dir, temp.itemOffset, temp.itemSize);
                    wr.Close();
                    fs.Close();
                    Trace.WriteLine("DIR: Extracted " + path + temp.itemName);
                }
            }
            else if (temp.itemType == 1)
            {
                if (!Directory.Exists(path + temp.itemName))
                {
                    Directory.CreateDirectory(path + temp.itemName);
                    Trace.WriteLine("DIR: Created Folder " + path + temp.itemName);
                }
                temp.children = new List<DIR_ITEM>();
                int pos = curPos;
                curPos = temp.itemOffset;
                path += temp.itemName + "\\";
                DIR_ITEM child = iterateDIR(path, dir, ref curPos);
                while (child.itemType != 0xff)
                {
                    temp.children.Add(child);
                    child = iterateDIR(path, dir, ref curPos);
                }
                curPos = pos;
            }
            return temp;
        }
    }
}
