using System;
using System.Collections.Generic;
using System.Text;

namespace GVMP
{
    public class customization
    {
        public int Gender
        {
            get;
            set;
        }

        public Parents Parents
        {
            get;
            set;
        }

        public List<float> Features
        {
            get;
            set;
        }

        public Hairs Hair
        {
            get;
            set;
        }

        public List<Appearance> Appearance
        {
            get;
            set;
        }

        public int EyebrowColor
        {
            get;
            set;
        }

        public int BeardColor
        {
            get;
            set;
        }

        public int EyeColor
        {
            get;
            set;
        }

        public int BlushColor
        {
            get;
            set;
        }
        
        public int LipstickColor
        {
            get;
            set;
        }

        public int ChestHairColor
        {
            get;
            set;
        }

        public List<uint> Tattoos
        {
            get;
            set;
        } = new List<uint>();
    }
}
