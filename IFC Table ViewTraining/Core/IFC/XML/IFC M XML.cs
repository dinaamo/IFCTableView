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
	public partial class IfcManifoldSolidBrep
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Outer") == 0)
					Outer = mDatabase.ParseXml<IfcClosedShell>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(Outer.GetXML(xml.OwnerDocument, "Outer", this, processed));
		}
	}
	public partial class IfcMapConversion
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Eastings"))
				Eastings = double.Parse(xml.Attributes["Eastings"].Value);
			if (xml.HasAttribute("Northings"))
				Northings = double.Parse(xml.Attributes["Northings"].Value);
			if (xml.HasAttribute("OrthogonalHeight"))
				OrthogonalHeight = double.Parse(xml.Attributes["OrthogonalHeight"].Value);
			if (xml.HasAttribute("XAxisAbscissa"))
				XAxisAbscissa = double.Parse(xml.Attributes["XAxisAbscissa"].Value);
			if (xml.HasAttribute("XAxisOrdinate"))
				XAxisOrdinate = double.Parse(xml.Attributes["XAxisOrdinate"].Value);
			if (xml.HasAttribute("Scale"))
				Scale = double.Parse(xml.Attributes["Scale"].Value);
			if (xml.HasAttribute("ScaleY"))
				ScaleY = double.Parse(xml.Attributes["Scale"].Value);
			if (xml.HasAttribute("ScaleZ"))
				ScaleZ = double.Parse(xml.Attributes["Scale"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Eastings", mEastings.ToString());
			xml.SetAttribute("Northings", mNorthings.ToString());
			xml.SetAttribute("OrthogonalHeight", mOrthogonalHeight.ToString());
			setAttribute(xml, "XAxisAbscissa", mXAxisAbscissa);
			setAttribute(xml, "XAxisOrdinate", mXAxisOrdinate);
			setAttribute(xml, "Scale", mScale);
			setAttribute(xml, "ScaleY", mScaleY);
			setAttribute(xml, "ScaleZ", mScaleZ);
		}
	}
	public partial class IfcMarineFacility
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
				Enum.TryParse<IfcMarineFacilityTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
	public partial class IfcMaterial
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Name"))
				Name = xml.Attributes["Name"].Value;
			if (xml.HasAttribute("Description"))
				Description = xml.Attributes["Description"].Value;
			if (xml.HasAttribute("Category"))
				Category = xml.Attributes["Category"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "HasRepresentation") == 0)
					HasRepresentation = mDatabase.ParseXml<IfcMaterialDefinitionRepresentation>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Name", Name);
			if (mDatabase.Release != ReleaseVersion.IFC2x3)
			{
				setAttribute(xml, "Description", Description);
				setAttribute(xml, "Category", Category);
			}
			if (mHasRepresentation != null)
				xml.AppendChild(mHasRepresentation.GetXML(xml.OwnerDocument, "HasRepresentation", this, processed));
		}
	}
	public partial class IfcMaterialDefinition
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "HasProperties") == 0)
				{
					foreach (XmlNode node in child.ChildNodes)
					{
						IfcMaterialProperties p = mDatabase.ParseXml<IfcMaterialProperties>(node as XmlElement);
						if (p != null)
							HasProperties.Add(p);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if(mHasProperties.Count> 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("HasProperties", mDatabase.mXmlNamespace);
				foreach (IfcMaterialProperties p in HasProperties)
					element.AppendChild(p.GetXML(xml.OwnerDocument, "", this, processed));
			}
		}
	}
	public partial class IfcMaterialDefinitionRepresentation
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "RepresentedMaterial") == 0)
					RepresentedMaterial = mDatabase.ParseXml<IfcMaterial>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			//xml.AppendChild(RepresentedMaterial.GetXML(xml.OwnerDocument, "RepresentedMaterial", this, processed));
		}
	}
	public partial class IfcMaterialLayer
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Material") == 0)
					Material = mDatabase.ParseXml<IfcMaterial>(child as XmlElement);
			}
			if (xml.HasAttribute("LayerThickness"))
				LayerThickness = double.Parse(xml.Attributes["LayerThickness"].Value);
			if (xml.HasAttribute("IsVentilated"))
				Enum.TryParse<IfcLogicalEnum>(xml.Attributes["IsVentilated"].Value, true, out mIsVentilated);
			if (xml.HasAttribute("Name"))
				Name = xml.Attributes["Name"].Value;
			if (xml.HasAttribute("Description"))
				Name = xml.Attributes["Description"].Value;
			if (xml.HasAttribute("Category"))
				Category = xml.Attributes["Category"].Value;
			if (xml.HasAttribute("Priority"))
				int.TryParse(xml.Attributes["Priority"].Value, out mPriority);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			IfcMaterial material = Material;
			if (material != null)
				xml.AppendChild(material.GetXML(xml.OwnerDocument, "Material", this, processed));
			xml.SetAttribute("LayerThickness", mLayerThickness.ToString());
			xml.SetAttribute("IsVentilated", mIsVentilated.ToString().ToLower());
			if (mDatabase.Release != ReleaseVersion.IFC2x3)
			{
				setAttribute(xml, "Name", Name);
				setAttribute(xml, "Description", Description);
				setAttribute(xml, "Category", Category);
				if (!double.IsNaN(mPriority))
					xml.SetAttribute("Priority", mPriority.ToString());
			}
		}
	}
	public partial class IfcMaterialLayerSet
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "MaterialLayers") == 0)
				{
					foreach (XmlNode node in child.ChildNodes)
					{
						IfcMaterialLayer l = mDatabase.ParseXml<IfcMaterialLayer>(node as XmlElement);
						if (l != null)
							MaterialLayers.Add(l);
					}
				}
			}
			if (xml.HasAttribute("LayerSetName"))
				LayerSetName = xml.Attributes["LayerSetName"].Value;
			if (xml.HasAttribute("Description"))
				Description = xml.Attributes["Description"].Value;
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			XmlElement element = xml.OwnerDocument.CreateElement("MaterialLayers", mDatabase.mXmlNamespace);
			foreach (IfcMaterialLayer l in MaterialLayers)
				element.AppendChild(l.GetXML(xml.OwnerDocument, "", this, processed));
			xml.AppendChild(element);
			setAttribute(xml, "LayerSetName", LayerSetName);
			setAttribute(xml, "Description", Description);
		}
	}
	public partial class IfcMaterialLayerSetUsage
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ForLayerSet") == 0)
					ForLayerSet = mDatabase.ParseXml<IfcMaterialLayerSet>(child as XmlElement);
			}
			if (xml.HasAttribute("LayerSetDirection"))
				Enum.TryParse<IfcLayerSetDirectionEnum>(xml.Attributes["LayerSetDirection"].Value, true, out mLayerSetDirection);
			if (xml.HasAttribute("DirectionSense"))
				Enum.TryParse<IfcDirectionSenseEnum>(xml.Attributes["DirectionSense"].Value, true, out mDirectionSense);
			if (xml.HasAttribute("OffsetFromReferenceLine"))
				mOffsetFromReferenceLine = double.Parse(xml.Attributes["OffsetFromReferenceLine"].Value);
			if (xml.HasAttribute("ReferenceExtent"))
				mReferenceExtent = double.Parse(xml.Attributes["ReferenceExtent"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(ForLayerSet.GetXML(xml.OwnerDocument, "ForLayerSet", this, processed));
			xml.SetAttribute("LayerSetDirection", mLayerSetDirection.ToString().ToLower());
			xml.SetAttribute("DirectionSense", mDirectionSense.ToString().ToLower());
			xml.SetAttribute("OffsetFromReferenceLine", mOffsetFromReferenceLine.ToString());
			if (!double.IsNaN(mReferenceExtent) && mDatabase.Release != ReleaseVersion.IFC2x3)
				xml.SetAttribute("ReferenceExtent", mReferenceExtent.ToString());
		}
	}
	public partial class IfcMaterialProfile
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Name"))
				Name = xml.Attributes["Name"].Value;
			if (xml.HasAttribute("Description"))
				Description = xml.Attributes["Description"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Material") == 0)
					Material = mDatabase.ParseXml<IfcMaterial>(child as XmlElement);
				else if (string.Compare(name, "Profile") == 0)
					Profile = mDatabase.ParseXml<IfcProfileDef>(child as XmlElement);
			}
			if (xml.HasAttribute("Priority"))
				int.TryParse(xml.Attributes["Priority"].Value, out mPriority);
			if (xml.HasAttribute("Category"))
				Category = xml.Attributes["Category"].Value;
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "Name", Name);
			setAttribute(xml, "Description", Description);
			if (mMaterial != null)
				xml.AppendChild(Material.GetXML(xml.OwnerDocument, "Material", this, processed));
			if (mProfile != null)
				xml.AppendChild(Profile.GetXML(xml.OwnerDocument, "Profile", this, processed));
			if (mPriority != int.MinValue)
				setAttribute(xml, "Priority", mPriority.ToString());
			setAttribute(xml, "Category", Category);
		}
	}
	public partial class IfcMaterialProfileSet
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "Name", Name);
			setAttribute(xml, "Description", Description);
			XmlElement element = xml.OwnerDocument.CreateElement("MaterialProfiles", mDatabase.mXmlNamespace);
			xml.AppendChild(element);
			foreach (IfcMaterialProfile p in MaterialProfiles)
				element.AppendChild(p.GetXML(xml.OwnerDocument, "", this, processed));
			if (mCompositeProfile != null)
				xml.AppendChild(CompositeProfile.GetXML(xml.OwnerDocument, "CompositeProfile", this, processed));
		}
	}
	public partial class IfcMaterialProfileSetUsage
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(ForProfileSet.GetXML(xml.OwnerDocument, "ForProfileSet", this, processed));
			if (mCardinalPoint != IfcCardinalPointReference.DEFAULT)
				xml.SetAttribute("CardinalPoint", ((int)mCardinalPoint).ToString());
			if(!double.IsNaN(mReferenceExtent))
				setAttribute(xml, "ReferenceExtent", ReferenceExtent);
		}
	}
	public partial class IfcMeasureWithUnit
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ValueComponent") == 0)
					mValueComponent = extractValue(child.FirstChild);
				else if (string.Compare(name, "UnitComponent") == 0)
					UnitComponent = mDatabase.ParseXml<IfcUnit>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(convert(xml.OwnerDocument, mValueComponent, "ValueComponent", mDatabase.mXmlNamespace));
			xml.AppendChild((mUnitComponent as BaseClassIfc).GetXML(xml.OwnerDocument, "UnitComponent", this, processed));
		}
	}
	public partial class IfcMetric
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("BenchMark"))
				Enum.TryParse<IfcBenchmarkEnum>(xml.Attributes["BenchMark"].Value, true, out mBenchMark);
			if (xml.HasAttribute("ValueSource"))
				ValueSource = xml.Attributes["ValueSource"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "DataValue") == 0)
				{
					if(child.HasChildNodes)
						mDataValue = extractValue(child.FirstChild) as IfcMetricValueSelect;
					if (mDataValue == null)
					{
						BaseClassIfc baseClass = mDatabase.ParseXml<BaseClassIfc>(child as XmlElement);
						IfcMetricValueSelect metric = baseClass as IfcMetricValueSelect;
						if (metric != null)
							DataValue = metric;
						else
							mDataValue = extractValue(child as XmlNode);
					}
				}
				else if (string.Compare(name, "ReferencePath") == 0)
					ReferencePath = mDatabase.ParseXml<IfcReference>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("BenchMark", mBenchMark.ToString().ToLower());
			setAttribute(xml, "ValueSource", ValueSource);
			if (mDataValue is BaseClassIfc o)
				xml.AppendChild(o.GetXML(xml.OwnerDocument, "DataValue", this, processed));
			else if(mDataValue is IfcValue val)
				xml.AppendChild(convert(xml.OwnerDocument, val, "DataValue", mDatabase.mXmlNamespace));
			if(mReferencePath != null)
				xml.AppendChild(ReferencePath.GetXML(xml.OwnerDocument, "ReferencePath", this, processed));
		}
	}
	public partial class IfcMobileTelecommunicationsAppliance
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPredefinedType != IfcMobileTelecommunicationsApplianceTypeEnum.NOTDEFINED)
				xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			XmlAttribute predefinedType = xml.Attributes["PredefinedType"];
			if (predefinedType != null)
				Enum.TryParse<IfcMobileTelecommunicationsApplianceTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
	public partial class IfcMobileTelecommunicationsApplianceType
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
				Enum.TryParse<IfcMobileTelecommunicationsApplianceTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
	public partial class IfcMooringDevice
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPredefinedType != IfcMooringDeviceTypeEnum.NOTDEFINED)
				xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			XmlAttribute predefinedType = xml.Attributes["PredefinedType"];
			if (predefinedType != null)
				Enum.TryParse<IfcMooringDeviceTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
	public partial class IfcMooringDeviceType
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
				Enum.TryParse<IfcMooringDeviceTypeEnum>(predefinedType.Value, out mPredefinedType);
		}
	}
}
