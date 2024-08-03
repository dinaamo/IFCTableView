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
	public partial class IfcParameterizedProfileDef 
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Position", true) == 0)
					Position = mDatabase.ParseXml<IfcAxis2Placement2D>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPosition != null)
				xml.AppendChild(Position.GetXML(xml.OwnerDocument, "Position", this, processed));
		}
	}
	public partial class IfcPavement 
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPredefinedType != IfcPavementTypeEnum.NOTDEFINED)
				xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			string str = xml.GetAttribute("PredefinedType");
			if (!string.IsNullOrEmpty(str))
				Enum.TryParse<IfcPavementTypeEnum>(str, out mPredefinedType);
		}
	}
	public partial class IfcPavementType
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			string str = xml.GetAttribute("PredefinedType");
			if (!string.IsNullOrEmpty(str))
				Enum.TryParse<IfcPavementTypeEnum>(str, out mPredefinedType);
		}
	}
	public partial class IfcPerson
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Identification"))
				Identification = xml.Attributes["Identification"].Value;
			if (xml.HasAttribute("FamilyName"))
				FamilyName = xml.Attributes["FamilyName"].Value;
			if (xml.HasAttribute("GivenName"))
				GivenName = xml.Attributes["GivenName"].Value;

			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "MiddleNames") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
						MiddleNames.Add(cn.InnerText);
				}
				else if (string.Compare(name, "Roles", true) == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
						Roles.Add(mDatabase.ParseXml<IfcActorRole>(cn as XmlElement));
				}
				else if (string.Compare(name, "Addresses", true) == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
						Addresses.Add(mDatabase.ParseXml<IfcAddress>(cn as XmlElement));
				}
				else if (string.Compare(name, "Identification", true) == 0)
					Identification = child.InnerText;
				else if (string.Compare(name, "FamilyName", true) == 0)
					FamilyName = child.InnerText;
				else if (string.Compare(name, "GivenName", true) == 0)
					GivenName = child.InnerText;
				else if (string.Compare(name, "Id", true) == 0)
					Identification = child.InnerText;
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			string str = Identification;
			if (!string.IsNullOrEmpty(str))
				xml.SetAttribute("Identification", str);
			str = FamilyName;
			if (!string.IsNullOrEmpty(str))
				xml.SetAttribute("FamilyName", str);
			str = GivenName;
			if (!string.IsNullOrEmpty(str))
				xml.SetAttribute("GivenName", str);
			if (mRoles.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("Roles", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (IfcActorRole role in Roles)
				{
					element.AppendChild(role.GetXML(xml.OwnerDocument, "", this, processed));
				}
			}
			if (mAddresses.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("Addresses", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (IfcAddress address in Addresses)
					element.AppendChild(address.GetXML(xml.OwnerDocument, "", this, processed));
			}
		}
	}
	public partial class IfcPersonAndOrganization
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ThePerson") == 0)
					ThePerson = mDatabase.ParseXml<IfcPerson>(child as XmlElement);
				else if (string.Compare(name, "TheOrganization") == 0)
					TheOrganization = mDatabase.ParseXml<IfcOrganization>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(ThePerson.GetXML(xml.OwnerDocument, "ThePerson", this, processed));
			xml.AppendChild(TheOrganization.GetXML(xml.OwnerDocument, "TheOrganization", this, processed));
		}
	}
	public partial class IfcPhysicalQuantity
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Name"))
				Name = xml.Attributes["Name"].Value;
			if (xml.HasAttribute("Description"))
				Description = xml.Attributes["Description"].Value;
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Name", Name);
			setAttribute(xml, "Description", Description);
		}
	}
	public partial class IfcPipeFittingType
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("PredefinedType"))
				Enum.TryParse<IfcPipeFittingTypeEnum>(xml.Attributes["PredefinedType"].Value, true, out mPredefinedType);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPredefinedType != IfcPipeFittingTypeEnum.NOTDEFINED)
				xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
	}
	public partial class IfcPlacement
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Location") == 0)
					mLocation = mDatabase.ParseXml<IfcPoint>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(mLocation.GetXML(xml.OwnerDocument, "Location", this, processed));
		}
	}
	public partial class IfcPlanarExtent
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("SizeInX"))
				mSizeInX = double.Parse(xml.Attributes["SizeInX"].Value);
			if (xml.HasAttribute("SizeInY"))
				mSizeInY = double.Parse(xml.Attributes["SizeInY"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("SizeInY", mSizeInY.ToString());
			xml.SetAttribute("SizeInY", mSizeInY.ToString());
		}
	}
	public partial class IfcPointByDistanceExpression
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "DistanceAlong") == 0)
				{
					if(child.HasChildNodes)
					{
						XmlNode distanceAlong = child.ChildNodes[0];
						string lowerName = distanceAlong.Name.ToLower();
							double d = 0;
						if (double.TryParse(distanceAlong.InnerText, out d))
						{
							if (lowerName.StartsWith("ifcnonnegativelengthmeasure"))
								DistanceAlong = new IfcNonNegativeLengthMeasure(d);
							else if (lowerName.StartsWith("ifcparametervalue"))
								DistanceAlong = new IfcParameterValue(d);
						}
					}
					BasisCurve = mDatabase.ParseXml<IfcCurve>(child as XmlElement);
				}
				else if (string.Compare(name, "BasisCurve") == 0)
					BasisCurve = mDatabase.ParseXml<IfcCurve>(child as XmlElement);
			}
			if (xml.HasAttribute("OffsetLateral"))
				mOffsetLateral = double.Parse(xml.Attributes["OffsetLateral"].Value);
			if (xml.HasAttribute("OffsetVertical"))
				mOffsetVertical = double.Parse(xml.Attributes["OffsetVertical"].Value);
			if (xml.HasAttribute("OffsetLongitudinal"))
				mOffsetLongitudinal = double.Parse(xml.Attributes["OffsetLongitudinal"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);

			if(mDistanceAlong != null)
			{
				xml.AppendChild(convert(xml.OwnerDocument, mDistanceAlong as IfcValue, "DistanceAlong", mDatabase.mXmlNamespace));
			}
			if(!double.IsNaN(mOffsetLateral))
				xml.SetAttribute("OffsetLateral", mOffsetLateral.ToString());
			if(!double.IsNaN(mOffsetVertical))
				xml.SetAttribute("OffsetVertical", mOffsetVertical.ToString());
			if(!double.IsNaN(mOffsetLongitudinal))
				xml.SetAttribute("OffsetLongitudinal", mOffsetLongitudinal.ToString());
			if(BasisCurve != null)
				xml.AppendChild(BasisCurve.GetXML(xml.OwnerDocument, "BasisCurve", this, processed));
		}
	}
	public partial class IfcPointOnCurve
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "BasisCurve") == 0)
					BasisCurve = mDatabase.ParseXml<IfcCurve>(child as XmlElement);
			}
			if (xml.HasAttribute("PointParameter"))
				mPointParameter = double.Parse(xml.Attributes["PointParameter"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(BasisCurve.GetXML(xml.OwnerDocument, "BasisCurve", this, processed));
			xml.SetAttribute("PointParameter", mPointParameter.ToString());
		}
	}
	public partial class IfcPointOnSurface
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "BasisSurface") == 0)
					BasisSurface = mDatabase.ParseXml<IfcSurface>(child as XmlElement);
			}
			if (xml.HasAttribute("PointParameterU"))
				mPointParameterU = double.Parse(xml.Attributes["PointParameterU"].Value);
			if (xml.HasAttribute("PointParameterV"))
				mPointParameterV = double.Parse(xml.Attributes["PointParameterV"].Value);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild(BasisSurface.GetXML(xml.OwnerDocument, "BasisSurface", this, processed));
			xml.SetAttribute("PointParameterU", mPointParameterU.ToString());
			xml.SetAttribute("PointParameterV", mPointParameterV.ToString());
		}
	}
	public partial class IfcPolyline
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Points") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcCartesianPoint p = mDatabase.ParseXml<IfcCartesianPoint>(cn as XmlElement);
						if (p != null)
							Points.Add(p);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			XmlElement element = xml.OwnerDocument.CreateElement("Points", mDatabase.mXmlNamespace);
			xml.AppendChild(element);
			foreach (IfcCartesianPoint p in Points)
				element.AppendChild(p.GetXML(xml.OwnerDocument, "", this, processed));
		}
	}
	public partial class IfcPolyLoop
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Polygon") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcCartesianPoint p = mDatabase.ParseXml<IfcCartesianPoint>(cn as XmlElement);
						if (p != null)
							mPolygon.Add(p);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			XmlElement element = xml.OwnerDocument.CreateElement("Polygon", mDatabase.mXmlNamespace);
			xml.AppendChild(element);
			foreach (IfcCartesianPoint p in Polygon)
				element.AppendChild(p.GetXML(xml.OwnerDocument, "", this, processed));
		}
	}
	public partial class IfcPolynomialCurve
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.AppendChild((Position as BaseClassIfc).GetXML(xml.OwnerDocument, "Position", this, processed));
			if(CoefficientsX.Count > 0)
				xml.SetAttribute("CoefficientsX", string.Join(" ", CoefficientsX.Select(x=> x.ToString())));
			if(CoefficientsY.Count > 0)
				xml.SetAttribute("CoefficientsY", string.Join(" ", CoefficientsY.Select(x => x.ToString())));
			if (CoefficientsZ.Count > 0)
				xml.SetAttribute("CoefficientsZ", string.Join(" ", CoefficientsZ.Select(x => x.ToString())));
		}
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			XmlAttribute att = xml.Attributes["CoefficientsX"];
			if (att != null)
				CoefficientsX.AddRange(att.InnerText.Split(" ".ToCharArray()).Select(x => double.Parse(x)));
			att = xml.Attributes["CoefficientsY"];
			if (att != null)
				CoefficientsY.AddRange(att.InnerText.Split(" ".ToCharArray()).Select(x => double.Parse(x)));
			att = xml.Attributes["CoefficientsZ"];
			if (att != null)
				CoefficientsZ.AddRange(att.InnerText.Split(" ".ToCharArray()).Select(x => double.Parse(x)));

			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Position", true) == 0)
					Position = mDatabase.ParseXml<IfcPlacement>(child as XmlElement);
			}
		}
	}
	public partial class IfcPostalAddress
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("InternalLocation"))
				InternalLocation = xml.Attributes["InternalLocation"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "AddressLines") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
						AddressLines.Add(cn.InnerText);
				}
				else if (string.Compare(name, "InternalLocation", true) == 0)
					InternalLocation = child.InnerText;
				else if (string.Compare(name, "PostalBox", true) == 0)
					PostalBox = child.InnerText;
				else if (string.Compare(name, "Town", true) == 0)
					Town = child.InnerText;
				else if (string.Compare(name, "Region", true) == 0)
					Region = child.InnerText;
				else if (string.Compare(name, "PostalCode", true) == 0)
					PostalCode = child.InnerText;
				else if (string.Compare(name, "Country", true) == 0)
					Country = child.InnerText;
			}
			if (xml.HasAttribute("PostalBox"))
				PostalBox = xml.Attributes["PostalBox"].Value;
			if (xml.HasAttribute("Town"))
				Town = xml.Attributes["Town"].Value;
			if (xml.HasAttribute("Region"))
				Region = xml.Attributes["Region"].Value;
			if (xml.HasAttribute("PostalCode"))
				PostalCode = xml.Attributes["PostalCode"].Value;
			if (xml.HasAttribute("Country"))
				Country = xml.Attributes["Country"].Value;
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "InternalLocation", InternalLocation);
			if (mAddressLines.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("AddressLines", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (string line in AddressLines)
				{
					XmlElement l = xml.OwnerDocument.CreateElement("IfcLabel-wrapper", mDatabase.mXmlNamespace);
					l.InnerText = line;
					element.AppendChild(l);
				}
			}
			setAttribute(xml, "PostalBox", PostalBox);
			setAttribute(xml, "Town", Town);
			setAttribute(xml, "Region", Region);
			setAttribute(xml, "PostalCode", PostalCode);
			setAttribute(xml, "Country", Country);
		}
	}
	public partial class IfcPresentationLayerAssignment
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
				if (string.Compare(name, "AssignedItems") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcLayeredItem i = mDatabase.ParseXml<IfcLayeredItem>(cn as XmlElement);
						if (i != null)
							AssignedItems.Add(i);
					}
				}
			}
			if (xml.HasAttribute("Identifier"))
				Identifier = xml.Attributes["Identifier"].Value;
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Name", Name);
            //XmlElement element = xml.OwnerDocument.CreateElement("AssignedItems", mDatabase.mXmlNamespace);
            //xml.AppendChild(element);
            //foreach (int item in mAssignedItems)
            //	element.AppendChild(mDatabase[item].GetXML(xml.OwnerDocument, "", this, processed));
            setAttribute(xml, "Description", Description);
			setAttribute(xml, "Identifier", Identifier);
		}
	}
	public partial class IfcPresentationLayerWithStyle
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("LayerOn"))
				Enum.TryParse<IfcLogicalEnum>(xml.Attributes["LayerOn"].Value, true, out mLayerOn);
			if (xml.HasAttribute("LayerFrozen"))
				Enum.TryParse<IfcLogicalEnum>(xml.Attributes["LayerFrozen"].Value, true, out mLayerFrozen);
			if (xml.HasAttribute("LayerBlocked"))
				Enum.TryParse<IfcLogicalEnum>(xml.Attributes["LayerBlocked"].Value, true, out mLayerBlocked);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "LayerStyles") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcPresentationStyle s = mDatabase.ParseXml<IfcPresentationStyle>(cn as XmlElement);
						if (s != null)
							LayerStyles.Add(s);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("LayerOn", mLayerOn.ToString().ToLower());
			xml.SetAttribute("LayerFrozen", mLayerFrozen.ToString().ToLower());
			xml.SetAttribute("LayerBlocked", mLayerBlocked.ToString().ToLower());
			if (mLayerStyles.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("LayerStyles", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (IfcPresentationStyle s in LayerStyles)
					element.AppendChild(s.GetXML(xml.OwnerDocument, "", this, processed));
			}
		}
	}
	public partial class IfcPresentationStyle 
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Name"))
				Name = xml.Attributes["Name"].Value;
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "Name", Name);
		}
	}
	public partial class IfcPresentationStyleAssignment
	{

		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Styles") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcPresentationStyleSelect s = mDatabase.ParseXml<IfcPresentationStyleSelect>(cn as XmlElement);
						if (s != null)
							Styles.Add(s);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			XmlElement element = xml.OwnerDocument.CreateElement("Styles", mDatabase.mXmlNamespace);
			xml.AppendChild(element);
			foreach (IfcPresentationStyleSelect item in mStyles)
				element.AppendChild((item as BaseClassIfc).GetXML(xml.OwnerDocument, "", this, processed));
		}
	}
	public partial class IfcProduct
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ObjectPlacement") == 0)
					ObjectPlacement = mDatabase.ParseXml<IfcObjectPlacement>(child as XmlElement);
				else if (string.Compare(name, "Representation") == 0)
					Representation = mDatabase.ParseXml<IfcProductDefinitionShape>(child as XmlElement);
				else if (string.Compare(name, "Placement") == 0)
					ObjectPlacement = mDatabase.ParseXml<IfcObjectPlacement>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			XmlElement placement = (mObjectPlacement != null ? ObjectPlacement.GetXML(xml.OwnerDocument, "ObjectPlacement", this, processed) : null);
			base.SetXML(xml, host, processed);
			if (placement != null)
				xml.AppendChild(placement);
			if (mRepresentation != null)
				xml.AppendChild(mRepresentation.GetXML(xml.OwnerDocument, "Representation", this, processed));

			XmlElement element = xml.OwnerDocument.CreateElement("ReferencedBy", mDatabase.mXmlNamespace);
			foreach (IfcRelAssignsToProduct rap in mReferencedBy)
				element.AppendChild(rap.GetXML(xml.OwnerDocument, "", this, processed));
			if(element.ChildNodes.Count > 0)	
				xml.AppendChild(element);
		}
	}
	public partial class IfcProductDefinitionShape
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ShapeOfProduct") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcProduct product = mDatabase.ParseXml<IfcProduct>(cn as XmlElement);
						if (product != null)
							mShapeOfProduct.Add(product);
					}
				}
				if (string.Compare(name, "HasShapeAspects") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcShapeAspect aspect = mDatabase.ParseXml<IfcShapeAspect>(cn as XmlElement);
						if (aspect != null)
							mHasShapeAspects.Add(aspect);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mHasShapeAspects.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("HasShapeAspects", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (IfcShapeAspect aspect in mHasShapeAspects)
					element.AppendChild(aspect.GetXML(xml.OwnerDocument, "", this, processed));
			}
		}
	}
	public partial class IfcProductRepresentation<Representation, RepresentationItem>
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
				if (string.Compare(name, "Representations") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						Representation r = mDatabase.ParseXml<Representation>(cn as XmlElement);
						if (r != null)
							Representations.Add(r);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "Name", Name);
			setAttribute(xml, "Description", Description);
			if (mRepresentations.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("Representations", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (Representation rep in Representations)
					element.AppendChild(rep.GetXML(xml.OwnerDocument, "", this, processed));
			}
		}
	}
	public partial class IfcProfileDef
	{  
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("ProfileType"))
				Enum.TryParse<IfcProfileTypeEnum>(xml.Attributes["ProfileType"].Value, true, out mProfileType);
			if (xml.HasAttribute("ProfileName"))
				ProfileName = xml.Attributes["ProfileName"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "ProfileType", true) == 0)
					Enum.TryParse<IfcProfileTypeEnum>(child.InnerText, true, out mProfileType);
				else if (string.Compare(name, "Depth", true) == 0)
					ProfileName = child.InnerText;	
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("ProfileType", mProfileType.ToString().ToLower());
			setAttribute(xml, "ProfileName", ProfileName);

			if (mDatabase.Release >= ReleaseVersion.IFC2x3)
			{
				XmlElement externalReferences = xml.OwnerDocument.CreateElement("HasExternalReference", mDatabase.mXmlNamespace);
				foreach (IfcExternalReferenceRelationship r in mHasExternalReference)
				{
					if (r == host)
						continue;
					XmlElement e = r.GetXML(xml.OwnerDocument, "", this, processed);
					if (e != null)
						externalReferences.AppendChild(e);
				}
				if (externalReferences.ChildNodes.Count > 0)
					xml.AppendChild(externalReferences);

				XmlElement hasProperties = xml.OwnerDocument.CreateElement("HasProperties", mDatabase.mXmlNamespace);
				foreach (IfcProfileProperties p in mHasProperties)
				{
					XmlElement e = p.GetXML(xml.OwnerDocument, "", this, processed);
					if (e != null)
						hasProperties.AppendChild(e);
				}
				if (hasProperties.ChildNodes.Count > 0)
					xml.AppendChild(hasProperties);
			}
		}
	}
	public partial class IfcProjectedCRS
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("VerticalDatum"))
				VerticalDatum = xml.Attributes["VerticalDatum"].Value;
			if (xml.HasAttribute("MapProjection"))
				MapProjection = xml.Attributes["MapProjection"].Value;
			if (xml.HasAttribute("MapZone"))
				MapZone = xml.Attributes["MapZone"].Value;

			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "MapUnit") == 0)
					MapUnit = mDatabase.ParseXml<IfcNamedUnit>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "VerticalDatum", VerticalDatum);
			setAttribute(xml, "MapProjection", MapProjection);
			setAttribute(xml, "MapZone", MapZone);
			if (mMapUnit != null)
				xml.AppendChild(MapUnit.GetXML(xml.OwnerDocument, "MapUnit", this, processed));
		}
	}
	public partial class IfcProperty
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("Name"))
				Name = xml.Attributes["Name"].Value;
			if (xml.HasAttribute("Specification"))
				Specification = xml.Attributes["Specification"].Value;
			else if (xml.HasAttribute("Description"))
				Specification = xml.Attributes["Description"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "Name", true) == 0)
					Name = child.InnerText;
				else if (string.Compare(name, "Specification", true) == 0)
					Specification = child.InnerText;
				else if (string.Compare(name, "Description",true) == 0)
					Specification = child.InnerText;
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			xml.SetAttribute("Name", Name);
			if(mDatabase != null && mDatabase.mRelease < ReleaseVersion.IFC4X3)
				setAttribute(xml, "Description", Specification);
			else
				setAttribute(xml, "Specification", Specification);
		}
	}
	public partial class IfcPropertyBoundedValue
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mUpperBoundValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mUpperBoundValue, "UpperBoundValue", mDatabase.mXmlNamespace));
			if (mLowerBoundValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mLowerBoundValue, "LowerBoundValue", mDatabase.mXmlNamespace));
			if (mUnit != null) 
				xml.AppendChild((mUnit as BaseClassIfc).GetXML(xml.OwnerDocument, "Unit", this, processed));
			if (mSetPointValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mSetPointValue, "SetPointValue", mDatabase.mXmlNamespace));
		}
	}
	public partial class IfcPropertyBoundedValue<T>
	{
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mUpperBoundValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mUpperBoundValue, "UpperBoundValue", mDatabase.mXmlNamespace));
			if (mLowerBoundValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mLowerBoundValue, "LowerBoundValue", mDatabase.mXmlNamespace));
			if(mUnit != null)
				xml.AppendChild((mUnit as BaseClassIfc).GetXML(xml.OwnerDocument, "Unit", this, processed));
			if (mSetPointValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mSetPointValue, "SetPointValue", mDatabase.mXmlNamespace));
		}
	}
	public partial class IfcPropertyDefinition
	{ 
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "HasAssociations") == 0)
				{
					foreach (XmlNode node in child.ChildNodes)
					{
						IfcRelAssociates ra = mDatabase.ParseXml<IfcRelAssociates>(node as XmlElement);
                        if (ra != null)
							HasAssociations.Add(ra);
					}
				}
			}
		}
        //internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
        //{
        //	base.SetXML(xml, host, processed);
        //	if (mHasAssociations.Count > 0)
        //	{
        //		XmlElement element = xml.OwnerDocument.CreateElement("HasAssociations", mDatabase.mXmlNamespace);
        //		foreach (IfcRelAssociates ra in HasAssociations)
        //		{
        //			if(ra.mRelatedObjects.Count > 0)	
        //				element.AppendChild(ra.GetXML(xml.OwnerDocument, "", this, processed));
        //		}
        //		if(element.HasChildNodes)
        //			xml.AppendChild(element);
        //	}
        //}

    }
    public partial class IfcPropertyAbstraction
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "HasExternalReference") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcExternalReferenceRelationship r = mDatabase.ParseXml<IfcExternalReferenceRelationship>(cn as XmlElement);
						if (r != null)
							r.RelatedResourceObjects.Add(this);
					}
				}
				else if (string.Compare(name, "HasConstraintRelationships") == 0)
				{
					foreach (XmlNode cn in child.ChildNodes)
					{
						IfcResourceConstraintRelationship r = mDatabase.ParseXml<IfcResourceConstraintRelationship>(cn as XmlElement);
						if (r != null)
							r.RelatedResourceObjects.Add(this);
					}
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mHasExternalReference.Count > 0)
			{
				XmlElement element = xml.OwnerDocument.CreateElement("HasExternalReference", mDatabase.mXmlNamespace);
				xml.AppendChild(element);
				foreach (IfcExternalReferenceRelationship r in HasExternalReference)
					element.AppendChild(r.GetXML(xml.OwnerDocument, "", this, processed));
			}
            //if (mHasConstraintRelationships.Count > 0)
            //{
            //	XmlElement element = xml.OwnerDocument.CreateElement("HasConstraintRelationships", mDatabase.mXmlNamespace);
            //	xml.AppendChild(element);
            //	foreach (IfcResourceConstraintRelationship r in HasConstraintRelationships)
            //		element.AppendChild(r.GetXML(xml.OwnerDocument, "", this, processed));
            //}
        }
    }
	public partial class IfcPropertyEnumeratedValue
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "EnumerationValues") == 0)
				{
					foreach (XmlNode c in child.ChildNodes)
					{
						IfcValue val = extractValue(c);
						if (val != null)
							mEnumerationValues.Add(val);
					}
				}
				else if (string.Compare(name, "EnumerationReference") == 0)
					EnumerationReference = mDatabase.ParseXml<IfcPropertyEnumeration>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			XmlElement element = xml.OwnerDocument.CreateElement("EnumerationValues", mDatabase.mXmlNamespace);
			foreach (IfcValue value in mEnumerationValues)
				element.AppendChild(convert(xml.OwnerDocument, value, "", mDatabase.mXmlNamespace));
			if (mEnumerationReference != null)
				element.AppendChild(EnumerationReference.GetXML(xml.OwnerDocument, "EnumerationReference", this, processed));
		}
	}
	public partial class IfcPropertyEnumeration
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			Name = extractString(xml, "Name");
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if(string.Compare(name, "IfcLabel-wrapper", true) == 0)
					mEnumerationValues.Add(new IfcLabel(child.InnerText));
				else if (string.Compare(name, "EnumerationValues", true) == 0)
				{
					foreach (XmlNode node in child.ChildNodes)
					{
						IfcValue val = extractValue(node);
						if (val != null)
							mEnumerationValues.Add(val);
					}
				}
				else if (string.Compare(name, "Unit") == 0)
					Unit = mDatabase.ParseXml<IfcUnit>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setAttribute(xml, "Name", Name);
			XmlElement element = xml.OwnerDocument.CreateElement("EnumerationValues", mDatabase.mXmlNamespace);
			xml.AppendChild(element);
			foreach (IfcValue val in mEnumerationValues)
				element.AppendChild(convert(xml.OwnerDocument, val, "", mDatabase.mXmlNamespace));
			if (mUnit != null)
				xml.AppendChild(((BaseClassIfc) mUnit).GetXML(xml.OwnerDocument, "Unit", this, processed));
		}
	}
	public partial class IfcPropertySet
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "HasProperties") == 0)
					mDatabase.ParseXMLList<IfcProperty>(child).ForEach(x=>addProperty(x));
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setChild(xml, "HasProperties", mHasProperties.Values, processed);	
		}
	}
	public partial class IfcPropertySetDefinition
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "DefinesType") == 0)
				{
					foreach (IfcTypeObject t in mDatabase.ParseXMLList<IfcTypeObject>(child))
						t.HasPropertySets.Add(this);	
				}
				else if (string.Compare(name, "IsDefinedBy") == 0)
				{
					foreach (IfcRelDefinesByTemplate r in mDatabase.ParseXMLList<IfcRelDefinesByTemplate>(child))
						r.RelatedPropertySets.Add(this);
				}
				else if (string.Compare(name, "DefinesOccurrence") == 0)
				{
					foreach (IfcRelDefinesByProperties r in mDatabase.ParseXMLList<IfcRelDefinesByProperties>(child))
						r.RelatingPropertyDefinition.Add(this);
				}
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			setChild(xml, "IsDefinedBy", IsDefinedBy, processed);
		}
	}
	public partial class IfcPropertySetTemplate
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("TemplateType"))
				Enum.TryParse<IfcPropertySetTemplateTypeEnum>(xml.Attributes["TemplateType"].Value,true, out mTemplateType);
			if (xml.HasAttribute("ApplicableEntity"))
				ApplicableEntity = xml.Attributes["ApplicableEntity"].Value;
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "HasPropertyTemplates") == 0)
					mDatabase.ParseXMLList<IfcPropertyTemplate>(child).ForEach(x=>AddPropertyTemplate(x));
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mTemplateType != IfcPropertySetTemplateTypeEnum.NOTDEFINED)
				xml.SetAttribute("TemplateType", mTemplateType.ToString().ToLower());
			setAttribute(xml,"ApplicableEntity", ApplicableEntity);
			setChild(xml, "HasPropertyTemplates", HasPropertyTemplates.Values, processed);
		}
	}
	public partial class IfcPropertySingleValue
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			foreach (XmlNode child in xml.ChildNodes)
			{
				string name = child.Name;
				if (string.Compare(name, "NominalValue") == 0)
					mNominalValue = extractValue(child.FirstChild);
				else if (string.Compare(name, "Unit") == 0)
					Unit = mDatabase.ParseXml<IfcUnit>(child as XmlElement);
			}
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mNominalValue != null)
				xml.AppendChild(convert(xml.OwnerDocument, mNominalValue, "NominalValue", mDatabase.mXmlNamespace));
			if (mUnit != null)
				xml.AppendChild((mUnit as BaseClassIfc).GetXML(xml.OwnerDocument, "Unit", this, processed));
            //if(mAppliedValueFor.Count > 0)
            //{
            //	XmlElement element = xml.OwnerDocument.CreateElement("AppliedValueFor", mDatabase.mXmlNamespace);
            //	xml.AppendChild(element);
            //	foreach (IfcAppliedValue v in mAppliedValueFor)
            //		element.AppendChild(v.GetXML(xml.OwnerDocument, "", this, processed));
            //}
        }
    }
	public partial class IfcPumpType
	{
		internal override void ParseXml(XmlElement xml)
		{
			base.ParseXml(xml);
			if (xml.HasAttribute("PredefinedType"))
				Enum.TryParse<IfcPumpTypeEnum>(xml.Attributes["PredefinedType"].Value, true, out mPredefinedType);
		}
		internal override void SetXML(XmlElement xml, BaseClassIfc host, Dictionary<string, XmlElement> processed)
		{
			base.SetXML(xml, host, processed);
			if (mPredefinedType != IfcPumpTypeEnum.NOTDEFINED)
				xml.SetAttribute("PredefinedType", mPredefinedType.ToString().ToLower());
		}
	}
}
