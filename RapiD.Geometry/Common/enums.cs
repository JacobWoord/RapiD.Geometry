using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry
{

    public enum ChainSide
    {
        Left, 
        Right,
        middle
    
    };


    public enum MirrorAxis { X, Y, Z }

    public enum MenuType
    {
        Door,
        Chain,
        Sphere,

    }
    public enum ParticleType
    {
        Bubbles,
        Dust,
        Smoke,
        Particle
    }
    public enum ViewMode
    {
        PrintPreview,
        DrawingView,
    }

    public enum MouseButton
    {
        Left,
        Middle,
        Right
    }

    public enum Mode
    {
        _2D,
        _3D,
        _2D3D
    }
    public enum Side
    {
        [Description("Bakboord")]
        PortSide,
        [Description("Stuurboord")]
        StarBoard
    }
    public enum NetGeometryType
    {
        LineGeometry,
        CylinderGeometry
    }


    public enum FishMethod
    {
        Boomkor,
        Flyshoot,
        Puls,
        Quadrig,
        Twinrig,
    }
    public enum LibraryType
    {
        Chains,
        Materials,
        Ropes,
        Articles,
        Nettypes,
        Clients,
        Users,
        PanelNames
    }
    public enum InsertUser
    {
        UsernameAlreadyExists,
        PasswordToShort,
        PasswordNoMatch,
        NoUperCases,
        UnKnown,
        Succes
    }
    public enum ConstraintsMode
    {
        Full,
        NetOnly
    }
    public enum ElementType
    {
        Straight,
        Parabolic
    }

    public enum BillBoardMode
    {
        Text,
        Dimension
    }
    public enum SnitPosition
    {
        BeneathArticle,
        AtPanel
    }
    public enum PrintType
    {
        CutDrawings,
        TransportDrawing,
        ProductionDrawing
    }
    public enum CameraMode
    {
        Orthographic,
        Perspective
    };

    public enum ChainShape
    {
        [Description("Recht")]
        Straight,
        [Description("Parabool")]
        Parabolic
    };
}
