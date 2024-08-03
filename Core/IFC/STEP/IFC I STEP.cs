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
using System.Collections.Generic;
using System.Linq;

namespace GeometryGym.Ifc
{
    public partial class IfcImageTexture
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        { return base.BuildStringSTEP(release) + ",'" + ParserSTEP.Encode(mUrlReference) + "'"; }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            mUrlReference = ParserSTEP.Decode(ParserSTEP.StripString(str, ref pos, len));
        }
    }

    public partial class IfcImprovedGround
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
                Enum.TryParse<IfcImprovedGroundTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }

    public partial class IfcIndexedColourMap
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return "#" + mMappedTo.StepId + "," + ParserSTEP.DoubleOptionalToString(mOpacity) + ",#" +
                mColours.StepId + ",(" + string.Join(",", mColourIndex) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            MappedTo = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcTessellatedFaceSet;
            mOpacity = ParserSTEP.StripDouble(str, ref pos, len); // Overrides : OPTIONAL IfcStrippedOptional;
            mColours = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcColourRgbList;
            mColourIndex = ParserSTEP.SplitListSTPIntegers(ParserSTEP.StripField(str, ref pos, len));
        }
    }

    public partial class IfcIndexedPolyCurve
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return "#" + mPoints.StepId +
                (mSegments.Count == 0 ? ",$," : ",(" + string.Join(",", mSegments) + "),") +
                (mSelfIntersect == IfcLogicalEnum.UNKNOWN && release != ReleaseVersion.IFC4X3 ? "$" : ParserIfc.LogicalToString(mSelfIntersect));
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            Points = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCartesianPointList;
            string field = ParserSTEP.StripField(str, ref pos, len);
            if (field != "$")
            {
                List<string> strs = ParserSTEP.SplitLineFields(field.Substring(1, field.Length - 2));
                foreach (string s in strs)
                {
                    if (s.ToUpper().StartsWith("IFCLINEINDEX"))
                        mSegments.Add(new IfcLineIndex(ParserSTEP.SplitListSTPIntegers(s.Substring(13, s.Length - 14))));
                    else
                    {
                        List<int> ints = ParserSTEP.SplitListSTPIntegers(s.Substring(12, s.Length - 13));
                        mSegments.Add(new IfcArcIndex(ints[0], ints[1], ints[2]));
                    }
                }
            }
            field = ParserSTEP.StripField(str, ref pos, len);
            if (field[0] == '.')
                mSelfIntersect = field[1] == 'T' ? IfcLogicalEnum.TRUE : IfcLogicalEnum.FALSE;
        }
    }

    public partial class IfcIndexedPolygonalFace
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return "(" + string.Join(",", mCoordIndex) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            mCoordIndex = ParserSTEP.SplitListSTPIntegers(ParserSTEP.StripField(str, ref pos, len));
        }
    }

    public partial class IfcIndexedPolygonalFaceWithVoids
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + ",(" + string.Join(",", mInnerCoordIndices.Select(x => "(" + string.Join(",", x) + ")")) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            List<string> fields = ParserSTEP.SplitLineFields(s.Substring(1, s.Length - 2));
            mInnerCoordIndices = fields.ConvertAll(x => ParserSTEP.SplitListSTPIntegers(x));
        }
    }

    public partial class IfcIndexedPolygonalTextureMap
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + ",(" +
                string.Join(",", mTexCoordIndices.Select(x => "#" + x.StepId)) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            foreach (int id in ParserSTEP.StripListLink(str, ref pos, len))
            {
                if (dictionary.TryGetValue(id, out BaseClassIfc obj) && obj is IfcTextureCoordinateIndices i)
                    mTexCoordIndices.Add(i);
            }
        }
    }

    public abstract partial class IfcIndexedTextureMap
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        { return base.BuildStringSTEP(release) + ",#" + mMappedTo + ",#" + mTexCoords; }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            MappedTo = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcTessellatedFaceSet;
            TexCoords = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcTextureVertexList;
        }
    }

    public partial class IfcIndexedTriangleTextureMap
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            if (mTexCoordList.Count == 0)
                return base.BuildStringSTEP(release) + ",$";
            return base.BuildStringSTEP(release) + ",(" +
                string.Join(",", mTexCoordList.Select(x => "(" + x.Item1 + "," + x.Item2 + "," + x.Item3 + ")")) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            mTexCoordList.AddRange(ParserSTEP.SplitListSTPIntTriple(ParserSTEP.StripField(str, ref pos, len)));
        }
    }

    public partial class IfcIntegerVoxelData
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + ",(" +
                string.Join(",", mValues.Select(x => x.ToString())) + (mUnit == null ? "),$" : "),#" + mUnit.StepId);
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            List<string> arrNodes = ParserSTEP.SplitLineFields(s.Substring(1, s.Length - 2));
            mValues = arrNodes.ConvertAll(x => ParserSTEP.ParseInt(x)).ToArray();
            mUnit = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcUnit;
        }
    }

    public partial class IfcInterceptor
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcInterceptorTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + ".")); }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            if (s.StartsWith("."))
                Enum.TryParse<IfcInterceptorTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }

    public partial class IfcInterceptorType
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            if (s.StartsWith("."))
                Enum.TryParse<IfcInterceptorTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }

    public partial class IfcInventory
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + ".,#" + mJurisdiction.StepId + ",(" +
                string.Join(",", mResponsiblePersons.Select(x => "#" + x.StepId)) +
                (release < ReleaseVersion.IFC4 ? "),#" + mLastUpdateDate : ")," + IfcDate.STEPAttribute(mLastUpdateDate)) +
                (mCurrentValue == null ? ",$" : ",#" + mCurrentValue.StepId) + (mOriginalValue == null ? ",$" : ",#" + mOriginalValue.StepId);
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            Enum.TryParse<IfcInventoryTypeEnum>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mPredefinedType);
            mJurisdiction = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcActorSelect;
            mResponsiblePersons.AddRange(ParserSTEP.StripListLink(str, ref pos, len).Select(x => dictionary[x] as IfcPerson));
            if (release < ReleaseVersion.IFC4)
                mLastUpdateDateSS = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCalendarDate;
            else
                mLastUpdateDate = IfcDate.ParseSTEP(ParserSTEP.StripString(str, ref pos, len));
            mCurrentValue = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCostValue;
            mOriginalValue = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCostValue;
        }
    }

    public partial class IfcImpactProtectionDevice
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + (mPredefinedType == IfcImpactProtectionDeviceTypeEnum.NOTDEFINED ? ",$" : mPredefinedType.ToString());
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            string s = ParserSTEP.StripField(str, ref pos, len);
            if (s.StartsWith("."))
                Enum.TryParse<IfcImpactProtectionDeviceTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }

    public partial class IfcImpactProtectionDeviceType
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
                Enum.TryParse<IfcImpactProtectionDeviceTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
        }
    }

    public partial class IfcIrregularTimeSeries
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) +
            ",(#" + string.Join(",#", mValues.ConvertAll(x => x.StepId.ToString())) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            Values.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcIrregularTimeSeriesValue));
        }
    }

    public partial class IfcIrregularTimeSeriesValue
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return IfcDateTime.STEPAttribute(mTimeStamp) + ",(#" + string.Join(",#", mListValues.ConvertAll(x => x.ToString())) + ")";
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            TimeStamp = IfcDateTime.ParseSTEP(ParserSTEP.StripField(str, ref pos, len));
            string s = ParserSTEP.StripField(str, ref pos, len);
            if (s != "$")
            {
                List<string> ss = ParserSTEP.SplitLineFields(s.Substring(1, s.Length - 2));
                for (int icounter = 0; icounter < ss.Count; icounter++)
                {
                    IfcValue v = ParserIfc.parseValue(ss[icounter]);
                    if (v != null)
                        mListValues.Add(v);
                }
            }
        }
    }

    public partial class IfcIShapeProfileDef
    {
        protected override string BuildStringSTEP(ReleaseVersion release)
        {
            return base.BuildStringSTEP(release) + "," + formatLength(mOverallWidth) + "," + formatLength(mOverallDepth) + "," + formatLength(mWebThickness) + "," + formatLength(mFlangeThickness) + "," + formatLength(mFilletRadius)
                + (release <= ReleaseVersion.IFC2x3 ? "" : "," + formatLength(mFlangeEdgeRadius) + "," + ParserSTEP.DoubleOptionalToString(mFlangeSlope));
        }

        internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
        {
            base.parse(str, ref pos, release, len, dictionary);
            mOverallWidth = ParserSTEP.StripDouble(str, ref pos, len);
            mOverallDepth = ParserSTEP.StripDouble(str, ref pos, len);
            mWebThickness = ParserSTEP.StripDouble(str, ref pos, len);
            mFlangeThickness = ParserSTEP.StripDouble(str, ref pos, len);
            mFilletRadius = ParserSTEP.StripDouble(str, ref pos, len);
            if (release != ReleaseVersion.IFC2x3)
            {
                mFlangeEdgeRadius = ParserSTEP.StripDouble(str, ref pos, len);
                mFlangeSlope = ParserSTEP.StripDouble(str, ref pos, len);
            }
        }
    }
}