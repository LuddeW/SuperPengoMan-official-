using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class LevelReader
    {
        String levelFilename;

        public LevelReader(string levelFilename)
        {
            this.levelFilename = levelFilename;
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
            return result;
        }
    }
}
