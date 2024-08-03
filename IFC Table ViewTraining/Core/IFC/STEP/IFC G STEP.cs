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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using GeometryGym.STEP;

namespace GeometryGym.Ifc
{
	public partial class IfcGasTerminalType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGasTerminalTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGeneralMaterialProperties
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + "," + ParserSTEP.DoubleOptionalToString(mMolecularWeight) + "," + ParserSTEP.DoubleOptionalToString(mPorosity) + "," + ParserSTEP.DoubleOptionalToString(mMassDensity); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mMolecularWeight = ParserSTEP.StripDouble(str, ref pos, len);
			mPorosity = ParserSTEP.StripDouble(str, ref pos, len);
			mMassDensity = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcGeneralProfileProperties
	{ 
		protected override string BuildStringSTEP(ReleaseVersion release) 
		{
			if (release < ReleaseVersion.IFC4)
			{
				string str = base.BuildStringSTEP(release);
				if (!string.IsNullOrEmpty(str))
					return str + "," + ParserSTEP.DoubleOptionalToString(mPhysicalWeight) + "," + ParserSTEP.DoubleOptionalToString(mPerimeter) + "," + ParserSTEP.DoubleOptionalToString(mMinimumPlateThickness) + "," + ParserSTEP.DoubleOptionalToString(mMaximumPlateThickness) + "," + ParserSTEP.DoubleOptionalToString(mCrossSectionArea);
			}
			return "";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mPhysicalWeight = ParserSTEP.StripDouble(str, ref pos, len);
			mPerimeter = ParserSTEP.StripDouble(str, ref pos, len);
			mMinimumPlateThickness = ParserSTEP.StripDouble(str, ref pos, len);
			mMaximumPlateThickness = ParserSTEP.StripDouble(str, ref pos, len);
			mCrossSectionArea = ParserSTEP.StripDouble(str, ref pos, len);
		}
	}
	public partial class IfcGeographicCRS
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			if (release < ReleaseVersion.IFC4X3_ADD2)
				return "";
			return base.BuildStringSTEP(release) + 
				(string.IsNullOrEmpty(mPrimeMeridian) ? ",$," : ",'" + ParserSTEP.Encode(mPrimeMeridian) + "',") +
				(mAngleUnit == null ? "$" : "#" + mAngleUnit.StepId) + (mHeightUnit == null ? ",$" : ",#" + mHeightUnit.StepId);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mPrimeMeridian = ParserSTEP.Decode(ParserSTEP.StripString(str, ref pos, len));
			mAngleUnit = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcNamedUnit;
			if(release >= ReleaseVersion.IFC4X4_DRAFT)
				mHeightUnit = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcNamedUnit;
		}
	}
	public partial class IfcGeographicElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) 
		{ 
			return (release < ReleaseVersion.IFC4 ? "" : base.BuildStringSTEP(release) + (mPredefinedType == IfcGeographicElementTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType + ".")); 
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGeographicElementTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGeographicElementType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return (release < ReleaseVersion.IFC4 ? "" : base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGeographicElementTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGeometricRepresentationContext
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			if (this as IfcGeometricRepresentationSubContext != null)
				return base.BuildStringSTEP(release) + ",*,*,*,*";

			return base.BuildStringSTEP(release) + "," + (mCoordinateSpaceDimension == 0 ? "*" : mCoordinateSpaceDimension.ToString()) + "," + (mPrecision == 0 ? "*" : ParserSTEP.DoubleOptionalToString(mPrecision)) + "," + ParserSTEP.ObjToLinkString(mWorldCoordinateSystem) + "," + ParserSTEP.ObjToLinkString(mTrueNorth);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			mCoordinateSpaceDimension = ParserSTEP.StripInt(str, ref pos, len);
			mPrecision = ParserSTEP.StripDouble(str, ref pos, len);
			WorldCoordinateSystem = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcAxis2Placement;
			TrueNorth = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcDirection;
		}
	}
	public partial class IfcGeometricRepresentationSubContext
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + ",#" + mParentContext.StepId + (double.IsNaN(mTargetScale) || mTargetScale <=0 ? ",$,." : "," + ParserSTEP.DoubleOptionalToString(mTargetScale) + ",.") + 
				mTargetView.ToString() + (string.IsNullOrEmpty(mUserDefinedTargetView) ?  ".,$" : ".,'" + ParserSTEP.Encode(mUserDefinedTargetView) + "'"); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			ParentContext = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcGeometricRepresentationContext;
			mTargetScale = ParserSTEP.StripDouble(str, ref pos, len);
			Enum.TryParse<IfcGeometricProjectionEnum>(ParserSTEP.StripField(str, ref pos, len).Replace(".", ""), true, out mTargetView);
			mUserDefinedTargetView = ParserSTEP.Decode(ParserSTEP.StripString(str, ref pos, len));
		}
	}
	public partial class IfcGeometricSet
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return (mElements.Count == 0 ? "" : "(" + string.Join(",", mElements.ConvertAll(x => "#" + x.StepId)) + ")");
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			List<int> stepIds = ParserSTEP.StripListLink(str, ref pos, len);
			Elements.AddRange(stepIds.ConvertAll(x => dictionary[x] as IfcGeometricSetSelect));
		}
	}
	public partial class IfcGeoScienceFeature
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) +  ",." + mPredefinedType.ToString() + ".";
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGeoScienceFeatureTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGeoScienceModel
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
				Enum.TryParse<IfcGeoScienceModelTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGeoScienceObservation
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (mPredefinedType == IfcGeoScienceObservationTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGeoScienceObservationTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGeotechnicalStratum
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (release < ReleaseVersion.IFC4X3 ? "" : (mPredefinedType == IfcGeotechnicalStratumTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType + ".")); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			if (release >= ReleaseVersion.IFC4X3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s.StartsWith("."))
					Enum.TryParse<IfcGeotechnicalStratumTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcGeotechTypicalSection
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) + (mPredefinedType == IfcGeotechTypicalSectionTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGeotechTypicalSectionTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGradientCurve
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return (release < ReleaseVersion.IFC4X3_RC3 ? "" : base.BuildStringSTEP(release)) + ",#" + mBaseCurve.StepId + 
				(release == ReleaseVersion.IFC4X3_RC2 ?  ",(#" + string.Join(",#", Segments.ConvertAll(x => x.StepId.ToString())) + ")" : "") +
				(mEndPoint == null ? ",$" : ",#" + mEndPoint.StepId);
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			if(release != ReleaseVersion.IFC4X3_RC2)
				base.parse(str, ref pos, release, len, dictionary);
			BaseCurve = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcBoundedCurve;
			if(release == ReleaseVersion.IFC4X3_RC2)
				Segments.AddRange(ParserSTEP.StripListLink(str, ref pos, len).ConvertAll(x => dictionary[x] as IfcCurveSegment));
			EndPoint = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcPlacement;
		}
	}
	public partial class IfcGrid
	{
		protected override string BuildStringSTEP(ReleaseVersion release)
		{
			return base.BuildStringSTEP(release) + ",(" + string.Join(",", mUAxes.Select(x => "#" + x.StepId)) + "),(" + 
				string.Join(",", mVAxes.Select(x => "#" + x.StepId)) + (mWAxes.Count == 0 ? "),$" : "),(" + 
				string.Join(",", mWAxes.Select(x=> "#" + x.StepId)) + ")") +  (release < ReleaseVersion.IFC4 ? "" : (mPredefinedType == IfcGridTypeEnum.NOTDEFINED ? ",$" : ",." + mPredefinedType.ToString() + "."));
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			UAxes.AddRange(ParserSTEP.StripListLink(str, ref pos, len).Select(x=>dictionary[x] as IfcGridAxis));
			VAxes.AddRange(ParserSTEP.StripListLink(str, ref pos, len).Select(x=>dictionary[x] as IfcGridAxis));
			WAxes.AddRange(ParserSTEP.StripListLink(str, ref pos, len).Select(x=>dictionary[x] as IfcGridAxis));
			if (release != ReleaseVersion.IFC2x3)
			{
				string s = ParserSTEP.StripField(str, ref pos, len);
				if (s[0] == '.')
					Enum.TryParse<IfcGridTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
			}
		}
	}
	public partial class IfcGridAxis
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return (string.IsNullOrEmpty(mAxisTag) ? "$,#" : "'" + ParserSTEP.Encode(mAxisTag) + "',#") + AxisCurve.StepId + "," + ParserSTEP.BoolToString(mSameSense); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mAxisTag = ParserSTEP.Decode(ParserSTEP.StripString(str, ref pos, len));
			AxisCurve = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcCurve;
			mSameSense = ParserSTEP.StripBool(str, ref pos, len);
		}
	}
	public partial class IfcGridPlacement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) 
		{
			if (mPlacesObject.Count == 0)
				return "";
			return (release > ReleaseVersion.IFC4X1 ? base.BuildStringSTEP(release) + ",#" : "#)" + mPlacementLocation.StepId + "," + ParserSTEP.ObjToLinkString(mPlacementRefDirection)); 
		}
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int,BaseClassIfc> dictionary)
		{
			mPlacementLocation = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcVirtualGridIntersection;
			mPlacementRefDirection = dictionary[ParserSTEP.StripLink(str, ref pos, len)] as IfcGridPlacementDirectionSelect;
		}
	}
	public partial class IfcGroundReinforcementElement
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return base.BuildStringSTEP(release) +  ",." + mPredefinedType.ToString() + "."; }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGroundReinforcementElementTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
	public partial class IfcGroundReinforcementElementType
	{
		protected override string BuildStringSTEP(ReleaseVersion release) { return (release < ReleaseVersion.IFC4X4_DRAFT ? "" : base.BuildStringSTEP(release) + ",." + mPredefinedType.ToString() + "."); }
		internal override void parse(string str, ref int pos, ReleaseVersion release, int len, ConcurrentDictionary<int, BaseClassIfc> dictionary)
		{
			base.parse(str, ref pos, release, len, dictionary);
			string s = ParserSTEP.StripField(str, ref pos, len);
			if (s.StartsWith("."))
				Enum.TryParse<IfcGroundReinforcementElementTypeEnum>(s.Replace(".", ""), true, out mPredefinedType);
		}
	}
}
