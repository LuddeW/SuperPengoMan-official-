using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPengoMan
{
    class LevelReader
    {
        String levelFilename;
        List<Level> levels;

        public LevelReader(string levelFilename)
        {
            this.levelFilename = levelFilename;
            levels = ReadFile();
        }

        public List<Level> ReadFile()
        {
            List<Level> result = new List<Level>();

            StreamReader sr = new StreamReader(levelFilename);
            
            Level level = new Level();

            while (!sr.EndOfStream)
            {
                if (level.ReadLevel(sr))
                {
                    result.Add(level);
                }
                level = new Level();
            }
            sr.Close();
            return result;
        }

        public Level this[int ix] => levels[ix];

        public int Count => levels.Count;

        public void Add(Level level)
        {
            if (level != null)
            {
                levels.Add(level);
            }
        }

        public void WriteFile()
        {
            WriteFile(levelFilename);
        }

        public void WriteFile(String fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);
            foreach (Level level in levels)
            {
                level.WriteLevel(sw);
                sw.WriteLine();
            }
            sw.Close();
        }


        internal void AddLevel(char gameobject, char option, int rows, int cols)
        {
            Level level = new Level();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    level.LevelFactory(gameobject, option, row, col);
                }
            }
            levels.Add(level);
        }

        public void DeleteLevel(Level level)
        {
            levels.Remove(level);
        }
    }
}
