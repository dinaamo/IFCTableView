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
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GeometryGym.Ifc
{
	public partial class IfcLibraryInformation
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			Name = extractString(xml, "Name");
			Version = extractString(xml, "Version");
			//versiondate
			Location = extractString(xml, "Location");
			Description = extractString(xml, "Description");
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Publisher") == 0)
					Publisher = mDatabase.ParseXml<IfcActorSelect>(child as XmlElement);
				else if (string.Compare(name, "LibraryRefForObjects") == 0)
				{
					//todo
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Name", Name);
			setAttribute(xml, "Version", Version);
			if (mPublisher != null)
				xml.AppendChild((mPublisher as BaseClassIfc).GetXML(xml.OwnerDocument, "Publisher", this, processed));
			//VersionDate
			setAttribute(xml, "Location", Location);
			setAttribute(xml, "Description", Description);
		}
	}
	public partial class IfcLibraryReference
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			Description = extractString(xml,"Description");
			Language = extractString(xml,"Language");
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ReferencedLibrary") == 0)
					ReferencedLibrary = mDatabase.ParseXml<IfcLibraryInformation>(child as XmlElement);
				else if (string.Compare(name, "LibraryRefForObjects") == 0)
				{
					//todo
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "Description", Description);
			setAttribute(xml, "Language", Language);
			if (mReferencedLibrary != null)
				xml.AppendChild(ReferencedLibrary.GetXML(xml.OwnerDocument, "ReferencedLibrary", this, processed));	
		}
	}
	public partial class IfcLine
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);

			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Pnt") == 0)
					Pnt = mDatabase.ParseXml<IfcCartesianPoint>(child as XmlElement);
				else if (string.Compare(name, "Dir") == 0)
					Dir = mDatabase.ParseXml<IfcVector>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(Pnt.GetXML(xml.OwnerDocument, "Pnt", this, processed));
			xml.AppendChild(Dir.GetXML(xml.OwnerDocument, "Dir", this, processed));
		}
	}
	public partial class IfcLinearAxisWithInclination
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(Directrix.GetXML(xml.OwnerDocument, "Directrix", this, processed));
			xml.AppendChild(Inclinating.GetXML(xml.OwnerDocument, "Inclinating", this, processed));
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Directrix", true) == 0)
					Directrix = mDatabase.ParseXml<IfcCurve>(child as XmlElement);
				else if (string.Compare(name, "Inclinating", true) == 0)
					Inclinating = mDatabase.ParseXml<IfcAxisLateralInclination>(child as XmlElement);
			}
		}
	}
	public partial class IfcLinearPositioningElement
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Axis") == 0)
					Axis = mDatabase.ParseXml<IfcCurve>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if(Axis != null)
				xml.AppendChild((Axis as BaseClassIfc).GetXML(xml.OwnerDocument, "Axis", this, processed));
		}
	}
	public partial class IfcLinearPlacement
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);

			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "RelativePlacement") == 0)
					RelativePlacement = mDatabase.ParseXml<IfcAxis2PlacementLinear>(child as XmlElement);
				else if (string.Compare(name, "CartesianPosition") == 0)
					CartesianPosition = mDatabase.ParseXml<IfcAxis2Placement3D>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild((mRelativePlacement as BaseClassIfc).GetXML(xml.OwnerDocument, "RelativePlacement", this, processed));
			if(mCartesianPosition != null)
				xml.AppendChild((mCartesianPosition as BaseClassIfc).GetXML(xml.OwnerDocument, "CartesianPosition", this, processed));
		}
	}
	internal partial class IfcLinearSpanPlacement
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Span", mSpan.ToString());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			string span = xml.GetAttribute("Span");
			if (!string.IsNullOrEmpty(span))
				double.TryParse(span, out mSpan);
		}
	}
	public partial class IfcLiquidTerminal
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPredefinedType != IfcLiquidTerminalTypeEnum.NOTDEFINED)
				xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			XmlAttribute predefinedType = xml.Attributes["PredefinedType"];
			if (predefinedType != null)
				Enum.TryParse<IfcLiquidTerminalTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
	public partial class IfcLiquidTerminalType
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			XmlAttribute predefinedType = xml.Attributes["PredefinedType"];
			if (predefinedType != null)
				Enum.TryParse<IfcLiquidTerminalTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
	public partial class IfcLocalPlacement
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);

			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "RelativePlacement") == 0)
					RelativePlacement = mDatabase.ParseXml<IfcAxis2Placement>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild((mRelativePlacement as BaseClassIfc).GetXML(xml.OwnerDocument, "RelativePlacement", this, processed));
		}
	}
	public partial class IfcLShapeProfileDef
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Depth"))
				mDepth = double.Parse(xml.Attributes["Depth"].Value);
			if (xml.HasAttribute("Width"))
				mWidth = double.Parse(xml.Attributes["Width"].Value);
			if (xml.HasAttribute("WebThickness"))
				mThickness = double.Parse(xml.Attributes["Thickness"].Value);
			if (xml.HasAttribute("FilletRadius"))
				mFilletRadius = double.Parse(xml.Attributes["FilletRadius"].Value);
			if (xml.HasAttribute("EdgeRadius"))
				mEdgeRadius = double.Parse(xml.Attributes["EdgeRadius"].Value);
			if (xml.HasAttribute("LegSlope"))
				mLegSlope = double.Parse(xml.Attributes["LegSlope"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Depth", mDepth.ToString());
			xml.SetAttribute("Width", mWidth.ToString());
			xml.SetAttribute("Thickness", mThickness.ToString());
			if (!double.IsNaN(mFilletRadius))
				xml.SetAttribute("FilletRadius", mFilletRadius.ToString());
			if (!double.IsNaN(mEdgeRadius))
				xml.SetAttribute("EdgeRadius", mEdgeRadius.ToString());
			if (!double.IsNaN(mLegSlope))
				xml.SetAttribute("LegSlope", mLegSlope.ToString());
		}
	}
}
