﻿namespace BackupsExtra.Entities
{
    public class JobObject
    {
        public JobObject(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}