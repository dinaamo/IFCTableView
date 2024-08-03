using GeometryGym.STEP;
using System.Collections.Generic;
using System.Drawing;

namespace GeometryGym.Ifc
{
    public abstract partial class IfcPreDefinedColour
    {
        public Color Color()
        { return System.Drawing.Color.Empty; }
    }

    public partial class IfcPresentationLayerAssignment
    {
        internal virtual Color LayerColour
        { get { return Color.Empty; } }
    }

    public partial class IfcPresentationLayerWithStyle
    {
        internal override Color LayerColour
        {
            get
            {
                SET<IfcPresentationStyle> styles = LayerStyles;
                foreach (IfcPresentationStyle ps in styles)
                {
                    IfcSurfaceStyle ss = ps as IfcSurfaceStyle;
                    if (ss != null)
                    {
                        List<IfcSurfaceStyleShading> sss = ss.Extract<IfcSurfaceStyleShading>();
                        if (sss.Count > 0)
                            return sss[0].SurfaceColour.Color();
                    }
                }
                foreach (IfcPresentationStyle ps in styles)
                {
                    IfcCurveStyle cs = ps as IfcCurveStyle;
                    if (cs != null)
                    {
                        IfcColour col = cs.CurveColour;
                        if (col != null)
                            return col.Color();
                    }
                }
                return base.LayerColour;
            }
        }
    }
}