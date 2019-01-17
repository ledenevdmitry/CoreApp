﻿using CoreApp.FixpackObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Keys
{
    public class ETLObject : Key
    {
        public Patch patch { get; set; }

        public ETLObject()
        {

        }

        public ETLObject(string objName, string objType, Patch patch) : base(objName, objType)
        {
            this.patch = patch;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode() * 23 + patch.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            ETLObject other = (ETLObject)obj;

            return patch.name.Equals(other.patch.name, StringComparison.CurrentCultureIgnoreCase);
        }

    }
}