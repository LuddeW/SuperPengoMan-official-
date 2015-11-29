using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    
    class Level
    {
        List<List<LevelItem>> levelItemRows = null; 
        //Level()
        //{
            
        //}

        public bool ReadLevel(StreamReader sr)
        {
            int row = 0;
            while (!sr.EndOfStream)
            {
                string objectStr = sr.ReadLine();
                if (objectStr.Length == 0)
                {
                    break;
                }
                int ix = 0;
                for (int col = 0; col < objectStr.Length; col++)
                {
                    // Handle old file format 
                    char option = '%';
                    if (objectStr.Length > (col + 1) && objectStr[col + 1] >= '0' && objectStr[col + 1] <= '9')
                    {
                        option = objectStr[col + 1];
                    }
                    LevelFactory(objectStr[col], option, row, ix++);
                    if (option != '%')
                    {
                        col++;
                    }
                }
                row++;
            }
            return levelItemRows.Count > 0;

        }

        private void LevelFactory(char gameObject, char option, int row, int col)
        {
            if (levelItemRows == null)
            {
                levelItemRows = new List<List<LevelItem>>();
            }
            while (levelItemRows.Count < (row + 1))
            {
                levelItemRows.Add( new List<LevelItem>());
            }
            while (levelItemRows[row].Count < (col + 1))
            {
                levelItemRows[row].Add(new LevelItem());
            }
            levelItemRows[row][col].GameObject = gameObject;
            levelItemRows[row][col].Option = option == '%' ? '0' : option;
        }

        public LevelItem Get(int row, int col)
        {
            return levelItemRows[row][col];
        }

        public int Rows
        {
            get { return levelItemRows.Count; }
        }

        public int Cols
        {
            get { return levelItemRows[0].Count; }
        }

        public void WriteLevel(StreamWriter sw)
        {
            foreach (var levelRow in levelItemRows)
            {
                char[] buf = new char[levelRow.Count*2];
                int ix = 0;
                foreach (var levelItem in levelRow)
                {
                    buf[ix++] = levelItem.GameObject;
                    buf[ix++] = levelItem.Option;
                }
                sw.WriteLine(buf);
            }
        }
    }
}
