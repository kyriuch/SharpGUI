
namespace iCreator.Utils
{
    internal interface IVectorHelper
    {
        System.Numerics.Vector2 ToSystemVector2(OpenTK.Vector2 vector2);
        System.Numerics.Vector2 ToSystemVector2(Models.Vector2 vector2);
        OpenTK.Vector2 ToTKVector2(Models.Vector2 vector2);
        OpenTK.Vector2 ToTKVector2(System.Numerics.Vector2 vector2);
    }

    internal sealed class VectorHelper : IVectorHelper
    {
        public System.Numerics.Vector2 ToSystemVector2(OpenTK.Vector2 vector2) => new System.Numerics.Vector2(vector2.X, vector2.Y);
        public System.Numerics.Vector2 ToSystemVector2(Models.Vector2 vector2) => new System.Numerics.Vector2(vector2.X, vector2.Y);
        public OpenTK.Vector2 ToTKVector2(Models.Vector2 vector2) => new OpenTK.Vector2(vector2.X, vector2.Y);
        public OpenTK.Vector2 ToTKVector2(System.Numerics.Vector2 vector2) => new OpenTK.Vector2(vector2.X, vector2.Y);
    }
}
