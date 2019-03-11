namespace iCreator.Utils
{
    internal interface IMathHelper
    {
        float GLNormalize(float value, float minValue, float maxValue);
        float Lerp(float start, float end, float value);
    }

    internal sealed class MathHelper : IMathHelper
    {
        public float GLNormalize(float value, float minValue, float maxValue)
        {
            float x = (2f * ((value - minValue) / (maxValue - minValue))) - 1f;

            return x;
        }

        public float Lerp(float start, float end, float value)
        {
            return (start + value * (end - start));
        }
    }
}
