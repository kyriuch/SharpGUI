namespace iCreator.Utils
{
    internal interface IMathHelper
    {
        float GLNormalize(float value, float minValue, float maxValue);
    }

    internal sealed class MathHelper : IMathHelper
    {
        public float GLNormalize(float value, float minValue, float maxValue)
        {
            float x = (2f * ((value - minValue) / (maxValue - minValue))) - 1f;

            return x;
        }
    }
}
