// MIT License
// Copyright (c) 2016 Geometry Gym Pty Ltd

// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using GeometryGym.STEP;
using System;
using System.Collections.Concurrent;

namespace GeometryGym.Ifc
{
    public abstract partial class IfcNamedUnit
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        { return (mDimensions == null ? "*" : "#" + mDimensions.StepId) + ",." + mUnitType.ToString() + "."; }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            mDimensions = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDimensionalExponents;
            Enum.TryParse<IfcUnitEnum>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mUnitType);
        }
    }

    public partial class IfcNavigationElement
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + (mPredefinedType == IfcNavigationElementTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".");
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            if (s.StartsWith("."))
                Enum.TryParse<IfcNavigationElementTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }

    public partial class IfcNavigationElementType
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + ".";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            if (s.StartsWith("."))
                Enum.TryParse<IfcNavigationElementTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }
}